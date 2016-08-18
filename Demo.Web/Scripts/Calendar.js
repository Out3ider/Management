$.fn.MakeCalendar = function (settings) {
    var item = $(this);
    var today = new Date();
    settings = jQuery.extend({
        year: today.getFullYear(),
        month: today.getMonth() + 1,
        day: today.getDate(),
        btnClass: "btn btn-info btn-xs  m_r_5",
        act: null,
        isShowDay: true
    }, settings);
    var ddl = $("<div class='form-inline'>");
    var yearselect = $("<select>").addClass("myYear form-control input-sm").css({ "width": "80px", "margin-bottom": "0", "margin-left": "5px;" });
    var monthslect = $("<select>").addClass("myMonth form-control input-sm").css({ "width": "80px", "margin-bottom": "0", "margin-right": "5px" });
    for (var i = settings.year - 10; i <= settings.year; i++) {
        yearselect.append("<option>" + i.toString() + "</option>");
    }
    yearselect.val(settings.year);
    for (var i = 1; i <= 12; i++) {
        monthslect.append("<option>" + i.toString() + "</option>");
    }
    monthslect.val(settings.month);
    var preMonth = $("<a data-toggle='tooltip' title='上一月'><i class='fa fa-arrow-left' title='上一月'></i></a>").addClass(settings.btnClass).appendTo(ddl);
    yearselect.appendTo(ddl);
    monthslect.appendTo(ddl);
    var nextMonth = $("<a data-toggle='tooltip' title='下一月'> <i class='fa fa-arrow-right'></i> </a>").addClass(settings.btnClass).appendTo(ddl);
    if ($.fn.tooltip) {
        nextMonth.tooltip();
        preMonth.tooltip();
    }
    ///月变更
    function monthChange(type) {
        var xyear = parseInt(yearselect.val());
        var srcYear = xyear;
        var xmonth = parseInt(monthslect.val());
        switch (type) {
            case 'Next':
                xmonth += 1;
                if (xmonth > 12) {
                    xmonth = 1;
                    xyear += 1;
                }
                break;
            case "Pre":
                xmonth -= 1;
                if (xmonth < 1) {
                    xmonth = 12;
                    xyear -= 1;
                }
                break;
        }
        yearselect.val(xyear);
        if (yearselect.val() == null) {
            var option = "<option>" + xyear.toString() + "</option>";
            if (xyear > srcYear)
                yearselect.append(option);
            else
                yearselect.html(option + yearselect.html());
            yearselect.val(xyear);
        }
        monthslect.val(xmonth).change();
    }
    //日变更
    function dayChange(type) {
        var year = $(item).find(".myYear").val();
        var month = $(item).find(".myMonth").val();
        var day = parseInt(item.find(".MyCalendar li.current").html());
        if (!day)
            day = 1;
        var result = new Date(year, month - 1, day);
        switch (type) {
            case 'Next':
                result = result.AddDays(1);
                break;
            case "Pre":
                result = result.AddDays(-1);
                break;
        }
        $(item).find(".myYear").val(result.getFullYear());
        $(item).find(".myMonth").val(result.getMonth() + 1);
        RenderDays(result.getFullYear(), result.getMonth() + 1, result.getDate());
    }
    preMonth.click(function () { monthChange("Pre"); });
    nextMonth.click(function () { monthChange("Next"); });
    var div = $("<div>").addClass("MyCalendar");
    //var preDay = $("<a id='btnPreDay'>上一天</a>").addClass(settings.btnClass).appendTo(div);
    //var nextDay = $("<a id='btnNextDay'>下一天</a>").addClass(settings.btnClass).appendTo(div);
    //preDay.click(function () { dayChange("Pre"); })
    //nextDay.click(function () { dayChange("Next"); })
    var weeks = new Array("周日", "周一", "周二", "周三", "周四", "周五", "周六");
    var head = $("<ul>").addClass('nohover');
    weeks.forEach(function (w) {
        $("<li>").html(w).appendTo(head);
    });
    var body = $("<ul>");
    ddl.appendTo(item);
    if (settings.isShowDay) {
        head.appendTo(div);
        body.appendTo(div);
        div.appendTo(item);
        RenderDays(settings.year, settings.month, settings.day);
    }
    else {
        div.width("180px");
    }
    //选择日期
    item.find(".MyCalendar").on("click", "li.MyDate", function () {
        var year = $(item).find(".myYear").val();
        var month = $(item).find(".myMonth").val();
        var date = $(this).html();
        if (date) {
            var result = new Date(year, month - 1, parseInt(date));
            item.find(".MyCalendar li").removeClass("current");
            $(this).addClass("current");
            if (settings.act)
                settings.act(result.format("yyyy-MM-dd"));
        }
    });
    //年月变动
    $(item).on("change", "select", function () {
        var xyear = $(item).find(".myYear").val();
        var xmonth = $(item).find(".myMonth").val();
        var day = parseInt(item.find(".MyCalendar li.current").html());
        if (!day)
            day = 1;
        RenderDays(xyear, xmonth, day);
    }).change();
    function RenderDays(xyear, xmonth, day) {
        var body = item.find(".MyCalendar ul").eq(1);
        body.html("");
        var result = new Date(xyear, xmonth - 1, day);
        var week = new Date(xyear, xmonth - 1, 1).getDay();
        var days = DateTime.DaysCount(xyear, xmonth);
        if (day > days)
            day = days;
        var counts = (week + days <= 35) ? 35 : 42;
        for (var i = 0; i < counts; i++) {
            $("<li>").html("&nbsp; ").appendTo(body);
        }
        var lis = body.find("li");
        for (var i = 0; i < days; i++) {
            var li = lis.eq(week + i).html((i + 1).toString()).addClass("MyDate");
            if ((i + 1) == day)
                li.addClass("current");
        }
        if (settings.act)
            settings.act(result.format("yyyy-MM-dd"));
    }
    return this;
};
