MainModule
    .controller("CashDocumentMasterCtlr", function ($scope, $http) {
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
            init_ViewSetup($scope, $http, '/Accounts/Voucher/GetInitializedCashDocument?IsFor=' + $scope.IsFor);
        }

        //////////////////////////////entry panel/////////////////////////
        init_Operations($scope, $http,
            '/Accounts/Voucher/CashDocumentMasterLoad', //--v_Load
            '/Accounts/Voucher/CashDocumentMasterGet', // getrow
            '/Accounts/Voucher/CashDocumentMasterPost' // PostRow
        );

        
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'CashDocumentMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'CashDocumentMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'CashDocumentMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'CashDocumentMasterCtlr').LoadByCard);
                init_Report($scope, data.find(o => o.Controller === 'CashDocumentMasterCtlr').Reports, '/Accounts/Voucher/GetCashDocumentReport');      
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'CashDocumentDetailCtlr') != undefined) {
                $scope.$broadcast('init_CashDocumentDetailCtlr', data.find(o => o.Controller === 'CashDocumentDetailCtlr'));
            }
        };

        init_COASearchModalGeneral($scope, $http);

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Ac_V_CashDocumentMaster.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                $scope.tbl_Ac_V_CashDocumentMaster.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;
            }
            else {
                $scope.tbl_Ac_V_CashDocumentMaster.FK_tbl_Ac_ChartOfAccounts_ID = null;
                $scope.tbl_Ac_V_CashDocumentMaster.FK_tbl_Ac_ChartOfAccounts_IDName = null;
            }
        };

        $scope.tbl_Ac_V_CashDocumentMaster = {
            'ID': 0, 'VoucherDate': new Date(), 'VoucherNo': '',
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'Debit1_Credit0': ($scope.IsFor === 'Payment' ? false : $scope.IsFor === 'Receive' ? true : null), 
            'IsSupervisedAll': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'Total': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_V_CashDocumentMasters = [$scope.tbl_Ac_V_CashDocumentMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_V_CashDocumentMaster = {
                'ID': 0, 'VoucherDate': new Date(), 'VoucherNo': '',
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'Debit1_Credit0': ($scope.IsFor === 'Payment' ? false : $scope.IsFor === 'Receive' ? true : null), 
                'IsSupervisedAll': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'Total': 0
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_V_CashDocumentMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Ac_V_CashDocumentMaster = data; $scope.tbl_Ac_V_CashDocumentMaster.VoucherDate = new Date(data.VoucherDate);
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID, IsFor: $scope.IsFor }; };
       
    })
    .controller("CashDocumentDetailCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('CashDocumentDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_CashDocumentDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
            init_Report($scope, itm.Reports, '/Accounts/Voucher/GetCashDocumentReport');
            $scope.CashTransactionModeList = itm.Otherdata === null ? [] : itm.Otherdata.CashTransactionModeList;
        });

        init_Operations($scope, $http,
            '/Accounts/Voucher/CashDocumentDetailLoad', //--v_Load
            '/Accounts/Voucher/CashDocumentDetailGet', // getrow
            '/Accounts/Voucher/CashDocumentDetailPost' // PostRow
        );

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                if ($scope.COASearch_CallerName === 'tbl_Ac_V_CashDocumentDetail.FK_tbl_Ac_ChartOfAccounts_IDName') {
                    $scope.tbl_Ac_V_CashDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                    $scope.tbl_Ac_V_CashDocumentDetail.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_V_CashDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_ForName') {
                    $scope.tbl_Ac_V_CashDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_For = item.ID;
                    $scope.tbl_Ac_V_CashDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_ForName = item.AccountName;
                }

            }
            else {
                if ($scope.COASearch_CallerName === 'tbl_Ac_V_CashDocumentDetail.FK_tbl_Ac_ChartOfAccounts_IDName') {
                    $scope.tbl_Ac_V_CashDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID = null;
                    $scope.tbl_Ac_V_CashDocumentDetail.FK_tbl_Ac_ChartOfAccounts_IDName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_V_CashDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_ForName') {
                    $scope.tbl_Ac_V_CashDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_For = null;
                    $scope.tbl_Ac_V_CashDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_ForName = null;
                }
            }
        };

        $scope.tbl_Ac_V_CashDocumentDetail = {
            'ID': 0, 'FK_tbl_Ac_V_CashDocumentMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Ac_V_CashTransactionMode_ID': null, 'FK_tbl_Ac_V_CashTransactionMode_IDName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'PostingDate': new Date(),
            'InstrumentNo': '', 'Narration': '', 'Amount': 0, 
            'FK_tbl_Ac_ChartOfAccounts_ID_For': null, 'FK_tbl_Ac_ChartOfAccounts_ID_ForName': '',
            'IsSupervised': false, 'SupervisedBy': null, 'SupervisedDate': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_V_CashDocumentDetails = [$scope.tbl_Ac_V_CashDocumentDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_V_CashDocumentDetail = {
                'ID': 0, 'FK_tbl_Ac_V_CashDocumentMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Ac_V_CashTransactionMode_ID': null, 'FK_tbl_Ac_V_CashTransactionMode_IDName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'PostingDate': new Date(),
                'InstrumentNo': '', 'Narration': '', 'Amount': 0,
                'FK_tbl_Ac_ChartOfAccounts_ID_For': null, 'FK_tbl_Ac_ChartOfAccounts_ID_ForName': '',
                'IsSupervised': false, 'SupervisedBy': null, 'SupervisedDate': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_V_CashDocumentDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_V_CashDocumentDetail = data;
            $scope.tbl_Ac_V_CashDocumentDetail.PostingDate = new Date(data.PostingDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    