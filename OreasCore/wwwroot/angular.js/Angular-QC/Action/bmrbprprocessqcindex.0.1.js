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
            '/QC/Action/BMRBPRMasterLoad', //--v_Load
            '/QC/Action/BMRBPRMasterGet', // getrow
            '/QC/Action/BMRBPRMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/QC/Action/GetInitializedBMRBPRProcess');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'BMRBPRMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'BMRBPRMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'BMRBPRMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'BMRBPRMasterCtlr').LoadByCard);
                if (data.find(o => o.Controller === 'BMRBPRMasterCtlr').Otherdata === null) {
                    $scope.ActionList = [];
                }
                else {
                    $scope.ActionList = data.find(o => o.Controller === 'BMRBPRMasterCtlr').Otherdata.ActionList;
                }
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'BMRSampleCtlr') != undefined) {
                $scope.$broadcast('init_BMRSampleCtlr', data.find(o => o.Controller === 'BMRSampleCtlr'));
            }
            if (data.find(o => o.Controller === 'BPRSampleCtlr') != undefined) {
                $scope.$broadcast('init_BPRSampleCtlr', data.find(o => o.Controller === 'BPRSampleCtlr'));
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
    .controller("BMRBPRSampleCtlr", function ($scope) {
        $scope.MasterObject = {};
        $scope.$on('BMRBPRSampleCtlr', function (e, itm) {
            $('[href="#BMRSample"]').tab('show');
            $scope.MasterObject = itm;
        });
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
            '/QC/Action/BMRSampleLoad', //--v_Load
            '/QC/Action/BMRSampleGet', // getrow
            '/QC/Action/BMRSamplePost' // PostRow
        );

        $scope.tbl_Qc_SampleProcessBMR = {
            'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR_ID': 0,
            'SampleDate': new Date(), 'SampleQty': 0, 
            'FK_tbl_Qc_ActionType_ID': 1, 'FK_tbl_Qc_ActionType_IDName': '',
            'ActionBy': null, 'ActionDate': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Qc_SampleProcessBMR };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Qc_SampleProcessBMR = data;
            //$scope.tbl_Qc_SampleProcessBMR.SampleDate = new Date(data.SampleDate);  
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
            '/QC/Action/BPRSampleLoad', //--v_Load
            '/QC/Action/BPRSampleGet', // getrow
            '/QC/Action/BPRSamplePost' // PostRow
        );

        $scope.tbl_Qc_SampleProcessBPR = {
            'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR_ID': 0,
            'SampleDate': new Date(), 'SampleQty': 0,
            'FK_tbl_Qc_ActionType_ID': 1, 'FK_tbl_Qc_ActionType_IDName': '',
            'ActionBy': null, 'ActionDate': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        $scope.postRowParam = function () {  return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Qc_SampleProcessBPR }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Qc_SampleProcessBPR = data;
            //$scope.tbl_Qc_SampleProcessBPR.SampleDate = new Date(data.SampleDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });
