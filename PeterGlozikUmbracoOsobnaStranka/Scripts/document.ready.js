$(document).ready(function () {
    // Submenu
    $('ul.nav li.dropdown').hover(
        function () { $(this).addClass('open'); },
        function () { $(this).removeClass('open'); }
    );

    $('img.lazy').lazy();

});