"use strict";

var updateCart = function updateCart() {
  var cartId = $('#cartId').val();
  $.ajax({
    url: "/Carts/MyCart",
    data: {
      id: cartId
    }
  }).done(function (data) {
    $('#exampleModalScrollable').html(data);
  });
};
/*$('.minus').on('click', function (event) {
    event.preventDefault(); 
    event.stopImmediatePropagation();
    var arr = (event.target.id).split('.');
    var productid = arr[1];
    console.log(productid);
    var oldquan = $('#input\\.' + productid);
    var newquan = parseInt(oldquan.val()) - 1;
    oldquan.val(newquan);
    changeQuantity(productid, newquan);
}
);*/

/**************************************/


function minus(productid) {
  $('#minus\\.' + productid).attr('disabled', true);
  $('#plus\\.' + productid).attr('disabled', true);
  var oldquan = $('#input\\.' + productid);
  var newquan = parseInt(oldquan.val()) - 1;
  oldquan.val(newquan);
  changeQuantity(productid, newquan);
}

function plus(productid) {
  $('#minus\\.' + productid).attr('disabled', true);
  $('#plus\\.' + productid).attr('disabled', true);
  var oldquan = $('#input\\.' + productid);
  var newquan = parseInt(oldquan.val()) + 1;
  oldquan.val(newquan);
  changeQuantity(productid, newquan);
}

function changeQuantity(productid, newquantitiy) {
  $('#input\\.' + productid).attr('disabled', true);
  $("#quantityError").text('');

  if (newquantitiy == 0 || newquantitiy == null) {
    newquantitiy = $('#input\\.' + productid).val();
  }

  if (newquantitiy > 99999 || newquantitiy < 0) {
    $("#quantityError").text("can't add this quantity");
    $('#input\\.' + productid).attr('disabled', false);
    return;
  }

  $.ajax({
    url: "/ProductCarts/changeQuantity",
    data: {
      id: productid,
      quantity: newquantitiy
    }
  }).done(function (data) {
    if (data == "quantityError") {
      $("#quantityError").text("can't add this quantity");
    } else if (data == "zeroQuantity") {
      updateCart();
    } else {
      var arr = data.split(','); //first is product price, second is total cart the thirs is the current
      //product total price the forth is quantity updated

      var price = arr[0];
      var totalCart = arr[1];
      var producttotalprice = arr[2];
      var updatedQuantity = arr[3];
      $('#totalprice').text(totalCart + '$');
      $('#totalproductprice\\.' + productid).text(producttotalprice + '$');
      $('#price\\.' + productid).text(price + '$');
      $('#input\\.' + productid).val(parseInt(updatedQuantity));
      /*            $('#price\\.' + productid).text(price);*/
    }

    $('#minus\\.' + productid).attr('disabled', false);
    $('#plus\\.' + productid).attr('disabled', false);
    $('#input\\.' + productid).attr('disabled', false);
  });
}
/*************************************/


function removeFromCart(productid) {
  $('#input.' + productid).text("55");
  $.ajax({
    url: "/ProductCarts/RemoveFromCart",
    data: {
      id: productid
    }
  }).done(function (data) {
    /*        updateCart();
    */
  });
}

function AddFromCart(productid) {
  $.ajax({
    url: "/ProductCarts/AddFromCart",
    data: {
      id: productid
    }
  }).done(function (data) {
    updateCart();
  });
}

function checkout(totalproduct, idcart) {
  var total = $('#totalprice').text();
  total = total.replace('$', ' ');
  $.ajax({
    url: "/Orders/OrderForm",
    type: 'GET',
    data: {
      total: totalproduct,
      cartid: idcart,
      currenttotalprice: total
    }
  }).done(function (data) {
    $('#exampleModalScrollable').html(data);
  });
}

;
/*function order(totalproduct, idcart) {
    $.ajax({
        url: "/Orders/OrderForm",
        type: 'GET',
        data: { total: totalproduct, cartid: idcart }
    }).done(function (data) {
        console.log(data);
        $('#exampleModalScrollable').html(data);
    });
}*/