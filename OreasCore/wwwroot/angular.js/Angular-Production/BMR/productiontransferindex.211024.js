MainModule
    .controller("ProductionTransferMasterCtlr", function ($scope, $http) {
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
            '/Production/BMR/ProductionTransferMasterLoad', //--v_Load
            '/Production/BMR/ProductionTransferMasterGet', // getrow
            '/Production/BMR/ProductionTransferMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Production/BMR/GetInitializedProductionTransfer');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'ProductionTransferMasterCtlr') != undefined) {                
                $scope.Privilege = data.find(o => o.Controller === 'ProductionTransferMasterCtlr').Privilege;    
                init_Filter($scope, data.find(o => o.Controller === 'ProductionTransferMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'ProductionTransferMasterCtlr').LoadByCard);
                if (data.find(o => o.Controller === 'ProductionTransferMasterCtlr').Otherdata === null) {
                    $scope.WareHouseList = [];
                }
                else {
                    $scope.WareHouseList = data.find(o => o.Controller === 'ProductionTransferMasterCtlr').Otherdata.WareHouseList;
                }
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'ProductionTransferDetailCtlr') != undefined) {
                $scope.$broadcast('init_ProductionTransferDetailCtlr', data.find(o => o.Controller === 'ProductionTransferDetailCtlr'));
            }
        };

        init_ProductSearchModalGeneral($scope, $http);
        init_ReferenceSearchModalGeneral($scope, $http);
        init_WHMSearchModalGeneral($scope, $http);

        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_ProductionTransferMaster.FK_tbl_Inv_WareHouseMaster_ID = item.ID;
                $scope.tbl_Pro_ProductionTransferMaster.FK_tbl_Inv_WareHouseMaster_IDName = item.WareHouseName;
            }
            else {
                $scope.tbl_Pro_ProductionTransferMaster.FK_tbl_Inv_WareHouseMaster_ID = null;
                $scope.tbl_Pro_ProductionTransferMaster.FK_tbl_Inv_WareHouseMaster_IDName = null;
            }
        };

        $scope.tbl_Pro_ProductionTransferMaster = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '', 'Remarks': '',
            'QAClearedAll': false, 'ReceivedAll': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_ProductionTransferMasters = [$scope.tbl_Pro_ProductionTransferMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_ProductionTransferMaster = {
                'ID': 0, 'DocNo': '', 'DocDate': new Date(),
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '', 'Remarks': '',
                'QAClearedAll': false, 'ReceivedAll': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_ProductionTransferMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Pro_ProductionTransferMaster = data; $scope.tbl_Pro_ProductionTransferMaster.DocDate = new Date(data.DocDate);   
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
        
    })
    .controller("ProductionTransferDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('ProductionTransferDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');         
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_ProductionTransferDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, itm.LoadByCard); 
            init_Report($scope, itm.Reports, '/Production/BMR/GetProductionTransferReport'); 
        });

       init_Operations($scope, $http,
            '/Production/BMR/ProductionTransferDetailLoad', //--v_Load
            '/Production/BMR/ProductionTransferDetailGet', // getrow
            '/Production/BMR/ProductionTransferDetailPost' // PostRow
        );

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID) {
                $scope.tbl_Pro_ProductionTransferDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Pro_ProductionTransferDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Pro_ProductionTransferDetail.MeasurementUnit = item.MeasurementUnit;
            }
            else {
                $scope.tbl_Pro_ProductionTransferDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Pro_ProductionTransferDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Pro_ProductionTransferDetail.MeasurementUnit = null;
            }
            if (item.IsDecimal) {
                $scope.wholeNumberOrNot = new RegExp("^(0\\.[0]*[1-9][0-9]{0,4}|[1-9][0-9]*(\\.[0-9]{1,5})?)$");
            }
            else {
                $scope.wholeNumberOrNot = new RegExp("^[1-9][0-9]*$");
            }

            $scope.tbl_Pro_ProductionTransferDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = null;
            $scope.tbl_Pro_ProductionTransferDetail.ReferenceNo = null;
        };

        $scope.ReferenceSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0) {
                $scope.tbl_Pro_ProductionTransferDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo;
                $scope.tbl_Pro_ProductionTransferDetail.ReferenceNo = item.ReferenceNo;
            }
            else {
                $scope.tbl_Pro_ProductionTransferDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = null;
                $scope.tbl_Pro_ProductionTransferDetail.ReferenceNo = null;
            }
        };

        $scope.tbl_Pro_ProductionTransferDetail = {
            'ID': 0, 'FK_tbl_Pro_ProductionTransferMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null, 'ReferenceNo': '',
            'Quantity': 0, 'Remarks': '', 'QACleared': null, 'QAClearedBy': null, 'QAClearedDate': null,
            'Received': false, 'ReceivedBy': null, 'ReceivedDate': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_ProductionTransferDetails = [$scope.tbl_Pro_ProductionTransferDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_ProductionTransferDetail = {
                'ID': 0, 'FK_tbl_Pro_ProductionTransferMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null, 'ReferenceNo': '',
                'Quantity': 0, 'Remarks': '', 'QACleared': null, 'QAClearedBy': null, 'QAClearedDate': null,
                'Received': false, 'ReceivedBy': null, 'ReceivedDate': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_ProductionTransferDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_ProductionTransferDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };        

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    