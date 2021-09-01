"use strict";

//runnig on the first time click search icon, bringing the frame+first 10 products
//this is hapeninig befor the user input text
var pathname = window.location.pathname;

if (pathname.includes("Login") || pathname.includes("Register")) {
  $('#searchIcon').hide();
}

$(function () {
  $('#searchIcon').click(function () {
    $.ajax({
      url: "/Products/SearchProductClientFrame",
      data: {}
    }).done(function (data) {
      $('#mainSectionClient').html(data);
      $('#searchIcon').fadeOut("slow");
    });
    $('#searchLoader').fadeIn();
    $('#searchLoader').fadeOut();
    $("#searchLoader").delay(300).fadeOut("slow");
    $.ajax({
      url: "/Products/SearchProductsClient",
      data: {
        input: "Popular Porducts",
        category: 0,
        maxprice: 0
      }
    }).done(function (data) {
      $('#displaySearchDiv').html(data);
      $('#displaySearchName').hide();
      $('#opsNotFound').hide();
      $('#searchLoader').fadeIn();
      $('#searchLoader').fadeOut();
      $("#searchLoader").delay(300).fadeOut("slow");
    });
  });
});