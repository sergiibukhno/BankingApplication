(function () {

    var LoginController = function ($scope, $window, accountService) {

        var storage = $window.localStorage;

        var loginSubmit = function () {
            
            var user = {
                Name: $scope.username,
                Password: $scope.password
            };

            if (!storage.getItem("jwtoken")) {
                accountService.login(user)
                .then(function successCallback(response) {
                    storage.setItem("jwtoken", response.data.responseContent);
                    storage.setItem("userAuth", true);
                    storage.setItem("currentUser", user.Name);
                    $window.location = '/Account';
                }, function errorCallback(response) {
                    alert("Login failed - "+response.data.Message);
                });
            }

        };

        $scope.loginSubmit = loginSubmit;
    };

    bankingApp.controller("LoginController", ["$scope", "$window", "accountService", LoginController]);

}());