bankingApp.factory('accountService', function ($http, ConstService) {
    return {
        login: function (user) {
            return $http.post(ConstService.serverApiConst + ConstService.loginControllerConst, user);
        },
        register: function (user) {
            return $http.post(ConstService.serverApiConst + ConstService.registerControllerConst, user);
        }
    };
});