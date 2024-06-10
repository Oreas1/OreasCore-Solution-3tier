MainModule
    .controller("StockTransferReceivingMasterCtlr", function ($scope, $http) {
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
            '/Inventory/Dispensing/StockTransferMasterReceivingLoad', //--v_Load
            '/Inventory/Dispensing/StockTransferMasterReceivingGet', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/Dispensing/GetInitializedStockTransferReceiving');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'StockTransferReceivingMasterCtlr') != undefined) {                
                $scope.Privilege = data.find(o => o.Controller === 'StockTransferReceivingMasterCtlr').Privilege;    
                init_Filter($scope, data.find(o => o.Controller === 'StockTransferReceivingMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'StockTransferReceivingMasterCtlr').LoadByCard);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'StockTransferReceivingDetailCtlr') != undefined) {
                $scope.$broadcast('init_StockTransferReceivingDetailCtlr', data.find(o => o.Controller === 'StockTransferReceivingDetailCtlr'));
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
    .controller("StockTransferReceivingDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('StockTransferReceivingDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');         
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_StockTransferReceivingDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, itm.LoadByCard); 
            init_Report($scope, itm.Reports, '/Inventory/Dispensing/GetStockTransferReceivingReport'); 
        });

       init_Operations($scope, $http,
            '/Inventory/Dispensing/StockTransferDetailReceivingLoad', //--v_Load
            '/Inventory/Dispensing/StockTransferDetailReceivingGet', // getrow
            '/Inventory/Dispensing/StockTransferDetailReceivingPost' // PostRow
        );

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
            if ($scope.tbl_Inv_StockTransferDetail.QAClearedDate != null
                ||
                $scope.tbl_Inv_StockTransferDetail.QAClearedDate != ''
                ||
                typeof $scope.tbl_Inv_StockTransferDetail.QAClearedDate != 'undefined'
            ) {
                $scope.tbl_Inv_StockTransferDetail.QAClearedDate = new Date(data.QAClearedDate);
            };
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };        

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    