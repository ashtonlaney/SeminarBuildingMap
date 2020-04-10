var map = L.map('mapid', {
    crs: L.CRS.Simple, //creates new map using CRS.Simple which uses a 1000,1000 coord system instead of geospacial
    minZoom: 0.5, //sets zoom settings
    zoomSnap: 0.25
});



// Grab query parameters for building and floor out of hidden inputs, set by the server
var building = document.getElementById("building").value;
var floor = document.getElementById("floor").value;
var bounds = [[0, 0], [1000, 1000]];

var image = L.imageOverlay('/images/'+ building + floor + '.svg', bounds).addTo(map);  //dynamically set the image


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

function decodeHtml(html) { //this is used for html -> js since it does some weird encoding with special characters that needs to be fixed, this is a roundabout and simple way to do so
    var txt = document.createElement("textarea");
    txt.innerHTML = html;
    return txt.value;
}

function getSchedule(e) { //does the ajax to get schedules for selected rooms

    var roomID = e.target.feature.properties.id; //gets the stored properties of the selected room from its geoJSON
    var name = e.target.feature.properties.name;

    scheduleJSON = $.getJSON('https://localhost:44317/?handler=Info&rmId=' + roomID, function (result) { //call ajax query

        var table = document.getElementById("InfoPanelTable"); //grab the table to insert it into
        
        if (rowNum != 0) { //if rows exist in the table, clear them out
            for (var i = table.rows.length - 1; i >= 0; i--) {
                table.deleteRow(i);
                rowNum -= 1;
            }  
        }
        
        for (var i = 0; i < result.length; i++) { //build the table from scratch
            var row = table.insertRow(rowNum);
            rowNum += 1; //keep track of rows

            let cell = row.insertCell() //make cell
            let text = document.createTextNode(decodeHtml(result[i].avName)); //grab the first column from the ajax response
            cell.appendChild(text); //place the text into the cell for the table

            cell = row.insertCell() //repeat for other columns
            text = document.createTextNode(decodeHtml(result[i].avStartTime));
            cell.appendChild(text);

            cell = row.insertCell()
            text = document.createTextNode(decodeHtml(result[i].avEndTime));
            cell.appendChild(text);
        }

        if (result.length > 0) { //this is used to make the headers

            let headers = ["Name", "Start Time", "End Time"]

            let thead = table.createTHead(); //this block sets the room name
            let row = thead.insertRow();
            let th = document.createElement("th");
            th.colSpan = 3;
            let text = document.createTextNode(decodeHtml(name));
            th.appendChild(text);
            row.appendChild(th);
            rowNum += 1;

         
            }

        row = thead.insertRow();
        rowNum += 1;
        for (let key of headers) { //this loop is for each indvidual column header
            th = document.createElement("th");
            text = document.createTextNode(decodeHtml(key));
            th.appendChild(text);
            row.appendChild(th);

        }
        
    });

}

function onEachFeature(feature, layer) { //adds the functions to each created room
    layer.on({
        click: getSchedule,
        mouseover: highlightFeature,
        mouseout: resetHighlight 
    });
}



