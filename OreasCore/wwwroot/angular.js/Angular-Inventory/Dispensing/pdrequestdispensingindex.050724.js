MainModule
    .controller("PDRequestCFPMasterCtlr", function ($scope, $http) {
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
            '/Inventory/Dispensing/PDRequestCFPMasterLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/Dispensing/GetInitializedPDRequest');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'PDRequestCFPMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'PDRequestCFPMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'PDRequestCFPMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'PDRequestCFPMasterCtlr').LoadByCard);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'PDRequestCFPDetailItemCtlr') != undefined) {
                $scope.$broadcast('init_PDRequestCFPDetailItemCtlr', data.find(o => o.Controller === 'PDRequestCFPDetailItemCtlr'));
            }
            if (data.find(o => o.Controller === 'PDRequestCFPDetailItemDispensingCtlr') != undefined) {
                $scope.$broadcast('init_PDRequestCFPDetailItemDispensingCtlr', data.find(o => o.Controller === 'PDRequestCFPDetailItemDispensingCtlr'));
            }
        };

        init_ReferenceSearchModalGeneral($scope, $http);
        
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("PDRequestCFPDetailItemCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('PDRequestCFPDetailItemCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');       
        });

        $scope.$on('init_PDRequestCFPDetailItemCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, itm.LoadByCard); 
        });

        init_Operations($scope, $http,
            '/Inventory/Dispensing/PDRequestCFPDetailItemLoad', //--v_Load
            '/Inventory/Dispensing/PDRequestCFPDetailItemGet', // getrow
            '/Inventory/Dispensing/PDRequestCFPDetailItemPost' // PostRow
        );

        $scope.tbl_PD_RequestDetailTR_CFP_Item = {
            'ID': 0, 'FK_tbl_PD_RequestDetailTR_CFP_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'RequiredTrue_ReturnFalse': true, 'Remarks': '', 'IsDispensed': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_PD_RequestDetailTR_CFP_Items = [$scope.tbl_PD_RequestDetailTR_CFP_Item];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_PD_RequestDetailTR_CFP_Item = {
                'ID': 0, 'FK_tbl_PD_RequestDetailTR_CFP_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'Quantity': 0, 'RequiredTrue_ReturnFalse': true, 'Remarks': '', 'IsDispensed': 0,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_PD_RequestDetailTR_CFP_Item }; };

        $scope.CloseDispensingCalled = false;

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_PD_RequestDetailTR_CFP_Item = data;

            if ($scope.CloseDispensingCalled) {
                $scope.tbl_PD_RequestDetailTR_CFP_Item.IsDispensed = true;
                $scope.CloseDispensingCalled = false;
            }                
                
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        $scope.CloseDispensing = function (id) {
            $scope.CloseDispensingCalled = true;
            $scope.GetRow(id, 'Add');
        };

    })
    .controller("PDRequestCFPDetailItemDispensingCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('PDRequestCFPDetailItemDispensingCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');

            if (itm.IsDecimal) { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+(\.[0-9]{1,4})?$"); }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+$"); }
        });

        $scope.$on('init_PDRequestCFPDetailItemDispensingCtlr', function (e, itm) {
            init_Filter($scope, null, null, null, null);
        });

        init_Operations($scope, $http,
            '/Inventory/Dispensing/PDRequestCFPDetailItemDispensingLoad', //--v_Load
            '/Inventory/Dispensing/PDRequestCFPDetailItemDispensingGet', // getrow
            '/Inventory/Dispensing/PDRequestCFPDetailItemDispensingPost' // PostRow
        );        
        $scope.Balance = 0;
        $scope.ReferenceSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {

            if (item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 || item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0) {
                $scope.tbl_Inv_PDRequestDispensing.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo;
                $scope.tbl_Inv_PDRequestDispensing.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo;
                $scope.tbl_Inv_PDRequestDispensing.ReferenceNo = item.ReferenceNo;
                $scope.Balance = item.Balance;
            }
            else {

                $scope.tbl_Inv_PDRequestDispensing.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = null;
                $scope.tbl_Inv_PDRequestDispensing.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = null;
                $scope.tbl_Inv_PDRequestDispensing.ReferenceNo = null;
                $scope.Balance = 0;
            }

            $scope.tbl_Inv_PDRequestDispensing.Quantity = 0;
        };
        $scope.tbl_Inv_PDRequestDispensing = {
            'ID': 0, 'FK_tbl_PD_RequestDetailTR_CFP_Item_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null,
            'ReferenceNo': '', 'Quantity': 0, 'DispensingDate': new Date(), 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        }; 

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PDRequestDispensings = [$scope.tbl_Inv_PDRequestDispensing];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_PDRequestDispensing = {
                'ID': 0, 'FK_tbl_PD_RequestDetailTR_CFP_Item_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null,
                'ReferenceNo': '', 'Quantity': 0, 'DispensingDate': new Date(), 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            }; 
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PDRequestDispensing }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_PDRequestDispensing = data;
            $scope.tbl_Inv_PDRequestDispensing.DispensingDate = new Date(data.DispensingDate);
            $scope.Balance = data.Quantity;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });
