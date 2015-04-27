$(function () {
    $.ajaxSetup({ cache: false });
});


String.prototype.isValidDateTime = function () {

    var regexString = /(\d{2}((0[1-9]|1[012])(0[1-9]|1\d|2[0-8])|(0[13456789]|1[012])(29|30)|(0[13578]|1[02])31)|([02468][048]|[13579][26])0229)/;
    return regexString.test(this);
}


var convertDate = function (stringdate) {
    var DateRegex = /([^-]*)-([^-]*)-([^-]{2})/;
    var DateRegexResult = stringdate.match(DateRegex);
    var DateResult;
    var StringDateResult = "";

    // try creating a new date in a format that both Firefox and Internet Explorer understand
    try {
        DateResult = new Date(DateRegexResult[2] + "/" + DateRegexResult[3] + "/" + DateRegexResult[1]);
    }

    catch (err) {
        alert(err);
    }
    return DateResult;
}

var validate = function (mandatoryControl, callback) {
    for (var i = 0, item; item = mandatoryControl[i]; i++) {
        if (item.$control.val() == "") {
            alert(item.requiredMsg);
            item.$control.focus();
            return false;
        }
    }

    if (callback) {
        if (!callback()) return false;
    }
    return true;
}


String.prototype.escapeSpecialChars = function () {
    return this.replace(/\\n/g, "\\n")
               .replace(/\\'/g, "\\'")
               .replace(/\\"/g, '\\"')
               .replace(/\\&/g, "\\&")
               .replace(/\\r/g, "\\r")
               .replace(/\\t/g, "\\t")
               .replace(/\\b/g, "\\b")
               .replace(/\\f/g, "\\f");
};



var StringBuilder = function () {
    this.stringArray = [];
    this.append = function (str) {
        this.stringArray.push(str);
    }
    this.clear = function () {
        this.stringArray.length = 0;
    }
    this.toString = function () {
        return this.stringArray.join("");
    }
}