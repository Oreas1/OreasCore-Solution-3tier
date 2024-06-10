MainModule
    .controller("POImportTermIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/Inventory/SetUp/POImportTermLoad', //--v_Load
            '/Inventory/SetUp/POImportTermGet', // getrow
            '/Inventory/SetUp/POImportTermPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/SetUp/GetInitializedPOImportTerm');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'POImportTermIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'POImportTermIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'POImportTermIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_Inv_PurchaseOrder_ImportTerms = {
            'ID': 0, 'TermName': null, 'AtSight': true, 'AtUsance': false, 'AtUsanceDays': 0,'DocumentsForDIL': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseOrder_ImportTermss = [$scope.tbl_Inv_PurchaseOrder_ImportTerms];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Inv_PurchaseOrder_ImportTerms = {
                'ID': 0, 'TermName': null, 'AtSight': true, 'AtUsance': false, 'AtUsanceDays': 0, 'DocumentsForDIL': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.atSightAtUsanceCheckChanged = function (AtSightORAtUsance)
        {

            if ($scope.tbl_Inv_PurchaseOrder_ImportTerms.AtSight === true && AtSightORAtUsance === 'AtSight')
                $scope.tbl_Inv_PurchaseOrder_ImportTerms.AtUsance = false;
            if ($scope.tbl_Inv_PurchaseOrder_ImportTerms.AtSight === false && AtSightORAtUsance === 'AtSight')
                $scope.tbl_Inv_PurchaseOrder_ImportTerms.AtUsance = true;
                

            if ($scope.tbl_Inv_PurchaseOrder_ImportTerms.AtUsance === true && AtSightORAtUsance === 'AtUsance')
                $scope.tbl_Inv_PurchaseOrder_ImportTerms.AtSight = false;
            if ($scope.tbl_Inv_PurchaseOrder_ImportTerms.AtUsance === false && AtSightORAtUsance === 'AtUsance')
                $scope.tbl_Inv_PurchaseOrder_ImportTerms.AtSight = true;
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseOrder_ImportTerms }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_PurchaseOrder_ImportTerms = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


