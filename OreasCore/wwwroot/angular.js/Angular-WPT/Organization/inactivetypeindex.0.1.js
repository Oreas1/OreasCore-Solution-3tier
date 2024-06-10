MainModule
    .controller("InActiveTypeIndexCtlr", function ($scope, $window, $http) {
   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/WPT/Organization/InActiveTypeLoad', //--v_Load
            '/WPT/Organization/InActiveTypeGet', // getrow
            '/WPT/Organization/InActiveTypePost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Organization/GetInitializedInActiveType');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'InActiveTypeIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'InActiveTypeIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'InActiveTypeIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_WPT_InActiveType = {
            'ID': 0, 'InActiveType': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_InActiveTypes = [$scope.tbl_WPT_InActiveType];


        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_InActiveType = {
                'ID': 0, 'InActiveType': ''
            };

        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_InActiveType }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_InActiveType = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


