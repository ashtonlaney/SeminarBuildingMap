var map = L.map('mapid', {
    crs: L.CRS.Simple
});

var bounds = [[0, 0], [1000, 1000]];
var image = L.imageOverlay('/images/2ndFloorFinal.svg', bounds).addTo(map);
map.fitBounds(bounds);

var geojsonFeature = {
    "type": "Feature",
    "properties": {
        "name": "Coors Field",
        "amenity": "Baseball Stadium",
        "popupContent": "This is where the Rockies play!"
    },
    "geometry": {
        "type": "Polygon",
        "coordinates": [[
            [293, 459.9],
            [295, 513.5],
            [330.5, 515],
            [330.5, 459.9]
        ]]
    }
};


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
    geoJSON.resetStyle(e.target);
}

function zoomToFeature(e) {
    map.fitBounds(e.target.getBounds());
}

function onEachFeature(feature, layer) {
    layer.on({
        click: highlightFeature,
        mouseout: resetHighlight,
    });
}

L.geoJSON(geojsonFeature, { onEachFeature: onEachFeature }).addTo(map);

