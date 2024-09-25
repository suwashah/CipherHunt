angular.module('common.services')
    .factory("MapServices", ["$resource", "appSettings", "$http", MapServices]);
function MapServices($resource, appSettings, $http) {
    $http.defaults.headers.post['Content-Type'] = 'application/json; charset=utf-8';
    return $resource(appSettings.serverPath + "/Home", null,
        {
            AppConfig: {
                method: 'GET',
                isArray: false,
                url: appSettings.serverPath + "/AppConfig",
                responseType: 'json'
            },
            ApplicationConfig: {
                method: 'GET',
                isArray: false,
                url: appSettings.serverPath + "/ApplicationConfig",
                responseType: 'json'
            }
        });
}
