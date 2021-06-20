

var updateCart = function () {
    var cartId = $('#cartId').val();

    $.ajax({
        url: "/Carts/MyCart",
        data: { id: cartId }
    }).done(function (data) {
        console.log(data);
        $('#exampleModalScrollable').html(data);
    });
};

function removeFromCart(productid) {
    $.ajax({
        url: "/ProductCarts/RemoveFromCart",
        data: { id: productid }
    }).done(function (data) {
        updateCart();
    });
};

function checkout(totalproduct, idcart) {
    $.ajax({
        url: "/Orders/OrderForm",
        type: 'GET',
        data: { total: totalproduct, cartid: idcart }
    }).done(function (data) {
        console.log(data);
        $('#exampleModalScrollable').html(data);
    });
}

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

