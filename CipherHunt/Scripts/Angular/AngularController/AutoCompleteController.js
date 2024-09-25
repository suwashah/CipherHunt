var app = angular.module('myapp', ['angucomplete-alt']); //add angucomplete-alt dependency in app  
app.controller('AutoCompleteController', ['$scope', '$http', function ($scope, $http) {
    $scope.Products = [];
    $scope.SelectedProduct = null;
    //event fires when click on textbox  
    $scope.SelectedProduct = function (selected) {
        if (selected) {
            $scope.SelectedProduct = selected.originalObject;
        }
    }
    //Gets data from the Database  
    $http({
        method: 'GET',
        url: '/Products/getAllProducts'
    }).then(function (data) {
        $scope.Products = data.data;
    }, function () {
        alert('Error');
    })
}]);