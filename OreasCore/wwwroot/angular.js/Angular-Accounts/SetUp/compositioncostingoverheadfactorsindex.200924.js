MainModule
    .controller("CompositionCostingOverHeadFactorsMasterCtlr", function ($scope, $http) {
        $scope.DivHideShow = function (v, itm, div_hide, div_show, scope) {
            if (typeof v !== 'undefined' && v !== '' && v !== null) {
                $scope.$broadcast(v, itm);
            }
            if (typeof scope !== 'undefined' && scope !== '' && scope !== null && typeof scope.$parent.pageNavigation === 'function')
            {
                scope.$parent.pageNavigation('Load');
            }
            
            $("#" + div_hide).hide('slow');
            $("#" + div_show).show('slow');          
        };
        //////////////////////////////entry panel/////////////////////////
        init_Operations($scope, $http,
            '/Accounts/SetUp/CompositionCostingOverHeadFactorsMasterLoad', //--v_Load
            '/Accounts/SetUp/CompositionCostingOverHeadFactorsMasterGet', // getrow
            '/Accounts/SetUp/CompositionCostingOverHeadFactorsMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/SetUp/GetInitializedCompositionCostingOverHeadFactors');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'CompositionCostingOverHeadFactorsMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'CompositionCostingOverHeadFactorsMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'CompositionCostingOverHeadFactorsMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'CompositionCostingOverHeadFactorsMasterCtlr').LoadByCard);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'CompositionCostingOverHeadFactorsDetailCtlr') != undefined) {
                $scope.$broadcast('init_CompositionCostingOverHeadFactorsDetailCtlr', data.find(o => o.Controller === 'CompositionCostingOverHeadFactorsDetailCtlr'));
            }
        };        

        $scope.tbl_Ac_CompositionCostingFactorsMaster = {
            'ID': 0, 'GroupName': null, 'IsDefault': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_CompositionCostingFactorsMasters = [$scope.tbl_Ac_CompositionCostingFactorsMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values

            $scope.tbl_Ac_CompositionCostingFactorsMaster = {
                'ID': 0, 'GroupName': null, 'IsDefault': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_CompositionCostingFactorsMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Ac_CompositionCostingFactorsMaster = data; 
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("CompositionCostingOverHeadFactorsDetailCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('CompositionCostingOverHeadFactorsDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first'); 
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_CompositionCostingOverHeadFactorsDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
        });

        init_Operations($scope, $http,
            '/Accounts/SetUp/CompositionCostingOverHeadFactorsDetailLoad', //--v_Load
            '/Accounts/SetUp/CompositionCostingOverHeadFactorsDetailGet', // getrow
            '/Accounts/SetUp/CompositionCostingOverHeadFactorsDetailPost' // PostRow
        );

        $scope.tbl_Ac_CompositionCostingFactorsDetail = {
            'ID': 0, 'FK_tbl_Ac_CompositionCostingOverHeadFactorsMaster_ID': $scope.MasterObject.ID,'FormulaName': '', 'FormulaExpression': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_CompositionCostingFactorsDetails = [$scope.tbl_Ac_CompositionCostingFactorsDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_CompositionCostingFactorsDetail = {
                'ID': 0, 'FK_tbl_Ac_CompositionCostingOverHeadFactorsMaster_ID': $scope.MasterObject.ID, 'FormulaName': '', 'FormulaExpression': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_CompositionCostingFactorsDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_CompositionCostingFactorsDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; }; 

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    