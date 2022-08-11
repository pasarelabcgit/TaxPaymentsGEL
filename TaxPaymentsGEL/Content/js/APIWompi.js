const urlApiWompi = "https://production.wompi.co";


function cargarListaBancos(token) {
    var myHeaders = new Headers();
    let parametro = "";
    myHeaders.append("Authorization", "Bearer "+token);
    var requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
    };
    debugger;
    fetch(urlApiWompi + "/v1/pse/financial_institutions", requestOptions)
        .then(response => response.text()).then(function (result) {
            console.log(result);
            if (result != null) {
                parametro = { "bancos": result }
                $.ajax({
                    type: "POST",
                    url: 'Tax_Payments.aspx/RecibirBancos',
                    data: parametro,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                })
            }
        });
}