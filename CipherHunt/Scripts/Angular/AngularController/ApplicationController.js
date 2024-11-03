(function () {
    'use strict';
    angular
        .module('Homeapp')
        .controller('ApplicationController', function ApplicationController($scope, MapServices, $log, $http, appSettings) {
            $scope.Height_Feet = "";
            $scope.Height_Inch = "";
            $scope.Weight = 0;
            $scope.Age = 15;
            $scope.Gender = "M";
            $scope.GraphType = "line";
            $scope.AppConfig = function () {
                MapServices.AppConfig(function (res) {
                    $scope.appdata = res;
                });
            };
            $scope.ApplicationConfig = function () {
                MapServices.ApplicationConfig(function (res) {
                    $scope.application = res;
                });
            };
            $scope.DrawWeeklyGraph = function (scriptDivId) {
                    var gType = $scope.GraphType;
                    var post = $http({
                        method: "GET",
                        url: appSettings.serverPath + '/DrawWeeklyGraph',
                        params: { type: gType }
                    }).then(function (d) {
                        var ct = d.data;
                        $("#" + scriptDivId).html(d.data);
                    }, function (error) {
                        $log.info(error);
                    });
            };
            $scope.NotificationCount = function () {
                setInterval(function () {
                    var post = $http({
                        method: "GET",
                        url: appSettings.serverPath + '/NotificationCount',
                        params: {}
                    }).then(function (d) {
                        var ct = d.data;
                        if (ct > 0) {
                            $('#divNotification').attr("data-original-title", ct + " new notification!");
                            $('#notiMsg').hide();
                            $('.count').show();
                        }
                        else {
                            $('#notiMsg').show();
                            $('.count').hide();
                        }
                        $scope.NotiCount = ct;
                    }, function (error) {
                        $log.info(error);
                    });
                }, 5000);//5 sec
            };
            $scope.NotificationList = function () {
                var post = $http({
                    method: "GET",
                    url: appSettings.serverPath + '/NotificationList',
                    params: {}
                }).then(function (d) {
                    $scope.Notifications = d.data;
                }, function (error) {
                    $log.info(error);
                });
            };
            $scope.CalorieCalculate = function () {
                $("#divResult").hide();
                $("#divMessage").hide();
                $("#divLoader").show();
                var pm = $scope.PaymentMet;
                var rec = $scope.Receiver;
                var transferAmt = $scope.SendAmount;
                var postData = {
                    Gender: $scope.Gender,
                    Height_Feet: $scope.Height_Feet,
                    Height_Inch: $scope.Height_Inch,
                    Weight: $scope.Weight,
                    Age: $scope.Age,
                    ActivityFactor: $scope.ActivityFactor
                };
                var post = $http({
                    method: "POST",
                    url: appSettings.serverPath + '/CalorieCalculate',
                    dataType: 'json',
                    data: postData,
                    headers: { "Content-Type": "application/json; charset=utf-8" }
                }).then(function (d) {
                    $log.info(d.data);
                    var code = d.data.CODE;
                    var msg = d.data.MESSAGE;
                    $scope.CalorieResult = d.data;
                    setTimeout(function () {
                        $("#divLoader").hide();
                    }, 1000);
                    if (code === "0") {
                        setTimeout(function () {
                            $("#divResult").show();
                            $("#divMessage").hide();
                        }, 1000);
                    }
                    else {
                        setTimeout(function () {
                            $("#divResult").hide();
                            $("#divMessage").show();
                        }, 1000);
                    }

                }, function (error) {
                    $log.info(error);
                });
            };
            var removeChart = function removeData(chart) {
                chart.data.labels.pop();
                chart.data.datasets.forEach((dataset) => {
                    dataset.data.pop();
                });
                chart.update();
            };
        });
})();
