var map = L.map('mapid', {
    crs: L.CRS.Simple //creates new map using CRS.Simple which uses a 1000,1000 coord system instead of geospacial
});

var bounds = [[0, 0], [1000, 1000]];
var image = L.imageOverlay('/images/Darden2nd.svg', bounds).addTo(map); //this will need to be a parameter eventually, since we don't want to hardcore the 2nd floor map in
map.fitBounds(bounds);
var scheduleJSON = {};

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

    scheduleJSON = $.getJSON('https://localhost:44317/?handler=Info&Building=Darden&RoomNumber=210', function (result) {
        console.log(result)
        // replaces text next to map. This is just a test. I will have to parse through json
        document.getElementById("test").innerHTML = result[0];
    });

}

function onEachFeature(feature, layer) { //adds the functions to each created feature
    layer.on({
        click: zoomToFeature,
        mouseover: highlightFeature,
        mouseout: resetHighlight 
    });

    layer.on({
        click: getSchedule
    });
}



