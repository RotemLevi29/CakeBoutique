$("#bottom").hide();
console.log("print something");
//$(document).ready(function () {
//    $("#bottom").show();
//});

$(window).on('load', function () {
	/*------------------
		Preloder
	--------------------*/
	$("#bottom").fadeIn();
	$("#bottom").delay(400).fadeIn("slow");

});
