﻿MainModule
    .controller("CompositionCostingMasterCtlr", function ($scope, $http) {
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
            '/Accounts/Costing/CompositionCostingMasterLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/Costing/GetInitializedCompositionCosting');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'CompositionCostingMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'CompositionCostingMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'CompositionCostingMasterCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'CompositionCostingDetailRawCtlr') != undefined) {
                $scope.$broadcast('init_CompositionCostingDetailRawCtlr', data.find(o => o.Controller === 'CompositionCostingDetailRawCtlr'));
            }
            if (data.find(o => o.Controller === 'CompositionCostingDetailPackagingCtlr') != undefined) {
                $scope.$broadcast('init_CompositionCostingDetailPackagingCtlr', data.find(o => o.Controller === 'CompositionCostingDetailPackagingCtlr'));
            }
        }; 

        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster = {
            'ID': 0, 'BatchSize': 0, 'SemiProduct': '', 'SemiProductUnit': '', 'PrimaryProduct': '', 'SecondaryProduct': '',
            'PackagingName': '', 'FK_tbl_Pro_CompositionMaster_ID': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMasters = [$scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster = {
                'ID': 0, 'BatchSize': 0, 'SemiProduct': '', 'SemiProductUnit': '', 'PrimaryProduct': '', 'SecondaryProduct': '',
                'PackagingName': '', 'FK_tbl_Pro_CompositionMaster_ID': 0
            };
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("CompositionCostingDetailRawCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('CompositionCostingDetailRawCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_CompositionCostingDetailRawCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/Accounts/Costing/GetCompositionCostingReport');  
        });

        init_Operations($scope, $http,
            '/Accounts/Costing/CompositionCostingDetailRawLoad', //--v_Load
            '/Accounts/Costing/CompositionCostingDetailRawGet', // getrow
            '/Accounts/Costing/CompositionCostingDetailRawPost' // PostRow
        );

        $scope.tbl_Pro_CompositionDetail_RawDetail_Items = {
            'ID': 0, 'FK_tbl_Pro_CompositionDetail_RawMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
            'CustomeRate': 0, 'PercentageOnRate': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionDetail_RawDetail_Itemss = [$scope.tbl_Pro_CompositionDetail_RawDetail_Items];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionDetail_RawDetail_Items = {
                'ID': 0, 'FK_tbl_Pro_CompositionDetail_RawMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'Quantity': 0, 'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
                'CustomeRate': 0, 'PercentageOnRate': 0
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionDetail_RawDetail_Items }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_CompositionDetail_RawDetail_Items = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.FK_tbl_Pro_CompositionMaster_ID }; };

    })
    .controller("CompositionCostingDetailPackagingCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('CompositionCostingDetailPackagingCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_CompositionCostingDetailPackagingCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/Accounts/Costing/GetCompositionCostingReport');
        });

        init_Operations($scope, $http,
            '/Accounts/Costing/CompositionCostingDetailPackagingLoad', //--v_Load
            '/Accounts/Costing/CompositionCostingDetailPackagingGet', // getrow
            '/Accounts/Costing/CompositionCostingDetailPackagingPost' // PostRow
        );

        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items = {
            'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingDetail_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
            'CustomeRate': 0, 'PercentageOnRate': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss = [$scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items = {
                'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingDetail_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'Quantity': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
                'CustomeRate': 0, 'PercentageOnRate': 0
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });
