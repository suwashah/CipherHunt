document.addEventListener("DOMContentLoaded", function (event) {
    const loader = document.getElementById('loader');
    setTimeout(function () { loader.classList.add('fadeOut'); }, 1000);
});
function openChat() {
    document.getElementById("chatBox").style.display = "block";
}

function closeChat() {
    document.getElementById("chatBox").style.display = "none";
}

function moveLoader(barID) {
    if (i === 0) {
        i = 1;
        var elem = document.getElementById(barID);
        var id = setInterval(frame(elem), 10);
    }
}
function frame(elem) {
    var width = 1;
    if (width >= 100) {
        clearInterval(id);
        i = 0;
    } else {
        width++;
        elem.style.width = width + "%";
    }
}