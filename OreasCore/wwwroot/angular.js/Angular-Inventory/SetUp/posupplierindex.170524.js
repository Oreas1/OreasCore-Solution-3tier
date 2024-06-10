MainModule
    .controller("POSupplierIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/Inventory/SetUp/POSupplierLoad', //--v_Load
            '/Inventory/SetUp/POSupplierGet', // getrow
            '/Inventory/SetUp/POSupplierPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/SetUp/GetInitializedPOSupplier');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'POSupplierIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'POSupplierIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'POSupplierIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_Inv_PurchaseOrder_Supplier = {
            'ID': 0, 'SupplierName': null, 'SupplierAddress': null, 'ContactNo': null, 'ContactPerson': null, 'Email': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseOrder_Suppliers = [$scope.tbl_Inv_PurchaseOrder_Supplier];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Inv_PurchaseOrder_Supplier = {
                'ID': 0, 'SupplierName': null, 'SupplierAddress': null, 'ContactNo': null, 'ContactPerson': null, 'Email': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseOrder_Supplier }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_PurchaseOrder_Supplier = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


