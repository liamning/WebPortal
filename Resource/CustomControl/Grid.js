var Grid = function (para) {

    //initialize the grid variables
    var me = this;
    this.$parent = $(para.parent);
    this.fields = para.fields;
    this.checkbox = para.checkbox;
    this.data;
    this.stringBuilder = new StringBuilder();

    //append the table header -- start
    this.stringBuilder.append("<div class='headerDiv' style='width:");
    this.stringBuilder.append(para.width + 17);
    this.stringBuilder.append("px;'><table class='gridviewHeader' style='width: ");
    this.stringBuilder.append(para.width);
    this.stringBuilder.append("px;'><tr>");


    //check box header
    for (var i in me.checkbox) {


        this.stringBuilder.append("<th style='width:");
        this.stringBuilder.append(me.checkbox.width);
        this.stringBuilder.append("px;")

        this.stringBuilder.append(me.checkbox.titleStyle);
        if (me.checkbox.selectAll) {
            this.stringBuilder.append("'><input type='checkbox' /></th>");
        } else {
            this.stringBuilder.append("'></th>");
        }
        break;
    }


    //field headers
    for (var j = 0, field; field = me.fields[j]; j++) {

        this.stringBuilder.append("<th style='cursor: pointer; width:");
        this.stringBuilder.append(field.width);
        this.stringBuilder.append("px;")

        this.stringBuilder.append(field.titleStyle);
        this.stringBuilder.append("' name='");
        this.stringBuilder.append(field.name);
        this.stringBuilder.append("'>");
        this.stringBuilder.append(field.title);
        this.stringBuilder.append("</th>");
    }
    this.stringBuilder.append("</tr></table></div>");
    this.stringBuilder.append("<div class='bodyDiv' style='height: 200px; width: ");
    this.stringBuilder.append(para.width + 17);
    this.stringBuilder.append("px;'><table class='gridview' style='width: ");
    this.stringBuilder.append(para.width);
    this.stringBuilder.append("px;'>");
    this.stringBuilder.append("</table></div>");
    me.$parent.html(this.stringBuilder.toString());
    me.$parent.find("th").click(function () {
        if ($(this).attr('name')) {
            me.sort($(this).attr('name'));
        }
    });
    this.stringBuilder.clear();
    //append the table header -- End

    //get the table body element
    this.$table = me.$parent.find("table.gridview");

    //register the check box click event
    this.$headerCheckbox = undefined;
    me.$headerCheckbox = me.$parent.find("table.gridviewHeader tr th:first-child").find(':checkbox');
    me.$headerCheckbox.click(function () {
        var ischecked = me.$headerCheckbox.is(':checked');
        select(ischecked);
    });

    //function
    this.setData = function (data) {
        me.allData = data;
        me.bindData(data);
    }


    //private method
    var select = function (checked) {
        me.$headerCheckbox.prop('checked', checked);
        me.$table.find("tr td:first-child").each(function () {
            $(this).find(":checkbox").prop('checked', checked);
        });

        for (var i = 0, item; item = me.allData[i]; i++) {
            item.checked = checked;
        }
    }


    this.singleSelect = function () {
        var foundFirstCheckbox = false;

        me.$table.find("tr td:first-child").each(function () {
            var $tmpCheckBox = $(this).find(":checkbox");
            var ischecked = $tmpCheckBox[0].checked;


            if (foundFirstCheckbox && ischecked) {
                $tmpCheckBox.prop('checked', false);
            }

            if (ischecked && !foundFirstCheckbox) {
                foundFirstCheckbox = true;
            }

        });
    }


    this.selectAll = function () {
        select(true);
    }

    this.unSelectAll = function () {
        select(false);
    }


    this.bindData = function (data) {

        me.data = data;
        me.$table.find("tr:gt(0)").remove();
        var headerCheck = me.$headerCheckbox.is(":checked");
        var checked;

        for (var i = 0, item; item = data[i]; i++) {
            this.stringBuilder.append("<tr>");
            checked = headerCheck || item.checked;

            for (var p in me.checkbox) {

                this.stringBuilder.append("<td style='width:");
                this.stringBuilder.append(me.checkbox.width);
                this.stringBuilder.append("px;")
                this.stringBuilder.append(me.checkbox.style);
                this.stringBuilder.append("'>");
                this.stringBuilder.append("<input type='checkbox' ");
                this.stringBuilder.append(checked ? "checked='checked'" : "");
                this.stringBuilder.append(" />");
                this.stringBuilder.append("</td>");
                break;
            }

            for (var j = 0, field; field = me.fields[j]; j++) {
                this.stringBuilder.append("<td style='width:");
                this.stringBuilder.append(field.width);
                this.stringBuilder.append("px;")
                this.stringBuilder.append(field.style);
                this.stringBuilder.append("'><p class='specialP' style='width:");
                this.stringBuilder.append(field.width);
                this.stringBuilder.append("px;white-space: nowrap;  overflow: hidden;'><span>");
                this.stringBuilder.append(item[field.name]);
                this.stringBuilder.append("</span></p></td>");
            }
            this.stringBuilder.append("</tr>");
        }

        me.$table.html(this.stringBuilder.toString());
        me.$table.find(".specialP").find("span").each(function () {

            var $this = $(this);
            var $parent = $this.parent();
            if ($this.width() > $parent.width()) {
                $this.attr('title', $(this).text());
                $parent.css('text-overflow', 'ellipsis');
                //$parent.width($parent.width());
                //$parent.css("float", "left");
                //$parent.after("<p style='float: left;font-weight:bold;'>&nbsp...</p>");
            }

        });
        this.stringBuilder.clear();
        if (me.$table.height() > me.$table.parent().height()) {
            me.$table.find("tr:last td").css("border-bottom", "0px solid gray");
        }
    }

    this.getSelectdData = function () {
        var checkbox = me.checkbox;

        var result = [];
        var data = me.data;

        var i = 0;
        me.$table.find("tr td:first-child").each(function () {
            var ischecked = $(this).find(":checkbox")[0].checked;
            if (ischecked) {
                result.push(data[i]);
            }
            i = i + 1;
        });
        return result;
    }

    this.filter = function (criteria, islike) {
        var dataFiltered = [];

        var flag;
        for (var i = 0, record; record = me.allData[i]; i++) {

            flag = true;
            for (var p in criteria) {
                if (islike) {
                    if (record[p].indexOf(criteria[p]) < 0 && criteria[p] != "All") {
                        flag = false;
                        break;
                    }
                } else {

                    if (record[p] != criteria[p] && criteria[p] != "All") {
                        flag = false;
                        break;
                    }
                }
            }
            if (flag) {
                dataFiltered.push(record);
            }
        }
        me.bindData(dataFiltered);
    }

    this.sort = function (field) {


        var dataWithSorting = [];
        var helperArray = [];
        var dict = {};

        var currentFieldName = undefined;

        var isDatefield = false;
        if (/\d{2}\/\d{2}\/\d{4}/.test(me.data[0][field])) {
            isDatefield = true;
        }

        var tmpArray;
        for (var i = 0, record; record = me.data[i]; i++) {
            currentFieldName = record[field];

            if (isDatefield) {
                tmpArray = currentFieldName.split("/");
                currentFieldName = tmpArray[2] + tmpArray[1] + tmpArray[0];
            }

            helperArray.push(currentFieldName + (i + 10000).toString());
            dict[currentFieldName + (i + 10000).toString()] = record;
        }

        var helperArrayOrigin = helperArray.slice(0);
        helperArray.sort();

        var isAsc = true;
        for (var i = 0, record; record = helperArray[i]; i++) {
            if (record != helperArrayOrigin[i]) {
                isAsc = false;
                break;
            }
        }

        helperArrayOrigin.length = 0;

        if (isAsc) {
            helperArray.reverse();
        }

        for (var i = 0, item; item = helperArray[i]; i++) {
            dataWithSorting.push(dict[item]);
        }

        //delete the reference
        helperArray.length = 0;
        for (var p in dict) {
            delete dict[p];
        }

        dict = undefined;

        me.bindData(dataWithSorting);
    }
}