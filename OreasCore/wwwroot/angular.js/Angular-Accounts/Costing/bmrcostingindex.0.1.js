MainModule
    .controller("BMRCostingMasterCtlr", function ($scope, $http) {
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
            '/Accounts/Costing/BMRCostingMasterLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/Costing/GetInitializedBMRCosting');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'BMRCostingMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'BMRCostingMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'BMRCostingMasterCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }

            if (data.find(o => o.Controller === 'BMRCostingDetailPackagingMasterCtlr') != undefined) {
                $scope.$broadcast('init_BMRCostingDetailPackagingMasterCtlr', data.find(o => o.Controller === 'BMRCostingDetailPackagingMasterCtlr'));
            }
        };

        $scope.tbl_Pro_BatchMaterialRequisitionMaster = {
            'ID': 0, DocNo: null, 'DocDate': new Date(), 'BatchNo': null, 'BatchMfgDate': new Date(), 
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'BatchSizeUnit': '',
            'BatchSize': 1, 'FK_tbl_Pro_CompositionDetail_Coupling_ID': 0, 'TotalProd': 0, 'Cost': 0, 'FinishedDate': null, 'IsDispensedR': false, 'IsDispensedP': false,
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BatchMaterialRequisitionMasters = [$scope.tbl_Pro_BatchMaterialRequisitionMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BatchMaterialRequisitionMaster = {
                'ID': 0, DocNo: null, 'DocDate': new Date(), 'BatchNo': null, 'BatchMfgDate': new Date(),
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'BatchSizeUnit': '',
                'BatchSize': 1, 'FK_tbl_Pro_CompositionDetail_Coupling_ID': 0, 'TotalProd': 0, 'Cost': 0, 'FinishedDate': null, 'IsDispensedR': false, 'IsDispensedP': false,
            };
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("BMRCostingDetailPackagingMasterCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('BMRCostingDetailPackagingMasterCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_BMRCostingDetailPackagingMasterCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/Accounts/Costing/GetBMRCostingReport');  
        });

        init_Operations($scope, $http,
            '/Accounts/Costing/BMRCostingDetailPackagingMasterLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster = {
            'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionMaster_ID': $scope.MasterObject.ID, 'PackagingName': '', 'BatchSize': 1, 'BatchSizeUnit': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID_Primary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName': '', 'Cost_Primary': 0, 'TotalProd_Primary': 0,
            'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID': 0, 'FK_tbl_Inv_OrderNoteDetail_ID': null
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters = [$scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster = {
                'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionMaster_ID': $scope.MasterObject.ID, 'PackagingName': '', 'BatchSize': 1, 'BatchSizeUnit': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID_Primary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName': '', 'Cost_Primary': 0, 'TotalProd_Primary': 0,
                'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID': 0, 'FK_tbl_Inv_OrderNoteDetail_ID': null
            };
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });
