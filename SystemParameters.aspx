<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SystemParameters.aspx.cs" Inherits="SystemParameters" %>

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
            $btnAdd = $("#btnAdd");
            $btnUpdate = $("#btnUpdate");
            $btnAbandon = $("#btnAbandon");
            $txtSystemType = $("#txtSystemType");
            $txtItemList = $("#txtItemList");
            $txtItem = $("#txtItem");
            $txtChildItem = $("#txtChildItem");
            $txtChildItem.parent().hide();
            var isMultiDesc = false;

            var requiredMsgSuffix = "required.";

            var checkDuplicate = function (mode) {
                var formData;
                var item;
                if (mode == "update") {
                    var value = itemArray[$txtItemList.val()];

                    for (var p in itemArray) {
                        item = itemArray[p];

                        if (item.id == value.id) continue;

                        if (item.description.toUpperCase() == $txtItem.val().toUpperCase())
                            return true;
                        if (item.cdescription && item.cdescription.toUpperCase() == $txtChildItem.val().toUpperCase())
                            return true;

                    }

                } else {

                    for (var p in itemArray) {
                        item = itemArray[p];

                        if (item.description.toUpperCase() == $txtItem.val().toUpperCase())
                            return true;
                        if (item.cdescription && item.cdescription.toUpperCase() == $txtChildItem.val().toUpperCase())
                            return true;


                    }
                }

                return false;
            }


            var itemArray = {};
            //function list
            var submit = function (formData) {
                var mandatoryControl = [
                       { $control: $txtItem, requiredMsg: "Item Description " + requiredMsgSuffix }
                ];
                if ($txtChildItem.parent().css('display') != 'none') {
                    mandatoryControl.push({ $control: $txtChildItem, requiredMsg: "Child Item Description " + requiredMsgSuffix });
                }
                if (!validate(mandatoryControl)) return;

                $.ajax({
                    type: 'POST',
                    url: "Service/AjaxService.aspx",
                    data: formData,
                    success: function (data) {
                        data = eval('(' + data + ')');
                        if (data.error) {
                            var decoded = $("<div/>").html(data.error).text();
                            alert(decoded);
                            return;
                        }
                        alert(data.message);
                        $txtSystemType.change();
                        $txtItem.val("");
                        $txtChildItem.val("");
                    }
                });
            }
            var defineEventDelegate = function () {
                $txtSystemType.change(function () {
                    var category = $txtSystemType.val();
                    $.ajax({
                        type: 'POST',
                        url: "Service/AjaxService.aspx",
                        data: {
                            action: "getSystemTypeItemList",
                            category: category
                        },
                        success: function (data) {
                            data = eval('(' + data + ')');
                            if (data.error) {
                                var decoded = $("<div/>").html(data.error).text();
                                alert(decoded);
                                return;
                            }
                            $txtItemList.find("option").remove();
                            itemArray = {};
                            $.each(data, function (key, value) {
                                if (value.subsequence != 0) {
                                    itemArray[value.id] = value;
                                    $txtItemList.append($("<option></option>")
                                             .attr("value", value.id)
                                             .text(value.description));
                                    $txtItemList.find("option").last().prop('disabled', value.isabandoned == "True");
                                }
                            });
                            $txtItem.val("");
                            $txtChildItem.val("");
                            if (data[0].ismultidesc == "True") {
                                isMultiDesc = true;
                                $txtChildItem.parent().show();
                            } else {
                                isMultiDesc = false;
                                $txtChildItem.parent().hide();
                            }
                             

                            $txtItemList.find("option").each(function () {
                                $(this).attr('title', $(this).text());
                            });

                        }
                    });
                });
                 
                $txtItemList.change(function () {
                    if (!$txtItemList.val()) return;
                    var value = itemArray[$txtItemList.val()];
                    $txtItem.val(value.description);
                    if (value.ismultidesc == "True") {
                        $txtChildItem.parent().show();
                        $txtChildItem.val(value.cdescription);
                    } else {
                        $txtChildItem.parent().hide();
                        $txtChildItem.val(value.cdescription);
                    }

                });
                $btnAbandon.click(function () {
                    if (!$txtSystemType.val()) return;
                    if (!$txtItemList.val()) return;

                    if (!confirm("Please confirm to abandon this system parameter")) return;
                    var formData = {
                        action: "abandonSystemItem",
                        ID: $txtItemList.val()
                    };
                    submit(formData);
                });
                $btnAdd.click(function () {
                    if (!$txtSystemType.val()) return;
                    if (checkDuplicate("create")) {
                        alert("Item duplicated.");
                        return;
                    }
                    if (!confirm("Please confirm to add this system parameter.")) return;

                    var formData = {
                        action: "addSystemItem",
                        Category: $txtSystemType.val(),
                        CategoryDesc: $txtSystemType.find("option:selected").text(),
                        Description: $txtItem.val(),
                        cDescription: $txtChildItem.val(),
                        IsMultiDesc: isMultiDesc
                    };
                    submit(formData);
                });
                $btnUpdate.click(function () {
                    if (!$txtSystemType.val()) return;
                    if (!$txtItemList.val()) return;
                    if (checkDuplicate("update")) {
                        alert("Item duplicated.");
                        return;
                    }
                    if (!confirm("Please confirm to save this system parameter.")) return;
                    var formData;

                    if ($txtItemList.val()) {
                        var value = itemArray[$txtItemList.val()];
                        formData = {
                            action: "updateSystemItem",
                            Description: $txtItem.val(),
                            cDescription: $txtChildItem.val(),
                            ID: value.id,
                            cID: value.cid
                        };

                        submit(formData);
                    }  
                    
                });

                $txtSystemType.find("option").each(function () {
                    $(this).attr('title', $(this).text());
                });

            }
            var init = function () {
                defineEventDelegate();
            }

            //initialize
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
            <p>Staff Portal > System Parameter Manager</p> 
        </div>
        <div id="center" class="font12pt">
            <table>
                
                <tr>
                    <td colspan="3">
                        <h4 class="bar">System Parameter Manager</h4>         
                    </td> 
                </tr> 
                <tr> 
                    <td colspan="3"> 
                        <div style="text-align:right;">
                        <input type="button" id="btnAdd" value="Add" /> 
                        <input type="button" id="btnUpdate" value="Update"/>
                         <input type="button" id="btnAbandon" value="Abandon" />
                        </div>
                    </td> 
                </tr> 
                <tr>
                    <td class="titleTd" style="width: 100px;">   
                        <span style="">System Types</span><br/>  
                        <asp:ListBox ID="txtSystemType" runat="server" style="margin-top: 5px;height: 200px; width:180px;"></asp:ListBox>
                    </td>
                    <td class="titleTd" style="width: 100px;">  
                        <span style="">System Type Items</span><br/> 
                        
                        <asp:ListBox ID="txtItemList" runat="server" style="margin-top: 5px; height: 200px; width:150px;"></asp:ListBox>
                    </td>
                    <td style="text-align:left; vertical-align:top;">   
                        <div>
                            <span id="labTitle1">Item Description</span><br/> 
                            <input type="text" id="txtItem" class="fileUploadWidth" style="margin-top: 5px;" />
                        </div><br/>
                        
                        <div>
                            <span id="labTitle2">Prefix Serial No.</span><br/>
                            <input type="text" id="txtChildItem" validate="hyphen" class="fileUploadWidth" style="margin-top: 5px;text-transform:uppercase;"  />
                        </div>
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