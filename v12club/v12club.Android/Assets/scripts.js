(function () {
    $('.catalogsCont > br').remove();
    $('.mainArticle').css({ 'padding-top': '15px' });
    $('.head').remove();
    $('.footer').remove();
    $('.topBar').css({ 'height': '4.5rem', 'display': 'flex', 'position': 'relative', 'position': 'sticky', 'top': '0', 'z-index': '10' });
    $('.openPanel').css({ 'position': 'absolute', 'left': '5%', 'bottom': '37%' });
    $('.wCart').css({ 'position': 'absolute', 'right': '5%', 'bottom': '20%' });
    $('.mainAsideMobile').css({ 'width': '50%', 'font-size': 'large', 'font-weight': '600' });
    $('.wMenu.wMenuMobile').css({ 'text-align': 'center' });
    $('.authMobile').hide();

    /* ИЗМЕНЕНИЕ ПОЗИЦИИ В МЕНЮ */
    $('#my-menu-mobile > .personal_cabinet').detach().insertBefore('#my-menu-mobile > .main');

    /* ИЗМЕНЕНИЕ СОДЕРЖАНИЯ ПУНКТОВ МЕНЮ */
    $('.menuItem.main > a').text('Каталог товаров');

    /* ВРЕМЕННЫЕ ФИКСЫ */
    $('.asideCatalogs').remove();
    $('.menuItem.auto2dV2').remove();
    $('.menuItem.carbase').remove();
    $('.menuItem.contacts').remove();
    $('.menuItem.vinqu').remove();

    setTimeout(() => console.log('Ready'), 500)
})();