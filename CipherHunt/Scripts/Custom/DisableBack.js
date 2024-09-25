// It works without the History API, but will clutter up the history
var history_api = typeof history.pushState !== 'undefined'
if (history_api) {
    history.pushState(null, '', '#/no-back')
    history.pushState(null, '', '#/stay')
}
else location.hash = '#/stay'
console.log(location.hash);
window.onhashchange = function () {
    console.log(location.hash);
    if (location.hash == '#/no-back') {
        if (history_api) history.pushState(null, '', '#/stay')
        else location.hash = '#/stay'
    }
}