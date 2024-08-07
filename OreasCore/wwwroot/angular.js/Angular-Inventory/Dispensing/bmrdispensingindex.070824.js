MainModule
    .controller("BMRDispensingMasterCtlr", function ($scope, $window, $http) {
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
            '/Inventory/Dispensing/BMRDispensingMasterLoad', //--v_Load
            '/Inventory/Dispensing/BMRDispensingMasterGet', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/Dispensing/GetInitializedBMRDispensing');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'BMRDispensingMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'BMRDispensingMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'BMRDispensingMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'BMRDispensingMasterCtlr').LoadByCard);                
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'BMRDispensingDetailRawItemsCtlr') != undefined) {
                $scope.$broadcast('init_BMRDispensingDetailRawItemsCtlr', data.find(o => o.Controller === 'BMRDispensingDetailRawItemsCtlr'));
            }
            if (data.find(o => o.Controller === 'BMRDispensingRawCtlr') != undefined) {
                $scope.$broadcast('init_BMRDispensingRawCtlr', data.find(o => o.Controller === 'BMRDispensingRawCtlr'));
            }
            if (data.find(o => o.Controller === 'BMRDispensingDetailPackagingDetailItemsCtlr') != undefined) {
                $scope.$broadcast('init_BMRDispensingDetailPackagingDetailItemsCtlr', data.find(o => o.Controller === 'BMRDispensingDetailPackagingDetailItemsCtlr'));
            }
            if (data.find(o => o.Controller === 'BMRDispensingPackagingCtlr') != undefined) {
                $scope.$broadcast('init_BMRDispensingPackagingCtlr', data.find(o => o.Controller === 'BMRDispensingPackagingCtlr'));
            }
        };

        init_ReferenceSearchModalGeneral($scope, $http);

        $scope.tbl_Pro_BatchMaterialRequisitionMaster = {
            'ID': 0, 'DocDate': new Date(), 'BatchNo': null, 'BatchMfgDate': new Date(), 'BatchExpiryDate': new Date(),
            'DimensionValue': 1, 'FK_tbl_Inv_MeasurementUnit_ID_Dimension': 0, 'FK_tbl_Inv_MeasurementUnit_ID_DimensionName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'BatchSizeUnit': '',
            'BatchSize': 1, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
            'FK_tbl_Pro_CompositionDetail_Coupling_ID': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BatchMaterialRequisitionMasters = [$scope.tbl_Pro_BatchMaterialRequisitionMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BatchMaterialRequisitionMaster = {
                'ID': 0, 'DocDate': new Date(), 'BatchNo': null, 'BatchMfgDate': new Date(), 'BatchExpiryDate': new Date(),
                'DimensionValue': 1, 'FK_tbl_Inv_MeasurementUnit_ID_Dimension': 0, 'FK_tbl_Inv_MeasurementUnit_ID_DimensionName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'BatchSizeUnit': '',
                'BatchSize': 1, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
                'FK_tbl_Pro_CompositionDetail_Coupling_ID': 0
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

        $scope.AutoIssuanceRequest = function (BMR_RawItemID, BMR_PackagingItemID, BMR_AdditionalItemID, OR_ItemID, scope, event) {
            event.target.disabled = true;  

            var successcallback = function (response) {
                alert(response.data);
                $scope.callerscope = scope;
                $scope.callerscope.pageNavigation('Load');
                event.target.disabled = false;
            };

            var errorcallback = function (error) {
                console.log('Post error', error);

                if (error.xhrStatus === 'timeout')
                    alert('Network Problem! Request timeout');

               event.target.disabled = false;
            };

            $http({
                method: "POST", url: '/Inventory/Dispensing/BMRStockIssuanceReservationItemPost', async: false,
                params: { BMR_RawItemID: BMR_RawItemID, BMR_PackagingItemID: BMR_PackagingItemID, BMR_AdditionalItemID: BMR_AdditionalItemID, OR_ItemID: OR_ItemID, operation: 'Save New' },
                headers: { 'X-Requested-With': 'XMLHttpRequest', 'NOSpinner': false, 'RequestVerificationToken': $scope.antiForgeryToken },
                timeout: 15000
            }).then(successcallback, errorcallback);
        };

        $scope.GotoReport = function (id) {
            $window.open('/Inventory/Dispensing/GetBMRDispensingReport?rn=BMR Dispensing Detail&id=' + id);
        };
    })
    .controller("BMRDispensingItemsCtlr", function ($scope) {
        $scope.MasterObject = {};
        $scope.$on('BMRDispensingItemsCtlr', function (e, itm) {
            $('[href="#BMRDispensingRaw"]').tab('show');
            $scope.MasterObject = itm;
        });
    })
    .controller("BMRDispensingDetailRawItemsCtlr", function ($scope, $http) {        
        $scope.MasterObject = {};
        $scope.$on('BMRDispensingDetailRawItemsCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');             
        });

        $scope.$on('init_BMRDispensingDetailRawItemsCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
            
        });


        init_Operations($scope, $http,
            '/Inventory/Dispensing/BMRDispensingDetailRawItemsLoad', //--v_Load
            '/Inventory/Dispensing/BMRDispensingDetailRawItemsGet', // getrow
            '' // PostRow
        ); 

        $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawDetail = {
            'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_RawMaster_ID': null,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawDetails = [$scope.tbl_Pro_BatchMaterialRequisitionDetail_RawDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_RawDetail = {
                'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_RawMaster_ID': null,
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
    .controller("BMRDispensingRawCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('BMRDispensingRawCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            if (itm.IsDecimal) { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+(\.[0-9]{1,4})?$"); }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+$"); }
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_BMRDispensingRawCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/Inventory/Dispensing/GetBMRDispensingReport'); 
        });

        init_Operations($scope, $http,
            '/Inventory/Dispensing/BMRDispensingRawLoad', //--v_Load
            '/Inventory/Dispensing/BMRDispensingRawGet', // getrow
            '/Inventory/Dispensing/BMRDispensingRawPost' // PostRow
        );

        $scope.Balance = 0;
        $scope.ReferenceSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {     

            if (item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 || item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0) {
                $scope.tbl_Inv_BMRDispensingRaw.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo;
                $scope.tbl_Inv_BMRDispensingRaw.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo;
                $scope.tbl_Inv_BMRDispensingRaw.ReferenceNo = item.ReferenceNo;
                $scope.Balance = item.Balance;
            }
            else {

                $scope.tbl_Inv_BMRDispensingRaw.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = null;
                $scope.tbl_Inv_BMRDispensingRaw.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = null;
                $scope.tbl_Inv_BMRDispensingRaw.ReferenceNo = null;
                $scope.Balance = 0;
            }

            $scope.tbl_Inv_BMRDispensingRaw.Quantity = 0;
        };

        $scope.tbl_Inv_BMRDispensingRaw = {
            'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_RawDetail_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null,
            'ReferenceNo': '', 'Quantity': 0, 'DispensingDate': new Date(), 'ReservationDate': null, 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };


        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_BMRDispensingRaws = [$scope.tbl_Inv_BMRDispensingRaw];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_BMRDispensingRaw = {
                'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_RawDetail_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null,
                'ReferenceNo': '', 'Quantity': 0, 'DispensingDate': new Date(), 'ReservationDate': null, 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_BMRDispensingRaw }; };

        $scope.GetRowResponse = function (data, operation) {
            if (operation === 'Add' || operation === 'Delete' || operation === 'View') {
                $scope.ng_readOnly = true;
            }
            else
            {
                $scope.ng_readOnly = false;
            }
            $scope.tbl_Inv_BMRDispensingRaw = data;
            $scope.tbl_Inv_BMRDispensingRaw.DispensingDate = data.IsDispensed ? new Date(data.DispensingDate) : new Date();
            $scope.Balance = data.Quantity;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("BMRDispensingDetailPackagingDetailItemsCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('BMRDispensingDetailPackagingDetailItemsCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_BMRDispensingDetailPackagingDetailItemsCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });


        init_Operations($scope, $http,
            '/Inventory/Dispensing/BMRDispensingDetailPackagingDetailItemsLoad', //--v_Load
            '/Inventory/Dispensing/BMRDispensingDetailPackagingDetailItemsGet', // getrow
            '' // PostRow
        );  

        $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items = {
            'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_ID': null,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        }; 

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss = [$scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items = {
                'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_ID': null,
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
    .controller("BMRDispensingPackagingCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('BMRDispensingPackagingCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            if (itm.IsDecimal) { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+(\.[0-9]{1,4})?$"); }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+$"); }
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_BMRDispensingPackagingCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/Inventory/Dispensing/GetBMRDispensingReport'); 
        });

        init_Operations($scope, $http,
            '/Inventory/Dispensing/BMRDispensingPackagingLoad', //--v_Load
            '/Inventory/Dispensing/BMRDispensingPackagingGet', // getrow
            '/Inventory/Dispensing/BMRDispensingPackagingPost' // PostRow
        );

        $scope.Balance = 0;
        $scope.ReferenceSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {

            if (item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 || item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0) {
                $scope.tbl_Inv_BMRDispensingPackaging.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo;
                $scope.tbl_Inv_BMRDispensingPackaging.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo;
                $scope.tbl_Inv_BMRDispensingPackaging.ReferenceNo = item.ReferenceNo;
                $scope.Balance = item.Balance;
            }
            else {

                $scope.tbl_Inv_BMRDispensingPackaging.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = null;
                $scope.tbl_Inv_BMRDispensingPackaging.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = null;
                $scope.tbl_Inv_BMRDispensingPackaging.ReferenceNo = null;
                $scope.Balance = 0;
            }

            $scope.tbl_Inv_BMRDispensingPackaging.Quantity = 0;
        };

        $scope.tbl_Inv_BMRDispensingPackaging = {
            'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null,
            'ReferenceNo': '', 'Quantity': 0, 'DispensingDate': new Date(), 'ReservationDate': null, 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };
 

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_BMRDispensingPackagings = [$scope.tbl_Inv_BMRDispensingPackaging];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_BMRDispensingPackaging = {
                'ID': 0, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null,
                'ReferenceNo': '', 'Quantity': 0, 'DispensingDate': new Date(), 'ReservationDate': null, 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_BMRDispensingPackaging }; };

        $scope.GetRowResponse = function (data, operation) {
            if (operation === 'Add' || operation === 'Delete' || operation === 'View') {
                $scope.ng_readOnly = true;
            }
            else {
                $scope.ng_readOnly = false;
            }
            $scope.tbl_Inv_BMRDispensingPackaging = data;
            $scope.tbl_Inv_BMRDispensingPackaging.DispensingDate = data.IsDispensed ? new Date(data.DispensingDate) : new Date();
            $scope.Balance = data.Quantity;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    