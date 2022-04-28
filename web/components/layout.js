fetch("header.html")
    .then(stream => stream.text())
    .then(text => define(text));

function define(html){
    class Partial extends HTMLElement{
        constructor(){
            super();
            var shadow = this.attachShadow({mode: 'open'});
            shadow.innerHTML = html;
        }
    }

    customElements.define('partial', Partial);
}