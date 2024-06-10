MainModule
    .controller("BMRBPRMasterCtlr", function ($scope, $http) {
        $scope.DivHideShow = function (v, itm, div_hide, div_show, scope) {
            if (typeof v !== 'undefined' && v !== '' && v !== null) {
                $scope.$broadcast(v, itm);
            }
            if (typeof scope !== 'undefined' && scope !== '' && scope !== null && typeof scope.$parent.pageNavigation === 'function')
            {            
               scope.$parent.pageNavigation('Load');
            }
            if (div_hide !== null)
                $("#" + div_hide).hide('slow');
            if (div_show !== null)
                $("#" + div_show).show('slow');   
        };
        //////////////////////////////entry panel/////////////////////////
        init_Operations($scope, $http,
            '/QA/Process/BMRBPRMasterLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/QA/Process/GetInitializedBMRBPRProcess');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'BMRBPRMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'BMRBPRMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'BMRBPRMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'BMRBPRMasterCtlr').LoadByCard);

                $scope.pageNavigation('first');
            }

            if (data.find(o => o.Controller === 'BMRProcessCtlr') != undefined) {
                $scope.$broadcast('init_BMRProcessCtlr', data.find(o => o.Controller === 'BMRProcessCtlr'));
            }
            if (data.find(o => o.Controller === 'BPRProcessCtlr') != undefined) {
                $scope.$broadcast('init_BPRProcessCtlr', data.find(o => o.Controller === 'BPRProcessCtlr'));
            }
        };

        $scope.tbl_Pro_BatchMaterialRequisitionMaster = {
            'ID': 0, 'DocDate': new Date(), 'BatchNo': null, 'BatchMfgDate': new Date(), 'BatchExpiryDate': new Date(),
            'DimensionValue': 1, 'FK_tbl_Inv_MeasurementUnit_ID_Dimension': 0, 'FK_tbl_Inv_MeasurementUnit_ID_DimensionName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'BatchSizeUnit': '',
            'BatchSize': 1, 'FK_tbl_Pro_CompositionDetail_Coupling_ID': 0, 'TotalProd': 0, 'Cost': 0, 'IsCompleted': false, 'FinishedDate': null,
            'IsDispensedR': false, 'IsDispensedP': false, 'IsQAClearanceBMRPending': false, 'IsQAClearanceBPRPending': false, 'IsQCSampleBMRPending': false, 'IsQCSampleBPRPending': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BatchMaterialRequisitionMasters = [$scope.tbl_Pro_BatchMaterialRequisitionMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BatchMaterialRequisitionMaster = {
                'ID': 0, 'DocDate': new Date(), 'BatchNo': null, 'BatchMfgDate': new Date(), 'BatchExpiryDate': new Date(),
                'DimensionValue': 1, 'FK_tbl_Inv_MeasurementUnit_ID_Dimension': 0, 'FK_tbl_Inv_MeasurementUnit_ID_DimensionName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'BatchSizeUnit': '',
                'BatchSize': 1, 'FK_tbl_Pro_CompositionDetail_Coupling_ID': 0, 'TotalProd': 0, 'Cost': 0, 'IsCompleted': false, 'FinishedDate': null,
                'IsDispensedR': false, 'IsDispensedP': false, 'IsQAClearanceBMRPending': false, 'IsQAClearanceBPRPending': false, 'IsQCSampleBMRPending': false, 'IsQCSampleBPRPending': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("BMRBPRProcessCtlr", function ($scope) {
        $scope.MasterObject = {};
        $scope.$on('BMRBPRProcessCtlr', function (e, itm) {
            $('[href="#BMRProcess"]').tab('show');
            $scope.MasterObject = itm;
        });
    })
    .controller("BMRProcessCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('BMRProcessCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_BMRProcessCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/QA/Process/BMRProcessLoad', //--v_Load
            '/QA/Process/BMRProcessGet', // getrow
            '/QA/Process/BMRProcessPost' // PostRow
        );

        $scope.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR = {
            'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Pro_Procedure_ID': null, 'FK_tbl_Pro_Procedure_IDName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName': '', 'MeasurementUnit': '',
            'IsQAClearanceBeforeStart': true, 'QACleared': null, 'QAClearedBy': null, 'QAClearedDate': null, 'Yield': 0, 'IsCompleted': false, 'CompletedDate': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMRs = [$scope.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR = {
                'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Pro_Procedure_ID': null, 'FK_tbl_Pro_Procedure_IDName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName': '', 'MeasurementUnit': '',
                'IsQAClearanceBeforeStart': true, 'QACleared': null, 'QAClearedBy': null, 'QAClearedDate': null, 'Yield': 0, 'IsCompleted': false, 'CompletedDate': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR = data;
            $scope.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.QAClearedDate = new Date(data.QAClearedDate);
            $scope.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.CompletedDate = new Date(data.CompletedDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("BPRProcessCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('BPRProcessCtlr', function (e, itm) {

            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_BPRProcessCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/QA/Process/BPRProcessLoad', //--v_Load
            '/QA/Process/BPRProcessGet', // getrow
            '/QA/Process/BPRProcessPost' // PostRow
        );

        $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR = {
            'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Pro_Procedure_ID': null, 'FK_tbl_Pro_Procedure_IDName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName': '', 'MeasurementUnit': '',
            'IsQAClearanceBeforeStart': true, 'QACleared': null, 'QAClearedBy': null, 'QAClearedDate': null, 'Yield': 0, 'IsCompleted': false, 'CompletedDate': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPRs = [$scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR = {
                'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Pro_Procedure_ID': null, 'FK_tbl_Pro_Procedure_IDName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName': '', 'MeasurementUnit': '',
                'IsQAClearanceBeforeStart': true, 'QACleared': null, 'QAClearedBy': null, 'QAClearedDate': null, 'Yield': 0, 'IsCompleted': false, 'CompletedDate': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR = data;
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.QAClearedDate = new Date(data.QAClearedDate);
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.CompletedDate = new Date(data.CompletedDate);
        };

 
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });
