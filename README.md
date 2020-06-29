# sample-client
Cross-platform C# client for SAMPLE project.

## General info
This is a client for gathering and visualizing data from the automatic greenhouse. It enables plotting data, making photos of the greenhouse and sending commands to its module. 
For more info on the project, check out [sample-main repository](https://github.com/Dinuirar/sample-main). 

## Screenshots
Starting screen of the app:
![Example screenshot](https://snipboard.io/akNvn7.jpg)

App window with plots:          
![SAMPLE client window with plots](https://snipboard.io/5HTz3f.jpg)

## Setup
#### If you want to use the app with [SAMPLE server module](https://github.com/Dinuirar/sample-server)
* download the binary from [Releases](https://github.com/sachiwhite/sample-client/releases) section.

#### If you want to build the app and test it without server module.
* clone the repository
* run command line in directory, where you cloned SAMPLE client to and run `dotnet build` command
##### Mocking is enabled by default. If you'd like to disable it, move to `App.xaml.cs` file, and remove/comment line `#define MOCKING`, then recompile the app.

## Features
* It enables downloading telemetry from the server at specified time intervals.





## Technologies
* C#
* .NET Core 3.0
* [Avalonia UI](https://github.com/AvaloniaUI/Avalonia) 0.9

## Contact
Created by Blanka Lewonowska, feel free to contact me

## Attribution:
* plot and picture icon made by [Kiranshastry](https://www.flaticon.com/authors/kiranshastry) from [flaticon](http://www.flaticon.com)
