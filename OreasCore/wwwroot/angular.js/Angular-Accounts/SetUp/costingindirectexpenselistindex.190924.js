MainModule
    .controller("CostingIndirectExpenseListIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/Accounts/SetUp/CostingIndirectExpenseListLoad', //--v_Load
            '/Accounts/SetUp/CostingIndirectExpenseListGet', // getrow
            '/Accounts/SetUp/CostingIndirectExpenseListPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/SetUp/GetInitializedCostingIndirectExpenseList');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'CostingIndirectExpenseListIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'CostingIndirectExpenseListIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'CostingIndirectExpenseListIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_Ac_CostingIndirectExpenseList = {
            'ID': 0, 'IndirectExpenseName': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_CostingIndirectExpenseLists = [$scope.tbl_Ac_CostingIndirectExpenseList];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Ac_CostingIndirectExpenseList = {
                'ID': 0, 'IndirectExpenseName': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_CostingIndirectExpenseList }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_CostingIndirectExpenseList = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


