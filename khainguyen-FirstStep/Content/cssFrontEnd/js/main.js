/* javascript by Uy Min */
$(function () {    
    Core_ShowHTML();
});

$(document).ready(function () {
    "use strict";
    // DETECT IE
    if (detectIE() > 0) { $("head").append("<link rel='stylesheet' href='css/iehelp.css'>"); }
    
    // FUNC
	Scroll_Anchor();
	ScrollHTML();
	Menu_Fix();
	Core_AllPage();
	tab_panel();
	form_HTML();
});

function form_HTML() {
    var changecolor = function () {
        $('.form-checkbox-hidden').each(function (i, v) {
            var val = $(this).find('input').val();
            $i = $(this).find('i');
            if (val == "1") { $i.addClass('color-tiny'); } else { $i.removeClass('color-tiny'); }
        });
    };
    $('.form-checkbox-hidden i').click(function (e) {
        e.preventDefault();
        $input = $(this).parent().find('input');
        if ($input.val() == "1") { $input.val("0"); } else { $input.val("1"); }
        changecolor();
    });
    changecolor();
}

function tab_panel() {
    var data_tab = null;
    var index = 0;
    $tab_panel = null;
    $('.tab-nav a').click(function (e) {
        e.preventDefault();
        index = $(this).index();
        data_tab = $(this).parent().attr('data-tab');
        $(this).parent().find('a').removeClass('active');
        $(this).addClass('active');
        $tab_panel = $(".tab-panel[data-tab='" + data_tab + "']");
        $tab_panel.find('.tab').removeClass('active');
        $tab_panel.find('.tab').eq(index).addClass('active');
    });
}

function Core_AllPage() {

    // text fly
    $('.menu-login:not(.loged) a span').letterfx({ fx: "swirl", timing: 200, letter_end: "stay" });

    // tooltip
    $('.tooltip').tooltipster({ animation: 'grow', contentAsHTML: true });

    // waypoint
    $('div[class^="panel"] .container').css('opacity', '0').css('margin-top', 50);
    $('div[class^="panel"] .container').waypoint(function (direction) {
        $(this).animate({ marginTop: 0, opacity: 1 }, 1000);
    }, { offset: '100%', triggerOnce: true })

    // cb-filter: combobox filter dùng cho page: discover
    $('div[class^="cb-filter"]').each(function (i, v) {
        $a = $(this).find('nav a.active');
        if($a.length==0){$a = $(this).find('nav a').first();}
        $(this).find('button span').text($a.text());
    })
    var iscbfilter = false;
    $('div[class^="cb-filter"] button').click(function (e) {
        e.preventDefault();
        $btn = $(this);
        $nav = $(this).parent().find('nav');
        
        if ($('#form-filter').length > 0) { $nav.css('top', $btn.offset().top - $('#form-filter').offset().top + $(this).parent().height() + 5); }
        
        $('div[class^="cb-filter"] nav').hide('300', function () { });
        if ($btn.attr('data-clicked') == "1") { $btn.removeAttr('data-clicked'); }
        else {
            $nav.show('2000', "easeOutBack", function () {
                $('.cb-filter button').removeAttr('data-clicked');
                $btn.attr('data-clicked', '1');
            });
        }
    }).mouseover(function () { iscbfilter = true; }).mouseout(function () { iscbfilter = false; });
    $('div[class^="cb-filter"] nav').mouseover(function () { iscbfilter = true; }).mouseout(function () { iscbfilter = false; });
    $(window).click(function (e)
    {
        if (!iscbfilter) { $('div[class^="cb-filter"] nav').hide(300); $('div[class^="cb-filter"] button').removeAttr('data-clicked'); }
    });

    //show popup
    $('.proj-popup-btnclose').click(function (e) {
        e.preventDefault();
        $(this).parent().parent().fadeOut();
    });

    $('.show-popup-proj').click(function (e) {
        e.preventDefault();
        var dataid = $(this).attr('data-id');
        if (dataid.length < 1) { return; }
        $('.proj-popup[data-id=' + dataid + ']').fadeIn(300);
    });

    // map-google
    $('.map-google').click(function () {
        $('.map-google iframe').css("pointer-events", "auto");
    }).mouseleave(function () {
        $('.map-google iframe').css("pointer-events", "none");
    });
}

function Menu_Fix() {
    $('.btnMenu').click(function () {
        $('.menu-mobile').toggle();
        $('.menu-user').hide();
    });
    var reload = function () {
        if ($('.btnMenu').is(':visible')) {
            $('.menu-mobile').hide();
        }
        else {
            $('.menu-mobile').fadeIn(500);
        }
    }
    $(window).resize(function () {
        reload();
    });

    $('.btnUser').click(function (e) {
        e.preventDefault();
        $('.menu-user').toggle();
        if ($('.btnMenu').is(':visible')) { $('.menu-mobile').hide(); }
    });
}

function Core_ShowHTML() {
    // PLAY NOTI WHEN LOAD DONE!!!
    var isPlay =false;
    if (isPlay)
    {
        $('body').append("<audio id='audio-loading'><source src='upload/noti/noti1.ogg' type='audio/ogg'></audio>");
        var audioElement = document.getElementById('audio-loading');
        audioElement.volume = 0.5;
        audioElement.play();
        $('#audio-loading').bind('ended', function () { $(this).remove(); });
    }
    // SHOW WHEN DONE!!!
    $(window).load(function () {
        if ($('.loading-container').is(":visible"))
            $('.loading-container').delay(350).fadeOut("slow");
    });
    setTimeout(function () {
        if ($('.loading-container').is(":visible"))
            $('.loading-container').fadeOut("slow");
    }, 1*1000);
}

function detectIE() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf('MSIE ');
    var trident = ua.indexOf('Trident/');
    if (msie > 0) {
        // IE 10 or older => return version number
        return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
    }
    if (trident > 0) {
        // IE 11 (or newer) => return version number
        var rv = ua.indexOf('rv:');
        return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
    }
    // other browser
    return 0;
}

function Scroll_Anchor() {
    var $root = $('html, body');
    $('a').click(function () {
        var href = $.attr(this, 'href');
        if (href == "#") { return;}
        $root.animate({
            scrollTop: $(href).offset().top-60
        }, 500, function () {
            //if (window.location.hash !== null)
            //    window.location.hash = href;
        });
        return false;
    });

    $("body").append("<div id='scrolltop'><i class='fa-arrow-up'></i></div><div id='iphone'></div>");
    $("#scrolltop").click(function (e) {
        e.preventDefault();
        $('html, body').animate({ scrollTop: 0 }, 500);
    });

    var isrun = false;
    var reload = function () {
        var height = $(window).scrollTop();
        if (height > 100 && !$("#nav-top2").is(':visible')) {
            if (!isrun) {
                isrun = true;
                $("#scrolltop").fadeIn(1000, function () {
                    isrun = false;
                });
            }            
        }
        else {
            $("#scrolltop").hide();
        }
    }
    reload();

    $(window).scroll(function () {
        reload();
    });

    $(window).resize(function () {
        reload();
    });
}

function ScrollHTML() {
    //$("html").niceScroll({
    //    styler: "fb",
    //    cursorcolor: "#000",
    //    //cursorborder: "1px solid #888",
    //    //cursorborderradius: "5px",
    //    zindex: "9996",
    //    //touchbehavior: false,
    //    //scrollspeed: 60,
    //    //mousescrollstep: 40
    //});
    $(".nicescroll").niceScroll({
        //styler: "fb",
        //cursorcolor: "#000",
        //cursorborder: "1px solid #888",
        //cursorborderradius: "5px",
        //zindex: "99",
        //touchbehavior: true,
        //scrollspeed: 60,
        //mousescrollstep: 40
        autohidemode: false
    });
}

$(document).ready(function () {
    $('body').append('<div class="overlay hide"></div>');
    $('.menu-login.loged').click(function () {
        $('.overlay').toggleClass('hide');
    });
    $('.overlay').click(function () {
        $(this).toggleClass('hide');
        $('.menu-user').hide();
    });
    $('.list-item').each(function () {
        var id = $(this).attr('id');
        create_number('#' + id);
    });
    function create_number(id) {
        count = 0;
        $(id + ' .item-project').each(function () {
            count++;
        });
        if (count < 3) {
            $(id).addClass('soitem-' + count);
        }
    }
});