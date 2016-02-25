var app = angular.module("entelectFines", ['entelectFines.login',
    'ui.router']);

app.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise("/login");

});