
function fnExportCsv(name, tableSelector = '#table-master-data') {
    //var html = document.querySelector(tableSelector).outerHTML;
    filename = name + ".csv";
    var csv = [];
    var rows = document.querySelectorAll(tableSelector + " tr");
    //var rows = document.querySelectorAll("#table-master-data tr");

    for (var i = 0; i < rows.length; i++) {
        var row = [],
            cols = rows[i].querySelectorAll("td, th");

        for (var j = 0; j < cols.length; j++) row.push(cols[j].innerText.replace(',', ''));

        csv.push(row.join(","));
    }

    // Download CSV
    download_csv(csv.join("\n"), filename);
}


function download_csv(csv, filename) {

    var csvFile;
    var downloadLink;

    // CSV FILE
    csvFile = new Blob([csv], { type: "text/csv" });

    // Download link
    downloadLink = document.createElement("a");

    // File name
    downloadLink.download = filename;

    // We have to create a link to the file
    downloadLink.href = window.URL.createObjectURL(csvFile);

    // Make sure that the link is not displayed
    downloadLink.style.display = "none";

    // Add the link to your DOM
    document.body.appendChild(downloadLink);

    // Lanzamos
    downloadLink.click();
}
