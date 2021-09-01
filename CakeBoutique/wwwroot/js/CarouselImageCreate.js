//main carousel


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



