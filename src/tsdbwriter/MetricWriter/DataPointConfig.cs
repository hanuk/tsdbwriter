/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricWriter
{
    public class DataPointConfig
    {
        private static readonly string  DELIMITER = " ";
        public DataPointConfig()
        {
            this.Tags = new Dictionary<string, string>();
        }
        public string TsdbHostName { get; set; }
        public int TsdbPortNumber { get; set; }
        public string MetricName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SamplingIntervalMinutes { get; set;}
        public int MetricLow { get; set; }
        public int MetricHigh { get; set; }
        public Dictionary<string, string> Tags { get; set; }
        public string GetTagString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var tagName in Tags.Keys)
            {
                sb.AppendFormat("{0}={1}{2}", tagName, Tags[tagName], DELIMITER);
            }
            return sb.ToString();
        }
        public void SetDefaults()
        {
            if (TsdbPortNumber == 0)
            {
                TsdbPortNumber = 4242;
            }

            if (StartDate == null)
            {
                StartDate = EndDate.Subtract(new TimeSpan(120, 0, 0, 0));
            }

            if (EndDate == null)
            {
                EndDate = DateTime.Now;
            }

            if (SamplingIntervalMinutes == 0)
            {
                SamplingIntervalMinutes = 5; 
            }
        }
        public bool IsValid(ref List<string> errors)
        {
            if (errors == null)
            {
                errors = new List<string>();
            }
            if (MetricName == null)
            {
                errors.Add(FormatError("MetricName")); 
            }
            if (TsdbHostName == null)
            {
                errors.Add(FormatError("TsdbHostName"));
            }
            if (StartDate == null)
            {
                errors.Add(FormatError("StartDate"));
            }
            if (EndDate == null)
            {
                errors.Add(FormatError("EndDate"));
            }
            if (TsdbPortNumber == 0)
            {
                errors.Add(FormatError("TsdbPortNumber"));
            }
            if (MetricLow == 0)
            {
                errors.Add(FormatError("MetricLow"));
            }
            if (MetricHigh == 0)
            {
                errors.Add(FormatError("MetricHigh"));
            }
            if (Tags.Count == 0)
            {
                errors.Add(FormatError("Tags"));
            }
            if (errors.Count == 0)
            {
                return true;
            }
            else
            {
                return false; 
            }
        }

        private string FormatError(string token)
        {
            return string.Format("Missing {0}; set it in config file", token);
        }
    }
}
