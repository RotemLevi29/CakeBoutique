/**
 *this function changing the input type while the user change the search type selector
 */
//Search of admin/editor

function searchType() {
    var type = $("#selectSearchType").val();
    switch (type){
        case '0':
            {
                document.getElementById("searchFormTitlediv").style.display = "block";
                document.getElementById("searchFormPricediv").style.display = "none";
                document.getElementById("searchFormTypediv").style.display = "none";
                return;
            }
        case '1':
            {
                document.getElementById("searchFormTitlediv").style.display = "none";
                document.getElementById("searchFormPricediv").style.display = "block";
                document.getElementById("searchFormTypediv").style.display = "none";
                return;
            }
        case '2':
            {
                document.getElementById("searchFormTitlediv").style.display = "none";
                document.getElementById("searchFormPricediv").style.display = "none";
                document.getElementById("searchFormTypediv").style.display = "block";
                return;
            }
    }
}

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
        var searchInput = "";
        if (searchtype == 0) {
            searchInput = $('#searchFormTitle').val();
        }
        else if (searchtype == 1) {
            searchInput = $('#searchFormPrice').val();
        }
        else if (searchtype == 2) {
            searchInput = $('#searchFormType').val();
        }

    $.ajax({
        url: "/Products/SearchProductsStaff",
        data: { input: searchInput, type: searchtype }
    }).done(function (data) {
        $('#prodctIndexSearch').html(data);
    });
    });
})
/**
 * This function will call when the type is title
 * and the searching will be automatic with keyup
 */
$(function () {
    $('#searchFormTitle').keyup(function (e) {
        console.log("key");
        value = $('#searchFormTitle').val();
        console.log(value);
        $.ajax({
            url: "/Products/SearchProductsStaff",
            data: { input: value, type: '0' }
        }).done(function (data) {
            $('#prodctIndexSearch').html(data);
        });
    });
})