MainModule
    .controller("InternationalCommercialTermIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/Inventory/SetUp/InternationalCommercialTermLoad', //--v_Load
            '/Inventory/SetUp/InternationalCommercialTermGet', // getrow
            '/Inventory/SetUp/InternationalCommercialTermPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/SetUp/GetInitializedInternationalCommercialTerm');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'InternationalCommercialTermIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'InternationalCommercialTermIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'InternationalCommercialTermIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_Inv_InternationalCommercialTerm = {
            'ID': 0, 'IncotermName': null, 'Abbreviation': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_InternationalCommercialTerms = [$scope.tbl_Inv_InternationalCommercialTerm];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Inv_InternationalCommercialTerm = {
                'ID': 0, 'IncotermName': null, 'Abbreviation': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_InternationalCommercialTerm }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_InternationalCommercialTerm = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


