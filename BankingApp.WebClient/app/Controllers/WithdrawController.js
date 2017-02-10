(function () {

    var WithdrawController = function ($scope, financialService) {

        var withdraw = function () {
            var withdrawModel = {
                amount: $scope.withdrawAmount
            };

            financialService.withdraw(withdrawModel)
                    .then(function successCallback(response) {
                        alert("Operation successfully performed, your current balance is " + response.data);
                    }, function errorCallback(response) {
                        alert("Operation failed - " + response.data.Message);
                    });
        };

        $scope.withdraw = withdraw;
    };

    bankingApp.controller("WithdrawController", ["$scope", "financialService", WithdrawController]);

}());