MainModule
    .controller("StockTransferMasterCtlr", function ($scope, $http) {
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
            '/Inventory/Requisition/StockTransferMasterLoad', //--v_Load
            '/Inventory/Requisition/StockTransferMasterGet', // getrow
            '/Inventory/Requisition/StockTransferMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/Requisition/GetInitializedStockTransfer');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'StockTransferMasterCtlr') != undefined) {                
                $scope.Privilege = data.find(o => o.Controller === 'StockTransferMasterCtlr').Privilege;    
                init_Filter($scope, data.find(o => o.Controller === 'StockTransferMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'StockTransferMasterCtlr').LoadByCard);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'StockTransferDetailCtlr') != undefined) {
                $scope.$broadcast('init_StockTransferDetailCtlr', data.find(o => o.Controller === 'StockTransferDetailCtlr'));
            }
        };

        init_ProductSearchModalGeneral($scope, $http);
        init_ReferenceSearchModalGeneral($scope, $http);
        init_WHMSearchModalGeneral($scope, $http);

        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                if ($scope.WHMSearch_CallerName === 'tbl_Inv_StockTransferMaster.FK_tbl_Inv_WareHouseMaster_ID_FromName') {
                    $scope.tbl_Inv_StockTransferMaster.FK_tbl_Inv_WareHouseMaster_ID_From = item.ID;
                    $scope.tbl_Inv_StockTransferMaster.FK_tbl_Inv_WareHouseMaster_ID_FromName = item.WareHouseName;
                }
                else if ($scope.WHMSearch_CallerName === 'tbl_Inv_StockTransferMaster.FK_tbl_Inv_WareHouseMaster_ID_ToName') {
                    $scope.tbl_Inv_StockTransferMaster.FK_tbl_Inv_WareHouseMaster_ID_To = item.ID;
                    $scope.tbl_Inv_StockTransferMaster.FK_tbl_Inv_WareHouseMaster_ID_ToName = item.WareHouseName;
                }

            }
            else {
                if ($scope.WHMSearch_CallerName === 'tbl_Inv_StockTransferMaster.FK_tbl_Inv_WareHouseMaster_ID_FromName') {
                    $scope.tbl_Inv_StockTransferMaster.FK_tbl_Inv_WareHouseMaster_ID_From = null;
                    $scope.tbl_Inv_StockTransferMaster.FK_tbl_Inv_WareHouseMaster_ID_FromName = null;
                }
                else if ($scope.WHMSearch_CallerName === 'tbl_Inv_StockTransferMaster.FK_tbl_Inv_WareHouseMaster_ID_ToName') {
                    $scope.tbl_Inv_StockTransferMaster.FK_tbl_Inv_WareHouseMaster_ID_To = null;
                    $scope.tbl_Inv_StockTransferMaster.FK_tbl_Inv_WareHouseMaster_ID_ToName = null;
                }
            }
        };

        $scope.tbl_Inv_StockTransferMaster = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
            'FK_tbl_Inv_WareHouseMaster_ID_From': null, 'FK_tbl_Inv_WareHouseMaster_ID_FromName': '',
            'FK_tbl_Inv_WareHouseMaster_ID_To': null, 'FK_tbl_Inv_WareHouseMaster_ID_ToName': '',
            'IsReceivedAll': false, 'Remarks': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_StockTransferMasters = [$scope.tbl_Inv_StockTransferMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_StockTransferMaster = {
                'ID': 0, 'DocNo': '', 'DocDate': new Date(),
                'FK_tbl_Inv_WareHouseMaster_ID_From': null, 'FK_tbl_Inv_WareHouseMaster_ID_FromName': '',
                'FK_tbl_Inv_WareHouseMaster_ID_To': null, 'FK_tbl_Inv_WareHouseMaster_ID_ToName': '',
                'IsReceivedAll': false, 'Remarks': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_StockTransferMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Inv_StockTransferMaster = data;
            $scope.tbl_Inv_StockTransferMaster.DocDate = new Date(data.DocDate);   
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
        
    })
    .controller("StockTransferDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('StockTransferDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');         
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_StockTransferDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, itm.LoadByCard); 
            init_Report($scope, itm.Reports, '/Inventory/Requisition/GetStockTransferReport'); 
        });

       init_Operations($scope, $http,
            '/Inventory/Requisition/StockTransferDetailLoad', //--v_Load
            '/Inventory/Requisition/StockTransferDetailGet', // getrow
            '/Inventory/Requisition/StockTransferDetailPost' // PostRow
        );

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID) {
                $scope.tbl_Inv_StockTransferDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Inv_StockTransferDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Inv_StockTransferDetail.MeasurementUnit = item.MeasurementUnit;
            }
            else {
                $scope.tbl_Inv_StockTransferDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Inv_StockTransferDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Inv_StockTransferDetail.MeasurementUnit = null;
            }
            if (item.IsDecimal) { $scope.wholeNumberOrNot = ''; }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9][^\.]*$"); }

            $scope.tbl_Inv_StockTransferDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = null;
            $scope.tbl_Inv_StockTransferDetail.ReferenceNo = null;
        };

        $scope.ReferenceSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 || item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0) {
                $scope.tbl_Inv_StockTransferDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo;
                $scope.tbl_Inv_StockTransferDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo;
                $scope.tbl_Inv_StockTransferDetail.ReferenceNo = item.ReferenceNo;
                $scope.Balance = item.Balance;
            }
            else {
                $scope.tbl_Inv_StockTransferDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = null;
                $scope.tbl_Inv_StockTransferDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = null;
                $scope.tbl_Inv_StockTransferDetail.ReferenceNo = null;
                $scope.Balance = 0;
            }

            $scope.tbl_Inv_StockTransferDetail.Quantity = 0;
        };

        $scope.tbl_Inv_StockTransferDetail = {
            'ID': 0, 'FK_tbl_Inv_StockTransferMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null,
            'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null, 'ReferenceNo': '',
            'Quantity': 0, 'IsReceived': false, 'ReceivedBy': null, 'ReceivedDate': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_StockTransferDetails = [$scope.tbl_Inv_StockTransferDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_StockTransferDetail = {
                'ID': 0, 'FK_tbl_Inv_StockTransferMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null,
                'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null, 'ReferenceNo': '',
                'Quantity': 0, 'IsReceived': false, 'ReceivedBy': null, 'ReceivedDate': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_StockTransferDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_StockTransferDetail = data;
            if ($scope.tbl_Inv_StockTransferDetail.ReceivedDate != null
                ||
                $scope.tbl_Inv_StockTransferDetail.ReceivedDate != ''
                ||
                typeof $scope.tbl_Inv_StockTransferDetail.ReceivedDate != 'undefined'
            ) {
                $scope.tbl_Inv_StockTransferDetail.ReceivedDate = new Date(data.ReceivedDate);
            };
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };        

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    