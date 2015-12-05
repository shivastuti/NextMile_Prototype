
var map = null, infobox = null, datalayer = null;
var bestview = null;
var pushPinWidth = 35;
var pushPinHeight = 35;
var allNeighborhoods = 'All Neighborhoods';
var allTrucknames = 'All Trucks';
var pinClusterer = null;

$(document).ready(function () {
    getTodayDateAndMealTime();

    var AllPushpinInfoData = $("#currentTruckPinData").data("value");
    var mapViewWidth = $("#myMap").width();
    var BingMapKey = $("#BingMapKey").data("value");
    var mapViewHeight = 400;
    var bestViewPadding = 150;

    var mapOptions = {
        credentials: BingMapKey,
        width: mapViewWidth, height: mapViewHeight,
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        showMapTypeSelector: false,
        dcl: 1, padding: bestViewPadding
    };

    map = new Microsoft.Maps.Map(
        document.getElementById("myMap"), mapOptions);

    document.getElementById('neighbourhoodList').value = allNeighborhoods;

    /*Show Clustered Pushpins Map view - For 'Show All' option*/
    pinClusterer = new PinClusterer(map, {
        onClusterToMap: function (pushpin, cluster) {
            Microsoft.Maps.Events.addHandler(pushpin, 'mouseover', function (e) {
                if (e.targetType == 'pushpin') {
                    displayClusteredInfoBox(pushpin, cluster);
                }
            });
            Microsoft.Maps.Events.addHandler(pushpin, 'click', function (e) {
                if (e.targetType == 'pushpin') {
                    displayClusteredInfoBox(pushpin, cluster);
                }
            });
            Microsoft.Maps.Events.addHandler(map, 'viewchange', function (e) {
                    hideClusteredInfoBox(pushpin, cluster);
            });
        },
        onPinToMap: function (pushpin) {
            Microsoft.Maps.Events.addHandler(pushpin, 'mouseover', function (e) {
                if (e.targetType == 'pushpin') {
                    displayInfobox(pushpin);
                }
            });
            Microsoft.Maps.Events.addHandler(pushpin, 'click', function (e) {
                if (e.targetType == 'pushpin') {
                    displayInfobox(pushpin);
                }
            });
            Microsoft.Maps.Events.addHandler(map, 'viewchange', function (e) {
                    hideInfobox();                
            });
        }
    });
    renderPushpinClusteredMap(AllPushpinInfoData, pinClusterer);
});

//pushpin clustered map view
function renderPushpinClusteredMap(AllPushpinInfoData, pinClusterer) {

    //not need here as pin_cluster.js is pushing map layer
    /*dataLayer = new Microsoft.Maps.EntityCollection();
    map.entities.push(dataLayer);*/

    //push the infobox layer on top of the map - only one info box can be present at a time
    var infoboxLayer = new Microsoft.Maps.EntityCollection();
    map.entities.push(infoboxLayer);

    infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(0, 0), { visible: false, offset: new Microsoft.Maps.Point(0, 25) });
    infoboxLayer.push(infobox);

    //set to Boston latitude and longitude
    map.setView({ center: new Microsoft.Maps.Location(42.347168, -71.080233), zoom: 13 });
    //render the clustered map
    pinClusterer.cluster(AllPushpinInfoData);

    datalayer = pinClusterer.layer; 

    // Utilize the user's location if available
    var locProvider = new Microsoft.Maps.GeoLocationProvider(map);
    locProvider.getCurrentPosition({ successCallback: ShowUserPosition }, { errorCallback: onPositionError });
}

// Shows the user's location as Pin
// Zooms the map to current location
// If we don't know user location, do nothing.
// If known location but outside Boston, show pin but don't re-center
// If known location INSIDE Boston, recenter and zoom to user's location
function ShowUserPosition(user) {

    // Recenter map if user is located in Boston
    if (map.getBounds().contains(user.position.coords)) {
        alert("You're in Boston!");
        map.setView({
            zoom: 15,
            center: user.center
        });
    }
    else {
        alert("You're NOT in Boston!");
        //set to Boston latitude and longitude
        map.setView({ center: new Microsoft.Maps.Location(42.347168, -71.080233), zoom: 13 });
    }

    // Create a Pushpin at the user's location
    var userPushpin = new Microsoft.Maps.Pushpin(user.center);
    userPushpin.Title = "You are Here";

    // Add Infobox
    userPinInfobox = new Microsoft.Maps.Infobox(userPushpin.getLocation(),
        {
            title: 'You are Here!',
            width: 100, height: 50,
            visible: true,
            offset: new Microsoft.Maps.Point(0, 15)
        });

    Microsoft.Maps.Events.addHandler(userPushpin, 'mouseover', function (e) {
        if (e.targetType == "pushpin") { displayUserInfobox(e.target) }
    });

    Microsoft.Maps.Events.addHandler(userPushpin, 'click', function (e) {
        if (e.targetType == "pushpin") { hideUserInfobox(e.target) }
    });

    Microsoft.Maps.Events.addHandler(userPushpin, 'viewchange', function (e) {
        if (e.targetType == "pushpin") { hideUserInfobox(e.target) }
    });
    map.entities.push(userPushpin);
    map.entities.push(userPinInfobox);

    //// Marks current location with a circle and sets its border width,
    //// border color and body color
    //geoLocationProvider.addAccuracyCircle(position.center, 30, 30, {
    //    polygonOptions: {
    //        strokeThickness: 2,
    //        fillColor: new Microsoft.Maps.Color(200, 255, 128, 0),
    //        strokeColor: new Microsoft.Maps.Color(255, 0, 128, 0)
    //    }
    //});
}

function displayUserInfobox(e) {
    userPinInfobox.setOptions({ visible: true });
}

function hideUserInfobox(e) {
    userPinInfobox.setOptions({ visible: false });
}

function onPositionError(err) {
    switch (err.code) {
        case 0:
            alert("Unknown error");
            break;
        case 1:
            alert("Browser Location settings are disabled by User.");
            break;
        case 2:
            alert("Location data unavailable.");
            break;
        case 3:
            alert("Location request timed out.");
            break;
    }
}

//display info box for clustered pushpins
//Number of pins and truck names are displayed
function displayClusteredInfoBox(pushpin, cluster) {
    infobox.setLocation(pushpin.getLocation());
    var infoBoxTitle = cluster.locations.length + ' Trucks Found.';
    var truckNames = '';
    var numOfTrucks = cluster.truckNamesInCluster.length;
    var infoBoxWidth = 275;
    var infoBoxHeight = 100 + (15 * numOfTrucks);

    var appendStr = "<br>";
    if (numOfTrucks > 15) {
        appendStr = ","; //Append , instead of <br> if the number of trucks to be displayed is more than 10
        infoBoxWidth = 300;
        infoBoxHeight = 100 + (8 * numOfTrucks);
    }

    for (i = 0; i < (numOfTrucks - 1) ; i++) {
        truckNames += cluster.truckNamesInCluster[i] + appendStr;
    }
    truckNames += cluster.truckNamesInCluster[numOfTrucks - 1];

    infobox.setOptions({
        title: infoBoxTitle,
        description: truckNames,
        width: infoBoxWidth, height: infoBoxHeight,
        visible: true,
        offset: new Microsoft.Maps.Point(0, 25),
        actions: [{ label: 'Double Click to Zoom In', eventHandler: function (mouseEvent) { cluster.zoom() } }]
    });

    infoboxAdjustToMapView(pushpin);
};

function hideClusteredInfoBox(pushpin) {
    infobox.setOptions({ visible: false });
};


function renderMap(PushpinInfoData) {
    var locationlist = []

    dataLayer = new Microsoft.Maps.EntityCollection();
    map.entities.push(dataLayer);

    var infoboxLayer = new Microsoft.Maps.EntityCollection();
    map.entities.push(infoboxLayer);

    infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(0, 0), { visible: false, offset: new Microsoft.Maps.Point(0, 25) });
    infoboxLayer.push(infobox);

    $.each(PushpinInfoData, function (i, truckPP) {
        // Select icon image based on user preference if any
        var image = "/Content/Images/Blue_Truck.png";
        if (truckPP.preference == 1)
            image = "/Content/Images/Green_Truck.png";
        else if (truckPP.preference == 2)
            image = "/Content/Images/Red_Truck.png";

        var pushpin = new Microsoft.Maps.Pushpin(map.getCenter(),
        {
            icon: image, width: pushPinWidth, height: pushPinHeight

        });
        var loc = new Microsoft.Maps.Location(truckPP.latitude, truckPP.longitude);
        pushpin.setLocation(loc);
        pushpin.Title = truckPP.truckName;
        var description = truckPP.location.concat("<br>").concat(truckPP.meal).concat("<br>").concat(truckPP.website);
        pushpin.Description = description;

        map.entities.push(pushpin);

        Microsoft.Maps.Events.addHandler(pushpin, 'click', function (e) {
            if (e.targetType == "pushpin") { displayInfobox(e.target) }
        });
        // Hide the info box when the map is moved.
        Microsoft.Maps.Events.addHandler(map, 'viewchange', function (e) {
            if (e.targetType == "pushpin") { hideInfobox(e.target) }
        });
        Microsoft.Maps.Events.addHandler(pushpin, 'mouseover', function (e) {
            if (e.targetType == "pushpin") { displayInfobox(e.target) }
        });
        //Microsoft.Maps.Events.addHandler(pushpin, 'mouseout', hideInfobox); -- To do giving timer

        dataLayer.push(pushpin);
        locationlist.push(loc);
    });

    /* If there is only one Location then LocationRect does not Zoom-in
       So additional logic to go to default zoom level added when there in only one pushpin*/
    if (locationlist.length == 1) {
        map.setView({ center: locationlist[0], zoom: 17 });
    } else if (locationlist.length > 1) {
        /*To determine the bounding box for the map that fits all trucks*/
        bestview = new Microsoft.Maps.LocationRect.fromLocations(locationlist);
        map.setView({
            bounds: bestview
        });
    }

    // Utilize the user's location if available
    var locProvider = new Microsoft.Maps.GeoLocationProvider(map);
    locProvider.getCurrentPosition({ successCallback: ShowUserPosition }, { errorCallback: onPositionError });
}

function displayInfoboxSettings(pushpin) {
    var infoBoxWidth = 275;
    var infoBoxHeight = 135;
    var infoBoxDesciption = pushpin.Description;
    var infoBoxTitle = pushpin.Title;
    var strLength = infoBoxDesciption.length + infoBoxTitle.length;
    console.log(infoBoxTitle + " " + infoBoxDesciption);

    if (strLength > 70)
        infoBoxHeight = 160;
    else if (strLength > 100)
        infoBoxHeight = 210;
    else if (strLength > 120)
        infoBoxHeight = 270;

    var pix = map.tryLocationToPixel(pushpin.getLocation(), Microsoft.Maps.PixelReference.control);
    infobox.setLocation(pushpin.getLocation());
    infobox.setOptions({
        visible: true, title: infoBoxTitle, description: infoBoxDesciption, width: infoBoxWidth, height: infoBoxHeight,
        offset: new Microsoft.Maps.Point(0, 25),
        actions: [{ label: '<div id="Upvote"><img class="Image" src="/Content/Images/Upvote.png" /></div>', eventHandler: function (mouseEvent) { btnVoteHandler(pushpin, "1") } },
        { label: '<div id="Downvote"><img class="Image" src="/Content/Images/Downvote.png" /></div>', eventHandler: function (mouseEvent) { btnVoteHandler(pushpin, "2") } }
        ]
    });
}

function displayInfobox(pushpin) {
    displayInfoboxSettings(pushpin);
    infoboxAdjustToMapView(pushpin);
}

//to show entire contents of infobox in map view
function infoboxAdjustToMapView(pushpin) {
    //A buffer limit to use to specify the infobox must be away from the edges of the map.
    var buffer = 25;

    var infoboxOffset = infobox.getOffset();
    var infoboxAnchor = infobox.getAnchor();
    var infoboxLocation = map.tryLocationToPixel(pushpin.getLocation(), Microsoft.Maps.PixelReference.control);

    var dx = infoboxLocation.x + infoboxOffset.x - infoboxAnchor.x;
    var dy = infoboxLocation.y - 25 - infoboxAnchor.y;

    if (dy < buffer) {    //Infobox overlaps with top of map.
        //Offset in opposite direction.
        dy *= -1;

        //add buffer from the top edge of the map.
        dy += buffer;
    } else {
        //If dy is greater than zero than it does not overlap.
        dy = 0;
    }

    if (dx < buffer) {    //Check to see if overlapping with left side of map.
        //Offset in opposite direction.
        dx *= -1;

        //add a buffer from the left edge of the map.
        dx += buffer;
    } else {              //Check to see if overlapping with right side of map.
        dx = map.getWidth() - infoboxLocation.x + infoboxAnchor.x - infobox.getWidth();

        //If dx is greater than zero then it does not overlap.
        if (dx > buffer) {
            dx = 0;
        } else {
            //add a buffer from the right edge of the map.
            dx -= buffer;
        }
    }

    //Adjust the map so infobox is in view
    if (dx != 0 || dy != 0) {
        map.setView({ centerOffset: new Microsoft.Maps.Point(dx, dy), center: map.getCenter() });
    }
}

function hideInfobox() {
    infobox.setOptions({ visible: false });
    /*if (bestview != null) {
        map.setView({
            bounds: bestview
        });
    }*/
}

//function to Upvote or Downvote a truck
function btnVoteHandler(pushpin, vote) {
    var truckName = pushpin.Title;
    $.ajax({
        url: "/Home/VoteFoodTruck",
        type: "POST",
        dataType: "json",
        data: "foodTruckName=" + encodeURIComponent(JSON.stringify(truckName)) + "&vote=" + encodeURIComponent(vote),
        success: function (data) {
            if (data.success) {
                changePushPinColor(pushpin, data);
            } else {
                alert(data.message);
            }
        },
        error: function () {
            alert("Preference Not Successfully Saved"); // will be removed if everything works
        }
    });
}

/*Dynamically change the pin truck color on selection */
function changePushPinColor(pushpin, data) {
    //console.log("changePushPinColor" + pushpin.Title + " " + data.newIconColor);
    if (data.newIconColor == 0) {
        pushpin.setOptions({ icon: "/Content/Images/Blue_Truck.png" });
    } else if (data.newIconColor == 1) {
        pushpin.setOptions({ icon: "/Content/Images/Green_Truck.png" });
    } else if (data.newIconColor == 2) {
        pushpin.setOptions({ icon: "/Content/Images/Red_Truck.png" });
    }

    /*Instead of Refetching the complete data we just send update for a
    changed truck data to be stored in pin_cluster truckPinData*/
    var dropList = document.getElementById('neighbourhoodList');
    if (dropList.options[dropList.selectedIndex].value === allNeighborhoods) {
        //console.log("show all");
        for (i = 0; i < pinClusterer._truckPinData.length; i++) {
            if (pinClusterer._truckPinData[i].truckName === pushpin.Title) {
                pinClusterer._truckPinData[i].preference = parseInt(data.newIconColor);
                break;
            }
        }
    }
}

//initialse today date and time
function getTodayDateAndMealTime() {
    var todayDate = new Date();
    var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    var mealTimes = ['Breakfast', 'Lunch', 'Dinner', 'Late Night'];
    var today = days[todayDate.getDay()];

    var mealTime = "";
    if (todayDate.getHours() >= 0 && todayDate.getHours() < 10)
    { mealTime = "Breakfast"; }
    else if (todayDate.getHours() >= 10 && todayDate.getHours() < 15)
    { mealTime = "Lunch"; }
    else if (todayDate.getHours() >= 15 && todayDate.getHours() < 22)
    { mealTime = "Dinner"; }
    else
    { mealTime = "Late Night"; }

    var dropdownListMealTime = document.getElementById('mealTime');
    for (var i = 0; i < mealTimes.length; i++)
    {
        var optn = document.createElement("OPTION");
        optn.text = optn.value = mealTimes[i];
        dropdownListMealTime.options.add(optn);
    }

    var dropdownListDays = document.getElementById('dayOfWeek');
    for (var i = 0; i < days.length; i++) {
        var optn = document.createElement("OPTION");
        optn.text = optn.value = days[i];
        dropdownListDays.options.add(optn);
    }

    //set current values
    dropdownListMealTime.value = mealTime;
    dropdownListDays.value = today;

}

function RenderFilteredTrucks() {
    var dropListNeighborhood = document.getElementById('neighbourhoodList');
    var neighborhoodSelected = dropListNeighborhood.options[dropListNeighborhood.selectedIndex].value;
    var dropListMeal = document.getElementById('mealTime');
    var mealSelected = dropListMeal.options[dropListMeal.selectedIndex].value;
    var dropListDay = document.getElementById('dayOfWeek');
    var daySelected = dropListDay.options[dropListDay.selectedIndex].value;
    var dropListTruckName = document.getElementById('foodTruckName');
    var trucknameSelected = dropListTruckName.options[dropListTruckName.selectedIndex].value;

    console.log(neighborhoodSelected + " " + mealSelected + " " + daySelected + " " + trucknameSelected);

    $.ajax({
        url: "../../Home/FilterTrucks",
        type: "POST",
        dataType: "json",
        data: "neighborhood=" + encodeURIComponent(JSON.stringify(neighborhoodSelected)) + "&truckname=" + encodeURIComponent(JSON.stringify(trucknameSelected)) + "&day=" + JSON.stringify(daySelected) + "&meal=" + JSON.stringify(mealSelected),
        success: function (PushpinFilteredData) {
            map.entities.clear();
            if (dropListNeighborhood.options[dropListNeighborhood.selectedIndex].value === allNeighborhoods) {
                //Pin Clustered Map View
                /*Show Clustered Pushpins Map view - For 'Show All' option*/
                if (pinClusterer != null) {
                    pinClusterer.reinitialize(map);
                    renderPushpinClusteredMap(PushpinFilteredData, pinClusterer);
                }
            } else {
                //Zoomed in Map View
                renderMap(PushpinFilteredData);
            }
        }
    });
}



