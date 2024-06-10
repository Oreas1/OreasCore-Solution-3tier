MainModule
    .controller("FiscalYearIndexCtlr", function ($scope, $http) {   
        $scope.DivHideShow = function (v, itm, div_hide, div_show, scope) {
            if (typeof v !== 'undefined' && v !== '' && v !== null) {
                $scope.$broadcast(v, itm);
            }
            if (typeof scope !== 'undefined' && scope !== '' && scope !== null && typeof scope.$parent.pageNavigation === 'function') {
                scope.$parent.pageNavigation('Load');
            }

            $("#" + div_hide).hide('slow');
            $("#" + div_show).show('slow');
        };
        //////////////////////////////entry panel/////////////////////////
        init_Operations($scope, $http,
            '/Accounts/SetUp/FiscalYearLoad', //--v_Load
            '/Accounts/SetUp/FiscalYearGet', // getrow
            '/Accounts/SetUp/FiscalYearPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/SetUp/GetInitializedFiscalYear');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'FiscalYearIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'FiscalYearIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'FiscalYearIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'FiscalYearCostingMasterCtlr') != undefined) {
                $scope.$broadcast('init_FiscalYearCostingMasterCtlr', data.find(o => o.Controller === 'FiscalYearCostingMasterCtlr'));
            }
            if (data.find(o => o.Controller === 'FiscalYearCostingDetailCtlr') != undefined) {
                $scope.$broadcast('init_FiscalYearCostingDetailCtlr', data.find(o => o.Controller === 'FiscalYearCostingDetailCtlr'));
            }
        };
        

        init_COASearchModalGeneral($scope, $http);
        var today = new Date();
        $scope.tbl_Ac_FiscalYear = {
            'ID': 0,
            'PeriodStart': new Date(today.getFullYear(), 6, 1, 0, 0, 0, 0),
            'PeriodEnd': new Date(today.getFullYear() + 1, 5, 30, 23, 59, 59, 999),
            'IsClosed': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_FiscalYears = [$scope.tbl_Ac_FiscalYear];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Ac_FiscalYear = {
                'ID': 0,
                'PeriodStart': new Date(today.getFullYear(), 6, 1, 0, 0, 1, 0),
                'PeriodEnd': new Date(today.getFullYear() + 1, 5, 30, 23, 59, 59, 0),
                'IsClosed': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_FiscalYear }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_FiscalYear = data;
            $scope.tbl_Ac_FiscalYear.PeriodStart = new Date(data.PeriodStart);
            $scope.tbl_Ac_FiscalYear.PeriodEnd = new Date(data.PeriodEnd);
       
            if ($scope.ClosingAction == 'Open') {
                $scope.tbl_Ac_FiscalYear.IsClosed = false;
                $scope.ng_readOnly = true;
            }
            else if ($scope.ClosingAction == 'Close') {
                $scope.tbl_Ac_FiscalYear.IsClosed = true;
                $scope.ng_readOnly = true;
            }                
            $scope.ClosingAction = '';
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    })
    .controller("FiscalYearCostingMasterCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('FiscalYearCostingMasterCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_FiscalYearCostingMasterCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            $scope.ClosingTypeList = itm.Otherdata === null ? [] : itm.Otherdata.ClosingTypeList;
        });

        init_Operations($scope, $http,
            '/Accounts/SetUp/FiscalYearCostingMasterLoad', //--v_Load
            '/Accounts/SetUp/FiscalYearCostingMasterGet', // getrow
            '/Accounts/SetUp/FiscalYearCostingMasterPost' // PostRow
        );

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Ac_FiscalYear_ClosingMaster.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                $scope.tbl_Ac_FiscalYear_ClosingMaster.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;
            }
            else {
                $scope.tbl_Ac_FiscalYear_ClosingMaster.FK_tbl_Ac_ChartOfAccounts_ID = null;
                $scope.tbl_Ac_FiscalYear_ClosingMaster.FK_tbl_Ac_ChartOfAccounts_IDName = null;
            }
        };

        $scope.tbl_Ac_FiscalYear_ClosingMaster = {
            'ID': 0, 'FK_tbl_Ac_FiscalYear_ID': $scope.MasterObject.ID,
            'FK_tbl_Ac_FiscalYear_ClosingEntryType_ID': null, 'FK_tbl_Ac_FiscalYear_ClosingEntryType_IDName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
            'TotalDebit': 0, 'TotalCredit': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_FiscalYear_ClosingMasters = [$scope.tbl_Ac_FiscalYear_ClosingMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_FiscalYear_ClosingMaster = {
                'ID': 0, 'FK_tbl_Ac_FiscalYear_ID': $scope.MasterObject.ID,
                'FK_tbl_Ac_FiscalYear_ClosingEntryType_ID': null, 'FK_tbl_Ac_FiscalYear_ClosingEntryType_IDName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
                'TotalDebit': 0, 'TotalCredit': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_FiscalYear_ClosingMaster };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_FiscalYear_ClosingMaster = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("FiscalYearCostingDetailCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('FiscalYearCostingDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_FiscalYearCostingDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });


        init_Operations($scope, $http,
            '/Accounts/SetUp/FiscalYearCostingDetailLoad', //--v_Load
            '/Accounts/SetUp/FiscalYearCostingDetailGet', // getrow
            '/Accounts/SetUp/FiscalYearCostingDetailPost' // PostRow
        );

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Ac_FiscalYear_ClosingDetail.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                $scope.tbl_Ac_FiscalYear_ClosingDetail.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;
            }
            else {
                $scope.tbl_Ac_FiscalYear_ClosingDetail.FK_tbl_Ac_ChartOfAccounts_ID = null;
                $scope.tbl_Ac_FiscalYear_ClosingDetail.FK_tbl_Ac_ChartOfAccounts_IDName = null;
            }

        };

        $scope.tbl_Ac_FiscalYear_ClosingDetail = {
            'ID': 0, 'FK_tbl_Ac_FiscalYear_ClosingMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
            'Debit': 0, 'Credit': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_FiscalYear_ClosingDetails = [$scope.tbl_Ac_FiscalYear_ClosingDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_FiscalYear_ClosingDetail = {
                'ID': 0, 'FK_tbl_Ac_FiscalYear_ClosingMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
                'Debit': 0, 'Credit': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_FiscalYear_ClosingDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_FiscalYear_ClosingDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


