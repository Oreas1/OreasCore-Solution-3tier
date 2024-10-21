MainModule
    .controller("PurchaseOrderMasterCtlr", function ($scope, $http) {
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
            '/Inventory/Orders/PurchaseOrderMasterLoad', //--v_Load
            '/Inventory/Orders/PurchaseOrderMasterGet', // getrow
            '/Inventory/Orders/PurchaseOrderMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/Orders/GetInitializedPurchaseOrder');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'PurchaseOrderMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'PurchaseOrderMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'PurchaseOrderMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'PurchaseOrderMasterCtlr').LoadByCard);

                if (data.find(o => o.Controller === 'PurchaseOrderMasterCtlr').Otherdata === null) {
                    $scope.POTermsConditionsList = [];
                }
                else {
                    $scope.POTermsConditionsList = data.find(o => o.Controller === 'PurchaseOrderMasterCtlr').Otherdata.POTermsConditionsList;
                }

                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'PurchaseOrderDetailCtlr') != undefined) {
                $scope.$broadcast('init_PurchaseOrderDetailCtlr', data.find(o => o.Controller === 'PurchaseOrderDetailCtlr'));
            }
            if (data.find(o => o.Controller === 'PurchaseOrderDetailPNCtlr') != undefined) {
                $scope.$broadcast('init_PurchaseOrderDetailPNCtlr', data.find(o => o.Controller === 'PurchaseOrderDetailPNCtlr'));
            }
            if (data.find(o => o.Controller === 'PurchaseOrderDetailPRNCtlr') != undefined) {
                $scope.$broadcast('init_PurchaseOrderDetailPRNCtlr', data.find(o => o.Controller === 'PurchaseOrderDetailPRNCtlr'));
            }
        };

        init_ProductSearchModalGeneral($scope, $http);
        init_COASearchModalGeneral($scope, $http);

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_PurchaseOrderMaster.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                $scope.tbl_Inv_PurchaseOrderMaster.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;
            }
            else {
                $scope.tbl_Inv_PurchaseOrderMaster.FK_tbl_Ac_ChartOfAccounts_ID = null;
                $scope.tbl_Inv_PurchaseOrderMaster.FK_tbl_Ac_ChartOfAccounts_IDName = null;
            }

        };
        

        $scope.tbl_Inv_PurchaseOrderMaster = {
            'ID': 0, 'PONo': '', 'PODate': new Date(),
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
            'TargetDate': new Date(), 'Remarks': '', 'FK_tbl_Inv_PurchaseOrderTermsConditions_ID': null,
            'FK_tbl_Inv_PurchaseOrderTermsConditions_IDName': '', 'LocalTrue_ImportFalse': true, 'IsSupervisedAll': true,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseOrderMasters = [$scope.tbl_Inv_PurchaseOrderMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_PurchaseOrderMaster = {
                'ID': 0, 'PONo': '', 'PODate': new Date(),
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
                'TargetDate': new Date(), 'Remarks': '', 'FK_tbl_Inv_PurchaseOrderTermsConditions_ID': null,
                'FK_tbl_Inv_PurchaseOrderTermsConditions_IDName': '', 'LocalTrue_ImportFalse': true, 'IsSupervisedAll': true,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseOrderMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Inv_PurchaseOrderMaster = data;
            $scope.tbl_Inv_PurchaseOrderMaster.PODate = new Date(data.PODate);
            $scope.tbl_Inv_PurchaseOrderMaster.TargetDate = new Date(data.TargetDate); 
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID, IsCanViewOnlyOwnData: $scope.Privilege.CanViewOnlyOwnData }; };

        $scope.EmailPO = function (id, ContactPersonName, Email, AcName, TargetDate, PONo) {

            if (confirm("Are you sure! you want to Email Purchase Order ") === true) {
                var successcallback = function (response) {
                    if (response.data === 'OK') { alert('Sucessfully Sent'); }
                    else
                        alert(response.data);
                };
                var errorcallback = function (error) { alert(error); };

                $http({ method: "GET", url: "/Inventory/Orders/EmailPO", params: { ID: id, ContactPersonName: ContactPersonName, Email: Email, AcName: AcName, TargetDate: TargetDate, PONo: PONo }, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
            }
        };
       
    })
    .controller("PurchaseOrderDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('PurchaseOrderDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_PurchaseOrderDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, itm.LoadByCard); 
            init_Report($scope, itm.Reports, '/Inventory/Orders/GetPurchaseOrderReport');
            if (itm.Otherdata === null) {
                $scope.AspNetOreasPriorityList = [];
                $scope.ManufacturerPOList = [];
            }
            else {
                $scope.AspNetOreasPriorityList = itm.Otherdata.AspNetOreasPriorityList;
                $scope.ManufacturerPOList = itm.Otherdata.ManufacturerPOList;
            }
        });

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_PurchaseOrderDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Inv_PurchaseOrderDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Inv_PurchaseOrderDetail.MeasurementUnit = item.MeasurementUnit;                
            }
            else {

                $scope.tbl_Inv_PurchaseOrderDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Inv_PurchaseOrderDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Inv_PurchaseOrderDetail.MeasurementUnit = null;
            }

            if (item.IsDecimal) {
                $scope.wholeNumberOrNot = new RegExp("^(0\\.[0]*[1-9][0-9]{0,4}|[1-9][0-9]*(\\.[0-9]{1,5})?)$");
            }
            else {
                $scope.wholeNumberOrNot = new RegExp("^[1-9][0-9]*$");
            }
            
        };

        init_Operations($scope, $http,
            '/Inventory/Orders/PurchaseOrderDetailLoad', //--v_Load
            '/Inventory/Orders/PurchaseOrderDetailGet', // getrow
            '/Inventory/Orders/PurchaseOrderDetailPost' // PostRow
        );

        $scope.tbl_Inv_PurchaseOrderDetail = {
            'ID': 0, 'FK_tbl_Inv_PurchaseOrderMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'FK_AspNetOreasPriority_ID': null, 'FK_AspNetOreasPriority_IDName': '', 'Quantity': 0, 'Rate': 0,
            'GSTPercentage': 0, 'DiscountAmount': 0, 'WHTPercentage': 0, 'NetAmount': 0, 'Remarks': '',
            'FK_tbl_Inv_PurchaseOrder_Manufacturer_ID': null, 'FK_tbl_Inv_PurchaseOrder_Manufacturer_IDName': '', 'ReceivedQty': 0, 'ClosedTrue_OpenFalse': false, 
            'Performance_Time': false, 'Performance_Quantity': false, 'Performance_Quality': false, 'FK_tbl_Inv_PurchaseRequestDetail_ID': null, 'IsSupervised': true,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseOrderDetails = [$scope.tbl_Inv_PurchaseOrderDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_PurchaseOrderDetail = {
                'ID': 0, 'FK_tbl_Inv_PurchaseOrderMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'FK_AspNetOreasPriority_ID': null, 'FK_AspNetOreasPriority_IDName': '', 'Quantity': 0, 'Rate': 0,
                'GSTPercentage': 0, 'DiscountAmount': 0, 'WHTPercentage': 0, 'NetAmount': 0, 'Remarks': '',
                'FK_tbl_Inv_PurchaseOrder_Manufacturer_ID': null, 'FK_tbl_Inv_PurchaseOrder_Manufacturer_IDName': '', 'ReceivedQty': 0, 'ClosedTrue_OpenFalse': false,
                'Performance_Time': false, 'Performance_Quantity': false, 'Performance_Quality': false, 'FK_tbl_Inv_PurchaseRequestDetail_ID': null, 'IsSupervised': true,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };

        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseOrderDetail }; };

        $scope.GetRowResponse = function (data, operation) { $scope.tbl_Inv_PurchaseOrderDetail = data; };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };        

    })
    .controller("PurchaseOrderDetailPNCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('PurchaseOrderDetailPNCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_PurchaseOrderDetailPNCtlr', function (e, itm) {
            init_Filter($scope, null, null, null, null);
        });


        init_Operations($scope, $http,
            '/Inventory/Orders/PurchaseOrderDetailPNLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.tbl_Inv_PurchaseNoteDetail = {
            'ID': 0, 'FK_tbl_Inv_PurchaseNoteMaster_ID': null,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'Quantity': 0, 'ReferenceNo': '', 'DocNo': '', 'DocDate': null, 'Action':''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseNoteDetails = [$scope.tbl_Inv_PurchaseNoteDetail];

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("PurchaseOrderDetailPRNCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('PurchaseOrderDetailPRNCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_PurchaseOrderDetailPRNCtlr', function (e, itm) {
            init_Filter($scope, null, null, null, null);
        });


        init_Operations($scope, $http,
            '/Inventory/Orders/PurchaseOrderDetailPRNLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.tbl_Inv_PurchaseReturnNoteDetail = {
            'ID': 0, 'FK_tbl_Inv_PurchaseReturnNoteMaster_ID': null,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'Quantity': 0, 'DocNo': '', 'DocDate': null
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseReturnNoteDetails = [$scope.tbl_Inv_PurchaseReturnNoteDetail];

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    