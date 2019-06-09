



    $(document).ready(function () {
       

    var hostweburl = decodeURIComponent(getQueryStringParameter("SPHostUrl"));
    var appweburl = decodeURIComponent(getQueryStringParameter("SPAppWebUrl"));
        //alert(hostweburl);
        //alert(appweburl);
    var scriptbase = hostweburl + "/_layouts/15/";
        $.getScript(scriptbase + 'SP.Runtime.js',

            function () {
                $.getScript(scriptbase + 'SP.js',
                    function () { $.getScript(scriptbase + 'SP.RequestExecutor.js'); }
                );
            }
        );
    });



function getQueryStringParameter(paramToRetrieve) {
            var params =
        document.URL.split("?")[1].split("&amp;");
    var strParams = "";
            for (var i = 0; i < params.length; i = i + 1) {
                var singleParam = params[i].split("=");
    if (singleParam[0] == paramToRetrieve)
        return singleParam[1];
}
}



    function showModalPopUp() {
        //Set options for Modal PopUp  
        var options = {
        url: 'upload.aspx?IsDlg=1', //Set the url of the page
    title: 'SharePoint Modal Pop Up', //Set the title for the pop up
    allowMaximize: false,
    showClose: true,
    width: 600,
    height: 400
};
//Invoke the modal dialog by passing in the options array variable
//SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
return false;
}







var appWebUrl, hostWebUrl;

$(document).ready(function () {

   // showModalPopUp();

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