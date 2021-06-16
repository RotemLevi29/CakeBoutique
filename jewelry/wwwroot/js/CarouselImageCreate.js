//main carousel
$('.CarouselSize').each(function (e) {
    $(this).style.display = "none";
    $(this).text = "32342";
   
})
SizesEdit();

var num = $("#imageNumber").val();
if (num >= 3) {
    $("#creationArea").hide();
    $("#editImages").show();
}
else {
    $("#creationArea").show();
    $("#editImages").hide();
}

//if there are more that 3 images the admin need to delete
$(document).ready(function () {
    SizesEdit();
    $("#checkbox").click(SizesEdit);
});


//functions
function SizesEdit() {
    if ($("#checkbox")[0].checked == true) {
        $('#CarouselSize').css('display', 'block');
    }
    else {
        $('#CarouselSize').css('display', 'none');
    }
}
