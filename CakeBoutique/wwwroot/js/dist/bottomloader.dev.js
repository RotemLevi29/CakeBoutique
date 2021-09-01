"use strict";

$("#bottom").hide();
$(window).on('load', function () {
  /*------------------
  	Preloder
  --------------------*/
  $("#bottom").fadeIn();
  $("#bottom").delay(400).fadeIn("slow");
});