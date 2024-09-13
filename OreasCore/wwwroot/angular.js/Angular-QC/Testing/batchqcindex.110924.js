MainModule
    .controller("BatchMasterCtlr", function ($scope, $http) {
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
            '/QC/Testing/BatchMasterLoad', //--v_Load
            '/QC/Testing/BatchMasterGet', // getrow
            '/QC/Testing/BatchMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/QC/Testing/GetInitializedBatch');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'BatchMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'BatchMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'BatchMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'BatchMasterCtlr').LoadByCard);
                if (data.find(o => o.Controller === 'BatchMasterCtlr').Otherdata === null) {
                    $scope.ActionList = []; $scope.QcTestList = []; $scope.MeasurementUnitList = [];
                }
                else {
                    $scope.ActionList = data.find(o => o.Controller === 'BatchMasterCtlr').Otherdata.ActionList;
                    $scope.QcTestList = data.find(o => o.Controller === 'BatchMasterCtlr').Otherdata.QcTestList;
                    $scope.MeasurementUnitList = data.find(o => o.Controller === 'BatchMasterCtlr').Otherdata.MeasurementUnitList;
                }
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'BatchBMRSampleCtlr') != undefined) {
                $scope.$broadcast('init_BatchBMRSampleCtlr', data.find(o => o.Controller === 'BatchBMRSampleCtlr'));
            }
            if (data.find(o => o.Controller === 'BatchBPRSampleCtlr') != undefined) {
                $scope.$broadcast('init_BatchBPRSampleCtlr', data.find(o => o.Controller === 'BatchBPRSampleCtlr'));
            }
            if (data.find(o => o.Controller === 'BatchBMRSampleQcTestCtlr') != undefined) {
                $scope.$broadcast('init_BatchBMRSampleQcTestCtlr', data.find(o => o.Controller === 'BatchBMRSampleQcTestCtlr'));
            }
            if (data.find(o => o.Controller === 'BatchBPRSampleQcTestCtlr') != undefined) {
                $scope.$broadcast('init_BatchBPRSampleQcTestCtlr', data.find(o => o.Controller === 'BatchBPRSampleQcTestCtlr'));
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
    .controller("BatchBMRBPRSampleCtlr", function ($scope) {
        $scope.MasterObject = {};
        $scope.$on('BatchBMRBPRSampleCtlr', function (e, itm) {
            $('[href="#BMRSample"]').tab('show');
            $scope.MasterObject = itm;
        });
    })
    .controller("BatchBMRSampleCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('BatchBMRSampleCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_BatchBMRSampleCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/QC/Testing/BatchBMRSampleLoad', //--v_Load
            '/QC/Testing/BatchBMRSampleGet', // getrow
            '/QC/Testing/BatchBMRSamplePost' // PostRow
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
    .controller("BatchBPRSampleCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('BatchBPRSampleCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_BatchBPRSampleCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/QC/Testing/BatchBPRSampleLoad', //--v_Load
            '/QC/Testing/BatchBPRSampleGet', // getrow
            '/QC/Testing/BatchBPRSamplePost' // PostRow
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
    .controller("BatchBMRSampleQcTestCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('BatchBMRSampleQcTestCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_BatchBMRSampleQcTestCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/QC/Testing/BatchBMRSampleQcTestLoad', //--v_Load
            '/QC/Testing/BatchBMRSampleQcTestGet', // getrow
            '/QC/Testing/BatchBMRSampleQcTestPost' // PostRow
        );

        $scope.tbl_Qc_SampleProcessBMR_QcTest = {
            'ID': 0, 'FK_tbl_Qc_SampleProcessBMR_ID': $scope.MasterObject.ID,
            'FK_tbl_Qc_Test_ID': null, 'FK_tbl_Qc_Test_IDName': '', 'TestDescription': null, 'Specification': null,
            'RangeFrom': null, 'RangeTill': null, 'FK_tbl_Inv_MeasurementUnit_ID': null, 'FK_tbl_Inv_MeasurementUnit_IDName': null,
            'ResultValue': 0, 'ResultRemarks': null, 'IsPrintOnCOA': true,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Qc_SampleProcessBMR_QcTests = [$scope.tbl_Qc_SampleProcessBMR_QcTest];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Qc_SampleProcessBMR_QcTest = {
                'ID': 0, 'FK_tbl_Qc_SampleProcessBMR_ID': $scope.MasterObject.ID,
                'FK_tbl_Qc_Test_ID': null, 'FK_tbl_Qc_Test_IDName': '', 'TestDescription': null, 'Specification': null,
                'RangeFrom': null, 'RangeTill': null, 'FK_tbl_Inv_MeasurementUnit_ID': null, 'FK_tbl_Inv_MeasurementUnit_IDName': null,
                'ResultValue': 0, 'ResultRemarks': null, 'IsPrintOnCOA': true,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Qc_SampleProcessBMR_QcTest }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Qc_SampleProcessBMR_QcTest = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        $scope.Replication = function (id) {
            var successcallback = function (response) {
                if (response.data === 'OK') {
                    $scope.pageNavigation('first');
                }
                else {
                    alert(response.data);
                }
            };

            var errorcallback = function (error) {
                alert(error.data);
                console.log(error);
            };

            if (confirm("Are you sure! you want to Fetch Standard Test List") === true) {
                $http({
                    method: "POST", url: '/QC/Testing/BatchBMRSampleQcTestPostReplication', async: true, params: { MasterID: id, operation: 'Save New' }, data: null, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
                }).then(successcallback, errorcallback);
            }

        };
    })
    .controller("BatchBPRSampleQcTestCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('BatchBPRSampleQcTestCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_BatchBPRSampleQcTestCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/QC/Testing/BatchBPRSampleQcTestLoad', //--v_Load
            '/QC/Testing/BatchBPRSampleQcTestGet', // getrow
            '/QC/Testing/BatchBPRSampleQcTestPost' // PostRow
        );

        $scope.tbl_Qc_SampleProcessBPR_QcTest = {
            'ID': 0, 'FK_tbl_Qc_SampleProcessBPR_ID': $scope.MasterObject.ID,
            'FK_tbl_Qc_Test_ID': null, 'FK_tbl_Qc_Test_IDName': '', 'TestDescription': null, 'Specification': null,
            'RangeFrom': null, 'RangeTill': null, 'FK_tbl_Inv_MeasurementUnit_ID': null, 'FK_tbl_Inv_MeasurementUnit_IDName': null,
            'ResultValue': 0, 'ResultRemarks': null, 'IsPrintOnCOA': true,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Qc_SampleProcessBPR_QcTests = [$scope.tbl_Qc_SampleProcessBPR_QcTest];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Qc_SampleProcessBPR_QcTest = {
                'ID': 0, 'FK_tbl_Qc_SampleProcessBPR_ID': $scope.MasterObject.ID,
                'FK_tbl_Qc_Test_ID': null, 'FK_tbl_Qc_Test_IDName': '', 'TestDescription': null, 'Specification': null,
                'RangeFrom': null, 'RangeTill': null, 'FK_tbl_Inv_MeasurementUnit_ID': null, 'FK_tbl_Inv_MeasurementUnit_IDName': null,
                'ResultValue': 0, 'ResultRemarks': null, 'IsPrintOnCOA': true,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Qc_SampleProcessBPR_QcTest }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Qc_SampleProcessBPR_QcTest = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        $scope.Replication = function (id) {
            var successcallback = function (response) {
                if (response.data === 'OK') {
                    $scope.pageNavigation('first');
                }
                else {
                    alert(response.data);
                }
            };

            var errorcallback = function (error) {
                alert(error.data);
                console.log(error);
            };

            if (confirm("Are you sure! you want to Fetch Standard Test List") === true) {
                $http({
                    method: "POST", url: '/QC/Testing/BatchBPRSampleQcTestPostReplication', async: true, params: { MasterID: id, operation: 'Save New' }, data: null, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
                }).then(successcallback, errorcallback);
            }

        };
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });
