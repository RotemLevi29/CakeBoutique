"use strict";

//run main
//choosing images from down below
function imageToMain(path) {
  $('#mainImage').attr('src', path);
}

$(document).ready(function () {
  var outofstock = $('#outofstock').val(); //=0 means that out of stock

  if (outofstock == 0) {
    $('#addtocart').prop('disabled', true);
    $('#nameOnProduct').prop('disabled', true);
  }
}); //hide this properties first

$('#shoppingcart').hide();
$('#error').hide();
$('#AddedSuccessfully').hide(); //on keyup(changing the input), remove the aproved adding to cart signs

$('#quantity').keyup(function () {
  $("#shoppingcart").delay(400).fadeOut("slow");
  $('#AddedSuccessfully').fadeOut("slow");
  $('#enteraName').removeClass('alert-danger');
});
$('#nameOnProduct').keyup(function () {
  $("#shoppingcart").delay(400).fadeOut("slow");
  $('#AddedSuccessfully').fadeOut("slow");
  $('#enteraName').removeClass('alert-danger');
});
$('#addtocart').click(function (event) {
  event.preventDefault();

  if ($('#nameOnProduct')) {
    if ($('#nameOnProduct').val() == "") {
      $('#enteraName').addClass('alert-danger');

      for (var i = 0; i < 3; i++) {
        $('#enteraName').fadeOut(500);
        $('#enteraName').fadeIn(500);
      }

      return false;
    } else {
      $('#enteraName').removeClass('alert-danger');
    }
  }
  /*data from html*/


  var jsproductId = $('#productId').val();
  var jsurl = $('#url').val();
  var jsproductName = $('#productName').val();
  var jsinput = $('#nameOnProduct').val();
  var jquantity = $('#quantity').val();

  if (jquantity <= 0) {
    $('#error').text("please enter positive quantity");
    $('#error').fadeIn();
    return;
  } //calling the addtoccart function, recieving boolean


  $.ajax({
    url: '/ProductCarts/AddToCart',
    type: 'POST',
    data: {
      productId: jsproductId,
      productName: jsproductName,
      input: jsinput,
      url: jsurl,
      quantity: jquantity
    },

    /*if the data return means success*/
    success: function success(callback) {
      if (callback == "NotLogin") {
        location.href = '/Users/Login?ReturnUrl=' + jsurl;
      } else if (callback == "Success") {
        $('#shoppingcart').hide();
        $('#AddedSuccessfully').hide();
        /*something coll for adding the product*/

        $('#error').hide();
        $('#shoppingcart').fadeIn("slow");
        $('#AddedSuccessfully').fadeIn("slow");
        $('#AddedSuccessfully').fadeOut("slow");
      } else if (callback == "Error") {
        $('#error').fadeIn();
        $('#addtocart').prop('disabled', true);
        $('#nameOnProduct').prop('disabled', true);
      } else if (callback == "toomany") {
        $('#error').text("can't add this amount to your cart");
        $('#error').fadeIn();
      }
    },
    error: function error(callback) {
      $('#error').fadeIn();
      $('#addtocart').prop('disabled', true);
      $('#nameOnProduct').prop('disabled', true);
    }
  });
});