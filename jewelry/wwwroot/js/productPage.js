

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
});
