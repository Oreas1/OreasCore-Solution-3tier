MainModule
    .controller("BMRAdditionalMasterCtlr", function ($scope, $http) {
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
            '/QA/BMR/BMRAdditionalMasterLoad', //--v_Load
            '/QA/BMR/BMRAdditionalMasterGet', // getrow
            '/QA/BMR/BMRAdditionalMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/QA/BMR/GetInitializedBMRAdditional');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'BMRAdditionalMasterCtlr') != undefined) {                
                $scope.Privilege = data.find(o => o.Controller === 'BMRAdditionalMasterCtlr').Privilege;    
                init_Filter($scope, data.find(o => o.Controller === 'BMRAdditionalMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'BMRAdditionalMasterCtlr').LoadByCard);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'BMRAdditionalDetailCtlr') != undefined) {
                $scope.$broadcast('init_BMRAdditionalDetailCtlr', data.find(o => o.Controller === 'BMRAdditionalDetailCtlr'));
            }
        };

        init_ProductSearchModalGeneral($scope, $http);
        init_WHMSearchModalGeneral($scope, $http);

        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Pro_BMRAdditionalMaster.FK_tbl_Inv_WareHouseMaster_ID = item.ID;
                $scope.tbl_Pro_BMRAdditionalMaster.FK_tbl_Inv_WareHouseMaster_IDName = item.WareHouseName;
            }
            else {
                $scope.tbl_Pro_BMRAdditionalMaster.FK_tbl_Inv_WareHouseMaster_ID = null;
                $scope.tbl_Pro_BMRAdditionalMaster.FK_tbl_Inv_WareHouseMaster_IDName = null;
            }
        };

        $scope.tbl_Pro_BMRAdditionalMaster = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
            'FK_tbl_Pro_BatchMaterialRequisitionMaster_ID': null, 'FK_tbl_Pro_BatchMaterialRequisitionMaster_IDName': '',
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '', 'IsDispensedAll': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BMRAdditionalMasters = [$scope.tbl_Pro_BMRAdditionalMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BMRAdditionalMaster = {
                'ID': 0, 'DocNo': '', 'DocDate': new Date(),
                'FK_tbl_Pro_BatchMaterialRequisitionMaster_ID': null, 'FK_tbl_Pro_BatchMaterialRequisitionMaster_IDName': '',
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '', 'IsDispensedAll': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_BMRAdditionalMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Pro_BMRAdditionalMaster = data; $scope.tbl_Pro_BMRAdditionalMaster.DocDate = new Date(data.DocDate);   
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

        //------------------------------modal----------------------------------//
        $scope.BatchNoSearchResult = [{ 'ID': '', 'BatchNo': '', 'BatchSize': 0, 'BatchMfgDate': null}];
        $scope.OpenBatchNoSearchModal = function () {
            $scope.BatchNoFilterBy = 'BatchNo'; $scope.BatchNoFilterValue = '';
            $scope.BatchNoSearchResult = [];
            $('#BatchNoSearchModal').modal('show');
        };
        $scope.BatchNoSearch = function () { 
            var successcallback = function (response) {
                $scope.BatchNoSearchResult = response.data;
            };
            var errorcallback = function (error) {           
            };
            $http({ method: "GET", url: "/QA/BMR/LoadBatchNoList?FilterBy=" + $scope.BatchNoFilterBy + "&FilterValue=" + $scope.BatchNoFilterValue, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

        };
        $scope.BatchNoSelectedAc = function (item) {
            $scope.tbl_Pro_BMRAdditionalMaster.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID = item.ID;
            $scope.tbl_Pro_BMRAdditionalMaster.FK_tbl_Pro_BatchMaterialRequisitionMaster_IDName = item.BatchNo;
            $('#BatchNoSearchModal').modal('hide');
        };
       
    })
    .controller("BMRAdditionalDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('BMRAdditionalDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');         
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_BMRAdditionalDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/QA/BMR/GetBMRAdditionalReport'); 
            $scope.BMRAdditionalTypeList = itm.Otherdata === null ? [] : itm.Otherdata.BMRAdditionalTypeList;
        });

       init_Operations($scope, $http,
            '/QA/BMR/BMRAdditionalDetailLoad', //--v_Load
            '/QA/BMR/BMRAdditionalDetailGet', // getrow
            '/QA/BMR/BMRAdditionalDetailPost' // PostRow
        );

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID) {
                $scope.tbl_Pro_BMRAdditionalDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Pro_BMRAdditionalDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Pro_BMRAdditionalDetail.MeasurementUnit = item.MeasurementUnit;
            }
            else {
                $scope.tbl_Pro_BMRAdditionalDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Pro_BMRAdditionalDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Pro_BMRAdditionalDetail.MeasurementUnit = null;
            }
            if (item.IsDecimal) {
                $scope.wholeNumberOrNot = new RegExp("^(0\\.[0]*[1-9][0-9]{0,4}|[1-9][0-9]*(\\.[0-9]{1,5})?)$");
            }
            else {
                $scope.wholeNumberOrNot = new RegExp("^[1-9][0-9]*$");
            }
        };

        $scope.tbl_Pro_BMRAdditionalDetail = {
            'ID': 0, 'FK_tbl_Pro_BMRAdditionalMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Pro_BMRAdditionalType_ID': null, 'FK_tbl_Pro_BMRAdditionalType_IDName': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'Remarks': '', 'IsDispensed': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BMRAdditionalDetails = [$scope.tbl_Pro_BMRAdditionalDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BMRAdditionalDetail = {
                'ID': 0, 'FK_tbl_Pro_BMRAdditionalMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Pro_BMRAdditionalType_ID': null, 'FK_tbl_Pro_BMRAdditionalType_IDName': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'Quantity': 0, 'Remarks': '', 'IsDispensed': false, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_BMRAdditionalDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_BMRAdditionalDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };        

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    