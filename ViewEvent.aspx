<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewEvent.aspx.cs" Inherits="ViewEvent" %>

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
            $txtDecision = $("#txtDecision");
            $btnJoin = $("#btnJoin");
            $btnNotAttend = $("#btnNotAttend");
            $eventID = $("#eventID");

            var metDeadline = '<%= metDeadline%>';
            if (metDeadline == 'True')
            {
                $btnJoin.hide();
                $btnNotAttend.hide();
            }

            var submit = function (action_log, action) {

                //get the form data
                var formData = {
                    activityID: $eventID.val(),
                    category: "Event",
                    action_log: action_log,
                    action: action
                };

                $.ajax({
                    url: "Service/AjaxService.aspx",
                    data: formData,
                    type: 'POST',
                    success: function (data) {
                        data = eval('(' + data + ')');

                        if (data.error) {
                            var decoded = $("<div/>").html(data.error).text();
                            alert(decoded);
                            return;
                        }

                        alert(data.message);
                        window.location = window.location;
                    }
                });

            } 
            $btnJoin.click(function () {
                submit("Join", "logActivity");
            });

            $btnNotAttend.click(function () {

                submit("NotAttend", "logActivity");
            });
            
        });
    </script>
</head>
<body>
    <form id="form1" runat="server"> 
        <uc1:MenuBar runat="server" ID="MenuBar" />
    <div id="content">
        <div class="clearLeftFloat"></div>
        <div id="navigationBar">
            <p>Staff Portal > Home> Event</p> 
        </div>  

             
        <div id="center">
    <div class="font12pt" style=" position: relative; z-index:0;"> 
        
        <div style="height: 130px;width:200px; position: absolute;  right: 200px;top: 40px;">
            
                
            
        </div>
    <table id="tableContent" class="viewTable">
            

        <tr> 
            <td class="articleTitle" colspan=4><label name="txtName" runat="server" id="txtName"/>
            <asp:HiddenField ID="eventID" runat="server" /></td>
        </tr>     
        <tr>
            <td   colspan=4>
            <asp:Image ID="imgSummary" runat="server" style="max-width:750px;"  />
            </td>
        </tr>
        <tr>
            <td style="width:110px;">Date & Time :</td>
            <td style="width:200px;">
                
                <label name="dateStartDate" runat="server" id="dateStartDate" />&nbsp&nbsp
                <label name="dateStartTime" runat="server" id="dateStartTime" />
                 -
                <label name="dateEndDate" runat="server" id="dateEndDate"  style="display:none;" />
                <label name="dateStartTime" runat="server" id="dateEndTime"  />
            </td>
            
            <td style="width:110px;">Deadline :</td>
            <td ><label name="dateDeadline" runat="server" id="dateDeadline" /></td>

        </tr>
        <tr>
            <td >Contact Person :</td>
            <td>
                <label name="txtContactPerson" runat="server" id="txtContactPerson"  /></td>
            <td>Phone Number :</td>
            <td ><label name="txtPhoneNumber" runat="server" id="txtPhoneNumber"  /></td>
        </tr>
        <tr>
            <td >Location :</td>
            <td colspan=3>
                <label name="txtLocation" runat="server" id="txtLocation" /></td>
        </tr>
        <tr>
            <td >Department :</td>
            <td colspan=3>
                <label name="txtDepartment" runat="server" id="txtDepartment" /></td>
        </tr>
        <tr>
            <td >Event Details :</td>
            <td colspan=3><div name="txtEventDetails" runat="server" id="txtEventDetails" class="view normal"></div> </td>
        </tr>
        <tr>
            <td >Last Decision:</td>
            <td colspan=3><span id="txtDecision" runat="server"></span>
            </td>
        </tr>
        <tr>
            <td  style="padding-top:20px;" colspan=4>
                <input type="button" value="Join" id="btnJoin" class="buttonWidth"/> 
                <input type="button" value="Not Attend" id="btnNotAttend" class="buttonWidth"/>  
            </td>
        </tr>
    </table>
    
    </div>

    
        </div>
        <div style="clear:both;"></div>
        <uc1:Footer runat="server" ID="Footer" />


    </div>


    </form>
</body>
</html>
