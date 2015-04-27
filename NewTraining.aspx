<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewTraining.aspx.cs" Inherits="NewTraining" %>

<%@ Register Src="~/Control/MenuBar.ascx" TagPrefix="uc1" TagName="MenuBar" %>
<%@ Register Src="~/Control/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>
<%@ Register Src="~/Control/PublicHeader.ascx" TagPrefix="uc1" TagName="PublicHeader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title> 
    <uc1:PublicHeader runat="server" ID="PublicHeader" />
    <script type="text/javascript" src="Resource/ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        $(function () {

            var mode = "<%= mode%>";
            var alertWhenUpdateTrainingSchedule = "<%= GlobalSetting.AlertMessage.AlertWhenUpdateTrainingSchedule%>";

            $form = $("form");
            $form.append("<input type='text' style='display:none;' id='txtAction' name='action' />");
            $form.append("<input type='text' style='display:none;' id='txtSerialNo' name='txtSerialNo' />");

            $action = $form.find("#txtAction");
            $txtSerialNo = $form.find("#txtSerialNo");

            $btnSave = $("#btnSave");
            $btnUpdate = $("#btnUpdate");
            $btnPublish = $("#btnPublish");
            $ScheButton = $(".ScheButton");

            $txtSerialNo0 = $("#txtSerialNo0");
            $txtSerialNo1 = $("#txtSerialNo1");
            $txtSerialNo2 = $("#txtSerialNo2");
            $comType = $("#comType");
            $txtName = $("#txtName");

            $scheduleDate = $("input[name=dateStartDate0]");
            $startTime = $("input[name=dateStartTime0]");
            $endTime = $("input[name=dateEndTime0]");
            $txtMaxAttendance = $("#txtMaxAttendance");
            $dateDeadline = $("#dateDeadline");
            $chkOptionalAttendance = $("#chkOptionalAttendance");

            $txtLocation = $("#txtLocation");
            $txtContactPerson = $("#txtContactPerson");
            $txtPhoneNumber = $("#txtPhoneNumber");
            $txtDepartment = $("#txtDepartment");
            $txtEmail = $("#txtEmail"); 
            $txtFormPath = $("#txtFormPath");
            $hdfID = $("#hdfID");
            $txtVersionNo = $("#txtVersionNo");
            $btnAddTrainingForm = $(".AddTrainingForm");
            $divTrainingForm = $("div.TrainingForm");
            $selectFormPath = $divTrainingForm.find("select.FormPath");
            $scheduleDiv = $("div.scheduleDiv");
            $txtIconExpiryDay = $("#txtIconExpiryDay");


            var originSchedule = [];
            var requiredMsgSuffix = "required.";
            var mandatoryControl = [
                { $control: $txtSerialNo1, requiredMsg: "Serial No. " + requiredMsgSuffix },
                { $control: $txtSerialNo2, requiredMsg: "Serial No. " + requiredMsgSuffix },
                { $control: $txtName, requiredMsg: "Training Course " + requiredMsgSuffix },
                { $control: $txtLocation, requiredMsg: "Location " + requiredMsgSuffix },
                { $control: $txtContactPerson, requiredMsg: "Contact Person " + requiredMsgSuffix },
                { $control: $txtDepartment, requiredMsg: "Department " + requiredMsgSuffix },
                { $control: $txtPhoneNumber, requiredMsg: "Phone Number " + requiredMsgSuffix },
                { $control: $dateDeadline, requiredMsg: "Deadline " + requiredMsgSuffix },
                { $control: $txtEmail, requiredMsg: "Email Address " + requiredMsgSuffix }
            ];


            var trainingTypeSerialList;
            var fillTrainingTypeSerialDesc = function () {
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
                        trainingTypeSerialList = data;

                        if (data.error) {
                            var decoded = $("<div/>").html(data.error).text();
                            alert(decoded);
                            return;
                        }

                        initSerial0()

                    }
                });
            }
            var fillTrainingSchedule = function () {
                var formData = {
                    action: "getTrainingSchedule",
                    ID: $hdfID.val()
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

                        otimeRange = data;
                        $scheduleDate.val(data[0].scheduleDate);
                        $startTime.val(data[0].startTime);
                        $endTime.val(data[0].endTime);

                        for (var sche, i = 1; sche = data[i]; i++) {
                            appendScheHTML($scheduleDiv, sche);
                        }


                        //get current training schedule
                        originSchedule = [];
                        $(".scheduleDiv").each(function (index) {
                            originSchedule.push({
                                StartTime: $(this).find("input[name='dateStartDate" + index + "']").val() + " " + $(this).find("input[name='dateStartTime" + index + "']").val() + ":00",
                                EndTime: $(this).find("input[name='dateStartDate" + index + "']").val() + " " + $(this).find("input[name='dateEndTime" + index + "']").val() + ":00"
                            });

                        });
                    }
                });
            }
            var fillTrainingForm = function () {
                var formData = {
                    action: "getTrainingFormList",
                    ID: $hdfID.val()
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
                         
                        
                        var i = 0;
                        if (data[i])
                        { 
                            $divTrainingForm.find("input[type=hidden][class=FormID]").val(data[i].ID);

                            $divTrainingForm.find("select").val(data[i].FormType).attr('disabled', 'disabled');
                            $divTrainingForm.find("input[type=text][class=FormPath]").css('width', '201px');
                            $divTrainingForm.find("input[type=text][class=Description]").css('display', 'none');
                            $divTrainingForm.find("input[type=file]").css('width', '205px').css('display', 'none');

                            $divTrainingForm.find("a.FormLink").css('display', '').attr('href', data[i].FormPath).text(data[i].Description);

                            $divTrainingForm.find("a.formButton").css('display', '').click(function () {
                                var $parent = $(this).parent().parent();
                                if ($(this).text() == "Change") {
                                    $parent.attr('formType', $parent.find("select").val());
                                    $(this).text("Cancel").css('padding-right', '4px');
                                    $parent.find("input[type=text][class=Description]").css('display', '');
                                    $parent.find("select").removeAttr('disabled');
                                    $parent.find("a.FormLink").css('display', 'none')
                                    $parent.find("select").prop('selectedIndex', 0);
                                    //$parent.find("select option:selected").index() = 0;
                                    $parent.find("input[type=file]").css('display', '');
                                    $parent.find("input[type=text][class=FormPath]").css('display', 'none');


                                    $parent.find("input[type][class=FormStatus]").val('updated');

                                    //alert($parent.find("input[type][class=FormID]").val());

                                } else {
                                    $(this).text("Change").css('padding-right', '0px');
                                    $parent.find("select").val($parent.attr('formType')).attr('disabled', 'disabled');
                                    $parent.find("input[type=text][class=Description]").css('display', 'none');
                                    $parent.find("a.FormLink").css('display', '')

                                    $parent.find("input[type=file]").css('display', 'none');
                                    $parent.find("input[type=text][class=FormPath]").css('display', 'none');

                                    $parent.find("input[type][class=FormStatus]").val('');

                                }
                            });

                            $divTrainingForm.find("a.delete").css('display', '').click(function () {
                                var $parent = $(this).parent().parent();
                                 
                                $parent.find("select").prop('selectedIndex', 0).removeAttr('disabled'); 
                                $parent.find("input[type=file]").css('display', ''); 
                                $parent.find("input[type=text][class=Description]").css('display', '');
                                $parent.find("input[type=text][class=FormPath]").css('display', 'none'); 

                                $parent.find("a").hide();

                                //store the deleted form ID
                                if (!window.trainingFormDeleted) {
                                    window.trainingFormDeleted = [];
                                }
                                window.trainingFormDeleted.push($parent.find("input[type=hidden][class=FormID]").val());
                                $parent.find("input[type=hidden][class=FormID]").val("0")
                               // alert(window.trainingFormDeleted);
                                
                            });
                              
                            window.grayOut($divTrainingForm);

                        }
                        i++;
                        for (var form; form = data[i]; i++) {
                            appendFormHTML($divTrainingForm, form);
                        }

                    }
                });
            }

            var appendScheHTML = function (target, sche) {
                $parent = target.parent();
                $newSche = target.clone();

                $newSche.find("input[type=button], input[type=checkbox], label").remove();
                $newSche.find("input[type=text]").val("");
                $newSche.append('<a href="javascript:;" class="delete" >Delete</a></div>');
                $newSche.css('padding-top', '2px');

                if (!window.scheduleCount) { window.scheduleCount = 1; }
                else { window.scheduleCount++; }

                $newSche.find('input[type=text][name=dateStartDate0]').attr('name', 'dateStartDate' + window.scheduleCount).val(sche.scheduleDate);
                $newSche.find('input[type=text][name=dateStartTime0]').attr('name', 'dateStartTime' + window.scheduleCount).val(sche.startTime);
                $newSche.find('input[type=text][name=dateEndTime0]').attr('name', 'dateEndTime' + window.scheduleCount).val(sche.endTime);

                $parent.append($newSche);
                /*
                $parent.append('<div class="scheduleDiv" style="padding-top:2px;"><input type="text" name="scheduleDate" validate="date"  class="dateWidth" value="' + sche.scheduleDate + '" />&nbsp'
                                  + '<input type="text" name="startTime"  class="timeWidth" validate="time" value="' + sche.startTime + '" />&nbsp'
                                  + '-&nbsp<input type="text" name="endTime"  class="timeWidth" validate="time" value="' + sche.endTime + '" />&nbsp'
                                  + '<a href="javascript:;" class="delete" >Delete</a></div>');*/

                refreshValidation($newSche);
                $newSche.find("a.delete").click(function () {
                     $(this).parent().remove(); 
                });
            }

            var appendFormHTML = function (target, form) {
                var $parent = target.parent(); 
                var $newElements = target.clone(true);

                var initTheNewFormInput = function () { 
                    $newElements.find("input[type=button]").css('visibility', 'hidden');
                    $newElements.find("*").removeAttr("id");
                    $newElements.find("input[type=hidden]").val(""); 
                }();

                var fillValueAndSetVisibility = function () { 
                    if (form) {
                        $newElements.find("input[type=hidden][class=FormID]").val(form.ID);

                        $newElements.find("select").val(form.FormType);
                        $newElements.find("input[type=text][class=Description]");
                        $newElements.find("input[type=text][class=FormPath]").hide().val("");
                        $newElements.find("a.FormLink").css('display', '').attr('href', form.FormPath).text(form.Description);
                    }
                    else {
                        $newElements.find("select").removeAttr('disabled');
                        $newElements.find("a").hide();
                        $newElements.find("input[type=file][class=FormPath]").show();
                        $newElements.find("input[type=text][class=FormPath]").hide().val("");
                        $newElements.find("input[type=text][class=Description]").show().val("");
                    }
                }();

                //rename the form name
                var renameFormName = function () {

                    if (!window.trainingFormCount) window.trainingFormCount = 1;
                    else window.trainingFormCount++;

                    $newElements.find("input[type=hidden][class=FormID]").attr('name', 'FormID' + window.trainingFormCount);
                    $newElements.find("input[type=hidden][class=FormStatus]").attr('name', 'FormStatus' + window.trainingFormCount);
                    $newElements.find("input[type=file][class=FormPath]").attr('name', 'TrainingForm' + window.trainingFormCount);
                    $newElements.find("select.FormPath").attr('name', 'FormType' + window.trainingFormCount);
                    $newElements.find("input[type=text][class=FormPath]").attr('name', 'FormPath' + window.trainingFormCount);
                    $newElements.find("input[type=text][class=Description]").attr('name', 'FormDescription' + window.trainingFormCount);
                }();

                //refresh the control with grayout function
                window.grayOut($newElements);

                //$newElements.find("span").append('');
                $parent.append($newElements);

                $newElements.find("a.delete").css("display", "").off('click').click(function () {

                    $div = $(this).parent().parent();
                    window.trainingFormCount--;

                    //store the delete form id
                    if(!window.trainingFormDeleted)
                    {
                        window.trainingFormDeleted = [];
                    }
                    var id = $div.find("input[type=hidden][class=FormID]").val();
                    if (id)
                        window.trainingFormDeleted.push(id);
                   // alert(window.trainingFormDeleted);

                    //remove the application form div
                    $div.remove();


                });
            }

            var initSerial0 = function () {
                var type = $comType.val();

                $.each(trainingTypeSerialList, function (index, value) {
                    if (type == value.id) {
                        $txtSerialNo0.text(value.cdescription);
                        return false;
                    }
                });
            }

            var scheduleChanged = function (formData) {

                var newSchedule = formData.Schedule;
                var isChanged = false;
                if (originSchedule.length == newSchedule.length) {
                    for (var i = 0, otimeRange, ntimeRange; otimeRange = originSchedule[i], ntimeRange = newSchedule[i]; i++) {
                        for (var ppty in otimeRange) {
                            if (otimeRange[ppty] != ntimeRange[ppty]) {
                                isChanged = true;
                                break;
                            }
                        }

                        if (isChanged) break;
                    }
                }
                else {
                    isChanged = true;
                }
                 
                formData.ResetAttendance = false;
                if (isChanged)
                {
                    if (confirm(alertWhenUpdateTrainingSchedule))
                    {
                        var serialNo = $txtSerialNo0.text() + "-" + $txtSerialNo1.val() + "-" + $txtSerialNo2.val();
                        window.open('http://localhost:81/test/TrainingReportDetails.aspx?serialNo=' + serialNo + '');
                        formData.ResetAttendance = true;
                        return true;
                    }
                    else
                    { 
                        return false;
                    }
                }

                return true;
            }

            var defineEventDelegate = function ()
            {
                $ScheButton.click(function () {

                    var value = $(this).val();
                    if (value == "Add") {
                        appendScheHTML($scheduleDiv, {
                            scheduleDate: "",
                            startTime: "",
                            endTime: ""
                        });
                    }

                });

                $comType.change(function () {
                    initSerial0();
                  
                });

                $btnAddTrainingForm.click(function () {
                    appendFormHTML($divTrainingForm, undefined);
                });

                $selectFormPath.change(function () {

                    $parent = $(this).parent();
                    var $trainingFormUpload = $parent.find("input[type=file][class=FormPath]");
                    if ($trainingFormUpload.css('display') == 'none') {
                        $trainingFormUpload.css('display', '');
                        $trainingFormUpload.attr('name', $trainingFormUpload.attr('name').replace('hidden', ''));
                        $parent.find("input[type=text][class=FormPath]").css('display', 'none');
                    } else {
                        $trainingFormUpload.css('display', 'none');
                        $trainingFormUpload.attr('name', $trainingFormUpload.attr('name') + "hidden");
                        $parent.find("input[type=text][class=FormPath]").css('display', '');
                    }
                     

                });

                if (mode == "Edit") {
                    $btnUpdate.click(function () {
                        submit("updateTraining");
                    }); 

                } else {
                    $btnSave.click(function () {
                        submit("saveTraining");
                    });
                    $btnPublish.click(function () {

                        submit("publishTraining");
                    }); 
                }

                //use iframe to post form
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

                        if (mode == "Edit")
                            window.location = window.location;
                    }
                });

            }

            var submit = function (action) {

                var allMandatoryControl = [];
                for (var i = 0, item; item = mandatoryControl[i]; i++)
                {
                    allMandatoryControl.push(item);
                }
                $(".scheduleDiv").find('input[type="text"]').each(function () {
                    if ($(this).val() == "") {
                        allMandatoryControl.push({
                            $control: $(this),
                            requiredMsg: "Date & Time " + requiredMsgSuffix
                        });
                    }
                });

                if (!validate(allMandatoryControl,
                    function () {

                    if (CKEDITOR.instances.txtDetails.getData() == "") {
                            alert('Details ' + requiredMsgSuffix);
                            CKEDITOR.instances.txtDetails.focus();
                            return false;
                }
                    return true;

                })) return;


                //get the form data
                var formData = {
                    ID: $hdfID.val(),
                    VersionNo: $txtVersionNo.text(),
                    SerialNo: $txtSerialNo0.text() + "-" + $txtSerialNo1.val() + "-" + $txtSerialNo2.val(),
                    Type: $comType.val(),
                    Name: $txtName.val(),
                    Schedule: [], 
                    OptionalAttendance: $chkOptionalAttendance.is(':checked'),
                    MaxAttendance: $txtMaxAttendance.val(),
                    Deadline: $dateDeadline.val(),
                    Location: $txtLocation.val(),
                    ContactPerson: $txtContactPerson.val(),
                    PhoneNumber: $txtPhoneNumber.val(),
                    Department: $txtDepartment.val(),
                    Email: $txtEmail.val(),
                    Details: CKEDITOR.instances.txtDetails.getData(),
                    FormPath: $txtFormPath.val(),
                    IconExpiryDay: $txtIconExpiryDay.val(),
                    action: action
                };

                $(".scheduleDiv").each(function (index) {
                    formData.Schedule.push({
                        StartTime: $(this).find("input[name='dateStartDate" + index + "'").val() + " " + $(this).find("input[name='dateStartTime" + index + "'").val() + ":00",
                        EndTime: $(this).find("input[name='dateStartDate" + index + "'").val() + " " + $(this).find("input[name='dateEndTime" + index + "'").val() + ":00"
                    });

                });

                formData.TrainingForms = [];
                $(".TrainingForm").each(function (index, value) {
                    $this = $(value); 
                    formData.TrainingForms.push({
                        FormType: $this.find("select").val(),
                        FormPath: $this.find("input[type=text][class=FormPath]").val(),
                        Description: $this.find("input[type=text][class=Description]").val()
                    });

                }); 

                if (action == "updateTraining")
                {
                    if (!scheduleChanged(formData)) {
                        return;
                    }
                } 

                $action.val(action);
                $txtSerialNo.val($txtSerialNo0.text() + "-" + $txtSerialNo1.val() + "-" + $txtSerialNo2.val());


                $("input[type=hidden].appendFields").remove();
                $('<input />').attr('type', 'hidden').attr('class', 'appendFields')
                        .attr('name', "TrainingFormDeleted")
                        .attr('value', window.trainingFormDeleted)
                        .appendTo('#form1');

                $('<input />').attr('type', 'hidden').attr('class', 'appendFields')
                        .attr('name', "ResetAttendance")
                        .attr('value', formData.ResetAttendance)
                        .appendTo('#form1');
                 
                $("#btnURL").click();

            }

            var init = function () {
                //page init
                if (mode == "Edit") {
                    $btnPublish.css("display", "none");
                    $btnSave.css("display", "none");
                    fillTrainingSchedule();
                    fillTrainingForm();

                } else {
                    $btnUpdate.css("display", "none");
                }

                defineEventDelegate();
                fillTrainingTypeSerialDesc();
                
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
            <p>Staff Portal > Article Manager> <%=mode %> Training</p> 
        </div>  

             
        <div id="center">

    <div id="newArt" class="font12pt"> 
    <table id="tableContent">
        <tr>
            <td colspan=4>
                <h4 class="bar">Training</h4> 
            </td>
        </tr>

                <tr>
                    <td class="titleTd"><span>Version No.</span></td> 
                    <td colspan="3">
                        <span id="txtVersionNo" runat="server"></span>
                        <input type="hidden" id="hdfVersionNo" runat="server" />
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
                        <span id="txtSerialNo0" runat="server"></span>
                        <span>-</span>
                        <input type="text" name="txtTitle" id="txtSerialNo1" runat="server" style="width:100px;text-transform:uppercase;"   />
                        <span>-</span>
                        <input type="text" name="txtTitle" validate="number" id="txtSerialNo2" runat="server" style="width:50px;" />
                    </td>
                </tr>
        <tr>
            <td class="titleTd">Training Course</td>
            <td colspan=3><input type="text" name="txtName" runat="server" id="txtName" class="editorWidth" />
                <asp:HiddenField ID="hdfID" runat="server" /></td>
        </tr>
        <tr>
            <td class="titleTd">Date & Time</td>
            <td colspan=3 > 
                <div class="scheduleDiv">
                 
                        <input type="text" validate="date" name="dateStartDate0" class="dateWidth" />
                        <input type="text" validate="time" name="dateStartTime0" class="timeWidth" />
                        -
                        <input type="text" validate="time" name="dateEndTime0" class="timeWidth" />
                    <input type="button" class="ScheButton" value="Add" style="width:80px"/>
                    <label><input runat="server" type="checkbox" id="chkOptionalAttendance" style="margin-left: 80px;margin-bottom:0px;"/>Optional</label>
                </div> 
                
            </td>
        </tr>
        <tr>
            <td class="titleTd">Maximum Attendance</td>
            <td >
                <input type="text" validate="number" runat="server" id="txtMaxAttendance" class="timeWidth"  /></td>
            <td class="editorWidth_short" style=" text-align:right; padding-right: 10px;">Deadline</td>
                    <td >
                        <input validate="date" type="text" name="Deadline" runat="server" id="dateDeadline" class="dateWidth" />
                        </td>
        </tr>
        <tr>
            <td class="titleTd">Location</td>
            <td colspan=3>
                <input type="text" runat="server" id="txtLocation" class="editorWidth" /></td>
        </tr>
        <tr>
            <td class="titleTd">Contact Person</td>
            <td class="editorWidth_short">
                <input type="text" name="txtContactPerson" runat="server" id="txtContactPerson" class="editorWidth_short" /></td>
            <td class="editorWidth_short" style=" text-align:right; padding-right: 10px;">Department</td>
            <td ><input type="text" name="txtDepartment" runat="server" id="txtDepartment" class="editorWidth_short" /></td>
        </tr>
        <tr>
            <td class="titleTd">Phone Number</td>
            <td class="editorWidth_short">
                <input type="text" name="txtPhoneNumber" validate="phone"  runat="server" id="txtPhoneNumber" class="editorWidth_short" /></td>
            <td class="editorWidth_short" style=" text-align:right; padding-right: 10px;">
                Email Address
            </td>
            <td >
                
                <input type="text" name="txtEmail"  runat="server" id="txtEmail" class="editorWidth_short" />
            </td>
        </tr> 
        <tr>
            <td class="titleTd">Icon Expiry Date</td>
            <td class="editorWidth_short">
                <input validate="date" type="text" name="txtIconExpiryDay" runat="server" id="txtIconExpiryDay" class="dateWidth" />
            </td> 
        </tr> 
        <tr>
            <td class="titleTd">Details</td>
            <td colspan=3><textarea name="txtDetails" runat="server" id="txtDetails" class="editorWidth editorHeight1"></textarea>
                     <script>
                         CKEDITOR.replace('txtDetails');
                    </script>
            </td>
        </tr>
        <tr>
            <td class="titleTd">Application Form</td>
            <td colspan=3>
                <div class="TrainingForm" style="margin-bottom: 3px; min-height:22px;">
                    <select class="FormPath" name="FormType0" >
                        <%=formType.ToString() %>
                    </select>
                    <input type="hidden" class="FormID" name="FormID0"/>
                    <input type="hidden" class="FormStatus" name="FormStatus0" />
                    <input type="text" class="FormPath" name="FormPath0" style="display:none;width: 270px;" maxlength="<%=GlobalSetting.FieldLength.Training.FormPath %>" />
                    <input type="file" runat="server" id="TrainingForm0" class="FormPath" style="width: 274px;"  />
                    <a href="javascript:;" class="FormLink" style="display:none;"></a>
                    <input validate="grayout" grayout="Add Description" name="FormDescription0" type="text" class="Description" style="width: 140px;" maxlength="<%=GlobalSetting.FieldLength.Training.FormPath %>" />
                    <span style="float:right;">
                    <a href="javascript:;" style="display:none;" class="formButton">Change</a>
                    <a href="javascript:;" style="display:none;" class="delete" >Delete</a>
                    <input type="button" value="Add" class="AddTrainingForm" style="width:80px" /></span>
                </div>
            </td>
        </tr>
        <tr>
            <td class="titleTd" style="padding-top:20px;" colspan=4>
                <asp:Button ID="btnURL" runat="server" Text="Button" style="display:none;" PostBackUrl="~/Service/AjaxService.aspx"/>
                <input type="button" value="Save" id="btnSave" class="buttonWidth"/>
                <input type="button" value="Update" id="btnUpdate" class="buttonWidth"/>
                <input type="button" value="Publish" id="btnPublish" class="buttonWidth"/>
                <input type="button" value="Discard" class="buttonWidth" onclick="if (confirm('Please confirm to discard the training.')) window.close();" />
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
