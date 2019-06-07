'use strict';

var appWebUrl, hostWebUrl;

jQuery(document).ready(function () {

    $("#MainContent_ddlProjCat").change(function () {
        var selectedProj = $(this).children("option:selected").val();
        $("#MainContent_txtProjCatURL").val(selectedProj);
        $("#MainContent_txtProjCatURL").attr('readonly', true);
        $("#MainContent_txtProjCatURL").css("background-color", "#dddddd");
    });

});

function getQueryStringParameter(paramToRetrieve) {
    var params = document.URL.split("?")[1].split("&amp;");
    alert(params);
    for (var i = 0; i < params.length; i = i + 1) {
        var singleParam = params[i].split("=");
        if (singleParam[0] === paramToRetrieve) return singleParam[1];
    }
}