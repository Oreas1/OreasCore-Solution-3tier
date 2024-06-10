MainModule
    .controller("GeneralSettingsIndexCtlr", function ($scope, $http) {   
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
        ////////////data structure define//////////////////
        //for entrypanel model

        init_ViewSetup($scope, $http, '/Identity/Account/GetInitializedGeneralSettings');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'GeneralSettingsIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'GeneralSettingsIndexCtlr').Privilege;
                $scope.Load1();
            }
            if (data.find(o => o.Controller === 'GeneralSettingsProductTypeCtlr') != undefined) {
                $scope.$broadcast('init_GeneralSettingsProductTypeCtlr', data.find(o => o.Controller === 'GeneralSettingsProductTypeCtlr'));
            }
        };        

        $scope.AspNetOreasGeneralSettings = {
            'ID': 0, 'OrderNoteRateAutoFromCRL': true, 'SalesNoteDetailRateAutoInsertFromCRL': false,
            'AcBankVoucherAutoSupervised': false, 'AcCashVoucherAutoSupervised': false, 'AcJournalVoucherAutoSupervised': false,
            'AcPurchaseNoteInvoiceAutoSupervised': false, 'AcPurchaseReturnNoteInvoiceAutoSupervised': false,
            'AcSalesNoteInvoiceAutoSupervised': false, 'AcSalesReturnNoteInvoiceAutoSupervised': false,
            'LetterHead_PaperSize': 'A4', 'LetterHead_HeaderHeight': 100, 'LetterHead_FooterHeight': 70
        };

        $scope.Load1 = function () {
            
            var successcallback = function (response) {                
                $scope.AspNetOreasGeneralSettings = response.data;
            };
            var errorcallback = function (error) { };

            $http({ method: "GET", url: "/Identity/Account/GeneralSettingsLoad", headers: { 'X-Requested-With': 'XMLHttpRequest'} }).then(successcallback, errorcallback);

        };

        

        $scope.PostRow = function () {      
            var successcallback = function (response) {
                if (response.data === 'OK') {
                    $scope.Load1();
                    alert('Updated Successfully');
                }
            };
            var errorcallback = function (error) { };
            if (confirm("Are you sure! you want to update record") === true) {
                $http({
                    method: "POST", url: "/Identity/Account/GeneralSettingsPost", async: true, params: { operation: 'Save Update' }, data: $scope.AspNetOreasGeneralSettings, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
                }).then(successcallback, errorcallback);
            }
        };


    })
    .controller("GeneralSettingsProductTypeCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('GeneralSettingsProductTypeCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        
        });

        $scope.$on('init_GeneralSettingsProductTypeCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
         });


        init_Operations($scope, $http,
            '/Identity/Account/ProductTypeLoad', //--v_Load
            '/Identity/Account/ProductTypeGet', // getrow
            '/Identity/Account/ProductTypePost' // PostRow
        );


        $scope.tbl_Inv_ProductType = {
            'ID': 0, 'ProductType': '', 'Prefix': '', 'FK_tbl_Qc_ActionType_ID_PurchaseNote': null, 'FK_tbl_Qc_ActionType_ID_PurchaseNoteName': '',
            'PurchaseNoteDetailRateAutoInsertFromPO': false, 'PurchaseNoteDetailWithOutPOAllowed': true,
            'SalesNoteDetailRateAutoInsertFromON': false, 'SalesNoteDetailWithOutONAllowed': true,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'NoOfCategories': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_ProductType_Categorys = [$scope.tbl_Inv_ProductType_Category];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_ProductType = {
                'ID': 0, 'ProductType': '', 'Prefix': '', 'FK_tbl_Qc_ActionType_ID_PurchaseNote': null, 'FK_tbl_Qc_ActionType_ID_PurchaseNoteName': '',
                'PurchaseNoteDetailRateAutoInsertFromPO': false, 'PurchaseNoteDetailWithOutPOAllowed': true,
                'SalesNoteDetailRateAutoInsertFromON': false, 'SalesNoteDetailWithOutONAllowed': true,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'NoOfCategories': 0
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_ProductType }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_ProductType = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


