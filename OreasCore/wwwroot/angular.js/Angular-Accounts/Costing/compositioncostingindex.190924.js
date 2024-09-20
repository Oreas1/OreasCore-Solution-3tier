MainModule
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
            '/Accounts/Costing/CompositionCostingMasterGet', // getrow
            '/Accounts/Costing/CompositionCostingMasterPost' // PostRow
        );

        init_ProductSearchModalGeneral($scope, $http);

        init_ViewSetup($scope, $http, '/Accounts/Costing/GetInitializedCompositionCosting');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'CompositionCostingMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'CompositionCostingMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'CompositionCostingMasterCtlr').WildCard, null, null, null);
                if (data.find(o => o.Controller === 'CompositionCostingMasterCtlr').Otherdata === null) {
                    $scope.CostingOverHeadFactorsList = []; 
                }
                else {
                    $scope.CostingOverHeadFactorsList = data.find(o => o.Controller === 'CompositionCostingMasterCtlr').Otherdata.CostingOverHeadFactorsList;
                }
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'CompositionCostingIndirectExpenseCtlr') != undefined) {
                $scope.$broadcast('init_CompositionCostingIndirectExpenseCtlr', data.find(o => o.Controller === 'CompositionCostingIndirectExpenseCtlr'));
            }
            if (data.find(o => o.Controller === 'CompositionCostingDetailRawCtlr') != undefined) {
                $scope.$broadcast('init_CompositionCostingDetailRawCtlr', data.find(o => o.Controller === 'CompositionCostingDetailRawCtlr'));
            }
            if (data.find(o => o.Controller === 'CompositionCostingDetailPackagingCtlr') != undefined) {
                $scope.$broadcast('init_CompositionCostingDetailPackagingCtlr', data.find(o => o.Controller === 'CompositionCostingDetailPackagingCtlr'));
            }
        }; 

        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster = {
            'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_ID': null,
            'FK_tbl_Inv_ProductRegistrationDetail_ID_Primary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName': '',
            'PackagingName': '', 'IsDiscontinue': false, 
            'FK_tbl_Ac_CompositionCostingOverHeadFactorsMaster_ID': null, 'FK_tbl_Ac_CompositionCostingOverHeadFactorsMaster_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };


        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMasters = [$scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster = {
                'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_ID': null,
                'FK_tbl_Inv_ProductRegistrationDetail_ID_Primary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName': '',
                'PackagingName': '', 'IsDiscontinue': false,
                'FK_tbl_Ac_CompositionCostingOverHeadFactorsMaster_ID': null, 'FK_tbl_Ac_CompositionCostingOverHeadFactorsMaster_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };

        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster = data;
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("CompositionCostingIndirectExpenseCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('CompositionCostingIndirectExpenseCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });        

        $scope.$on('init_CompositionCostingIndirectExpenseCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/Accounts/Costing/GetCompositionCostingReport'); 
            if (itm.Otherdata === null) {
                $scope.CostingIndirectExpenseList = [];
            }
            else {
                $scope.CostingIndirectExpenseList = itm.Otherdata.CostingIndirectExpenseList;
            }
        });

        init_Operations($scope, $http,
            '/Accounts/Costing/CompositionCostingIndirectExpenseLoad', //--v_Load
            '/Accounts/Costing/CompositionCostingIndirectExpenseGet', // getrow
            '/Accounts/Costing/CompositionCostingIndirectExpensePost' // PostRow
        );

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Ac_CompositionCostingIndirectExpense.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Ac_CompositionCostingIndirectExpense.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Ac_CompositionCostingIndirectExpense.MeasurementUnit = item.MeasurementUnit;

                $scope.tbl_Ac_CompositionCostingIndirectExpense.FK_tbl_Ac_CostingIndirectExpenseList_ID = null;
                $scope.tbl_Ac_CompositionCostingIndirectExpense.FK_tbl_Ac_CostingIndirectExpenseList_IDName = null;
            }
            else {

                $scope.tbl_Ac_CompositionCostingIndirectExpense.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Ac_CompositionCostingIndirectExpense.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Ac_CompositionCostingIndirectExpense.MeasurementUnit = null;
            }
            if (item.IsDecimal) { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+(\.[0-9]{1,4})?$"); }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+$"); }
        };

        $scope.tbl_Ac_CompositionCostingIndirectExpense = {
            'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Ac_CostingIndirectExpenseList_ID': null, 'FK_tbl_Ac_CostingIndirectExpenseList_IDName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'CustomRate': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_CompositionCostingIndirectExpenses = [$scope.tbl_Ac_CompositionCostingIndirectExpense];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_CompositionCostingIndirectExpense = {
                'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Ac_CostingIndirectExpenseList_ID': null, 'FK_tbl_Ac_CostingIndirectExpenseList_IDName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'Quantity': 0, 'CustomRate': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_CompositionCostingIndirectExpense }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_CompositionCostingIndirectExpense = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        $scope.OnChangeOfExpense = function ()
        {
            if ($scope.tbl_Ac_CompositionCostingIndirectExpense.FK_tbl_Ac_CostingIndirectExpenseList_ID > 0)
            {
                $scope.tbl_Ac_CompositionCostingIndirectExpense.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Ac_CompositionCostingIndirectExpense.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Ac_CompositionCostingIndirectExpense.MeasurementUnit = null;
            }
        };
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
            'ID': 0, 'FK_tbl_Pro_CompositionDetail_RawMaster_ID': $scope.MasterObject.FK_tbl_Pro_CompositionMaster_ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
            'CustomeRate': 0, 'PercentageOnRate': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionDetail_RawDetail_Itemss = [$scope.tbl_Pro_CompositionDetail_RawDetail_Items];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionDetail_RawDetail_Items = {
                'ID': 0, 'FK_tbl_Pro_CompositionDetail_RawMaster_ID': $scope.MasterObject.FK_tbl_Pro_CompositionMaster_ID,
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
