MainModule
    .controller("OrderNoteMasterCtlr", function ($scope, $http) {
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
            '/Inventory/Orders/OrderNoteMasterLoad', //--v_Load
            '/Inventory/Orders/OrderNoteMasterGet', // getrow
            '/Inventory/Orders/OrderNoteMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/Orders/GetInitializedOrderNote');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'OrderNoteMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'OrderNoteMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'OrderNoteMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'OrderNoteMasterCtlr').LoadByCard);
                init_Report($scope, data.find(o => o.Controller === 'OrderNoteMasterCtlr').Reports, '/Inventory/Orders/GetOrderNoteReport'); 
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'OrderNoteDetailCtlr') != undefined) {
                $scope.$broadcast('init_OrderNoteDetailCtlr', data.find(o => o.Controller === 'OrderNoteDetailCtlr'));
            }
            if (data.find(o => o.Controller === 'OrderNoteDetailSubDistributorCtlr') != undefined) {
                $scope.$broadcast('init_OrderNoteDetailSubDistributorCtlr', data.find(o => o.Controller === 'OrderNoteDetailSubDistributorCtlr'));
            }
            if (data.find(o => o.Controller === 'OrderNoteDetailBMRDetailCtlr') != undefined) {
                $scope.$broadcast('init_OrderNoteDetailBMRDetailCtlr', data.find(o => o.Controller === 'OrderNoteDetailBMRDetailCtlr'));
            }
            if (data.find(o => o.Controller === 'OrderNoteDetailSalesNoteDetailCtlr') != undefined) {
                $scope.$broadcast('init_OrderNoteDetailSalesNoteDetailCtlr', data.find(o => o.Controller === 'OrderNoteDetailSalesNoteDetailCtlr'));
            }
            if (data.find(o => o.Controller === 'OrderNoteDetailSalesReturnNoteDetailCtlr') != undefined) {
                $scope.$broadcast('init_OrderNoteDetailSalesReturnNoteDetailCtlr', data.find(o => o.Controller === 'OrderNoteDetailSalesReturnNoteDetailCtlr'));
            }
        };

        init_ProductSearchModalGeneral($scope, $http);
        init_COASearchModalGeneral($scope, $http);
        init_SDSearchModalGeneral($scope, $http);

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_OrderNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                $scope.tbl_Inv_OrderNoteMaster.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;
            }
            else {
                $scope.tbl_Inv_OrderNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID = null;
                $scope.tbl_Inv_OrderNoteMaster.FK_tbl_Ac_ChartOfAccounts_IDName = null;
            }

        };
        

        $scope.tbl_Inv_OrderNoteMaster = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
            'TargetDate': new Date(), 'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_OrderNoteMasters = [$scope.tbl_Inv_OrderNoteMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_OrderNoteMaster = {
                'ID': 0, 'DocNo': '', 'DocDate': new Date(),
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
                'TargetDate': new Date(), 'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_OrderNoteMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Inv_OrderNoteMaster = data;
            $scope.tbl_Inv_OrderNoteMaster.DocDate = new Date(data.DocDate);
            $scope.tbl_Inv_OrderNoteMaster.TargetDate = new Date(data.TargetDate); 
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("OrderNoteDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('OrderNoteDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_OrderNoteDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, itm.LoadByCard); 
            init_Report($scope, itm.Reports, '/Inventory/Orders/GetOrderNoteReport'); 
            if (itm.Otherdata === null) {
                $scope.AspNetOreasPriorityList = [];
            }
            else {
                $scope.AspNetOreasPriorityList = itm.Otherdata.AspNetOreasPriorityList;
            }
        });

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_OrderNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Inv_OrderNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName + " [" + item.Split_Into + "'s]";
                $scope.tbl_Inv_OrderNoteDetail.MeasurementUnit = item.MeasurementUnit;

                $scope.tbl_Inv_OrderNoteDetail.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID = item.MasterProdID;
                $scope.tbl_Inv_OrderNoteDetail.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_IDName = item.OtherDetail;
            }
            else {
                $scope.tbl_Inv_OrderNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Inv_OrderNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Inv_OrderNoteDetail.MeasurementUnit = null;

                $scope.tbl_Inv_OrderNoteDetail.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID = null;
                $scope.tbl_Inv_OrderNoteDetail.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_IDName = null;      
            }

            if (item.IsDecimal) { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+(\.[0-9]{1,4})?$"); }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+$"); }          
        };

        init_Operations($scope, $http,
            '/Inventory/Orders/OrderNoteDetailLoad', //--v_Load
            '/Inventory/Orders/OrderNoteDetailGet', // getrow
            '/Inventory/Orders/OrderNoteDetailPost' // PostRow
        );

        $scope.tbl_Inv_OrderNoteDetail = {
            'ID': 0, 'FK_tbl_Inv_OrderNoteMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID': null, 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_IDName': '', 
            'Quantity': 0, 'FK_AspNetOreasPriority_ID': null, 'FK_AspNetOreasPriority_IDName': '',
            'Remarks': '', 'MfgQtyLimit': 0, 'ManufacturingQty': 0, 'SoldQty': 0, 'ClosedTrue_OpenFalse': false, 'Rate': 0, 'CustomMRP': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_OrderNoteDetails = [$scope.tbl_Inv_OrderNoteDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_OrderNoteDetail = {
                'ID': 0, 'FK_tbl_Inv_OrderNoteMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID': null, 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_IDName': '',
                'Quantity': 0, 'FK_AspNetOreasPriority_ID': null, 'FK_AspNetOreasPriority_IDName': '',
                'Remarks': '', 'MfgQtyLimit': 0, 'ManufacturingQty': 0, 'SoldQty': 0, 'ClosedTrue_OpenFalse': false, 'Rate': 0, 'CustomMRP': 0,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_OrderNoteDetail }; };

        $scope.GetRowResponse = function (data, operation) { $scope.tbl_Inv_OrderNoteDetail = data; };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };        

    })
    .controller("OrderNoteDetailSubDistributorCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('OrderNoteDetailSubDistributorCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_OrderNoteDetailSubDistributorCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, itm.LoadByCard);
        });

        init_Operations($scope, $http,
            '/Inventory/Orders/OrderNoteDetailSubDistributorLoad', //--v_Load
            '/Inventory/Orders/OrderNoteDetailSubDistributorGet', // getrow
            '/Inventory/Orders/OrderNoteDetailSubDistributorPost' // PostRow
        );

        $scope.SDSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_OrderNoteDetail_SubDistributor.FK_tbl_Ac_CustomerSubDistributorList_ID = item.ID;
                $scope.tbl_Inv_OrderNoteDetail_SubDistributor.FK_tbl_Ac_CustomerSubDistributorList_IDName = item.Name;
                $scope.tbl_Inv_OrderNoteDetail_SubDistributor.Address = item.Address;
            }
            else {
                $scope.tbl_Inv_OrderNoteDetail_SubDistributor.FK_tbl_Ac_CustomerSubDistributorList_ID = null;
                $scope.tbl_Inv_OrderNoteDetail_SubDistributor.FK_tbl_Ac_CustomerSubDistributorList_IDName = null;
                $scope.tbl_Inv_OrderNoteDetail_SubDistributor.Address = null;
            }
        };

        $scope.tbl_Inv_OrderNoteDetail_SubDistributor = {
            'ID': 0, 'FK_tbl_Inv_OrderNoteDetail_ID': $scope.MasterObject.ID,
            'FK_tbl_Ac_CustomerSubDistributorList_ID': null, 'FK_tbl_Ac_CustomerSubDistributorList_IDName': '', 'Address': '',
            'Quantity': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_OrderNoteDetail_SubDistributors = [$scope.tbl_Inv_OrderNoteDetail_SubDistributor];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_OrderNoteDetail_SubDistributor = {
                'ID': 0, 'FK_tbl_Inv_OrderNoteDetail_ID': $scope.MasterObject.ID,
                'FK_tbl_Ac_CustomerSubDistributorList_ID': null, 'FK_tbl_Ac_CustomerSubDistributorList_IDName': '', 'Address': '',
                'Quantity': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_OrderNoteDetail_SubDistributor }; };

        $scope.GetRowResponse = function (data, operation) { $scope.tbl_Inv_OrderNoteDetail_SubDistributor = data; };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };
    })
    .controller("OrderNoteDetailBMRDetailCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('OrderNoteDetailBMRDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');

        });

        $scope.$on('init_OrderNoteDetailBMRDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/Inventory/Orders/OrderNoteDetailBMRDetailLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster = {
            'FK_tbl_Pro_BatchMaterialRequisitionMaster_ID': 0, 'BatchNo': '', 'BatchMfgDate': new Date(), 'BatchSize': 0,
            'PrimaryProduct': '', 'PrimarySecondary': '', 'PackagingName': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters = [$scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster = {
                'FK_tbl_Pro_BatchMaterialRequisitionMaster_ID': 0, 'BatchNo': '', 'BatchMfgDate': new Date(), 'BatchSize': 0,
                'PrimaryProduct': '', 'PrimarySecondary': '', 'PackagingName': ''
            };
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("OrderNoteDetailSalesNoteDetailCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('OrderNoteDetailSalesNoteDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_OrderNoteDetailSalesNoteDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/Inventory/Orders/OrderNoteDetailSalesNoteDetailLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.tbl_Inv_SalesNoteDetails = {
            'DocNo': 0, 'DocDate': new Date(), 'FK_tbl_Inv_SalesNoteMaster_ID': null,
            'ProductName': '', 'MeasurementUnit': '', 'Quantity': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_SalesNoteDetails = [$scope.tbl_Inv_SalesNoteDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_SalesNoteDetails = {
                'DocNo': 0, 'DocDate': new Date(), 'FK_tbl_Inv_SalesNoteMaster_ID': null,
                'ProductName': '', 'MeasurementUnit': '', 'Quantity': 0
            };
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("OrderNoteDetailSalesReturnNoteDetailCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('OrderNoteDetailSalesReturnNoteDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_OrderNoteDetailSalesReturnNoteDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/Inventory/Orders/OrderNoteDetailSalesReturnNoteDetailLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.tbl_Inv_SalesReturnNoteDetail = {
            'DocNo': 0, 'DocDate': new Date(), 'FK_tbl_Inv_SalesReturnNoteMaster_ID': null,
            'ProductName': '', 'MeasurementUnit': '', 'Quantity': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_SalesReturnNoteDetails = [$scope.tbl_Inv_SalesReturnNoteDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_SalesReturnNoteDetail = {
                'DocNo': 0, 'DocDate': new Date(), 'FK_tbl_Inv_SalesReturnNoteMaster_ID': null,
                'ProductName': '', 'MeasurementUnit': '', 'Quantity': 0
            };
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    