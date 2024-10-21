MainModule
    .controller("PurchaseRequestMasterCtlr", function ($scope, $http) {
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
            '/Inventory/Orders/PurchaseRequestMasterLoad', //--v_Load
            '/Inventory/Orders/PurchaseRequestMasterGet', // getrow
            '/Inventory/Orders/PurchaseRequestMasterPost' // PostRow
        );
      

        init_ViewSetup($scope, $http, '/Inventory/Orders/GetInitializedPurchaseRequest');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'PurchaseRequestMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'PurchaseRequestMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'PurchaseRequestMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'PurchaseRequestMasterCtlr').LoadByCard);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'PurchaseRequestDetailCtlr') != undefined) {
                $scope.$broadcast('init_PurchaseRequestDetailCtlr', data.find(o => o.Controller === 'PurchaseRequestDetailCtlr'));
            }
            if (data.find(o => o.Controller === 'PurchaseRequestDetailPOCtlr') != undefined) {
                $scope.$broadcast('init_PurchaseRequestDetailPOCtlr', data.find(o => o.Controller === 'PurchaseRequestDetailPOCtlr'));

                if (data.find(o => o.Controller === 'PurchaseRequestDetailPOCtlr').Privilege != null)
                    $scope.PrivilegePO = data.find(o => o.Controller === 'PurchaseRequestDetailPOCtlr').Privilege;

              
            }
        };

        init_ProductSearchModalGeneral($scope, $http);
        init_WHMSearchModalGeneral($scope, $http);

        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_PurchaseRequestMaster.FK_tbl_Inv_WareHouseMaster_ID = item.ID;
                $scope.tbl_Inv_PurchaseRequestMaster.FK_tbl_Inv_WareHouseMaster_IDName = item.WareHouseName;
            }
            else {
                $scope.tbl_Inv_PurchaseRequestMaster.FK_tbl_Inv_WareHouseMaster_ID = null;
                $scope.tbl_Inv_PurchaseRequestMaster.FK_tbl_Inv_WareHouseMaster_IDName = null;
            }
        };

        $scope.tbl_Inv_PurchaseRequestMaster = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDname': '', 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseRequestMasters = [$scope.tbl_Inv_PurchaseRequestMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_PurchaseRequestMaster = {
                'ID': 0, 'DocNo': '', 'DocDate': new Date(),
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDname': '', 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseRequestMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Inv_PurchaseRequestMaster = data;
            $scope.tbl_Inv_PurchaseRequestMaster.DocDate = new Date(data.DocDate); 
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("PurchaseRequestDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('PurchaseRequestDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_PurchaseRequestDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, itm.LoadByCard);   
            init_Report($scope, itm.Reports, '/Inventory/Orders/GetPurchaseRequestReport');
            if (itm.Otherdata === null) {
                $scope.AspNetOreasPriorityList = [];
            }
            else {
                $scope.AspNetOreasPriorityList = itm.Otherdata.AspNetOreasPriorityList;
            }
        });

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_PurchaseRequestDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Inv_PurchaseRequestDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Inv_PurchaseRequestDetail.MeasurementUnit = item.MeasurementUnit;                
            }
            else {

                $scope.tbl_Inv_PurchaseRequestDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Inv_PurchaseRequestDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Inv_PurchaseRequestDetail.MeasurementUnit = null;
            }

            if (item.IsDecimal) {
                $scope.wholeNumberOrNot = new RegExp("^(0\\.[0]*[1-9][0-9]{0,4}|[1-9][0-9]*(\\.[0-9]{1,5})?)$");
            }
            else {
                $scope.wholeNumberOrNot = new RegExp("^[1-9][0-9]*$");
            }
            
        };

        init_Operations($scope, $http,
            '/Inventory/Orders/PurchaseRequestDetailLoad', //--v_Load
            '/Inventory/Orders/PurchaseRequestDetailGet', // getrow
            '/Inventory/Orders/PurchaseRequestDetailPost' // PostRow
        );

        $scope.tbl_Inv_PurchaseRequestDetail = {
            'ID': 0, 'FK_tbl_Inv_PurchaseRequestMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'FK_AspNetOreasPriority_ID': null, 'FK_AspNetOreasPriority_IDName': '', 'Quantity': 0,
            'Remarks': '', 'IsApproved': false, 'IsRjected': false, 'IsPending': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseRequestDetails = [$scope.tbl_Inv_PurchaseRequestDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_PurchaseRequestDetail = {
                'ID': 0, 'FK_tbl_Inv_PurchaseRequestMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'FK_AspNetOreasPriority_ID': null, 'FK_AspNetOreasPriority_IDName': '', 'Quantity': 0,
                'Remarks': '', 'IsApproved': false, 'IsRjected': false, 'IsPending': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseRequestDetail }; };

        $scope.GetRowResponse = function (data, operation) { $scope.tbl_Inv_PurchaseRequestDetail = data; };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };    

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    