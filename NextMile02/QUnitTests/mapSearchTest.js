/***TEST****/
QUnit.begin(function (details) {
    console.log("Test amount:", details.totalTests);
});

QUnit.module("Module - Preset values for day and mealtime dropdowns");
QUnit.test('DayDropDownListTestDefault', function (assert) {
    assert.equal(document.getElementById('dayOfWeek').value, "Wednesday", "Test today day set");
    assert.equal(document.getElementById('mealTime').value, "Lunch", "Test today meal time set");
});

QUnit.module("Module - Testing dropdowns after Filtering");
QUnit.test('TruckNameDropDownListTestDefault', function (assert) {
    assert.equal(document.getElementById('foodTruckName').value, allTrucknames, "Test Dropdown Truck Name Selected on Page Load");
});

QUnit.test('NeighborhoodDropDownListTestDefault', function (assert) {
    assert.equal(document.getElementById('neighbourhoodList').value, allNeighborhoods, "Test Dropdown Neighborhood Name Selected on Page Load");
});

QUnit.test('populateTruckNameDropDownListTest1', function (assert) {
    populateTruckNameDropDownList(["Bon Me", "Roxy Cafe", "Mango Thai"], "Bon Me");
    assert.equal(document.getElementById('foodTruckName').value, "Bon Me", "Test Dropdown Truck Name Selected value");
});

QUnit.test('populateTruckNameDropDownListTestNoTrucksFound', function (assert) {
    populateTruckNameDropDownList([allTrucknames], "Bon Me");
    assert.equal(document.getElementById('foodTruckName').value, "Bon Me", "Test Dropdown Truck Name Selected value");
});

QUnit.test('populateNeighborhoodDropDownListTest1', function (assert) {
    populateNeighborhoodDropDownList(["Boston Public Library", "City Hall Plaza", "NEU, on Opera House"], "NEU, on Opera House");
    assert.equal(document.getElementById('neighbourhoodList').value, "NEU, on Opera House", "Test Dropdown Neigbhorhood Selected value");
});

QUnit.test('populateNeighborhoodDropDownListTestNoNeighborhoodFound', function (assert) {
    populateNeighborhoodDropDownList([allNeighborhoods], "NEU, on Opera House");
    assert.equal(document.getElementById('neighbourhoodList').value, "NEU, on Opera House", "Test Dropdown Neigbhorhood Selected value");
});

QUnit.module("Module - Testing pushpins in Map View");
QUnit.test('RenderMapTestPushpins1', function (assert) {
    map.entities.clear();
    renderMap([{
        latitude: 42.3620209,
        location: "Boston Police Headquarters",
        longitude: -71.0607565,
        meal: "Lunch",
        preference: null,
        truckName: "Capriotti's",
        website: ""
    }]);
    assert.equal(map.entities.getLength(), 3, "Testing Number of Entities on Map");

    var pushpinTest = map.entities.pop();
    assert.equal(pushpinTest.Title, "Capriotti's", "Check Title");
    assert.equal(pushpinTest.Description, "Boston Police Headquarters<br>Lunch<br>", "Check Description");
});

QUnit.test('RenderMapTest3PushpinsWithPreferences', function (assert) {
    map.entities.clear();
    renderMap([{
        latitude: 42.3620209,
        location: "Boston Police Headquarters",
        longitude: -71.0607565,
        meal: "Lunch",
        preference: null,
        truckName: "Capriotti's",
        website: ""
    },
    {
        latitude: 42.351202,
        location: "Back Bay, Copley Square North at Clarendon St",
        longitude: -71.075451,
        meal: "Dinner",
        preference: 1,
        truckName: "Compliments",
        website: "http://www.complimentsfood.com"
    },
    {
        latitude: 42.3562804,
        location: "Financial District, Pearl Street at Franklin",
        longitude: -71.0570633,
        meal: "Breakfast",
        preference: 2,
        truckName: "Chicken & Rice Guys",
        website: "http://thechickenriceguys.com"
    }]);
    assert.equal(map.entities.getLength(), 5, "Testing Number of Entities on Map");

    var pushpin3 = map.entities.pop();
    assert.equal(pushpin3.Title, "Chicken & Rice Guys", "Check Title");
    assert.equal(pushpin3.Description, "Financial District, Pearl Street at Franklin<br>Breakfast<br>http://thechickenriceguys.com", "Check Description");
    assert.equal(pushpin3.getIcon(), "/Content/Images/Red_Truck.png", "Check Icon assigned");

    var pushpin2 = map.entities.pop();
    assert.equal(pushpin2.Title, "Compliments", "Check Title");
    assert.equal(pushpin2.Description, "Back Bay, Copley Square North at Clarendon St<br>Dinner<br>http://www.complimentsfood.com", "Check Description");
    assert.equal(pushpin2.getIcon(), "/Content/Images/Green_Truck.png", "Check Icon assigned");

    var pushpin1 = map.entities.pop();
    assert.equal(pushpin1.Title, "Capriotti's", "Check Title");
    assert.equal(pushpin1.Description, "Boston Police Headquarters<br>Lunch<br>", "Check Description");
    assert.equal(pushpin1.getIcon(), "/Content/Images/Blue_Truck.png", "Check Icon assigned");
});

QUnit.module("Module - Global");
/*
QUnit.log(function (details) {
    console.log("QUnit: '" + details.name + "' = "
        + (details.result ? "PASS" : "FAIL,  " + "'" + details.message + "'"));
});

QUnit.done(function (details) {
    console.log("Total: ", details.total, " Failed: ", details.failed, " Passed: ", details.passed, " Runtime: ", details.runtime);
});*/