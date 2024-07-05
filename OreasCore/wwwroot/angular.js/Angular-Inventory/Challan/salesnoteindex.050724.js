MainModule
    .controller("SalesNoteMasterCtlr", function ($scope, $http) {
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
            '/Inventory/Challan/SalesNoteMasterLoad', //--v_Load
            '/Inventory/Challan/SalesNoteMasterGet', // getrow
            '/Inventory/Challan/SalesNoteMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/Challan/GetInitializedSalesNote');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'SalesNoteMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'SalesNoteMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'SalesNoteMasterCtlr').WildCard, null, null, null);
                init_Report($scope, data.find(o => o.Controller === 'SalesNoteMasterCtlr').Reports, '/Inventory/Challan/GetSalesNoteReport');
                $scope.rptID = 0;

                const urlParams = new URLSearchParams(window.location.search);
                if (urlParams.get('byDocNo') != null) {
                    $scope.FilterByText = 'byDocNo';
                    $scope.FilterValueByText = urlParams.get('byDocNo');
                }
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'SalesNoteDetailCtlr') != undefined) {
                $scope.$broadcast('init_SalesNoteDetailCtlr', data.find(o => o.Controller === 'SalesNoteDetailCtlr'));
            }
            if (data.find(o => o.Controller === 'SalesNoteDetailReturnCtlr') != undefined) {
                $scope.$broadcast('init_SalesNoteDetailReturnCtlr', data.find(o => o.Controller === 'SalesNoteDetailReturnCtlr'));
            }
        };

        init_ProductSearchModalGeneral($scope, $http);
        init_ReferenceSearchModalGeneral($scope, $http);
        init_COASearchModalGeneral($scope, $http);
        init_WHMSearchModalGeneral($scope, $http);
        init_SDSearchModalGeneral($scope, $http);

        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Inv_WareHouseMaster_ID = item.ID;
                $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Inv_WareHouseMaster_IDName = item.WareHouseName;
            }
            else {
                $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Inv_WareHouseMaster_ID = null;
                $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Inv_WareHouseMaster_IDName = null;
            }
        };

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                
                if ($scope.COASearch_CallerName === 'tbl_Inv_SalesNoteMaster.FK_tbl_Ac_ChartOfAccounts_IDName') {
                    $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                    $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;

                    $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID_Transporter = null;
                    $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID_TransporterName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Inv_SalesNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID_TransporterName') {
                    $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID_Transporter = item.ID;
                    $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID_TransporterName = item.AccountName;
                }
                $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Ac_CustomerSubDistributorList_ID = null;
                $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Ac_CustomerSubDistributorList_IDName = null;
            }
            else {
                
                if ($scope.COASearch_CallerName === 'tbl_Inv_SalesNoteMaster.FK_tbl_Ac_ChartOfAccounts_IDName') {
                    $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID = null;
                    $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Ac_ChartOfAccounts_IDName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Inv_SalesNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID_TransporterName') {
                    $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID_Transporter = null;
                    $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID_TransporterName = null;
                }
            }

        };

        $scope.SDSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Ac_CustomerSubDistributorList_ID = item.ID;
                $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Ac_CustomerSubDistributorList_IDName = item.Name;
            }
            else {
                $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Ac_CustomerSubDistributorList_ID = null;
                $scope.tbl_Inv_SalesNoteMaster.FK_tbl_Ac_CustomerSubDistributorList_IDName = null;
            }
        };
        

        $scope.tbl_Inv_SalesNoteMaster = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
            'FK_tbl_Ac_CustomerSubDistributorList_ID': null, 'FK_tbl_Ac_CustomerSubDistributorList_IDName': '',
            'SupplierChallanNo': '', 'Remarks': '', 'TotalNetAmount': 0, 'IsProcessedAll': false, 'IsSupervisedAll': false,
            'FK_tbl_Ac_ChartOfAccounts_ID_Transporter': null, 'FK_tbl_Ac_ChartOfAccounts_ID_TransporterName': '', 'TransportCharges': 0, 'TransporterBiltyNo': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_SalesNoteMasters = [$scope.tbl_Inv_SalesNoteMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_SalesNoteMaster = {
                'ID': 0, 'DocNo': '', 'DocDate': new Date(),
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
                'FK_tbl_Ac_CustomerSubDistributorList_ID': null, 'FK_tbl_Ac_CustomerSubDistributorList_IDName': '',
                'SupplierChallanNo': '', 'Remarks': '', 'TotalNetAmount': 0, 'IsProcessedAll': false, 'IsSupervisedAll': false,
                'FK_tbl_Ac_ChartOfAccounts_ID_Transporter': null, 'FK_tbl_Ac_ChartOfAccounts_ID_TransporterName': '', 'TransportCharges': 0, 'TransporterBiltyNo': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_SalesNoteMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Inv_SalesNoteMaster = data; $scope.tbl_Inv_SalesNoteMaster.DocDate = new Date(data.DocDate);  
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

       
    })
    .controller("SalesNoteDetailCtlr", function ($scope, $http) {        
        $scope.MasterObject = {};
        $scope.$on('SalesNoteDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');  
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_SalesNoteDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
            init_Report($scope, itm.Reports, '/Inventory/Challan/GetSalesNoteReport'); 
        });

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {

            //--------when product change then Order note should be selected again with respect to new product
            $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Inv_OrderNoteDetail_ID = null;
            $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Inv_OrderNoteDetail_IDName = '';

            if (item.ID > 0) {
                $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Inv_SalesNoteDetail.MeasurementUnit = item.MeasurementUnit;                
            }
            else {

                $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Inv_SalesNoteDetail.MeasurementUnit = null;
                $scope.tbl_Inv_SalesNoteDetail.Quantity = 0;
            }

            if (item.IsDecimal) { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+(\.[0-9]{1,4})?$"); }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+$"); }

            $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = null;
            $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = null;
            $scope.tbl_Inv_SalesNoteDetail.ReferenceNo = null;
            $scope.Balance = 0;            
        };

        $scope.Balance = 0;
        $scope.ReferenceSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {

            if (item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 || item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0) {
                $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = item.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo;
                $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = item.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo;
                $scope.tbl_Inv_SalesNoteDetail.ReferenceNo = item.ReferenceNo;
                $scope.Balance = item.Balance;
            }
            else {
                $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo = null;
                $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo = null;
                $scope.tbl_Inv_SalesNoteDetail.ReferenceNo = null;
                $scope.Balance = 0;
            }

            $scope.tbl_Inv_SalesNoteDetail.Quantity = 0;
        };

        init_Operations($scope, $http,
            '/Inventory/Challan/SalesNoteDetailLoad', //--v_Load
            '/Inventory/Challan/SalesNoteDetailGet', // getrow
            '/Inventory/Challan/SalesNoteDetailPost' // PostRow
        );

        $scope.tbl_Inv_SalesNoteDetail = {
            'ID': 0, 'FK_tbl_Inv_SalesNoteMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null,
            'ReferenceNo': '', 'Quantity': 0, 'Remarks': '', 'IsProcessed': false, 'IsSupervised': false, 'NoOfPackages': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
            'FK_tbl_Inv_OrderNoteDetail_ID': null, 'FK_tbl_Inv_OrderNoteDetail_IDName': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_SalesNoteDetails = [$scope.tbl_Inv_SalesNoteDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_SalesNoteDetail = {
                'ID': 0, 'FK_tbl_Inv_SalesNoteMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null,
                'ReferenceNo': '', 'Quantity': 0, 'Remarks': '', 'IsProcessed': false, 'IsSupervised': false, 'NoOfPackages': 0,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
                'FK_tbl_Inv_OrderNoteDetail_ID': null, 'FK_tbl_Inv_OrderNoteDetail_IDName': ''
            };
            $scope.Balance = 0;
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_SalesNoteDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_SalesNoteDetail = data;
            $scope.Balance = data.Quantity;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };  


        //------------------------Order Note Search Modal-------------------------//
        $scope.ONSearchResult = [{ 'ID': null, 'DocNo': null, 'DocNoDate': null, 'TargetDate': null, 'ProductName': null, 'MeasurementUnit': null, 'Quantity': 0, 'SoldQty': 0 }];

        $scope.OpenONSearchModalGeneral = function () {
            $scope.ONFilterBy = 'byONNo';
            $scope.ONSearchResult.length = 0;
            $scope.ONFilterValue = '';
            $('#ONSearchModalGeneral').modal('show');
        };

        $scope.General_ONSearch = function () {

            var successcallback = function (response) {
                $scope.ONSearchResult = response.data;
            };
            var errorcallback = function (error) {
            };
            $http({ method: "GET", url: "/Inventory/Orders/GetOrderNoteList?QueryName=OrderNote" + "&ONFilterBy=" + $scope.ONFilterBy + "&ONFilterValue=" + $scope.ONFilterValue + "&CustomerID=" + $scope.MasterObject.FK_tbl_Ac_ChartOfAccounts_ID + "&ProductID=" + $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID, async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

        };

        $scope.General_ONSelectedAc = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Inv_OrderNoteDetail_ID = item.ID;
                $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Inv_OrderNoteDetail_IDName = item.DocNo;
                $scope.tbl_Inv_SalesNoteDetail.Quantity = 0;

            }
            else {
                $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Inv_OrderNoteDetail_ID = null;
                $scope.tbl_Inv_SalesNoteDetail.FK_tbl_Inv_OrderNoteDetail_IDName = null;
            }
        };

        $(function () {
            $('#ONSearchModalGeneral').on('shown.bs.modal', function () {
                $('#ONFilterBy').focus();
            });
        });

        $(function () {
            $('#ONSearchModalGeneral').on('hidden.bs.modal', function () {
                $('[name="tbl_Inv_PurchaseNoteDetail.FK_tbl_Inv_PurchaseOrderDetail_IDName"]').focus();
            });
        });

    })
    .controller("SalesNoteDetailReturnCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('SalesNoteDetailReturnCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_SalesNoteDetailReturnCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/Inventory/Challan/SalesNoteDetailReturnLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.tbl_Inv_SalesReturnNoteDetail = {
            'ID': 0, 'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '',
            'MeasurementUnit': '', 'ReferenceNo': '', 'Quantity': 0, 'Remarks': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_SalesReturnNoteDetails = [$scope.tbl_Inv_SalesReturnNoteDetail];

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    