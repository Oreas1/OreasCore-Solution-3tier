MainModule
    .controller("CompositionMasterCtlr", function ($scope, $http) {
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
        //$scope.$on('CompositionMasterCtlr', function (e, itm) {
        //    console.log('c');
        //    $scope.pageNavigation('Load');
        //});
        //////////////////////////////entry panel/////////////////////////
        init_Operations($scope, $http,
            '/Production/Composition/CompositionMasterLoad', //--v_Load
            '/Production/Composition/CompositionMasterGet', // getrow
            '/Production/Composition/CompositionMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Production/Composition/GetInitializedComposition');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'CompositionMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'CompositionMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'CompositionMasterCtlr').WildCard, null, null, null);
                if (data.find(o => o.Controller === 'CompositionMasterCtlr').Otherdata === null) {
                    $scope.MeasurementUnitList = []; $scope.QcTestList = [];
                }
                else {
                    $scope.MeasurementUnitList = data.find(o => o.Controller === 'CompositionMasterCtlr').Otherdata.MeasurementUnitList;
                    $scope.QcTestList = data.find(o => o.Controller === 'CompositionMasterCtlr').Otherdata.QcTestList;
                }
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'CompositionDetailRawMasterCtlr') != undefined) {
                $scope.$broadcast('init_CompositionDetailRawMasterCtlr', data.find(o => o.Controller === 'CompositionDetailRawMasterCtlr'));
            }
            if (data.find(o => o.Controller === 'CompositionDetailRawDetailCtlr') != undefined) {
                $scope.$broadcast('init_CompositionDetailRawDetailCtlr', data.find(o => o.Controller === 'CompositionDetailRawDetailCtlr'));
            }
            if (data.find(o => o.Controller === 'CompositionDetailCouplingMasterCtlr') != undefined) {
                $scope.$broadcast('init_CompositionDetailCouplingMasterCtlr', data.find(o => o.Controller === 'CompositionDetailCouplingMasterCtlr'));
            }
            if (data.find(o => o.Controller === 'CompositionDetailCouplingDetailPackagingMasterCtlr') != undefined) {
                $scope.$broadcast('init_CompositionDetailCouplingDetailPackagingMasterCtlr', data.find(o => o.Controller === 'CompositionDetailCouplingDetailPackagingMasterCtlr'));
            }
            if (data.find(o => o.Controller === 'CompositionDetailCouplingDetailPackagingDetailMasterCtlr') != undefined) {
                $scope.$broadcast('init_CompositionDetailCouplingDetailPackagingDetailMasterCtlr', data.find(o => o.Controller === 'CompositionDetailCouplingDetailPackagingDetailMasterCtlr'));
            }
            if (data.find(o => o.Controller === 'CompositionDetailCouplingDetailPackagingDetailDetailCtlr') != undefined) {
                $scope.$broadcast('init_CompositionDetailCouplingDetailPackagingDetailDetailCtlr', data.find(o => o.Controller === 'CompositionDetailCouplingDetailPackagingDetailDetailCtlr'));
            }
            if (data.find(o => o.Controller === 'BMRProcessCtlr') != undefined) {
                $scope.$broadcast('init_BMRProcessCtlr', data.find(o => o.Controller === 'BMRProcessCtlr'));
            }
            if (data.find(o => o.Controller === 'BPRProcessCtlr') != undefined) {
                $scope.$broadcast('init_BPRProcessCtlr', data.find(o => o.Controller === 'BPRProcessCtlr'));
            }

        };

        init_ProductSearchModalGeneral($scope, $http);

        $scope.tbl_Pro_CompositionMaster = {
            'ID': 0, 'DocNo': null, 'DocDate': new Date(), 'CompositionName': '', 'ShelfLifeInMonths': 1,
            'DimensionValue': 1, 'FK_tbl_Inv_MeasurementUnit_ID_Dimension': null, 'FK_tbl_Inv_MeasurementUnit_ID_DimensionName': '',
            'RevisionNo': null, 'RevisionDate': new Date(), 
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionMasters = [$scope.tbl_Pro_CompositionMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionMaster = {
                'ID': 0, 'DocNo': null, 'DocDate': new Date(), 'CompositionName': '', 'ShelfLifeInMonths': 1,
                'DimensionValue': 1, 'FK_tbl_Inv_MeasurementUnit_ID_Dimension': null, 'FK_tbl_Inv_MeasurementUnit_ID_DimensionName': '',
                'RevisionNo': null, 'RevisionDate': new Date(), 
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Pro_CompositionMaster = data; $scope.tbl_Pro_CompositionMaster.DocDate = new Date(data.DocDate); $scope.tbl_Pro_CompositionMaster.RevisionDate = new Date(data.RevisionDate);
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("CompositionDetailRawMasterCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('CompositionDetailRawMasterCtlr', function (e, itm) {
            $('[href="#BMRFormulaType"]').tab('show');
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');         
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_CompositionDetailRawMasterCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
            init_Report($scope, itm.Reports, '/Production/Composition/GetCompositionReport'); 
            $scope.CompositionFilterList = itm.Otherdata === null ? [] : itm.Otherdata.CompositionFilterList;
        });

       init_Operations($scope, $http,
            '/Production/Composition/CompositionDetailRawMasterLoad', //--v_Load
            '/Production/Composition/CompositionDetailRawMasterGet', // getrow
            '/Production/Composition/CompositionDetailRawMasterPost' // PostRow
        );

        $scope.tbl_Pro_CompositionDetail_RawMaster = {
            'ID': 0, 'FK_tbl_Pro_CompositionMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Pro_CompositionFilterPolicyDetail_ID': null, 'FK_tbl_Pro_CompositionFilterPolicyDetail_IDName': '',
            'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionDetail_RawMasters = [$scope.tbl_Pro_CompositionDetail_RawMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionDetail_RawMaster = {
                'ID': 0, 'FK_tbl_Pro_CompositionMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Pro_CompositionFilterPolicyDetail_ID': null, 'FK_tbl_Pro_CompositionFilterPolicyDetail_IDName': '',
                'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionDetail_RawMaster }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_CompositionDetail_RawMaster = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };        

    })
    .controller("CompositionDetailRawDetailCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('CompositionDetailRawDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');

        });

        $scope.$on('init_CompositionDetailRawDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/Production/Composition/CompositionDetailRawDetailLoad', //--v_Load
            '/Production/Composition/CompositionDetailRawDetailGet', // getrow
            '/Production/Composition/CompositionDetailRawDetailPost' // PostRow
        );

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_CompositionDetail_RawDetail_Items.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Pro_CompositionDetail_RawDetail_Items.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Pro_CompositionDetail_RawDetail_Items.MeasurementUnit = item.MeasurementUnit;
            }
            else {

                $scope.tbl_Pro_CompositionDetail_RawDetail_Items.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Pro_CompositionDetail_RawDetail_Items.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Pro_CompositionDetail_RawDetail_Items.MeasurementUnit = null;
            }

            if (item.IsDecimal) { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+(\.[0-9]{1,4})?$"); }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+$"); }

        };

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

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("CompositionDetailCouplingMasterCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('CompositionDetailCouplingMasterCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');

        });

        $scope.$on('init_CompositionDetailCouplingMasterCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/Production/Composition/CompositionDetailCouplingMasterLoad', //--v_Load
            '/Production/Composition/CompositionDetailCouplingMasterGet', // getrow
            '/Production/Composition/CompositionDetailCouplingMasterPost' // PostRow
        );

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_CompositionDetail_Coupling.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Pro_CompositionDetail_Coupling.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Pro_CompositionDetail_Coupling.MeasurementUnit = item.MeasurementUnit;
            }
            else {

                $scope.tbl_Pro_CompositionDetail_Coupling.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Pro_CompositionDetail_Coupling.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Pro_CompositionDetail_Coupling.MeasurementUnit = null;
            }

            if (item.IsDecimal) { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+(\.[0-9]{1,4})?$"); }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+$"); }

        };

        $scope.tbl_Pro_CompositionDetail_Coupling = {
            'ID': 0, 'FK_tbl_Pro_CompositionMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'BatchSize': $scope.MasterObject.DimensionValue, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionDetail_Couplings = [$scope.tbl_Pro_CompositionDetail_Coupling];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionDetail_Coupling = {
                'ID': 0, 'FK_tbl_Pro_CompositionMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'BatchSize': $scope.MasterObject.DimensionValue, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionDetail_Coupling }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_CompositionDetail_Coupling = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("CompositionDetailCouplingDetailPackagingMasterCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('CompositionDetailCouplingDetailPackagingMasterCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');   
        });

        $scope.$on('init_CompositionDetailCouplingDetailPackagingMasterCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            
        });

        init_Operations($scope, $http,
            '/Production/Composition/CompositionDetailCouplingDetailPackagingMasterLoad', //--v_Load
            '/Production/Composition/CompositionDetailCouplingDetailPackagingMasterGet', // getrow
            '/Production/Composition/CompositionDetailCouplingDetailPackagingMasterPost' // PostRow
        );

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                if ($scope.ProductSearch_CallerName === 'tbl_Pro_CompositionDetail_Coupling_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName') {
                    $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary = item.ID;
                    $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = item.MeasurementUnit + ' ' + item.Description;

                    //------when primary changed then de-select secondary
                    $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary = null;
                    $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName = null;
                }
                else if ($scope.ProductSearch_CallerName === 'tbl_Pro_CompositionDetail_Coupling_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName') {
                    $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary = item.ID;
                    $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName = item.MeasurementUnit + ' ' + item.Description;
                }
            }
            else {
                if ($scope.ProductSearch_CallerName === 'tbl_Pro_CompositionDetail_Coupling_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName') {
                    $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary = null;
                    $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = null;

                    //------when primary changed then de-select secondary
                    $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary = null;
                    $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName = null;
                }
                else if ($scope.ProductSearch_CallerName === 'tbl_Pro_CompositionDetail_Coupling_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName') {
                    $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary = null;
                    $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName = null;
                }
            }
        };


        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster = {
            'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID_Primary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName': '',
            'PackagingName': '', 'IsDiscontinue': false, 
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMasters = [$scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster = {
                'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID_Primary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName': '',
                'PackagingName': '', 'IsDiscontinue': false, 
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("CompositionDetailCouplingDetailPackagingDetailMasterCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('CompositionDetailCouplingDetailPackagingDetailMasterCtlr', function (e, itm) {
            $('[href="#BPRFormulaType"]').tab('show');
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_CompositionDetailCouplingDetailPackagingDetailMasterCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/Production/Composition/GetCompositionReport'); 
            $scope.CompositionFilterList = itm.Otherdata === null ? [] : itm.Otherdata.CompositionFilterList;
        });

        init_Operations($scope, $http,
            '/Production/Composition/CompositionDetailCouplingDetailPackagingDetailMasterLoad', //--v_Load
            '/Production/Composition/CompositionDetailCouplingDetailPackagingDetailMasterGet', // getrow
            '/Production/Composition/CompositionDetailCouplingDetailPackagingDetailMasterPost' // PostRow
        );

        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail = {
            'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Pro_CompositionFilterPolicyDetail_ID': null, 'FK_tbl_Pro_CompositionFilterPolicyDetail_IDName': '', 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetails = [$scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail = {
                'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Pro_CompositionFilterPolicyDetail_ID': null, 'FK_tbl_Pro_CompositionFilterPolicyDetail_IDName': '', 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("CompositionDetailCouplingDetailPackagingDetailDetailCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('CompositionDetailCouplingDetailPackagingDetailDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');

        });

        $scope.$on('init_CompositionDetailCouplingDetailPackagingDetailDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/Production/Composition/CompositionDetailCouplingDetailPackagingDetailDetailLoad', //--v_Load
            '/Production/Composition/CompositionDetailCouplingDetailPackagingDetailDetailGet', // getrow
            '/Production/Composition/CompositionDetailCouplingDetailPackagingDetailDetailPost' // PostRow
        );

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items.MeasurementUnit = item.MeasurementUnit;
            }
            else {

                $scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items.MeasurementUnit = null;
            }

            if (item.IsDecimal) { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+(\.[0-9]{1,4})?$"); }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+$"); }

        };

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
    .controller("BMRProcessCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('BMRProcessCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_BMRProcessCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            $scope.BMRProcedureList = itm.Otherdata === null ? [] : itm.Otherdata.BMRProcedureList;
        });

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_CompositionMaster_ProcessBMR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample = item.ID;
                $scope.tbl_Pro_CompositionMaster_ProcessBMR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = item.ProductName + ' ' + item.MeasurementUnit;
            }
            else {
                $scope.tbl_Pro_CompositionMaster_ProcessBMR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample = null;
                $scope.tbl_Pro_CompositionMaster_ProcessBMR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = null;
            }
        };

        init_Operations($scope, $http,
            '/Production/Composition/BMRProcessLoad', //--v_Load
            '/Production/Composition/BMRProcessGet', // getrow
            '/Production/Composition/BMRProcessPost' // PostRow
        );

        $scope.tbl_Pro_CompositionMaster_ProcessBMR = {
            'ID': 0, 'FK_tbl_Pro_CompositionMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Pro_Procedure_ID': null, 'FK_tbl_Pro_Procedure_IDName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName': '', 'MeasurementUnit': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionMaster_ProcessBMRs = [$scope.tbl_Pro_CompositionMaster_ProcessBMR];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionMaster_ProcessBMR = {
                'ID': 0, 'FK_tbl_Pro_CompositionMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Pro_Procedure_ID': null, 'FK_tbl_Pro_Procedure_IDName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName': '', 'MeasurementUnit': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionMaster_ProcessBMR }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_CompositionMaster_ProcessBMR = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };



    })
    .controller("BPRProcessCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('BPRProcessCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_BPRProcessCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            $scope.BPRProcedureList = itm.Otherdata === null ? [] : itm.Otherdata.BPRProcedureList;
        });

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample = item.ID;
                $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = item.ProductName + ' ' + item.MeasurementUnit;
            }
            else {
                $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample = null;
                $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = null;
            }
        };

        init_Operations($scope, $http,
            '/Production/Composition/BPRProcessLoad', //--v_Load
            '/Production/Composition/BPRProcessGet', // getrow
            '/Production/Composition/BPRProcessPost' // PostRow
        );

        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR = {
            'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Pro_Procedure_ID': null, 'FK_tbl_Pro_Procedure_IDName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName': '', 'MeasurementUnit': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPRs = [$scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR = {
                'ID': 0, 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Pro_Procedure_ID': null, 'FK_tbl_Pro_Procedure_IDName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName': '', 'MeasurementUnit': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });
