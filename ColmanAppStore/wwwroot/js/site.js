
//Dark mode function
$(document).ready(function () {
    $('#checkbox').click(function () {
        var element = document.body;
        element.classList.toggle("dark");
        document.getElementById("navBarStyle").classList.toggle("dark");
        document.getElementById("mainWindow").classList.toggle("dark");
        document.getElementById("header").classList.toggle("dark");
        document.getElementById("footerStyle").classList.toggle("dark");
    });
});


function GetMap() {
    var map = new Microsoft.Maps.Map('#myMap', {
        credentials: 'ArSVHkQb2Q4uCxFV8HB41H0ZCCNbXAhEWXyXUYlJpAHnTFGRTbGRxXVSp9l0aOAQ',

        //setting tel aviv as map center
        center: new Microsoft.Maps.Location(31.9700919, 34.77205380048267),
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        zoom: 8
    });

    let name;
    const bing_key = 'ArSVHkQb2Q4uCxFV8HB41H0ZCCNbXAhEWXyXUYlJpAHnTFGRTbGRxXVSp9l0aOAQ';
    var pin;
    var pin_location;
    let count = 0;
    $.ajax({
        url: 'https://' + new URL(window.location.host) + '/Payments/GetCitiesList',
        type: 'GET',
        success: function (data) {
            $.each(data, function (index) {
                setTimeout(() => {
                    name = data[index];
                    console.log(name);

                    pin_location = getLatLon(name, bing_key);

                    pin = new Microsoft.Maps.Pushpin(pin_location);
                    map.entities.push(pin);
                }, index * 200); //timeout between each city calc
            });
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function getLatLon(query, bing_key) {
    var latlon;
    var mapObject;
    $.ajax({
        method: 'GET',
        url: `https://dev.virtualearth.net/REST/v1/Locations?q=${query}&key=${bing_key}`,
        async: false,
        success: function (data) {
            latlon = data.resourceSets[0].resources[0].point.coordinates;
            mapObject = new Microsoft.Maps.Location(latlon[0], latlon[1])
        },
        error: function (err) {
            console.log(err);
        }
    });
    return mapObject;
}


