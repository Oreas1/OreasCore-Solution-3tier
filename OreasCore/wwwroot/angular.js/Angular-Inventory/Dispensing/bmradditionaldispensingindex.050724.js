MainModule
    .controller("BMRAdditionalDispensingMasterCtlr", function ($scope, $http) {
       
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
            '/Inventory/Dispensing/BMRAdditionalDispensingMasterLoad', //--v_Load
            '/Inventory/Dispensing/BMRAdditionalDispensingMasterGet', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/Dispensing/GetInitializedBMRAdditionalDispensing');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'BMRAdditionalDispensingMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'BMRAdditionalDispensingMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'BMRAdditionalDispensingMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'BMRAdditionalDispensingMasterCtlr').LoadByCard);                
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'BMRAdditionalDispensingDetailCtlr') != undefined) {
                $scope.$broadcast('init_BMRAdditionalDispensingDetailCtlr', data.find(o => o.Controller === 'BMRAdditionalDispensingDetailCtlr'));
            }
            if (data.find(o => o.Controller === 'BMRAdditionalDispensingDetailDispensingCtlr') != undefined) {
                $scope.$broadcast('init_BMRAdditionalDispensingDetailDispensingCtlr', data.find(o => o.Controller === 'BMRAdditionalDispensingDetailDispensingCtlr'));
            }
        };

        init_ReferenceSearchModalGeneral($scope, $http);

        $scope.tbl_Pro_BMRAdditionalMaster = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
            'FK_tbl_Pro_BatchMaterialRequisitionMaster_ID': null, 'FK_tbl_Pro_BatchMaterialRequisitionMaster_IDName': '',
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '', 'IsDispensedAll': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BMRAdditionalMasters = [$scope.tbl_Pro_BMRAdditionalMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BMRAdditionalMaster = {
                'ID': 0, 'DocNo': '', 'DocDate': new Date(),
                'FK_tbl_Pro_BatchMaterialRequisitionMaster_ID': null, 'FK_tbl_Pro_BatchMaterialRequisitionMaster_IDName': '',
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '', 'IsDispensedAll': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_BMRAdditionalMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Pro_BMRAdditionalMaster = data; $scope.tbl_Pro_BMRAdditionalMaster.DocDate = new Date(data.DocDate);
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
                method: "POST", url: '/Inventory/Dispensing/BMRAdditionalStockIssuanceReservationItemPost', async: false, params: { BMR_RawItemID: BMR_RawItemID, BMR_PackagingItemID: BMR_PackagingItemID, BMR_AdditionalItemID: BMR_AdditionalItemID, OR_ItemID: OR_ItemID, operation: 'Save New' }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'NOSpinner': true, 'RequestVerificationToken': $scope.antiForgeryToken }
            }).then(successcallback, errorcallback);
        };
       
    })
    .controller("BMRAdditionalDispensingDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('BMRAdditionalDispensingDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            

        });

        $scope.$on('init_BMRAdditionalDispensingDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
        });


        init_Operations($scope, $http,
            '/Inventory/Dispensing/BMRAdditionalDispensingDetailLoad', //--v_Load
            '/Inventory/Dispensing/BMRAdditionalDispensingDetailGet', // getrow
            '/Inventory/Dispensing/BMRAdditionalDispensingDetailPost' // PostRow
        ); 

        $scope.tbl_Pro_BMRAdditionalDetail = {
            'ID': 0, 'FK_tbl_Pro_BMRAdditionalMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Pro_BMRAdditionalType_ID': null, 'FK_tbl_Pro_BMRAdditionalType_IDName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'Remarks': '', 'IsDispensed': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BMRAdditionalDetails = [$scope.tbl_Pro_BMRAdditionalDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BMRAdditionalDetail = {
                'ID': 0, 'FK_tbl_Pro_BMRAdditionalMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Pro_BMRAdditionalType_ID': null, 'FK_tbl_Pro_BMRAdditionalType_IDName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'Quantity': 0, 'Remarks': '', 'IsDispensed': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_BMRAdditionalDetail }; };

        $scope.CloseDispensingCalled = false;

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_BMRAdditionalDetail = data;

            if ($scope.CloseDispensingCalled) {
                $scope.tbl_Pro_BMRAdditionalDetail.IsDispensed = true;
                $scope.CloseDispensingCalled = false;
            } 
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        $scope.CloseDispensing = function (id) {
            $scope.CloseDispensingCalled = true;
            $scope.GetRow(id, 'Add');
        };

    })
    .controller("BMRAdditionalDispensingDetailDispensingCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('BMRAdditionalDispensingDetailDispensingCtlr', function (e, itm) {
            $scope.MasterObject = itm;      

            if (itm.IsDecimal) { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+(\.[0-9]{1,4})?$"); }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+$"); }

            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_BMRAdditionalDispensingDetailDispensingCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/Inventory/Dispensing/GetBMRAdditionalDispensingDetailDispensingReport'); 
        });


        init_Operations($scope, $http,
            '/Inventory/Dispensing/BMRAdditionalDispensingDetailDispensingLoad', //--v_Load
            '/Inventory/Dispensing/BMRAdditionalDispensingDetailDispensingGet', // getrow
            '/Inventory/Dispensing/BMRAdditionalDispensingDetailDispensingPost' // PostRow
        );

        $scope.Balance = 0;
        $scope.ReferenceSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {

            if (item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 || item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0) {
                $scope.tbl_Inv_BMRAdditionalDispensing.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo;
                $scope.tbl_Inv_BMRAdditionalDispensing.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo;
                $scope.tbl_Inv_BMRAdditionalDispensing.ReferenceNo = item.ReferenceNo;
                $scope.Balance = item.Balance;
            }
            else {

                $scope.tbl_Inv_BMRAdditionalDispensing.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = null;
                $scope.tbl_Inv_BMRAdditionalDispensing.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = null;
                $scope.tbl_Inv_BMRAdditionalDispensing.ReferenceNo = null;
                $scope.Balance = 0;
            }

            $scope.tbl_Inv_BMRAdditionalDispensing.Quantity = 0;
        };

        $scope.tbl_Inv_BMRAdditionalDispensing = {
            'ID': 0, 'FK_tbl_Pro_BMRAdditionalDetail_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null,
            'ReferenceNo': '', 'Quantity': 0, 'DispensingDate': new Date(), 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };  

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_BMRAdditionalDispensings = [$scope.tbl_Inv_BMRAdditionalDispensing];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_BMRAdditionalDispensing = {
                'ID': 0, 'FK_tbl_Pro_BMRAdditionalDetail_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null,
                'ReferenceNo': '', 'Quantity': 0, 'DispensingDate': new Date(), 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };  
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_BMRAdditionalDispensing }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_BMRAdditionalDispensing = data;
            $scope.tbl_Inv_BMRAdditionalDispensing.DispensingDate = new Date(data.DispensingDate);
            $scope.Balance = data.Quantity;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    