/* 
 * http://www.w3schools.com/angular/angular_bootstrap.asp
 */
angular.module('myApp', []).controller('OrderCtrl', function ($scope) {
    $scope.OrderID = '';
    $scope.OrderDate = '';

    $scope.orders = [
        { OrderID: 1, OrderID: '1', OrderDate: "20/5/18" },
        { OrderID: 2, OrderID: '2', OrderDate: "20/5/18" },
        { OrderID: 3, OrderID: '3', OrderDate: "20/5/18" },
        { OrderID: 4, OrderID: '4', OrderDate: "20/5/18" },
        { OrderID: 5, OrderID: '5', OrderDate: "20/5/18" },
        { OrderID: 6, OrderID: '6', OrderDate: "20/5/18" }
    ];
    $scope.view = true;
    $scope.error = false;
    $scope.incomplete = false;

    $scope.viewOrder = function (OrderID) {
        if (OrderID == 'new') {
            $scope.view = true;
            $scope.incomplete = true;
            $scope.OrderID = '';
            $scope.OrderDate = '';
        } else {
            //$scope.edit = false;
            $scope.OrderID = $scope.orders[OrderID - 1].OrderID;
            $scope.OrderDate = $scope.orders[OrderID - 1].OrderDate;
        }
    };


    $scope.$watch('OrderID', function () {
        $scope.test();
    });
    $scope.$watch('OrderDate', function () {
        $scope.test();
    });

    $scope.test = function () {
        
        $scope.incomplete = false;
        if ($scope.view && (!$scope.OrderID.length ||
            !$scope.OrderDate.length )) {
            $scope.incomplete = true;
        }
    };

});

