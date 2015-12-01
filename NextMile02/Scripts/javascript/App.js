/*
    Author : Shivastuti Koul
    File   : app.js
    Details: JS for 'Console'.
    Email  : koul.sh@husky.neu.edu
*/

var app = angular.module("NEXTMILEApp", ["ngRoute"]);
console.log("%c>NEXTMILE",
            "color: navy; font-family: Courier New; font-weight: bold");


app.config(function($routeProvider)
{
    console.log("%c>Main controller has been initialized",
                "font-family: Courier New;");

    $routeProvider
    .when("/",
    {
        templateUrl: "../../Views/Home/Welcome.cshtml",
        controller: "LoginCtrl"
    })
    .when("/profile",
    {
        templateUrl: "../../Views/Account/UserDetails.cshtml",
        controller: "ProfileCtrl"
    })
});
/* End of app.js */