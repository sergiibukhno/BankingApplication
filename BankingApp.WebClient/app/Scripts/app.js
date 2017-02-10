var bankingApp = angular.module("bankingApp", ['ngRoute']);

bankingApp.config(function ($routeProvider) {
    $routeProvider
    .when("/", {
        templateUrl: 'app/Views/Home.html'
    })
    .when("/Register", {
        templateUrl: 'app/Views/Register.html',
        controller: 'RegisterController'
    })
    .when("/Login", {
        templateUrl: 'app/Views/Login.html',
        controller: 'LoginController'
    })
    .when("/Account", {
        templateUrl: 'app/Views/Account.html',
        controller: 'AccountController',
        authorize: true
    })
    .when("/UserTransactions", {
        templateUrl: 'app/Views/Transactions.html',
        controller: 'TransactionController',
        authorize: true
    })
    .when("/Deposit", {
        templateUrl: 'app/Views/Deposit.html',
        controller: 'DepositController',
        authorize: true
    })
    .when("/Withdraw", {
        templateUrl: 'app/Views/Withdraw.html',
        controller: 'WithdrawController',
        authorize: true
    })
    .when("/Transfer", {
        templateUrl: 'app/Views/Transfer.html',
        controller: 'TransferController',
        authorize: true
    });
});

bankingApp.config(["$httpProvider", "$locationProvider", function ($httpProvider, $locationProvider) {
    $httpProvider.interceptors.push(
        ["$window", function($window) {

                var storage = $window.localStorage;

                return {
                    request: function(config) {
                        var token = storage.getItem("jwtoken");
                        if (token) {
                            config.headers.Authorization = "Bearer" + " " + token;
                        }
                        return config;
                    },

                    response: function(response) {
                        return response;
                    }
                };
            }
        ]);

    $locationProvider.html5Mode(true);
}]);

bankingApp.run(function ($rootScope,$window) {

    var storage = $window.localStorage;

    var logOut = function () {
        storage.removeItem("jwtoken");
        storage.removeItem("userAuth");
        storage.removeItem("currentUser");
        $window.location = '/';
    }

    var validateUser = function () {
        var isAuthenticated = storage.getItem("userAuth");
        if (isAuthenticated) {
            $rootScope.userAuth = isAuthenticated;
            $rootScope.currentUser = storage.getItem("currentUser");
        }
        else {
            $rootScope.userAuth = false;
        }
    }

    $rootScope.logOut = logOut;
    $rootScope.$on('$routeChangeStart', function (evt, to, from) {
        validateUser();
        if (to.authorize === true) {
            if ($rootScope.userAuth) {
                
            } else {
                $window.location = '/';
                throw 302;
            }
        }
    });
});