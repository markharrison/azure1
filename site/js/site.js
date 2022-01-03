function setCookie(strCookieName, strCookieValue) {
    var myDate = new Date();
    myDate.setMonth(myDate.getMonth() + 12);
    document.cookie = strCookieName + "=" + strCookieValue + ";expires=" + myDate;
}

function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length === 2) return parts.pop().split(";").shift();
}

function getInternetExplorerVersion() {
    var rv = -1;
    if (navigator.appName == 'Microsoft Internet Explorer') {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    else if (navigator.appName == 'Netscape') {
        var ua = navigator.userAgent;
        var re = new RegExp("Trident/.*rv:([0-9]{1,}[\\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
}

function openNav() {

    $("#idNavOverlay").css("display", "block");
    $("#idNavSidebar").css("marginLeft", "0px");
    $("#idNavSidebar").css("marginRight", "0px");
    $("#idMainContent").css("marginLeft", "252px");
    $("#idMainContent").css("marginRight", "-248px");
}

function closeNav() {

    $("#idNavSidebar").css("marginLeft", "-250px");
    $("#idNavSidebar").css("marginRight", "250px");
    $("#idMainContent").css("marginLeft", "2px");
    $("#idMainContent").css("marginRight", "2px");
    $("#idNavOverlay").css("display", "none");
}

function toggleNav() {
    if ($("#idNavButton").hasClass("is-active"))
        closeNav();
    else
        openNav();
    $("#idNavButton").toggleClass("is-active");
}

var gLastCatName = "";

function setHash(strHash) {
    history.replaceState({}, "Azure 1", "#" + strHash);
}

function resetHash() {
    setHash("");
    gLastCatName = "";
}

function showLightBox(text, url) {
    var width = $(window).width();
    var height = $(window).height();
    var maxheight;
    var maxwidth;
    if (width > height) {
        maxheight = height - 100;
        maxwidth = Math.min(maxheight * 16 / 9, (width * .80) - 32);
    } else {
        maxwidth = (width * 0.95) - 32;
        maxheight = maxwidth * 9 / 16;
    }

    var vMaxHeightStyle = "max-height: " + maxheight + "px;";
    var vMaxWidthStyle = "max-width: " + maxwidth + "px;";

    var vHtml = "";
    vHtml += "<div class='embed-responsive embed-responsive-16by9' style='" + vMaxHeightStyle + vMaxWidthStyle + "' >";
    vHtml += "<iframe  src='" + url + "' allowfullscreen></iframe>";
    vHtml += "</div>";

    $('#idLightboxBody').html(vHtml);
    $('#idLightbox').modal('show');
    $('#idServiceModal').hide();
}

function showService(service) {
    var serviceX = service.toLowerCase().replace(/\s+/g, '');
    var title = $("#idSCT-" + serviceX).html();
    if (title != undefined) {
        $('#idServiceModalTitle').html($("#idSCT-" + serviceX).html());
        $('#idServiceModalBody').html($("#idSC-" + serviceX).html());
        $('#idServiceModal').modal('show');
        setHash("//" + serviceX);

        gtag('config', 'UA-81679476-1', { 'page_path': '/' + serviceX + '.svc.modal' });
    }
    
};

function showCategory(category) {
    $("#idSearchBox").hide();
    grid.filter(function (item) {
        return (item.getElement().getAttribute("data-cat").includes(category));
    });

    var categoryX = category.toLowerCase().replace(/\s+/g, '');
    setHash("/" + categoryX);
    gLastCatName = categoryX;

    gtag('config', 'UA-81679476-1', { 'page_path': '/' + categoryX + '.cat.modal' });
};

function showOption(option) {
    resetHash();
    if (option == "Search") {
        $("#idSearchText").val("");
        $("#idSearchBox").toggle();
        $("#idSearchText").focus();
    }
    else {
        $("#idSearchBox").hide();
        if (option == "Home") {
            gtag('config', 'UA-81679476-1', { 'page_path': '/home.view' });
            grid.filter(function (item) {
                return (item.getElement().getAttribute("data-cat").includes("category"));
            });
        }
        else if (option == "All") {
            gtag('config', 'UA-81679476-1', { 'page_path': '/all.view' });
            grid.filter('.item');
        }
    }
}

var grid;

function getIconFromClass(obj) {
    var ret = null;

    var classNames = $(obj).children("i").attr("class").toString().split(' ');
    $.each(classNames, function (i, className) {
        if (className.startsWith("icon-")) {
            var xclassName = className.split('_');
            ret = xclassName[1];
        }
    });
    return ret;
};

function doCallBacks() {

    $(document).keyup(function (e) {
        if (e.keyCode == 27) { // escape  
            if ($("#idNavButton").hasClass("is-active")) {
                toggleNav();
            }
        }
    });

    $('#idLightbox').on('hide.bs.modal', function (e) {
        $('#idLightboxBody').html("");
        $('#idServiceModal').show();
        setTimeout(function () { $('#idServiceModalClose').focus(); }, 500);
    });

    $('#idServiceModal').on('hide.bs.modal', function (e) {
        setHash("/" + gLastCatName);
    });

    $('#idSearchText').on('input', function (e) {
        resetHash();
        var searchtext = $('#idSearchText').val().toLowerCase();
        if (searchtext.length > 1) {
            grid.filter(function (item) {
                return (item.getElement().getAttribute("data-name").toLowerCase().includes(searchtext));
            });
        }
        if (searchtext.length == 0) {
            grid.filter(function (item) {
                return (item.getElement().getAttribute("data-cat").includes("category"));
            });
        }
    });


    $(".iconsidebar").on({
        mouseover: function () {
            var icon = getIconFromClass(this);
            this.style.cursor = "pointer";
            $(this).children("i").addClass("icon-red_" + icon).removeClass("icon-white_" + icon);
            $(this).css("color", "red");
        },
        mouseout: function () {
            var icon = getIconFromClass(this);
            this.style.cursor = "default";
            $(this).children("i").addClass("icon-white_" + icon).removeClass("icon-red_" + icon);
            $(this).css("color", "white");
        }
    });

    $("#idModalBody").on({
        mouseover: function () {
            var icon = getIconFromClass(this);
            this.style.cursor = "pointer";
            $(this).children("i").addClass("icon-red_" + icon).removeClass("icon-black_" + icon);
            $(this).css("color", "red");
            $(this).css("text-decoration", "underline");
        },
        mouseout: function () {
            var icon = getIconFromClass(this);
            this.style.cursor = "default";
            $(this).children("i").addClass("icon-black_" + icon).removeClass("icon-red_" + icon);
            $(this).css("color", "black");
            $(this).css("text-decoration", "none");
        }
    }, ".iconmodal");

    $("#idServiceModalBody").on({
        mouseover: function () {
            var icon = getIconFromClass(this);
            this.style.cursor = "pointer";
            $(this).children("i").addClass("icon-red_" + icon).removeClass("icon-black_" + icon);
            $(this).css("color", "red");
            $(this).css("text-decoration", "underline");
        },
        mouseout: function () {
            var icon = getIconFromClass(this);
            this.style.cursor = "default";
            $(this).children("i").addClass("icon-black_" + icon).removeClass("icon-red_" + icon);
            $(this).css("color", "black");
            $(this).css("text-decoration", "none");
        }
    }, ".iconservicemodal");

}

function setConfigColorScheme(strColorScheme) {
    setCookie("ColorScheme", strColorScheme);
}

function getConfigColorScheme() {
    var _strColorScheme = getCookie("ColorScheme");
    if (_strColorScheme == undefined) {
        _strColorScheme = "light";
        setConfigColorScheme(_strColorScheme);
    }
    return _strColorScheme;
}

function doColorScheme() {

    var strColorScheme = getConfigColorScheme();

    if (strColorScheme == "light") {
        $("html").css("background-color", "white");
        $("hr").css("border-top", "1px solid rgba(0,0,0,.1)");
        $("#idSearchBox").css("color", "#212529");
    } else {
        $("html").css("background-color", "#191919");
        $("hr").css("border-top", "1px solid rgba(200, 200, 200, 0.3)");
        $("#idSearchBox").css("color", "white");
    }
}

function changeColorScheme(strColorScheme) {
    setConfigColorScheme(strColorScheme);
    doColorScheme();
}

function doConfig() {
    var strHTML = "";
    strHTML += "<i class='icon-black_fa-moon iconfa20xp'></i> &nbsp; <a href='#' class='modal-link' onclick='changeColorScheme(\"dark\");return false'>Set Dark scheme</a><br /><br />";
    strHTML += "<i class='icon-black_fa-sun  iconfa20xp'></i> &nbsp; <a href='#' class='modal-link' onclick='changeColorScheme(\"light\");return false'>Set Light scheme</a><br />";

    toggleNav();

    $('#idModalTitle').html("Config");
    $('#idModalBody').html(strHTML);
    $('#idModal').modal("show");

}

function doInfo() {

    var part1 = "mark.harrison";
    var part2 = "microsoft.com";
    var part3 = "?subject=azure1";
    var mailaddr = "mai" + "lto:" + part1 + '@' + part2 + part3;

    var strHTML = "";
    strHTML += "This site 'azure1.dev' was only built for my own use - to learn and demo.<br /><br />";
    strHTML += "If it's useful for others thats great - but it is provided 'as is' with no warranties, and confer no rights.<br /><br />";
    strHTML += "Feedback welcome / appreciated.<br /><br />";
    strHTML += "Mark Harrison<br /><br />";
    strHTML += "<span class='iconmodal' onclick='window.open(\"" + mailaddr + "\",\"_self\");' >";
    strHTML += "<i class='icon-black_fa-envelope iconfa20xp'></i>Email</span>";
    strHTML += "&nbsp;&nbsp;&nbsp;&nbsp;";
    strHTML += "<span class='iconmodal' onclick='window.open(\"https://www.linkedin.com/in/markharrison-uk/\",\"_blank\");' >";
    strHTML += "<i class='icon-black_fa-linkedin iconfa20xp'></i>LinkedIn</span>";
    strHTML += "&nbsp;&nbsp;&nbsp;&nbsp;";
    strHTML += "<span class='iconmodal' onclick='window.open(\"https://github.com/markharrison/\",\"_blank\");' >";
    strHTML += "<i class='icon-black_fa-github iconfa20xp'></i>Github</span>";
    strHTML += "&nbsp;&nbsp;&nbsp;&nbsp;";
    strHTML += "<span class='iconmodal' onclick='window.open(\"https://aka.ms/markharrisonvideo\",\"_blank\");' >";
    strHTML += "<i class='icon-black_fa-videoplay iconfa20xp'></i>Videos</span>";
    strHTML += "<br /><br />";


    

    toggleNav();

    $('#idModalTitle').html("About Azure1");
    $('#idModalBody').html(strHTML);
    $('#idModal').modal("show");

}

function doIndexReady() {

    if (getInternetExplorerVersion() > 0 ) {
        var msg = "Internet Explorer is not supported - please switch to a modern browser";
        alert(msg);
        $("body").html(msg);
        return;
    }

    if (window.location.hostname.toLowerCase().indexOf("azure1.io") >= 0) {
        var msg = "This site is moving to https://azure1.dev - update your bookmarks";
        alert(msg);
        $("body").html("<br><br>&nbsp;&nbsp;" + msg + "<br><br>&nbsp;&nbsp;<i class='iconfa15xp icon-black_fa-link'></i>&nbsp;<a href='https://azure1.dev'>https://azure1.dev</a>");
        return;
    }

    doColorScheme();

    $("#idLoaded").css("display", "block");
    $("body").removeClass("preload");
    $('.tooltipicons').tooltip({ boundary: 'window' })

    var vHash = location.hash.toLowerCase();

    doCallBacks();

    grid = new Muuri('.grid', {
        items: [],
        dragEnabled: false,
        layout: {
            fillGaps: true
        }
    });

    grid.add(document.querySelectorAll('.item'), { active: false, layout: false });

    if (vHash.slice(0, 3) === "#//") {
        showService(vHash.slice(3));
    }
    else if (vHash.slice(0, 2) === "#/") {
        showCategory(vHash.slice(2));
    }
    else {
        showOption("Home");
    }

}
