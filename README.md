# Assembly Information Viewer

---

.Net Application to assist in finding discrepancies in assembly versions. 
Written in C# & .Net 4.6.2, using Visual Studio 2015. 
[Licensed](https://github.com/akoimeexx/AssemblyInformationViewer/blob/master/LICENSE.BSD3) under 
[BSD-3](https://opensource.org/licenses/BSD-3-Clause).

### Unit Tests
Unit tests were written for the earlist version of Assembly Information Viewer 
(at the time, called "VersionChecker"), but were all wiped when the the entire 
project had to be refactored. Unit tests will be re-implemented, but at this 
time this software should be considered to be in very early alpha stages.

### Non-.Net Binaries
Initial testing shows this does not work with binaries compiled outside of a 
.Net environment. Currently, there are no plans to change this.

### Screenshots

##### Main Screen (Turn On)
![Startup.Window.Xaml](https://raw.githubusercontent.com/akoimeexx/AssemblyInformationViewer/master/Screenshots/MainScreenTurnOn.png)

##### Assembly Details
![Details.Window.Xaml](https://raw.githubusercontent.com/akoimeexx/AssemblyInformationViewer/master/Screenshots/DetailsView.png)

### Nifty Bits

* **_[ReflectionAnalyzer.Class.cs](https://github.com/akoimeexx/AssemblyInformationViewer/blob/master/AssemblyInfo/ReflectionAnalyzer.Class.cs)_:**  
  provides access to information inside .Net 
libraries via &mdash;you guessed it&mdash; Reflection. It does this in a 
separate `AppDomain`, which allows the program to view information on multiple 
versions of the same assembly. If this is something you've had issues with 
(late-binding the same assembly twice will highlight this experience), I 
recommend checking that file out.  
  
* Lots of wpf binding goodness  
