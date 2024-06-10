MainModule
    .controller("QcDashBoardCtlr", function ($scope, $http) {

        $scope.GetDashBoardData = function () {

            var successcallback = function (response) {
                $scope.data = response.data;                
            };
            var errorcallback = function (error) {
                console.log(error);
            };
            $http({ method: "GET", url: "/QC/Home/DashBoardGet", async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
        };

        $scope.GetDashBoardData();

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


