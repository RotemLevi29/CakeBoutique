/**
 *this function changing the input type while the user change the search type selector
 */

/**
 *This function will work after the submit button pressed
 * it will send the input and the input type into the search function
 * of the product controller
 * 
 */
$(function () {
    $('#submit-btn').click(function (e) {
        e.preventDefault(); //prevent to send the data into the server.
        var searchtype = $("#selectSearchType").val();

    $.ajax({
        url: "/Users/Search",
        data: { input: searchInput }
    }).done(function (data) {
        console.log(data);
        $('tbody').html(data);
    });
    });
})
/**
 * This function will call when the type is title
 * and the searching will be automatic with keyup
 */
$(function () {
    $('#searchFormTitle').keyup(function (e) {
        console.log("exe");
        value = $('#searchFormTitle').val();
        $.ajax({
            url: "/Users/Search",
            data: { input: value }
        }).done(function (data) {
            console.log(data);
            $('tbody').html(data);
        });
    });
})