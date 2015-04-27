<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SystemLinkManager.aspx.cs" Inherits="SystemLinkManager" %>

<%@ Register Src="~/Control/MenuBar.ascx" TagPrefix="uc1" TagName="MenuBar" %>
<%@ Register Src="~/Control/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>
<%@ Register Src="~/Control/PublicHeader.ascx" TagPrefix="uc1" TagName="PublicHeader" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
 
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title> 
    <uc1:PublicHeader runat="server" ID="PublicHeader" />
    <script type="text/javascript">
        $(function () {
            //init the user grid
            $userGridContainer = $("#userGridContainer");
            $btnAdd = $("#btnAdd");
            $btnEdit = $("#btnEdit");
            $btnUpdate = $("#btnUpdate");
            $btnTrash = $("#btnTrash");

            $txtName = $("#txtName");
            $txtLink = $("#txtLink");

            var requiredMsgSuffix = "required.";
            var mandatoryControl = [
                { $control: $txtName, requiredMsg: "Name " + requiredMsgSuffix },
                { $control: $txtLink, requiredMsg: "Link " + requiredMsgSuffix }
            ];

            var userGrid = new function () {
                return new Grid({
                    parent: $userGridContainer[0],
                    checkbox: {
                        selectAll: true, width: 20, style: "background:#E5F0D4;", titleStyle: "text-align:left;"
                    },
                    width: 880,
                    fields: [
                    { name: "name", title: "System Name", width: 210, style: "text-align: left;", titleStyle: "text-align: left;" },
                    { name: "link", title: "Link", width: 610, style: "text-align: left;", titleStyle: "text-align: left;" }
                    ]
                });
            }
            var fillGrid = function () {

                $.ajax({
                    type: 'POST',
                    url: "Service/AjaxService.aspx",
                    data: {
                        action: "getSystemLinkList"
                    },
                    success: function (data) {
                        data = eval('(' + data + ')');

                        if (data.error) {
                            var decoded = $("<div/>").html(data.error).text();
                            alert(decoded);
                            return;
                        }

                        userGrid.setData(data); 
                        //window.refreshSystemLink();
                    }
                });
            }

            var udpateSystemLink = function (mode,ID) {
                var formData = {
                    Name: $txtName.val(),
                    Link: $txtLink.val()
                };
                if (mode == "delete") {
                    formData.IDs = ID;
                    formData.action = "deleteSystemLinks";
                } else if (mode == "update") {
                    formData.ID = ID;
                    formData.action = "updateSystemLink";
                }
                else {
                    formData.action = "saveSystemLink";
                }

                $.ajax({
                    type: 'POST',
                    url: "Service/AjaxService.aspx",
                    data: formData,
                    success: function (data) {
                        data = eval('(' + data + ')');

                        if (data.error) {
                            var decoded = $("<div/>").html(data.error).text();
                            alert(decoded);
                            return;
                        }

                        alert(data.message);
                        //userGrid.setData(data.records);
                        fillGrid();
                    }
                });
            }
            
            var init = function()
            {

                fillGrid();

                $btnAdd.click(function () {

                    if (!validate(mandatoryControl)) return;
                     
                    udpateSystemLink("create");

                });
                $btnEdit.click(function () {
                    var dataSelected = userGrid.getSelectdData(); 
                    if (dataSelected.length == 0) return;

                    userGrid.singleSelect();

                    $txtName.val(dataSelected[0].name);
                    $txtLink.val(dataSelected[0].link);

                });
                $btnUpdate.click(function () {
                     
                    if (!validate(mandatoryControl)) return;


                    var dataSelected = userGrid.getSelectdData();
                    var IDs = [];
                    for (var i = 0, user; user = dataSelected[i]; i++) {
                        IDs.push(user.id);
                        break;
                    }
                    if (IDs.length != 0) {  
                        udpateSystemLink("update",IDs[0]); 
                    }

                });

                $btnTrash.click(function () {

                    

                    var dataSelected = userGrid.getSelectdData();
                    var IDs = [];
                    for (var i = 0, user; user = dataSelected[i]; i++) {
                        IDs.push(user.id);
                    }

                    if (IDs.length == 0) return;
                    if (!confirm("Please confirm to trash this system link.")) return;

                    udpateSystemLink("delete",IDs);
                });
            }

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
            <p>Staff Portal > System Link Manager</p> 
        </div>
        <div id="center" class="font12pt">
            <table>
                <tr>
                    <td colspan="2">
                        <h4 class="bar">System Link Manager</h4>         
                    </td> 
                </tr> 
                <tr>
                    <td style="padding-left: 30px; width: 255px; padding-bottom: 10px;padding-top: 8px;"> 
                        Name : 
                        <input type="text" id="txtName" runat="server" style="width: 180px;"/> 
                    </td>
                    <td style="padding-bottom: 10px;padding-top: 8px;">
                        Link : 
                        <input type="text" id="txtLink" runat="server"  style="width: 400px;"/> 
                        <div style="float: right;">
                            
                        <input type="button" value="Add" id="btnAdd" />
                        <input type="button" value="Edit" id="btnEdit" />
                        <input type="button" value="Update" id="btnUpdate" />
                        <input type="button" value="Trash" id="btnTrash" />
                        </div>
                    </td> 
                </tr>
                <tr>
                    <td class="titleTd" colspan=2>
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
