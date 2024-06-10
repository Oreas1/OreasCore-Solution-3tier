MainModule
    .controller("POTermsConditionsIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/Inventory/SetUp/POTermsConditionsLoad', //--v_Load
            '/Inventory/SetUp/POTermsConditionsGet', // getrow
            '/Inventory/SetUp/POTermsConditionsPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/SetUp/GetInitializedPOTermsConditions');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'POTermsConditionsIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'POTermsConditionsIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'POTermsConditionsIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_Inv_PurchaseOrderTermsConditions = {
            'ID': 0, 'TCName': '', 'TermsCondition': '', 'Note': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseOrderTermsConditionss = [$scope.tbl_Inv_PurchaseOrderTermsConditions];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Inv_PurchaseOrderTermsConditions = {
                'ID': 0, 'TCName': '', 'TermsCondition': '', 'Note': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseOrderTermsConditions }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_PurchaseOrderTermsConditions = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


