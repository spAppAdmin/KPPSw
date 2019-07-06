



                                                                                               $(document).ready(function () {
       
                                                                                                  
                                                                                                    var hostweburl = decodeURIComponent(getQueryStringParameter("SPHostUrl"));
                                                                                                    var appweburl = decodeURIComponent(getQueryStringParameter("SPAppWebUrl"));
                                                                                                       //alert(hostweburl);
                                                                                                       // alert(appweburl);
                                                                                                        var scriptbase = hostweburl + "/_layouts/15/";
                                                                                                        $.getScript(scriptbase + "SP.UI.Controls.js", renderChrome);
        
                                                                                                  

                                                                                                        $("#csvFile").val("C:\\temp\\ITL.csv");
                                                                                                        $("#txtProjURL").val("https://kineticsys.sharepoint.com/sites/projects/construction/SPP/000000000");

                                                                                                       
                                                                                                    });


                                                                                                function pageLoad() {
                                                                                                    Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(cancelPostBack);
                                                                                                }

                                                                                                function cancelPostBack(sender, args) {
                                                                                                    if (Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack()) {
                                                                                                        alert('One postback at a time please');
                                                                                                        args.set_cancel(true);
                                                                                                        $("#imgProgress").hide();
                                                                                                    }
                                                                                                }  




                                                                                                function renderChrome() {

                                                                                                    var options = {
                                                                                                        "appIconUrl": "/images/siteicon.png",
                                                                                                        "appTitle": "CUFS - CSV Uploader For SharePoint"
                                                                                                    };

                                                                                                    var nav = new SP.UI.Controls.Navigation(
                                                                                                        "chrome_ctrl_placeholder",
                                                                                                        options);
                                                                                                    nav.setVisible(true);
                                                                                                }


                                                                                                function getQueryStringParameter(paramToRetrieve) {
                                                                                                            var params =
                                                                                                        document.URL.split("?")[1].split("&amp;");
                                                                                                    var strParams = "";
                                                                                                            for (var i = 0; i < params.length; i = i + 1) {
                                                                                                                var singleParam = params[i].split("=");
                                                                                                    if (singleParam[0] === paramToRetrieve)
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
                                                                                                    return false;
                                                                                                }


                                                                                                function move() {
                                                                                                    var elem = document.getElementById("progressBar");
                                                                                                    var width = 1;
                                                                                                    var id = setInterval(frame, 50);
                                                                                                    function frame() {
                                                                                                        if (width >= 100) {
                                                                                                            clearInterval(id);
                                                                                                        } else {
                                                                                                            width++;
                                                                                                            elem.style.width = width + '%';
                                                                                                        }
                                                                                                    }
                                                                                                }




                                                                                                var appWebUrl, hostWebUrl;

                                                                                                $(document).ready(function () {

                                                                                                    //showModalPopUp();

                                                                                                    $("#ddlProjName").change(function () {
                                                                                                        
                                                                                                        var selectedProj = $(this).children("option:selected").val();
                                                                                                        $("#txtProjURL").val(selectedProj);
                                                                                                        $("#txtProjURL").val(selectedProj);
                                                                                                        $("#txtProjURL").attr('readonly', true);
                                                                                                        $("#txtProjURL").css("background-color", "#dddddd");
                                                                                                    }); 

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



   







                                                                                        var availableTags = [
                                                                                            "ActionScript",
                                                                                            "AppleScript",
                                                                                            "Asp",
                                                                                            "BASIC",
                                                                                            "C",
                                                                                            "C++",
                                                                                            "Clojure",
                                                                                            "COBOL",
                                                                                            "ColdFusion",
                                                                                            "Erlang",
                                                                                            "Fortran",
                                                                                            "Groovy",
                                                                                            "Haskell",
                                                                                            "Java",
                                                                                            "JavaScript",
                                                                                            "Lisp",
                                                                                            "Perl",
                                                                                            "PHP",
                                                                                            "Python",
                                                                                            "Ruby",
                                                                                            "Scala",
                                                                                            "Scheme"
                                                                                        ];
                                                                                        $("#autocomplete").autocomplete({
                                                                                            source: availableTags
                                                                                        });
                                                                                        $("#accordion").accordion();
                                                                                        $("#button").button();
                                                                                        $("#button-icon").button({
                                                                                            icon: "ui-icon-gear",
                                                                                            showLabel: false
                                                                                        });

                                                                                        $("#radioset").buttonset();
                                                                                        $("#controlgroup").controlgroup();
                                                                                        $("#tabs").tabs();
                                                                                        $("#dialog").dialog({
                                                                                            autoOpen: false,
                                                                                            width: 400,
                                                                                            buttons: [
                                                                                                {
                                                                                                    text: "Ok",
                                                                                                    click: function () {
                                                                                                        $(this).dialog("close");
                                                                                                    }
                                                                                                },
                                                                                                {
                                                                                                    text: "Cancel",
                                                                                                    click: function () {
                                                                                                        $(this).dialog("close");
                                                                                                    }
                                                                                                }
                                                                                            ]
                                                                                        });

                                                                                        // Link to open the dialog
                                                                                        $("#dialog-link").click(function (event) {
                                                                                            $("#dialog").dialog("open");
                                                                                            event.preventDefault();
                                                                                        });

                                                                                        $("#datepicker").datepicker({
                                                                                            inline: true
                                                                                        });

                                                                                        $("#slider").slider({
                                                                                            range: true,
                                                                                            values: [17, 67]
                                                                                        });


                                                                                        $("#progressbar").progressbar({
                                                                                            value: 20
                                                                                        });

                                                                                        $("#spinner").spinner();

                                                                                        $("#menu").menu();

                                                                                        $("#tooltip").tooltip();

                                                                                        $("#selectmenu").selectmenu();

                                                                                        $("#dialog-link, #icons li").hover(
                                                                                            function () {
                                                                                                $(this).addClass("ui-state-hover");
                                                                                            },
                                                                                            function () {
                                                                                                $(this).removeClass("ui-state-hover");
                                                                                            }
                                                                                        );


                                                                                            





