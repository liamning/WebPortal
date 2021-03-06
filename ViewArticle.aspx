<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewArticle.aspx.cs" Inherits="ViewArticle" %>

<%@ Register Src="~/Control/MenuBar.ascx" TagPrefix="uc1" TagName="MenuBar" %>
<%@ Register Src="~/Control/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>
<%@ Register Src="~/Control/PublicHeader.ascx" TagPrefix="uc1" TagName="PublicHeader" %>
<%@ Register Src="~/Control/CalendarHeader.ascx" TagPrefix="uc1" TagName="CalendarHeader" %>





<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title> 

    <uc1:PublicHeader runat="server" ID="PublicHeader" />
    <uc1:CalendarHeader runat="server" ID="CalendarHeader" />

    <script type="text/javascript">
        $(function () {

            //get the element
            $divArticle = $("#divArticle");
            $divGallery = $("#divGallery");
            $tabLi = $('.tab li');


            //init the tab style
            $tabLi.each(function () {
                $(this).addClass('tabUnActived');
            });
            $tabLi.first().removeClass('tabUnActived');
            $tabLi.first().addClass('tabActived');


            //click event of the tab
            $tabLi.click(function () {

                $tabLi.each(function () {
                    $(this).removeClass('tabActived');
                });
                $(this).removeClass('tabUnActived');
                $(this).addClass('tabActived');
                if($(this).attr('id') == "liArticle"){
                    $divArticle.css("display", "block");
                    $divGallery.css("display", "none");
                }else{
                    $divArticle.css("display", "none");
                    $divGallery.css("display", "block");
                }
                
            });

            $('.list  li a').click(function () {
                var $parent = $(this).parent();
                $parent.find('ul').toggle();
                if ($parent.find('ul').find('li').length == 0) {
                    $parent.addClass("list0");
                    return;
                }

                if ($parent.hasClass("list3")) {
                    $parent.removeClass("list3").addClass("list2")
                        .find('li').removeClass("list2").addClass("list3");
                }
                else {
                    $parent.removeClass("list2").addClass("list3")
                        .find('li').removeClass("list3").addClass("list2");
                }
            }).click();



            (function () {

                $txtSuggestionName = $("#txtSuggestionName");
                $txtSuggestionEmail = $("#txtSuggestionEmail");
                $comSuggestionType = $("#comSuggestionType");
                $txtSuggestionTel = $("#txtSuggestionTel");
                $txtSuggestionOtherEmail = $("#txtSuggestionOtherEmail");
                $txtSuggestion = $("#txtSuggestion");
                $btnSubmitSuggestion = $("#btnSubmitSuggestion");
                $closeButton = $(".close-reveal-modal");
                $labWaiting = $("#labWaiting");

                $labWaiting.css('display', 'none');

                var requiredMsgSuffix = "required.";
                var mandatoryControl = [
                    { $control: $txtSuggestion, requiredMsg: "Suggestion " + requiredMsgSuffix }
                ];

                var clearSuggestion = function () {
                    $comSuggestionType.find('option').first().prop('selected', true);
                    $txtSuggestionTel.val("");
                    $txtSuggestionOtherEmail.val("");
                    $txtSuggestion.val("");
                }

                var commitSuggestion = function () {

                    if (!validate(mandatoryControl)) return;

                    var formData = {
                        action: "commitSuggestion",
                        UserName: $txtSuggestionName.val(),
                        Email: $txtSuggestionEmail.val(),
                        Type: $comSuggestionType.val(),
                        PhoneNumber: $txtSuggestionTel.val(),
                        OtherEmail: $txtSuggestionOtherEmail.val(),
                        Suggestion: $txtSuggestion.val()
                    };


                    $closeButton.css('display', 'none');
                    $btnSubmitSuggestion.css('display', 'none');
                    $labWaiting.css('display', '');
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
                            $closeButton.css('display', '');
                            $btnSubmitSuggestion.css('display', '');
                            $labWaiting.css('display', 'none');

                            alert(data.message);
                            $closeButton.click();
                            clearSuggestion();

                        }
                    });

                }

                var defineSuggestionEventDelegate = function () {

                    //define the click event for the submit button
                    $btnSubmitSuggestion.click(function () {
                        commitSuggestion();
                    });

                    //define the button click event for the suggestion box
                    $("#btnReveal").click(function () {
                        $('#myModal').reveal({
                            animation: 'fadeAndPop',                   //fade, fadeAndPop, none
                            animationspeed: 300,                       //how fast animtions are
                            closeonbackgroundclick: false,              //if you click background will modal close?
                            dismissmodalclass: 'close-reveal-modal'    //the class of a button or element that will close an open modal
                        });
                    });

                }

                var menuInit = function () {

                    defineSuggestionEventDelegate();

                }

                menuInit();

            })();
  
        });  

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:MenuBar runat="server" ID="MenuBar" />
        <div id="content">
        <div class="clearLeftFloat"></div>
        <div id="navigationBar">
            <p>Staff Portal > Home> News</p> 
        </div>
        <div id="center">
            <div id="centerLeft">

                <div id ="tabDiv">
                    <div>
                    <ul class="tab">
                        <li id="liArticle"><a href="javascript:;">Article</a></li>
                        <li id="liGallery"><a href="javascript:;">Gallery</a></li>
                    </ul>
                    </div> 
                    <div class="clearLeftFloat"></div>
                    <div id="tabContent">
                        <div id="divArticle">
                            <div runat="server" id="labHeadline" class="title blueFont"></div>
                            <p runat="server" id="labSummary" class="subTitle">
                            </p>
                            <div style="margin: 8px 0px 8px 0px;">
                                <asp:Image ID="imgSummary" runat="server" />
                            </div>
    
                            <div runat="server" id="labContent" style="word-wrap:break-word;"  class="normal" >
                            </div>

                            <div runat="server" id="labAttachment" style="font-size:9.5pt;" >

                            </div>
                        </div>
                        <div id="divGallery" style="display:none;">
                            
                            <div id="divAlbumList">
                                <%= imageContent.ToString() %> 
                            </div>
                            
                        <div class="clearLeftFloat"></div>

                        </div>
                    </div>
                </div>
            </div>
    
            <div id="centerRight">
                <div class="container">

                    <div>
                        <div id="newsLetters">
                            <p class="title">Newsletters</p>
                            
                            <div class="content" runat="server" id="divNewsLetters"> 
                                <ul class="list"> 
                                </ul>
                            </div>
                        </div>
                    </div>
                    
                    
                    <div id="datepicker">
                    </div> 
                    <div id="eventDiv" style='border:1px solid #DDDDDD; border-top:0px;'>
                    </div> 

                    <div class="big-link divSuggestionBox" id="btnReveal" >
                        <div  class="suggestionMessage" style=" ">
                            <div style="  display:none;">
                                
                                <div>Want to leave message?</div>
                            </div>
                            <div> 
                                
                                <div class="rotate45">
                                    <div class="rotateContainer">
                                    <div class="rotateContent">Your Opinion</div> 
                                    </div>
                                </div>
                                <p style="color: white; padding: 10px;">Click Here To Say</p>
                            </div>

                        </div>
                        <div class="suggestionHeader" >Connect & Collaboration</div>
                        <div class="suggestionBoxImage"></div>
                    </div>
                </div>
            </div>
        </div>
        <div style="clear:both;"></div>
            <uc1:Footer runat="server" ID="Footer" />
        </div>


                    
            <script type="text/javascript">
                $(document).ready(function () {
                    $('#divAlbumList').magnificPopup({
                        delegate: 'a',
                        type: 'image',
                        closeOnContentClick: false,
                        closeBtnInside: false,
                        mainClass: 'mfp-with-zoom mfp-img-mobile',
                        image: {
                            verticalFit: true,
                            titleSrc: function (item) {
                                // return item.el.attr('title') + ' &middot; <a class="image-source-link" href="' + item.el.attr('data-source') + '" target="_blank">image source</a>';

                                return "<span style='font-family: Arial;font-size: 9pt;'>" + item.el.attr('title') + "</span>";
                            }
                        },
                        gallery: {
                            enabled: true
                        },
                        zoom: {
                            enabled: false,
                            duration: 300, // don't foget to change the duration also in CSS
                            opener: function (element) {
                                return element.find('img');
                            }
                        }

                    });
                });


            </script>
            <style type="text/css">
                .image-source-link {
                    color: #98C3D1;
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


<div style="position: absolute; left: 15%;width: 80%;   top: 0px; ">

    <div id="myModal" class="reveal-modal">
        <h2 style="margin-top:0px;margin-bottom:10px;font-size:18pt;">Suggestion Box</h2>
        <p style="font-size:10pt;">
            If you have any suggestions on how we can improve our work and workplace environment, please share your ideas with us by completing the form below.

        </p>
        <a class="close-reveal-modal">&#215;</a>
        <table  style="width: 100%;font-size:10pt;">
            <tr style="display:none;">
                <td>
                    Name*<br />
                    <input type="text" id="txtSuggestionName" style="width:180px;" value="<%=Session["LOGINNAME"] %>" disabled="disabled" />
                </td>
                <td>
                    Email Address*<br />
                    <input type="text" id="txtSuggestionEmail" style="width:180px;" value="<%=Session["EMAIL"] %>" disabled="disabled"/>
                </td>
            </tr>
            <tr style="display:none;">
                <td colspan="2">
                    Type of Feedback*<br />
                    <select id="comSuggestionType" runat="server" style="width:180px;"> 
                    </select>
                </td>
                <td></td>
            </tr> 
            <tr style="display:none;">
                <td> 
                    Tel.<br />
                    <input type="text" style="width:120px;" runat="server" id="txtSuggestionTel" validate="phone" />
                </td>
                <td> 
                    Other Email<br />
                    <input type="text" style="width:180px;" runat="server" id="txtSuggestionOtherEmail" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Comments 
                    <textarea style="width: 100%; height: 80px;" runat="server" id="txtSuggestion"></textarea>
                    <label style="color:blue; display:none;">Remarks:  The fields marked with "*" are required.</label>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center; ">
                    <input type="button" value="Submit" id="btnSubmitSuggestion" style="padding: 5px 25px 5px 25px;"/> 
                    <span id="labWaiting" style="font-weight:bold;">Please Wait...</span>
                </td>
            </tr>
        </table>
    </div>

</div>

  
  
    </form> 
</body>
</html>

