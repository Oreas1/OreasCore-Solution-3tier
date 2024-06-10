MainModule
    .controller("LeavePolicyIndexCtlr", function ($scope, $window, $http) {
   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/WPT/Leave/LeavePolicyLoad', //--v_Load
            '/WPT/Leave/LeavePolicyGet', // getrow
            '/WPT/Leave/LeavePolicyPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Leave/GetInitializedLeavePolicy');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'LeavePolicyIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'LeavePolicyIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'LeavePolicyIndexCtlr').WildCard, null, null, null);
                if (data.find(o => o.Controller === 'LeavePolicyIndexCtlr').Otherdata === null) {
                    $scope.LeaveCFOptionsList = [];
                    $scope.EmployeeLevelList = [];
                    $scope.EmploymentTypeList = [];
                }
                else {
                    $scope.LeaveCFOptionsList = data.find(o => o.Controller === 'LeavePolicyIndexCtlr').Otherdata.LeaveCFOptionsList;
                    $scope.CalculationMethodList = data.find(o => o.Controller === 'LeavePolicyIndexCtlr').Otherdata.CalculationMethodList;
                    $scope.EncashablePeriodList = data.find(o => o.Controller === 'LeavePolicyIndexCtlr').Otherdata.EncashablePeriodList;
                }
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_WPT_LeavePolicy = {
            'ID': 0, 'PolicyName': '', 'PolicyPrefix': '',
            'Leave': 0, 'WithOutRequest': true, 'MonthlyRestrict_MaxNoOfLeavesCanAvail': 0,
            'FK_tbl_WPT_LeaveCFOptions_ID': null, 'FK_tbl_WPT_LeaveCFOptions_IDName': '',
            'IsHOSApprovalReq': true, 'IsHRApprovalReq': true, 'FinalGranter': null,
            'EncashableLeave': 0, 'EncashablePeriod': '', 'EL_MinBalance': 0,
            'FK_tbl_WPT_CalculationMethod_ID_EL': null, 'FK_tbl_WPT_CalculationMethod_ID_ELName': '', 'Min_WD_Per_ForELMonth': 1,
            'CarryFowardLeave': 0, 'CFL_MinBalance': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_LeavePolicys = [$scope.tbl_WPT_LeavePolicy];


        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_LeavePolicy = {
                'ID': 0, 'PolicyName': '', 'PolicyPrefix': '',
                'Leave': 0, 'WithOutRequest': true, 'MonthlyRestrict_MaxNoOfLeavesCanAvail': 0,
                'FK_tbl_WPT_LeaveCFOptions_ID': null, 'FK_tbl_WPT_LeaveCFOptions_IDName': '',
                'IsHOSApprovalReq': true, 'IsHRApprovalReq': true, 'FinalGranter': null,
                'EncashableLeave': 0, 'EncashablePeriod': $scope.EncashablePeriodList[0].Value, 'EL_MinBalance': 0,
                'FK_tbl_WPT_CalculationMethod_ID_EL': $scope.CalculationMethodList[0].ID, 'FK_tbl_WPT_CalculationMethod_ID_ELName': '', 'Min_WD_Per_ForELMonth': 1,
                'CarryFowardLeave': 0, 'CFL_MinBalance': 0,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };

        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_LeavePolicy }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_LeavePolicy = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


