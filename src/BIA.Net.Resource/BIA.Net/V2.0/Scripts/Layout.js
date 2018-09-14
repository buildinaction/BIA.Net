$.fn.textWidth = function () {
    var html_org = $(this).html();
    var html_calc = '<span>' + html_org + '</span>';
    $(this).html(html_calc);
    var width = $(this).find('span:first').width();
    $(this).html(html_org);
    return width;
};

function WidthJS() {
    $(".WidthJS").each(function () {
        var width = $(this).textWidth();
        if (width != 0) {
            $(this).width(width);
        }
        //$(this).css('margin','auto');
    });
}

function ResizePadding() {
    $("#bottomBarPadder").height($("#bottomBar").height());
}

function SetFlipContentRightHeight() {
    $(".FlipContentRight").height($(window).height() - $("#bottomBar").height() - $(".navbar-header").height());
}
function closeMenu() {
    $('.navbar.navbar-bia').removeClass('open');
    $('#pusher').off('click touch', closeMenu);
}

function toogleMenu() {
    $(this).parents('.navbar.navbar-bia').toggleClass('open');
    $('#pusher').on('click touch', closeMenu);
}

var DefaultModeFullScreen = false;
var ManualToogleFullScreen = false;

var DefaultTheme = "BIANetThemeLight";
var ManualTheme = false;
var possibleTheme = ['BIANetThemeDark','BIANetThemeLight'];

function InitThemeAndMode(theme, fullScreen)
{
    DefaultTheme = theme;
    possibleTheme.forEach(function(item) {
        if ((theme != item) && $('body').hasClass(item)) {
            $('body').removeClass(item);
        }
    });
    if (!$('body').hasClass(theme)) {
        $('body').addClass(theme);
    }

    var themeShort = theme.replace("BIANetTheme","");
    $('img').each(function (index) {
        var src = $(this).attr('src');
        if (src.indexOf("/Img/Themes/")>=0) {
            var re = /\/Img\/Themes\/[^/]*\//;
            var srcnew = src.replace(re, "/Img/Themes/" + themeShort + "/");
            $(this).attr('src', srcnew);
        }
    });

    if (fullScreen)
    {
        DefaultModeFullScreen = true;
        if (!ManualToogleFullScreen) SetFullScreen();
    }
    else
    {
        DefaultModeFullScreen = false;
        if (!ManualToogleFullScreen) OutFullScreen();
    }
}

function SetFullScreen() {
    if (!$('body').hasClass('fullscreen')) {
        $('body').addClass('fullscreen');
        $('#linkFullScreen').removeClass('glyphicon-resize-full');
        $('#linkFullScreen').addClass('glyphicon-resize-small');
    }
}

function OutFullScreen() {
    if ($('body').hasClass('fullscreen')) {
        $('body').removeClass('fullscreen');
        $('#linkFullScreen').addClass('glyphicon-resize-full');
        $('#linkFullScreen').removeClass('glyphicon-resize-small');
    }
}

function toogleFullScreen() {
    ManualToogleFullScreen = true;
    if ($('body').hasClass('fullscreen')) {
        if (!DefaultModeFullScreen) ManualToogleFullScreen = false;
        OutFullScreen();
    }
    else {
        if (DefaultModeFullScreen) ManualToogleFullScreen = false;
        SetFullScreen();
    }
}


$(document).ready(function ($) {
    ResizePadding();
    SetFlipContentRightHeight();
    WidthJS();

    // Handle the display/collapse of the menu in mobile mode
    $('.navbar button.navbar-toggle').click(toogleMenu);
});
$(window).resize(function ($) {
    ResizePadding();
    SetFlipContentRightHeight();
    //WidthJS();
});

//Resize Dialog
function ResizePaddingDialog(dialog) {
    dialog.find("#bottomBarPadderDialog").height(dialog.find("#bottomBarDialog").height());
}

function SetFlipContentRightHeightDialog(dialog) {
    dialog.find(".FlipContentRight").height($(window).height() - dialog.find("#bottomBarDialog").height());
}


var scrollBarHeight = -1;
function getScrollBarHeight() {
    if (scrollBarHeight == -1) {
        var $outer = $('<div>').css({ visibility: 'hidden', height: 100, overflow: 'scroll' }).appendTo('body'), widthHeightScroll = $('<div>').css({ height: '100%' }).appendTo($outer).outerHeight();
        $outer.remove();
        scrollBarHeight = 100 - widthHeightScroll;
    }
    return scrollBarHeight;
}

function ResizeDialog(dialog) {
    //set width - scroll for padding calculation
    var dialogContainer = dialog.find('.dialogContainer');
    if (dialogContainer.length > 0) {
        dialog.find('#bottomBarDialog').width(dialogContainer.get(0).clientWidth);
        ResizePaddingDialog(dialog);
        SetFlipContentRightHeightDialog(dialog);

        dialogContainer.height(dialog.height() - 10);

        //var hasVerticalScrollbar = dialogContainer.get(0).scrollHeight > dialogContainer.get(0).clientHeight;
        //var hScrollDec = hasVerticalScrollbar ? getScrollBarWidth() : 0;
        var hasHorizontalScrollbar = dialogContainer.get(0).scrollWidth > dialogContainer.get(0).clientWidth;
        var vScrollDec = hasHorizontalScrollbar ? getScrollBarHeight() : 0;
        dialog.find('#bottomBarDialog').css("margin-bottom", vScrollDec);

        //Reset exact width if no scroll
        dialog.find('#bottomBarDialog').width(dialogContainer.get(0).clientWidth);
    }
}