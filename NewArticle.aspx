<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewArticle.aspx.cs" Inherits="NewArticle" %>

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

            $form = $("form");
            var mode = '<%=mode %>';
            $form.append("<input type='text' style='display:none;' id='txtAction' name='action' />");
            $form.append("<input type='text' style='display:none;' id='txtSerialNo' name='txtSerialNo' />");

            $action = $form.find("#txtAction");
            $txtSerialNo = $form.find("#txtSerialNo");
            $tableContent = $("#tableContent"); 
            $comType = $("#comType");
            $txtSerialNo0 = $("#txtSerialNo0");
            $txtSerialNo1 = $("#txtSerialNo1");
            $txtSerialNo2 = $("#txtSerialNo2");

            $txtTitle = $("#txtTitle");
            txtHeadline = $("#txtHeadline");
            $txtSummary = $("#txtSummary");
            $txtMainBody = $("#txtMainBody");

            $Thumbnail0 = $("#Thumbnail0");
            $Thumbnail1 = $("#Thumbnail1");
            $Enlarge0 = $("#Enlarge0");

            $Attachment1 = $("#Attachment1");
            $Attachment2 = $("#Attachment2");

            $hdfID = $("#hdfID");
            $btnAddMoreImages = $("#btnAddMoreImages");
            $txtVersionNo = $("#txtVersionNo");

            $ddlYear = $("#ddlYear");
            $ddlMonth = $("#ddlMonth");
            $ddlDay = $("#ddlDay");

            $ddlIconExpiryYear = $("#ddlIconExpiryYear");
            $ddlIconExpiryMonth = $("#ddlIconExpiryMonth");
            $ddlIconExpiryDay = $("#ddlIconExpiryDay");
             
            var requiredMsgSuffix = "required.";
            var mandatoryControl = [
                { $control: $txtSerialNo1, requiredMsg: "Serial No. " + requiredMsgSuffix },
                { $control: $txtSerialNo2, requiredMsg: "Serial No. " + requiredMsgSuffix },
                { $control: $txtTitle, requiredMsg: "Title " + requiredMsgSuffix },
               // { $control: txtHeadline, requiredMsg: "Headline " + requiredMsgSuffix },
                //{ $control: $txtSummary, requiredMsg: "Summary " + requiredMsgSuffix }
            ];

            
            var newsTypeSerialList;
            var imageList;
            var imageDict = {};         //dictionary to easy retrieve the image data by the file upload control id
            var fileEnlargeCount = 0;

            var attachmentDict = {};

            //function
            var getNewsTypeSerial = function()
            {
                var formData = {
                    action: "getSystemPara",
                    category: "NewsType",
                    includeabandon: true
                };
                $.ajax({
                    url: "Service/AjaxService.aspx",
                    data: formData,
                    type: 'POST',
                    success: function (data) {
                        data = eval('(' + data + ')');
                        newsTypeSerialList = data;

                        if (data.error) {
                            var decoded = $("<div/>").html(data.error).text();
                            alert(decoded);
                            return;
                        }

                        initSerialNo0();
                    }
                });
            }

            var initSerialNo0 =  function () {

                var type = $comType.val();

                $.each(newsTypeSerialList, function (index, value) {
                    if (type == value.id) {
                        $txtSerialNo0.text(value.cdescription);
                        return false;
                    }
                });

            } 

            var addImage = function (callback) {

                fileEnlargeCount = fileEnlargeCount + 1;
                var newFileName = "Enlarge" + fileEnlargeCount;
                var innerHTML = "<tr><td></td><td><input  class=\"fileUploadWidth\" id=\"" + newFileName + "\" name=\"" + newFileName + "\" type=\"file\" accept=\"image/*\"  />"
                              + "<br><input type='text' name=\"" + newFileName + "\" class='fileUploadWidth' style='margin-right: 10px;margin-top: 3px;' validate='grayout' grayout='Add Description' /><a href=\"javascript:;\" class=\"delete\">Delete</a>"
                              + "</td></tr>";

                $tableContent.find("tr:last").after(innerHTML);

                grayOut($tableContent.find("tr:last"));

                var $newTr = $tableContent.find("tr:last");

                $newTr.find(":text").change(function () {
                    var $parent = $(this).parent();
                    var $fileUploadControl = $parent.find(":file");
                    var tmpItem = imageDict[$fileUploadControl.attr('id')];
                    if (tmpItem) {
                        tmpItem.status = 'descchanged';
                    }
                });

                $newTr.find("a.delete").click(function () {
                    var $parent = $(this).parent();
                    var $fileUploadControl = $parent.find(":file");
                    var tmpItem = imageDict[$fileUploadControl.attr('id')];
                    if (tmpItem) {
                        tmpItem.status = 'deleted';
                    }
                    $parent.parent().remove();
                });

                if (callback && mode == "Edit") {
                    callback({
                        controlID: newFileName
                    });
                }

            }


            var submit = function () {
                var validateMainBody = function () {
                    if (CKEDITOR.instances.txtMainBody.getData() == "") {
                       // alert('Main Body ' + requiredMsgSuffix);
                       // CKEDITOR.instances.txtMainBody.focus();
                       // return false;
                    }
                    if ($("input[type=file]").length == 2) {
                        alert('Add more images ' + requiredMsgSuffix);
                        $btnAddMoreImages.focus();
                        return false;
                    } else {
                        var blankFileUpload = false;
                        $("input[type=file]").each(function () {
                            if ($(this).attr('name').indexOf('Attachment') >= 0) return;

                            if ($(this).val() == "" && $(this).css('display') != 'none') {
                                blankFileUpload = true;
                                alert("Please select the image");
                                $(this).focus();
                                return false;
                            }
                        });
                        if (blankFileUpload) {
                            return false;
                        }
                    }

                    return true;
                };

                if (!validate(mandatoryControl, validateMainBody)) return;


                if (mode == "Edit") {
                    $('#form1').submit(function () { //listen for submit event


                        $("input[type=hidden].appendFields").remove();

                        $.each(imageList, function (index, imageInfo) {
                            $('<input />').attr('type', 'hidden').attr('class', 'appendFields')
                                .attr('name', "status_" + imageInfo.controlID)
                                .attr('value', imageInfo.id + "-" + imageInfo.status)
                                .appendTo('#form1');
                        });

                        //add the attachment info to the form data for tracking
                        $.each(attachmentDict, function (property, attachmentInfo) {
                            $('<input />').attr('type', 'hidden').attr('class', 'appendFields')
                                .attr('name', "attachmentStatus_" + attachmentInfo.controlID)
                                .attr('value', attachmentInfo.id + "-" + attachmentInfo.status)
                                .appendTo('#form1');
                        });

                        $('<input />').attr('type', 'hidden').attr('class', 'appendFields')
                                .attr('name', "txtVersionNo")
                                .attr('value', $txtVersionNo.text())
                                .appendTo('#form1');



                        return true;
                    });
                }
                $("#btnURL").click();

            }


            var defineEventDelegate = function () {
                
                $btnAddMoreImages.click(function () {
                    
                    addImage(function (data) {
                        var newItem = {};
                        newItem.controlID = data.controlID;
                        newItem.status = "added";
                        newItem.id = 0;
                        imageList.push(newItem);
                    });

                });

                $("#ulImageList li a").click(function () {
                    document.imageID = $(this).attr("imageID");
                    window.open('ViewImage.html');
                });

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
                //save button
                $("#btnSave").click(function () {

                    $action.val("createNews");
                    $txtSerialNo.val($txtSerialNo0.text() + "-" + $txtSerialNo1.val() + "-" + $txtSerialNo2.val());
                    submit();
                    return false;
                });


                //save button
                $("#btnEdit").click(function () {

                    $action.val("updateNews");
                    $txtSerialNo.val($txtSerialNo0.text() + "-" + $txtSerialNo1.val() + "-" + $txtSerialNo2.val());
                    submit();
                    return false;
                });


                $comType.change(initSerialNo0);

                var specialDateDict = {
                    '02': [30, 31],
                    '04': [31],
                    '06': [31],
                    '09': [31],
                    '11': [31]
                };

                var init_ddlDay = function ($day) {
                    var $options = $day.find("option[value!='0']");
                    for (var i = $options.length + 1; i <= 31; i++) {
                        $day.append($("<option></option>")
                                .attr("value", i.toString())
                                .text(i.toString()));
                    }
                }

                function checkNewsDate($day, $month, $year) {
                    
                    init_ddlDay($day);
                    var dayToRemove = specialDateDict[$month.val()];
                    if (dayToRemove)
                    {
                        for (var i = 0, item; item = dayToRemove[i]; i++) {
                            $day.find("option[value='" + item + "']").remove();
                        }
                    }

                    if ($year.val() && $year.val() != "0" && $month.val())
                    {

                        var dateString = $year.val().substring(2) + $month.val() + 29;
                        if (!dateString.isValidDateTime()) {
                            $day.find("option[value='29']").remove();
                        }
                    }
                    
                } 
                $ddlMonth.change(function () {
                    checkNewsDate($ddlDay, $ddlMonth, $ddlYear);
                });
                $ddlYear.change(function () {
                    checkNewsDate($ddlDay, $ddlMonth, $ddlYear);
                });


                $ddlIconExpiryYear.change(function () {
                    checkNewsDate($ddlIconExpiryDay, $ddlIconExpiryMonth, $ddlIconExpiryYear);
                });


                $ddlIconExpiryMonth.change(function () {
                    checkNewsDate($ddlIconExpiryDay, $ddlIconExpiryMonth, $ddlIconExpiryYear);
                });

                 
            }

            var getImagesForEditMode = function () {
                var formData = {
                    action: "getImageJsonArray",
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

            var getFilesForEditMode = function () {
                var formData = {
                    action: "getFileJsonArray",
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
                        contructAttachmentList(data);
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
                          + '</a>';
                        $fileUpload.after(html);

                        if ($description) {
                            $description.next().after('<a style="margin-left: 20px;" href="javascript:;" class="imageRemove">Change</a>');
                        }
                        else
                        {
                            $fileUpload.after('<a style="float: right;" href="javascript:;" class="imageRemove">Change</a>');
                        }


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

                        if($description)
                        {
                            $description.val(item.description);
                            imageDict[$fileUpload.attr('id')] = item;
                        }
                    }

                    $fileUpload.hide();
                }

                addLink(data[0], $Thumbnail0);
                addLink(data[1], $Thumbnail1);
                addLink(data[2], $Enlarge0);
                 
                for (var i = 3; item = data[i]; i++) { 
                    addImage();
                    addLink(data[i], $tableContent.find("tr:last").find(":file"), $tableContent.find("tr:last").find(":text"));
                }

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
             
            var contructAttachmentList = function (data) {


                var html = "";
                var item;

                var addLink = function (item, $fileUpload, $description) {
                    
                    var html = '';
                    if (item) {
                        html = html + '<a class="file-popup-no-margins" href=Service/FileService.aspx?ID=' + item.id + '>' + item.filename
                          + '</a>';
                        $fileUpload.after(html);

                        $fileUpload.after('<a style="float: right;" href="javascript:;" class="imageRemove">Clear</a>');
                         
                        $fileUpload.parent().find(".imageRemove").click(function () {
                            if ($(this).text() == "Clear") {
                                $fileUpload.show();
                                $(this).text("Restore").parent().find('.file-popup-no-margins').hide();
                                item.status = 'updated';
                            } else {
                                $fileUpload.hide();
                                $(this).text("Clear").parent().find('.file-popup-no-margins').show();
                                item.status = 'normal';
                            }
                        });

                        if ($description) {
                            $description.val(item.description);
                            imageDict[$fileUpload.attr('id')] = item;
                        }

                        $fileUpload.hide();

                    }
                    else {
                        item = {};
                        item.status = "blank";
                        $fileUpload.show();
                    }

                    item.controlID = $fileUpload.attr('id');
                    $fileUpload.change(function () {
                        if ($fileUpload.val()) {

                            if (item.status == "blank") {
                                item.status = "added";
                                item.id = 0;
                            } 
                        }
                    });

                    //store the attachment object to the dictionary by the control ID.
                    attachmentDict[$fileUpload.attr('id')] = item;
                }

                addLink(data[0], $Attachment1);
                addLink(data[1], $Attachment2);
                  
            }

            var init = function () { 

                defineEventDelegate();
                getNewsTypeSerial();

                if (mode == "Edit") { 
                    getImagesForEditMode();
                    getFilesForEditMode();
                }
            }

            //initialize
            init();

        });
    </script>
</head>
<body>
    <form id="form1" runat="server" >
        <uc1:MenuBar runat="server" ID="MenuBar" />
         
    <div id="content">
        <div class="clearLeftFloat"></div>
        <div id="navigationBar">
            <p>Staff Portal > Article Manager> <%=mode %> Article</p> <asp:HiddenField ID="hdfID" runat="server" /> 
        </div>        
        <div id="center">
            <!--<div id="newArt" class="font12pt">-->
            <div id="newArt" class="font12pt">
            <table>
                <tr>
                    <td  colspan="4">
                        <h4 class="bar"><span>Article</span></h4> 
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
                    <td width="70px"><span>Serial No.</span></td> 
                    <td>
                        <span id="txtSerialNo0" runat="server"></span>-
                        <input type="text" id="txtSerialNo1" runat="server"  style="width:100px;text-transform:uppercase;"  />
                        <span>-</span>
                        <input type="text" id="txtSerialNo2" runat="server" validate="number"  style="width:50px;"  />
                    </td>
                </tr>
                <tr>
                    <td class="titleTd">Title</td> 
                    <td colspan="3">
                        <input type="text" name="txtTitle" runat="server" id="txtTitle" class="editorWidth"  />
                    </td>
                </tr>

                <tr>
                    <td class="titleTd">Date</td>
                    <td colspan="1">
                        <select id="ddlDay" runat="server" style="margin-left:0px;"></select> /
                        <select id="ddlMonth" runat="server"></select> /
                        <select id="ddlYear" runat="server"></select>
                    </td>
                    
                    <td colspan="2" ><span style="padding-right: 20px;">Icon Expiry Date</span>

                        
                        <select id="ddlIconExpiryDay" runat="server" style="margin-left:0px;"></select> /
                        <select id="ddlIconExpiryMonth" runat="server"></select> /
                        <select id="ddlIconExpiryYear" runat="server"></select>
                    </td> 
                </tr>
                <tr>
                    <td class="titleTd">Headline</td> 
                    <td colspan="3"><textarea name="txtHeadline" id="txtHeadline" runat="server" class="editorWidth editorHeight1"></textarea> </td>
                </tr>

                <tr>
                    <td class="titleTd">Summary</td> 
                    <td colspan="3"><textarea name="txtSummary" id="txtSummary" runat="server" class="editorWidth editorHeight1"></textarea> </td>
                </tr>
                
                 
                <tr>
                    <td class="titleTd">Main Body</td>
                    <td colspan="3">
                    <textarea name="txtMainBody" id="txtMainBody" runat="server" class="editorWidth editorHeight1"></textarea>
                    <script>
                        CKEDITOR.replace('txtMainBody');
                    </script>
                    </td>
                </tr>
        
                <tr>
                    <td colspan="4">
                
                        <div ID="labDisplay" ></div>

                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <h4 class="bar">Image</h4>
                    </td>
                </tr>
        
            </table>
            <table id="tableContent" >
                <tr>
                    <td class="titleTd" style="width: 100px;">Thumbnail 1:</td>
                    <td>
                        <input id="Thumbnail0" name="Thumbnail0" accept="image/*" runat="server" type="file" class="fileUploadWidth"   />
                         
                    </td>
                </tr>
                <tr>
                    <td class="titleTd" style="width: 100px;">Thumbnail 2:</td>
                    <td>
                        <input id="Thumbnail1" name="Thumbnail1" accept="image/*" runat="server" type="file" class="fileUploadWidth"   />
                         
                    </td>
                </tr>
    
                <tr>
                    <td class="titleTd">Enlarge:</td>
                    <td>
                        <input id="Enlarge0" name="Enlarge0" accept="image/*" runat="server" type="file" class="fileUploadWidth"  />&nbsp;</td> 
                </tr>
    
                <tr>
                    <td class="titleTd">Gallery:</td>
                </tr>

                
</table>    
            <table> 
                <tr>
                    <td class="titleTd" colspan=2>                         
                        <input type="button" id="btnAddMoreImages" value="Add more images" class="buttonWidth">
                    </td>
                </tr>
            </table>

               
            <table style="margin: 20px 0 20px 0;width: 791px;">  
     
                <tr>
                    <td colspan="4">
                        <h4 class="bar">Attachment</h4>
                    </td>
                </tr>
                <tr>
                    <td class="titleTd"  style="width: 100px;">Attachment 1:</td>
                    <td>
                        <input id="Attachment1" name="Attachment1" runat="server" type="file" class="fileUploadWidth"   />
                         
                    </td>
                </tr>

                
                <tr>
                    <td class="titleTd">Attachment 2:</td>
                    <td>
                        <input id="Attachment2" name="Attachment2" runat="server" type="file" class="fileUploadWidth"   />
                         
                    </td>
                </tr>
            </table>

            <table> 
                <tr>
                    <td class="titleTd" colspan=2 > 

                        <asp:Button ID="btnURL" runat="server" Text="Button" style="display:none;" PostBackUrl="~/Service/AjaxService.aspx"/>

                        <asp:Button ID="btnSave" runat="server" Text="Save" class="buttonWidth" />
                        <asp:Button ID="btnEdit" runat="server" Text="Update" class="buttonWidth" />
                    </td>
                </tr>
            </table>
    
            </div> 
        </div>
        <div style="clear:both;"></div>
        <uc1:Footer runat="server" ID="Footer" />
    </div>
    </form>  

           
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
