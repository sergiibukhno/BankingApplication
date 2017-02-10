bankingApp.factory('financialService', function ($http, ConstService) {
    return {
        deposit : function (depositModel) {
            return $http.post(ConstService.serverApiConst + ConstService.depositControllerConst, depositModel);
        },
        getUserBalance : function () {
            return $http.get(ConstService.serverApiConst + ConstService.balanceControllerConst);
        },
        getUserTransactions: function () {
            return $http.get(ConstService.serverApiConst + ConstService.transactionControllerConst);
        },
        getUsers : function () {
            return $http.get(ConstService.serverApiConst + ConstService.userControllerConst);
        },
        transfer: function (transferModel) {
            return $http.post(ConstService.serverApiConst + ConstService.transferControllerConst, transferModel);
        },
        withdraw: function (withdrawModel) {
            return $http.post(ConstService.serverApiConst + ConstService.withdrawControllerConst, withdrawModel);
        }
    };
});