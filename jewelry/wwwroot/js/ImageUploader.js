//start
$('#submitbtn').prop('disabled', true);
$('#preview2').hide();
$('#preview3').hide();



//maximum 3 images for each product
$("#inputGroupFile01").change(function (event) {
    if ($("#inputGroupFile01")[0].files.length > 3) {
        $('#errormessage').text('You can select only 3 images');
        $('#submitbtn').prop('disabled', true);
        $("#inputGroupFile01").input.val() = "";
    } else {
        RecurFadeIn();
        readURL(this);
    }
});

 

var load1 = function (e) {
    //first picture
    $('#preview').attr('src', e.target.result);
    $('#preview').show();
    $('#preview').fadeIn(500);
    //$('.custom-file-label').text(filename);
    $('#submitbtn').prop('disabled', false);
}
var load2 = function (e) {
    //first picture
    $('#preview2').attr('src', e.target.result);
    $('#preview2').show();
    $('#preview2').fadeIn(500);
    //$('.custom-file-label').text(filename);
    //second image
    $('#submitbtn').prop('disabled', false);
}
var load3 = function (e) {
    //first picture
    $('#preview3').attr('src', e.target.result);
    $('#preview3').show();
    $('#preview3').fadeIn(500);
    //$('.custom-file-label').text(filename);
    //second image
    $('#submitbtn').prop('disabled', false);
}
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader1 = new FileReader();
        var reader2 = new FileReader();
        var reader3 = new FileReader();
        reader1.onload = load1;
        reader2.onload = load2;
        reader3.onload = load3;

        var filename = $("#inputGroupFile01").val();
        filename = filename.substring(filename.lastIndexOf('\\') + 1);
        console.log("file name is:" + filename);
        if (input.files.length == 3) {
            reader1.readAsDataURL(input.files[0]);
            reader2.readAsDataURL(input.files[1]);
            reader3.readAsDataURL(input.files[2]);
        }
        if (input.files.length == 2) {
            reader1.readAsDataURL(input.files[0]);
            reader2.readAsDataURL(input.files[1]);
        }
        if (input.files.length == 1) {
            reader1.readAsDataURL(input.files[0]);
        }

       
    }
    $(".alert").removeClass("loading").hide();
}
function RecurFadeIn() {
    FadeInAlert("Wait for it...");
}
function FadeInAlert(text) {
    $(".alert").show();
    $(".alert").text(text).addClass("loading");
}