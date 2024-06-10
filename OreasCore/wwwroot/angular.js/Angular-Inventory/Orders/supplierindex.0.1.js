MainModule
    .controller("SupplierIndexCtlr", function ($scope, $http) {   
        $scope.DivHideShow = function (v, itm, div_hide, div_show, scope) {
            if (typeof v !== 'undefined' && v !== '' && v !== null) {
                $scope.$broadcast(v, itm);
            }
            if (typeof scope !== 'undefined' && scope !== '' && scope !== null && typeof scope.$parent.pageNavigation === 'function') {
                scope.$parent.pageNavigation('Load');
            }

            $("#" + div_hide).hide('slow');
            $("#" + div_show).show('slow');
        };
        init_Operations($scope, $http,
            '/Inventory/Orders/SupplierLoad', //--v_Load
            '/Inventory/Orders/SupplierGet', // getrow
            '/Inventory/Orders/SupplierPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/Orders/GetInitializedSupplier');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'SupplierIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'SupplierIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'SupplierIndexCtlr').WildCard, null, null, null);
                init_Report($scope, data.find(o => o.Controller === 'SupplierIndexCtlr').Reports, '/Inventory/Orders/GetSupplierReport');

                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'SupplierEvaluationCtlr') != undefined) {
                $scope.$broadcast('init_SupplierEvaluationCtlr', data.find(o => o.Controller === 'SupplierEvaluationCtlr'));
            }
        };

        $scope.tbl_Ac_ChartOfAccounts = {
            'ID': 0, 'ParentID': null, 'ParentName': '', 'FK_tbl_Ac_ChartOfAccounts_Type_ID': null, 'FK_tbl_Ac_ChartOfAccounts_Type_IDName': '', 'AccountCode': '', 'AccountName': '',
            'IsTransactional': false, 'IsDiscontinue': false, 'CompanyName': '', 'Address': '',
            'NTN': '', 'STR': '', 'Telephone': '', 'Mobile': '', 'Fax': '', 'Email': '', 'ContactPersonName': '',
            'ContactPersonNumber': '', 'FK_tbl_Ac_PolicyWHTaxOnPurchase_ID': null, 'FK_tbl_Ac_PolicyWHTaxOnPurchase_IDName': '',
            'FK_tbl_Ac_PolicyWHTaxOnSales_ID': null, 'FK_tbl_Ac_PolicyWHTaxOnSales_IDName': '',
            'FK_tbl_Ac_PolicyPaymentTerm_ID': null, 'FK_tbl_Ac_PolicyPaymentTerm_IDName': '',
            'Supplier_Approved': null, 'Supplier_EvaluatedOn': null, 'Supplier_EvaluationScore': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'ChildCount': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_ChartOfAccountss = [$scope.tbl_Ac_ChartOfAccounts];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Ac_ChartOfAccounts = {
                'ID': 0, 'ParentID': null, 'ParentName': '', 'FK_tbl_Ac_ChartOfAccounts_Type_ID': null, 'FK_tbl_Ac_ChartOfAccounts_Type_IDName': '', 'AccountCode': '', 'AccountName': '',
                'IsTransactional': false, 'IsDiscontinue': false, 'CompanyName': '', 'Address': '',
                'NTN': '', 'STR': '', 'Telephone': '', 'Mobile': '', 'Fax': '', 'Email': '', 'ContactPersonName': '',
                'ContactPersonNumber': '', 'FK_tbl_Ac_PolicyWHTaxOnPurchase_ID': null, 'FK_tbl_Ac_PolicyWHTaxOnPurchase_IDName': '',
                'FK_tbl_Ac_PolicyWHTaxOnSales_ID': null, 'FK_tbl_Ac_PolicyWHTaxOnSales_IDName': '',
                'FK_tbl_Ac_PolicyPaymentTerm_ID': null, 'FK_tbl_Ac_PolicyPaymentTerm_IDName': '',
                'Supplier_Approved': null, 'Supplier_EvaluatedOn': null, 'Supplier_EvaluationScore': 0,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'ChildCount': 0
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
    .controller("SupplierEvaluationCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('SupplierEvaluationCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.ng_entryPanelSubmitBtnText = 'Save New';
            $scope.rptID = itm.ID;  
        });

        init_Operations($scope, $http,
            '', //--v_Load
            '', // getrow
            '/Inventory/Orders/SupplierPost' // PostRow
        );

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.MasterObject };
        };
        $scope.PostRowResponse = function (data)
        {
            if (data === 'OK')
                alert('Sucessfull');
        };

        $scope.$on('init_SupplierEvaluationCtlr', function (e, itm) {
            init_Report($scope, itm.Reports, '/Inventory/Orders/GetSupplierReport');
        });

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


