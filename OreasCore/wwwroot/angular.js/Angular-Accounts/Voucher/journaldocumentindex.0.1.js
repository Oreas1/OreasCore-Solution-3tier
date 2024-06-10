MainModule
    .controller("JournalDocumentMasterCtlr", function ($scope, $http) {
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
            '/Accounts/Voucher/JournalDocumentMasterLoad', //--v_Load
            '/Accounts/Voucher/JournalDocumentMasterGet', // getrow
            '/Accounts/Voucher/JournalDocumentMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/Voucher/GetInitializedJournalDocument');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'JournalDocumentMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'JournalDocumentMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'JournalDocumentMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'JournalDocumentMasterCtlr').LoadByCard);
                init_Report($scope, data.find(o => o.Controller === 'JournalDocumentMasterCtlr').Reports, '/Accounts/Voucher/GetJournalDocumentReport');    
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'JournalDocumentDetailCtlr') != undefined) {
                $scope.$broadcast('init_JournalDocumentDetailCtlr', data.find(o => o.Controller === 'JournalDocumentDetailCtlr'));
            }
        };

        init_COASearchModalGeneral($scope, $http);

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Ac_V_JournalDocumentMaster.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                $scope.tbl_Ac_V_JournalDocumentMaster.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;
            }
            else {
                $scope.tbl_Ac_V_JournalDocumentMaster.FK_tbl_Ac_ChartOfAccounts_ID = null;
                $scope.tbl_Ac_V_JournalDocumentMaster.FK_tbl_Ac_ChartOfAccounts_IDName = null;
            }

        };

        $scope.tbl_Ac_V_JournalDocumentMaster = {
            'ID': 0, 'VoucherDate': new Date(), 'VoucherNo': '',
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'Debit1_Credit0': null, 
            'IsSupervisedAll': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'Total': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_V_JournalDocumentMasters = [$scope.tbl_Ac_V_JournalDocumentMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_V_JournalDocumentMaster = {
                'ID': 0, 'VoucherDate': new Date(), 'VoucherNo': '',
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'Debit1_Credit0': null,
                'IsSupervisedAll': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'Total': 0
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_V_JournalDocumentMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Ac_V_JournalDocumentMaster = data; $scope.tbl_Ac_V_JournalDocumentMaster.VoucherDate = new Date(data.VoucherDate);
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("JournalDocumentDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('JournalDocumentDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_JournalDocumentDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
            init_Report($scope, itm.Reports, '/Accounts/Voucher/GetJournalDocumentReport');
        });


        init_Operations($scope, $http,
            '/Accounts/Voucher/JournalDocumentDetailLoad', //--v_Load
            '/Accounts/Voucher/JournalDocumentDetailGet', // getrow
            '/Accounts/Voucher/JournalDocumentDetailPost' // PostRow
        );

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                if ($scope.COASearch_CallerName === 'tbl_Ac_V_JournalDocumentDetail.FK_tbl_Ac_ChartOfAccounts_IDName') {
                    $scope.tbl_Ac_V_JournalDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                    $scope.tbl_Ac_V_JournalDocumentDetail.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_V_JournalDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_ForName') {
                    $scope.tbl_Ac_V_JournalDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_For = item.ID;
                    $scope.tbl_Ac_V_JournalDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_ForName = item.AccountName;
                }

            }
            else {
                if ($scope.COASearch_CallerName === 'tbl_Ac_V_JournalDocumentDetail.FK_tbl_Ac_ChartOfAccounts_IDName') {
                    $scope.tbl_Ac_V_JournalDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID = null;
                    $scope.tbl_Ac_V_JournalDocumentDetail.FK_tbl_Ac_ChartOfAccounts_IDName = null;
                }
                else if ($scope.COASearch_CallerName === 'tbl_Ac_V_JournalDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_ForName') {
                    $scope.tbl_Ac_V_JournalDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_For = null;
                    $scope.tbl_Ac_V_JournalDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_ForName = null;
                }
            }
        };

        $scope.tbl_Ac_V_JournalDocumentDetail = {
            'ID': 0, 'FK_tbl_Ac_V_JournalDocumentMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'PostingDate': new Date(),
            'Narration': '', 'Amount': 0, 'FK_tbl_Ac_ChartOfAccounts_ID_For': null, 'FK_tbl_Ac_ChartOfAccounts_ID_ForName': '',
            'IsSupervised': false, 'SupervisedBy': null, 'SupervisedDate': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_V_JournalDocumentDetails = [$scope.tbl_Ac_V_JournalDocumentDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_V_JournalDocumentDetail = {
                'ID': 0, 'FK_tbl_Ac_V_JournalDocumentMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '', 'PostingDate': new Date(),
                'Narration': '', 'Amount': 0, 'FK_tbl_Ac_ChartOfAccounts_ID_For': null, 'FK_tbl_Ac_ChartOfAccounts_ID_ForName': '',
                'IsSupervised': false, 'SupervisedBy': null, 'SupervisedDate': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_V_JournalDocumentDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_V_JournalDocumentDetail = data;
            $scope.tbl_Ac_V_JournalDocumentDetail.PostingDate = new Date(data.PostingDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        //-----------------------Excel Upload----------------------//
        $scope.LoadFileData = function (files) {
            var formData = new FormData();
            formData.append("JDExcelFile", files[0]);

            var successcallback = function (response) {
                if (response.data === 'OK') {
                    document.getElementById('UploadExcelFile').value = '';
                    $scope.pageNavigation('first');
                    alert('Successfully Updated');
                }
                else {
                    console.log(response.data);
                }
            };
            var errorcallback = function (error) {
            };

            $http({
                method: "POST", url: "/Accounts/Voucher/JournalDocumentDetailUploadExcelFile", params: { MasterID: $scope.MasterObject.ID, operation: 'Save New' }, data: formData, headers: { 'Content-Type': undefined, 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }, transformRequest: angular.identity
            }).then(successcallback, errorcallback);
        };
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    