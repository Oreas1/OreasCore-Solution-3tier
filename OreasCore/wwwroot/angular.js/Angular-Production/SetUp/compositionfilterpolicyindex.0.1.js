MainModule
    .controller("CompositionFilterPolicyMasterCtlr", function ($scope, $http) {
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
            '/Production/SetUp/CompositionFilterPolicyMasterLoad', //--v_Load
            '/Production/SetUp/CompositionFilterPolicyMasterGet', // getrow
            '/Production/SetUp/CompositionFilterPolicyMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Production/SetUp/GetInitializedCompositionFilterPolicy');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'CompositionFilterPolicyMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'CompositionFilterPolicyMasterCtlr').Privilege;
                if (data.find(o => o.Controller === 'CompositionFilterPolicyMasterCtlr').Otherdata === null) {
                    $scope.ProductType = [];
                    $scope.ActionList = [];
                }
                else {
                    $scope.ProductType = data.find(o => o.Controller === 'CompositionFilterPolicyMasterCtlr').Otherdata.ProductType;                   
                    $scope.ActionList = data.find(o => o.Controller === 'CompositionFilterPolicyMasterCtlr').Otherdata.ActionList;
                }
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'CompositionFilterPolicyDetailCtlr') != undefined) {
                $scope.$broadcast('init_CompositionFilterPolicyDetailCtlr', data.find(o => o.Controller === 'CompositionFilterPolicyDetailCtlr'));
            }
        };

        init_WHMSearchModalGeneral($scope, $http);

        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_CompositionFilterPolicyMaster.FK_tbl_Inv_WareHouseMaster_ID_By = item.ID;
                $scope.tbl_Pro_CompositionFilterPolicyMaster.FK_tbl_Inv_WareHouseMaster_ID_ByName = item.WareHouseName;
            }
            else {
                $scope.tbl_Pro_CompositionFilterPolicyMaster.FK_tbl_Inv_WareHouseMaster_ID_By = null;
                $scope.tbl_Pro_CompositionFilterPolicyMaster.FK_tbl_Inv_WareHouseMaster_ID_ByName = null;
            }
        };

        $scope.tbl_Pro_CompositionFilterPolicyMaster = {
            'ID': 0, 'FK_tbl_Inv_ProductType_ID_For_Coupling': null, 'FK_tbl_Inv_ProductType_ID_For_CouplingName': '',
            'FK_tbl_Inv_WareHouseMaster_ID_By': null, 'FK_tbl_Inv_WareHouseMaster_ID_ByName': '',
            'FK_tbl_Inv_ProductType_ID_QCSample': null, 'FK_tbl_Inv_ProductType_ID_QCSampleName': '',
            'FK_tbl_Qc_ActionType_ID_BMRProcessTestingSample': null, 'FK_tbl_Qc_ActionType_ID_BMRProcessTestingSampleName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionFilterPolicyMasters = [$scope.tbl_Pro_CompositionFilterPolicyMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionFilterPolicyMaster = {
                'ID': 0, 'FK_tbl_Inv_ProductType_ID_For_Coupling': null, 'FK_tbl_Inv_ProductType_ID_For_CouplingName': '',
                'FK_tbl_Inv_WareHouseMaster_ID_By': null, 'FK_tbl_Inv_WareHouseMaster_ID_ByName': '',
                'FK_tbl_Inv_ProductType_ID_QCSample': null, 'FK_tbl_Inv_ProductType_ID_QCSampleName': '',
                'FK_tbl_Qc_ActionType_ID_BMRProcessTestingSample': null, 'FK_tbl_Qc_ActionType_ID_BMRProcessTestingSampleName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionFilterPolicyMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Pro_CompositionFilterPolicyMaster = data; 
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("CompositionFilterPolicyDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('CompositionFilterPolicyDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');         

        });

        $scope.$on('init_CompositionFilterPolicyDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
        });

       init_Operations($scope, $http,
            '/Production/SetUp/CompositionFilterPolicyDetailLoad', //--v_Load
            '/Production/SetUp/CompositionFilterPolicyDetailGet', // getrow
            '/Production/SetUp/CompositionFilterPolicyDetailPost' // PostRow
        );

        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_CompositionFilterPolicyDetail.FK_tbl_Inv_WareHouseMaster_ID = item.ID;
                $scope.tbl_Pro_CompositionFilterPolicyDetail.FK_tbl_Inv_WareHouseMaster_IDName = item.WareHouseName;
            }
            else {
                $scope.tbl_Pro_CompositionFilterPolicyDetail.FK_tbl_Inv_WareHouseMaster_ID = null;
                $scope.tbl_Pro_CompositionFilterPolicyDetail.FK_tbl_Inv_WareHouseMaster_IDName = null;
            }
        };

        $scope.tbl_Pro_CompositionFilterPolicyDetail = {
            'ID': 0, 'FK_tbl_Pro_CompositionFilterPolicyMaster_ID': $scope.MasterObject.ID,
            'FilterName': null, 'ForRaw1_Packaging0': null, 'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionFilterPolicyDetails = [$scope.tbl_Pro_CompositionFilterPolicyDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionFilterPolicyDetail = {
                'ID': 0, 'FK_tbl_Pro_CompositionFilterPolicyMaster_ID': $scope.MasterObject.ID,
                'FilterName': null, 'ForRaw1_Packaging0': null, 'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionFilterPolicyDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_CompositionFilterPolicyDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };        

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    