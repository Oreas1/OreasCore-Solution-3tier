MainModule
    .controller("PurchaseNoteActionIndexCtlr", function ($scope, $window, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/QC/Action/PurchaseNoteActionLoad', //--v_Load
            '/QC/Action/PurchaseNoteActionGet', // getrow
            '/QC/Action/PurchaseNoteActionPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/QC/Action/GetInitializedPurchaseNoteAction');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'PurchaseNoteActionIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'PurchaseNoteActionIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'PurchaseNoteActionIndexCtlr').WildCard, null, null, data.find(o => o.Controller === 'PurchaseNoteActionIndexCtlr').LoadByCard);
                if (data.find(o => o.Controller === 'PurchaseNoteActionIndexCtlr').Otherdata === null) {
                    $scope.ActionList = [];
                }
                else {
                    $scope.ActionList = data.find(o => o.Controller === 'PurchaseNoteActionIndexCtlr').Otherdata.ActionList;
                }
                $scope.FilterByLoad = 'byPending';
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_Inv_PurchaseNoteDetail = {
            'ID': 0, 'FK_tbl_Inv_PurchaseNoteMaster_ID': null, 'DocNo': null, 'DocDate': new Date(), 'AccountName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'ReferenceNo': '', 'FK_tbl_Qc_ActionType_ID': 1, 'FK_tbl_Qc_ActionType_IDName': '',
            'QuantitySample': 0, 'RetestDate': null, 'PotencyPercentage': 0,
            'CreatedByQcQa': '', 'CreatedDateQcQa': '', 'ModifiedByQcQa': '', 'ModifiedDateQcQa': '',
            'IsProcessed': false, 'IsSupervised': false
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseNoteDetails = [$scope.tbl_Inv_PurchaseNoteDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Inv_PurchaseNoteDetail = {
                'ID': 0, 'FK_tbl_Inv_PurchaseNoteMaster_ID': null, 'DocNo': null, 'DocDate': new Date(), 'AccountName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'Quantity': 0, 'ReferenceNo': '', 'FK_tbl_Qc_ActionType_ID': 1, 'FK_tbl_Qc_ActionType_IDName': '',
                'QuantitySample': 0, 'RetestDate': null, 'PotencyPercentage': 0,
                'CreatedByQcQa': '', 'CreatedDateQcQa': '', 'ModifiedByQcQa': '', 'ModifiedDateQcQa': '',
                'IsProcessed': false, 'IsSupervised': false
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseNoteDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_PurchaseNoteDetail = data;
            $scope.tbl_Inv_PurchaseNoteDetail.DocDate = new Date(data.DocDate);            
            if (data.RetestDate !== null)
                $scope.tbl_Inv_PurchaseNoteDetail.RetestDate = new Date(data.RetestDate);
            if (data.ExpiryDate !== null)
                $scope.tbl_Inv_PurchaseNoteDetail.ExpiryDate = new Date(data.ExpiryDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

        $scope.GotoReport = function (id) {
            $window.open('/QC/Action/GetPurchaseNoteActionReport?rn=Purchase Note Label&id=' + id);
        };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


