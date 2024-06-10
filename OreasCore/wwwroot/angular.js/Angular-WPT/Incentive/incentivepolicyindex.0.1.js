MainModule
    .controller("IncentivePolicyIndexCtlr", function ($scope, $http) {
        $scope.DivHideShow = function (v, itm, div_hide, div_show, scope) {
            if (typeof v !== 'undefined' && v !== '' && v !== null) {
                $scope.$broadcast(v, itm);
            }
            if (typeof scope !== 'undefined' && scope !== '' && scope !== null && typeof scope.$parent.pageNavigation === 'function')
            {
                scope.$parent.pageNavigation('Load');
            }
            
            $("#" + div_hide).hide('slow');
            $("#" + div_show).show('slow');          
        };
        //////////////////////////////entry panel/////////////////////////
        init_Operations($scope, $http,
            '/WPT/Incentive/IncentivePolicyLoad', //--v_Load
            '/WPT/Incentive/IncentivePolicyGet', // getrow
            '/WPT/Incentive/IncentivePolicyPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Incentive/GetInitializedIncentivePolicy');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'IncentivePolicyIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'IncentivePolicyIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'IncentivePolicyIndexCtlr').WildCard, null, null, null); 

                if (data.find(o => o.Controller === 'IncentivePolicyIndexCtlr').Otherdata === null) {
                    $scope.IncentiveTypeList = [];
                    $scope.CalculationMethodList = [];
                }
                else {
                    $scope.IncentiveTypeList = data.find(o => o.Controller === 'IncentivePolicyIndexCtlr').Otherdata.IncentiveTypeList;
                    $scope.CalculationMethodList = data.find(o => o.Controller === 'IncentivePolicyIndexCtlr').Otherdata.CalculationMethodList;
                }

                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'IncentivePolicyDesignationCtlr') != undefined) {
                $scope.$broadcast('init_IncentivePolicyDesignationCtlr', data.find(o => o.Controller === 'IncentivePolicyDesignationCtlr'));
            }
            if (data.find(o => o.Controller === 'IncentivePolicyEmployeeCtlr') != undefined) {
                $scope.$broadcast('init_IncentivePolicyEmployeeCtlr', data.find(o => o.Controller === 'IncentivePolicyEmployeeCtlr'));
            }
        };

        init_EmployeeSearchModalGeneral($scope, $http);

        $scope.tbl_WPT_IncentivePolicy = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(), 'IncentiveName': '',
            'FK_tbl_WPT_IncentiveType_ID': null, 'FK_tbl_WPT_IncentiveType_IDName': '',
            'Amount': 0, 'FK_tbl_WPT_IncentiveType_ID': null, 'FK_tbl_WPT_IncentiveType_IDName': '', 'Factor': 1,
            'OT_MinutesFrom_PerDay': 0, 'OT_MinutesTill_PerDay': 0,
            'OT_AfterShiftMinutesFrom_PerDay': 0, 'OT_AfterShiftMinutesTill_PerDay': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_IncentivePolicys = [$scope.tbl_WPT_IncentivePolicy];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_IncentivePolicy = {
                'ID': 0, 'DocNo': '', 'DocDate': new Date(), 'IncentiveName': '',
                'FK_tbl_WPT_IncentiveType_ID': null, 'FK_tbl_WPT_IncentiveType_IDName': '',
                'Amount': 0, 'FK_tbl_WPT_IncentiveType_ID': null, 'FK_tbl_WPT_IncentiveType_IDName': '', 'Factor': 1,
                'OT_MinutesFrom_PerDay': 0, 'OT_MinutesTill_PerDay': 0,
                'OT_AfterShiftMinutesFrom_PerDay': 0, 'OT_AfterShiftMinutesTill_PerDay': 0,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };

        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_IncentivePolicy };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_WPT_IncentivePolicy = data;
            $scope.tbl_WPT_IncentivePolicy.DocDate = new Date(data.DocDate);
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("IncentivePolicyDesignationCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('IncentivePolicyDesignationCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_IncentivePolicyDesignationCtlr', function (e, itm) {           
            init_Filter($scope, itm.WildCard, null, null, null);
            $scope.DesignationList = itm.Otherdata === null ? [] : itm.Otherdata.DesignationList;
        });

        init_Operations($scope, $http,
            '/WPT/Incentive/IncentivePolicyDesignationLoad', //--v_Load
            '/WPT/Incentive/IncentivePolicyDesignationGet', // getrow
            '/WPT/Incentive/IncentivePolicyDesignationPost' // PostRow
        );

        $scope.tbl_WPT_IncentivePolicyDesignation = {
            'ID': 0, 'FK_tbl_WPT_IncentivePolicy_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Designation_ID': null, 'FK_tbl_WPT_Designation_IDName': '', 'Gender': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_IncentivePolicyDesignations = [$scope.tbl_WPT_IncentivePolicyDesignation];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_IncentivePolicyDesignation = {
                'ID': 0, 'FK_tbl_WPT_IncentivePolicy_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Designation_ID': null, 'FK_tbl_WPT_Designation_IDName': '', 'Gender': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_IncentivePolicyDesignation }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_IncentivePolicyDesignation = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("IncentivePolicyEmployeeCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('IncentivePolicyEmployeeCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_IncentivePolicyEmployeeCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });
        init_Operations($scope, $http,
            '/WPT/Incentive/IncentivePolicyEmployeeLoad', //--v_Load
            '/WPT/Incentive/IncentivePolicyEmployeeGet', // getrow
            '/WPT/Incentive/IncentivePolicyEmployeePost' // PostRow
        );

        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_IncentivePolicyEmployees.FK_tbl_WPT_Employee_ID = item.ID;
                $scope.tbl_WPT_IncentivePolicyEmployees.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_IncentivePolicyEmployees.FK_tbl_WPT_Employee_ID = null;
                $scope.tbl_WPT_IncentivePolicyEmployees.FK_tbl_WPT_Employee_IDName = null;
            }
        };

        $scope.tbl_WPT_IncentivePolicyEmployees = {
            'ID': 0, 'FK_tbl_WPT_IncentivePolicy_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '', 'Apply': true,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_IncentivePolicyEmployeess = [$scope.tbl_WPT_IncentivePolicyEmployees];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_IncentivePolicyEmployees = {
                'ID': 0, 'FK_tbl_WPT_IncentivePolicy_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '', 'Apply': true,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_IncentivePolicyEmployees }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_IncentivePolicyEmployees = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    