MainModule
    .controller("AcPolicyWHTaxOnPurchaseMasterCtlr", function ($scope, $http) {
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
            '/Accounts/SetUp/AcPolicyWHTaxOnPurchaseMasterLoad', //--v_Load
            '/Accounts/SetUp/AcPolicyWHTaxOnPurchaseMasterGet', // getrow
            '/Accounts/SetUp/AcPolicyWHTaxOnPurchaseMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/SetUp/GetInitializedAcPolicyWHTaxOnPurchase');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'AcPolicyWHTaxOnPurchaseMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'AcPolicyWHTaxOnPurchaseMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'AcPolicyWHTaxOnPurchaseMasterCtlr').WildCard, null, null, null);                    
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'AcPolicyWHTaxOnPurchaseDetailCtlr') != undefined) {
                $scope.$broadcast('init_AcPolicyWHTaxOnPurchaseDetailCtlr', data.find(o => o.Controller === 'AcPolicyWHTaxOnPurchaseDetailCtlr'));
            }
        };

        $scope.tbl_Ac_PolicyWHTaxOnPurchase = {
            'ID': 0, 'WHTaxName': '', 'WHTaxPer': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_PolicyWHTaxOnPurchases = [$scope.tbl_Ac_PolicyWHTaxOnPurchase];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_PolicyWHTaxOnPurchase = {
                'ID': 0, 'WHTaxName': '', 'WHTaxPer': 0,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_PolicyWHTaxOnPurchase };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Ac_PolicyWHTaxOnPurchase = data; 
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("AcPolicyWHTaxOnPurchaseDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('AcPolicyWHTaxOnPurchaseDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_AcPolicyWHTaxOnPurchaseDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
        });


        init_Operations($scope, $http,
            '/Accounts/SetUp/AcPolicyWHTaxOnPurchaseDetailLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.tbl_Ac_ChartOfAccounts = {
            'ID': 0, 'FK_tbl_Ac_PolicyWHTaxOnPurchase_ID': $scope.MasterObject.ID,
            'ParentID': null, 'ParentName': '', 'FK_tbl_Ac_ChartOfAccounts_Type_ID': null, 'FK_tbl_Ac_ChartOfAccounts_Type_IDName': '',
            'AccountCode': '', 'AccountName': '', 'IsTransactional': false
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_ChartOfAccountss = [$scope.tbl_Ac_ChartOfAccounts];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_ChartOfAccounts = {
                'ID': 0, 'FK_tbl_Ac_PolicyWHTaxOnPurchase_ID': $scope.MasterObject.ID,
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


    