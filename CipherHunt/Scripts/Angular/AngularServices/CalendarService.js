(function () {
    'use strict';
    angular
        .module('common.services')
        .factory('CalendarService', ["$resource", "appSettings", "$http", CalendarService]);


    function CalendarService($resource, appSettings, $http, CalendarService) {
        $http.defaults.headers.post['Content-Type'] = 'application/json';
        return $resource(appSettings.serverPath + "/Home", null,
    {
        GetThemeStyle: {
            method: 'GET',
            isArray: true,
            url: appSettings.serverPath + "/Admin/DashBoard/GetThemeStyle",
            responseType: 'json'
        },
        SetHoliday: {
            method: 'POST',
            isArray: true,
            url: appSettings.serverPath + "/Home/SetHoliday",
            responseType: 'json',
            params: { id: '@holidaydate', month: '@holidaydescription' }
        },
        RemoveHoliday: {
            method: 'POST',
            isArray: true,
            url: appSettings.serverPath + "/Home/RemoveHoliday",
            responseType: 'json',
            params: { id: '@holidaydate', month: '@holidaydescription' }
        }
    });
    }
})();