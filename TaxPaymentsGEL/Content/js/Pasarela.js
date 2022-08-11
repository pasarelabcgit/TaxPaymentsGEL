

function Mostrarprogreso() {
    $('#processing-modal').modal({
        backdrop: 'static',
        keyboard: true,
        show: true
    });

}

function HideModal(url) {
    setTimeout(function () {
        $('#processing-modal').modal('hide');
        $('.modal-backdrop').hide();
        $('.modal').hide();
        redirect(url);
    }, 2000);
}

function redirect(url) {
    location.href = url;
}

function ValidarFormulario(GrupoValidar) {
    if (ValidarControles(GrupoValidar)) {
        return true;

    } else {
        return false;

    }
}

function ValidarControles(valGrp) {
    var rtnVal = true;
    for (i = 0; i < Page_Validators.length; i++) {
        if (Page_Validators[i].validationGroup == valGrp) {
            ValidatorValidate(Page_Validators[i]);
            if (!Page_Validators[i].isvalid) { //at least one is not valid.
                MostrarDivErrores(Page_Validators[i], Page_Validators[i].controltovalidate);
                rtnVal = false;
            }
        }
    }
    return rtnVal;
}

function MostrarDivErrores(span, textbox) {
    var textbox = $(document.getElementById(textbox));
    divMostrar = $(textbox).next();
    idspanmostrar = $(span).attr("id");
    var visible = $(span).css("visibility");
    $(span).addClass("mostrar");
    $(divMostrar).removeClass("ocultar").addClass("mostrar");
    $("html, body").animate({ scrollTop: "0px" });
}

function OcultarDivErrores(div) {
    var divOcultar = $(div);
    $(divOcultar).removeClass("mostrar").addClass("ocultar");
}


function nobackbutton() {
    window.location.hash = "no-back-button";

    window.location.hash = "Again-No-back-button" //chrome

    window.onhashchange = function () { window.location.hash = "no-back-button"; }
}

function MostrarMensaje(Titulo, Mensaje, Tipo) {
    setTimeout(function () {
        $('#processing-modal').modal('hide');
        $('.modal-backdrop').hide();
        $('.modal').hide();
        $('#spantitulomensaje').text(Titulo);
        $('#spanmensaje').text(Mensaje);

        var imagen = $("#ImagenMensaje");

        if (Tipo == 1) {
            imagen.attr("src", "/Content/img/informacion.png");
        }

        if (Tipo == 2) {
            imagen.attr("src", "/Content/img/Alert.png");
        }

        if (Tipo == 3) {
            imagen.attr("src", "/Content/img/Error.png");
        }
        $("#myModal").modal("show");
    }, 1000);

}


function Notifications(Url) {
    Lobibox.base.DEFAULTS = $.extend({}, Lobibox.base.DEFAULTS, {
        iconSource: 'fontAwesome'
    });
    Lobibox.notify.DEFAULTS = $.extend({}, Lobibox.notify.DEFAULTS, {
        iconSource: 'fontAwesome'
    });
    Lobibox.notify('success', {
        delay: false,
        img: 'https://registro.pse.com.co/PSEUserRegister/assets/logo-pse.png',
        title: 'Historial de transacciones',
        showClass: 'fadeInDown',
        hideClass: 'fadeUpDown',
        msg: "<a href='" + Url + "' style='color: #fff !important;' id='first'> Si has realizado un pago por medio de <i>pagos en l&iacutenea</i>, puedes conocer el estado de todas tus transacciones en este "
          + "enlace</a>",
    });
}

function myFunction() {
    window.print();
}