
//web service for gold price
var settings = {
    "url": "https://www.goldapi.io/api/XAU/USD",
    "method": "GET",
    "timeout": 0,
    "headers": {
        "x-access-token": "goldapi-dl2qpukqpk4jic-io",
        "Content-Type": "application/json"
    },
};

$.ajax(settings).done(function (response) {
    $('#goldprice').text("Gold price: " + response.open_price + "$");
});

var settings2 = {
    "url": "https://www.goldapi.io/api/XAG/USD",
    "method": "GET",
    "timeout": 0,
    "headers": {
        "x-access-token": "goldapi-dl2qpukqpk4jic-io",
        "Content-Type": "application/json"
    },
};  

$.ajax(settings2).done(function (response) {
    $('#silverprice').text("Silver price: " + response.open_price + "$");
});