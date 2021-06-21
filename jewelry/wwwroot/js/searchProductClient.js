
$(function () {
    $('#searchButton').click(function (e) {
        e.preventDefault(); //prevent to send the data into the server.
        var inputToSearch = $("#inputToSearch").val();
        var categoryInput = $('#categoryInput').val();
        var maxprice = $('#maxprice').val();

    $.ajax({
        url: "/Products/SearchProductsClient",
        data: { input: inputToSearch, category: categoryInput, maxprice: maxprice  }
    }).done(function (data) {
        console.log(data);
        $('#displaySearchDiv').html(data);
    });
    });
})
/**
 * This function will call when the type is title
 * and the searching will be automatic with keyup
 */
/*$(function () {
    $('#searchFormTitle').keyup(function (e) {
        console.log("exe");
        value = $('#searchFormTitle').val();
        $.ajax({
            url: "/Products/SearchSatff",
            data: { input: value, type: '0' }
        }).done(function (data) {
            console.log(data);
            $('#prodctIndexSearch').html(data);
        });
    });
})*/