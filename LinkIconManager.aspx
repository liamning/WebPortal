<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LinkIconManager.aspx.cs" Inherits="LinkIconManager" %>
 

<%@ Register Src="~/Control/MenuBar.ascx" TagPrefix="uc1" TagName="MenuBar" %>
<%@ Register Src="~/Control/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>
<%@ Register Src="~/Control/PublicHeader.ascx" TagPrefix="uc1" TagName="PublicHeader" %> 




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
 
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <uc1:PublicHeader runat="server" ID="PublicHeader" />
    <script type="text/javascript">

      
        $(function () {

            $userGridContainer = $("#userGridContainer");
            $form = $("form");
            $btnPublish = $("#btnPublish");
            $btnUnPublish = $("#btnUnPublish");
            $btnAdd = $("#btnAdd");
            $btnUpdate = $("#btnUpdate");
            $btnEdit = $("#btnEdit");
            $btnTrash = $("#btnTrash");
            $btnPostBack = $("#btnPostBack");
            $txtSequenceNo = $("#txtSequenceNo");
            $txtIconName = $("#txtIconName");
            $txtLink = $("#txtLink");
            $txtIconUpload = $("#LinkIcon0");

            var published = "<%=GlobalSetting.ArticleStatus.Published%>";
            var unpublished = "<%=GlobalSetting.ArticleStatus.Unpublished%>";



            var requiredMsgSuffix = "required.";
            var mandatoryControl = [
                //{ $control: $txtIconName, requiredMsg: "Icon name " + requiredMsgSuffix },
                { $control: $txtSequenceNo, requiredMsg: "Sequence No. " + requiredMsgSuffix },
                { $control: $txtLink, requiredMsg: "Link " + requiredMsgSuffix },
                { $control: $txtIconUpload, requiredMsg: "Icon " + requiredMsgSuffix }
            ];
            var mandatoryControl_update = [
                //{ $control: $txtIconName, requiredMsg: "Icon name " + requiredMsgSuffix },
                { $control: $txtSequenceNo, requiredMsg: "Sequence No. " + requiredMsgSuffix },
                { $control: $txtLink, requiredMsg: "Link " + requiredMsgSuffix }
            ];

            var userGrid = new function () {
                return new Grid({
                    parent: $userGridContainer[0],
                    checkbox: {
                        selectAll: true, width: 20, style: "background:#E5F0D4;", titleStyle: "text-align:left;"
                    },
                    width: 900,
                    fields: [
                    { name: "icon", title: "Icon", width: 50, style: "text-align: center;", titleStyle: "text-align: center;" },
                    { name: "IconName", title: "Icon Name", width: 140, style: "text-align: left;", titleStyle: "text-align: left;" },
                    { name: "SequenceNo", title: "Sequence No.", width: 110, style: "text-align: left;", titleStyle: "text-align: left;" },
                    { name: "Link", title: "Link", width: 410, style: "text-align: left;", titleStyle: "text-align: left;" },
                    { name: "Status", title: "Status", width: 100, style: "text-align: left;", titleStyle: "text-align: left;" }
                    ]
                });
            }
            var fillGrid = function () {

                $.ajax({
                    type: 'POST',
                    url: "Service/AjaxService.aspx",
                    data: {
                        action: "getAllIconJsonArray"
                    },
                    success: function (data) {
                        data = eval('(' + data + ')');

                        if (data.error) {
                            var decoded = $("<div/>").html(data.error).text();
                            alert(decoded);
                            return;
                        }
                        $.each(data, function (key, value) {
                            value.icon = '<img src="Service/ImageHandler.ashx?ID=' + value.ImageID + '&timestamp='+ new Date().getMilliseconds() +'"/>';
                        })
                        
                        userGrid.setData(data);
                        //window.refreshSystemLink();
                    }
                });
            }
            var submit = function (formData) {


                $("input[type=hidden].appendFields").remove();
                  
                for (var pro in formData)
                { 
                    $('<input />').attr('type', 'hidden').attr('class', 'appendFields')
                            .attr('name', pro)
                            .attr('value', formData[pro])
                            .appendTo('#form1');
                }
                 
                $btnPostBack.click();
            }


            //init
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
                          fillGrid();
                      }
                  });
            (function () {

                //form submit init
               

                //event
                $btnPublish.click(function () {
                     
                    var dataSelected = userGrid.getSelectdData();
                    var IDs = [];
                    for (var i = 0, user; user = dataSelected[i]; i++) {
                        IDs.push(user.ID);
                    }
                    if (IDs.length == 0) return;


                    var formData = {
                        action: "updateLinkIconStaus",
                        Status: published,
                        IDs:IDs
                    };
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
                });
                $btnUnPublish.click(function () {
                     
                    var dataSelected = userGrid.getSelectdData();
                    var IDs = [];
                    for (var i = 0, user; user = dataSelected[i]; i++) {
                        IDs.push(user.ID);
                    }
                    if (IDs.length == 0) return;
                     
                    var formData = {
                        action: "updateLinkIconStaus",
                        Status: unpublished,
                        IDs: IDs
                    };
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
                });
                $btnAdd.click(function () {

                    if (!validate(mandatoryControl)) return;

                    var formData = {
                        action: "createLinkIcon"
                    };

                    submit(formData);
                });
                $btnUpdate.click(function () {
                     
                    var dataSelected = userGrid.getSelectdData();
                    var IDs = [];
                    for (var i = 0, user; user = dataSelected[i]; i++) {
                        IDs.push(user.ID); 
                    }
                    if (IDs.length == 0) return;

                    if (!validate(mandatoryControl_update)) return;

                    var formData = {
                        action: "updateLinkIcon",
                        ID: IDs[0]
                    };
                    submit(formData);
                });

                $btnEdit.click(function () {
                    var dataSelected = userGrid.getSelectdData();
                     
                    if (dataSelected.length == 0) return;
                    $txtIconName.val(dataSelected[0].IconName);
                    $txtLink.val(dataSelected[0].Link);
                    $txtSequenceNo.val(dataSelected[0].SequenceNo);

                });

                $btnTrash.click(function () {


                    var dataSelected = userGrid.getSelectdData();
                    var IDs = [];
                    for (var i = 0, user; user = dataSelected[i]; i++) {
                        IDs.push(user.ID); 
                    }
                    if (IDs.length != 0) {

                        var formData = {
                            action: "deleteLinkIcon",
                            ID: IDs
                        };
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

                });
                
                fillGrid();

            })();

        });

    </script>

    
    <title></title>  
</head>

<body>
    <form id="form1" runat="server">
        <uc1:MenuBar runat="server" ID="MenuBar" />
    <div id="content">
        <div class="clearLeftFloat"></div>
        <div id="navigationBar">
            <p>Staff Portal > Link Icon Manager</p> 
        </div>
        <div id="center" class="font12pt">
            <table style="min-width: 80%;">
                <tr>
                    <td colspan="5">
                        <h4 class="bar">Link Icon Manager</h4>         
                    </td> 
                </tr>   
                
                <tr>
                    <td class="titleTd"> Icon Name
                    </td>
                    <td  >   
                        <input type="text" id="txtIconName" name="txtIconName" style="width: 100%;" />
                    </td>
                    <td style="padding-left:30px; width: 120px;">   
                       
                    <span>Sequence No.</span>
                    </td>                                       
                    <td >
                        <input type="text" name="txtSequenceNo" validate="number" id="txtSequenceNo"   style="width: 100%;" /> 
                    </td>  
                </tr>
                 
                <tr>
                    <td class="titleTd" style="width: 100px;">   
                        Link
                    </td>
                    <td colspan="3" >   
                        <input type="text" id="txtLink" name="txtLink"  style="width: 100%;"/>
                    </td>
                    <td style="width: 280px;"> </td>
                </tr>
                 
                
                <tr runat="server" >
                    <td class="titleTd">Upload Icon</td> 
                    <td colspan="2" > 
                    <input id="LinkIcon0" name="LinkIcon0" runat="server" type="file" class="fileUploadWidth" accept="image/*"   />  
                       </td>
                    <td colspan="2" class="titleTd" > 
                    </td>
                     
                </tr> 
                
                
                <tr runat="server" >
                    <td class="titleTd"> </td> 
                    <td colspan="2" >    
                       </td>
                    <td colspan="2" class="titleTd" style="padding-left: 0px;" >    
                        <div style="float: right;" >
                        <input type="button" value="Publish" id="btnPublish" />
                        <input type="button" value="UnPublish" id="btnUnPublish" />
                        <input type="button" value="Add" id="btnAdd" />
                        <input type="button" value="Edit" id="btnEdit" />
                        <input type="button" value="Update" id="btnUpdate" />
                        <input type="button" value="Trash" id="btnTrash" />
                            
                        <asp:Button ID="btnPostBack" runat="server" style="display:none;" PostBackUrl="~/Service/AjaxService.aspx" />
                        </div>
                    </td>
                     
                </tr> 
                
                <tr>
                    <td class="titleTd" colspan="5">
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
