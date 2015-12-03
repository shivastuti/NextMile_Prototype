/*This javascript will ensure that we're subscribed to login
event on which we'll submit fb access token to our controller 
and save it in session. Also, on each window load, we'll check
for fb login status and alert user accordingly.
*/

function InitialiseFacebook(appId)
{
    window.fbAsyncInit = function ()
    {
        FB.init(
        {
            appId : appId,
            status: true,
            cookie: true,
            xfbml : true
        });

        FB.Event.subscribe('auth.login', function(res)
        {
            var credentials =
            {
                uid        : res.authResponse.userID,
                accessToken: res.authResponse.accessToken,
                flag       : "-1"
            };
            SubmitLogin(credentials);
        });

        FB.Event.subscribe('auth.logout', function (res)
        {
            $.ajax(
            {
                url: "/account/logout",
                type: "POST",
                success: function (res)
                {
                    window.location.reload();
                    console.log("The user has been signed out.");
                }
            });
            
        });

        FB.getLoginStatus(function (res) {
            if (res.status === 'connected') {
                console.log("The user is signed into Facebook.");
            }
            else if (res.status === 'not_authorized') {
                console.log("The user is not authorised.");
            }
            else {
                console.log("The user is not connected to Facebook.");
            }
        });

        function SubmitLogin(credentials)
        {
            $.ajax(
            {
                url  : "/account/login",
                type : "POST",
                data : credentials,
                error: function ()
                {
                    alert("There was an error signing in to your Facebook account.");
                },
                success: function ()
                {
                    $.ajax(
                    {
                        url    : "/account/getflag",
                        type   : "GET",
                        success: function (res)
                        {
                            if (res == "-1")
                            {
                                $.ajax(
                                {
                                    url: "/account/setflag",
                                    type: "POST",
                                    success: function (res)
                                    {
                                        window.location.reload();
                                      ('#Profile').load('/Account/UserDetails');
                                      
                                    }
                                });
                            }
                        }
                    });
                }
            });
        }
    };
    
    (function (d)
    {
        var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
        if (d.getElementById(id))
        {
            return;
        }
        js = d.createElement('script');
        js.id = id;
        js.async = true;
        js.src = "//connect.facebook.net/en_US/all.js";
        ref.parentNode.insertBefore(js, ref);
    }(document));
}
/* End of Facebook.js */