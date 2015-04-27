<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewCareer.aspx.cs" Inherits="NewCareer" %>
<%@ Register Src="~/Control/MenuBar.ascx" TagPrefix="uc1" TagName="MenuBar" %>
<%@ Register Src="~/Control/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>
<%@ Register Src="~/Control/PublicHeader.ascx" TagPrefix="uc1" TagName="PublicHeader" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title>
    <uc1:PublicHeader runat="server" ID="PublicHeader" />
    <script type="text/javascript" src="Resource/ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        $(function () {
            var mode = "<%= mode%>";
            $btnSave = $("#btnSave");
            $btnUpdate = $("#btnUpdate");
            $btnPublish = $("#btnPublish");
            $comType = $("#comType");
            $txtSerialNo0 = $("#txtSerialNo0");
            $txtSerialNo1 = $("#txtSerialNo1");
            $txtSerialNo2 = $("#txtSerialNo2");
            $txtCareerLevel = $("#txtCareerLevel");
            $txtExp = $("#txtExp");
            $comQualification = $("#comQualification");
            $txtJobFunction = $("#txtJobFunction");
            $txtDivision = $("#txtDivision");
            $txtDepartment = $("#txtDepartment");
            $txtLocation = $("#txtLocation");
            $txtType = $("#txtType");
            $txtEmail = $("#txtEmail");
            $txtDisclaimer = $("#txtDisclaimer");
            $hdfID = $("#hdfID");
            $txtVersionNo = $("#txtVersionNo");
            $txtIconExpiryDay = $("#txtIconExpiryDay");
            var requiredMsgSuffix = "required.";
            var mandatoryControl = [
                { $control: $txtSerialNo1, requiredMsg: "Serial No. " + requiredMsgSuffix },
                { $control: $txtSerialNo2, requiredMsg: "Serial No. " + requiredMsgSuffix },
                //{ $control: $txtCareerLevel, requiredMsg: "Career Level " + requiredMsgSuffix },
                { $control: $txtExp, requiredMsg: "Yr(s) of Exp " + requiredMsgSuffix },
                { $control: $txtJobFunction, requiredMsg: "Position " + requiredMsgSuffix },
                { $control: $txtType, requiredMsg: "Employment Type " + requiredMsgSuffix },
                { $control: $txtDivision, requiredMsg: "Division " + requiredMsgSuffix },
                { $control: $txtDepartment, requiredMsg: "Department " + requiredMsgSuffix },
                { $control: $txtLocation, requiredMsg: "Location " + requiredMsgSuffix },
                { $control: $txtEmail, requiredMsg: "Email Address " + requiredMsgSuffix },
                { $control: $txtDisclaimer, requiredMsg: "Disclaimer " + requiredMsgSuffix }
            ];

            var careerTypeSerialList;
            var getCareerTypeSerial = function () {
                var formData = {
                    action: "getSystemPara",
                    category: "CareerType",
                    includeabandon: true
                };
                $.ajax({
                    url: "Service/AjaxService.aspx",
                    data: formData,
                    type: 'POST',
                    success: function (data) {
                        data = eval('(' + data + ')');
                        careerTypeSerialList = data;
                        if (data.error) {
                            var decoded = $("<div/>").html(data.error).text();
                            alert(decoded);
                            return;
                        }

                        initSerialNo0();
                    }
                });
            }
            var initSerialNo0 = function () {
                var type = $comType.val();
                $.each(careerTypeSerialList, function (index, value) {
                    if (type == value.id) {
                        $txtSerialNo0.text(value.cdescription);
                        return false;
                    }
                });
            }
            var submit = function (action) {
                var validateDetails = function () {
                    if (CKEDITOR.instances.txtDetails.getData() == "") {
                        alert('Details ' + requiredMsgSuffix);
                        CKEDITOR.instances.txtDetails.focus();
                        return false;
                    }
                    return true;
                }
                if (!validate(mandatoryControl, validateDetails)) return;
                //get the form data
                var formData = {
                    ID: $hdfID.val(),
                    Type: $comType.val(),
                    VersionNo: $txtVersionNo.text(),
                    SerialNo: $txtSerialNo0.text() + "-" + $txtSerialNo1.val() + "-" + $txtSerialNo2.val(),
                    CareerLevel: $txtCareerLevel.val(),
                    Experience: $txtExp.val(),
                    Qualification: $comQualification.val(),
                    // Salary : $txtSalary.val(),
                    JobFunction: $txtJobFunction.val(),
                    Division: $txtDivision.val(),
                    Department: $txtDepartment.val(),
                    Location: $txtLocation.val(),
                    EmploymentType: $txtType.val(),
                    Email: $txtEmail.val(),
                    Details: CKEDITOR.instances.txtDetails.getData(),
                    Disclaimer: $txtDisclaimer.val(),
                    IconExpiryDay: $txtIconExpiryDay.val(),
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
                        if (mode == "Edit")
                            window.location = window.location;
                    }
                });
            }
            var init = function () {
                if (mode == "Edit") {
                    $btnUpdate.click(function () {
                        submit("updateCareer");
                    });
                    $btnPublish.css("display", "none");
                    $btnSave.css("display", "none");
                } else {
                    $btnSave.click(function () {
                        submit("saveCareer");
                    });
                    $btnPublish.click(function () {
                        submit("publishCareer");
                    });
                    $btnUpdate.css("display", "none");
                }

                $comType.change(initSerialNo0);
                getCareerTypeSerial();
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
            <p>Staff Portal > Article Manager> <%=mode %> Career</p> 
        </div> 
             
        <div id="center">
    <div id="newArt" class="font12pt"> 
    <table id="tableContent">
        <tr>
            <td colspan=4>
                <h4 class="bar">Career</h4> 
            </td>
        </tr>
       
        <tr>
            <td class="titleTd"><span>Version No.</span></td> 
            <td colspan="3">
                <span id="txtVersionNo" runat="server"></span>
            </td>
        </tr>
        <tr>
            <td class="titleTd"><span>Type</span></td> 
            <td> 
                <select id="comType" runat="server" style="margin-left:0px;"> 
                </select>
            </td>
            <td  class="editorWidth_short" style=" text-align:right; padding-right: 20px;"><span>Serial No.</span></td> 
            <td>
                <span id="txtSerialNo0" runat="server" ></span>-
                <input type="text" runat="server" id="txtSerialNo1" style="width:100px;text-transform:uppercase;" />
                <span>-</span>
                <input type="text" runat="server" validate="number" id="txtSerialNo2" style="width:50px;" maxlength="100" />
            </td>
        </tr>
        <tr> 
  
            <td class="titleTd">Education Level</td>
            <td colspan=1>  
            
                    <input style="display:none;" type="text" name="txtCareerLevel" runat="server" id="txtCareerLevel" class="editorWidth_short" />
                    <asp:HiddenField ID="hdfID" runat="server" />               
                        <select id="comQualification" runat="server" style="margin-left:0px;">
                        </select>
            </td>           
            <td class="editorWidth_short" style=" text-align:right; padding-right: 10px;">Yr(s) of Exp</td>
            <td colspan=1><input type="text" name="txtExp" validate="year" runat="server" id="txtExp" class="editorWidth_short" /></td>
        </tr>  
        <tr>
            <td class="titleTd">Position</td>
            <td colspan=1>
                       
                        <select id="txtJobFunction" runat="server" style="margin-left:0px;">
                        </select>
                
            </td>
            <td class="editorWidth_short" style=" text-align:right; padding-right: 10px;">Employment Type</td>
            <td colspan=1>
            
                        <select id="txtType" runat="server" style="margin-left:0px;">
                        </select>          
            
            </td>
        </tr>
        <tr>
            <td class="titleTd">Division</td>
            <td colspan=1>
            
                        <select id="txtDivision" runat="server" style="margin-left:0px;">
                        </select>
                        
                
            </td>
            <td class="editorWidth_short" style=" text-align:right; padding-right: 10px;">Department</td>
            <td colspan=1>
            
                        <select id="txtDepartment" runat="server" style="margin-left:0px;">
                        </select>
            
            </td>
        </tr>
        <tr>
            <td class="titleTd">Location</td>
            <td colspan=3>
                <input type="text" name="txtLocation" runat="server" id="txtLocation" class="editorWidth" /></td>
        </tr>  
        <tr>
            <td class="titleTd">Email Address</td>
            <td colspan=3>
                <input type="text" name="txtEmail" runat="server" id="txtEmail" class="fileUploadWidth" /></td>
        </tr>
        <tr>
            <td class="titleTd">Icon Expiry Date</td>
            <td class="editorWidth_short">
                <input validate="date" type="text" name="txtIconExpiryDay" runat="server" id="txtIconExpiryDay" class="dateWidth" />
            </td> 
        </tr> 
        <tr>
            <td class="titleTd">Details</td>
            <td colspan="3">
            <textarea name="txtDetails" id="txtDetails" runat="server" class="editorWidth editorHeight1"></textarea>
            <script>
                CKEDITOR.replace('txtDetails');
            </script>
            </td>
        </tr>
        <tr>
            <td class="titleTd" style="font-weight:bold;">Disclaimer :</td>
            <td colspan="3"> 
                <textarea id="txtDisclaimer" runat="server" class="editorWidth editorHeight1"></textarea>
            </td>
        </tr>
    </table>
         
    <table> 
        <tr>
            <td class="titleTd" style="padding-top:20px;" colspan=4>
                <input type="button" value="Save" id="btnSave" class="buttonWidth" />
                <input type="button" value="Update" id="btnUpdate" class="buttonWidth"/>
                <input type="button" value="Publish" id="btnPublish" class="buttonWidth" />
                <input type="button" value="Discard" class="buttonWidth" onclick="if(confirm('Please confirm to discard the career.')) window.close();" />
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