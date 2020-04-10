//I'm not redetailing on the repeated code in this file, check MainMap.js to see the details on everything 
var map = L.map('mapid', {
    crs: L.CRS.Simple
});

var building = document.getElementById("building").value;
var floor = document.getElementById("floor").value;
var bounds = [[0, 0], [1000, 1000]];

var image = L.imageOverlay('/images/' + building + floor + '.svg', bounds).addTo(map);

map.fitBounds(bounds);

function highlightFeature(e) {
    var layer = e.target;

    layer.setStyle({
        weight: 5,
        color: '#666',
        dashArray: '',
        fillOpacity: 0.7
    });

    if (!L.Browser.ie && !L.Browser.opera && !L.Browser.edge) {
        layer.bringToFront();
    }
}

function resetHighlight(e) {
    geoJson.resetStyle(e.target);
}

function selectRoom(e) { //this sets the textboxes to display the selected room information to edit
    document.getElementById("idLabel").value = e.target.feature.properties.id; 
    document.getElementById("numberLabel").value = e.target.feature.properties.number
    L.DomEvent.stopPropagation(e); //this is used so when you click a room, it doesn't trigger the map canvas onClick()
}

var markers = L.layerGroup().addTo(map); //makes an empty marker group and places it on our map

map.on('click', function (e) {
    var newMarker = new L.marker(e.latlng).addTo(markers); //places a marker in the marker group at the mouses coordinates
    document.getElementById("coords").value += (e.latlng.lng + ", " + e.latlng.lat + ";") //adds the coords to the input, so it can be sent to server at the end
});

function clearMarkers() { //gets rid of the markers on the map, and resets the input
    document.getElementById("coords").value = "";
    markers.clearLayers();
}


function onEachFeature(feature, layer) {
    layer.on({
        click: selectRoom,
        mouseover: highlightFeature,
        mouseout: resetHighlight,
    });
}



