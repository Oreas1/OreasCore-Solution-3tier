MainModule
    .controller("POIndenterIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/Inventory/SetUp/POIndenterLoad', //--v_Load
            '/Inventory/SetUp/POIndenterGet', // getrow
            '/Inventory/SetUp/POIndenterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/SetUp/GetInitializedPOIndenter');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'POIndenterIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'POIndenterIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'POIndenterIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_Inv_PurchaseOrder_Indenter = {
            'ID': 0, 'IndenterName': null, 'IndenterAddress': null, 'ContactNo': null, 'ContactPerson': null, 'Email': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseOrder_Indenters = [$scope.tbl_Inv_PurchaseOrder_Indenter];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Inv_PurchaseOrder_Indenter = {
                'ID': 0, 'IndenterName': null, 'IndenterAddress': null, 'ContactNo': null, 'ContactPerson': null, 'Email': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseOrder_Indenter }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_PurchaseOrder_Indenter = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


