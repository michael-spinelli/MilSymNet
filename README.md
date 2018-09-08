# MilSymNet
Prototype 2525C renderer for the Windows 10 Universal Platform and .NET Framework taking advantage of .NET Standard 2.0.
MilSymUwp and MilSymNet resepectively.

This is something I have done on my own time, on my own hardware and does not fall under the work done on the [Mission Command](https://github.com/missioncommand) GitHub page with the Java, JavaScript and Android MilStd 2525 Renderer.  However, it is based on that work.  I wanted to explore the Windows 10 Universal App platform and also play around with Microsoft's [Win2D](https://github.com/Microsoft/Win2D) project.  I also wanted to give .NET Standard a try with sharing code between MilSymNet and MilSymUwp.  The renderers on the Mission Command page use the Apache 2.0 license.  Win2D uses the MIT license.  I'm only targeting single point symbology in this project.  I will not be generating kml or GeoJSON or anything else for multipoints.

Currently, for UWP, it renders all single points in 2525C including those among the Tactical Graphics and METOC symbols.  I will gradually work on implementing modifiers.
For .NET it is close to complete.  I have very little time to work on this so progress will be slow.
