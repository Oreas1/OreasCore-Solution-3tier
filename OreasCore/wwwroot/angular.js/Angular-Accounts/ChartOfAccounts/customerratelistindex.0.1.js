MainModule
    .controller("CustomerRateListCtlr", function ($scope, $http) {
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
            '/Accounts/ChartOfAccounts/CustomerRateListLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/ChartOfAccounts/GetInitializedCustomerRateList');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'CustomerRateListCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'CustomerRateListCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'CustomerRateListCtlr').WildCard, null, null, null);                    
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'CustomerRateListDetailCtlr') != undefined) {
                $scope.$broadcast('init_CustomerRateListDetailCtlr', data.find(o => o.Controller === 'CustomerRateListDetailCtlr'));
            }
        };

        init_ProductSearchModalGeneral($scope, $http);

        $scope.tbl_Ac_ChartOfAccounts = {
            'ID': 0, 'AccountName': '', 'AccountCode': '', 'ParentAccountName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_ChartOfAccountss = [$scope.tbl_Ac_ChartOfAccounts];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_ChartOfAccounts = {
                'ID': 0, 'AccountName': '', 'AccountCode': '', 'ParentAccountName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_ChartOfAccounts };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Ac_ChartOfAccounts = data; 
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("CustomerRateListDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('CustomerRateListDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_CustomerRateListDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
        });

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Ac_CustomerApprovedRateList.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Ac_CustomerApprovedRateList.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName + " [" + item.Split_Into + "'s]";
                $scope.tbl_Ac_CustomerApprovedRateList.MeasurementUnit = item.MeasurementUnit;
            }
            else {
                $scope.tbl_Ac_CustomerApprovedRateList.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Ac_CustomerApprovedRateList.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Ac_CustomerApprovedRateList.MeasurementUnit = null;
            }
        };

        init_Operations($scope, $http,
            '/Accounts/ChartOfAccounts/CustomerRateListDetailLoad', //--v_Load
            '/Accounts/ChartOfAccounts/CustomerRateListDetailGet', // getrow
            '/Accounts/ChartOfAccounts/CustomerRateListDetailPost' // PostRow
        );

        $scope.tbl_Ac_CustomerApprovedRateList = {
            'ID': 0, 'FK_tbl_Ac_ChartOfAccounts_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Rate': 0, 'AppliedDate': new Date(), 'PreviousRate': 0, 'PreviousAppliedDate': new Date(), 'Remarks':'',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_CustomerApprovedRateLists = [$scope.tbl_Ac_CustomerApprovedRateList];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_CustomerApprovedRateList = {
                'ID': 0, 'FK_tbl_Ac_ChartOfAccounts_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'Rate': 0, 'AppliedDate': new Date(), 'PreviousRate': 0, 'PreviousAppliedDate': new Date(), 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_CustomerApprovedRateList }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_CustomerApprovedRateList = data;
            $scope.tbl_Ac_CustomerApprovedRateList.AppliedDate = new Date(data.AppliedDate);
            $scope.tbl_Ac_CustomerApprovedRateList.PreviousAppliedDate = new Date(data.PreviousAppliedDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    