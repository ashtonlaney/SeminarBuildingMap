var map = L.map('mapid', {
    crs: L.CRS.Simple
});

var bounds = [[0, 0], [1000, 1000]];
var image = L.imageOverlay('/images/2ndFloorFinal.svg', bounds).addTo(map);
map.fitBounds(bounds);