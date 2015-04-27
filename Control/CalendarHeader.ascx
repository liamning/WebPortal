<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CalendarHeader.ascx.cs" Inherits="Control_CalendarHeader" %>
    

	<link rel="stylesheet" href="Resource/jquery-ui-1.10.4.custom/css/custom-theme/jquery-ui-1.10.4.custom.min.css" />
	<script src="Resource/jquery-ui-1.10.4.custom/js/jquery-ui-1.10.4.custom.min.js"></script> 
    <style>
        
     .highlight_weekend a
    { 
        background: #BBB!important;    
        border: 1px #BBB solid !important;
    }
    .ui-datepicker { width: 16.5em;  padding: .2em .2em 0; display: none; font-family: Arial; font-size: 9pt; }
    .ui-datepicker table {width: 100%;font-size: .9em; border-collapse: collapse;   } 
    td a {  padding: 4px!important; margin:0px; }
    td.highlight a {background: #489DC0!important;  border: 1px #489DC0 solid !important; padding: 4px!important;  }
    .ui-datepicker-calendar td a {cursor: default!important;}
    .ui-datepicker-prev, .ui-datepicker-next {cursor: pointer!important;} 
    .ui-widget-content {
    border-top-color: rgb(221, 221, 221);
    border: 1px solid #ddd; 
    border-bottom:0px !important; 
    padding-right:3px;
    color: #333;
    }
    td.highlight_holiday a
    {
        color: #aa0000!important; 
        font-weight: bold!important;  
        border: 1px #aa0000 solid !important;   
    }
    </style>

<div style="display:none;" id="div_eventData"><%= eventData %></div>
<script>

    $(function () {

        var $eventDiv = $("#eventDiv");
        var globalData;
        var ispollingData = false;
        var publicHolidayCode = '<%= GlobalSetting.SystemBuildinCode.PublicHoliday %>';
        var currentMonthEventData = eval('(' + $("#div_eventData").html() + ')');

        var appendEventSummary = function (data, year, month) {
            $eventDiv.html("");
            var hasEvent = false;
            var eventList = "Next Training & Events:<br />";
            var page = "ViewEvent";
            for (var i = 0, eventItem; eventItem = data[i]; i++) {
                if (!eventItem.programcode
                    && year == eventItem.starttime.getFullYear()
                    && month - 1 == eventItem.starttime.getMonth()) {
                    page = (eventItem.type == "Training" ? "ViewTraining" : "ViewEvent");
                    eventList = eventList + eventItem.starttime.getDate() + "/" + (eventItem.starttime.getMonth() + 1) + "/" + eventItem.starttime.getFullYear() + "<br />";
                    eventList = eventList + "<font color=#3894C4><a href='" + page + ".aspx?ID="
                                + eventItem.id + "'>" + eventItem.name + "</a></font><br />";
                    hasEvent = true;
                }
            }
            if (hasEvent) {
                $eventDiv.append("<p style='padding: 0px 5px 5px 5px; font-size: 9pt; font-family:Arial;'><b>" + eventList + "</b></p>");
            }
        }

        var delay_getEventByMonth = function (year, month) {
            if (ispollingData) {
                setTimeout(function () { delay_getEventByMonth(year, month); }, 100);
            } else {
                getEventByMonth(year, month);
            }
        }

        //create the calendar
        $("#datepicker").datepicker({
            onChangeMonthYear: function (year, month, inst) {
                delay_getEventByMonth(year, month);
            },
            beforeShowDay: function (date) {
                var styleClass = "";
                if (date.getDay() == 0 || date.getDay() == 6) {
                    styleClass = 'highlight_weekend';
                }

                if (globalData) {
                    for (var i = 0, eventItem; eventItem = globalData[i]; i++) {
                        if (date.getFullYear() == eventItem.starttime.getFullYear()
                            && date.getMonth() == eventItem.starttime.getMonth()
                            && date.getDate() == eventItem.starttime.getDate()) {


                            if (!eventItem.programcode)
                                styleClass = styleClass + ' highlight';
                            else
                                styleClass = styleClass + ' highlight_holiday';
                        }
                    }
                }

                return [true, styleClass, ""];
            }
        });

        //get Event List
        var getEventByMonth = function (year, month) {
            ispollingData = true;
            $.ajax({
                type: 'POST',
                url: "Service/AjaxService.aspx",
                data: {
                    action: "getEventByMonth",
                    DateOfMonth: year + "-" + month + "-01"
                },
                success: function (data) {
                    data = eval('(' + data + ')');

                    if (data.error) {
                        var decoded = $("<div/>").html(data.error).text();
                        alert(decoded);
                        return;
                    }

                    globalData = data;
                    ispollingData = false;

                    $.each(globalData, function (index, value) {
                        value.starttime = convertDate(value.starttime);
                        value.endtime = convertDate(value.endtime);
                    });

                    $("#datepicker").datepicker("refresh");
                    appendEventSummary(data, year, month);
                }
            });
        }

        //getEventByMonth(new Date().getFullYear(), new Date().getMonth() + 1);


        //init the event of current month

        var init = function () {

            var year, month;
            year = new Date().getFullYear();
            month = new Date().getMonth() + 1;

            if (currentMonthEventData.error) {
                var decoded = $("<div/>").html(currentMonthEventData.error).text();
                alert(decoded);
                return;
            }

            globalData = currentMonthEventData;
            ispollingData = false;

            $.each(globalData, function (index, value) {
                value.starttime = convertDate(value.starttime);
                value.endtime = convertDate(value.endtime);
            });

            $("#datepicker").datepicker("refresh");
            appendEventSummary(currentMonthEventData, year, month);

        };
        init();

    });
</script>

