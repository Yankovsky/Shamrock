/// <reference path="jquery-1.6.1-vsdoc.js" />
/// <reference path="jquery-1.6.1.js" />
/// <reference path="jquery-ui-1.8.14.custom.js" />

$.ajaxSetup({ cache: false });

$(document).ready(function () {
    localizeDatepicker();
    openDialog();
    pagingNavigation();
    supressSelectedLinks();
    //dishCategoriesNavigation();
    //bookmark();
});

function localizeDatepicker() {
    var lang = $('html').attr("lang");
    if (lang === "en")
        lang = "";
    $.datepicker.setDefaults($.datepicker.regional[lang]);
    $(":input[data-datepicker]").datepicker({ minDate: 0 }, $.datepicker.regional['']);
    /*$("#datepicker *").remove();

    $("#datepicker").datepicker({ minDate: 0, width: 20 }, $.datepicker.regional[''])
    .bind('dateSelected', function(e, selectedDate, $td){
    console.log('You selected ' + selectedDate);
    });
    */
}

function openDialog () {
    $(".openDialog").live("click", function (e) {
        e.preventDefault();

        $("<div></div>")
            .addClass("dialog")
            .attr("id", $(this).attr("data_dialog_id"))
            .dialog({
                title: $(this).attr("data_dialog_title"),
                close: function () { $(this).remove() },
                modal: true,
                position: ['center', 240],
                width: 600,
                show: "fade",
                hide: "fade"
            })
            .load(this.href);
        });

        $(".close").live("click", function (e) {
            e.preventDefault();
            window.location.reload(true);
            $(this).closest(".dialog").dialog("close");
        });
}

function pagingNavigation() {
    $(".paginator a").live("click", function () {
        $.get($(this).attr("href"), function (response) {
            replaceWithAnimation(".pagingList", response);
        });
        return false;
    });
}

function supressSelectedLinks() {
    $("a.selected").live("click", function () {
        return false;
    });
}

//function dishCategoriesNavigation() {
//    $("#dishCategoriesList a[class!='selected']").live("click", function () {
//        $(".pointer").remove();
//        $("#dishCategoriesList a.selected").removeClass('selected');
//        $(this).addClass("selected");
//        var selectedDishCategory = $(this).closest(".dishCategory");
//        $(this).closest(".dishCategory") = selectedDishCategory.add('<img alt="pointer" class="pointer" src="/Content/Images/Pointer.png">');
//        $.get($(this).attr("href"), function (response) {
//            //TODO
//        });
//        return false;
//    });
//}

function replaceWithAnimation(selector, value) {
    $(selector).fadeOut(500, function () {
        $(selector).replaceWith(value);
        $(selector).hide();
        $(selector).fadeIn(500);
    });    
}

//function bookmark() {
//    $("#bookmark").click(function () {
//        if (window.sidebar) // Mozilla Firefox Bookmark
//            window.sidebar.addPanel(location.href, document.title, "");
//        else if (window.external) // IE Favorite
//            window.external.AddFavorite(location.href, document.title);
//        else if (window.opera && window.print) // Opera Hotlist
//            this.title = document.title;
//        return true;
//    });
//}