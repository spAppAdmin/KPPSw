


// retrieve the appWebUrl and hostWebUrl from the querystring somewhere above here
// you'll need them to retrieve the scripts and make the CSOM calls
var scriptBase = hostweburl + "/_layouts/15/";
$.getScript(scriptBase + "MicrosoftAjax.js").then(function (data) {
    return $.getScript(scriptbase + "SP.Runtime.js");
}).then(function (data) {
    return $.getScript(scriptbase + "SP.js");
}).then(function (data) {
    $.getScript(scriptBase + "SP.RequestExecutor.js");
}).then(function (data) {
    var ctx = new SP.ClientContext(appWebUrl),
        factory = new SP.ProxyWebRequestExecutorFactory(appWebUrl),
        web;

    ctx.set_webRequestExecutorFactory(factory);
    web = ctx.get_web();
    ctx.load(web);
    ctx.executeQueryAsync(function () {
        // log the name of the app web to the console
        console.log(web.get_title());
    }, function (sender, args) {
        console.log("Error : " + args.get_message());
    });
});




var appWebUrl, hostWebUrl;

$(document).ready(function () {

    $("#ddlProjName").change(function () {
        var selectedProj = $(this).children("option:selected").val();
        $("#txtProjURL").val(selectedProj);
        $("#txtProjURL").attr('readonly', true);
        $("#txtProjURL").css("background-color", "#dddddd");
    }); 

    //$("#msgHtml").html("<table border=0><tr><td><img src='../Images/alert.png'/></td><td>" + " Project Site Required" + "</td></tr></table>");

    $("#Button1_Click1").click(function () {

        if ($("#txtProjURL").val().length === 0) {
            
            $("#msgHtml").html("<table border=0><tr><td><img src='../Images/alert.png'/></td><td>" + " Project Required" + "</td></tr></table>");
            return false;
        } else {
            $("#msgHtml").html("");
            return true;

        }
    });
            
});







var ItemContainer = { ItemList: [] };


//------------------- Get Project Catalog -------------------------------------------------------------

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
    if (ddl !== null) {
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
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
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