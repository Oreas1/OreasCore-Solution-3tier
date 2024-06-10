MainModule
    .controller("OTPolicyIndexCtlr", function ($scope, $window, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/WPT/Organization/OTPolicyLoad', //--v_Load
            '/WPT/Organization/OTPolicyGet', // getrow
            '/WPT/Organization/OTPolicyPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Organization/GetInitializedOTPolicy');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'OTPolicyIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'OTPolicyIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'OTPolicyIndexCtlr').WildCard, null, null, null);
                $scope.CalculationMethodList = data.find(o => o.Controller === 'OTPolicyIndexCtlr').Otherdata === null ? []
                    : data.find(o => o.Controller === 'OTPolicyIndexCtlr').Otherdata.CalculationMethodList;
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_WPT_tbl_OTPolicy = {
            'ID': 0, 'PolicyName': '', 'FixedRate': 0,
            'MaximumLimitRate': 0, 'MultiplyFactor': 1, 'FK_tbl_WPT_CalculationMethod_ID': null, 'FK_tbl_WPT_CalculationMethod_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };
        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_tbl_OTPolicys = [$scope.tbl_WPT_tbl_OTPolicy];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_tbl_OTPolicy = {
                'ID': 0, 'PolicyName': '', 'FixedRate': 0,
                'MaximumLimitRate': 0, 'MultiplyFactor': 1, 'FK_tbl_WPT_CalculationMethod_ID': null, 'FK_tbl_WPT_CalculationMethod_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_tbl_OTPolicy }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_tbl_OTPolicy = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


