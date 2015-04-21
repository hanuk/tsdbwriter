# tsdbwriter
This is a simple tool to write test data points to OpenTSDB from a C#  client. It only supports analog metrics for now.

Execute MetricWriter.exe after changing Config.json.

The following is the sample Config.json file: 

    
	{
	    "TsdbHostName": "localhost", //the DNS name of the TSDB HOST
	    "TsdbPortNumber": "4242", //TSDB port number; the default is 4242;
	    "MetricName": "LivingRoom.Temperature", //any meaningful name
	    "StartDate": "01/01/2014", //mandatory
	    "EndDate": "04/21/2015", //if not specificed current DateTime.Now will be used
	    "SamplingIntervalMinutes": "5", //minutes
	    "MetricLow": "65", //low end of the metric
	    "MetricHigh": "85", //high end of the metric
	    "Tags": { "host": "gateway", "app": "test" }//specify atleast one tag as it is required by TSDB
	} 

