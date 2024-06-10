MainModule
    .controller("PurchaseRequestBidsMasterCtlr", function ($scope, $http) {
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
            '/Inventory/Orders/PurchaseRequestBidsMasterLoad', //--v_Load
            '/Inventory/Orders/PurchaseRequestBidsMasterGet', // getrow
            '' // PostRow
        );
      

        init_ViewSetup($scope, $http, '/Inventory/Orders/GetInitializedPurchaseRequestBids');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'PurchaseRequestBidsMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'PurchaseRequestBidsMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'PurchaseRequestBidsMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'PurchaseRequestBidsMasterCtlr').LoadByCard);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'PurchaseRequestBidsDetailCtlr') != undefined) {
                $scope.$broadcast('init_PurchaseRequestBidsDetailCtlr', data.find(o => o.Controller === 'PurchaseRequestBidsDetailCtlr'));
            }
        };

        init_COASearchModalGeneral($scope, $http);



        $scope.tbl_Inv_PurchaseRequestDetail = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
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
                'ID': 0, 'DocNo': '', 'DocDate': new Date(),
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'FK_AspNetOreasPriority_ID': null, 'FK_AspNetOreasPriority_IDName': '', 'Quantity': 0,
                'Remarks': '', 'IsApproved': false, 'IsRjected': false, 'IsPending': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Inv_PurchaseRequestDetail = data;
            $scope.tbl_Inv_PurchaseRequestDetail.DocDate = new Date(data.DocDate); 
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("PurchaseRequestBidsDetailCtlr", function ($scope, $http) {        
        $scope.MasterObject = {};
        $scope.$on('PurchaseRequestBidsDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.GetLastPO(itm.FK_tbl_Inv_ProductRegistrationDetail_ID); 
            $scope.pageNavigation('first');
        });

        $scope.$on('init_PurchaseRequestBidsDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, itm.LoadByCard);   
            if (itm.Otherdata === null) {
                $scope.AspNetOreasPriorityList = [];
                $scope.ManufacturerPOList = [];
                $scope.IsPurchaseRequestApprover = false;
            }
            else {
                $scope.AspNetOreasPriorityList = itm.Otherdata.AspNetOreasPriorityList;
                $scope.ManufacturerPOList = itm.Otherdata.ManufacturerPOList;
                $scope.IsPurchaseRequestApprover = itm.Otherdata.IsPurchaseRequestApprover;
            }
        });


        init_Operations($scope, $http,
            '/Inventory/Orders/PurchaseRequestBidsDetailLoad', //--v_Load
            '/Inventory/Orders/PurchaseRequestBidsDetailGet', // getrow
            '/Inventory/Orders/PurchaseRequestBidsDetailPost' // PostRow
        );

        $scope.tbl_Inv_PurchaseRequestDetail_Bids = {
            'ID': 0, 'FK_tbl_Inv_PurchaseRequestDetail_ID': $scope.MasterObject.ID,
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'Quantity': $scope.MasterObject.Quantity,
            'Rate': 0, 'GSTPercentage': 0, 'TargetDate': new Date(), 'FK_tbl_Inv_PurchaseOrder_Manufacturer_ID': null, 'FK_tbl_Inv_PurchaseOrder_Manufacturer_IDName':'',
            'Remarks': '', 'Approved': null,'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };


        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {

            if (item.ID > 0) {
                $scope.tbl_Inv_PurchaseRequestDetail_Bids.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                $scope.tbl_Inv_PurchaseRequestDetail_Bids.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;
              }
            else {
                $scope.tbl_Inv_PurchaseRequestDetail_Bids.FK_tbl_Ac_ChartOfAccounts_ID = null;
                $scope.tbl_Inv_PurchaseRequestDetail_Bids.FK_tbl_Ac_ChartOfAccounts_IDName = null;
            }

        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseRequestDetail_Bidss = [$scope.tbl_Inv_PurchaseRequestDetail_Bids];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_PurchaseRequestDetail_Bids = {
                'ID': 0, 'FK_tbl_Inv_PurchaseRequestDetail_ID': $scope.MasterObject.ID,
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'Quantity': $scope.MasterObject.Quantity,
                'Rate': 0, 'GSTPercentage': 0, 'TargetDate': new Date(), 'FK_tbl_Inv_PurchaseOrder_Manufacturer_ID': null, 'FK_tbl_Inv_PurchaseOrder_Manufacturer_IDName': '',
                'Remarks': '', 'Approved': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };          
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseRequestDetail_Bids }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_PurchaseRequestDetail_Bids = data;
            $scope.tbl_Inv_PurchaseRequestDetail_Bids.TargetDate = new Date(data.TargetDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };  

        $scope.GetLastPO = function (id) {
            var successcallback = function (response) {
                $scope.LastPO = response.data;
            };
            var errorcallback = function (error) { console.log('GetLastPO', error); };

            $http({ method: "GET", url: '/Inventory/Orders/GetPOSuggestions', async: true, params: { ProdID: id }, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
        };

        $scope.PostDecision = function (id, decision) {
        
            $scope.tbl_Inv_PurchaseRequestDetail_Bids = $scope.pageddata.Data.find(a => a.ID === id);

            //---------i am setting up the object in proper way to receive at server, 1. setting date format which was in string 
            $scope.tbl_Inv_PurchaseRequestDetail_Bids.TargetDate = new Date($scope.tbl_Inv_PurchaseRequestDetail_Bids.TargetDate);
            $scope.tbl_Inv_PurchaseRequestDetail_Bids.Approved = decision;
      

            if ($scope.tbl_Inv_PurchaseRequestDetail_Bids != null) {

                    var successcallback = function (response) {
                    if (response.data === 'OK') {
                        $scope.pageNavigation('Load');
                    }
                    else {
                        alert(response.data);
                    }
                    };
                    var errorcallback = function (error) { console.log('Decision error', error); };
                    $http({
                        method: "POST", url: '/Inventory/Orders/PurchaseRequestBidsDetailPostDecision', async: true, params: { ID: id, Decision: decision, operation: 'Save Update' }, data: $scope.tbl_Inv_PurchaseRequestDetail_Bids, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
                    }).then(successcallback, errorcallback);
            }
            else
            {
                alert('Record Not Found');
            }

        
        };


    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    