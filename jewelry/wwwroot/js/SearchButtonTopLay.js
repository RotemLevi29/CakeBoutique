

$(function () {
    $('#searchIcon').click(function () {
        $.ajax({
            url: "/Products/SearchProductClientFrame",
            data: {}
        }).done(function (data) {
            console.log(data);
            $('#mainSectionClient').html(data);
            $('#searchIcon').fadeOut("slow");
        });
        $('#searchLoader').fadeIn();
        $('#searchLoader').fadeOut();
        $("#searchLoader").delay(300).fadeOut("slow");
        $.ajax({
            url: "/Products/SearchProductsClient",
            data: { input: "Popular Porducts", category: 0, maxprice: 0 }
        }).done(function (data) {
            console.log(data);
            $('#displaySearchDiv').html(data);
            $('#searchLoader').fadeIn();
            $('#searchLoader').fadeOut();
            $("#searchLoader").delay(300).fadeOut("slow");
        });
    });

});
