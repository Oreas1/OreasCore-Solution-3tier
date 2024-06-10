MainModule
    .controller("JournalDocument2MasterCtlr", function ($scope, $http) {
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
            '/Accounts/Voucher/JournalDocument2MasterLoad', //--v_Load
            '/Accounts/Voucher/JournalDocument2MasterGet', // getrow
            '/Accounts/Voucher/JournalDocument2MasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/Voucher/GetInitializedJournalDocument2');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'JournalDocument2MasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'JournalDocument2MasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'JournalDocument2MasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'JournalDocument2MasterCtlr').LoadByCard);
                init_Report($scope, data.find(o => o.Controller === 'JournalDocument2MasterCtlr').Reports, '/Accounts/Voucher/GetJournalDocument2Report');    
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'JournalDocument2DetailCtlr') != undefined) {
                $scope.$broadcast('init_JournalDocument2DetailCtlr', data.find(o => o.Controller === 'JournalDocument2DetailCtlr'));
            }
        };

        init_COASearchModalGeneral($scope, $http);

        $scope.tbl_Ac_V_JournalDocument2Master = {
            'ID': 0, 'VoucherDate': new Date(), 'VoucherNo': '', 'Remarks': '',
            'TotalDebit': 0, 'TotalCredit': 0, 'IsPosted': false,
            'IsSupervisedAll': false, 'SupervisedBy': null, 'SupervisedDate': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_V_JournalDocument2Masters = [$scope.tbl_Ac_V_JournalDocument2Master];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_V_JournalDocument2Master = {
                'ID': 0, 'VoucherDate': new Date(), 'VoucherNo': '', 'Remarks': '',
                'TotalDebit': 0, 'TotalCredit': 0, 'IsPosted': false,
                'IsSupervisedAll': false, 'SupervisedBy': null, 'SupervisedDate': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_V_JournalDocument2Master };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Ac_V_JournalDocument2Master = data; $scope.tbl_Ac_V_JournalDocument2Master.VoucherDate = new Date(data.VoucherDate);
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };


       
    })
    .controller("JournalDocument2DetailCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('JournalDocument2DetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_JournalDocument2DetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
            init_Report($scope, itm.Reports, '/Accounts/Voucher/GetJournalDocument2Report');
        });


        init_Operations($scope, $http,
            '/Accounts/Voucher/JournalDocument2DetailLoad', //--v_Load
            '/Accounts/Voucher/JournalDocument2DetailGet', // getrow
            '/Accounts/Voucher/JournalDocument2DetailPost' // PostRow
        );

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                if ($scope.COASearch_CallerName === 'tbl_Ac_V_JournalDocument2Detail.FK_tbl_Ac_ChartOfAccounts_IDName') {
                    $scope.tbl_Ac_V_JournalDocument2Detail.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                    $scope.tbl_Ac_V_JournalDocument2Detail.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;
                }

            }
            else {
                if ($scope.COASearch_CallerName === 'tbl_Ac_V_JournalDocument2Detail.FK_tbl_Ac_ChartOfAccounts_IDName') {
                    $scope.tbl_Ac_V_JournalDocument2Detail.FK_tbl_Ac_ChartOfAccounts_ID = null;
                    $scope.tbl_Ac_V_JournalDocument2Detail.FK_tbl_Ac_ChartOfAccounts_IDName = null;
                }
            }
        };

        $scope.tbl_Ac_V_JournalDocument2Detail = {
            'ID': 0, 'FK_tbl_Ac_V_JournalDocument2Master_ID': $scope.MasterObject.ID,
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'Debit': 0, 'Credit': 0, 'Narration': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        $scope.OnlyDebitCredit = function (v)
        {
            if (v === 'Credit' && $scope.tbl_Ac_V_JournalDocument2Detail.Credit > 0  && $scope.tbl_Ac_V_JournalDocument2Detail.Debit > 0)
                $scope.tbl_Ac_V_JournalDocument2Detail.Debit = 0;

            if (v === 'Debit' && $scope.tbl_Ac_V_JournalDocument2Detail.Credit > 0 && $scope.tbl_Ac_V_JournalDocument2Detail.Debit > 0)
                $scope.tbl_Ac_V_JournalDocument2Detail.Credit = 0;
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_V_JournalDocument2Details = [$scope.tbl_Ac_V_JournalDocument2Detail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_V_JournalDocument2Detail = {
                'ID': 0, 'FK_tbl_Ac_V_JournalDocument2Master_ID': $scope.MasterObject.ID,
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'Debit': 0, 'Credit': 0, 'Narration': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_V_JournalDocument2Detail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_V_JournalDocument2Detail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        $scope.GetpageNavigationResponse = function (data) {
            $scope.pageddata = data.pageddata;
           
            $scope.MasterObject.TotalDebit = data.pageddata.otherdata.TotalDebit;
            $scope.MasterObject.TotalCredit = data.pageddata.otherdata.TotalCredit;
            $scope.MasterObject.IsPosted = data.pageddata.otherdata.IsPosted;
        };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    