

var updateCart = function () {
    var cartId = $('#cartId').val();
    $.ajax({
        url: "/Carts/MyCart",
        data: { id: cartId }
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
    $("#quantityError").text('');
    if (newquantitiy == 0 || newquantitiy == null) {
        newquantitiy = $('#input\\.' + productid).val();
    }
    $.ajax({
        url: "/ProductCarts/changeQuantity",
        data: { id: productid, quantity: newquantitiy }
    }).done(function (data) {
        if (data == "quantityError") {
            $("#quantityError").text("can't add this quantity");
        }
        else if (data == "zeroQuantity") {
            updateCart();
        }
        else {
            var arr = data.split(',');
            //first is product price, second is total cart the thirs is the current
            //product total price the forth is quantity updated
            var price = arr[0];
            var totalCart = arr[1];
            var producttotalprice = arr[2];
            var updatedQuantity = arr[3];
            $('#totalprice').text(totalCart + '$');
            $('#totalproductprice\\.' + productid).text(producttotalprice + '$');
            $('#price\\.' + productid).text(price + '$');
            $('#input\\.' + productid).val(parseInt(updatedQuantity));
            $('#minus\\.' + productid).attr('disabled', false);
            $('#plus\\.' + productid).attr('disabled', false);
/*            $('#price\\.' + productid).text(price);*/
        }
    })
}
/*************************************/


function removeFromCart(productid) {
    $('#input.' + productid).text("55");
    $.ajax({
        url: "/ProductCarts/RemoveFromCart",
        data: { id: productid }
    }).done(function (data) {
        
/*        updateCart();
*/    });
}

function AddFromCart(productid) {
    $.ajax({
        url: "/ProductCarts/AddFromCart",
        data: { id: productid }
    }).done(function (data) {
        updateCart();
    });
}

function checkout(totalproduct, idcart) {
    console.log($('#totalprice').text());

    var total = $('#totalprice').text();
    total = total.replace('$', ' ');
    console.log(total);
    $.ajax({
        url: "/Orders/OrderForm",
        type: 'GET',
        data: { total: totalproduct, cartid: idcart, currenttotalprice: total }
    }).done(function (data) {
        $('#exampleModalScrollable').html(data);
    });
};

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

