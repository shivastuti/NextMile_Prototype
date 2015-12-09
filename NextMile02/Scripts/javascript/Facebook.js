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
                                      
                                      
                                    }
                                });
                            }
                        }
                    });
                }
            });
        }
    };
    $(".jumper").on("click", function (e) {

        e.preventDefault();

        $("body, html").animate({
            scrollTop: $($(this).attr('href')).offset().top
        }, 600);

    });
    (function () {
        var infoModal = $('#preferences');
        $('#preferences').on('click', function () {

         /*   $.ajax({
                type: "GET",
                url: '/home/GetPreferencesForUser/' + $(this).data(),
                dataType: 'json',
                
            });*/
            $("#result").text("hi from the javascript")
        });
        return false;
    })(jQuery);

    $(function()
    {
        $.ajax(
                               {
                                   url: "/home/GetPreferencesForUser",
                                   type: "POST",
                                   data: {},
                                   contentType: "application/json; charset=utf-8",
                                   dataType: "json",
                                   success: function (res) {
                                       displaypreferences(res);
                                }
                               });
    });    
    function displaypreferences(data)
    {
        console.log(data);
        var truckList = data.PreferenceData;
        console.log(truckList);
        $('#result').empty();
        $.each(truckList, function (index, truckPreferenceData) {
            console.log(truckPreferenceData);
            if(truckPreferenceData.preference == 1)
                $('#result').append($('<ul>').append( truckPreferenceData.truckname + '</p>'));
            else  if(truckPreferenceData.preference == 2)
                $('#result1').append($('<ul>').append( truckPreferenceData.truckname + '</p>'));


        });
    }
    
      
    var amountScrolled = 300;

    $(window).scroll(function () {
        if ($(window).scrollTop() > amountScrolled) {
            $('a.back-to-top').fadeIn('slow');
        } else {
            $('a.back-to-top').fadeOut('slow');
        }
    });

    $('a.back-to-top').click(function () {
        $('html, body').animate({
            scrollTop: 0
        }, 700);
        return false;
    });

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