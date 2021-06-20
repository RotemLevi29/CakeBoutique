$('#successPost').hide();
window.fbAsyncInit = function () {
    FB.init({
        appId: '109642161359606',
        autoLogAppEvents: true,
        xfbml: true,
        version: 'v11.0'
    });
};

//facebook post to jewelry page 

$('#FacebookPost').click(function () {
    console.log("facebook post");
    var input = $('#poststring').val();
    if (input != "") {
        FB.api(
            '/109642161359606/feed',
            'POST',
            {
                "message": input,
                "access_token": "EAAMskAqVPusBALeCKZAp7ntQ4QJfUOORSm3ODgxzdTFMBZAYXmT8H0swqlxaarzbhKLlL3IYqNgG1jdYjhDZCo8CvmubdDjHwZBpiVnkPFobiQYRxUHbcJIx6ahpNTx92D4RJL1RK3t2V97Malef5aTCLvY9SGOrGElYGI1eAol2laMJFaMZA"
            },
            function (response) {
                /*respone.error=="" =>no error = success*/
               /* console.log("code: " + response.error == undefined);
                console.log("subcode: " + response.error_subcode)
                console.log(response);*/

                if (response.error == undefined) {
                    console.log("suceess");
                    $('#successPost').fadeIn('slow');
                }
            }
        )
    }
});