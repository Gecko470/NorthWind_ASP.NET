// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function Excel() {

    let numRegistros = document.getElementById("numRegistros").value;
    let arrayRegistros = [];

    for (let i = 0; i < numRegistros; i++) {
        let registro = {
            ProductId: document.getElementById(i + "_ProductId").value,
            ProductName: document.getElementById(i + "_ProductName").value,
            SupplierId: document.getElementById(i + "_SupplierId").value,
            CategoryId: document.getElementById(i + "_CategoryId").value,
            QuantityPerUnit: document.getElementById(i + "_QuantityPerUnit").value,
            UnitPrice: document.getElementById(i + "_UnitPrice").value,
            UnitsInStock: document.getElementById(i + "_UnitsInStock").value,
            UnitsOnOrder: document.getElementById(i + "_UnitsOnOrder").value,
            ReorderLevel: document.getElementById(i + "_ReorderLevel").value,
            Discontinued: document.getElementById(i + "_Discontinued").value,
        }

        arrayRegistros.push(registro);
    }

    $.ajax({
        type: "POST",
        url: '/Home/Excel',
        data: { lista: JSON.stringify(arrayRegistros) },
        success: function (rpta) {
            window.location = rpta;
        },
        error: function (req, textStatus, errorThrown) {
            
        }
    });

}

function Word() {

    let numRegistros = document.getElementById("numRegistros").value;
    let arrayRegistros = [];

    for (let i = 0; i < numRegistros; i++) {
        let registro = {
            ProductId: document.getElementById(i + "_ProductId").value,
            ProductName: document.getElementById(i + "_ProductName").value,
            SupplierId: document.getElementById(i + "_SupplierId").value,
            CategoryId: document.getElementById(i + "_CategoryId").value,
            QuantityPerUnit: document.getElementById(i + "_QuantityPerUnit").value,
            UnitPrice: document.getElementById(i + "_UnitPrice").value,
            UnitsInStock: document.getElementById(i + "_UnitsInStock").value,
            UnitsOnOrder: document.getElementById(i + "_UnitsOnOrder").value,
            ReorderLevel: document.getElementById(i + "_ReorderLevel").value,
            Discontinued: document.getElementById(i + "_Discontinued").value,
        }

        arrayRegistros.push(registro);
    }

    $.ajax({
        type: "POST",
        url: '/Home/Word',
        data: { lista: JSON.stringify(arrayRegistros) },
        success: function (rpta) {
            window.location = rpta;
        },
        error: function (req, textStatus, errorThrown) {

        }
    });

}

function Pdf() {

    let numRegistros = document.getElementById("numRegistros").value;
    let arrayRegistros = [];

    for (let i = 0; i < numRegistros; i++) {
        let registro = {
            ProductId: document.getElementById(i + "_ProductId").value,
            ProductName: document.getElementById(i + "_ProductName").value,
            SupplierId: document.getElementById(i + "_SupplierId").value,
            CategoryId: document.getElementById(i + "_CategoryId").value,
            QuantityPerUnit: document.getElementById(i + "_QuantityPerUnit").value,
            UnitPrice: document.getElementById(i + "_UnitPrice").value,
            UnitsInStock: document.getElementById(i + "_UnitsInStock").value,
            UnitsOnOrder: document.getElementById(i + "_UnitsOnOrder").value,
            ReorderLevel: document.getElementById(i + "_ReorderLevel").value,
            Discontinued: document.getElementById(i + "_Discontinued").value,
        }

        arrayRegistros.push(registro);
    }

    $.ajax({
        type: "POST",
        url: '/Home/Pdf',
        data: { lista: JSON.stringify(arrayRegistros) },
        success: function (rpta) {
            var a = document.createElement("a");
            a.href = rpta;
            a.download = "Products.pdf";
            a.click();
        },
        error: function (req, textStatus, errorThrown) {

        }
    });

}


