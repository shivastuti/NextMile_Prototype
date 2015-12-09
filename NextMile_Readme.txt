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
  


##############################################################################
Part 2: Operating Instructions
------------------------------------------------------------------------------


##############################################################################
Part 3: Management Instructions
------------------------------------------------------------------------------
[ @Shiva: Please update this section ]


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
[ @Smitha: Please update this section]

