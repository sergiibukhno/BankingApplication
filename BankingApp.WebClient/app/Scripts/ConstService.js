bankingApp.service("ConstService", function () {
    this.serverApiConst = 'http://localhost:52633/api/';
    this.loginControllerConst = 'Auth/Login';
    this.balanceControllerConst = 'Balance';
    this.transactionControllerConst = 'Transaction';
    this.depositControllerConst = 'Deposit';
    this.withdrawControllerConst = 'Withdraw';
    this.userControllerConst = 'User';
    this.transferControllerConst = 'Transfer';
    this.registerControllerConst = 'Auth/Register';
});