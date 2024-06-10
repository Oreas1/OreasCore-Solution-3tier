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
            '/Accounts/Invoice/SalesNoteMasterLoad', //--v_Load
            '/Accounts/Invoice/SalesNoteMasterGet', // getrow
            '/Accounts/Invoice/SalesNoteMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/Invoice/GetInitializedSalesNote');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'SalesNoteMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'SalesNoteMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'SalesNoteMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'SalesNoteMasterCtlr').LoadByCard);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'SalesNoteDetailCtlr') != undefined) {
                $scope.$broadcast('init_SalesNoteDetailCtlr', data.find(o => o.Controller === 'SalesNoteDetailCtlr'));
            }
            if (data.find(o => o.Controller === 'SalesNoteDetailReturnCtlr') != undefined) {
                $scope.$broadcast('init_SalesNoteDetailReturnCtlr', data.find(o => o.Controller === 'SalesNoteDetailReturnCtlr'));
            }
        };        

        $scope.tbl_Inv_SalesNoteMaster = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
            'FK_tbl_Ac_CustomerSubDistributorList_ID': null, 'FK_tbl_Ac_CustomerSubDistributorList_IDName': '',
            'Remarks': '', 'TotalNetAmount': 0,
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
                'Remarks': '', 'TotalNetAmount': 0,
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
            init_Report($scope, itm.Reports, '/Accounts/Invoice/GetSalesNoteReport'); 
        });

        init_Operations($scope, $http,
            '/Accounts/Invoice/SalesNoteDetailLoad', //--v_Load
            '/Accounts/Invoice/SalesNoteDetailGet', // getrow
            '/Accounts/Invoice/SalesNoteDetailPost' // PostRow
        );

        $scope.tbl_Inv_SalesNoteDetail = {
            'ID': 0, 'FK_tbl_Inv_SalesNoteMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null, 'ReferenceNo': '',
            'Quantity': 0, 'Rate': 0, 'GrossAmount': 0, 'STPercentage': 0, 'STAmount': 0, 'FSTPercentage': 0, 'FSTAmount': 0, 'WHTPercentage': 0, 'WHTAmount': 0,
            'DiscountAmount': 0, 'NetAmount': 0, 'Remarks': '', 'NoOfPackages': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'FK_tbl_Inv_OrderNoteDetail_ID': null
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_SalesNoteDetails = [$scope.tbl_Inv_SalesNoteDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_SalesNoteDetail = {
                'ID': 0, 'FK_tbl_Inv_SalesNoteMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null, 'ReferenceNo': '',
                'Quantity': 0, 'Rate': 0, 'GrossAmount': 0, 'STPercentage': 0, 'STAmount': 0, 'FSTPercentage': 0, 'FSTAmount': 0, 'WHTPercentage': 0, 'WHTAmount': 0,
                'DiscountAmount': 0, 'NetAmount': 0, 'Remarks': '', 'NoOfPackages': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'FK_tbl_Inv_OrderNoteDetail_ID': null
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_SalesNoteDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_SalesNoteDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };        

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
            '/Accounts/Invoice/SalesNoteDetailReturnLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.tbl_Inv_SalesReturnNoteDetail = {
            'ID': 0, 'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '',
            'MeasurementUnit': '', 'ReferenceNo': '', 'Quantity': 0, 'Rate': 0, 'DiscountAmount': 0,
            'STPercentage': 0, 'FSTPercentage': 0, 'NetAmount': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_SalesReturnNoteDetails = [$scope.tbl_Inv_SalesReturnNoteDetail];

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    