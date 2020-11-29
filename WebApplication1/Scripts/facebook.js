/// <reference path="jquery-1.7.1.min.js" />

(function () {
    var accessToken = '';

    $.fn.facebookLogin = function () {
        var url = this.attr('href');

        this.click(function (e) {
            e.preventDefault();

            FB.login(function (response) {
                if (response.status === "connected") {
                    accessToken = response.authResponse.accessToken;
                    getUserData(url);
                }
            }, { scope: 'public_profile, email' });
        });

     };
    $.fn.facebooklogout = function () {
        var url = this.attr('href');

        this.click(function (e) {
            e.preventDefault();

            FB.getLoginStatus(function(response) {
                if (response.status === 'connected') {
                    FB.logout(function (response) {
                        window.location = url;
                    });
                }
            });
        });

     };

       function getUserData(url) {
            FB.api('/me', 'get', { fields: 'id, first_name, name, picture, email' }, function (response) {
                var data = {
                    id: response.id,
                    first_name: response.first_name,
                    name: response.name,
                    picture: response.picture.url,
                    email: response.email
                };

                $.sendRequest(url, data, function (response) {
                    window.location = '/';
                });
            });
        }

    window.fbAsyncInit = function () {
        FB.init({
            appId: '242548113025704',
            cookie: true,
            xfbml: true,
            version: 'v3.0'
        });

        FB.AppEvents.logPageView();
        $(document).ready(function () {
            if ($('.accountlogs ul').length == 0) {
                FB.getLoginStatus(function (response) {
                    if (response.status === "connected") {
                        getUserData('/account/facebooklogin');
                    }
                });
            }
        });
    };

    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) { return; }
        js = d.createElement(s); js.id = id;
        js.src = "https://connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

})();
