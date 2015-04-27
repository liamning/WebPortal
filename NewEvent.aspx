<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewEvent.aspx.cs" Inherits="NewEvent" %>

<%@ Register Src="~/Control/MenuBar.ascx" TagPrefix="uc1" TagName="MenuBar" %>
<%@ Register Src="~/Control/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>
<%@ Register Src="~/Control/PublicHeader.ascx" TagPrefix="uc1" TagName="PublicHeader" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title> 
    <uc1:PublicHeader runat="server" ID="PublicHeader" />
    <script type="text/javascript" src="Resource/ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
    $(function () {

        var mode = "<%= mode%>";
        $form = $("form");
        $form.append("<input type='text' style='display:none;' id='txtAction' name='action' />");
        $form.append("<input type='text' style='display:none;' id='txtSerialNo' name='txtSerialNo' />");

        $action = $form.find("#txtAction");
        $txtSerialNo = $form.find("#txtSerialNo");

        $btnSave = $("#btnSave");
        $btnUpdate = $("#btnUpdate");
        $btnPublish = $("#btnPublish");

        $txtSerialNo0 = $("#txtSerialNo0");
        $txtSerialNo1 = $("#txtSerialNo1");
        $txtSerialNo2 = $("#txtSerialNo2");

        $txtName = $("#txtName");
        $comType = $("#comType");
        $dateStartDate = $("#dateStartDate");
        $dateStartTime = $("#dateStartTime");
        $dateEndTime = $("#dateEndTime");
        $txtLocation = $("#txtLocation");
        $txtContactPerson = $("#txtContactPerson");
        $txtPhoneNumber = $("#txtPhoneNumber");
        $txtDepartment = $("#txtDepartment");
        $txtEventDetails = $("#txtEventDetails");
        $hdfID = $("#hdfID");
        $btnPostBack = $("#btnPostBack");

        $fileEventImage = $("#fileEventImage");
        $txtVersionNo = $("#txtVersionNo");
        $dateDeadline = $("#dateDeadline");
        $txtIconExpiryDay = $("#txtIconExpiryDay");

        var originSchedule = {};
        var requiredMsgSuffix = "required.";
        var mandatoryControl = [
            { $control: $txtSerialNo1, requiredMsg: "Serial No. " + requiredMsgSuffix },
            { $control: $txtSerialNo2, requiredMsg: "Serial No. " + requiredMsgSuffix },
            { $control: $txtName, requiredMsg: "Event Name " + requiredMsgSuffix },
            { $control: $dateStartDate, requiredMsg: "Date & Time " + requiredMsgSuffix },
            { $control: $dateStartTime, requiredMsg: "Date & Time " + requiredMsgSuffix },
            { $control: $dateEndTime, requiredMsg: "Date & Time " + requiredMsgSuffix },
            { $control: $txtLocation, requiredMsg: "Location " + requiredMsgSuffix },
            { $control: $txtContactPerson, requiredMsg: "Contact Person " + requiredMsgSuffix },
            { $control: $txtDepartment, requiredMsg: "Department " + requiredMsgSuffix },
            { $control: $dateDeadline, requiredMsg: "Deadline " + requiredMsgSuffix },
            { $control: $txtPhoneNumber, requiredMsg: "Phone Number " + requiredMsgSuffix }
        ];
        var mandatoryControl_holiday = [
            { $control: $txtSerialNo1, requiredMsg: "Serial No. " + requiredMsgSuffix },
            { $control: $txtSerialNo2, requiredMsg: "Serial No. " + requiredMsgSuffix },
            { $control: $txtName, requiredMsg: "Event Name " + requiredMsgSuffix },
            { $control: $dateStartDate, requiredMsg: "Date & Time " + requiredMsgSuffix }
        ];

        var controlDisableInPH = [
            //$comType,
            //$txtSerialNo1,
            //$txtSerialNo2,
            //$txtName,
            //$dateStartDate,
            $dateStartTime,
            $dateEndTime,
            $("tr.hiddenHoliday, span.hiddenHoliday")
           // $txtLocation,
            //$txtContactPerson,
           // $txtDepartment,
          //  $dateDeadline,
           // $txtPhoneNumber,
           // $txtIconExpiryDay, 
          //  $fileEventImage
        ];

        var imageList;

        var eventTypeSerialList; 
        var systemParaBuildin = eval('(' + '<%= systemParaBuildin%>' + ')');
        var publicHolidayProgramCode = '<%= GlobalSetting.SystemBuildinCode.PublicHoliday%>';
        var isPublicHoliday = ('<%= isPublicHoliday%>' == 'True');
        var alertWhenUpdateEventSchedule = "<%= GlobalSetting.AlertMessage.AlertWhenUpdateEventSchedule%>";


        var scheduleChanged = function (newSchedule) {

            var isChanged = false;
            if (originSchedule.date != $dateStartDate.val()  ||
                originSchedule.startTime != $dateStartTime.val() ||
                originSchedule.endTime != $dateEndTime.val()) {

                if (confirm(alertWhenUpdateEventSchedule)) {
                    $('<input />').attr('type', 'hidden').attr('class', 'appendFields')
                               .attr('name', "resetAttendance")
                               .attr('value', 'true')
                               .appendTo('#form1');
                    return true;
                }
                else {
                    return false;
                } 

            }

            return true;
        }

        var geteventTypeSerial = function () {
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
                    eventTypeSerialList = data;

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

            $.each(eventTypeSerialList, function (index, value) {
                if (type == value.id) {
                    $txtSerialNo0.text(value.cdescription);
                    return false;
                }
            });

        }

        var checkPublicHoliday = function () {
            var typeID = $comType.val()
            return (systemParaBuildin[typeID] && systemParaBuildin[typeID].ProgramCode == publicHolidayProgramCode);
        }

        var disableControlForHoliday = function(){

            if (checkPublicHoliday()) {
                for (var i = 0, $item; $item = controlDisableInPH[i]; i++) {
                    $item.css('display', 'none');
                }

            }
            else {
                for (var i = 0, $item; $item = controlDisableInPH[i]; i++) {
                    $item.css('display', '');
                } 
            } 
        
        }

        var submit = function (action) {

            var controlToValidate;
            var validateDelegate;
            if (checkPublicHoliday())
            {
                controlToValidate = mandatoryControl_holiday;
                validateDelegate = undefined;


                $('<input />').attr('type', 'hidden').attr('class', 'appendFields')
                        .attr('name', "chkPublicHoliday")
                        .attr('value', "True")
                        .appendTo('#form1');
            }
            else {
                if (action == "updateEvent")
                    if (!scheduleChanged()) return;
                controlToValidate = mandatoryControl;
                validateDelegate = function () {

                    if (CKEDITOR.instances.txtEventDetails.getData() == "") {
                        alert('Details ' + requiredMsgSuffix);
                        CKEDITOR.instances.txtEventDetails.focus();
                        return false;
                    }
                    return true;

                };
                if ($fileEventImage.css('display') != 'none') {
                    controlToValidate.push({ $control: $fileEventImage, requiredMsg: "Upload Image " + requiredMsgSuffix });
                }
            }


            if (!validate(controlToValidate, validateDelegate)) return;


            $action.val(action);
            $txtSerialNo.val($txtSerialNo0.text() + "-" + $txtSerialNo1.val() + "-" + $txtSerialNo2.val());
            $btnPostBack.click();
        }

        var defineEventDelegate = function () {
 

            if (mode == "Edit") {
                $btnUpdate.click(function () {

                    $("input[type=hidden].appendFields").remove();
                                        
                    $.each(imageList, function (index, imageInfo) {
                        $('<input />').attr('type', 'hidden').attr('class', 'appendFields')
                            .attr('name', "status_" + imageInfo.controlID)
                            .attr('value', imageInfo.id + "-" + imageInfo.status)
                            .appendTo('#form1');
                    });
                    $('<input />').attr('type', 'hidden').attr('class', 'appendFields')
                            .attr('name', "txtVersionNo")
                            .attr('value', $txtVersionNo.text())
                            .appendTo('#form1'); 

                    submit("updateEvent");
                });
                $btnPublish.css("display", "none");
                $btnSave.css("display", "none");

            } else {
                $btnSave.click(function () {
                    $("input[type=hidden].appendFields").remove();
                    submit("saveEvent");
                });
                $btnPublish.click(function () {
                    $("input[type=hidden].appendFields").remove();
                    submit("publishEvent");
                });
                $btnUpdate.css("display", "none");
            }


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

            $comType.change(function () {
                initSerialNo0();

                window.ISCKEDITORReady = true;
                disableControlForHoliday();
            });
        }

        var getImagesForEditMode = function () {
            var formData = {
                action: "getEventImageJsonArray",
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


                    imageList = data;
                    contructImageList(imageList);
                }
            });
        }

        var contructImageList = function (data) {
            var html = "";
            var item;

            var addLink = function (item, $fileUpload, $description) {

                var html = '';
                if (item) {
                    html = html + '<a class="image-popup-no-margins" href="Service/PreviewImageHandler.ashx?maxLength=600&ID=' + item.id + '">' + item.filename
                      + '<img style="display:none;" />'
                      + '</a>'
                      + '<a style="margin-left: 40px; float: right;" href="javascript:;" class="imageRemove">Change</a>';
                    $fileUpload.after(html);

                    item.controlID = $fileUpload.attr('id');

                    $fileUpload.parent().find(".imageRemove").click(function () {
                        if ($(this).text() == "Change") {
                            $fileUpload.show();
                            $(this).text("Cancel").parent().find('.image-popup-no-margins').hide();
                            item.status = 'updated';
                        } else {
                            $fileUpload.hide();
                            $(this).text("Change").parent().find('.image-popup-no-margins').show();
                            item.status = 'normal';
                        }
                    });

                    if ($description) {
                        $description.val(item.description);
                        imageDict[$fileUpload.attr('id')] = item;
                    }
                }

                $fileUpload.hide();
            }

            addLink(data[0], $fileEventImage);

            $('.image-popup-no-margins').magnificPopup({
                type: 'image',
                closeOnContentClick: true,
                closeBtnInside: false,
                fixedContentPos: true,
                mainClass: 'mfp-no-margins mfp-with-zoom', // class to remove default margin from left and right side
                image: {
                    verticalFit: true
                },
                zoom: {
                    enabled: false,
                    duration: 300 // don't foget to change the duration also in CSS
                }
            });

        }

        //init
        var init = function () {

            defineEventDelegate();
            geteventTypeSerial();

            if (mode == "Edit")
                getImagesForEditMode();
             
            originSchedule.date = $dateStartDate.val();
            originSchedule.startTime = $dateStartTime.val();
            originSchedule.endTime = $dateEndTime.val();

            if(isPublicHoliday)
            {
                disableControlForHoliday();
            }
        }

        init();
    });
    </script>
</head>
<body>

        <uc1:MenuBar runat="server" ID="MenuBar" />
    <div id="content">
        <div class="clearLeftFloat"></div>
        <div id="navigationBar">
            <p>Staff Portal > Article Manager > <%=mode %> Event</p> 
        </div>  
    
        <div id="center"> 
            <form id="form1" runat="server"> 
            <div id="newArt" class="font12pt"> 
            <table id="tableContent">
                <tr>
                    <td colspan=4>
                        <h4 class="bar">Event</h4>
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
                        <select style="margin-left:0px" runat="server" id="comType">
                        </select>
                    </td>
                    <td  class="editorWidth_short" style=" text-align:right; padding-right: 20px;">
                        <span>Serial No.</span></td> 
                    <td>
                        <span id="txtSerialNo0" runat="server"></span>-
                        <input type="text" name="txtTitle" id="txtSerialNo1" runat="server" style="width:100px;text-transform:uppercase;" maxlength="100"  />
                        <span>-</span>
                        <input type="text" name="txtTitle" validate="number" id="txtSerialNo2" runat="server" style="width:50px;" maxlength="100" />
                    </td>
                </tr>

                <tr>
                    <td class="titleTd">Event Name</td>
                    <td colspan=3>
                        <input type="text" name="Name" runat="server" id="txtName"   class="editorWidth" />
                        <asp:HiddenField ID="hdfID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="titleTd">Date & Time</td>
                    <td colspan=4>
                
                        <input validate="date" type="text" name="EventDate" runat="server" id="dateStartDate" class="dateWidth" />
                        <input validate="time" type="text" name="StartTime" runat="server" id="dateStartTime" class="timeWidth" />
                        <span class="hiddenHoliday">-</span>
                        <input validate="time" type="text" name="EndTime" runat="server" id="dateEndTime" class="timeWidth" />
                    </td>
                </tr>
                <tr class="hiddenHoliday">
                    <td class="titleTd">Location</td>
                    <td colspan=3>
                        <input type="text" name="Location"  runat="server" id="txtLocation" class="editorWidth" /></td>
                </tr>
                <tr  class="hiddenHoliday">
                    <td class="titleTd">Contact Person</td>
                    <td class="editorWidth_short">
                        <input type="text" name="ContactPerson" runat="server" id="txtContactPerson" class="editorWidth_short" /></td>
                    <td class="editorWidth_short" style=" text-align:right; padding-right: 10px;">Phone Number</td>
                    <td ><input type="text" name="PhoneNumber" runat="server" validate="phone" id="txtPhoneNumber" class="editorWidth_short" /></td>
                </tr>
                <tr  class="hiddenHoliday">
                    <td class="titleTd">Department</td>
                    <td >
                        <input type="text" name="Department" runat="server" id="txtDepartment" class="editorWidth_short" /></td>
                    <td class="editorWidth_short" style=" text-align:right; padding-right: 10px;">Deadline</td>
                    <td >
                        <input validate="date" type="text" name="Deadline" runat="server" id="dateDeadline" class="dateWidth" />
                        </td>
                </tr>
        <tr  class="hiddenHoliday">
            <td class="titleTd">Icon Expiry Date</td>
            <td class="editorWidth_short">
                <input validate="date" type="text" name="txtIconExpiryDay" runat="server" id="txtIconExpiryDay" class="dateWidth" />
            </td> 
        </tr>
                <tr id="trEventDetails"  class="hiddenHoliday">
                    <td class="titleTd">Event Details</td> 

                    <td colspan=3>
                        <textarea   runat="server" name="EventDetails" id="txtEventDetails" class="editorWidth editorHeight1"></textarea>
                         <script>
                             CKEDITOR.replace('txtEventDetails');
                        </script>
                    </td>
                </tr>
                <tr  class="hiddenHoliday">
                    <td class="titleTd">Upload Image</td>
                    <td colspan=3><input type="file" id="fileEventImage" runat="server" accept="image/*"  /></td>
                </tr>
                <tr>
                    <td class="titleTd" style="padding-top:20px;" colspan=4>
                        <asp:Button ID="btnPostBack" runat="server" style="display:none;" PostBackUrl="~/Service/AjaxService.aspx" />
                        <input type="button" value="Save" id="btnSave" class="buttonWidth"/>
                        <input type="button" value="Update" id="btnUpdate" class="buttonWidth"/>
                        <input type="button" value="Publish" id="btnPublish" class="buttonWidth"/>
                        <input type="button" value="Discard" class="buttonWidth" onclick="if (confirm('Please confirm to discard the event.')) window.close();" />
                    </td>
                </tr>
            </table>
    
            </div>
            </form>
        </div>
        <div style="clear:both;"></div>
        <uc1:Footer runat="server" ID="Footer" />


    </div>

            <style type="text/css">
                /* padding-bottom and top for image */
                .mfp-no-margins img.mfp-img {
	                padding: 0;
                }
                /* position of shadow behind the image */
                .mfp-no-margins .mfp-figure:after {
	                top: 0;
	                bottom: 0;
                }
                /* padding for main container */
                .mfp-no-margins .mfp-container {
	                padding: 0;
                }

                
                .mfp-with-zoom .mfp-container,
                .mfp-with-zoom.mfp-bg {
                    opacity: 0.001;
                    -webkit-backface-visibility: hidden;
                    /* ideally, transition speed should match zoom duration */
                    -webkit-transition: all 0.3s ease-out;
                    -moz-transition: all 0.3s ease-out;
                    -o-transition: all 0.3s ease-out;
                    transition: all 0.3s ease-out;
                }

                .mfp-with-zoom.mfp-ready .mfp-container {
                    opacity: 1;
                }

                .mfp-with-zoom.mfp-ready.mfp-bg {
                    opacity: 0.8;
                }

                .mfp-with-zoom.mfp-removing .mfp-container,
                .mfp-with-zoom.mfp-removing.mfp-bg {
                    opacity: 0;
                }

            </style>


</body>
</html>
