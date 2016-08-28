ServerTrack 
========

A simple real-time server load tracking service.


Build
========

Created with Visual Studio 2015 Update 3.

This is an Asp.Net Web Api project. It should run on any Windows server capable of running such a service.

Detailed instructions, assuming you have cloned the Git repository and have Visual Studio installed:

1. In Visual Studio, File->Open->Project/Solution. Select the .sln file in the root of the reporting
2. In Visual Studio, Debug->StartWithoutDebugging

This will open a browser with the local URL of the service, but there is no home page as this is a service. 

Usage
========

Use a tool like curl, PostMan, Advanced Rest Client, or Invoke-RestMethod to interact with the service. The examples below use Invoke-RestMethod

Each server should POST its current CPU and RAM load data to /api/load/{serverName}, for example:

	PS> Invoke-RestMethod -Method Post http://localhost:54251/api/load/server1 -Body @{ CpuLoad = 0.1; RamLoad = 0.2}

A reporting client can get the latest summary for load data for a server with a GET to the same URI, for example:

	PS> Invoke-RestMethod -Method Get http://localhost:54251/api/load/server1  | Format-List


	last60Minutes : {@{TimeBin=2016-08-27T23:12:00-07:00; AverageCpuLoad=0.1; AverageRamLoad=0.2},
					@{TimeBin=2016-08-27T23:13:00-07:00; AverageCpuLoad=0.1; AverageRamLoad=0.3}}
	last24Hours   : {@{TimeBin=2016-08-27T23:00:00-07:00; AverageCpuLoad=0.1; AverageRamLoad=0.25}}
