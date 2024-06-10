MainModule
    .controller("AllowanceTypeIndexCtlr", function ($scope, $window, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/WPT/Organization/AllowanceTypeLoad', //--v_Load
            '/WPT/Organization/AllowanceTypeGet', // getrow
            '/WPT/Organization/AllowanceTypePost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Organization/GetInitializedAllowanceType');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'AllowanceTypeIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'AllowanceTypeIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'AllowanceTypeIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_WPT_AllowanceType = {
            'ID': 0, 'AllowanceName': '', 'Prefix': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_AllowanceTypes = [$scope.tbl_WPT_AllowanceType];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_AllowanceType = {
                'ID': 0, 'AllowanceName': '', 'Prefix': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_AllowanceType }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_AllowanceType = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


