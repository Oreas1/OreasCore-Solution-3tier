MainModule
    .controller("PolicyGeneralIndexCtlr", function ($scope, $window, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model

        init_ViewSetup($scope, $http, '/WPT/Organization/GetInitializedPolicyGeneral');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'PolicyGeneralIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'PolicyGeneralIndexCtlr').Privilege;
                /*init_Filter($scope, data.find(o => o.Controller === 'PolicyGeneralIndexCtlr').WildCard, null, null, null);*/
                $scope.Load();
            }
        };
        

        $scope.tbl_WPT_PolicyGeneral = {
            'ID': 0, 'CalendarYear_StartMonth': 1,
            'CalendarYear_StartDayNoOfEveryMonth': 1, 'CalendarYear_RecreateOnClosingMonth': 1,
            'WageCash_RoundOnesIntoTens': 0, 'MinWDToGenerateWageForFirstMonth': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        $scope.Load = function () {

            var successcallback = function (response) {
                $scope.tbl_WPT_PolicyGeneral = response.data;
            };
            var errorcallback = function (error) { };

            $http({ method: "GET", url: "/WPT/Organization/PolicyGeneralLoad", headers: { 'X-Requested-With': 'XMLHttpRequest'} }).then(successcallback, errorcallback);

        };

        $scope.PostRow = function () {
            var successcallback = function (response) {
                if (response.data === 'OK') {

                    $scope.Load();
                    alert('Updated Successfully');
                }
            };
            var errorcallback = function (error) { };
            if (confirm("Are you sure! you want to update record") === true) {
                $http({
                    method: "POST", url: "/WPT/Organization/PolicyGeneralPost", params: { operation: 'Save Update' }, data: $scope.tbl_WPT_PolicyGeneral, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken  }
                }).then(successcallback, errorcallback);
            }
        };



    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


