MainModule
    .controller("AcPolicyWHTaxOnSalesMasterCtlr", function ($scope, $http) {
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
            '/Accounts/SetUp/AcPolicyWHTaxOnSalesMasterLoad', //--v_Load
            '/Accounts/SetUp/AcPolicyWHTaxOnSalesMasterGet', // getrow
            '/Accounts/SetUp/AcPolicyWHTaxOnSalesMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/SetUp/GetInitializedAcPolicyWHTaxOnSales');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'AcPolicyWHTaxOnSalesMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'AcPolicyWHTaxOnSalesMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'AcPolicyWHTaxOnSalesMasterCtlr').WildCard, null, null, null);                    
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'AcPolicyWHTaxOnSalesDetailCtlr') != undefined) {
                $scope.$broadcast('init_AcPolicyWHTaxOnSalesDetailCtlr', data.find(o => o.Controller === 'AcPolicyWHTaxOnSalesDetailCtlr'));
            }
        };

        $scope.tbl_Ac_PolicyWHTaxOnSales = {
            'ID': 0, 'WHTaxName': '', 'WHTaxPer': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_PolicyWHTaxOnSaless = [$scope.tbl_Ac_PolicyWHTaxOnSales];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_PolicyWHTaxOnSales = {
                'ID': 0, 'WHTaxName': '', 'WHTaxPer': 0,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_PolicyWHTaxOnSales };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Ac_PolicyWHTaxOnSales = data; 
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("AcPolicyWHTaxOnSalesDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('AcPolicyWHTaxOnSalesDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_AcPolicyWHTaxOnSalesDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
        });


        init_Operations($scope, $http,
            '/Accounts/SetUp/AcPolicyWHTaxOnSalesDetailLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.tbl_Ac_ChartOfAccounts = {
            'ID': 0, 'FK_tbl_Ac_PolicyWHTaxOnSales_ID': $scope.MasterObject.ID,
            'ParentID': null, 'ParentName': '', 'FK_tbl_Ac_ChartOfAccounts_Type_ID': null, 'FK_tbl_Ac_ChartOfAccounts_Type_IDName': '',
            'AccountCode': '', 'AccountName': '', 'IsTransactional': false
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_ChartOfAccountss = [$scope.tbl_Ac_ChartOfAccounts];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_ChartOfAccounts = {
                'ID': 0, 'FK_tbl_Ac_PolicyWHTaxOnSales_ID': $scope.MasterObject.ID,
                'ParentID': null, 'ParentName': '', 'FK_tbl_Ac_ChartOfAccounts_Type_ID': null, 'FK_tbl_Ac_ChartOfAccounts_Type_IDName': '',
                'AccountCode': '', 'AccountName': '', 'IsTransactional': false
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_ChartOfAccounts }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_ChartOfAccounts = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    