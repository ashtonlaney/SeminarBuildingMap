var map = L.map('mapid', {
    crs: L.CRS.Simple, //creates new map using CRS.Simple which uses a 1000,1000 coord system instead of geospacial
    minZoom: 0.5,
    zoomSnap: 0.25
});



// Grabs the URL of the page and splits it on the '?' and looks for map=(SomeMapName) then sets images accordingly. Only two maps right now so only two statements
var building = document.getElementById("building").value;
var floor = document.getElementById("floor").value;
var bounds = [[0, 0], [1000, 1000]];

var image = L.imageOverlay('/images/'+ building + floor + '.svg', bounds).addTo(map); //this will need to be a parameter eventually, since we don't want to hardcore the 2nd floor map in


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

function decodeHtml(html) {
    var txt = document.createElement("textarea");
    txt.innerHTML = html;
    return txt.value;
}

function getSchedule(e) {

    var roomID = e.target.feature.properties.id;
    var name = e.target.feature.properties.name;

    scheduleJSON = $.getJSON('https://localhost:44317/?handler=Info&Building=Darden&rmId=' + roomID, function (result) {
        console.log(result);

        var table = document.getElementById("InfoPanelTable");
        
        //var obj = JSON.stringify(result);
        //obj = JSON.parse(obj);
        //console.log(obj[0].avId)
        //console.log(obj[3]);
        //console.log(result.Array)

        if (rowNum != 0) {
            for (var i = table.rows.length - 1; i >= 0; i--) {
                table.deleteRow(i);
                rowNum -= 1;
            }  
        }
        
        for (var i = 0; i < result.length; i++) {
            var row = table.insertRow(rowNum);
            rowNum += 1;

            let cell = row.insertCell()
            let text = document.createTextNode(decodeHtml(result[i].avName));
            cell.appendChild(text);

            cell = row.insertCell()
            text = document.createTextNode(decodeHtml(result[i].avStartTime));
            cell.appendChild(text);

            cell = row.insertCell()
            text = document.createTextNode(decodeHtml(result[i].avEndTime));
            cell.appendChild(text);
        }

        if (result.length > 0) {

            let headers = ["Name", "Start Time", "End Time"]

            let thead = table.createTHead();
            let row = thead.insertRow();
            let th = document.createElement("th");
            th.colSpan = 3;
            let text = document.createTextNode(decodeHtml(name));
            th.appendChild(text);
            row.appendChild(th);
            rowNum += 1;

            row = thead.insertRow();
            rowNum += 1;
            for (let key of headers) {
                th = document.createElement("th");
                text = document.createTextNode(decodeHtml(key));
                th.appendChild(text);
                row.appendChild(th);
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



