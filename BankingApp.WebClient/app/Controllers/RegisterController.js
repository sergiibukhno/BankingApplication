(function () {

    var RegisterController = function ($scope, $window, accountService) {

        var storage = $window.localStorage;

        var registerSubmit = function () {

            var user = {
                Name: $scope.username,
                Password: $scope.password
            };

            if (!storage.getItem("jwtoken")) {
                accountService.register(user)
                .then(function successCallback(response) {
                    storage.setItem("jwtoken", response.data.responseContent);
                    storage.setItem("userAuth", true);
                    storage.setItem("currentUser", user.Name);
                    $window.location = '/Account';
                }, function errorCallback(response) {
                    alert("Registration failed - " + response.data.Message);
                });
            }

        };

        $scope.registerSubmit = registerSubmit;
    };

    bankingApp.controller("RegisterController", ["$scope", "$window", "accountService", RegisterController]);

}());