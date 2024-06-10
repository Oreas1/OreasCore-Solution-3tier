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
            '/Accounts/Invoice/SalesReturnNoteMasterLoad', //--v_Load
            '/Accounts/Invoice/SalesReturnNoteMasterGet', // getrow
            '/Accounts/Invoice/SalesReturnNoteMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/Invoice/GetInitializedSalesReturnNote');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'SalesReturnNoteMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'SalesReturnNoteMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'SalesReturnNoteMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'SalesReturnNoteMasterCtlr').LoadByCard);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'SalesReturnNoteDetailCtlr') != undefined) {
                $scope.$broadcast('init_SalesReturnNoteDetailCtlr', data.find(o => o.Controller === 'SalesReturnNoteDetailCtlr'));
            }
        };        

        $scope.tbl_Inv_SalesReturnNoteMaster = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
            'FK_tbl_Ac_CustomerSubDistributorList_ID': null, 'FK_tbl_Ac_CustomerSubDistributorList_IDName': '',
            'Remarks': '', 'TotalNetAmount': 0,
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
                'FK_tbl_Ac_CustomerSubDistributorList_ID': null, 'FK_tbl_Ac_CustomerSubDistributorList_IDName': '',
                'Remarks': '', 'TotalNetAmount': 0,
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
            init_Report($scope, itm.Reports, '/Accounts/Invoice/GetSalesReturnNoteReport'); 
        });

        init_Operations($scope, $http,
            '/Accounts/Invoice/SalesReturnNoteDetailLoad', //--v_Load
            '/Accounts/Invoice/SalesReturnNoteDetailGet', // getrow
            '/Accounts/Invoice/SalesReturnNoteDetailPost' // PostRow
        );

        $scope.tbl_Inv_SalesReturnNoteDetail = {
            'ID': 0, 'FK_tbl_Inv_SalesReturnNoteMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_SalesNoteDetail_ID': null, 'FK_tbl_Inv_SalesNoteDetail_IDName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null, 'ReferenceNo': '',
            'Quantity': 0, 'Rate': 0, 'GrossAmount': 0, 'STPercentage': 0, 'STAmount': 0, 'FSTPercentage': 0, 'FSTAmount': 0, 'WHTPercentage': 0, 'WHTAmount': 0,
            'DiscountAmount': 0, 'NetAmount': 0, 'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_SalesReturnNoteDetails = [$scope.tbl_Inv_SalesReturnNoteDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_SalesReturnNoteDetail = {
                'ID': 0, 'FK_tbl_Inv_SalesReturnNoteMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_SalesNoteDetail_ID': null, 'FK_tbl_Inv_SalesNoteDetail_IDName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null, 'ReferenceNo': '',
                'Quantity': 0, 'Rate': 0, 'GrossAmount': 0, 'STPercentage': 0, 'STAmount': 0, 'FSTPercentage': 0, 'FSTAmount': 0, 'WHTPercentage': 0, 'WHTAmount': 0,
                'DiscountAmount': 0, 'NetAmount': 0, 'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_SalesReturnNoteDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_SalesReturnNoteDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };        

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    