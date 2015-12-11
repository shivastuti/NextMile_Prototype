/*      Copyright (c) 2015
 *      by NextMile Team(Jason Campoli, Shivastuti Kohl, Smitha Bangalore Naresh), 
 *      College of Computer and Information Science, Northeastern University, Boston MA
 *
 *      This software is the intellectual property of NextMile Team
 */

NextMile Boston Food Truck Search Web Application

Authored by Fall 2015 CS5500 Team NextMile:
	Smitha Bangalore Naresh
	Shivastuti Kohl
	Jason Campoli

This ReadMe file contains the following information for this VS2013 Solution:

1. Solution's key structure and files
2. Operating Instructions
3. Management Instructions
4. Unit Testing Instructions


##########################################################################################################
Part 1: Solution's key structure and files
----------------------------------------------------------------------------------------------------------
NextMile02.sln			Solution file used to open the NextMile02 and NextMile02.Tests projects
NextMile02\ 		
  NextMile02.csproj		Project containing the NextMile Web Application code
  Global.asax.cs		Contains some global statics including the locations lookup table and init
  Controller\
    AccountController.cs	Controller for Facebook Login
    HomeController.cs		Controller for interface with Truck events data and User Preferences
    TruckDataHelper.cs		Helper class that performs web scraping for food truck data
  Models\
    Facebook*.cs		Models relating to Facebook login
    *TruckDataRepository.cs	Models for interfacing with food truck events data
    *PreferenceRepository.cs	Models for interfacing with user preference data
    *CurrentUser.cs		Models abstracting a current user for HomeController
    TruckPushpinInfo.cs		Model representing truck event pushpins to be rendered in BingMaps.
  Scripts\
    javascript\
      mapSearch.js		Performs all of heavy lifting for the Bing Maps functionality
      pin_clusterer.js		Bing Maps pin clustering helper lib
      Facebook.js		Functionality for Facebook login
      UserDetails.js		Functionality for UserDetails pane
  Views\
    Home\			Home Index views including Welcome, Index, and About
    Account\			Views for the UserDetails pane
    Shared\			Layout and Errors views
    
NextMile02.Tests\
  NextMile02.Tests.csproj	Project containing the NextMile Web Application unit tests
  
NextMile_FinalInstaller		Installation package for the application

##############################################################################
Part 2: Operating Instructions
------------------------------------------------------------------------------


##############################################################################
Part 3: Management Instructions
------------------------------------------------------------------------------
General Application Installation(using an executable file) - 
- Nextmile can be run locally on any machine that supports Visual Studio 4.5.
- Run the setup.exe file at the below location in the package to install the application on your machine
NextMile_Prototype\NextMile_FinalInstaller\NextMile_FinalInstaller\Express\DVD-5\DiskImages\DISK1
------------------------------------------------------------------------------
Common issues encountered 

1. Install Shield may be missing - Please download it from http://installshield-professional.en.softonic.com/
2. Some machines may encounter a failure in deployment of the application.This may happen due to the 
following reasons - 
	- WMSvc Service (Web Management Service) is not started - Please make sure that it is
	  set to 'Auto' (and started) so it's on when you restart 
	- IIS7 is not installed - To install IIS to serve static content
		a. Click the Start button , click Control Panel, click Programs, and then click Turn Windows features on or off. ...
		b. In the list of Windows features, select Internet Information Services, and then click OK.

From a developer's standpoint, while deploying this application locally - 
1. Right click on the solution file in the Solution explorer.
2. Select Clean from the drop down menu.
3. After cleaning the project,again right click on the solution file and click on build
4. This triggers a local build of the applicaiton on the IIS server (Visual Studio).
 - The results/output of a build can be viewed in the output view (Got to View -> Output )
 - After a successful build the applicaiton is deployed on the localhost of the default browser that you set in Visual studio


##############################################################################
Part 4: Unit Testing Instructions
------------------------------------------------------------------------------
There are two forms of automated unit testing in the NextMile solution
framework: Controller/Model Unit Tests and View Unit Tests.

4a. To Run Controller/Model Unit Tests:
 *  Note that when the NextMile solution is deployed to AppHarbor (currently 
    configured to occur automatically on deployment to github) the Visual Studio
    unit tests will run automatically, block AppHarbor deployment on any unit test
    failures, and display a unit test result summary in the view for the deployment.
 *  To run manually using Visual Studio 2013, first perform a full build of the solution.
    Then in Visual Studio select "TEST" -> "Run" -> "All Tests".
    All tests will run and "Test Runs Finished" will show up in the Output window.
    Open the "Test Explorer" view for a summary of the unit test passes and failures.
    To open Test Explorer, select "TEST" -> "Windows" -> "Test Explorer".

4b. To Run View Unit Tests:
 *  QUnit framework is used for testing Javascript and Views. QUnitTests are placed
    under QUnitTests folder and IndexTest View  which is a replica of Index page is 
    created under views and also associated with IndexTest Controller to pass test data on this 
    page load. Extra thing in this file is addition of qunit related div
    for the test to run automatically.
 *  Goto to index page/Home/IndexTest to run all the tests and see the results.