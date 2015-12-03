
var map = null, infobox = null, datalayer = null;
var bestview = null;
var pushPinWidth = 35;
var pushPinHeight = 35;
var showAll = 'Show All';

$(document).ready(function () {
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

    document.getElementById('neighbourhoodList').value = showAll;

    /*Show Clustered Pushpins Map view - For 'Show All' option*/
    pinClusterer = new PinClusterer(map, {
        onClusterToMap: function (pushpin, cluster) {
            Microsoft.Maps.Events.addHandler(pushpin, 'mouseover', function (e) {
                if (e.targetType == 'pushpin') {
                    displayClusteredInfoBox(pushpin, cluster);
                }
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

    for (i = 0; i < (numOfTrucks - 1); i++) {
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
    } else {
        /*To determine the bounding box for the map that fits all trucks*/
        bestview = new Microsoft.Maps.LocationRect.fromLocations(locationlist);
        map.setView({
            bounds: bestview
        });
    }
}

function displayInfoboxSettings(pushpin) {
    var infoBoxWidth = 275;
    var infoBoxHeight = 135;
    var infoBoxDesciption = pushpin.Description;
    var infoBoxTitle = pushpin.Title;
    var strLength = infoBoxDesciption.length + infoBoxTitle.length;

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

function hideInfobox(pushpin) {
    infobox.setOptions({ visible: false });
    if (bestview != null) {
        map.setView({
            bounds: bestview
        });
    }
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
    if (dropList.options[dropList.selectedIndex].value === showAll) {
        //console.log("show all");
        for (i = 0; i < pinClusterer._truckPinData.length; i++) {
            if (pinClusterer._truckPinData[i].truckName === pushpin.Title) {
                pinClusterer._truckPinData[i].preference = parseInt(data.newIconColor);
                break;
            }
        }
    }
}

function RenderSpecificTrucksNew(e) {
    var dropList = document.getElementById('neighbourhoodList');
    var selectedLocation = dropList.options[dropList.selectedIndex].value;

    $.ajax({
        url: "../../Home/FilterNeighborhood",
        type: "POST",
        dataType: "json",
        data: "selection=" + JSON.stringify(selectedLocation),
        success: function (PushpinFilteredData) {
            map.entities.clear();
            if (dropList.options[dropList.selectedIndex].value === showAll) {
                //Pin Clustered Map View
                /*Show Clustered Pushpins Map view - For 'Show All' option*/
                pinClusterer = new PinClusterer(map, {
                    onClusterToMap: function (pushpin, cluster) {
                        Microsoft.Maps.Events.addHandler(pushpin, 'mouseover', function (e) {
                            if (e.targetType == 'pushpin') {
                                displayClusteredInfoBox(pushpin, cluster);
                            }
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
                    }
                });
                renderPushpinClusteredMap(PushpinFilteredData, pinClusterer);
            } else {
                //Zoomed in Map View
                renderMap(PushpinFilteredData);
            }
        }
    });
}



