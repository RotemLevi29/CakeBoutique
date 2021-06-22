


$(function () {
    var flag = false;
    $('#searchButton').mouseup(function () {
        console.log("mouseup");
        flag = true;
    });

    $('#searchButton').mousedown(function (e) {
        console.log("key down");
        if (flag) {
            flag = false;

            $('#searchButton').attr('disabled', 'true');
            $('#displaySearchDiv').html("");
            $('#searchLoader').fadeIn();
            $('#searchLoader').fadeOut();
            $("#searchLoader").delay(300).fadeOut("slow");

            console.log("sasasa");
            e.preventDefault(); //prevent to send the data into the server.
            var inputToSearch = $("#inputToSearch").val();
            var categoryInput = $('#categoryInput').val();
            var maxprice = $('#maxprice').val();

            $.ajax({
                url: "/Products/SearchProductsClient",
                data: { input: inputToSearch, category: categoryInput, maxprice: maxprice }
            }).done(function (data) {
                console.log(data);
                $('#displaySearchDiv').html(data);
            });
        }
        flag = false;
    });
        
});