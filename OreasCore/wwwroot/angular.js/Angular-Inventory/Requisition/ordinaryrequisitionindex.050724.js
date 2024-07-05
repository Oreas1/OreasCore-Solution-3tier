MainModule
    .controller("OrdinaryRequisitionMasterCtlr", function ($scope, $http) {
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
            '/Inventory/Requisition/OrdinaryRequisitionMasterLoad', //--v_Load
            '/Inventory/Requisition/OrdinaryRequisitionMasterGet', // getrow
            '/Inventory/Requisition/OrdinaryRequisitionMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/Requisition/GetInitializedOrdinaryRequisition');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'OrdinaryRequisitionMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'OrdinaryRequisitionMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'OrdinaryRequisitionMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'OrdinaryRequisitionMasterCtlr').LoadByCard);
                $scope.SectionList = data.find(o => o.Controller === 'OrdinaryRequisitionMasterCtlr').Otherdata === null ? [] : data.find(o => o.Controller === 'OrdinaryRequisitionMasterCtlr').Otherdata.SectionList;
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'OrdinaryRequisitionDetailCtlr') != undefined) {
                $scope.$broadcast('init_OrdinaryRequisitionDetailCtlr', data.find(o => o.Controller === 'OrdinaryRequisitionDetailCtlr'));
            }
        };

        init_ProductSearchModalGeneral($scope, $http);
        init_WHMSearchModalGeneral($scope, $http);

        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_OrdinaryRequisitionMaster.FK_tbl_Inv_WareHouseMaster_ID = item.ID;
                $scope.tbl_Inv_OrdinaryRequisitionMaster.FK_tbl_Inv_WareHouseMaster_IDName = item.WareHouseName;
            }
            else {
                $scope.tbl_Inv_OrdinaryRequisitionMaster.FK_tbl_Inv_WareHouseMaster_ID = null;
                $scope.tbl_Inv_OrdinaryRequisitionMaster.FK_tbl_Inv_WareHouseMaster_IDName = null;
            }
        };

        $scope.tbl_Inv_OrdinaryRequisitionMaster = {
            'ID': 0, 'DocNo': null, 'DocDate': new Date(),
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '', 'IsDispensedAll': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        }; 

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_OrdinaryRequisitionMasters = [$scope.tbl_Inv_OrdinaryRequisitionMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_OrdinaryRequisitionMaster = {
                'ID': 0, 'DocNo': null, 'DocDate': new Date(),
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '', 'IsDispensedAll': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            }; 
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_OrdinaryRequisitionMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Inv_OrdinaryRequisitionMaster = data; $scope.tbl_Inv_OrdinaryRequisitionMaster.DocDate = new Date(data.DocDate);
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID, IsCanViewOnlyOwnData: $scope.Privilege.CanViewOnlyOwnData }; };
       
    })
    .controller("OrdinaryRequisitionDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('OrdinaryRequisitionDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_OrdinaryRequisitionDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
            init_Report($scope, itm.Reports, '/Inventory/Requisition/GetOrdinaryRequisitionReport'); 
            $scope.OrdinaryRequisitionTypeList = itm.Otherdata === null ? [] : itm.Otherdata.OrdinaryRequisitionTypeList;
        });


        init_Operations($scope, $http,
            '/Inventory/Requisition/OrdinaryRequisitionDetailLoad', //--v_Load
            '/Inventory/Requisition/OrdinaryRequisitionDetailGet', // getrow
            '/Inventory/Requisition/OrdinaryRequisitionDetailPost' // PostRow
        );

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_OrdinaryRequisitionDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Inv_OrdinaryRequisitionDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Inv_OrdinaryRequisitionDetail.MeasurementUnit = item.MeasurementUnit;
            }
            else {

                $scope.tbl_Inv_OrdinaryRequisitionDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Inv_OrdinaryRequisitionDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Inv_OrdinaryRequisitionDetail.MeasurementUnit = null;
            }

            if (item.IsDecimal) { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+(\.[0-9]{1,4})?$"); }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+$"); }

        };

        $scope.tbl_Inv_OrdinaryRequisitionDetail = {
            'ID': 0, 'FK_tbl_Inv_OrdinaryRequisitionMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_OrdinaryRequisitionType_ID': null, 'FK_tbl_Inv_OrdinaryRequisitionType_IDName': '', 'RequiredTrue_ReturnFalse': true,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'Remarks': '', 'IsDispensed': false,'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_OrdinaryRequisitionDetails = [$scope.tbl_Inv_OrdinaryRequisitionDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_OrdinaryRequisitionDetail = {
                'ID': 0, 'FK_tbl_Inv_OrdinaryRequisitionMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_OrdinaryRequisitionType_ID': null, 'FK_tbl_Inv_OrdinaryRequisitionType_IDName': '', 'RequiredTrue_ReturnFalse': true,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'Quantity': 0, 'Remarks': '', 'IsDispensed': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_OrdinaryRequisitionDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_OrdinaryRequisitionDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    