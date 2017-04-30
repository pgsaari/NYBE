


$(document).ready(function () {
   
    var chart1 = new CanvasJS.Chart("chartContainer1", {
        title: {
            text: "Price Range by Condition",
            fontWeight: "lighter",
        },
        zoomEnabled: true,
        animationEnabled: true,
        axisY: {
            title: "Price ($)",
        },
        axisX: {
            valueFormatString: "MMM",
        },
        toolTip: {
            content: "<b>Date</b> <span style='\"'color: dimgrey;'\"'>{x}</span> <br/> <span style='\"'color: {color};'\"'><strong>Avg</strong></span> <span style='\"'color: dimgrey;'\"'>${y[2]}</span> <br/> <span style='\"'color: {color};'\"'><strong>Range</strong></span> <span style='\"'color: dimgrey;'\"'>${y[0]} - ${y[1]}</span>",
        },
        data: [{
            type: "rangeSplineArea",
            showInLegend: true,
            name: "New",
            color: "rgba(0,180,0,.6)",
            dataPoints: rangeData[0]
        },
        {
            type: "rangeSplineArea",
            showInLegend: true,
            name: "Excellent",
            color: "rgba(90,180,0,.6)",
            dataPoints: rangeData[1]
        },
        {
            type: "rangeSplineArea",
            showInLegend: true,
            name: "Good",
            color: "rgba(180,180,0,.6)",
            dataPoints: rangeData[2]
        },
        {
            type: "rangeSplineArea",
            showInLegend: true,
            name: "Fair",
            color: "rgba(180,90,0,.6)",
            dataPoints: rangeData[3]
        },
        {
            type: "rangeSplineArea",
            showInLegend: true,
            name: "Bad",
            color: "rgba(180,0,0,.6)",
            dataPoints: rangeData[4]
        }],
        legend: {
            cursor: "pointer",
            itemclick: function (e) {
                if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                    e.dataSeries.visible = false;
                } else {
                    e.dataSeries.visible = true;
                }
                chart1.render();
            }
        }
    });
    chart1.render();

    var chart = new CanvasJS.Chart("chartContainer",
    {
        title: {
            text: "Volume by Condition",
            fontWeight: "lighter",
        },
        zoomEnabled: true,
        animationEnabled: true,
        toolTip: {
            shared: true,
            contentFormatter: function (e) {
                var date = e.entries[0].dataPoint.x;
                var monthNames = [
                    "Jan", "Feb", "Mar",
                    "Apr", "May", "Jun", "Jul",
                    "Aug", "Sep", "Oct",
                    "Nov", "Dec"
                ];
                var day = date.getDate();
                var monthIndex = date.getMonth();
                var year = "" + date.getFullYear();
                var total = 0;
                var str = "<strong>Date</strong> <span style=\"color: dimgrey;\">" + day + " " + monthNames[monthIndex] + " " + year.slice(2) + "</span>";
                for (var i = e.entries.length - 1; i > -1; i--) {
                    if (e.entries[i].dataSeries.visible) {
                        var color = e.entries[i].dataSeries.color;
                        var name = e.entries[i].dataSeries.name;
                        var y = e.entries[i].dataPoint.y;
                        total += y;
                        str += ("<br/>" + "<span style=\"color: " + color + ";\"><strong>" + name + "</strong></span> <span style=\"color: dimgrey;\">" + y + "</span>");
                    }
                };
                str += "<br/> <strong>Total</strong> <text style=\"color: dimgrey;\">" + total + "</text>";
    	        return str;
            }
        },
        axisY: {
            title: "Volume",
        },
        axisX: {
            valueFormatString: "MMM",
        },
        data: [
        {
            type: "stackedColumn",
            showInLegend: true,
            name: "Bad",
            color: "rgba(180,0,0,.6)",
            dataPoints: volumeData[4]
        },
        {
            type: "stackedColumn",
            showInLegend: true,
            name: "Fair",
            color: "rgba(180,90,0,.6)",
            dataPoints: volumeData[3]
        },
        {
            type: "stackedColumn",
            showInLegend: true,
            name: "Good",
            color: "rgba(180,180,0,.6)",
            dataPoints: volumeData[2]
        },
        {
            type: "stackedColumn",
            showInLegend: true,
            name: "Excellent",
            color: "rgba(90,180,0,.6)",
            dataPoints: volumeData[1]
        },
        {
            type: "stackedColumn",
            showInLegend: true,
            name: "New",
            color: "rgba(0,180,0,.6)",
            dataPoints: volumeData[0]
        }],
        legend: {
            cursor: "pointer",
            reversed: true,
            itemclick: function (e) {
                if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                    e.dataSeries.visible = false;
                } else {
                    e.dataSeries.visible = true;
                }
                chart.render();
            }
        },
    });

    chart.render();
});