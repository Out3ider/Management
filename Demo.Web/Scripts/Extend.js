
var DictType = {
    家装公司类型: 1,
    小区类型: 2,
    经销商类型: 3,
    网点类型: 4,
    购买方式: 21,
    装修方式: 22,
    装修情况: 23,
    品牌选择因素: 24,
    试压情况: 25,
    管道类型: 26,
    管道规格: 27,
    改约原因: 28,
    漏水原因: 29,
    品牌: 30,
    走管方式: 31
};

function Refresh($data, tool, modal, isInside) {
    var data = $data.data();
    var $select = isInside ? modal.modal.find(data.target) : tool.$Editor.find(data.target);
    var val = $select.val();
    $select.off("ajax.Done").on("ajax.Done", function () {
        $select.val(val);
    }).ajaxAddOptions();
}

function setEmergency(div, data) {
    div.find("#CName").val(data.Emergency.Name);
    div.find("#CPhone").val(data.Emergency.Phone);
    div.find("#CRelation").val(data.Emergency.Relation);
    div.find("#CBirthDay").val(data.Emergency.BirthDay);
    div.find("#CBirthMonth").val(data.Emergency.BirthMonth);
    div.find("#CBirthYear").val(data.Emergency.BirthYear);
    div.find("#CCalendarType").val(data.Emergency.CalendarType);
}

function getEmergency(div) {
    return {
        Name: div.find("#CName").val(),
        Phone: div.find("#CPhone").val(),
        Relation: div.find("#CRelation").val(),
        BirthDay: div.find("#CBirthDay").val(),
        BirthMonth: div.find("#CBirthMonth").val(),
        BirthYear: div.find("#CBirthYear").val(),
        CalendarType: div.find("#CCalendarType").val()
    };
}





//function TypeChang(className, idText, idType) {
//    $(className).on("click", "li", function () {
//        var value = $(this).data("value");
//        var text = $(this).data("text");
//        $(idText).text(text);
//        $(idType).val(value);
//    });
//}

function TimeChange() {
    $(".timeType").on("click", "li", function () {
        var value = $(this).data("value");
        var text = $(this).data("text");
        $("#timeText").text(text);
        $("#selectTime").val(value);
    });
}

function IsShowAdd(id, tool) {
    if (id == "add") {
        $("#btnAdd").click();
    }
    tool.content.on("after.Save", function () {
        if (id == "add") {
            window.close();
        }
    }).on("after.Cancel", function () {
        if (id == "add") {
            window.close();
        }
    });
}

$(function () {
    $(".myScroller").each(function () {
        var height = $(this).data("height");
        if (!height) height = "600px";
        $(this).slimScroll({ height: height });
    });
});

function colorShow() {
    return ['#058DC7', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4', '#12a89d', '#b64c92', '#71b383', '#239db5', '#b0cc78', '#ccd86c', '#67c7bc', '#da4654', '#edb53a', '#8acda4', '#8d5690', '#ebc44f'];
}

function GetRangerDictTYpe(type, act) {
    $.get("/api/RangerDict/GetDicts", { type: type }, function (result) {
        if (result.IsSuccess) {
            $("#TypeList").Link("#TypeListTmpl", result.Data);
            if (result.Data.length == 0) {
                $("#btnAddDict").show();
                ShowError("请去系统设置设置区域字典");
                return;
            }
            if (act)
                act();
        } else {
            ShowError(result.Msg);
        }
    });
}



function GetBaseData() {
    var data = $("#toolbarTable").GetJson();
    var json = $("#TestShow .tab-pane.active .table-toolbar").GetJson();
    var newData = $.extend(json, data);
    return newData;
}

function GetQuery(data, controller, title, div) {
    var tmpl = div.data("tmpl");
    var text = div.data("text");
    var api = "/api/Reports/" + div.data("api") + controller;
    $.get(api, data, function (result) {
        MsgBox.Action(result, function () {
            $("#pageHead").MakeHead(["名称", text]);
            if (result.Data.length > 0) $("#pageShow").Render(tmpl, result.Data);
            else Sail.noData($("#pageShow"));

            var ticks = [];
            var series = {
                name: title,
                colorByPoint: true,
                data: []
            };
            var sum = 0;
            $.each(result.Data, function (i, o) {
                ticks.push(o.Name);
                series.data.push(o.Ratio);
                sum += o.Ratio;
            });
            if (sum == 0) {
                $("#pageBar").html('<div class="no-data"> <img src="/Content/images/no1.png" /><p>暂无数据</p></div>');
            } else {
                $("#pageBar").highcharts({
                    chart: {
                        type: 'column'

                    },
                    colors: colorShow(),
                    title: {
                        text: title
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                            '<td style="padding:0"><b>{point.y:1f} </b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    xAxis: {
                        categories: ticks
                    },
                    yAxis: {
                        title: {
                            text: text
                        }
                    },
                    plotOptions: {
                        column: {
                            pointPadding: 0.2,
                            borderWidth: 0
                        }
                    },
                    series: [series]
                });
            }

        });
    });
}

function GetRateQuery(data, controller, div) {
    var tmpl = div.data("tmpl");
    var api = "/api/GrowthRate/" + div.data("api") + controller;
    $.get(api, data, function (result) {
        console.log(result.Data);
        MsgBox.Action(result, function () {
            $("#pageHead").MakeHead(result.Data.head);
            if (result.Data.listRate.length > 0) $("#pageShow").Render("#RateListTmpl", result.Data.listRate);
            else Sail.noData($("#pageShow"));

            if (result.Data.listRate.length == 0) $("#pageColumn").html('<div class="no-data"> <img src="/Content/images/no1.png" /><p>暂无数据</p></div>');
            else {
                var ticks = [];
                //var series = {
                //    name: "",
                //    data: []
                //};
                var dataSeries = []
                $.each(result.Data.listRate[0].listDec, function (i, o) {
                    ticks.push(o.Month);

                });
                $.each(result.Data.listRate, function (i, o) {
                    var data = []
                    $.each(o.listDec, function (j, l) {
                        data.push(l.Qty);
                    })

                    dataSeries.push({ name: o.Name, data: data });
                });
                console.log(dataSeries)

                $("#pageColumn").highcharts({
                    chart: {
                        type: 'column'

                    },
                    colors: colorShow(),
                    title: {
                        text: "增长率柱状图"
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                            '<td style="padding:0"><b>{point.y:1f} %</b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    xAxis: {
                        categories: ticks
                    },
                    yAxis: {
                        title: {
                            text: "数据增长率"
                        }
                    },
                    plotOptions: {
                        column: {
                            pointPadding: 0.2,
                            borderWidth: 0
                        }
                    },
                    series: dataSeries
                });
            }


        });
    });
}

function RateClick(controller) {
    $("#TabType").on("click", ".nav a", function () {
        $("#TabType .nav a.active").removeClass("active");
        $(this).addClass("active");
        $("#TestShow .tab-pane").removeClass("active");
        $("#TestShow .tab-pane").hide();
        var id = $(this).data("type");
        $("#" + id).addClass("active").show();
        //$("#toolbarTable").ResetForm();
        $("#TestShow .tab-pane.active .table-toolbar").ResetForm();
        GetRateQuery(GetBaseData(), controller, $(this));

    });
}

function btnClick(controller, str) {
    $("#TabType").on("click", ".nav a", function () {
        $("#TabType .nav a.active").removeClass("active");
        $(this).addClass("active");
        $("#TestShow .tab-pane").removeClass("active");
        $("#TestShow .tab-pane").hide();
        var id = $(this).data("type");
        $("#" + id).addClass("active").show();
        $("#toolbarTable").ResetForm();
        GetQuery(GetBaseData(), controller, str, $(this));

    });
}

function GetData(api, id, year, div) {
    $.get("/api/BaseReports/" + div.data("api") + api, { id: id, year: year }, function (result) {

        MsgBox.Action(result, function () {
            var ticks = [];
            var series = {
                name: div.data("text"),
                data: []
            };
            var sum = 0;
            $.each(result.Data, function (i, o) {
                ticks.push(o.Month);
                series.data.push(o.Count);
                sum += o.Count;
            });
            if (sum == 0) {
                div.html('<div class="no-data"> <img src="/Content/images/no1.png" /><p>暂无数据</p></div>');
            } else {
                div.highcharts({
                    chart: {
                        type: 'line'

                    },
                    title: {
                        text: div.data("text")
                    },
                    xAxis: {
                        categories: ticks
                    },
                    yAxis: {
                        title: {
                            text: div.data("text")
                        }
                    },
                    tooltip: {
                        enabled: false,
                        formatter: function () {
                            return '<b>' + this.series.name + '</b><br/>' + this.x + ': ' + this.y + '°C';
                        }
                    },
                    plotOptions: {
                        line: {
                            dataLabels: {
                                enabled: true
                            },
                            enableMouseTracking: false
                        }
                    },
                    series: [series]
                });
            }
        });

    });
}

function GetBuyWayData(api, buyData, title, div) {
    $.get(api, buyData, function (result) {
        MsgBox.Action(result, function () {

            var series = {
                name: title,
                data: []
            };
            var sum = 0;
            $.each(result.Data, function (i, o) {
                series.data.push({ name: o.Name, y: o.Count });
                sum += o.Count;
            });

            if (sum == 0) {
                div.html('<div class="no-data"> <img src="/Content/images/no1.png" /><p>暂无数据</p></div>');
            } else {
                div.highcharts({
                    chart: {
                        type: 'pie'
                    },
                    colors: colorShow(),
                    title: {
                        text: title
                    },
                    tooltip: {
                        pointFormat: '数量 :{point.y}  占比: <b>{point.percentage:.1f}%</b>'
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: { format: '{point.name} :{point.percentage:.1f} %' },
                            showInLegend: true
                        }
                    },
                    series: [series]
                });
            }

        });
    });
}

function TabChange() {


    $(".nav-pills").on("click", "li .nav-link", function () {
        $(".nav-pills li .nav-link").removeClass("active");
        $(this).addClass("active");
        $(".tab-pane").removeClass("active");
        var id = $(this).data("target");
        var $target = $("#" + id);
        $target.addClass("active");
        $target.find(".table-toolbar").initForm();
        $target.find(".btnQuery").click();


    });
}


function ExchangeModel() {
    var pagePointLog = new Sail.Pagination({
        titles: ["姓名", "手机号码", "类型", "原误工补助", "变动误工补助", "变动后误工补助", "备注", "时间"],
        handleName: "/api/BaseReports/GetPointLogListById",
        bodyContainer: $("#pointlog table tbody"),
        headContainer: $("#pointlog table thead"),
        footContainer: $("#pointlog .table-pages"),
        reQueryHandle: $("#pointlog .btnQuery"),
        bodyTmpl: "#PointLogListTmpl",
        getPostKey: function () { return $("#pointlog").GetJson(); }
    });
    $("#pointlog .btnQuery").click(function () {
        pagePointLog.Query(1);
    });

    $("#pointlog .btnReset").click(function () {
        $(".form-group").ResetForm();
        pagePointLog.Query(1);
    });
    return pagePointLog;
}

function GetDitchData(api, id, year) {
    $.get(api, { id: id, year: year }, function (result) {
        MsgBox.Action(result, function () {
            console.log(result.Data.details.length);
            $("#ditch thead").MakeHead(result.Data.head);
            if (result.Data.details.length > 0) {
                $("#ditch tbody").Render("#DitchListTmpl", result.Data.details);
                $("#ditch table tbody").AddSum([[1, 0], [2, 0], [3, 0], [4, 0], [5, 0], [6, 0], [7, 0], [8, 0], [9, 0], [10, 0], [11, 0], [12, 0], [13, 0]]);
            }
            else Sail.noData($("#ditch tbody"));

        });
    });
}

function GetSortPie(data) {
    var series = {
        name: $('#type option:selected').text(),
        colorByPoint: true,
        data: []
    };
    var sum = 0
    $.each(data, function (i, o) {
        series.data.push({ name: o.Name, y: o.Count });
        sum += o.Count;
    });

    if (sum == 0) {
        $("#page").html('<div class="no-data"> <img src="/Content/images/no1.png" /><p>暂无数据</p></div>')


    } else {
        $("#page").highcharts({
            chart: { type: 'pie' },
            colors: colorShow(),
            title: {
                text: $('#type option:selected').text() + "饼状图"
            },
            tooltip: {
                pointFormat: '数量 :{point.y}  占比: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: { format: '{point.name} :{point.percentage:.1f} %' },
                    showInLegend: true
                }
            },
            series: [series]
        });
    }


}

function GeIndexPie(data, title, div) {
    var series = {
        name: title,
        colorByPoint: true,
        data: []
    };
    var sum = 0;
    $.each(data, function (i, o) {
        series.data.push({ name: o.Name, y: o.Count });
        sum += o.Count;
    });
    if (sum == 0) {
        div.html('<div class="no-data"> <img src="/Content/images/no1.png" /><p>暂无数据</p></div>');
    } else {
        div.highcharts({
            chart: { type: 'pie' },
            colors: colorShow(),
            title: {
                text: title + "饼状图"
            },
            tooltip: {
                pointFormat: '数量 :{point.y}  占比: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: { format: '{point.name} :{point.percentage:.1f} %' },
                    showInLegend: true
                }
            },
            series: [series]
        });
    }
}



$(function () {
    $.views.converters({
        hidePhone: function (str) {
            if (str == null) return "";
            if (str.length < 7) return str;
            return "*******" + str.substr(7, 4);
        }
    });

    $(".selectMode").each(function () {
        var $thisMode = $(this);
        $(this).on("click", "li", function () {
            var $this = $(this);
            var value = $this.data("value");
            var text = $this.data("text");
            console.log(text);
            $thisMode.find("span.displayName").text(text);
            $thisMode.find("input[type=hidden]").val(value);
        });
    });
});

function InitDate() {
    $("#DateStart").MakeCalendar({
        year: new Date().getFullYear(),
        month: new Date().getMonth() + 1,
        isShowDay: false,
        act: function (result) {
            var date = DateTime.Parse(result);
            $("#yearStart").val(date.getFullYear());
            $("#monthStart").val(date.getMonth() + 1);
            if (date.getFullYear() > new Date().getFullYear() || ((date.getFullYear() == new Date().getFullYear()) && date.getMonth() > new Date().getMonth())) {
                ShowError("开始时间不能超过当前时间");
                return;
            }
        }
    });
    $("#DateEnd").MakeCalendar({
        year: new Date().getFullYear(),
        month: new Date().getMonth() + 1,
        isShowDay: false,
        act: function (result) {
            var date = DateTime.Parse(result);
            $("#yearEnd").val(date.getFullYear());
            $("#monthEnd").val(date.getMonth() + 1);
            if (date.getFullYear() > new Date().getFullYear() || ((date.getFullYear() == new Date().getFullYear()) && date.getMonth() > new Date().getMonth())) {
                ShowError("结束时间不能超过当前时间");
                return;
            }
        }
    });
}


