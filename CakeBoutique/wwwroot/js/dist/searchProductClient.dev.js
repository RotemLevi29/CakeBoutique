"use strict";

$(function () {
  var flag = false;
  $('#searchButton').mouseup(function () {
    flag = true;
  });
  $('#searchButton').mousedown(function (e) {
    if (flag) {
      flag = false;
      $('#searchButton').attr('disabled', 'true');
      $('#displaySearchDiv').html("");
      $('#searchLoader').fadeIn();
      $('#searchLoader').fadeOut();
      $("#searchLoader").delay(300).fadeOut("slow");
      e.preventDefault(); //prevent to send the data into the server.

      var inputToSearch = $("#inputToSearch").val();
      var categoryInput = $('#categoryInput').val();
      var maxprice = $('#maxprice').val();
      $.ajax({
        url: "/Products/SearchProductsClient",
        data: {
          input: inputToSearch,
          category: categoryInput,
          maxprice: maxprice
        }
      }).done(function (data) {
        $('#displaySearchDiv').html(data);
      });
    }

    flag = false;
  });
});