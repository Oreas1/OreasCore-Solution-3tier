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
            '/Accounts/Invoice/PurchaseReturnNoteMasterLoad', //--v_Load
            '/Accounts/Invoice/PurchaseReturnNoteMasterGet', // getrow
            '/Accounts/Invoice/PurchaseReturnNoteMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/Invoice/GetInitializedPurchaseReturnNote');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'PurchaseReturnNoteMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'PurchaseReturnNoteMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'PurchaseReturnNoteMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'PurchaseReturnNoteMasterCtlr').LoadByCard);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'PurchaseReturnNoteDetailCtlr') != undefined) {
                $scope.$broadcast('init_PurchaseReturnNoteDetailCtlr', data.find(o => o.Controller === 'PurchaseReturnNoteDetailCtlr'));
            }
        };        

        $scope.tbl_Inv_PurchaseReturnNoteMaster = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
            'Remarks': '', 'TotalNetAmount': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseReturnNoteMasters = [$scope.tbl_Inv_PurchaseReturnNoteMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_PurchaseReturnNoteMaster = {
                'ID': 0, 'DocNo': '', 'DocDate': new Date(),
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
                'Remarks': '', 'TotalNetAmount': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
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
            init_Report($scope, itm.Reports, '/Accounts/Invoice/GetPurchaseReturnNoteReport'); 
        });

        init_Operations($scope, $http,
            '/Accounts/Invoice/PurchaseReturnNoteDetailLoad', //--v_Load
            '/Accounts/Invoice/PurchaseReturnNoteDetailGet', // getrow
            '/Accounts/Invoice/PurchaseReturnNoteDetailPost' // PostRow
        );

        $scope.tbl_Inv_PurchaseReturnNoteDetail = {
            'ID': 0, 'FK_tbl_Inv_PurchaseReturnNoteMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'ReferenceNo': '',
            'Quantity': 0, 'Rate': 0, 'GrossAmount': 0, 'GSTPercentage': 0, 'GSTAmount': 0, 'FreightIn': 0, 'DiscountAmount': 0, 'CostAmount': 0,
            'WHTPercentage': 0, 'WHTAmount': 0, 'NetAmount': 0, 'MfgBatchNo': '', 'ExpiryDate': null, 'Remarks': '', 'ReferenceNo': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseReturnNoteDetails = [$scope.tbl_Inv_PurchaseReturnNoteDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_PurchaseReturnNoteDetail = {
                'ID': 0, 'FK_tbl_Inv_PurchaseReturnNoteMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'ReferenceNo': '',
                'Quantity': 0, 'Rate': 0, 'GrossAmount': 0, 'GSTPercentage': 0, 'GSTAmount': 0, 'FreightIn': 0, 'DiscountAmount': 0, 'CostAmount': 0,
                'WHTPercentage': 0, 'WHTAmount': 0, 'NetAmount': 0, 'MfgBatchNo': '', 'ExpiryDate': null, 'Remarks': '', 'ReferenceNo': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseReturnNoteDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_PurchaseReturnNoteDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };        

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    