var nav_bar_open = true
function fechar_nav_bar(){
    var nav = document.querySelector("#nav_bar")
    var icon_open = document.querySelector("#nav_bar_icon_open")
    var logo_nav_bar = document.querySelector("#logo_nav_bar")
    var perfil_nav_bar = document.querySelector("#perfil_nav_bar")
    var nav_bar_links = document.querySelector("#nav_bar_links")
    var nav_bar_icons_closed = document.querySelector("#div_nav_bar_icons_closed")
    var conteudo_principal = document.querySelector("#conteudo_principal")
    var pesquisar_input = document.querySelector("#Pesquisar")
    var item_container = document.querySelector("#item_container")

    //quando não encontrado na página o pesquisar input será null
    nav_bar_open = !nav_bar_open;
    if(nav_bar_open == true){
        nav_bar_icons_closed.style.display = "none"
        icon_open.className = "fas fa-angle-double-left"
        nav.style.borderRight = "8px solid #b22222"
        nav.style.width = "20%"
        logo_nav_bar.style.display = "flex"
        perfil_nav_bar.style.display = "block"
        nav_bar_links.style.display = "block"
        icon_open.style.fontSize = "5vmin"
        icon_open.style.marginLeft = "90%"
        conteudo_principal.style.marginLeft = "20%"
        conteudo_principal.style.width = "80%"
        if(item_container != null){item_container.style.width = "95%"; item_container.style.marginLeft = "10px"}
        if(pesquisar_input != null){
            pesquisar_input.style.width = "80%"
        }
    }
    else{
        nav.style.borderRight = "89px solid #b22222"
        nav.style.width = "0px"
        icon_open.className = "fas fa-angle-double-right"
        logo_nav_bar.style.display = "none"
        perfil_nav_bar.style.display = "none"
        nav_bar_links.style.display = "none"
        conteudo_principal.style.marginLeft = "0px"
        conteudo_principal.style.width = "100%"
        if(item_container != null){item_container.style.width = "90%"; item_container.style.marginLeft = "93px"}
        if(pesquisar_input != null){
            pesquisar_input.style.width = "70%"
        }
        
        setTimeout(() => {
            icon_open.style.fontSize = "9vmin"
            icon_open.style.marginLeft = "20px"
            nav_bar_icons_closed.style.display = "block"
        }, 1000);
    }
}

function irParaPagina(url, abrirFundo = null){
    window.location.href = url
    if (abrirFundo != null) {
        abrir_fundo(abrirFundo)
    }
}

function abrir_fundo(element, section = null, section2 = null, redirect = null){
    // console.log(element.dataset.section)
    // console.log(element.dataset.section2)
    if (section == null && section2 == null) {
        var fundo_escuro = document.querySelector(element.dataset.section);
        var sub_section = document.querySelector(element.dataset.section2);
    }
    else {
        var fundo_escuro = document.querySelector(section);
        var sub_section = document.querySelector(section2);
    }
    var display = "block"

    if(sub_section.dataset.display == "flex"){
        display = "flex"
    }
    else{
        display = "block"
    }

    if(fundo_escuro.style.display == "none"){
        fundo_escuro.style.display = "block"
    }else{
        fundo_escuro.style.display = "none"
    }

    if(sub_section.style.display == "none"){
        sub_section.style.display = display
    }else{
        sub_section.style.display = "none"
    }

    if (redirect != null) {
        irParaPagina(redirect + "FerramentaID=" + element.dataset.id_ferramenta, element)
    }
}

function openDiv(element, alterDiv = false, width = null, height = null, widthOpen = null, heightOpen = null){
    var div = document.querySelector(String(element.dataset.div))

    if(div.style.display == "none"){
        if(alterDiv == true){
            var parentDiv = div.parentElement
            if(width  != null){
                parentDiv.style.width = widthOpen
            }
            if(height != null){
                parentDiv.style.height = heightOpen
            }
        }
        div.style.display = "block"
        // element.className = "fas fa-caret-down"
       
    }
    else{
        if(alterDiv == true){
            var parentDiv = div.parentElement
            if(width  != null){
                parentDiv.style.width = width
            }
            if(height != null){
                parentDiv.style.height = height
            }
        }
        div.style.display = "none"
        // element.className = "fas fa-sort-up"
    }

    if(element.className == "fas fa-sort-up"){
        element.className = "fas fa-caret-down"
    }
    else{
        element.className = "fas fa-sort-up"
    }
}

function AdicionarRemoverBtn(onde) {
    if (onde == 1) {
        var link = document.querySelector("#remover_ferramenta_a");

        if (link != null) {
            for (var a = 0; a < link.length; a++) {
                var icon = document.createElement("I");
                icon.setAttribute("class", "fas fa-cog");
                icon.setAttribute("id", "item_carrinho_remover");
                icon.setAttribute("data-section", "#fundo_escuro");
                icon.setAttribute("data-section2", "#alterar_ferramenta");
                icon.setAttribute("onclick", "abrir_fundo(this)");
                icon.setAttribute("style", "margin-left: 50%; font-size: 5vmin");

                link[a].appendChild(icon);
            }
        }
    }
}

function alterarPagamento(element) {
    var elemento = element
    var cartao_section = document.querySelector('#cartao_credito_selected');
    var boleto_section = document.querySelector('#boleto_selected');
    var pix_section = document.querySelector('#pix_section');

    if (elemento.value == 'cartao_credito') {
        cartao_section.style.display = "block";
        boleto_section.style.display = "none";
        pix_section.style.display = "none";
    }
    else if (elemento.value == "boleto") {
        cartao_section.style.display = "none";
        boleto_section.style.display = "block";
        pix_section.style.display = "none";
    }
    else {
        cartao_section.style.display = "none";
        boleto_section.style.display = "none";
        pix_section.style.display = "block";
    }
}

function alterarParcelas(element, value) {
    var elemento = element;
    var parcelas_valor = document.querySelector("#parcelado");

    var parcela = (Number(value).toFixed(2) / elemento.value).toFixed(2)

    parcelas_valor.innerHTML = elemento.value + "x de R$" + parcela;


}

function alertar(string) {
    alert(string);
}

function Dizer(elemento) {
    const artyom = new Artyom();

    artyom.initialize({
        lang: 'pt-BR',
        continuos: false,
        debug: false,
        listen: false
    });

    var title = document.querySelector(elemento.dataset.question);
    var paragraph = document.querySelector(elemento.dataset.questionp);
    artyom.say(title.innerHTML);
    artyom.fatality();
    artyom.say(paragraph.innerHTML);
    artyom.fatality();
}

function AlterarEnvioSuporte(elemento) {
    if (elemento.value == "Wpp") {
        var section = document.querySelector("#EnviarWpp");
        var section2 = document.querySelector("#EnviarEmail");
        section.style.display = "block";
    }
    else {

    }
}

function rdcNovaPagina(url) {
    var mensagem = document.querySelector("#mensagem_area");

    var win = window.open(url + mensagem.value, '_blank');

    mensagem.value = "";

    win.focus();

}

function suporteCod() {
    var email = document.querySelector("#RedefEmailWpp");
    var Wpp = document.querySelector("#nWpp");
    var errorEmail = document.querySelector("#ErrorEmail");
    var errorWpp = document.querySelector("#ErrorWpp");
    var redirecionar = true;
    var cod = Math.floor(Math.random() * 89999) + 10000;

    if (email.value.length <= 0) {
        errorEmail.style.display = "block";
        errorEmail.innerHTML = "O campo E-mail é obrigatório!";
        redirecionar = false;
    }
    else {
        errorEmail.style.display = "none";
    }
    if (Wpp.value.length <= 0) {
        errorWpp.style.display = "block";
        errorWpp.innerHTML = "O número do WhatsApp é obrigatório!";
        redirecionar = false;
    }
    else {
        errorWpp.style.display = "none";
    }

    if (redirecionar) {
        errorWpp.style.display = "none";
        errorEmail.style.display = "none";

        //irParaPagina('/Home/RSenhaW?Email=' + email.value +'&Numero=' + Wpp.value + '&Cod=' + cod);

        var win = window.open("https://wa.me/5511959786924?text=" + "Olá, o código de segurança para redefinir sua senha é: " + cod, '_blank');

        win.focus();
    }
}

function alterarMetodo(element) {
    var metodo = document.querySelector("#EmailMetodo");
    var metodo2 = document.querySelector("#WppMetodo");
    if (element.value == "E") {
        metodo.style.display = "block";
        metodo2.style.display = "none";
    }
    else {
        metodo2.style.display = "block";
        metodo.style.display = "none";
    }
}

function irParaAbrir(url, element) {
    var metodo = document.querySelector("#" + element);
    metodo.style.display = "block";
    window.location.href = url;
}