(function () {

    var AccountController = function ($scope, financialService) {

        var getUserBalance = function () {
            financialService.getUserBalance().then(function successCallback(response) {
                $scope.userBalance = response.data;
            }, function errorCallback(response) {
                alert("Balance is unavailable - " + response.data.Message);
            });
        };

        $scope.getUserBalance = getUserBalance;
        getUserBalance();
    };

    bankingApp.controller("AccountController", ["$scope", "financialService", AccountController]);

}());