var map = L.map('mapid', {
    crs: L.CRS.Simple
});

var building = document.getElementById("building").value;
var floor = document.getElementById("floor").value;
var bounds = [[0, 0], [1000, 1000]];

var image = L.imageOverlay('/images/' + building + floor + '.svg', bounds).addTo(map); //this will need to be a parameter eventually, since we don't want to hardcore the 2nd floor map in

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

function selectRoom(e) {
    document.getElementById("idLabel").value = e.target.feature.properties.id;
    document.getElementById("numberLabel").value = e.target.feature.properties.number
    L.DomEvent.stopPropagation(e);
}

var markers = L.layerGroup().addTo(map);

map.on('click', function (e) {
    var newMarker = new L.marker(e.latlng).addTo(markers);
    document.getElementById("coords").value += (e.latlng.lng + ", " + e.latlng.lat + ";")
});

function clearMarkers() {
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

L.control.mousePosition().addTo(map);



