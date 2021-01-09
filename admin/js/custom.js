/*=========================================================================
        Password Show/Hide for Login, SignUp and Change Password Page
===========================================================================*/

$(function () {
    // For Login Page
    $("#eye-password").click(function () {
        $(this).toggleClass("fa-eye fa-eye-slash");
        var input = $("#password");

        if (input.attr("type") == "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password")
        }
    });

    // For SignUp Page 

    $("#eye-create-password").click(function () {
        $(this).toggleClass("fa-eye fa-eye-slash");
        var input = $("#create-password");

        if (input.attr("type") == "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password")
        }
    });
    $("#eye-confirm-create-password").click(function () {
        $(this).toggleClass("fa-eye fa-eye-slash");
        var input = $("#confirm-create-password");

        if (input.attr("type") == "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password")
        }
    });

    // For Change Password Page

    $("#eye-old-password").click(function () {
        $(this).toggleClass("fa-eye fa-eye-slash");
        var input = $("#old-password");

        if (input.attr("type") == "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password")
        }
    });
    $("#eye-new-password").click(function () {
        $(this).toggleClass("fa-eye fa-eye-slash");
        var input = $("#new-password");

        if (input.attr("type") == "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password")
        }
    });
    $("#eye-confirm-changed-password").click(function () {
        $(this).toggleClass("fa-eye fa-eye-slash");
        var input = $("#confirm-changed-password");

        if (input.attr("type") == "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password")
        }

    });

});
/*=========================================
                Navigation Bar
============================================*/
// Show And Hide White Navigation Bar
$(function() {

    // Show / Hide Nav On Page Load
    showHideNav();

    $(window).scroll(function() {
        // Show / Hide Nav On Windows Scroll
        showHideNav();
    });

    function showHideNav() {
        if( $(window).scrollTop() > 50 ) {

            // Show White Nav
            $("nav#home-page-nav").addClass("white-navbar");

            // Change Logo
            $("#home-page-nav .navbar-header img").attr("src", "images/Homepage/logo.png");

            // Change color of Mobile Menu Icon
            $("#home-page-header #mobile-nav-open-btn").css("color", "#6255a5");
        } else {
            // Hide White Nav
            $("nav#home-page-nav").removeClass("white-navbar");

            // Change Logo
            $("#home-page-nav .navbar-header img").attr("src", "images/login/top-logo.png");

            // Change color of Mobile Menu Icon
            $("#home-page-header #mobile-nav-open-btn").css("color", "#fff");

        }
    }

});
// For Popup Modal
$(function() {
    $('#myModal').on('shown.bs.modal', function () {
         $('#myInput').trigger('focus')
    });
});

// For FAQ Section
$(function() {

    // for Q1
    $('#collapseOne').on('shown.bs.collapse', function () {
        // do something...
        $("#card1").css({"border": "1px solid #d1d1d1", "border-radius": "3px"});
        $("#headingOne").css({"background-color": "#fff"});
        $("#card1 img").attr("src", "images/FAQ/minus.png");
    });
    $('#collapseOne').on('hidden.bs.collapse', function () {
        // do something...
        $("#headingOne").css("background-color", "#f3f3f3");
        $("#card1 img").attr("src", "images/FAQ/add.png");
        $("#card1").css("border", "none");
    });


    // FOr Q2
    $('#collapseTwo').on('shown.bs.collapse', function () {
        // do something...
        $("#card2").css("border", "1px solid #d1d1d1");
        $("#headingTwo").css({"background-color": "#fff"});
        $("#card2 img").attr("src", "images/FAQ/minus.png");
    });
    $('#collapseTwo').on('hidden.bs.collapse', function () {
        // do something...
        $("#headingTwo").css("background-color", "#f3f3f3");
        $("#card2 img").attr("src", "images/FAQ/add.png");
        $("#card2").css("border", "none");
    });

    // For Q3
    $('#collapseThree').on('shown.bs.collapse', function () {
        // do something...
        $("#card3").css("border", "1px solid #d1d1d1");
        $("#headingThree").css({"background-color": "#fff"});
        $("#card3 img").attr("src", "images/FAQ/minus.png");
    });
    $('#collapseThree').on('hidden.bs.collapse', function () {
        // do something...
        $("#headingThree").css("background-color", "#f3f3f3");
        $("#card3 img").attr("src", "images/FAQ/add.png");
        $("#card3").css("border", "none");
    });

    // For Q4
    $('#collapseFour').on('shown.bs.collapse', function () {
        // do something...
        $("#card4").css("border", "1px solid #d1d1d1");
        $("#headingFour").css({"background-color": "#fff"});
        $("#card4 img").attr("src", "images/FAQ/minus.png");
    });
    $('#collapseFour').on('hidden.bs.collapse', function () {
        // do something...
        $("#headingFour").css("background-color", "#f3f3f3");
        $("#card4 img").attr("src", "images/FAQ/add.png");
        $("#card4").css("border", "none");
    });

    // FOr Q5
    $('#collapseFive').on('shown.bs.collapse', function () {
        // do something...
        $("#card5").css("border", "1px solid #d1d1d1");
        $("#headingFive").css({"background-color": "#fff"});
        $("#card5 img").attr("src", "images/FAQ/minus.png");
    });
    $('#collapseFive').on('hidden.bs.collapse', function () {
        // do something...
        $("#headingFive").css("background-color", "#f3f3f3");
        $("#card5 img").attr("src", "images/FAQ/add.png");
        $("#card5").css("border", "none");
    });

    // FOr Q6
    $('#collapseSix').on('shown.bs.collapse', function () {
        // do something...
        $("#card6").css("border", "1px solid #d1d1d1");
        $("#headingSix").css({"background-color": "#fff"});
        $("#card6 img").attr("src", "images/FAQ/minus.png");
    });
    $('#collapseSix').on('hidden.bs.collapse', function () {
        // do something...
        $("#headingSix").css("background-color", "#f3f3f3");
        $("#card6 img").attr("src", "images/FAQ/add.png");
        $("#card6").css("border", "none");
    });

     // FOr Q7
     $('#collapseSeven').on('shown.bs.collapse', function () {
        // do something...
        $("#card7").css("border", "1px solid #d1d1d1");
        $("#headingSeven").css({"background-color": "#fff"});
        $("#card7 img").attr("src", "images/FAQ/minus.png");
    });
    $('#collapseSeven').on('hidden.bs.collapse', function () {
        // do something...
        $("#headingSeven").css("background-color", "#f3f3f3");
        $("#card7 img").attr("src", "images/FAQ/add.png");
        $("#card7").css("border", "none");
    });

});

/* ================================== 
                Mobile Menu
===================================== */
$(function () {

    // Show Mobile Navigation
    $('#mobile-nav-open-btn').click(function () {
        $("#mobile-nav").css("height", "100%");
    });
    // Hide Mobile Navigation
    $("#mobile-nav-close-btn, #mobile-nav a").click(function () {
        $("#mobile-nav").css("height", "0%");
    });
}); 


// Ratings
$(function(){
	
	$('.rating__button').on('click', function(e){
		var $t = $(this), // the clicked star
				$ct = $t.parent(); // the stars container
		
		// add .is--active to the user selected star 
		$t.siblings().removeClass('is--active').end().toggleClass('is--active');
		// add .has--rating to the rating container, if there's a star selected. remove it if there's no star selected.
		$ct.find('.rating__button.is--active').length ? $ct.addClass('has--rating') : $ct.removeClass('has--rating');
	});
	
});