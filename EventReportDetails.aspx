<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EventReportDetails.aspx.cs" Inherits="EventReportDetails" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title>
    
    <style>
        body {
            font-family: Arial;
        }

        ul,li{
            padding-left: 0px;
        }

        table.ReportTable
        {
            border-collapse: collapse;
        }

        table.ReportTable td,table.ReportTable th
        {
            border: solid 1px #D4D4D4;
            padding: 3px 8px 3px 8px;
        }


        li {
            list-style-type: none!important;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h3>Event Report</h3>

        
        <ul>
            <li runat="server" id="liType">Type.: <span runat="server" id="txtType"></span></li>
            <li runat="server" id="liSerialNo">Serial No.: <span runat="server" id="txtSerialNo"></span></li>
            <li>Name: <span runat="server" id="txtName"></span></li>
            <li>Date Range: <span runat="server" id="txtDateRange"></span></li>
            <li>Decision: <span runat="server" id="txtDecision"></span></li> 
        </ul>

    <%=reportStr%>
    </div>
    </form>
</body>
</html>

