MainModule
    .controller("ChartOfAccountsIndexCtlr", function ($scope, $http, $window) {   
      
        init_Operations($scope, $http,
            '/Accounts/ChartOfAccounts/ChartOfAccountsLoad', //--v_Load
            '/Accounts/ChartOfAccounts/ChartOfAccountsGet', // getrow
            '/Accounts/ChartOfAccounts/ChartOfAccountsPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/ChartOfAccounts/GetInitializedChartOfAccounts');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'ChartOfAccountsIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'ChartOfAccountsIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'ChartOfAccountsIndexCtlr').WildCard, null, null, null);
                init_Report($scope, data.find(o => o.Controller === 'ChartOfAccountsIndexCtlr').Reports, '/Accounts/ChartOfAccounts/GetChartOfAccountsReport');
                $scope.rptID = 0;

                if (data.find(o => o.Controller === 'ChartOfAccountsIndexCtlr').Otherdata === null) {
                    $scope.COATypeList = [];
                    $scope.PolicyWHTaxOnPurchaseList = [];
                    $scope.PolicyWHTaxOnSalesList = [];
                    $scope.PolicyPaymentTermList = [];
                }
                else {
                    $scope.COATypeList = data.find(o => o.Controller === 'ChartOfAccountsIndexCtlr').Otherdata.COATypeList;
                    $scope.PolicyWHTaxOnPurchaseList = data.find(o => o.Controller === 'ChartOfAccountsIndexCtlr').Otherdata.PolicyWHTaxOnPurchaseList;
                    $scope.PolicyWHTaxOnSalesList = data.find(o => o.Controller === 'ChartOfAccountsIndexCtlr').Otherdata.PolicyWHTaxOnSalesList;
                    $scope.PolicyPaymentTermList = data.find(o => o.Controller === 'ChartOfAccountsIndexCtlr').Otherdata.PolicyPaymentTermList;
                }
                $scope.pageNavigation('first');
                $scope.LoadNode();
            }
        };

        init_COASearchModalGeneral($scope, $http);

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Ac_ChartOfAccounts.ParentID = item.ID;
                $scope.tbl_Ac_ChartOfAccounts.ParentName = item.AccountName;
            }
            else {
                $scope.tbl_Ac_ChartOfAccounts.ParentID = null;
                $scope.tbl_Ac_ChartOfAccounts.ParentName = null;
            }
            
        };

        $scope.tbl_Ac_ChartOfAccounts = {
            'ID': 0, 'ParentID': null, 'ParentName': '', 'FK_tbl_Ac_ChartOfAccounts_Type_ID': null, 'FK_tbl_Ac_ChartOfAccounts_Type_IDName': '', 'AccountCode': '', 'AccountName': '',
            'IsTransactional': false, 'IsDiscontinue': false, 'CompanyName': '', 'Address': '',
            'NTN': '', 'STR': '', 'Telephone': '', 'Mobile': '', 'Fax': '', 'Email': '', 'ContactPersonName': '',
            'ContactPersonNumber': '', 'FK_tbl_Ac_PolicyWHTaxOnPurchase_ID': null, 'FK_tbl_Ac_PolicyWHTaxOnPurchase_IDName': '',
            'FK_tbl_Ac_PolicyWHTaxOnSales_ID': null, 'FK_tbl_Ac_PolicyWHTaxOnSales_IDName': '',
            'FK_tbl_Ac_PolicyPaymentTerm_ID': null, 'FK_tbl_Ac_PolicyPaymentTerm_IDName': '',
            'Supplier_Approved': null, 'Supplier_EvaluatedOn': null, 'Supplier_EvaluationScore': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'ChildCount': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_ChartOfAccountss = [$scope.tbl_Ac_ChartOfAccounts];

        $scope.clearEntryPanel = function () {
            $('[href="#Main"]').tab('show');
            //rededine to orignal values            
            $scope.tbl_Ac_ChartOfAccounts = {
                'ID': 0, 'ParentID': null, 'ParentName': '', 'FK_tbl_Ac_ChartOfAccounts_Type_ID': null, 'FK_tbl_Ac_ChartOfAccounts_Type_IDName': '', 'AccountCode': '', 'AccountName': '',
                'IsTransactional': false, 'IsDiscontinue': false, 'CompanyName': '', 'Address': '',
                'NTN': '', 'STR': '', 'Telephone': '', 'Mobile': '', 'Fax': '', 'Email': '', 'ContactPersonName': '',
                'ContactPersonNumber': '', 'FK_tbl_Ac_PolicyWHTaxOnPurchase_ID': null, 'FK_tbl_Ac_PolicyWHTaxOnPurchase_IDName': '',
                'FK_tbl_Ac_PolicyWHTaxOnSales_ID': null, 'FK_tbl_Ac_PolicyWHTaxOnSales_IDName': '',
                'FK_tbl_Ac_PolicyPaymentTerm_ID': null, 'FK_tbl_Ac_PolicyPaymentTerm_IDName': '',
                'Supplier_Approved': null, 'Supplier_EvaluatedOn': null, 'Supplier_EvaluationScore': null,
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

        $scope.rpt = function (id) {
            $window.open('/Accounts/ChartOfAccounts/GetChartOfAccountsReport?rn=Child Accounts List Excel&id=' + id);
        };
        //-----------------------Excel Upload----------------------//
        $scope.LoadFileData = function (files) {
            var formData = new FormData();
            formData.append("COAExcelFile", files[0]);

            var successcallback = function (response) {
                if (response.data === 'OK') {
                    document.getElementById('UploadExcelFile').value = '';
                    $scope.pageNavigation('Load');
                    alert('Successfully Updated');
                }
                else {
                    console.log(response.data);
                }
            };
            var errorcallback = function (error) {
            };

            $http({
                method: "POST", url: "/Accounts/ChartOfAccounts/ChartOfAccountsUploadExcelFile", params: { MasterID: $scope.tbl_Ac_ChartOfAccounts.ID, operation: 'Save New' }, data: formData, headers: { 'Content-Type': undefined, 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }, transformRequest: angular.identity
            }).then(successcallback, errorcallback);
        };

        ////////////////////////////////////////-------------tree view----------------------/////////////////////////////////////////////////////

        $scope.SelectedNode = '';
        $scope.nodedata = [{ 'sign': '+', 'ID': null, 'AccountName': '', 'ParentID': null, 'ChildCount': 0, 'spacing': '', 'IsParent': false }];

        $scope.LoadNode = function () {
            setOperationMessage('Please Wait while Loading Tree...', 0);
            var successcallback = function (response) {
                $scope.nodedata.length = 0;
                $scope.nodedata = response.data;
            };
            var errorcallback = function (error) { };
            $http({ method: "GET", url: "/Accounts/chartofaccounts/GetNodes", params: { PID: 0 }, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

        };
        
        $scope.range = function (n) {
            return new Array(n);
        };

        $scope.getchild = function (Value_ID) {

            var index = $scope.nodedata.findIndex(x => x.ID === Value_ID);


            var successcallback = function (response) {

                for (var i = 0; i < response.data.length; i++) {
                    $scope.nodedata.splice(index + i + 1, 0, response.data[i]);
                    $scope.nodedata[index + i + 1].spacing = $scope.nodedata[index].spacing + '_';
                    if ($scope.nodedata[index + i + 1].ChildCount > 0)
                        $scope.nodedata[index + i + 1].sign = '+';
                    else
                        $scope.nodedata[index + i + 1].sign = '-';
                    $scope.nodedata[index].sign = "-";
                }
            };
            var errorcallback = function (error) { };

            if ($scope.nodedata[index].ChildCount > 0 && $scope.nodedata.findIndex(x => x.ParentID === Value_ID) === -1) {
                $http({ method: "GET", url: "/Accounts/chartofaccounts/GetNodes", params: { PID: Value_ID }, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
            }
            else {
                setOperationMessage('Please Wait while unloading data...', 0);
                $scope.RemoveChildNotes(Value_ID);
                setOperationMessage('', 0);
            }



        };

        $scope.RemoveChildNotes = function (Value_ID) {

            var index = $scope.nodedata.findIndex(x => x.ID === Value_ID);
            if ($scope.nodedata[index].ChildCount > 0)
                $scope.nodedata[index].sign = '+';
            else
                $scope.nodedata[index].sign = '-';
            for (var i = index + 1; i < $scope.nodedata.length; i++) {
                if ($scope.nodedata.findIndex(x => x.ParentID === $scope.nodedata[i].ID) > 0 && $scope.nodedata[i].ParentID === Value_ID) {
                    $scope.RemoveChildNotes($scope.nodedata[i].ID);
                    i = i - 1;
                }
                else {
                    if ($scope.nodedata[i].ParentID === Value_ID) {
                        $scope.nodedata.splice(i, 1);
                        i = i - 1;
                    }

                }
            }
        };

        $scope.SelectFromTree = function (Value_ID) {            
            $scope.GetRow(Value_ID, 'Edit');
        };

    //////////////////////////////end/////////////////////////

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


