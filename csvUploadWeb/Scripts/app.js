
'use strict';

var appWebUrl, hostWebUrl;

$(document).ready(function () {

    


    $("#ddlProjName").change(function () {

   
        var selectedProj = $(this).children("option:selected").val();
        $("#txtProjURL").val(selectedProj);
        $("#txtProjURL").attr('readonly', true);
        $("#txtProjURL").css("background-color", "#dddddd");
    });


});


var ItemContainer = { ItemList: [] };


//------------------- Get Manufacturer Dropdown -------------------------------------------------------------

function getProjCatalog(m) {

    var clientContext = SP.ClientContext.get_current();
    var oList = clientContext.get_web().get_lists().getByTitle(m);
    var camlQuery = new SP.CamlQuery();
    //camlQuery.set_viewXml('<Where><Eq><FieldRef Name="Catalog Num" /><Value Type="text"></Value></Eq></Where>');
    //camlQuery.set_viewXml('<OrderBy><FieldRef Name="Title" /></OrderBy><GroupBy Collapse ="TRUE" GroupLimit ="100"><FieldRef Name="Title"></FieldRef></GroupBy>')
    this.collListItem = oList.getItems(camlQuery);
    clientContext.load(collListItem, 'Include(Proj_x0020_Site_x0020_URL)');
    clientContext.executeQueryAsync(
        Function.createDelegate(this, this.onListDataLoadQuerySucceeded),
        Function.createDelegate(this, this.onListDataLoadQueryFailed));
}


function onListDataLoadQuerySucceeded(sender, args) {
    var listItemEnumerator = collListItem.getEnumerator();
    while (listItemEnumerator.moveNext()) {
        var oListItem = listItemEnumerator.get_current();
        var tempItem = { ID: oListItem.get_item('Proj_x0020_Site_x0020_URL').get_url(), Value: oListItem.get_item('Proj_x0020_Site_x0020_URL').get_description() };
        ItemContainer.ItemList.push(tempItem);
    }
    var ddMan = document.getElementById('ddlProjName');
    fillDropDown(ddMan);
}


function fillDropDown(ddl) {
    //var ddl = document.getElementById('ddlProjName');
    //alert(ddl);
    if (ddl != null) {
        for (var i = 0; i < ItemContainer.ItemList.length; i++) {
            var theOption = new Option;
            theOption.value = ItemContainer.ItemList[i].ID;
            theOption.text = ItemContainer.ItemList[i].Value;
            ddl.options[i] = theOption;
        }
    }
}


function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"), results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}


//function to get a parameter value by a specific key
function getQueryStringParameter(urlParameterKey) {
    var params = document.URL.split('?')[1].split('&');
    var strParams = '';
    for (var i = 0; i < params.length; i = i + 1) {
        var singleParam = params[i].split('=');
        if (singleParam[0] === urlParameterKey)
            return singleParam[1];
    }
}


// Get parameters from the query string.
// For production purposes you may want to use a library to handle the query string.
function getQueryStringParameter2(paramToRetrieve) {
    var params = document.URL.split("?")[1].split("&amp;");
    alert(params);
    for (var i = 0; i < params.length; i = i + 1) {
        var singleParam = params[i].split("=");
        if (singleParam[0] === paramToRetrieve) return singleParam[1];
    }
}