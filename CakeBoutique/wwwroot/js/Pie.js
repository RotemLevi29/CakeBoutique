$(document).ready(function () {
    var catnum = $('#numberOfCategory').val();
    let categories = [];
    categories.push(['Categories', 'Interest']);
    console.log('#' + (1).toString() + " interest")
    for (var i = 0; i < catnum; i++) {
        var name = $('#' + i.toString()).val();
        var num = parseInt($('#' + name).val());
        console.log(name + "  "  + num);
        categories.push([name, num]);
    }

    console.log(categories);

    google.charts.load("current", { packages: ["corechart"] });
    google.charts.setOnLoadCallback(drawChart);
    function drawChart() {
        var data = google.visualization.arrayToDataTable(
            categories
        );

        var options = {
            pieHole: 0.4,
        };

        var chart = new google.visualization.PieChart(document.getElementById('donutchart'));
        chart.draw(data, options);
    }

    //chart 2 -
    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(drawChart1);
    function drawChart1() {
        var data = google.visualization.arrayToDataTable(categories);

        var options = {
            title: 'Company Performance',
            hAxis: { title: 'Category', titleTextStyle: { color: 'red' } }
        };

        var chart = new google.visualization.ColumnChart(document.getElementById('chart_div1'));
        chart.draw(data, options);
    }


});