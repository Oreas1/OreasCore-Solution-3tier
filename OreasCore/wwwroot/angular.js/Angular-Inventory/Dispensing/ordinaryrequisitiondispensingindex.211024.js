MainModule
    .controller("OrdinaryRequisitionDispensingMasterCtlr", function ($scope, $http) {
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
            '/Inventory/Dispensing/OrdinaryRequisitionDispensingMasterLoad', //--v_Load
            '/Inventory/Dispensing/OrdinaryRequisitionDispensingMasterGet', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/Dispensing/GetInitializedOrdinaryRequisitionDispensing');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'OrdinaryRequisitionDispensingMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'OrdinaryRequisitionDispensingMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'OrdinaryRequisitionDispensingMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'OrdinaryRequisitionDispensingMasterCtlr').LoadByCard);                
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'OrdinaryRequisitionDispensingDetailCtlr') != undefined) {
                $scope.$broadcast('init_OrdinaryRequisitionDispensingDetailCtlr', data.find(o => o.Controller === 'OrdinaryRequisitionDispensingDetailCtlr'));
            }
            if (data.find(o => o.Controller === 'OrdinaryRequisitionDispensingDetailDispensingCtlr') != undefined) {
                $scope.$broadcast('init_OrdinaryRequisitionDispensingDetailDispensingCtlr', data.find(o => o.Controller === 'OrdinaryRequisitionDispensingDetailDispensingCtlr'));
            }
        };

        init_ReferenceSearchModalGeneral($scope, $http); ($scope, $http);

        $scope.tbl_Inv_OrdinaryRequisitionMaster = {
            'ID': 0, 'DocNo': null, 'DocDate': new Date(),
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '', 'IsDispensedAll': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        }; 

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_OrdinaryRequisitionMasters = [$scope.tbl_Inv_OrdinaryRequisitionMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_OrdinaryRequisitionMaster = {
                'ID': 0, 'DocNo': null, 'DocDate': new Date(),
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '', 'IsDispensedAll': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            }; 
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_OrdinaryRequisitionMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Inv_OrdinaryRequisitionMaster = data; $scope.tbl_Inv_OrdinaryRequisitionMaster.DocDate = new Date(data.DocDate);
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
                method: "POST", url: '/Inventory/Dispensing/OrdinaryStockIssuanceReservationItemPost', async: false,
                params: { BMR_RawItemID: BMR_RawItemID, BMR_PackagingItemID: BMR_PackagingItemID, BMR_AdditionalItemID: BMR_AdditionalItemID, OR_ItemID: OR_ItemID, operation: 'Save New' },
                headers: { 'X-Requested-With': 'XMLHttpRequest', 'NOSpinner': false, 'RequestVerificationToken': $scope.antiForgeryToken },
                timeout: 15000
            }).then(successcallback, errorcallback);
        };
       
    })
    .controller("OrdinaryRequisitionDispensingDetailCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('OrdinaryRequisitionDispensingDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first'); 
        });

        $scope.$on('init_OrdinaryRequisitionDispensingDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
        });

        init_Operations($scope, $http,
            '/Inventory/Dispensing/OrdinaryRequisitionDispensingDetailLoad', //--v_Load
            '/Inventory/Dispensing/OrdinaryRequisitionDispensingDetailGet', // getrow
            '/Inventory/Dispensing/OrdinaryRequisitionDispensingDetailPost' // PostRow
        ); 

        $scope.tbl_Inv_OrdinaryRequisitionDetail = {
            'ID': 0, 'FK_tbl_Inv_OrdinaryRequisitionMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_OrdinaryRequisitionType_ID': null, 'FK_tbl_Inv_OrdinaryRequisitionType_IDName': '', 'RequiredTrue_ReturnFalse': true,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'Remarks': '', 'IsDispensed': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_OrdinaryRequisitionDetails = [$scope.tbl_Inv_OrdinaryRequisitionDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_OrdinaryRequisitionDetail = {
                'ID': 0, 'FK_tbl_Inv_OrdinaryRequisitionMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_OrdinaryRequisitionType_ID': null, 'FK_tbl_Inv_OrdinaryRequisitionType_IDName': '', 'RequiredTrue_ReturnFalse': true,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'Quantity': 0, 'Remarks': '', 'IsDispensed': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_OrdinaryRequisitionDetail }; };

        $scope.CloseDispensingCalled = false;

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_OrdinaryRequisitionDetail = data;

            if ($scope.CloseDispensingCalled) {
                $scope.tbl_Inv_OrdinaryRequisitionDetail.IsDispensed = true;
                $scope.CloseDispensingCalled = false;
            } 
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        $scope.CloseDispensing = function (id) {
            $scope.CloseDispensingCalled = true;
            $scope.GetRow(id, 'Add');
        };
    })
    .controller("OrdinaryRequisitionDispensingDetailDispensingCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('OrdinaryRequisitionDispensingDetailDispensingCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            if (itm.IsDecimal) {
                $scope.wholeNumberOrNot = new RegExp("^(0\\.[0]*[1-9][0-9]{0,4}|[1-9][0-9]*(\\.[0-9]{1,5})?)$");
            }
            else {
                $scope.wholeNumberOrNot = new RegExp("^[1-9][0-9]*$");
            }
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });
        

        $scope.$on('init_OrdinaryRequisitionDispensingDetailDispensingCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/Inventory/Dispensing/GetOrdinaryRequisitionDispensingReport'); 
        });


        init_Operations($scope, $http,
            '/Inventory/Dispensing/OrdinaryRequisitionDispensingDetailDispensingLoad', //--v_Load
            '/Inventory/Dispensing/OrdinaryRequisitionDispensingDetailDispensingGet', // getrow
            '/Inventory/Dispensing/OrdinaryRequisitionDispensingDetailDispensingPost' // PostRow
        );

        $scope.Balance = 0;
        $scope.ReferenceSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {

            if (item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 || item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0) {
                $scope.tbl_Inv_OrdinaryRequisitionDispensing.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo;
                $scope.tbl_Inv_OrdinaryRequisitionDispensing.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo;
                $scope.tbl_Inv_OrdinaryRequisitionDispensing.ReferenceNo = item.ReferenceNo;
                $scope.Balance = item.Balance;
            }
            else {

                $scope.tbl_Inv_OrdinaryRequisitionDispensing.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = null;
                $scope.tbl_Inv_OrdinaryRequisitionDispensing.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = null;
                $scope.tbl_Inv_OrdinaryRequisitionDispensing.ReferenceNo = null;
                $scope.Balance = 0;
            }

            $scope.tbl_Inv_OrdinaryRequisitionDispensing.Quantity = 0;
        };

        $scope.tbl_Inv_OrdinaryRequisitionDispensing = {
            'ID': 0, 'FK_tbl_Inv_OrdinaryRequisitionDetail_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null,
            'ReferenceNo': '', 'Quantity': 0, 'DispensingDate': new Date(), 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };  

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_OrdinaryRequisitionDispensings = [$scope.tbl_Inv_OrdinaryRequisitionDispensing];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_OrdinaryRequisitionDispensing = {
                'ID': 0, 'FK_tbl_Inv_OrdinaryRequisitionDetail_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null,
                'ReferenceNo': '', 'Quantity': 0, 'DispensingDate': new Date(), 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };  
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_OrdinaryRequisitionDispensing }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_OrdinaryRequisitionDispensing = data;
            $scope.tbl_Inv_OrdinaryRequisitionDispensing.DispensingDate = new Date(data.DispensingDate);
            $scope.Balance = data.Quantity;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    