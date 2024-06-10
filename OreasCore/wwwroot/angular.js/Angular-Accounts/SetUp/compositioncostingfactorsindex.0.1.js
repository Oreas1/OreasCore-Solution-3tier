MainModule
    .controller("CompositionCostingFactorsIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/Accounts/SetUp/CompositionCostingFactorsLoad', //--v_Load
            '/Accounts/SetUp/CompositionCostingFactorsGet', // getrow
            '/Accounts/SetUp/CompositionCostingFactorsPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/SetUp/GetInitializedCompositionCostingFactors');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'CompositionCostingFactorsIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'CompositionCostingFactorsIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'CompositionCostingFactorsIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };


        $scope.tbl_Ac_CompositionCostingFactors = {
            'ID': 0, 'FormulaName': '', 'FormulaExpression': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_CompositionCostingFactorss = [$scope.tbl_Ac_CompositionCostingFactors];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Ac_CompositionCostingFactors = {
                'ID': 0, 'FormulaName': '', 'FormulaExpression': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_CompositionCostingFactors }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_CompositionCostingFactors = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


