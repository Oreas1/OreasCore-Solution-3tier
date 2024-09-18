MainModule
    .controller("ProductRegistrationPNQcTestCtlr", function ($scope, $http) {
        $scope.DivHideShow = function (v, itm, div_hide, div_show, scope) {
            if (typeof v !== 'undefined' && v !== '' && v !== null) {
                $scope.$broadcast(v, itm);
            }
            if (typeof scope !== 'undefined' && scope !== '' && scope !== null && typeof scope.$parent.pageNavigation === 'function')
            {            
               scope.$parent.pageNavigation('Load');
            }
            if (div_hide !== null)
                $("#" + div_hide).hide('slow');
            if (div_show !== null)
                $("#" + div_show).show('slow');   
        };
        //////////////////////////////entry panel/////////////////////////
        init_Operations($scope, $http,
            '/QC/Testing/ProductRegistrationPNQcTestLoad', //--v_Load
            '/QC/Testing/ProductRegistrationPNQcTestGet', // getrow
            '/QC/Testing/ProductRegistrationPNQcTestPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/QC/Testing/GetInitializedProductRegistrationPNQcTest');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'ProductRegistrationPNQcTestCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'ProductRegistrationPNQcTestCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'ProductRegistrationPNQcTestCtlr').WildCard, null, null, data.find(o => o.Controller === 'ProductRegistrationPNQcTestCtlr').LoadByCard);
                if (data.find(o => o.Controller === 'ProductRegistrationPNQcTestCtlr').Otherdata === null) {
                    $scope.QcTestList = []; $scope.MeasurementUnitList = [];
                }
                else {
                    $scope.QcTestList = data.find(o => o.Controller === 'ProductRegistrationPNQcTestCtlr').Otherdata.QcTestList;
                    $scope.MeasurementUnitList = data.find(o => o.Controller === 'ProductRegistrationPNQcTestCtlr').Otherdata.MeasurementUnitList;
                }
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'ProductRegistrationPNQcTestDetailCtlr') != undefined) {
                $scope.$broadcast('init_ProductRegistrationPNQcTestDetailCtlr', data.find(o => o.Controller === 'ProductRegistrationPNQcTestDetailCtlr'));
            }
        };

        $scope.tbl_Inv_ProductRegistrationDetail = {
            'ID': 0, 'ProductName': null, 'ProductType': null, 'CategoryName': null, 'MeasurementUnit': null, 'ProductCode': null,
            'ControlProcedureNo': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_ProductRegistrationDetails = [$scope.tbl_Inv_ProductRegistrationDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_ProductRegistrationDetail = {
                'ID': 0, 'ProductName': null, 'ProductType': null, 'CategoryName': null, 'MeasurementUnit': null, 'ProductCode': null,
                'ControlProcedureNo': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("ProductRegistrationPNQcTestDetailCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('ProductRegistrationPNQcTestDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_ProductRegistrationPNQcTestDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/QC/Testing/ProductRegistrationPNQcTestDetailLoad', //--v_Load
            '/QC/Testing/ProductRegistrationPNQcTestDetailGet', // getrow
            '/QC/Testing/ProductRegistrationPNQcTestDetailPost' // PostRow
        );

        $scope.tbl_Inv_ProductRegistrationDetail_PNQcTest = {
            'ID': 0, 'FK_tbl_Inv_ProductRegistrationDetail_ID': $scope.MasterObject.ID,
            'FK_tbl_Qc_Test_ID': null, 'FK_tbl_Qc_Test_IDName': '', 'TestDescription': null, 'Specification': null,
            'RangeFrom': null, 'RangeTill': null, 'FK_tbl_Inv_MeasurementUnit_ID': null, 'FK_tbl_Inv_MeasurementUnit_IDName': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_ProductRegistrationDetail_PNQcTest = {
                'ID': 0, 'FK_tbl_Inv_ProductRegistrationDetail_ID': $scope.MasterObject.ID,
                'FK_tbl_Qc_Test_ID': null, 'FK_tbl_Qc_Test_IDName': '', 'TestDescription': null, 'Specification': null,
                'RangeFrom': null, 'RangeTill': null, 'FK_tbl_Inv_MeasurementUnit_ID': null, 'FK_tbl_Inv_MeasurementUnit_IDName': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_ProductRegistrationDetail_PNQcTest };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_ProductRegistrationDetail_PNQcTest = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });
