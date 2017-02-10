(function () {

    var TransactionController = function ($scope, financialService) {

        var getUserTransactions = function () {
            financialService.getUserTransactions()
                .then(function successCallback(response) {
                    $scope.transactions = response.data;
                }, function errorCallback(response) {
                    alert("Transactions unavailable - " + response.data.Message);
                });
        };

        $scope.getUserTransactions = getUserTransactions;
        getUserTransactions();
    };

    bankingApp.controller("TransactionController", ["$scope", "financialService", TransactionController]);

}());