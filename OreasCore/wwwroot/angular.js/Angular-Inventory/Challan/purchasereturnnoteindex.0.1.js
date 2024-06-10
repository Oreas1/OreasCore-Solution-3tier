MainModule
    .controller("PurchaseReturnNoteMasterCtlr", function ($scope, $http) {
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
            '/Inventory/Challan/PurchaseReturnNoteMasterLoad', //--v_Load
            '/Inventory/Challan/PurchaseReturnNoteMasterGet', // getrow
            '/Inventory/Challan/PurchaseReturnNoteMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/Challan/GetInitializedPurchaseReturnNote');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'PurchaseReturnNoteMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'PurchaseReturnNoteMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'PurchaseReturnNoteMasterCtlr').WildCard, null, null, null);

                const urlParams = new URLSearchParams(window.location.search);

                if (urlParams.get('byDocNo') != null) {
                    $scope.FilterByText = 'byDocNo';
                    $scope.FilterValueByText = urlParams.get('byDocNo');
                }

                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'PurchaseReturnNoteDetailCtlr') != undefined) {
                $scope.$broadcast('init_PurchaseReturnNoteDetailCtlr', data.find(o => o.Controller === 'PurchaseReturnNoteDetailCtlr'));
            }
        };

        init_ProductSearchModalGeneral($scope, $http);
        init_ReferenceSearchModalGeneral($scope, $http);
        init_COASearchModalGeneral($scope, $http);
        init_WHMSearchModalGeneral($scope, $http);

        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_PurchaseReturnNoteMaster.FK_tbl_Inv_WareHouseMaster_ID = item.ID;
                $scope.tbl_Inv_PurchaseReturnNoteMaster.FK_tbl_Inv_WareHouseMaster_IDName = item.WareHouseName;
            }
            else {
                $scope.tbl_Inv_PurchaseReturnNoteMaster.FK_tbl_Inv_WareHouseMaster_ID = null;
                $scope.tbl_Inv_PurchaseReturnNoteMaster.FK_tbl_Inv_WareHouseMaster_IDName = null;
            }
        };

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_PurchaseReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                $scope.tbl_Inv_PurchaseReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;
            }
            else {
                $scope.tbl_Inv_PurchaseReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID = null;
                $scope.tbl_Inv_PurchaseReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_IDName = null;
            }

        };
        
        $scope.tbl_Inv_PurchaseReturnNoteMaster = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'Remarks': '', 'TotalNetAmount': 0,
            'IsProcessedAll': false, 'IsSupervisedAll': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseReturnNoteMasters = [$scope.tbl_Inv_PurchaseReturnNoteMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_PurchaseReturnNoteMaster = {
                'ID': 0, 'DocNo': '', 'DocDate': new Date(),
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'Remarks': '', 'TotalNetAmount': 0,
                'IsProcessedAll': false, 'IsSupervisedAll': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseReturnNoteMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Inv_PurchaseReturnNoteMaster = data; $scope.tbl_Inv_PurchaseReturnNoteMaster.DocDate = new Date(data.DocDate);  
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("PurchaseReturnNoteDetailCtlr", function ($scope, $http) {        
        $scope.MasterObject = {};
        $scope.$on('PurchaseReturnNoteDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_PurchaseReturnNoteDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
            init_Report($scope, itm.Reports, '/Inventory/Challan/GetPurchaseReturnNoteReport'); 
        });

        
        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_PurchaseReturnNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Inv_PurchaseReturnNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Inv_PurchaseReturnNoteDetail.MeasurementUnit = item.MeasurementUnit; 
            }
            else {

                $scope.tbl_Inv_PurchaseReturnNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Inv_PurchaseReturnNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Inv_PurchaseReturnNoteDetail.MeasurementUnit = null;                
            }

            $scope.tbl_Inv_PurchaseReturnNoteDetail.Quantity = 0;
            $scope.tbl_Inv_PurchaseReturnNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = null;
            $scope.tbl_Inv_PurchaseReturnNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNoName = '';

            if (item.IsDecimal) { $scope.wholeNumberOrNot = ''; }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9][^\.]*$"); }
            
        };

        $scope.Balance = 0;
        $scope.ReferenceSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
  
            
            if (item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 || item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0) {
                $scope.tbl_Inv_PurchaseReturnNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo;
                $scope.tbl_Inv_PurchaseReturnNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNoName = item.ReferenceNo;
                $scope.tbl_Inv_PurchaseReturnNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_IDName = item.OtherDetail;
                $scope.Balance = item.Balance;
            }
            else {

                $scope.tbl_Inv_PurchaseReturnNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = null;
                $scope.tbl_Inv_PurchaseReturnNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNoName = null;
                $scope.tbl_Inv_PurchaseReturnNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_IDName = null;
                $scope.Balance = 0;
            }

            $scope.tbl_Inv_PurchaseReturnNoteDetail.Quantity = 0;
        };

        init_Operations($scope, $http,
            '/Inventory/Challan/PurchaseReturnNoteDetailLoad', //--v_Load
            '/Inventory/Challan/PurchaseReturnNoteDetailGet', // getrow
            '/Inventory/Challan/PurchaseReturnNoteDetailPost' // PostRow
        );

        $scope.tbl_Inv_PurchaseReturnNoteDetail = {
            'ID': 0, 'FK_tbl_Inv_PurchaseReturnNoteMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNoName': '',
            'Quantity': 0, 'Remarks': '', 'IsProcessed': false, 'IsSupervised': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'FK_tbl_Inv_PurchaseNoteDetail_IDName': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseReturnNoteDetails = [$scope.tbl_Inv_PurchaseReturnNoteDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_PurchaseReturnNoteDetail = {
                'ID': 0, 'FK_tbl_Inv_PurchaseReturnNoteMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNoName': '',
                'Quantity': 0, 'Remarks': '', 'IsProcessed': false, 'IsSupervised': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'FK_tbl_Inv_PurchaseNoteDetail_IDName': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseReturnNoteDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_PurchaseReturnNoteDetail = data;
            $scope.Balance = data.Quantity;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };        

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    