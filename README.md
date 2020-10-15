# HWMonitor Service
## A Windows Service for hardware monitoring

This is a simple API implemented in C# that runs in a Windows Service and provides real-time hardware monitoring data in JSON formatted output through a http server (running in TCP port 8000).
This project was developed to attend to the final project in the [CS50x](https://www.edx.org/course/cs50s-introduction-to-computer-science) course at [edx.org](https://www.edx.org/).

## Features:
* Kept simple to be as lighweight as possible.
* Since its output is JSON formatted, it can be easily consumed by any modern framework like React, Angular, Vue or any language of preference.
* Runs automatically in a Windows Service, so it runs background since bootup
* internal http server
* uses openhardwarelib.dll from [openhardwaremonitor project](https://openhardwaremonitor.org/)

Cons:
* Since extracting hardware data requires privileged system access, it needs an administrative account on the local machine that will run the service.

Main sources used on development:
* [openhardwaremonitor](https://openhardwaremonitor.org/)
* [Tutorial using openhardware library](https://www.lattepanda.com/topic-f11t3004.html)
* [Microsoft documentation - Develop Windows service apps](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/)
* [Microsoft documentation - HttpListener Class](https://docs.microsoft.com/en-us/dotnet/api/system.net.httplistener?view=netcore-3.1)

## Usage
Build it on Visual Studio (tested on Visual Studio Community edition 2017 and 2019)
Install it as a service with the "Developer Command Line tool" using the InstallUtil command:
For example
```
C:\> CD "C:\Users\XYZ\source\repos\hwmonitor\hwmonitor-service\bin\release"
C:\Users\XYZ\source\repos\hwmonitor\hwmonitor-service\bin\release> InstallUtil hwmonitor-service.exe
```
(Optional) Check if the service was installed properly using Services in the control Panel.
Start manually the service the 1st time (using Services in the control panel) or restart the computer, that the service will start automatically.
Browse into [http://localhost:8000/](http://localhost:8000/) to see the data.
(Optional) If you are using this service from another location (internet/intranet), the program itself doesn't add an exception in your firewall, so don't forget to add it.

## TODO Next
This project will be delivered as an API but next features that will be added could be:
* A front end in some SPA architecture served on its own internal http server
* Some entries in the Windows System Event Log


## Developed between diaper changes and preparing milk bottles for my 3 children!

# This was CS50x!
