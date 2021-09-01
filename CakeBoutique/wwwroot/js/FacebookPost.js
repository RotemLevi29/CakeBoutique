$('#successPost').hide();
window.fbAsyncInit = function () {
    FB.init({
        appId: '109642161359606',
        autoLogAppEvents: true,
        xfbml: true,
        version: 'v11.0'
    });
};

//facebook post to CakeBotique page 

$('#FacebookPost').click(function () {
    var input = $('#poststring').val();
    if (input != "") {
        FB.api(
            '/109642161359606/feed',
            'POST',
            {
                "message": input,
                "access_token": "EAAMskAqVPusBAHUpbcmPuGIoINpmZA4J4paiKZCQ78jbKKhbQpzb3ZArm9oIb9TgotNNyNavRgEI2qroJqZBeqZCNHGXHaMrooHVKkCbnICHnkQErLjhCx8foX9GiVMOZA2NSbZAEmWHZBHPresO4PoydcBnJXFSvCsMDf4JdmDBwErmlct8tDm3"
            },
            function (response) {
                /*respone.error=="" =>no error = success*/
               /* console.log("code: " + response.error == undefined);
                console.log("subcode: " + response.error_subcode)
                console.log(response);*/
                if (response.error == undefined) {
                    $('#successPost').fadeIn('slow');
                }
            }
        )
    }
});