var map = L.map('mapid', {
    crs: L.CRS.Simple //creates new map using CRS.Simple which uses a 1000,1000 coord system instead of geospacial
});


// Grabs the URL of the page and splits it on the '?' and looks for map=(SomeMapName) then sets images accordingly. Only two maps right now so only two statements
var URL = window.location.href;
var mapRequest = URL.split("?");
var bounds = [[0, 0], [1000, 1000]];

if (mapRequest[1] == "map=Darden1stFloor") {
    var image = L.imageOverlay('/images/Darden1st.svg', bounds).addTo(map);
    document.getElementById("hMapTitle").innerHTML = "Darden 1st Floor";
}
else if (mapRequest[1] == "map=Darden2ndFloor") {
    var image = L.imageOverlay('/images/Darden2nd.svg', bounds).addTo(map); //this will need to be a parameter eventually, since we don't want to hardcore the 2nd floor map in
    document.getElementById("hMapTitle").innerHTML = "Darden 2nd Floor";
}
else {
    var image = L.imageOverlay('/images/Darden2nd.svg', bounds).addTo(map);
    document.getElementById("hMapTitle").innerHTML = "Darden 2nd Floor";
}

map.fitBounds(bounds);
var scheduleJSON = {};
var rowNum = 0;

function highlightFeature(e) { //this function changes the style of a highlighted room
    var layer = e.target;

    layer.setStyle({
        weight: 5,
        color: '#666',
        dashArray: '',
        fillOpacity: 0.7
    });

    if (!L.Browser.ie && !L.Browser.opera && !L.Browser.edge) { //doesn't work right on edge, ie, or opera
        layer.bringToFront();
    }
}

function resetHighlight(e) { //reset styling when not highlighted
    geoJson.resetStyle(e.target);
}

function zoomToFeature(e) { //this function is called when a room is clicked, we want to remove this and add updated the side panel instead
    map.fitBounds(e.target.getBounds());
}

function getSchedule(e) {

    var roomID = e.target.feature.properties.id;

    scheduleJSON = $.getJSON('https://localhost:44317/?handler=Info&Building=Darden&rmId=' + roomID, function (result) {
        console.log(result);

        var table = document.getElementById("InfoPanelTable");
        
        //var obj = JSON.stringify(result);
        //obj = JSON.parse(obj);
        //console.log(obj[0].avId)
        //console.log(obj[3]);
        //console.log(result.Array)

        if (rowNum === 0) {
            for (var i = 0; i < result.length; i++) {
                var row = table.insertRow(rowNum);
                rowNum += 1;
                row.innerHTML = "Name of Class:" + " " + result[i].avName;

                row = table.insertRow(rowNum);
                rowNum += 1;
                row.innerHTML = "Start Time:" + " " + result[i].avStartTime;

                row = table.insertRow(rowNum);
                rowNum += 1;
                row.innerHTML = "End Time:" + " " + result[i].avEndTime;

            }
        }

        else {
            for (var i = table.rows.length - 1; i >= 0; i--) {
                table.deleteRow(i);
                rowNum -= 1;
            }

            for (var i = 0; i < result.length; i++) {
                var row = table.insertRow(rowNum);
                rowNum += 1;
                row.innerHTML = "Name of Class:" + " " + result[i].avName;

                row = table.insertRow(rowNum);
                rowNum += 1;
                row.innerHTML = "Start Time:" + " " + result[i].avStartTime;

                row = table.insertRow(rowNum);
                rowNum += 1;
                row.innerHTML = "End Time:" + " " + result[i].avEndTime;

            }
        }
        console.log(rowNum);
        console.log(table.rows.length);
        
    });

}

function onEachFeature(feature, layer) { //adds the functions to each created feature
    layer.on({
        click: getSchedule,
        mouseover: highlightFeature,
        mouseout: resetHighlight 
    });
}



