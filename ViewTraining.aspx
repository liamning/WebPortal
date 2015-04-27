<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewTraining.aspx.cs" Inherits="ViewTraining" %>

<%@ Register Src="~/Control/MenuBar.ascx" TagPrefix="uc1" TagName="MenuBar" %>
<%@ Register Src="~/Control/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>
<%@ Register Src="~/Control/PublicHeader.ascx" TagPrefix="uc1" TagName="PublicHeader" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title> 
    <uc1:PublicHeader runat="server" ID="PublicHeader" />
    <div style="display:none;" id="div_schedulesJSString"><%= schedulesJSString %></div>
    <div style="display:none;" id="div_decisionsJSString"><%= decisionsJSString %></div>
    <div style="display:none;" id="div_formsJSString"><%= formsJSString %></div>
    <script type="text/javascript">
        $(function () {

            //control
            $btnJoin = $("#btnJoin");
            $btnNotAttend = $("#btnNotAttend");
            $trainingID = $("#trainingID");
            $hdfID = $("#hdfID");
            $hdfOptional = $("#hdfOptional");
            $tdSchedule = $("#tdSchedule");
            $txtMaximumAttendance = $("#txtMaximumAttendance");
            $txtDecision = $("#txtDecision");
            $divTrainingForm = $("#divTrainingForm");

            var schedulesData = eval('(' + $("#div_schedulesJSString").html() + ')');
            var decisionsData = eval('(' + $("#div_decisionsJSString").html()  + ')');
            var formsData = eval('(' + $("#div_formsJSString").html() + ')');
            var meetDeadline = '<%= metDeadline%>';

            if (meetDeadline == 'True')
            {
                $btnJoin.hide();
                $btnNotAttend.hide(); 
            }
            
            //function
            var fillTrainingSchedule = function () {
                 
                if (schedulesData.error) {
                    var decoded = $("<div/>").html(schedulesData.error).text();
                    alert(decoded);
                    return;
                } 
                var schedules = schedulesData; 

                if (decisionsData.error) {
                    var decoded = $("<div/>").html(decisionsData.error).text();
                    alert(decoded);
                    return;
                }
                var decisions = decisionsData;

                $tdSchedule.html("");
                for (var sche, i = 0; sche = schedules[i]; i++) {
                    appendScheHTML($tdSchedule, sche, decisions, $hdfOptional.val() == "False");

                    var $optionControl = $(".optionControl").last();

                    if ($hdfOptional.val() == "False") {
                        $optionControl.css('display', 'none');
                    } else {
                        $optionControl.css('display', '');
                    } 
                }
                     
            }

            var fillTrainingFormList = function () {
                  
                if (formsData.error) {
                    var decoded = $("<div/>").html(formsData.error).text();
                    alert(decoded);
                    return;
                }
                 
                var formList = formsData;

                for (var i = 0, form; form = formList[i]; i++) {
                    appendFormHTML($divTrainingForm, form);
                }

                if (i > 0) {
                    $("#trAttachment").css('display', '');
                }
            }

            var appendScheHTML = function (target, sche, decisions, optional) {
                $parent = target;

                var isChecked = "";
                for (var j = 0, decision; decision = decisions[j]; j++) {
                    $txtDecision.text(decision.decision);
                    if (decision.datetime == sche.scheduleDate + " " + sche.startTime) {
                        isChecked = "checked = 'checked'";
                        break;
                    }

                }

                var spanClass='';
                if (optional) {
                    isChecked = isChecked + " style='display:none;margin-bottom:0px!important;' ";
                }
                else {
                    spanClass = 'checkBoxSpan';
                    isChecked = isChecked + " style='margin-bottom:0px!important;' ";
                }

                var innerHTML = '<div  class="checkBoxLabel">'
                    + '<label for="chkSchedule_' + sche.ID + '">'
                    + '<input type="checkbox" ' + isChecked + ' id="chkSchedule_' + sche.ID + '" class="optionControl" /><span class="'+spanClass+'">'
                    + sche.scheduleDate
                    + ' &nbsp &nbsp '
                    + sche.startTime
                    + ' &nbsp - &nbsp '
                    + sche.endTime + '</span>'
                    + '</div>';

                $parent.append(innerHTML);

            }

            var appendFormHTML = function (target, form) {
                $parent = target;
                var innerHTML;
                innerHTML = '<div style="margin-bottom: 3px;"><a href="' + form.FormPath + '">' + form.Description + '</a></div>';
                /*if (form.SubSequence == "1")
                    innerHTML = '<a href="Service/FileService.aspx?type=training&ID=' + form.ID + '">' + form.Description + '</a><br/>';
                else
                    innerHTML = '<a href="' + form.FormPath + '">' + form.Description + '</a><br/>';
                    */
                $parent.append(innerHTML);

            }

            var submit = function (action_log, action, eventSchedules) {


                //get the form data
                var formData = {
                    activityID: $hdfID.val(),
                    category: "Training",
                    action_log: action_log,
                    schedules: eventSchedules,
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
                        //fillTrainingSchedule();

                        window.location = window.location;
                    }
                });

            }

            //control event
            $btnJoin.click(function () {

                var eventSchedules = [];

                if ($hdfOptional.val() != "False") { 
                    var max = parseInt($txtMaximumAttendance.text());
                    $("input:checkbox.optionControl").each(function (key) {
                        if ($(this).is(':checked')) {
                            eventSchedules.push($(this).attr('id').replace("chkSchedule_", ""));
                        }
                    });
                    if (eventSchedules.length == 0) {
                        alert('At least one Date & Time selected');
                        return false;
                    } else if (eventSchedules.length > max) {
                        alert('Cannot attend the training more than ' + max + ' time(s)');
                        return false;
                    }
                     
                } else
                {
                    $("input:checkbox.optionControl").each(function (key) {
                        eventSchedules.push($(this).attr('id').replace("chkSchedule_", ""));
                    });
                }

                submit("Join", "logActivity", eventSchedules);
            });

            $btnNotAttend.click(function () {

                if ($hdfOptional.val() != "False") { 
                    $("input:checkbox.optionControl").each(function (key) {
                        $(this).prop('checked', false);
                    });
                }

                submit("NotAttend", "logActivity", []);
            });

            //execute when page load
            fillTrainingSchedule();
            fillTrainingFormList();
             
        });
    </script>
</head>
<body>
    <form id="form1" runat="server"> 
        <uc1:MenuBar runat="server" ID="MenuBar" />
    <div id="content">
        <div class="clearLeftFloat"></div>
        <div id="navigationBar">
            <p>Staff Portal > Home> Training</p> 
        </div>  

     <div id="center">
    <div class="font12pt"> 
    <table id="tableContent" class="viewTable">

        <tr>
            <td>Training Course :</td>
            <td colspan=3 class="articleTitle">
                <span name="txtName" runat="server" id="txtName" ></span>
                <asp:HiddenField ID="hdfID" runat="server" />
                <asp:HiddenField ID="hdfOptional" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="width: 110px;">Date & Time :</td>
            <td id="tdSchedule" style="width: 210px;" > 
                
            </td>
             
            <td style="width: 110px;">Deadline :</td>
            <td > <span name="dateDeadline" runat="server" id="dateDeadline" ></span>
                <div style="display:none;">
                    
                <span>Maximum Attendance : </span>
                <span id="txtMaximumAttendance" runat="server"></span>
                </div>

            </td> 
        </tr>  
        <tr>
            <td>Contact Person :</td>
            <td>
                <span name="txtContactPerson" runat="server" id="txtContactPerson" ></span></td>
            <td>Phone Number :</td>
            <td ><span name="txtPhoneNumber" runat="server" id="txtPhoneNumber"  ></span></td>
        </tr>
        <tr>
            <td>Department :</td>
            <td>
                <span name="txtDepartment" runat="server" id="txtDepartment"  ></span></td>
            <td>Email Address :</td>
            <td >
                <a runat="server" href="javascript:;" id="txtEmail"></a>

            </td>
        </tr>

        <tr>
            <td>Location :</td>
            <td colspan=3>
                <span name="txtLocation" runat="server" id="txtLocation" ></span></td>
        </tr>
        <tr>
            <td>Event Details :</td>
            <td colspan=3><div class="editorWidth normal"  runat="server" id="txtDetails"  ></div>
                <div ></div>
            </td>
        </tr>
        <tr id="trAttachment" style="display:none;">
            <td>Attachment :</td>
            <td colspan=3><div id="divTrainingForm">


                          </div>
                <div ></div>
            </td>
        </tr>
        <tr>
            <td>Last Decision:</td>
            <td colspan=3><span id="txtDecision" runat="server"></span>
            </td>
        </tr>
        <tr>
            <td style="padding-top:20px;" colspan=4>
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
