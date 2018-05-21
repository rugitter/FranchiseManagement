var App = angular.module('OrderApp', []);

App.controller('OrderController', function ($scope, OrderService) {

    getOrders();

    function getOrders() {
        OrderService.getOrders()
            .then(function (ords) {
                $scope.orders = ords.data;
                console.log($scope.orders);
            });
    }
});

App.factory('OrderService', ['$http', function ($http) {

    var OrderService = {};

    OrderService.getOrders = function () {
        return $http.get('/Orders/GetAll/');
    };

    return OrderService;
}]);