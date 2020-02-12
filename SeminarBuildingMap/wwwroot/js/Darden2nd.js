var map = L.map('mapid', {
    crs: L.CRS.Simple
});

var bounds = [[0, 0], [1000, 1000]];
var image = L.imageOverlay('/images/Darden2nd.svg', bounds).addTo(map);
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

function zoomToFeature(e) {
    map.fitBounds(e.target.getBounds());
    document.getElementById("testlabel2").textContent = e.target.feature.properties.name;
}

map.on('click', function (e) {
    document.getElementById("testlabel").textContent += (e.latlng.lng + ", " + e.latlng.lat + ";")
});

function onEachFeature(feature, layer) {
    layer.on({
        click: zoomToFeature,
        mouseover: highlightFeature,
        mouseout: resetHighlight 
    });
}

L.control.mousePosition().addTo(map);



