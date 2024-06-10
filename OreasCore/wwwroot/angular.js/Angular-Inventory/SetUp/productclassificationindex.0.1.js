MainModule
    .controller("ProductClassificationIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/Inventory/SetUp/ProductClassificationLoad', //--v_Load
            '/Inventory/SetUp/ProductClassificationGet', // getrow
            '/Inventory/SetUp/ProductClassificationPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/SetUp/GetInitializedProductClassification');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'ProductClassificationIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'ProductClassificationIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'ProductClassificationIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_Inv_ProductClassification = {
            'ID': 0, 'ClassName': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_ProductClassifications = [$scope.tbl_Inv_ProductClassification];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Inv_ProductClassification = {
                'ID': 0, 'ClassName': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_ProductClassification }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_ProductClassification = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


