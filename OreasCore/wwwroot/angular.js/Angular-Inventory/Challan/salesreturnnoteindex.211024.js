MainModule
    .controller("SalesReturnNoteMasterCtlr", function ($scope, $http) {
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
            '/Inventory/Challan/SalesReturnNoteMasterLoad', //--v_Load
            '/Inventory/Challan/SalesReturnNoteMasterGet', // getrow
            '/Inventory/Challan/SalesReturnNoteMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/Challan/GetInitializedSalesReturnNote');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'SalesReturnNoteMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'SalesReturnNoteMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'SalesReturnNoteMasterCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'SalesReturnNoteDetailCtlr') != undefined) {
                $scope.$broadcast('init_SalesReturnNoteDetailCtlr', data.find(o => o.Controller === 'SalesReturnNoteDetailCtlr'));
            }
        };

        init_ProductSearchModalGeneral($scope, $http);
        init_ReferenceSearchModalGeneral($scope, $http);
        init_COASearchModalGeneral($scope, $http);
        init_WHMSearchModalGeneral($scope, $http);
        init_SDSearchModalGeneral($scope, $http);

        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Inv_WareHouseMaster_ID = item.ID;
                $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Inv_WareHouseMaster_IDName = item.WareHouseName;
            }
            else {
                $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Inv_WareHouseMaster_ID = null;
                $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Inv_WareHouseMaster_IDName = null;
            }
        };

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {

                if ($scope.COASearch_CallerName === 'tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_IDName') {
                    $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                    $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;

                    $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_CustomerSubDistributorList_ID = null;
                    $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_CustomerSubDistributorList_IDName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID_TransporterName') {
                    $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID_Transporter = item.ID;
                    $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID_TransporterName = item.AccountName;
                }
            }
            else {

                if ($scope.COASearch_CallerName === 'tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_IDName') {
                    $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID = null;
                    $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_IDName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID_TransporterName') {
                    $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID_Transporter = null;
                    $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID_TransporterName = null;
                }
            }

        };

        $scope.SDSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_CustomerSubDistributorList_ID = item.ID;
                $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_CustomerSubDistributorList_IDName = item.Name;
            }
            else {
                $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_CustomerSubDistributorList_ID = null;
                $scope.tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_CustomerSubDistributorList_IDName = null;
            }
        };
        
        $scope.tbl_Inv_SalesReturnNoteMaster = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
            'FK_tbl_Ac_CustomerSubDistributorList_ID': null, 'FK_tbl_Ac_CustomerSubDistributorList_IDName': '', 'Remarks': '', 'TotalNetAmount': 0,
            'IsProcessedAll': false, 'IsSupervisedAll': false,
            'FK_tbl_Ac_ChartOfAccounts_ID_Transporter': null, 'FK_tbl_Ac_ChartOfAccounts_ID_TransporterName': '', 'TransportCharges': 0, 'TransporterBiltyNo': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_SalesReturnNoteMasters = [$scope.tbl_Inv_SalesReturnNoteMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_SalesReturnNoteMaster = {
                'ID': 0, 'DocNo': '', 'DocDate': new Date(),
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
                'FK_tbl_Ac_CustomerSubDistributorList_ID': null, 'FK_tbl_Ac_CustomerSubDistributorList_IDName': '', 'Remarks': '', 'TotalNetAmount': 0,
                'IsProcessedAll': false, 'IsSupervisedAll': false,
                'FK_tbl_Ac_ChartOfAccounts_ID_Transporter': null, 'FK_tbl_Ac_ChartOfAccounts_ID_TransporterName': '', 'TransportCharges': 0, 'TransporterBiltyNo': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_SalesReturnNoteMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Inv_SalesReturnNoteMaster = data; $scope.tbl_Inv_SalesReturnNoteMaster.DocDate = new Date(data.DocDate);  
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("SalesReturnNoteDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('SalesReturnNoteDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');         
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_SalesReturnNoteDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
            init_Report($scope, itm.Reports, '/Inventory/Challan/GetSalesReturnNoteReport'); 
        });

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Inv_SalesReturnNoteDetail.MeasurementUnit = item.MeasurementUnit;                
            }
            else {

                $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Inv_SalesReturnNoteDetail.MeasurementUnit = null;
            }

            if (item.IsDecimal) {
                $scope.wholeNumberOrNot = new RegExp("^(0\\.[0]*[1-9][0-9]{0,4}|[1-9][0-9]*(\\.[0-9]{1,5})?)$");
            }
            else {
                $scope.wholeNumberOrNot = new RegExp("^[1-9][0-9]*$");
            }

            $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = null;
            $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = null;
            $scope.tbl_Inv_SalesReturnNoteDetail.ReferenceNo = null;
            $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_SalesNoteDetail_ID = null;
            $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_SalesNoteDetail_IDName = '';
            $scope.tbl_Inv_SalesReturnNoteDetail.Quantity = 0;

            
        };

        $scope.ReferenceSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {

            if (item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 || item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0) {
                $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo;
                $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo;
                $scope.tbl_Inv_SalesReturnNoteDetail.ReferenceNo = item.ReferenceNo;
                $scope.Balance = item.Balance;
            }
            else {
                $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = null;
                $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = null;
                $scope.tbl_Inv_SalesReturnNoteDetail.ReferenceNo = null;                
          
            }

            $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_SalesNoteDetail_ID = null;
            $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_SalesNoteDetail_IDName = '';
            $scope.tbl_Inv_SalesReturnNoteDetail.Quantity = 0;
        };

        init_Operations($scope, $http,
            '/Inventory/Challan/SalesReturnNoteDetailLoad', //--v_Load
            '/Inventory/Challan/SalesReturnNoteDetailGet', // getrow
            '/Inventory/Challan/SalesReturnNoteDetailPost' // PostRow
        );

        $scope.tbl_Inv_SalesReturnNoteDetail = {
            'ID': 0, 'FK_tbl_Inv_SalesReturnNoteMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_SalesNoteDetail_ID': null, 'FK_tbl_Inv_SalesNoteDetail_IDName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null,
            'ReferenceNo': '', 'Quantity': 0, 'Remarks': '', 'IsProcessed': false, 'IsSupervised': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_SalesReturnNoteDetails = [$scope.tbl_Inv_SalesReturnNoteDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_SalesReturnNoteDetail = {
                'ID': 0, 'FK_tbl_Inv_SalesReturnNoteMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_SalesNoteDetail_ID': null, 'FK_tbl_Inv_SalesNoteDetail_IDName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null,
                'ReferenceNo': '', 'Quantity': 0, 'Remarks': '', 'IsProcessed': false, 'IsSupervised': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_SalesReturnNoteDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_SalesReturnNoteDetail = data;
            if (data.IsDecimal) {
                $scope.wholeNumberOrNot = new RegExp("^(0\\.[0]*[1-9][0-9]{0,4}|[1-9][0-9]*(\\.[0-9]{1,5})?)$");
            }
            else {
                $scope.wholeNumberOrNot = new RegExp("^[1-9][0-9]*$");
            }
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };   

        //---------------------------Sale Note Search Modal----------------------------//
        $scope.SalesNoteSearchResult = [{ 'ID': null, 'InvoiceNo': null, 'InvoiceDate': null, 'Quantity': 0, 'MeasurementUnit': '' }];

        $scope.OpenSalesNoteSearchModal = function () {
            if ($scope.SalesNoteFilterBy === undefined || $scope.SalesNoteFilterBy === null) {
                $scope.SalesNoteFilterBy = 'InvoiceNo';
            }

            $scope.SalesNoteSearchResult.length = 0;
            $scope.SalesNoteFilterValue = '';

            $('#SalesNoteSearchModal').modal('show');
        };

        $scope.SalesNoteSearch = function () {

            var successcallback = function (response) {
                $scope.SalesNoteSearchResult = response.data;
            };
            var errorcallback = function (error) {
            };
            $http({ method: "GET", url: "/Inventory/Challan/GetSalesNoteForReturn?CustomerID=" + $scope.MasterObject.FK_tbl_Ac_ChartOfAccounts_ID + "&PurchaseRefID=" + $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo + "&BMRRefID=" + $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo + '&SalesNoteFilterBy=' + $scope.SalesNoteFilterBy + '&SalesNoteFilterValue=' + $scope.SalesNoteFilterValue, async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

        };

        $scope.SalesNoteSelectedAc = function (item) {
           
            if (item.ID > 0) {
                
                $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_SalesNoteDetail_ID = item.ID;
                $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_SalesNoteDetail_IDName = item.InvoiceNo;
            }
            else
            {
                $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_SalesNoteDetail_ID = null;
                $scope.tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_SalesNoteDetail_IDName = '';
            }               
        };

        $(function () {
            $('#SalesNoteSearchModal').on('shown.bs.modal', function () {
                $('#SalesNoteFilterBy').focus();
            });
        });

        $(function () {
            $('#SalesNoteSearchModal').on('hidden.bs.modal', function () {

                $('[name="tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_SalesNoteDetail_IDName"]').focus();
            });
        });


    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    