MainModule
    .controller("PurchaseNoteMasterCtlr", function ($scope, $http) {
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
            '/Accounts/Invoice/PurchaseNoteMasterLoad', //--v_Load
            '/Accounts/Invoice/PurchaseNoteMasterGet', // getrow
            '/Accounts/Invoice/PurchaseNoteMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/Invoice/GetInitializedPurchaseNote');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'PurchaseNoteMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'PurchaseNoteMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'PurchaseNoteMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'PurchaseNoteMasterCtlr').LoadByCard);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'PurchaseNoteDetailCtlr') != undefined) {
                $scope.$broadcast('init_PurchaseNoteDetailCtlr', data.find(o => o.Controller === 'PurchaseNoteDetailCtlr'));
            }
            if (data.find(o => o.Controller === 'PurchaseNoteDetailOfDetailCtlr') != undefined) {
                $scope.$broadcast('init_PurchaseNoteDetailOfDetailCtlr', data.find(o => o.Controller === 'PurchaseNoteDetailOfDetailCtlr'));
            }
        };        

        $scope.tbl_Inv_PurchaseNoteMaster = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
            'SupplierChallanNo': '', 'SupplierInvoiceNo': '', 'Remarks': '', 'TotalNetAmount': 0,
            'IsProcessedAll': false, 'IsSupervisedAll': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseNoteMasters = [$scope.tbl_Inv_PurchaseNoteMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_PurchaseNoteMaster = {
                'ID': 0, 'DocNo': '', 'DocDate': new Date(),
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
                'SupplierChallanNo': '', 'SupplierInvoiceNo': '', 'Remarks': '', 'TotalNetAmount': 0,
                'IsProcessedAll': false, 'IsSupervisedAll': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseNoteMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Inv_PurchaseNoteMaster = data; $scope.tbl_Inv_PurchaseNoteMaster.DocDate = new Date(data.DocDate);  
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("PurchaseNoteDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('PurchaseNoteDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first'); 
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_PurchaseNoteDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
            init_Report($scope, itm.Reports, '/Accounts/Invoice/GetPurchaseNoteReport'); 
        });

        init_Operations($scope, $http,
            '/Accounts/Invoice/PurchaseNoteDetailLoad', //--v_Load
            '/Accounts/Invoice/PurchaseNoteDetailGet', // getrow
            '/Accounts/Invoice/PurchaseNoteDetailPost' // PostRow
        );

        $scope.tbl_Inv_PurchaseNoteDetail = {
            'ID': 0, 'FK_tbl_Inv_PurchaseNoteMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'Rate': 0, 'GrossAmount': 0, 'GSTPercentage': 0, 'GSTAmount': 0, 'FreightIn': 0, 'DiscountAmount': 0, 'CostAmount': 0,
            'WHTPercentage': 0, 'WHTAmount': 0, 'NetAmount': 0, 'MfgBatchNo': '', 'ExpiryDate': null, 'Remarks': '', 'ReferenceNo': '',
            'FK_tbl_Inv_PurchaseOrderDetail_ID': null, 'IsProcessed': false, 'IsSupervised': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
            'FK_tbl_Qc_ActionType_ID': 0, 'FK_tbl_Qc_ActionType_IDName': '', 'QuantitySample': 0,
            'CreatedByQcQa': '', 'CreatedDateQcQa': '', 'ModifiedByQcQa': '', 'ModifiedDateQcQa': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseNoteDetails = [$scope.tbl_Inv_PurchaseNoteDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_PurchaseNoteDetail = {
                'ID': 0, 'FK_tbl_Inv_PurchaseNoteMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'Quantity': 0, 'Rate': 0, 'GrossAmount': 0, 'GSTPercentage': 0, 'GSTAmount': 0, 'FreightIn': 0, 'DiscountAmount': 0, 'CostAmount': 0,
                'WHTPercentage': 0, 'WHTAmount': 0, 'NetAmount': 0, 'MfgBatchNo': '', 'ExpiryDate': null, 'Remarks': '', 'ReferenceNo': '',
                'FK_tbl_Inv_PurchaseOrderDetail_ID': null, 'IsProcessed': false, 'IsSupervised': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
                'FK_tbl_Qc_ActionType_ID': 0, 'FK_tbl_Qc_ActionType_IDName': '', 'QuantitySample': 0,
                'CreatedByQcQa': '', 'CreatedDateQcQa': '', 'ModifiedByQcQa': '', 'ModifiedDateQcQa': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseNoteDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_PurchaseNoteDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; }; 

    })
    .controller("PurchaseNoteDetailOfDetailCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('PurchaseNoteDetailOfDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_PurchaseNoteDetailOfDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/Accounts/Invoice/PurchaseNoteDetailOfDetailLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.tbl_Inv_Ledger = {
            'ID': 0, 'PostingDate': new Date(), 'Narration': '', 'QuantityIn': 0, 'QuantityOut': 0, 'Ref': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_Ledgers = [$scope.tbl_Inv_Ledger];

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    