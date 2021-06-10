//main carousel
$('.CarouselSize').each(function (e) {
    $(this).style.display = "none";
    $(this).text = "32342";
   
    console.log("im here");
})
SizesEdit();




$(document).ready(function () {
    SizesEdit();
    $("#checkbox").click(SizesEdit);
});


//functions
function SizesEdit() {
    console.log("size edit func");
    if ($("#checkbox")[0].checked == true) {
        $('#CarouselSize').css('display', 'block');
    }
    else {
        $('#CarouselSize').css('display', 'none');
    }
}
