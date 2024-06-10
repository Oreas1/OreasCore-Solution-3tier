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
            '/Production/Process/BMRBPRMasterLoad', //--v_Load
            '/Production/Process/BMRBPRMasterGet', // getrow
            '/Production/Process/BMRBPRMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Production/Process/GetInitializedBMRBPR');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'BMRBPRMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'BMRBPRMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'BMRBPRMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'BMRBPRMasterCtlr').LoadByCard);
                init_Report($scope, data.find(o => o.Controller === 'BMRBPRMasterCtlr').Reports, '/Production/Process/GetBMRReport'); 
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'BMRBPRProcessCtlr') != undefined) {
                $scope.$broadcast('init_BMRBPRProcessCtlr', data.find(o => o.Controller === 'BMRBPRProcessCtlr'));
            }

            if (data.find(o => o.Controller === 'BMRProcessCtlr') != undefined) {
                $scope.$broadcast('init_BMRProcessCtlr', data.find(o => o.Controller === 'BMRProcessCtlr'));
            }
            if (data.find(o => o.Controller === 'BMRSampleCtlr') != undefined) {
                $scope.$broadcast('init_BMRSampleCtlr', data.find(o => o.Controller === 'BMRSampleCtlr'));
            }
            if (data.find(o => o.Controller === 'BPRProcessCtlr') != undefined) {
                $scope.$broadcast('init_BPRProcessCtlr', data.find(o => o.Controller === 'BPRProcessCtlr'));
            }
            if (data.find(o => o.Controller === 'BPRSampleCtlr') != undefined) {
                $scope.$broadcast('init_BPRSampleCtlr', data.find(o => o.Controller === 'BPRSampleCtlr'));
            }
        };

        $scope.tbl_Pro_BatchMaterialRequisitionMaster = {
            'ID': 0, DocNo: null, 'DocDate': new Date(), 'BatchNo': null, 'BatchMfgDate': new Date(), 'BatchExpiryDate': new Date(),
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
                'ID': 0, DocNo: null, 'DocDate': new Date(), 'BatchNo': null, 'BatchMfgDate': new Date(), 'BatchExpiryDate': new Date(),
                'DimensionValue': 1, 'FK_tbl_Inv_MeasurementUnit_ID_Dimension': 0, 'FK_tbl_Inv_MeasurementUnit_ID_DimensionName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'BatchSizeUnit': '',
                'BatchSize': 1, 'FK_tbl_Pro_CompositionDetail_Coupling_ID': 0, 'TotalProd': 0, 'Cost': 0, 'IsCompleted': false, 'FinishedDate': null,
                'IsDispensedR': false, 'IsDispensedP': false, 'IsQAClearanceBMRPending': false, 'IsQAClearanceBPRPending': false, 'IsQCSampleBMRPending': false, 'IsQCSampleBPRPending': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_BatchMaterialRequisitionMaster };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_BatchMaterialRequisitionMaster = data;
            $scope.tbl_Pro_BatchMaterialRequisitionMaster.DocDate = new Date(data.DocDate);
            $scope.tbl_Pro_BatchMaterialRequisitionMaster.BatchMfgDate = new Date(data.BatchMfgDate);
            $scope.tbl_Pro_BatchMaterialRequisitionMaster.BatchExpiryDate = new Date(data.BatchExpiryDate);
        };

      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("BMRBPRProcessCtlr", function ($scope) {
        $scope.MasterObject = {};
        $scope.$on('BMRBPRProcessCtlr', function (e, itm) {
            $('[href="#BMRProcess"]').tab('show');
            $scope.MasterObject = itm;
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_BMRBPRProcessCtlr', function (e, itm) {         
            init_Report($scope, itm.Reports, '/Production/Process/GetBMRReport');
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
            $scope.BMRProcedureList = itm.Otherdata === null ? [] : itm.Otherdata.BMRProcedureList;
        });

        init_Operations($scope, $http,
            '/Production/Process/BMRProcessLoad', //--v_Load
            '/Production/Process/BMRProcessGet', // getrow
            '/Production/Process/BMRProcessPost' // PostRow
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
    .controller("BMRSampleCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('BMRSampleCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_BMRSampleCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/Production/Process/BMRSampleLoad', //--v_Load
            '/Production/Process/BMRSampleGet', // getrow
            '/Production/Process/BMRSamplePost' // PostRow
        );

        $scope.tbl_Qc_SampleProcessBMR = {
            'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR_ID': $scope.MasterObject.ID,
            'SampleDate': new Date(), 'SampleQty': 0, 
            'FK_tbl_Qc_ActionType_ID': 1, 'FK_tbl_Qc_ActionType_IDName': '',
            'ActionBy': null, 'ActionDate': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Qc_SampleProcessBMRs = [$scope.tbl_Qc_SampleProcessBMR];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Qc_SampleProcessBMR = {
                'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR_ID': $scope.MasterObject.ID,
                'SampleDate': new Date(), 'SampleQty': 0,
                'FK_tbl_Qc_ActionType_ID': 1, 'FK_tbl_Qc_ActionType_IDName': '',
                'ActionBy': null, 'ActionDate': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Qc_SampleProcessBMR };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Qc_SampleProcessBMR = data;
            $scope.tbl_Qc_SampleProcessBMR.SampleDate = new Date(data.SampleDate);  
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
            $scope.BPRProcedureList = itm.Otherdata === null ? [] : itm.Otherdata.BPRProcedureList;
        });


        init_Operations($scope, $http,
            '/Production/Process/BPRProcessLoad', //--v_Load
            '/Production/Process/BPRProcessGet', // getrow
            '/Production/Process/BPRProcessPost' // PostRow
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
    .controller("BPRSampleCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('BPRSampleCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_BPRSampleCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/Production/Process/BPRSampleLoad', //--v_Load
            '/Production/Process/BPRSampleGet', // getrow
            '/Production/Process/BPRSamplePost' // PostRow
        );

        $scope.tbl_Qc_SampleProcessBPR = {
            'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR_ID': $scope.MasterObject.ID,
            'SampleDate': new Date(), 'SampleQty': 0,
            'FK_tbl_Qc_ActionType_ID': 1, 'FK_tbl_Qc_ActionType_IDName': '',
            'ActionBy': null, 'ActionDate': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Qc_SampleProcessBPRs = [$scope.tbl_Qc_SampleProcessBPR];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Qc_SampleProcessBPR = {
                'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR_ID': $scope.MasterObject.ID,
                'SampleDate': new Date(), 'SampleQty': 0,
                'FK_tbl_Qc_ActionType_ID': 1, 'FK_tbl_Qc_ActionType_IDName': '',
                'ActionBy': null, 'ActionDate': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {  return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Qc_SampleProcessBPR }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Qc_SampleProcessBPR = data;
            $scope.tbl_Qc_SampleProcessBPR.SampleDate = new Date(data.SampleDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });
