/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

namespace MetricWriter
{
    class Program
    {
        private static int _consoleLeft = 5;
        private static int _consoleTop = 5;
        private static int _tagCount = 0;
        private static Stopwatch _stopwatch = new Stopwatch();
        static void Main(string[] args)
        {
            string fileName = ConfigurationManager.AppSettings["ConfigFileName"];
            if (fileName == null)
            {
                Console.WriteLine("Configuration file is missing; configure \"ConfigFileName\" in app.config");
                return; 
            }
            string config = ConfigUtility.ReadFile(fileName);
            DataPointConfig dpc = JsonConvert.DeserializeObject<DataPointConfig>(config);

            dpc.SetDefaults();
            List<string> errors = null; 
            if (!dpc.IsValid(ref errors))
            {
                foreach (string error in errors)
                {
                    Console.WriteLine(error);
                }
                return; 
            }

            DataPoint dataPoint = new DataPoint();
 
            //compose tags
            string tagStr = ConfigurationManager.AppSettings["Tags"];

            dataPoint.Name = dpc.MetricName;
            dataPoint.Timestamp = dpc.StartDate;
            dataPoint.Value = GetRandomMetric(dpc.MetricLow, dpc.MetricHigh);
            dataPoint.Tags = dpc.Tags;
            _stopwatch.Start();
            WriteRange(dpc, dataPoint);
            _stopwatch.Stop(); 
            WriteSummary(dpc);
            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }

        private static void WriteSummary(DataPointConfig dpc)
        {
            Console.CursorLeft = 5;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Number of data points created: {0}", _tagCount);
            Console.CursorLeft = 5;
            Console.WriteLine("Metric Name: {0}", dpc.MetricName);
            Console.CursorLeft = 5;
            Console.WriteLine("Start Date: {0}", dpc.StartDate);
            Console.CursorLeft = 5;
            Console.WriteLine("Tags : {0}", dpc.GetTagString());
            Console.CursorLeft = 5;
            Console.WriteLine("Elapsed time: {0} msec", _stopwatch.ElapsedMilliseconds);
            Console.CursorLeft = 5;
        }

        private static void WriteRange(DataPointConfig dataPointConfig, DataPoint dataPoint)
        {
            DateTime start = dataPointConfig.StartDate; 
            _tagCount = 0;
            while (start <= dataPointConfig.EndDate)
            {
                string msg = "put " + dataPoint.ToString();
                TsdbClient.Write(msg, dataPointConfig.TsdbHostName, dataPointConfig.TsdbPortNumber, true);
                start = start.AddMinutes(dataPointConfig.SamplingIntervalMinutes);
                dataPoint.Timestamp = start;
                dataPoint.Value = GetRandomMetric(dataPointConfig.MetricLow, dataPointConfig.MetricHigh);
                Console.SetCursorPosition(_consoleLeft, _consoleTop);
                Console.Write("Created data point # {0}", ++_tagCount);
            }
        }

        private static double GetRandomMetric(int low, int high)
        {
            Random rnd = new Random();
            double n = (double)rnd.Next(low, high);
            n = n + rnd.NextDouble();
            return n; 
        }
    }
}
