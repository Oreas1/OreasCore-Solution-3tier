MainModule
    .controller("ChartOfAccountsTypeIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/Accounts/SetUp/ChartOfAccountsTypeLoad', //--v_Load
            '/Accounts/SetUp/ChartOfAccountsTypeGet', // getrow
            '/Accounts/SetUp/ChartOfAccountsTypePost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/SetUp/GetInitializedChartOfAccountsType');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'ChartOfAccountsTypeIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'ChartOfAccountsTypeIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'ChartOfAccountsTypeIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_Ac_ChartOfAccounts_Type = {
            'ID': 0, 'AccountType': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_ChartOfAccounts_Types = [$scope.tbl_Ac_ChartOfAccounts_Type];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Ac_ChartOfAccounts_Type = {
                'ID': 0, 'AccountType': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_ChartOfAccounts_Type }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_ChartOfAccounts_Type = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


