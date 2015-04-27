<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EventList.aspx.cs" Inherits="EventList" %>


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

            $txtDateFrom = $("#txtDateFrom");
            $txtDateTo = $("#txtDateTo");
            $txtName = $("#txtName");
            $btnGenerate = $("#btnGenerate");

            $txtSerialNo = $("#txtSerialNo");
            $tdType = $("#tdType");

            var trainingTypes;
            var getTrainingType = function () {
                var formData = {
                    action: "getSystemPara",
                    category: "TrainingType",
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

                        trainingTypes = data;
                        constuctTrainingTypeCheckboxes(trainingTypes);

                    }
                });
            }
            var constuctTrainingTypeCheckboxes = function (trainingTypes) {
                $.each(trainingTypes, function (key, value) {
                    $tdType.append('<input id="chkType_' + value.id + '" type="checkbox" value="'
                        + value.id + '" checked="checked"><label style="margin-right: 10px;" for="chkType_' + value.id + '">' + value.description + '</label>');
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

                if (types == "") types = "0";
                var serialNo = $txtSerialNo.val();

                window.open('TrainingReportDetails.aspx?serialNo='
                    + serialNo
                    + '&name=' + $txtName.val()
                    + '&type=' + types
                    + '&from=' + $txtDateFrom.val()
                    + '&to=' + $txtDateTo.val());

            });

            var init = function () {
                // getTrainingType();
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
            <p>Staff Portal > Event List</p> 
        </div>
        <div id="center" class="font12pt" style="margin-left: 20px;">
             <ul class="fullListItem">
                                <%=latestEvent.ToString()%>
                            </ul>
        </div>
        <div style="clear:both;"></div>
        <uc1:Footer runat="server" ID="Footer" />

    </div>
    </form>
</body>
</html>
