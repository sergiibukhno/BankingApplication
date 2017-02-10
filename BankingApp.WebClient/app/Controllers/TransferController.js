(function () {

    var TransferController = function ($scope, financialService) {

        var getUsers = function () {
            financialService.getUsers()
                .then(function successCallback(response) {
                    $scope.users = response.data;
                }, function errorCallback(response) {
                    alert("Users are unavailable - " + response.data.Message);
                });
        };

        var setToUserId = function (id) {
            $scope.toUser = id;
        };

        var transfer = function () {
            var transferModel = {
                amount: $scope.transferAmount,
                toUserId: $scope.toUser
            };

            financialService.transfer(transferModel)
                    .then(function successCallback(response) {
                        alert("Operation successfully performed, your current balance is " + response.data);
                    }, function errorCallback(response) {
                        alert("Operation failed - " + response.data.Message);
                    });
        };

        $scope.getUsers = getUsers;
        $scope.setToUserId = setToUserId;
        $scope.transfer = transfer;
        getUsers();
    };

    bankingApp.controller("TransferController", ["$scope", "financialService", TransferController]);

}());