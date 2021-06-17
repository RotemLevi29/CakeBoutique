//run main
//hide this properties first
$('#shoppingcart').hide();
$('#error').hide();
$('#AddedSuccessfully').hide();
//disable input if there is text-input
var loop;

$("form#addForm").submit(function (event) {
    console.log($('#nameOnProduct').val());

    if ($('#nameOnProduct')) {
        if ($('#nameOnProduct').val() == "") {
            console.log("empty");
            $('#enteraName').addClass('alert-danger');
            function blink_text() {
                $('#enteraName').fadeOut(500);
                $('#enteraName').fadeIn(500);
            }
            loop = setInterval(blink_text, 100);
            $('#nameOnProduct').keyup(function () {
                console.log(loop);
                clearInterval(loop);

            });
            return false;

        }
    }
    /*data from html*/
    var jsproductId = $('#productId').val();
    var jsurl = $('#url').val();
    var jsproductName = $('#productName').val();
    var jscartId = $('#cartId').val();
    var jsinput = $('#nameOnProduct').val();
    console.log('/Users/Login?ReturnUrl=' + jsurl);
    $.ajax({
        url: '/ProductCarts/AddToCart',
        type: 'POST',
        data: {
            productId: jsproductId,
            productName: jsproductName,
            cartId: jscartId,
            input: jsinput,
            url: jsurl,
        },
        /*if the data return means success*/
        success: function (callback) {
            console.log("callback is:" + callback);
            if (callback == false) {
                console.log("false");
                setTimeout(check, 2000);
                check();
                location.href = '/Users/Login?ReturnUrl=' + jsurl;
                

            }
            else {
            /*something coll for adding the product*/
                console.log("true");
                $('#shoppingcart').fadeIn("slow");
                $("#shoppingcart").delay(400).fadeOut("slow");
                $('#AddedSuccessfully').fadein("slow");
                setTimeout(check, 2000);
                check();

            }
        },
        error: function (callback) {
            $('#error').fadeIn();
        },

    });
});

