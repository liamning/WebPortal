<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileUpload.aspx.cs" Inherits="_Default" %>


<%@ Register Src="~/Control/MenuBar.ascx" TagPrefix="uc1" TagName="MenuBar" %>
<%@ Register Src="~/Control/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>
<%@ Register Src="~/Control/PublicHeader.ascx" TagPrefix="uc1" TagName="PublicHeader" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
 
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title> 
    <uc1:PublicHeader runat="server" ID="PublicHeader" />
    
  <link type="text/css" rel="stylesheet" href="Resource/autoComplete/jquery-ui.css" /> 
  <script type="text/javascript" src="Resource/autoComplete/jquery-ui.js"></script>
  <style type="text/css"> 
  
    .ui-tooltip {
    font-size:9pt;  
    }
    .ui-autocomplete {
    font-size:9pt; 
    font-family:Arial;
    }
    .ui-autocomplete.source:hover {
    font-size:9pt; 
    }
    .ui-menu .ui-menu-item { 
	padding: 1px;
}

  </style>
     
    <script type="text/javascript">
        $(function () {
            //init the user grid
            $userGridContainer = $("#userGridContainer");
            $ddlFilter = $("#ddlFilter");
            $btnUpload = $("#btnUpload");
            $txtFileUpload = $("#txtFileUpload")
            var $form = $("form");
            //$form.append("<input type='text' style='display:none;' id='txtAction' name='action' />");
            //$action = $form.find("#txtAction");
            $btnEdit = $("#btnEdit");
            $btnPublish = $form.find("#btnPublish");
            $btnUnPublish = $form.find("#btnUnPublish");
            $btnTrash = $form.find("#btnTrash");
            $labPath = $form.find("#labPath");
            $txtFileName = $form.find("#txtFileName");
            $txtDirectory = $form.find("#txtDirectory");
            $txtDescription = $form.find("#txtDescription");
            $comType = $form.find("#comType");
            $comDirectory = $form.find("#comDirectory");
 
            var requiredMsgSuffix = "required.";
            var mandatoryControl = [
                { $control: $txtDirectory, requiredMsg: "Directory " + requiredMsgSuffix },
                { $control: $txtFileName, requiredMsg: "File Name " + requiredMsgSuffix },
                { $control: $txtDescription, requiredMsg: "Description " + requiredMsgSuffix },
                { $control: $txtFileUpload, requiredMsg: "File " + requiredMsgSuffix }
            ];
            var mandatoryControl_update = [
                { $control: $txtDirectory, requiredMsg: "Directory " + requiredMsgSuffix },
                { $control: $txtFileName, requiredMsg: "File Name " + requiredMsgSuffix },
                { $control: $txtDescription, requiredMsg: "Description " + requiredMsgSuffix }
            ];

            //function
           /* var updatePreviewPath = function(){
                $labPath.text($txtDirectory.val() + "/" + $txtFileName.val());
                if ($labPath.text() == "/") $labPath.text("");
        }*/
             
            var userGrid = new function () {
                return new Grid({
                    parent: $userGridContainer[0],
                    checkbox: { selectAll: true, width:20, style: "background:#E5F0D4;", titleStyle: "text-align:left;" },
                    width: 880,
                    fields: [
                    { name: "directory", title: "Directory", width: 220, style: "text-align: left;", titleStyle: "text-align: left;" },
                    //{ name: "filename", title: "File Name", width: 150, style: "text-align: left;", titleStyle: "text-align: left;" },
                    //{ name: "orginfilename", title: "Original File Name", width: 150, style: "text-align: left;", titleStyle: "text-align: left;" },
                    { name: "description", title: "Description", width: 400, style: "text-align: left;", titleStyle: "text-align: left;" },
                    { name: "status", title: "Status", width: 80, style: "text-align: left;", titleStyle: "text-align: left;" },
                    { name: "uploaddate", title: "Upload Date", width: 80, style: "text-align: left;", titleStyle: "text-align: left;" }

                    ]
                });
            }

            var fillFileGrid = function (category) {

                $.ajax({
                    type: 'POST',
                    url: "Service/AjaxService.aspx",
                    data: {
                        action: "getFileList",
                        type: $comType.val(),
                        directory: $comDirectory.val()
                    },
                    success: function (data) {
                        data = eval('(' + data + ')');
                        if (data.error) {
                            var decoded = $("<div/>").html(data.error).text();
                            alert(decoded);
                            return;
                        }
                        userGrid.setData(data);
                    }
                });
            }

            var fillDirectory = function (category) {

                $.ajax({
                    type: 'POST',
                    url: "Service/AjaxService.aspx",
                    data: {
                        action: "GetDirectoryList",
                        type: $comType.val()
                    },
                    success: function (data) {
                        data = eval('(' + data + ')');
                        if (data.error) {
                            var decoded = $("<div/>").html(data.error).text();
                            alert(decoded);
                            return;
                        }
                        $comDirectory.find('option')
                                 .remove()
                                 .end();
                        $comDirectory.append($("<option></option>")
                                .attr("value", "All")
                                .text("All"));
                        $.each(data, function (key, value) {
                            $comDirectory.append($("<option></option>")
                                 .attr("value", value.fullname)
                                 .text(value.fullname));
                        });

                        fillFileGrid();
                    }
                });
            }

            var upload = function (formData) {
                 
                $("input[type=hidden].appendFields").remove();

                //$action.val("uploadFile");

                for (var pro in formData) {
                    $('<input />').attr('type', 'hidden').attr('class', 'appendFields')
                            .attr('name', pro)
                            .attr('value', formData[pro])
                            .appendTo('#form1');
                }
                 
                $("#btnURL").click(); 
            }
            var syncFileName = function () {
                $txtFileName.val($txtDescription.val());
            }

            var defineEventDelegate = function () {
                 
               /* $txtFileName.change(function () {
                    updatePreviewPath();
                });
                $txtDirectory.change(function () {
                    updatePreviewPath();
                }).blur(function () {
                    updatePreviewPath();
                });*/
                  
                $comType.change(function () {
                    //fillFileGrid();
                    fillDirectory();
                });

                $comDirectory.change(function () {
                    fillFileGrid();
                });

                $form.iframePostForm
                ({
                    json: true,
                    post: function () {
                    },
                    complete: function (data) {

                        if (data.error) {
                            var decoded = $("<div/>").html(data.error).text();
                            alert(decoded);
                            return;
                        }

                        //upload successful
                        alert(data.message);

                        fillDirectory();

                    }
                });
                var directoryChecking = function (oldDirectory, newDirectory)
                {
                    var oldArray = oldDirectory.split('/');
                    var newArray = newDirectory.split('/');
                    var error = "Cannot change the parent directory name.";
                    if (oldArray.length != newArray.length) {
                        alert(error);
                        return false;
                    }
                    for (var i = 0, oldItem, newItem; oldItem = oldArray[i], newItem = newArray[i]; i++) {
                        if (newItem != oldItem && i < oldArray.length - 1)
                        { 
                            alert(error);
                            return false;
                        }
                    }

                    return true;
                }
                $btnUpload.click(function () {

                    syncFileName();

                    var dataSelected = userGrid.getSelectdData(); 

                    var formData;
                    if (dataSelected.length == 0) {
                        if (!validate(mandatoryControl)) return;
                        formData = {
                            action: "uploadFile"
                        }
                    } else {
                        if (!validate(mandatoryControl_update)) return;
                        formData = {
                            action: "updateFile",
                            ID: dataSelected[0].id,
                            DirectoryID: dataSelected[0].directoryid
                        }
                        
                        //check whether the parent directory is changed
                        var newDirectory = $txtDirectory.val();
                        if (!directoryChecking(dataSelected[0].directory, newDirectory)) return;
                    }

                    upload(formData);
                });



                $btnEdit.click(function () {
                    var dataSelected = userGrid.getSelectdData(); 
                    for (var i = 0, user; user = dataSelected[i]; i++) {
                        $txtDescription.val(user.description); 
                        $txtDirectory.val(user.directory);
                        break;
                    } 
                     
                });
                 


                $btnPublish.click(function () {
                    var dataSelected = userGrid.getSelectdData();
                    var IDs = [];
                    for (var i = 0, user; user = dataSelected[i]; i++) {
                        IDs.push(user.id);
                    }
                    if (IDs.length == 0) return;
                    $.ajax({
                        type: 'POST',
                        url: "Service/AjaxService.aspx",
                        data: {
                            action: "publishFiles",
                            IDs: IDs
                        },
                        success: function (data) {
                            data = eval('(' + data + ')');
                            if (data.error) {
                                var decoded = $("<div/>").html(data.error).text();
                                alert(decoded);
                                return;
                            }

                            alert(data.message);

                            //fillFileGrid();
                            fillDirectory();
                        }
                    });
                });

                $btnUnPublish.click(function () {
                    var dataSelected = userGrid.getSelectdData();
                    var IDs = [];
                    for (var i = 0, user; user = dataSelected[i]; i++) {
                        IDs.push(user.id);
                    }
                    if (IDs.length == 0) return;
                    $.ajax({
                        type: 'POST',
                        url: "Service/AjaxService.aspx",
                        data: {
                            action: "unpublishFiles",
                            IDs: IDs
                        },
                        success: function (data) {
                            data = eval('(' + data + ')');
                            if (data.error) {
                                var decoded = $("<div/>").html(data.error).text();
                                alert(decoded);
                                return;
                            }

                            alert(data.message);

                            //fillFileGrid();
                            fillDirectory();
                        }
                    });
                });
                 
                $btnTrash.click(function () {


                    var dataSelected = userGrid.getSelectdData();
                    var IDs = [];
                    for (var i = 0, user; user = dataSelected[i]; i++) {
                        IDs.push(user.id);
                    }
                    if (IDs.length == 0) return;
                    if (!confirm("Please confirm to trash this file.")) return;

                    $.ajax({
                        type: 'POST',
                        url: "Service/AjaxService.aspx",
                        data: {
                            action: "deleteFiles",
                            IDs: IDs
                        },
                        success: function (data) {
                            data = eval('(' + data + ')');
                            if (data.error) {
                                var decoded = $("<div/>").html(data.error).text();
                                alert(decoded);
                                return;
                            }

                            alert(data.message);

                            //fillFileGrid();
                            fillDirectory();
                        }
                    });
                });

                //autocomplete
                $txtDirectory.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: 'POST',
                            url: "Service/AjaxService.aspx",
                            data: {
                                action: "autoComplete",
                                term: request.term,
                                type: $comType.val()
                            },
                            success: function (msg) {
                                var data = $.parseJSON(msg);
                                if (data.error) {
                                    var decoded = $("<div/>").html(data.error).text();
                                    alert(decoded);
                                    return;
                                }

                                response(data);
                            },
                            error: function (msg) {
                                var data = $.parseJSON(msg);
                                alert(data.message);
                            }
                        })
                    }
                });

            }

            var init = function () {
                defineEventDelegate();
               // fillFileGrid();
                fillDirectory();
            }

            //initialize
            init();


        });
    </script>
</head>

<body>
    <form id="form1" runat="server">
        <uc1:MenuBar runat="server" ID="MenuBar" />
    <div id="content">
        <div class="clearLeftFloat"></div>
        <div id="navigationBar">
            <p>Staff Portal > File Upload Manager</p> 
        </div>
        <div id="center" class="font12pt">
            <table>
                <tr>
                    <td colspan="3">
                        <h4 class="bar">File Upload</h4>         
                    </td> 
                </tr>
                
                <tr>
                    <td class="titleTd">
                    <span>Type</span>
                    </td>                                       
                    <td >
                        <select id="comType" runat="server">  
                        </select>  
                    </td>                                 
                    <td  style="text-align:right;"> 
                        <input type="button" value="Save" id="btnUpload" />
                        <asp:Button ID="btnURL" runat="server" Text="Button" style="display:none;" PostBackUrl="~/Service/AjaxService.aspx" />
                    </td>
                </tr>

                <tr>
                    <td class="titleTd" style="width:80px;">
                    <span>Directory:</span>
                    </td>                                       
                    <td colspan="2">   
                        <input id="txtDirectory" runat="server" name="directory" style="margin-left:0px; width: 500px;" type="text"  /> 
                    </td>   
                </tr>
                <tr style="display:none;">
                    <td class="titleTd" >
                    <span>File Name:</span>
                    </td>                                       
                    <td colspan="2">
                        <input type="text" id="txtFileName" name="name" runat="server"  style="margin-left:0px;width: 310px;"  /> 
                    </td>
                </tr>
                <tr>
                    <td class="titleTd">
                    <span>Description:</span>
                    </td>                                       
                    <td colspan="2">
                        <input type="text" name="description"  id="txtDescription" runat="server"  style="margin-left:0px;width: 310px;" /> 
                    </td>
                </tr>
                <tr>
                    <td class="titleTd">
                    </td>                                       
                    <td colspan="2">
                    <input id="txtFileUpload" name="Thumbnail0" runat="server" type="file" class="fileUploadWidth"  /> 
                    </td>
                </tr> 
                <tr style="display:none;">
                    <td class="titleTd">
                    <span>File Path:</span>
                    </td>                                       
                    <td colspan="2">
                        <span style="font-weight:bold;" id="labPath"></span>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="padding-top:20px;">
                        <h4 class="bar">File List</h4>         
                    </td> 
                </tr>
                <tr>
                    <td colspan="2"  class="titleTd">    
                        <span style="padding-right: 5px;">Directory</span>  
                        <select id="comDirectory">  
                        </select>   
                    </td>   
                    <td  style="padding-bottom: 10px; text-align:right;"> 
                        <input type="button" value="Edit" id="btnEdit" />
                        <input type="button" value="Publish" id="btnPublish" />
                        <input type="button" value="UnPublish" id="btnUnPublish" />
                        <input type="button" value="Trash" id="btnTrash" />
                    </td>         
                </tr>
                <tr>
                    <td class="titleTd" colspan=3>
                        <div id="userGridContainer" ></div>
                    </td>
                </tr>
            </table>
        </div>
        <div style="clear:both;"></div>
        <uc1:Footer runat="server" ID="Footer" />

    </div>
    </form>
</body>
</html>
