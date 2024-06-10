MainModule
    .controller("BMRMasterCtlr", function ($scope, $http) {
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
            '/QA/BMR/BMRMasterLoad', //--v_Load
            '/QA/BMR/BMRMasterGet', // getrow
            '/QA/BMR/BMRMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/QA/BMR/GetInitializedBMR');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'BMRMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'BMRMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'BMRMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'BMRMasterCtlr').LoadByCard);
                const urlParams = new URLSearchParams(window.location.search);

                if (urlParams.get('byBatchNo') != null) {
                    $scope.FilterByText = 'byBatchNo';
                    $scope.FilterValueByText = urlParams.get('byBatchNo');
                }

                $scope.pageNavigation('first');
            }

            if (data.find(o => o.Controller === 'BMRDetailRawMasterCtlr') != undefined) {
                $scope.$broadcast('init_BMRDetailRawMasterCtlr', data.find(o => o.Controller === 'BMRDetailRawMasterCtlr'));
            }
            if (data.find(o => o.Controller === 'BMRDetailRawDetailItemCtlr') != undefined) {
                $scope.$broadcast('init_BMRDetailRawDetailItemCtlr', data.find(o => o.Controller === 'BMRDetailRawDetailItemCtlr'));
            }
            if (data.find(o => o.Controller === 'BMRProcessCtlr') != undefined) {
                $scope.$broadcast('init_BMRProcessCtlr', data.find(o => o.Controller === 'BMRProcessCtlr'));
            }
            if (data.find(o => o.Controller === 'BMRDetailPackagingMasterCtlr') != undefined) {
                $scope.$broadcast('init_BMRDetailPackagingMasterCtlr', data.find(o => o.Controller === 'BMRDetailPackagingMasterCtlr'));
            }
            if (data.find(o => o.Controller === 'BMRDetailPackagingDetailFilterCtlr') != undefined) {
                $scope.$broadcast('init_BMRDetailPackagingDetailFilterCtlr', data.find(o => o.Controller === 'BMRDetailPackagingDetailFilterCtlr'));
            }
            if (data.find(o => o.Controller === 'BMRDetailPackagingDetailFilterDetailItemCtlr') != undefined) {
                $scope.$broadcast('init_BMRDetailPackagingDetailFilterDetailItemCtlr', data.find(o => o.Controller === 'BMRDetailPackagingDetailFilterDetailItemCtlr'));
            }
            if (data.find(o => o.Controller === 'BMRDetailPackagingDetailProductionCtlr') != undefined) {
                $scope.$broadcast('init_BMRDetailPackagingDetailProductionCtlr', data.find(o => o.Controller === 'BMRDetailPackagingDetailProductionCtlr'));
            }
            if (data.find(o => o.Controller === 'BPRProcessCtlr') != undefined) {
                $scope.$broadcast('init_BPRProcessCtlr', data.find(o => o.Controller === 'BPRProcessCtlr'));
            }
        };

        init_ProductSearchModalGeneral($scope, $http);
        init_WHMSearchModalGeneral($scope, $http);

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_BatchMaterialRequisitionMaster.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Pro_BatchMaterialRequisitionMaster.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Pro_BatchMaterialRequisitionMaster.BatchSizeUnit = item.MeasurementUnit;
                $scope.tbl_Pro_BatchMaterialRequisitionMaster.FK_tbl_Pro_CompositionDetail_Coupling_ID = item.MasterProdID;
            }
            else {

                $scope.tbl_Pro_BatchMaterialRequisitionMaster.FK_tbl_Inv_ProductRegistrationDetail_ID = 0;
                $scope.tbl_Pro_BatchMaterialRequisitionMaster.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Pro_BatchMaterialRequisitionMaster.BatchSizeUnit = null;
                $scope.tbl_Pro_BatchMaterialRequisitionMaster.FK_tbl_Pro_CompositionDetail_Coupling_ID = 0;
            }

            if (item.IsDecimal) { $scope.wholeNumberOrNot = ''; }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9][^\.]*$"); }

        };

        $scope.tbl_Pro_BatchMaterialRequisitionMaster = {
            'ID': 0, DocNo: null, 'DocDate': new Date(), 'BatchNo': null, 'BatchMfgDate': new Date(), 'BatchExpiryDate': new Date(),
            'DimensionValue': 1, 'FK_tbl_Inv_MeasurementUnit_ID_Dimension': 0, 'FK_tbl_Inv_MeasurementUnit_ID_DimensionName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'BatchSizeUnit': '',
            'BatchSize': 1, 'FK_tbl_Pro_CompositionDetail_Coupling_ID': 0, 'TotalProd': 0, 'Cost': 0, 'FinishedDate': null,
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
                'BatchSize': 1, 'FK_tbl_Pro_CompositionDetail_Coupling_ID': 0, 'TotalProd': 0, 'Cost': 0, 'FinishedDate': null,
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

        $scope.AutoIssuanceRequest = function (BMR_RawItemID, BMR_PackagingItemID, BMR_AdditionalItemID, OR_ItemID, scope) {

            var successcallback = function (response) {
                alert(response.data);
                $scope.callerscope = scope;
                $scope.callerscope.pageNavigation('Load');
            };
            var errorcallback = function (error) {
                console.log('Post error', error);
            };
            $http({
                method: "POST", url: '/QA/BMR/BMRStockIssuanceReservationItemPost', async: false, params: { BMR_RawItemID: BMR_RawItemID, BMR_PackagingItemID: BMR_PackagingItemID, BMR_AdditionalItemID: BMR_AdditionalItemID, OR_ItemID: OR_ItemID, operation: 'Save New' }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'NOSpinner': true, 'RequestVerificationToken': $scope.antiForgeryToken }
            }).then(successcallback, errorcallback);


        };

       
    })
    .controller("BMRDetailRawMasterCtlr", function ($scope, $http) {        
        $scope.MasterObject = {};
        $scope.$on('BMRDetailRawMasterCtlr', function (e, itm) {
            $('[href="#BMRFormulaType"]').tab('show');
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');              
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_BMRDetailRawMasterCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
            init_Report($scope, itm.Reports, '/QA/BMR/GetBMRReport'); 
            $scope.CompositionFilterList = itm.Otherdata === null ? [] : itm.Otherdata.CompositionFilterList;
        });

       init_Operations($scope, $http,
            '/QA/BMR/BMRDetailRawMasterLoad', //--v_Load
            '/QA/BMR/BMRDetailRawMasterGet', // getrow
            '/QA/BMR/BMRDetailRawMasterPost' // PostRow
        );

        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.FK_tbl_Inv_WareHouseMaster_ID = item.ID;
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.FK_tbl_Inv_WareHouseMaster_IDName = item.WareHouseName;
            }
            else {
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.FK_tbl_Inv_WareHouseMaster_ID = null;
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.FK_tbl_Inv_WareHouseMaster_IDName = null;
            }
        };

        $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster = {
            'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Pro_CompositionFilterPolicyDetail_ID': null, 'FK_tbl_Pro_CompositionFilterPolicyDetail_IDName': '',
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'FK_tbl_Pro_CompositionDetail_RawMaster_ID': null
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawMasters = [$scope.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster = {
                'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Pro_CompositionFilterPolicyDetail_ID': null, 'FK_tbl_Pro_CompositionFilterPolicyDetail_IDName': '',
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'FK_tbl_Pro_CompositionDetail_RawMaster_ID': null
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };    



    })
    .controller("BMRDetailRawDetailItemCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('BMRDetailRawDetailItemCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');

        });

        $scope.$on('init_BMRDetailRawDetailItemCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/QA/BMR/BMRDetailRawDetailItemLoad', //--v_Load
            '/QA/BMR/BMRDetailRawDetailItemGet', // getrow
            '/QA/BMR/BMRDetailRawDetailItemPost' // PostRow
        );

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.MeasurementUnit = item.MeasurementUnit;
            }
            else {

                $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.MeasurementUnit = null;
            }

            if (item.IsDecimal) { $scope.wholeNumberOrNot = ''; }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9][^\.]*$"); }

        };

        $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawDetail = {
            'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_RawMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawDetails = [$scope.tbl_Pro_BatchMaterialRequisitionDetail_RawDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawDetail = {
                'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_RawMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'Quantity': 0, 'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };


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

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample = item.ID;
                $scope.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = item.ProductName + ' ' + item.MeasurementUnit;
            }
            else {
                $scope.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample = null;
                $scope.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = null;
            }
        };

        init_Operations($scope, $http,
            '/QA/BMR/BMRProcessLoad', //--v_Load
            '/QA/BMR/BMRProcessGet', // getrow
            '/QA/BMR/BMRProcessPost' // PostRow
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
    .controller("BMRDetailPackagingMasterCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('BMRDetailPackagingMasterCtlr', function (e, itm) {            
            $scope.MasterObject = itm;               
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_BMRDetailPackagingMasterCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/QA/BMR/GetBMRReport'); 
        });

        init_Operations($scope, $http,
            '/QA/BMR/BMRDetailPackagingMasterLoad', //--v_Load
            '/QA/BMR/BMRDetailPackagingMasterGet', // getrow
            '/QA/BMR/BMRDetailPackagingMasterPost' // PostRow
        );

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            
            if (item.ID > 0) {
                if ($scope.ProductSearch_CallerName === 'tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName') {
                    $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary = item.ID;
                    $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = item.MeasurementUnit + ' ' + item.Description;
                    $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID = item.MasterProdID;
                    $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.PackagingName = item.OtherDetail;
                    //------when primary changed then de-select secondary
                    $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary = null;
                    $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName = null;
                }
                else if ($scope.ProductSearch_CallerName === 'tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName') {
                    $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary = item.ID;
                    $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName = item.MeasurementUnit + ' ' + item.Description;
                }
            }
            else {
                if ($scope.ProductSearch_CallerName === 'tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName') {
                    $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary = null;
                    $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = null;
                    $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID = 0;
                    $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.PackagingName = null;
                    //------when primary changed then de-select secondary
                    $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary = null;
                    $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName = null;
                }
                else if ($scope.ProductSearch_CallerName === 'tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName') {
                    $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary = null;
                    $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName = null;
                }
            }
        };


        $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster = {
            'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionMaster_ID': $scope.MasterObject.ID, 'PackagingName': '', 'BatchSize': 1, 'BatchSizeUnit': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID_Primary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName': '', 'GTINCode': '', 'Cost_Primary': 0, 'TotalProd_Primary': 0,
            'FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName': '', 'Cost_Secondary': 0, 'TotalProd_Secondary': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID': 0, 'FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID': null
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters = [$scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster = {
                'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionMaster_ID': $scope.MasterObject.ID, 'PackagingName': '', 'BatchSize': 1, 'BatchSizeUnit': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID_Primary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName': '', 'GTINCode': '', 'Cost_Primary': 0, 'TotalProd_Primary': 0,
                'FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName': '', 'Cost_Secondary': 0, 'TotalProd_Secondary': 0,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID': 0, 'FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID': null
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("BMRDetailPackagingDetailFilterCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('BMRDetailPackagingDetailFilterCtlr', function (e, itm) {
            $('[href="#BPRFormulaType"]').tab('show');
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');

        });

        $scope.$on('init_BMRDetailPackagingDetailFilterCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            $scope.CompositionFilterList = itm.Otherdata === null ? [] : itm.Otherdata.CompositionFilterList;
        });

        init_Operations($scope, $http,
            '/QA/BMR/BMRDetailPackagingDetailFilterLoad', //--v_Load
            '/QA/BMR/BMRDetailPackagingDetailFilterGet', // getrow
            '/QA/BMR/BMRDetailPackagingDetailFilterPost' // PostRow
        );

        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.FK_tbl_Inv_WareHouseMaster_ID = item.ID;
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.FK_tbl_Inv_WareHouseMaster_IDName = item.WareHouseName;
            }
            else {
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.FK_tbl_Inv_WareHouseMaster_ID = null;
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.FK_tbl_Inv_WareHouseMaster_IDName = null;
            }
        };

        $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail = {
            'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Pro_CompositionFilterPolicyDetail_ID': null, 'FK_tbl_Pro_CompositionFilterPolicyDetail_IDName': '',
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
            'FK_tbl_Pro_CompositionDetail_Coupling_PackagingDetail_ID': null
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetails = [$scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail = {
                'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Pro_CompositionFilterPolicyDetail_ID': null, 'FK_tbl_Pro_CompositionFilterPolicyDetail_IDName': '',
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
                'FK_tbl_Pro_CompositionDetail_Coupling_PackagingDetail_ID': null
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("BMRDetailPackagingDetailFilterDetailItemCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('BMRDetailPackagingDetailFilterDetailItemCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');

        });

        $scope.$on('init_BMRDetailPackagingDetailFilterDetailItemCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/QA/BMR/BMRDetailPackagingDetailFilterDetailItemLoad', //--v_Load
            '/QA/BMR/BMRDetailPackagingDetailFilterDetailItemGet', // getrow
            '/QA/BMR/BMRDetailPackagingDetailFilterDetailItemPost' // PostRow
        );

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.MeasurementUnit = item.MeasurementUnit;
            }
            else {

                $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.MeasurementUnit = null;
            }

            if (item.IsDecimal) { $scope.wholeNumberOrNot = ''; }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9][^\.]*$"); }

        };

        $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items = {
            'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss = [$scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items = {
                'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'Quantity': 0, 'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items = data;
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

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample = item.ID;
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = item.ProductName + ' ' + item.MeasurementUnit;
            }
            else {
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample = null;
                $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = null;
            }
        };

        init_Operations($scope, $http,
            '/QA/BMR/BPRProcessLoad', //--v_Load
            '/QA/BMR/BPRProcessGet', // getrow
            '/QA/BMR/BPRProcessPost' // PostRow
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
