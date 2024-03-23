$(document).ready(function () {
    // Kontrola, Ëi je cookie "cookiesAccepted" nastavenÈ
    if (getCookie("cookiesAccepted") !== "true") {
        // Cookie nie je nastavenÈ, zobrazÌme div s cookies ozn·menÌm
        $('.cookies-div').css('display', 'block');
    }

    // Nastavenie akcie pre tlaËidlo "S⁄HLASÕM"
    $('.cookies-ok').click(function (e) {
        e.preventDefault();
        pgsoftwebCookiesOk();
    });

    // Nastavenie akcie pre tlaËidlo "ODMIETNUç" a zatv·racÌ krÌûik
    $('.cookies-cancel, .cookies-close').click(function (e) {
        e.preventDefault();
        pgsoftwebCookiesClose();
    });
});

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + exdays * 24 * 60 * 60 * 1000);
    var expires = 'expires=' + d.toUTCString();
    document.cookie = cname + '=' + cvalue + ';' + expires + ';path=/';
}

function getCookie(cname) {
    var name = cname + '=';
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) === 0) {
            return c.substring(name.length, c.length);
        }
    }
    return '';
}

function pgsoftwebCookiesOk() {
    setCookie('cookiesAccepted', 'true', 365); // NastavÌ cookies na 365 dnÌ
    $('.cookies-div').css('display', 'none'); // Skryje div s cookies ozn·menÌm
}

function pgsoftwebCookiesClose() {
    setCookie('cookiesAccepted', 'true', 365); // NastavÌ cookies na 365 dnÌ
    $('.cookies-div').css('display', 'none'); // Skryje div s cookies ozn·menÌm
}
