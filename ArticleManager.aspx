
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArticleManager.aspx.cs" Inherits="ArticleManager" %>

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
            $ddlFilter = $("#ddlFilter");
            $btnPublish = $("#btnPublish");
            $btnUnPublish = $("#btnUnPublish");
            $btnAdd = $("#btnAdd");
            $btnEdit = $("#btnEdit");
            $btnTrash = $("#btnTrash");

            var userGrid = new function () {
                return new Grid({
                    parent: $userGridContainer[0],
                    checkbox: { selectAll: true, width: 20, style: "background:#E5F0D4;", titleStyle: "text-align:left;" },
                    width: 880,
                    fields: [
                    { name: "serialno", title: "Serial No.", width: 110, style: "text-align: left;", titleStyle: "text-align: left;" },
                    { name: "title", title: "Title", width: 280, style: "text-align: left;", titleStyle: "text-align: left;" },
                    { name: "category", title: "Category", width: 90, style: "text-align: left;", titleStyle: "text-align: left;" },
                    { name: "type", title: "Type", width: 80, style: "text-align: left;", titleStyle: "text-align: left;" },
                    { name: "status", title: "Status", width: 80,style: "text-align: left;", titleStyle: "text-align: left;" },
                    { name: "effectivedate", title: "Date", width: 80, style: "text-align: left;", titleStyle: "text-align: left;" }
                    ]
                });
            }
            var fillUserGrid = function (category) {

                $.ajax({
                    type: 'POST',
                    url: "Service/AjaxService.aspx",
                    data: {
                        action: "getArticleByCategory",
                        category: category
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
            

            fillUserGrid("News");
            $ddlFilter.find('option').first().prop('selected', true);

            var udpateStatus = function (IDs, newStatus, category) {

                //start to update the status
                $.ajax({
                    type: 'POST',
                    url: "Service/AjaxService.aspx",
                    data: {
                        action: "updateArticleStatus",
                        IDs: IDs,
                        newStatus: newStatus,
                        category: category
                    },
                    success: function (data) {
                        data = eval('(' + data + ')');
                        if (data.error) {
                            var decoded = $("<div/>").html(data.error).text();
                            alert(decoded);
                            return;
                        }
                        alert(data.message);
                        if (data.result) {
                            fillUserGrid($ddlFilter.val()); 
                        } 
                    }
                });
            }

            $btnPublish.click(function () {
                var dataSelected = userGrid.getSelectdData();
                var IDs = [];
                for (var i = 0, user; user = dataSelected[i]; i++) {
                    IDs.push(user.id);
                }

                if (IDs.length == 0) return;

                udpateStatus(IDs, "<%=GlobalSetting.ArticleStatus.Published%>", $ddlFilter.val());

            });
            $btnUnPublish.click(function () {
                var dataSelected = userGrid.getSelectdData();
                var IDs = [];
                for (var i = 0, user; user = dataSelected[i]; i++) {
                    IDs.push(user.id);
                }

                if (IDs.length == 0) return;

                udpateStatus(IDs, "<%=GlobalSetting.ArticleStatus.Unpublished%>", $ddlFilter.val());

            });
            $btnAdd.click(function () {
                var category = $ddlFilter.val();
                var url;
                if (category == "News") {
                    url = "NewArticle.aspx"
                }
                else if (category == "Event") {
                    url = "NewEvent.aspx"
                }
                else if (category == "Training") {
                    url = "NewTraining.aspx"
                }
                else if (category == "Career") {
                    url = "NewCareer.aspx"
                } 
                var page = window.open(url, "New");
                page.focus();
            });
            $btnEdit.click(function () {
                var dataSelected = userGrid.getSelectdData();
                var IDs = [];
                for (var i = 0, user; user = dataSelected[i]; i++) {
                    IDs.push(user.id);
                    break;
                }
                if (IDs.length == 0) return;
                var category = $ddlFilter.val();
                var url;
                if (category == "News") {
                    url = "NewArticle.aspx";
                }
                else if (category == "Event") {
                    url = "NewEvent.aspx";
                }
                else if (category == "Training") {
                    url = "NewTraining.aspx";
                }
                else if (category == "Career") {
                    url = "NewCareer.aspx";
                }
                var page = window.open(url + "?ID=" + IDs[0], "New");
                page.focus();
            });
            $btnTrash.click(function () {
                var dataSelected = userGrid.getSelectdData();
                var IDs = [];
                for (var i = 0, user; user = dataSelected[i]; i++) {
                    IDs.push(user.id);
                }

                if (IDs.length == 0) return;
                if (!confirm("Please confirm to trash this article.")) return;
                 
                udpateStatus(IDs, "Trashed", $ddlFilter.val());
            });
            $ddlFilter.change(function () {
                fillUserGrid($(this).val());
            });

              

            /*alert($userGridContainer.find(".bodyDiv").height())
            var height = $userGridContainer.find(".bodyDiv").height() - 37;
            alert(height);
            $userGridContainer.find(".bodyDiv").css("height", height + "px");
            alert($("body").height());
            alert($(window).height());


            alert($("body").height() + 40 - $(window).height());
            */
        });
    </script>
</head>

<body>
    <form id="form1" runat="server">
        <uc1:MenuBar runat="server" ID="MenuBar" />
    <div id="content">
        <div class="clearLeftFloat"></div>
        <div id="navigationBar">
            <p>Staff Portal > Article Manager</p> 
        </div>
        <div id="center" class="font12pt">
            <table>
                <tr>
                    <td colspan="2">
                        <h4 class="bar">Article Manager</h4>         
                    </td> 
                </tr>
                <tr>
                    <td class="titleTd">
                        <label>Category</label>  <select id="ddlFilter" runat="server" style="width:100px;"> 
                        </select>            
                    </td>   
                    <td  style="padding-bottom: 10px; text-align:right;">
                        <input type="button" value="Publish" id="btnPublish" />
                        <input type="button" value="UnPublish" id="btnUnPublish" />
                        <input type="button" value="Add" id="btnAdd" />
                        <input type="button" value="Edit" id="btnEdit" />
                        <input type="button" value="Trash" id="btnTrash" /> 
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
