function replaceAll(string, search, replace) {
    return string.split(search).join(replace);
}
function ExisteObj(sObjeto) {
    return (document.all(sObjeto) != null);
}
function checkRut(rut) {
    // Despejar Puntos
    //var valor = rut.value.replace('.', '').replace('-', '');
    var valor = replaceAll(replaceAll(rut.value, '.', ''), '-', '');
    // Despejar Guión

    if (valor.length > 1) {
        // Aislar Cuerpo y Dígito Verificador
        cuerpo = valor.slice(0, -1);
        dv = valor.slice(-1).toUpperCase();

        // Formatear RUN
        rut.value = cuerpo + '-' + dv;
    } else {
        rut.value = valor;
    }
}

function EnterTabNom(obj, e, IdFoco) {
    key = (document.all) ? e.keyCode : e.which;
    if (key == 13) {
        document.getElementById(IdFoco).focus();
        return false;
    }
}
function EnterTab(obj, e) {
    key = (document.all) ? e.keyCode : e.which;
    if (key == 13) {
        foco(obj.id);
        return false;
    }
}

function foco(id) {
    var form = document.forms[0].elements;
    var newtab = 0;
    for (x = 0; x < form.length - 1; x++) {
        newtab = x;
        if (form[x].id == id) {
            break;
        }
    }
    document.forms[0].elements[newtab + 1].focus();
}

function StopEnter(obj, e) {
    key = (document.all) ? e.keyCode : e.which;
    if (key == 13)
        return false;
    else
        return true;
}

function ValNum(e) {
    key = (document.all) ? e.keyCode : e.which;
    if (key < 48 || key > 57)
        if (key == 8)
            return true;
        else
            return false;
}

function ValCaract(e) {
    key = (document.all) ? e.keyCode : e.which;
    if (key < 48 || key > 57)
        if (key < 64 || key > 91)
            if (key < 97 || key > 122)
                if (key == 8)
                    return true;
                else
                    return false;
}

function ValPassword(e) {
    key = (document.all) ? e.keyCode : e.which;
    if (key < 48 || key > 57)
        if (key < 64 || key > 91)
            if (key < 97 || key > 122)
                if (key == 64 || key == 46 || key == 95 || key == 45 || key == 33 || key == 35 || key == 37 || key == 38 || key == 42) // @ . _ - ! # % & *
                    return true;
                else
                    return false;
}

function ValFono(e) {
    key = (document.all) ? e.keyCode : e.which;
    if (key < 48 || key > 57)
        if (key == 8 || key == 32 || key == 43)
            return true;
        else
            return false;
}

function ValObservac(e) {
    key = (document.all) ? e.keyCode : e.which;
    if (key < 48 || key > 57)
        if (key < 64 || key > 91)
            if (key < 97 || key > 122)
                if (key == 8 || key == 13 || key == 32 || key == 44 || key == 45 || key == 46 || key == 64 || key == 95 || key == 193 || key == 201 || key == 205 || key == 209 || key == 211 || key == 218 || key == 225 || key == 233 || key == 237 || key == 241 || key == 243 || key == 250)
                    return true;
                else {
                    if (key == 39) {
                        this.event.target.value += String.fromCharCode(180);
                    }
                    return false;
                }
}

function ValObservacStopEnter(e) {
    key = (document.all) ? e.keyCode : e.which;
    if (key < 48 || key > 57)
        if (key < 64 || key > 91)
            if (key < 97 || key > 122)
                if (key == 8 || key == 32 || key == 44 || key == 45 || key == 46 || key == 64 || key == 95 || key == 193 || key == 201 || key == 205 || key == 209 || key == 211 || key == 218 || key == 225 || key == 233 || key == 237 || key == 241 || key == 243 || key == 250)
                    return true;
                else {
                    if (key == 39) {
                        this.event.target.value += String.fromCharCode(180);
                    }
                    return false;
                }
}

function ValSobreStopEnter(e) {
    key = (document.all) ? e.keyCode : e.which;
    /*NUMEROS, LETRAS, GUION, ESPACION [SOLICITADO POR JR]*/
    if (((key >= 48 && key <= 57) || (key == 8)) || ((key >= 97 && key <= 122) || (key >= 65 && key <= 90) || (key == 45) || (key == 32)))
        return true;
    else
        return false;
}

function ValNombre(e) {
    key = (document.all) ? e.keyCode : e.which;
    if (key < 64 || key > 91)
        if (key < 97 || key > 122)
            if (key == 8 || key == 13 || key == 32 || key == 46 || key == 225 || key == 233 || key == 237 || key == 243 || key == 250 || key == 241 || key == 193 || key == 201 || key == 205 || key == 211 || key == 218 || key == 209)
                return true;
            else
                if (key == 39) {
                    this.event.target.value += String.fromCharCode(180);
                }
    return false;
}

function SoloNumEnteros(obj, e, largo) {
    key = (document.all) ? e.keyCode : e.which;
    if ((key >= 48 && key <= 57) || (key == 8)) {
        if (key == 8)
            return true;
        else {
            if (obj.value.length < largo)
                return true;
            else
                return false;
        }
    } else
        return false;
}

function parseBigInt(obj) {
    obj.value = BigInt(obj.value);
}

