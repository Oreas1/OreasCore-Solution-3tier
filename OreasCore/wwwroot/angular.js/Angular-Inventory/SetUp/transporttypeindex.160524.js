MainModule
    .controller("TransportTypeIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/Inventory/SetUp/TransportTypeLoad', //--v_Load
            '/Inventory/SetUp/TransportTypeGet', // getrow
            '/Inventory/SetUp/TransportTypePost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/SetUp/GetInitializedTransportType');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'TransportTypeIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'TransportTypeIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'TransportTypeIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_Inv_TransportType = {
            'ID': 0, 'TypeName': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_TransportTypes = [$scope.tbl_Inv_TransportType];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Inv_TransportType = {
                'ID': 0, 'TypeName': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_TransportType }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_TransportType = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


