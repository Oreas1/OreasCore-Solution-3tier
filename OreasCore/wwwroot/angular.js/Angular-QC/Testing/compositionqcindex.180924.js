MainModule
    .controller("CompositionMasterCtlr", function ($scope, $http) {
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
        //$scope.$on('CompositionMasterCtlr', function (e, itm) {
        //    console.log('c');
        //    $scope.pageNavigation('Load');
        //});
        //////////////////////////////entry panel/////////////////////////
        init_Operations($scope, $http,
            '/QC/Testing/CompositionMasterLoad', //--v_Load
            '/QC/Testing/CompositionMasterGet', // getrow
            '/QC/Testing/CompositionMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/QC/Testing/GetInitializedComposition');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'CompositionMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'CompositionMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'CompositionMasterCtlr').WildCard, null, null, null);
                if (data.find(o => o.Controller === 'CompositionMasterCtlr').Otherdata === null) {
                    $scope.MeasurementUnitList = []; $scope.QcTestList = [];
                }
                else {
                    $scope.MeasurementUnitList = data.find(o => o.Controller === 'CompositionMasterCtlr').Otherdata.MeasurementUnitList;
                    $scope.QcTestList = data.find(o => o.Controller === 'CompositionMasterCtlr').Otherdata.QcTestList;
                }
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'CompositionPackagingMasterCtlr') != undefined) {
                $scope.$broadcast('init_CompositionPackagingMasterCtlr', data.find(o => o.Controller === 'CompositionPackagingMasterCtlr'));
            }
            if (data.find(o => o.Controller === 'CompositionBMRProcessCtlr') != undefined) {
                $scope.$broadcast('init_CompositionBMRProcessCtlr', data.find(o => o.Controller === 'CompositionBMRProcessCtlr'));
            }
            if (data.find(o => o.Controller === 'CompositionBPRProcessCtlr') != undefined) {
                $scope.$broadcast('init_CompositionBPRProcessCtlr', data.find(o => o.Controller === 'CompositionBPRProcessCtlr'));
            }
            if (data.find(o => o.Controller === 'CompositionBMRProcessQcTestCtlr') != undefined) {
                $scope.$broadcast('init_CompositionBMRProcessQcTestCtlr', data.find(o => o.Controller === 'CompositionBMRProcessQcTestCtlr'));
            }
            if (data.find(o => o.Controller === 'CompositionBPRProcessQcTestCtlr') != undefined) {
                $scope.$broadcast('init_CompositionBPRProcessQcTestCtlr', data.find(o => o.Controller === 'CompositionBPRProcessQcTestCtlr'));
            }

        };

        init_ProductSearchModalGeneral($scope, $http);

        $scope.tbl_Pro_CompositionMaster = {
            'ID': 0, 'DocNo': null, 'DocDate': new Date(), 'CompositionName': '', 'ShelfLifeInMonths': 1,
            'DimensionValue': 1, 'FK_tbl_Inv_MeasurementUnit_ID_Dimension': null, 'FK_tbl_Inv_MeasurementUnit_ID_DimensionName': '',
            'RevisionNo': null, 'RevisionDate': new Date(), 'ControlProcedureNo': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionMasters = [$scope.tbl_Pro_CompositionMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionMaster = {
                'ID': 0, 'DocNo': null, 'DocDate': new Date(), 'CompositionName': '', 'ShelfLifeInMonths': 1,
                'DimensionValue': 1, 'FK_tbl_Inv_MeasurementUnit_ID_Dimension': null, 'FK_tbl_Inv_MeasurementUnit_ID_DimensionName': '',
                'RevisionNo': null, 'RevisionDate': new Date(), 'ControlProcedureNo': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Pro_CompositionMaster = data; $scope.tbl_Pro_CompositionMaster.DocDate = new Date(data.DocDate); $scope.tbl_Pro_CompositionMaster.RevisionDate = new Date(data.RevisionDate);
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("CompositionBMRProcessCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('CompositionBMRProcessCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_CompositionBMRProcessCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            $scope.BMRProcedureList = itm.Otherdata === null ? [] : itm.Otherdata.BMRProcedureList;
        });

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_CompositionMaster_ProcessBMR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample = item.ID;
                $scope.tbl_Pro_CompositionMaster_ProcessBMR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = item.ProductName + ' ' + item.MeasurementUnit;
            }
            else {
                $scope.tbl_Pro_CompositionMaster_ProcessBMR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample = null;
                $scope.tbl_Pro_CompositionMaster_ProcessBMR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = null;
            }
        };

        init_Operations($scope, $http,
            '/QC/Testing/CompositionBMRProcessLoad', //--v_Load
            '/QC/Testing/CompositionBMRProcessGet', // getrow
            '/QC/Testing/CompositionBMRProcessPost' // PostRow
        );

        $scope.tbl_Pro_CompositionMaster_ProcessBMR = {
            'ID': 0, 'FK_tbl_Pro_CompositionMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Pro_Procedure_ID': null, 'FK_tbl_Pro_Procedure_IDName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName': '', 'MeasurementUnit': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionMaster_ProcessBMRs = [$scope.tbl_Pro_CompositionMaster_ProcessBMR];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionMaster_ProcessBMR = {
                'ID': 0, 'FK_tbl_Pro_CompositionMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Pro_Procedure_ID': null, 'FK_tbl_Pro_Procedure_IDName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName': '', 'MeasurementUnit': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionMaster_ProcessBMR }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_CompositionMaster_ProcessBMR = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };



    })
    .controller("CompositionBMRProcessQcTestCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('CompositionBMRProcessQcTestCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_CompositionBMRProcessQcTestCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/QC/Testing/CompositionBMRProcessQcTestLoad', //--v_Load
            '/QC/Testing/CompositionBMRProcessQcTestGet', // getrow
            '/QC/Testing/CompositionBMRProcessQcTestPost' // PostRow
        );

        $scope.tbl_Pro_CompositionMaster_ProcessBMR_QcTest = {
            'ID': 0, 'FK_tbl_Pro_CompositionMaster_ProcessBMR_ID': $scope.MasterObject.ID,
            'FK_tbl_Qc_Test_ID': null, 'FK_tbl_Qc_Test_IDName': '', 'TestDescription': null, 'Specification': null,
            'RangeFrom': null, 'RangeTill': null, 'FK_tbl_Inv_MeasurementUnit_ID': null, 'FK_tbl_Inv_MeasurementUnit_IDName': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionMaster_ProcessBMR_QcTests = [$scope.tbl_Pro_CompositionMaster_ProcessBMR_QcTest];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionMaster_ProcessBMR_QcTest = {
                'ID': 0, 'FK_tbl_Pro_CompositionMaster_ProcessBMR_ID': $scope.MasterObject.ID,
                'FK_tbl_Qc_Test_ID': null, 'FK_tbl_Qc_Test_IDName': '', 'TestDescription': null, 'Specification': null,
                'RangeFrom': null, 'RangeTill': null, 'FK_tbl_Inv_MeasurementUnit_ID': null, 'FK_tbl_Inv_MeasurementUnit_IDName': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionMaster_ProcessBMR_QcTest }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_CompositionMaster_ProcessBMR_QcTest = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("CompositionPackagingMasterCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('CompositionPackagingMasterCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_CompositionPackagingMasterCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);

        });

        init_Operations($scope, $http,
            '/QC/Testing/CompositionPackagingMasterLoad', //--v_Load
            '/QC/Testing/CompositionPackagingMasterGet', // getrow
            '/QC/Testing/CompositionPackagingMasterPost' // PostRow
        );

        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster = {
            'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID_Primary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName': '',
            'PackagingName': '', 'IsDiscontinue': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMasters = [$scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster = {
                'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID_Primary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName': '',
                'PackagingName': '', 'IsDiscontinue': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("CompositionBPRProcessCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('CompositionBPRProcessCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_CompositionBPRProcessCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            $scope.BPRProcedureList = itm.Otherdata === null ? [] : itm.Otherdata.BPRProcedureList;
        });

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample = item.ID;
                $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = item.ProductName + ' ' + item.MeasurementUnit;
            }
            else {
                $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample = null;
                $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = null;
            }
        };

        init_Operations($scope, $http,
            '/QC/Testing/CompositionBPRProcessLoad', //--v_Load
            '/QC/Testing/CompositionBPRProcessGet', // getrow
            '/QC/Testing/CompositionBPRProcessPost' // PostRow
        );

        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR = {
            'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Pro_Procedure_ID': null, 'FK_tbl_Pro_Procedure_IDName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName': '', 'MeasurementUnit': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPRs = [$scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR = {
                'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Pro_Procedure_ID': null, 'FK_tbl_Pro_Procedure_IDName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName': '', 'MeasurementUnit': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })    
    .controller("CompositionBPRProcessQcTestCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('CompositionBPRProcessQcTestCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_CompositionBPRProcessQcTestCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/QC/Testing/CompositionBPRProcessQcTestLoad', //--v_Load
            '/QC/Testing/CompositionBPRProcessQcTestGet', // getrow
            '/QC/Testing/CompositionBPRProcessQcTestPost' // PostRow
        );

        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest = {
            'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_ID': $scope.MasterObject.ID,
            'FK_tbl_Qc_Test_ID': null, 'FK_tbl_Qc_Test_IDName': '', 'TestDescription': null, 'Specification': null,
            'RangeFrom': null, 'RangeTill': null, 'FK_tbl_Inv_MeasurementUnit_ID': null, 'FK_tbl_Inv_MeasurementUnit_IDName': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTests = [$scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest = {
                'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_ID': $scope.MasterObject.ID,
                'FK_tbl_Qc_Test_ID': null, 'FK_tbl_Qc_Test_IDName': '', 'TestDescription': null, 'Specification': null,
                'RangeFrom': null, 'RangeTill': null, 'FK_tbl_Inv_MeasurementUnit_ID': null, 'FK_tbl_Inv_MeasurementUnit_IDName': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });
