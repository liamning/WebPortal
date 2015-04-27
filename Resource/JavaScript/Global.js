
$(function () {


    //window.refreshSystemLink = function () {

    //    $.ajax({
    //        type: 'POST',
    //        url: "Service/AjaxService.aspx",
    //        data: {
    //            action: "getSystemLinkList"
    //        },
    //        success: function (data) {
    //            data = eval('(' + data + ')');


    //            var appendHTML = '<ul class="sub-menu">';
    //            for (var i = 0, item; item = data[i]; i++) {
    //                appendHTML = appendHTML + '<li><a href="javascript: var newWin =  window.open(\'' + item.link + '\', \'\'); newWin.focus();">' + item.name + '</a></li>'
    //            }
    //            appendHTML = appendHTML + '</ul>';
    //            $("#liSystemLink").find('ul').remove();
    //            $("#liSystemLink").append(appendHTML);
    //        }
    //    });
    //}
    //window.refreshSystemLink();


    window.refreshValidation = function ($containner) {
        var $dateInput;
        var $timeInput;
        var $numberInput;
        var $phoneInput;
        var $yearInput;
        var $hyphenInput;

        if ($containner) {
            $dateInput = $containner.find("input[validate=date]");
            $timeInput = $containner.find("input[validate=time]");
            $numberInput = $containner.find("input[validate=number]");
            $phoneInput = $containner.find("input[validate=phone]");
            $yearInput = $containner.find("input[validate=year]");
            $hyphenInput = $containner.find("input[validate=hyphen]");
        } else {
            $dateInput = $("input[validate=date]");
            $timeInput = $("input[validate=time]");
            $numberInput = $("input[validate=number]");
            $phoneInput = $("input[validate=phone]");
            $yearInput = $("input[validate=year]");
            $hyphenInput = $("input[validate=hyphen]");
        }

        $dateInput.mask("99/99/9999").blur(function () {
            var valueArray = $(this).val().split("/");
            if (!valueArray[2]) return;
            var newValue = valueArray[2].substring(2) + valueArray[1] + valueArray[0];
            if (!newValue.isValidDateTime()) {
                $(this).val("");

            } else if (valueArray[2] < "2004") {
                $(this).val("");
            }

        });

        $yearInput.mask("99.99").blur(function () {
            var year = parseFloat($(this).val());
            if (!year) return;
            $(this).val(year);
        }).focusin(function () {
            if ($(this).val() == "__.__" || $(this).val() == "") return;
            var year = parseFloat($(this).val());
            if (!year) return;
            year = 100.001 + year;
            var valueArray = year.toString().split(".");
            valueArray[0] = valueArray[0].substring(1) + ".";
            valueArray[1] = valueArray[1].substring(0, 2);
            $(this).val(valueArray[0] + valueArray[1]);
        }).focusin().blur();

        $timeInput.mask("99:99").blur(function () {
            var valueArray = $(this).val().split(":");
            if (valueArray[0] >= 24) {
                $(this).val("");
            } else if (valueArray[1] >= 60) {
                $(this).val("");
            }
        });


        $("#txtSerialNo1").keydown(function (key) {
            if (key.keyCode != 189 && key.keyCode != 109) return true;
            return false;
        }).keyup(function () {
            var $this = $(this);
            if (/-/.test($this.val())) {
                $this.val($(this).val().replace(/-/g, ""));

            }
        }).change(function () {
            var $this = $(this);
            if (/-/.test($this.val())) {
                $this.val($(this).val().replace(/-/g, ""));

            }
        });
        $hyphenInput.change(function () {
            var $this = $(this);
            if (!/^[^-]+-[^-]+$/.test($this.val())) {
                $this.val("");

            }
        });


        $numberInput.keydown(function (key) {
            if ((event.shiftKey && event.keyCode == 9) || (event.ctrlKey && event.keyCode == 82)) return true;
            if (key.shiftKey) return false;
            if ((key.keyCode >= 48 && key.keyCode <= 57) || key.keyCode == 9 ||
                key.keyCode == 8 || key.keyCode == 37 || key.keyCode == 39
                || key.keyCode == 46 || key.keyCode == 35 || key.keyCode == 36
                || key.keyCode == 144 || (key.keyCode >= 96 && key.keyCode <= 105)) return true;
            return false;
        }).keyup(function () {

            var $this = $(this);
            if (/[^\d]/.test($this.val())) {
                $this.val($(this).val().replace(/[^\d]/g, ""));

            }
        }).change(function () {
            var $this = $(this);
            if (/[^\d]/.test($this.val())) {
                $this.val($(this).val().replace(/[^\d]/g, ""));

            }
        });

        $phoneInput.keydown(function (key) {
            // alert(key.keyCode);

            if ((event.shiftKey && event.keyCode == 9) || (event.ctrlKey && event.keyCode == 82)) return true;
            if ((key.keyCode >= 48 && key.keyCode <= 57 && !key.shiftKey)
                || ((key.keyCode == 57 || key.keyCode == 48) && key.shiftKey)
                || key.keyCode == 9 ||
                key.keyCode == 8 || key.keyCode == 37 || key.keyCode == 39
                || key.keyCode == 109 || (key.keyCode == 189 && !key.shiftKey) || key.keyCode == 32
                || key.keyCode == 46 || key.keyCode == 35 || key.keyCode == 36
                || key.keyCode == 144 || (key.keyCode >= 96 && key.keyCode <= 105)) return true;
            return false;
        }).keyup(function () {
            var $this = $(this);
            if (/[^\d^\-^\(^\)^\s]/.test($this.val())) {
                $this.val($(this).val().replace(/[^\d^\-^\(^\)^\s]/g, ""));

            }
        }).change(function () {
            var $this = $(this);
            if (/[^\d^\-^\(^\)^\s]/.test($this.val())) {
                $this.val($(this).val().replace(/[^\d^\-^\(^\)^\s]/g, ""));

            }
        });



    };

    refreshValidation();

    window.grayOut = function ($parent) {
        if (!$parent) {
            $parent = $('body');
        }
        
        $parent.find("input[type=text][validate=grayout]").blur(function () {
            $this = $(this);
            if ($this.val() == "" || $this.val() == $this.attr('grayout')) {
                $this.val($this.attr('grayout'));
                $this.css('color', 'gray');
            }
            else {

                $this.css('color', '');
            }

        }).focusin(function () {
            $this = $(this);
            $this.css('color', '');
            if ($this.val() == $this.attr('grayout')) {
                $this.val("");
            }
        }).each(function () {
            $this = $(this);
            if ($this.val() == "" || $this.val() == $this.attr('grayout')) {
                $this.val($this.attr('grayout'));
                $this.css('color', 'gray');
            }
            else {

                $this.css('color', '');
            }
        });
    };


    //String.prototype.Htmldecode = function () {
    //    return $('<div/>').html(this).text();
    //};


    window.grayOut();
});