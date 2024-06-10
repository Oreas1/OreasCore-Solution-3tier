MainModule
    .controller("RewardTypeIndexCtlr", function ($scope, $window, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/WPT/Organization/RewardTypeLoad', //--v_Load
            '/WPT/Organization/RewardTypeGet', // getrow
            '/WPT/Organization/RewardTypePost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Organization/GetInitializedRewardType');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'RewardTypeIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'RewardTypeIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'RewardTypeIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_WPT_RewardType = {
            'ID': 0, 'RewardName': '', 'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_RewardTypes = [$scope.tbl_WPT_RewardType];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_RewardType = {
                'ID': 0, 'RewardName': '', 'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_RewardType }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_RewardType = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


