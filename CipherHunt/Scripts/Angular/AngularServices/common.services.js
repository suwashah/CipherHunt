var baseUrl = document.location.origin;
if (baseUrl.indexOf('localhost') > -1) {
    baseUrl = baseUrl;
}
angular.module('common.services', ['ngResource', 'ngSanitize'])
    .constant("appSettings", {
        serverPath: baseUrl + "/Rest"
    });
