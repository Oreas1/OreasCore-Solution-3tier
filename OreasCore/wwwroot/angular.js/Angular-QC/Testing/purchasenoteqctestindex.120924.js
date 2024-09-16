MainModule
    .controller("PurchaseNoteQcTestCtlr", function ($scope, $window, $http) {   
        $scope.DivHideShow = function (v, itm, div_hide, div_show, scope) {
            if (typeof v !== 'undefined' && v !== '' && v !== null) {
                $scope.$broadcast(v, itm);
            }
            if (typeof scope !== 'undefined' && scope !== '' && scope !== null && typeof scope.$parent.pageNavigation === 'function') {
                scope.$parent.pageNavigation('Load');
            }
            if (div_hide !== null)
                $("#" + div_hide).hide('slow');
            if (div_show !== null)
                $("#" + div_show).show('slow');
        };
        //for entrypanel model
        init_Operations($scope, $http,
            '/QC/Testing/PurchaseNoteQcTestLoad', //--v_Load
            '/QC/Testing/PurchaseNoteQcTestGet', // getrow
            '/QC/Testing/PurchaseNoteQcTestPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/QC/Testing/GetInitializedPurchaseNoteQcTest');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'PurchaseNoteQcTestCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'PurchaseNoteQcTestCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'PurchaseNoteQcTestCtlr').WildCard, null, null, data.find(o => o.Controller === 'PurchaseNoteQcTestCtlr').LoadByCard);
                if (data.find(o => o.Controller === 'PurchaseNoteQcTestCtlr').Otherdata === null) {
                    $scope.ActionList = []; $scope.MeasurementUnitList = []; $scope.QcTestList = [];
                }
                else {
                    $scope.ActionList = data.find(o => o.Controller === 'PurchaseNoteQcTestCtlr').Otherdata.ActionList;
                    $scope.MeasurementUnitList = data.find(o => o.Controller === 'PurchaseNoteQcTestCtlr').Otherdata.MeasurementUnitList;
                    $scope.QcTestList = data.find(o => o.Controller === 'PurchaseNoteQcTestCtlr').Otherdata.QcTestList;
                }
                $scope.FilterByLoad = 'byPending';
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'PurchaseNoteQcTestDetailCtlr') != undefined) {
                $scope.$broadcast('init_PurchaseNoteQcTestDetailCtlr', data.find(o => o.Controller === 'PurchaseNoteQcTestDetailCtlr'));
            }
        };

        $scope.tbl_Inv_PurchaseNoteDetail = {
            'ID': 0, 'FK_tbl_Inv_PurchaseNoteMaster_ID': null, 'DocNo': null, 'DocDate': new Date(), 'AccountName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'ReferenceNo': '', 'FK_tbl_Qc_ActionType_ID': 1, 'FK_tbl_Qc_ActionType_IDName': '',
            'QuantitySample': 0, 'RetestDate': null, 'QCComments': null, 'PotencyPercentage': 0,
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
                'QuantitySample': 0, 'RetestDate': null, 'QCComments': null, 'PotencyPercentage': 0,
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

            if (data.IsDecimal) { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+(\.[0-9]{1,4})?$"); }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+$"); }
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

        $scope.GotoReport = function (id) {
            $window.open('/QC/Testing/GetPurchaseNoteQcTestReport?rn=Purchase Note Label&id=' + id);
        };

    })
    .controller("PurchaseNoteQcTestDetailCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('PurchaseNoteQcTestDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_PurchaseNoteQcTestDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/QC/Testing/PurchaseNoteQcTestDetailLoad', //--v_Load
            '/QC/Testing/PurchaseNoteQcTestDetailGet', // getrow
            '/QC/Testing/PurchaseNoteQcTestDetailPost' // PostRow
        );

        $scope.tbl_Qc_PurchaseNoteDetail_QcTest = {
            'ID': 0, 'FK_tbl_Inv_PurchaseNoteDetail_ID': $scope.MasterObject.ID,
            'FK_tbl_Qc_Test_ID': null, 'FK_tbl_Qc_Test_IDName': '', 'TestDescription': null, 'Specification': null,
            'RangeFrom': null, 'RangeTill': null, 'FK_tbl_Inv_MeasurementUnit_ID': null, 'FK_tbl_Inv_MeasurementUnit_IDName': null,
            'ResultValue': 0, 'ResultRemarks': null, 'IsPrintOnCOA': true,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Qc_PurchaseNoteDetail_QcTest = {
                'ID': 0, 'FK_tbl_Inv_PurchaseNoteDetail_ID': $scope.MasterObject.ID,
                'FK_tbl_Qc_Test_ID': null, 'FK_tbl_Qc_Test_IDName': '', 'TestDescription': null, 'Specification': null,
                'RangeFrom': null, 'RangeTill': null, 'FK_tbl_Inv_MeasurementUnit_ID': null, 'FK_tbl_Inv_MeasurementUnit_IDName': null,
                'ResultValue': 0, 'ResultRemarks': null, 'IsPrintOnCOA': true,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Qc_PurchaseNoteDetail_QcTest };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Qc_PurchaseNoteDetail_QcTest = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        $scope.Replication = function (id)
        {
            var successcallback = function (response) {
                if (response.data === 'OK')
                {
                    $scope.pageNavigation('first');
                }
                else {
                    alert(response.data);
                }                
            };

            var errorcallback = function (error) {
                alert(error.data);
                console.log(error);
            };

            if (confirm("Are you sure! you want to Fetch Standard Test List") === true)
            {
                $http({
                    method: "POST", url: '/QC/Testing/PurchaseNoteQcTestDetailPostReplication', async: true, params: { MasterID: id, operation: 'Save New' }, data: null, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
                }).then(successcallback, errorcallback); 
            }
            
        };
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


