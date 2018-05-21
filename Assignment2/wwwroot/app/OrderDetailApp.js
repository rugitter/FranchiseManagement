var App = angular.module('OrderDetailApp', []);

App.controller('OrderDetailController', function ($scope, OrderDetailService) {

    getOrderDetails();

    function getOrderDetails() {
        OrderDetailService.getOrderDetails()
            .then(function (ords) {
                $scope.orderItems = ords.data;
                console.log($scope.orderitems);
            });
    }
});

App.factory('OrderDetailService', ['$http', function ($http) {

    var OrderDetailService = {};

    OrderDetailService.getOrderDetails = function () {
        return $http.get('/Orders/Get/1');
    };

    return OrderDetailService;
}]);