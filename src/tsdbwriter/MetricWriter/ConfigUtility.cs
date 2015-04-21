/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricWriter
{
    public class ConfigUtility
    {
        /// <summary>
        /// reads json file and returns it as string
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <returns></returns>
        public static string ReadFile(string fileName)
        {
            string codebase = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            string workingDir = new Uri(Path.GetDirectoryName(codebase)).LocalPath;
            return File.ReadAllText(workingDir + @"/" + fileName);
        }

        public static void WriteFile(string fileName, string text)
        {
            string codebase = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            string workingDir = new Uri(Path.GetDirectoryName(codebase)).LocalPath;
            File.WriteAllText(workingDir + @"/" + fileName, text);
        }

        public static T ReadConfig<T>(string fileName)
        {
            string config = ReadFile(fileName);
            return JsonConvert.DeserializeObject<T>(config);
        }
        public static void SaveConfig<T>(string fileName, T config)
        {
            WriteFile(fileName, JsonConvert.SerializeObject(config));
        }
    }
}
