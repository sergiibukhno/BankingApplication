(function () {

    var DepositController = function ($scope, financialService) {

        var deposit = function () {
            var depositModel = {
                amount: $scope.depositAmount
            };

            financialService.deposit(depositModel).then(function successCallback(response) {
                alert("Operation successfully performed, your current balance is " + response.data);
            }, function errorCallback(response) {
                alert("Operation failed - " + response.data.Message);
            });
        };

        $scope.deposit = deposit;
    };

    bankingApp.controller("DepositController", ["$scope", "financialService", DepositController]);

}());