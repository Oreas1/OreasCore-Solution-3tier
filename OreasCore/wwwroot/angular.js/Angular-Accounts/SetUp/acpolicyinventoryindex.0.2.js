MainModule
    .controller("AcPolicyInventoryIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/Accounts/SetUp/AcPolicyInventoryLoad', //--v_Load
            '/Accounts/SetUp/AcPolicyInventoryGet', // getrow
            '/Accounts/SetUp/AcPolicyInventoryPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/SetUp/GetInitializedAcPolicyInventory');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'AcPolicyInventoryIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'AcPolicyInventoryIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'AcPolicyInventoryIndexCtlr').WildCard, null, null, null);
                if (data.find(o => o.Controller === 'AcPolicyInventoryIndexCtlr').Otherdata === null) {
                    $scope.ProductTypeList = [];
                }
                else {
                    $scope.ProductTypeList = data.find(o => o.Controller === 'AcPolicyInventoryIndexCtlr').Otherdata.ProductTypeList;
                }
                $scope.pageNavigation('first');
            }
        };

        init_COASearchModalGeneral($scope, $http);

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_InvName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_Inv = item.ID;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_InvName = item.AccountName;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_COGSName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_COGS = item.ID;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_COGSName = item.AccountName;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_ExpenseName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_Expense = item.ID;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_ExpenseName = item.AccountName;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_InProcessName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_InProcess = item.ID;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_InProcessName = item.AccountName;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_WHT_PurchaseName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_WHT_Purchase = item.ID;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_WHT_PurchaseName = item.AccountName;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_GST_PurchaseName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_GST_Purchase = item.ID;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_GST_PurchaseName = item.AccountName;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_ST_SalesName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_ST_Sales = item.ID;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_ST_SalesName = item.AccountName;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_FST_SalesName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_FST_Sales = item.ID;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_FST_SalesName = item.AccountName;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_WHT_SalesName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_WHT_Sales = item.ID;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_WHT_SalesName = item.AccountName;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_SalesName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_Sales = item.ID;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_SalesName = item.AccountName;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_ExpenseTRName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_ExpenseTR = item.ID;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_ExpenseTRName = item.AccountName;
                }
            }
            else {
                if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_InvName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_Inv = null;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_InvName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_COGSName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_COGS = null;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_COGSName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_ExpenseName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_Expense = null;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_ExpenseName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_InProcessName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_InProcess = null;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_InProcessName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_WHT_PurchaseName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_WHT_Purchase = null;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_WHT_PurchaseName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_GST_PurchaseName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_GST_Purchase = null;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_GST_PurchaseName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_ST_SalesName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_ST_Sales = null;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_ST_SalesName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_FST_SalesName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_FST_Sales = null;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_FST_SalesName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_WHT_SalesName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_WHT_Sales = null;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_WHT_SalesName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_SalesName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_Sales = null;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_SalesName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_ExpenseTRName') {
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_ExpenseTR = null;
                    $scope.tbl_Ac_PolicyInventory.FK_tbl_Ac_ChartOfAccounts_ID_ExpenseTRName = null;
                }
            }

        };

        $scope.tbl_Ac_PolicyInventory = {
            'ID': 0, 'FK_tbl_Inv_ProductType_ID': null, 'FK_tbl_Inv_ProductType_IDName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID_Inv': null, 'FK_tbl_Ac_ChartOfAccounts_ID_InvName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID_COGS': null, 'FK_tbl_Ac_ChartOfAccounts_ID_COGSName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID_Expense': null, 'FK_tbl_Ac_ChartOfAccounts_ID_ExpenseName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID_InProcess': null, 'FK_tbl_Ac_ChartOfAccounts_ID_InProcessName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID_WHT_Purchase': null, 'FK_tbl_Ac_ChartOfAccounts_ID_WHT_PurchaseName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID_GST_Purchase': null, 'FK_tbl_Ac_ChartOfAccounts_ID_GST_PurchaseName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID_WHT_Sales': null, 'FK_tbl_Ac_ChartOfAccounts_ID_WHT_SalesName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID_ST_Sales': null, 'FK_tbl_Ac_ChartOfAccounts_ID_ST_SalesName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID_FST_Sales': null, 'FK_tbl_Ac_ChartOfAccounts_ID_FST_SalesName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID_Sales': null, 'FK_tbl_Ac_ChartOfAccounts_ID_SalesName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID_ExpenseTR': null, 'FK_tbl_Ac_ChartOfAccounts_ID_ExpenseTRName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_PolicyInventorys = [$scope.tbl_Ac_PolicyInventory];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Ac_PolicyInventory = {
                'ID': 0, 'FK_tbl_Inv_ProductType_ID': null, 'FK_tbl_Inv_ProductType_IDName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID_Inv': null, 'FK_tbl_Ac_ChartOfAccounts_ID_InvName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID_COGS': null, 'FK_tbl_Ac_ChartOfAccounts_ID_COGSName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID_Expense': null, 'FK_tbl_Ac_ChartOfAccounts_ID_ExpenseName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID_InProcess': null, 'FK_tbl_Ac_ChartOfAccounts_ID_InProcessName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID_WHT_Purchase': null, 'FK_tbl_Ac_ChartOfAccounts_ID_WHT_PurchaseName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID_GST_Purchase': null, 'FK_tbl_Ac_ChartOfAccounts_ID_GST_PurchaseName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID_WHT_Sales': null, 'FK_tbl_Ac_ChartOfAccounts_ID_WHT_SalesName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID_ST_Sales': null, 'FK_tbl_Ac_ChartOfAccounts_ID_ST_SalesName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID_FST_Sales': null, 'FK_tbl_Ac_ChartOfAccounts_ID_FST_SalesName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID_Sales': null, 'FK_tbl_Ac_ChartOfAccounts_ID_SalesName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID_ExpenseTR': null, 'FK_tbl_Ac_ChartOfAccounts_ID_ExpenseTRName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_PolicyInventory }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_PolicyInventory = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


