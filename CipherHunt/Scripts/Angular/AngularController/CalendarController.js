(function () {
    'use strict';

    angular
        .module('Homeapp')
        .controller('CalendarController', function CalendarController($http,$scope) {
            $scope.GetDates = function () {
                //CalendarService.GetThemeStyle({}, function (response) {
                //    console.log(response);
                    
                //    });

                $http.get('/Admin/DashBoard/GetThemeStyle').then(function (d) {
                    $scope.regdata = d.data;
                    console.log(d);
                }, function (error) {
                    alert('failed');
                });
                    //$scope.CheckSetDate($scope.date, $scope.lastdate,$scope.syschrdate, year,mnth);
                
            };
        });
})();
