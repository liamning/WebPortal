<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EventReport.aspx.cs" Inherits="EventReport" %>
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

            $txtName = $("#txtName");
            $txtDateFrom = $("#txtDateFrom");
            $txtDateTo = $("#txtDateTo"); 
            $btnGenerate = $("#btnGenerate");

            $txtSerialNo0 = $("#txtSerialNo0");
            $txtSerialNo1 = $("#txtSerialNo1");
            $txtSerialNo2 = $("#txtSerialNo2");
            $tdType = $("#tdType");
            $tdDecision = $("#tdDecision");


            var eventTypes;
            var getEventType = function () {
                var formData = {
                    action: "getSystemPara",
                    category: "EventType",
                    includeabandon: true
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

                        eventTypes = data;
                        constuctEventTypeCheckboxes(eventTypes);

                    }
                });
            }
            var constuctEventTypeCheckboxes = function (eventTypes) {
                $.each(eventTypes, function (key, value) {
                    $tdType.append('<input id="chkType_' + value.id + '" type="checkbox" value="'
                        + value.id + '" checked="checked" ><label style="margin-right: 10px;" for="chkType_' + value.id + '">' + value.description + '</label>');
                });
            }
             
            $btnGenerate.click(function () {

                var types = "";
                $tdType.find(":checkbox").each(function () {
                    if ($(this).is(':checked')) {
                        if (types == "") {
                            types = $(this).val();
                        } else {
                            types = types + "," + $(this).val();
                        }
                    }

                });
                var decision = "";
                $tdDecision.find(":checkbox").each(function () {
                    if ($(this).is(':checked')) {
                        if (decision == "") {
                            decision = "'" + $(this).val() + "'";
                        } else {
                            decision = decision + "," + "'" + $(this).val() + "'";
                        }
                    }

                });

                if (types == "") types = "0";
                if (decision == "") decision = "'0'";
                var serialNo = $txtSerialNo0.val() + "-" + $txtSerialNo1.val() + "-" + $txtSerialNo2.val();
                if ($txtSerialNo0.val() == "0" || $txtSerialNo1.val() == "" || $txtSerialNo2.val() == "")
                    serialNo = "";

                window.open('EventReportDetails.aspx?serialNo='
                    + serialNo
                    + '&name=' + $txtName.val()
                    + '&type=' + types
                    + '&decision=' + decision
                    + '&from=' + $txtDateFrom.val()
                    + '&to=' + $txtDateTo.val());

            });
               
            var init = function () {
                getEventType();
            }();
  
        });

    </script>
</head>

<body>
    <form id="form1" runat="server">
        <uc1:MenuBar runat="server" ID="MenuBar" />
    <div id="content">
        <div class="clearLeftFloat"></div>
        <div id="navigationBar">
            <p>Staff Portal > Event Report</p> 
        </div>
        <div id="center" class="font12pt">
            <table style="min-width: 80%;">
                <tr>
                    <td colspan="2">
                        <h4 class="bar">Event Report</h4>         
                    </td> 
                </tr>  
                
                
                <tr runat="server" id="trType">
                    <td class="titleTd"><span>Type</span></td> 
                    <td id="tdType"> 
                    </td>
                     
                </tr>
                <tr runat="server" id="trSerial">
                    <td class="titleTd" style="width: 100px;">   
                        Serial No.
                    </td>
                    <td> 
                        <asp:DropDownList ID="txtSerialNo0" runat="server"></asp:DropDownList>
                        <span>-</span>
                        <input type="text" name="txtTitle" id="txtSerialNo1" runat="server" style="width:100px;text-transform:uppercase;"   />
                        <span>-</span>
                        <input type="text" name="txtTitle" validate="number" id="txtSerialNo2" runat="server" style="width:50px;" />
                   
                    </td>
                </tr>

                <tr>
                    <td class="titleTd" style="width: 100px;">   
                        Event Name
                    </td>
                    <td>   
                        <input type="text" id="txtName" style="width: 400px;"/>
                    </td>
                </tr>
                
                <tr>
                    <td class="titleTd" style="width: 100px;">   
                        Date Range
                    </td>
                    <td><input type="text" id="txtDateFrom" validate="date" style="width:100px;"/> To
                        <input type="text" id="txtDateTo" validate="date"  style="width:100px;"/>
                    </td>
                </tr>
                
                <tr runat="server" id="trDecision">
                    <td class="titleTd"><span>Decision</span></td> 
                    <td id="tdDecision">   
                        <label><input type="checkbox" value="Join" checked="checked" />Join</label>
                        <label><input type="checkbox" value="NotAttend"  checked="checked"  />Not Attend</label>
                    </td>
                     
                </tr>
                <tr>
                    <td class="titleTd" style="width: 100px;">   
                        <input type="button" id="btnGenerate" value="Generate">
                    </td>
                    <td>    
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

