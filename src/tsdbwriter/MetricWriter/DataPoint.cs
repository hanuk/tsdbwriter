/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetricWriter
{
    public class DataPoint
    {
        private static readonly string DELIMITER = " ";
        public DataPoint()
        {
            Tags = new Dictionary<string, string>(); 
        }

        public string Name { get; set; }
        public DateTime Timestamp { get; set; }

        public Dictionary<string, string>  Tags { get; set; }

        public double Value { get; set; }
        public static long ToUnixTime(DateTime dateTime)
        {
            long unixTime = (dateTime.Ticks  - new DateTime(1970,1,1).Ticks)/TimeSpan.TicksPerSecond;

            return unixTime;
        }
        
        public static DateTime FromUnixTime(long unixTime)
        {
            long unixTicks = unixTime * TimeSpan.TicksPerSecond;
            return new DateTime(unixTicks + new DateTime(1970, 1, 1).Ticks);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}{1}", Name, DELIMITER);
            sb.AppendFormat("{0}{1}", ToUnixTime(Timestamp), DELIMITER);
            sb.Append(Value.ToString("F3")); //only three digits after the decimal point  
            if (Tags.Count > 0)
            {
                sb.Append(DELIMITER);
            }
            foreach( var tagName in Tags.Keys)
            {
                sb.AppendFormat("{0}={1}{2}", tagName, Tags[tagName], DELIMITER);
            }
            return sb.ToString().Trim();
        }
    }
}
