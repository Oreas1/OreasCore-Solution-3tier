MainModule
    .controller("POManufacturerIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/Inventory/SetUp/POManufacturerLoad', //--v_Load
            '/Inventory/SetUp/POManufacturerGet', // getrow
            '/Inventory/SetUp/POManufacturerPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/SetUp/GetInitializedPOManufacturer');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'POManufacturerIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'POManufacturerIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'POManufacturerIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_Inv_PurchaseOrder_Manufacturer = {
            'ID': 0, 'ManufacturerName': null, 'ManufacturerAddress': null, 'ContactNo': null, 'ContactPerson': null, 'Email': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseOrder_Manufacturers = [$scope.tbl_Inv_PurchaseOrder_Manufacturer];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Inv_PurchaseOrder_Manufacturer = {
                'ID': 0, 'ManufacturerName': null, 'ManufacturerAddress': null, 'ContactNo': null, 'ContactPerson': null, 'Email': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseOrder_Manufacturer }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_PurchaseOrder_Manufacturer = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


