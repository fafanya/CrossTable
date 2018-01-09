$(document).ready(function () {

    $("input[id*='TransferQuantity']").inputmask("numeric", {
        radixPoint: ",",
        //groupSeparator: " ",
        digits: 3,
        autoGroup: true,
        prefix: '',
        rightAlign: false,
        oncleared: function () { self.Value(''); }
    });
    $("span[id*='_Total']").inputmask("numeric", {
        radixPoint: ",",
        groupSeparator: " ",
        digits: 2,
        autoGroup: true,
        prefix: '',
        rightAlign: true,
        oncleared: function () { self.Value(''); }
    });
    $("input[id*='TotalQuantityMO']").inputmask("numeric", {
        radixPoint: ",",
        //groupSeparator: " ",
        digits: 3,
        autoGroup: true,
        prefix: '',
        rightAlign: false,
        oncleared: function () { self.Value(''); }
    });
    $("input[id*='TransferQuantity']").on("change", function () {
        /*
		    var tbl = $('#tbl-offers').DataTable();
    var rslt = GetVariantById(dataOffers.offers, tbl.cell(rowIndex, 0).data());

    var costInRub = rslt[0].Cost.replace(",",".").replace(/\s/g,"") * rslt[0].ExchangeRate.replace(",",".").replace(/\s/g,"");
    var total = costInRub * rslt[0].transferQuantity.replace(",",".");
    rslt[0].CostInRub = costInRub.toLocaleString('ru');
    rslt[0].Total = total.toLocaleString('ru');
    $("#variant-CostInRub-" + rowIndex).html(parseFloat(costInRub).toFixed(2).replace(".",","));
    $("#variant-totalRub-" + rowIndex).html(parseFloat(total).toFixed(2).replace(".",","));
		*/
        var costInRub = $(this).closest("td").next("td").find("span").html().replace(/\s/g, "");
        costInRub = costInRub.replace("&nbsp;", "");
        if (costInRub) {
            var totalRow = $(this).closest("td").next("td").next("td").find("span");
            var totalByOneRow = totalRow.html();
            var total = costInRub.replace(",", ".").replace(/\s/g, "") * $(this).val().replace(",", ".");
            totalRow.html(total.toFixed(2).replace(".", ","));

            var temp = this.id.split("_");
            var cagentNum = temp[temp.length - 2];
            if (!isNaN(cagentNum)) {
                var totalAll = $('[id*="1_FOOTER_CELL_2_' + cagentNum + '"]');
                if (totalAll.length > 0) {
                    var totalByAllRows = totalAll.html();
                    totalByAllRows = totalByAllRows.replace("&nbsp;", "");
                    totalByAllRows = totalByAllRows.replace(",", ".").replace(/\s/g, "");

                    totalByOneRow = totalByOneRow.replace("&nbsp;", "");
                    totalByOneRow = totalByOneRow.replace(",", ".").replace(/\s/g, "");
                    totalByAllRows = totalByAllRows - totalByOneRow;

                    totalByAllRows = totalByAllRows + total;
                    totalByAllRows = totalByAllRows.toFixed(2);
                    totalByAllRows = AddSpaces(totalByAllRows.toString());
                    totalAll.html(totalByAllRows.replace(".", ","));
                }
            }
        }
    });

    $("span[id*='_Total']").css('white-space', 'nowrap');
});

function AddSpaces(nStr)
{
    var remainder = nStr.length % 3;
    return (nStr.substr(0, remainder) + nStr.substr(remainder).replace(/(\d{3})/g, ' $1')).trim();
}

function LoadPriorityImg() {
    $("span[id$='Priority']").each(function () {
        switch ($(this).text().toLowerCase()) {
            case "нет":
                $(this).text("");
                //$(this).append("<img src='/_layouts/15/Purchase/Resource/images/rsz_priority_clock.png' alt='срок' title='срок' />");
                break;
            case "срок":
                $(this).text("");
                $(this).append("<img src='/_layouts/15/Purchase/Resource/images/rsz_priority_clock.png' alt='срок' title='срок' />");
                break;
            case "стоимость":
                $(this).text("");
                $(this).append("<img src='/_layouts/15/Purchase/Resource/images/rsz_priority_money.png' alt='стоимость' title='стоимость' />");
                break;
        }
    });
}