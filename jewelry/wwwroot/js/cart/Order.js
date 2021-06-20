console.log("this is order");

function makeorder1() {
    var phone = $('#PhoneNumber').val();
    var state = $('#state').val();
    var city = $('#city').val();
    var street = $('#street').val();
    var housenumber = $('#housenumber').val();
    var apartmentnumber = $('#apartmentnumber').val();
    var postal = $('#postal').val();
    var payment = $('#payment').val();

    if (PhoneNumber == "" || state == "" || city == "" || street == "" || housenumber == "" ||
        apartmentnumber == "" || postal == "" || payment == "") {
        //please fill all the fields
    }
    else {
        $.ajax({
            url: "/Orders/OrderForm",
            type:'POST',
            data: {
                Payment: payment,
                PhoneNumber: phone,
                State: state,
                City: city,
                Street: street,
                HouseNumber: housenumber,
                ApartmentNumber: apartmentnumber,
                PostalCode: postal
                            }
        }).done(function (data) {
            console.log(data);
            $('#exampleModalScrollable').html(data);
        });
      
    };
}

$('#makeorder').click(makeorder1());