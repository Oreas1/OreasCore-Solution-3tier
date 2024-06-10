MainModule
    .controller("MeasurementUnitIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/Inventory/SetUp/MeasurementUnitLoad', //--v_Load
            '/Inventory/SetUp/MeasurementUnitGet', // getrow
            '/Inventory/SetUp/MeasurementUnitPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/SetUp/GetInitializedMeasurementUnit');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'MeasurementUnitIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'MeasurementUnitIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'MeasurementUnitIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_Inv_MeasurementUnit = {
            'ID': 0, 'MeasurementUnit': null, 'IsDecimal': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_MeasurementUnits = [$scope.tbl_Inv_MeasurementUnit];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Inv_MeasurementUnit = {
                'ID': 0, 'MeasurementUnit': null, 'IsDecimal': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_MeasurementUnit }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_MeasurementUnit = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


