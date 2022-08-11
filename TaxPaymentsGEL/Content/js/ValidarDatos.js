var expr = /^[a-zA-Z0-9_\.\-]+@[a-zA-Z0-9\-]+\.[a-zA-Z0-9\-\.]+$/;

$(document).ready(function () {
    $("#BtnGuardar").click(function () {
        var Document = $("#txtDocument").val();
        var Nombre = $("#txtName").val();
        var correo = $("#txtEmail").val();

        if (Document == "") {
            $("#mensaje1").fadeIn("slow");
            return false;
        }
        else {
            $("#mensaje1").fadeOut();

            if ($("#txtDocument").val().length < 7) {
                $("#mensaje2").fadeIn("slow");
                return false;
            }
            else {
                $("#mensaje2").fadeOut();

                if (Nombre == "") {
                    $("#mensaje3").fadeIn("slow");
                    return false;
                }
                else {
                    $("#mensaje3").fadeOut();

                    if ($("#txtName").val().length < 7) {
                        $("#mensaje4").fadeIn("slow");
                        return false;
                    }
                    else {
                        $("#mensaje4").fadeOut();


                        if (!expr.test(correo)) {
                            $("#mensaje5").fadeIn("slow");
                            return false;
                        }
                        else {
                            $("#mensaje5").fadeOut();

                        }
                    }
                }
            }
        }

    });

    /*Las siguientes funciones son una mejora al ejemplo anterior que mostré
     * Si el mensaje se mostró, el usuario tenía que volver a oprimir el boton
     * de registrar para que el error se ocultará (si era el caso).
     *
     *Con estas funciones de keyup, el mensaje de error se muestra y
     * se ocultará automáticamente, si el usuario escribe datos admitidos.
     * Sin necesidad de oprimir de nuevo el boton de registrar.
     *
     * La función keyup lee lo último que se ha escrito y comparamos
     * con nuestras condiciones, si cumple se quita el error.
     *
     * Es cuestión de analizar un poco para entenderlas
     * Cualquier duda, comenten
     * */
    $("#h, #apellidos").keyup(function () {
        if ($(this).val() != "" && expr1.test($(this).val())) {
            $("#mensaje1, #mensaje2").fadeOut();
            return false;
        }
    });

    $("#correo").keyup(function () {
        if ($(this).val() != "" && expr.test($(this).val())) {
            $("#mensaje3").fadeOut();
            return false;
        }
    });

    var valido = false;
    $("#repass").keyup(function (e) {
        var pass = $("#pass").val();
        var re_pass = $("#repass").val();

        if (pass != re_pass) {
            $("#repass").css({ "background": "#F22" }); //El input se pone rojo
            valido = true;
        }
        else if (pass == re_pass) {
            $("#repass").css({ "background": "#8F8" }); //El input se ponen verde
            $("#mensaje4").fadeOut();
            valido = true;
        }
    });//fin keyup repass

});//fin ready