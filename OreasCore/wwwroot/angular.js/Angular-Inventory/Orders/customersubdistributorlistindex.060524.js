MainModule
    .controller("CustomerSubDistributorListCtlr", function ($scope, $http) {
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
            '/Inventory/Orders/CustomerSubDistributorListLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/Orders/GetInitializedCustomerSubDistributorList');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'CustomerSubDistributorListCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'CustomerSubDistributorListCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'CustomerSubDistributorListCtlr').WildCard, null, null, null);                    
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'CustomerSubDistributorListDetailCtlr') != undefined) {
                $scope.$broadcast('init_CustomerSubDistributorListDetailCtlr', data.find(o => o.Controller === 'CustomerSubDistributorListDetailCtlr'));
            }
        };

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
    .controller("CustomerSubDistributorListDetailCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('CustomerSubDistributorListDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_CustomerSubDistributorListDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
        });

        init_Operations($scope, $http,
            '/Inventory/Orders/CustomerSubDistributorListDetailLoad', //--v_Load
            '/Inventory/Orders/CustomerSubDistributorListDetailGet', // getrow
            '/Inventory/Orders/CustomerSubDistributorListDetailPost' // PostRow
        );

        $scope.tbl_Ac_CustomerSubDistributorList = {
            'ID': 0, 'FK_tbl_Ac_ChartOfAccounts_ID': $scope.MasterObject.ID,
            'Name': null, 'Address': null, 'ContactNo': null, 'ContactPerson': null, 'Email': null, 'Remarks': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_CustomerSubDistributorLists = [$scope.tbl_Ac_CustomerSubDistributorList];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_CustomerSubDistributorList = {
                'ID': 0, 'FK_tbl_Ac_ChartOfAccounts_ID': $scope.MasterObject.ID,
                'Name': null, 'Address': null, 'ContactNo': null, 'ContactPerson': null, 'Email': null, 'Remarks': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_CustomerSubDistributorList }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_CustomerSubDistributorList = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    