MainModule
    .controller("BankDocumentMasterCtlr", function ($scope, $http) {

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

        $scope.init = function (v) {
            $scope.IsFor = v;
            init_ViewSetup($scope, $http, '/Accounts/Voucher/GetInitializedBankDocument?IsFor=' + $scope.IsFor);
        }
        //////////////////////////////entry panel/////////////////////////
        init_Operations($scope, $http,
            '/Accounts/Voucher/BankDocumentMasterLoad', //--v_Load
            '/Accounts/Voucher/BankDocumentMasterGet', // getrow
            '/Accounts/Voucher/BankDocumentMasterPost' // PostRow
        );

        
        $scope.init_ViewSetup_Response = function (data) {

            if (data.find(o => o.Controller === 'BankDocumentMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'BankDocumentMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'BankDocumentMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'BankDocumentMasterCtlr').LoadByCard);
                // $scope.FYS $scope.FYE
                init_FiscalYear($scope, data.find(o => o.Controller === 'BankDocumentMasterCtlr').Otherdata);
                init_Report($scope, data.find(o => o.Controller === 'BankDocumentMasterCtlr').Reports, '/Accounts/Voucher/GetBankDocumentReport');  
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'BankDocumentDetailCtlr') != undefined) {
                $scope.$broadcast('init_BankDocumentDetailCtlr', data.find(o => o.Controller === 'BankDocumentDetailCtlr'));
            }
        };

        init_COASearchModalGeneral($scope, $http);

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Ac_V_BankDocumentMaster.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                $scope.tbl_Ac_V_BankDocumentMaster.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;
            }
            else {
                $scope.tbl_Ac_V_BankDocumentMaster.FK_tbl_Ac_ChartOfAccounts_ID = null;
                $scope.tbl_Ac_V_BankDocumentMaster.FK_tbl_Ac_ChartOfAccounts_IDName = null;
            }

        };

        $scope.tbl_Ac_V_BankDocumentMaster = {
            'ID': 0, 'VoucherDate': new Date(), 'VoucherNo': '',
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'Debit1_Credit0': ($scope.IsFor === 'Payment' ? false : $scope.IsFor === 'Receive' ? true : null), 
            'IsSupervisedAll': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'Total': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_V_BankDocumentMasters = [$scope.tbl_Ac_V_BankDocumentMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_V_BankDocumentMaster = {
                'ID': 0, 'VoucherDate': new Date(), 'VoucherNo': '',
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'Debit1_Credit0': ($scope.IsFor === 'Payment' ? false : $scope.IsFor === 'Receive' ? true : null ),
                'IsSupervisedAll': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'Total': 0
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_V_BankDocumentMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Ac_V_BankDocumentMaster = data; $scope.tbl_Ac_V_BankDocumentMaster.VoucherDate = new Date(data.VoucherDate);
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID, IsFor: $scope.IsFor }; };
       
    })
    .controller("BankDocumentDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('BankDocumentDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_BankDocumentDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
            init_Report($scope, itm.Reports, '/Accounts/Voucher/GetBankDocumentReport');  
            $scope.BankTransactionModeList = itm.Otherdata === null ? [] : itm.Otherdata.BankTransactionModeList;
        });


        init_Operations($scope, $http,
            '/Accounts/Voucher/BankDocumentDetailLoad', //--v_Load
            '/Accounts/Voucher/BankDocumentDetailGet', // getrow
            '/Accounts/Voucher/BankDocumentDetailPost' // PostRow
        );


        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                if ($scope.COASearch_CallerName === 'tbl_Ac_V_BankDocumentDetail.FK_tbl_Ac_ChartOfAccounts_IDName') {
                    $scope.tbl_Ac_V_BankDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                    $scope.tbl_Ac_V_BankDocumentDetail.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_V_BankDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_ForName') {
                    $scope.tbl_Ac_V_BankDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_For = item.ID;
                    $scope.tbl_Ac_V_BankDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_ForName = item.AccountName;
                }
                
            }
            else {
                if ($scope.COASearch_CallerName === 'tbl_Ac_V_BankDocumentDetail.FK_tbl_Ac_ChartOfAccounts_IDName') {
                    $scope.tbl_Ac_V_BankDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID = null;
                    $scope.tbl_Ac_V_BankDocumentDetail.FK_tbl_Ac_ChartOfAccounts_IDName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_V_BankDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_ForName') {
                    $scope.tbl_Ac_V_BankDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_For = null;
                    $scope.tbl_Ac_V_BankDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_ForName = null;
                }                
            }
        };


        $scope.tbl_Ac_V_BankDocumentDetail = {
            'ID': 0, 'FK_tbl_Ac_V_BankDocumentMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Ac_V_BankTransactionMode_ID': null, 'FK_tbl_Ac_V_BankTransactionMode_IDName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'PostingDate': new Date(),
            'InstrumentNo': '', 'InstrumentDate': new Date(), 'Narration': '', 'Amount': 0, 'Cleared1_Cancelled0': null,
            'FK_tbl_Ac_ChartOfAccounts_ID_For': null, 'FK_tbl_Ac_ChartOfAccounts_ID_ForName': '',
            'IsSupervised': false, 'SupervisedBy': null, 'SupervisedDate': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_V_BankDocumentDetails = [$scope.tbl_Ac_V_BankDocumentDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_V_BankDocumentDetail = {
                'ID': 0, 'FK_tbl_Ac_V_BankDocumentMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Ac_V_BankTransactionMode_ID': null, 'FK_tbl_Ac_V_BankTransactionMode_IDName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'PostingDate': new Date(),
                'InstrumentNo': '', 'InstrumentDate': new Date(), 'Narration': '', 'Amount': 0, 'Cleared1_Cancelled0': null,
                'FK_tbl_Ac_ChartOfAccounts_ID_For': null, 'FK_tbl_Ac_ChartOfAccounts_ID_ForName': '',
                'IsSupervised': false, 'SupervisedBy': null, 'SupervisedDate': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
            $scope.disabledOnDetailAdd = false;
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_V_BankDocumentDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_V_BankDocumentDetail = data;
            $scope.tbl_Ac_V_BankDocumentDetail.PostingDate = new Date(data.PostingDate);
            $scope.tbl_Ac_V_BankDocumentDetail.InstrumentDate = new Date(data.InstrumentDate);

            if (operation === 'Add')
                $scope.disabledOnDetailAdd = true;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    