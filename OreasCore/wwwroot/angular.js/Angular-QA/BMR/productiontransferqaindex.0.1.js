MainModule
    .controller("ProductionTransferMasterCtlr", function ($scope, $http) {
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
            '/QA/BMR/ProductionTransferMasterLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/QA/BMR/GetInitializedProductionTransfer');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'ProductionTransferMasterCtlr') != undefined) {                
                $scope.Privilege = data.find(o => o.Controller === 'ProductionTransferMasterCtlr').Privilege;    
                init_Filter($scope, data.find(o => o.Controller === 'ProductionTransferMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'ProductionTransferMasterCtlr').LoadByCard);
                $scope.FilterByLoad = 'byQANotCleared';
                $scope.pageNavigation('first');
                
               
            }
            if (data.find(o => o.Controller === 'ProductionTransferDetailCtlr') != undefined) {
                $scope.$broadcast('init_ProductionTransferDetailCtlr', data.find(o => o.Controller === 'ProductionTransferDetailCtlr'));
            }
        };


        $scope.tbl_Pro_ProductionTransferMaster = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '', 'Remarks': '',
            'QAClearedAll': false, 'ReceivedAll': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_ProductionTransferMasters = [$scope.tbl_Pro_ProductionTransferMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_ProductionTransferMaster = {
                'ID': 0, 'DocNo': '', 'DocDate': new Date(),
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '', 'Remarks': '',
                'QAClearedAll': false, 'ReceivedAll': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_ProductionTransferMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Pro_ProductionTransferMaster = data; $scope.tbl_Pro_ProductionTransferMaster.DocDate = new Date(data.DocDate);   
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
        
    })
    .controller("ProductionTransferDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('ProductionTransferDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');         
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_ProductionTransferDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, itm.LoadByCard); 
        });

       init_Operations($scope, $http,
            '/QA/BMR/ProductionTransferDetailLoad', //--v_Load
            '/QA/BMR/ProductionTransferDetailGet', // getrow
            '/QA/BMR/ProductionTransferDetailPost' // PostRow
        );


        $scope.tbl_Pro_ProductionTransferDetail = {
            'ID': 0, 'FK_tbl_Pro_ProductionTransferMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null, 'ReferenceNo': '',
            'Quantity': 0, 'Remarks': '', 'QACleared': null, 'QAClearedBy': null, 'QAClearedDate': null,
            'Received': false, 'ReceivedBy': null, 'ReceivedDate': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_ProductionTransferDetails = [$scope.tbl_Pro_ProductionTransferDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_ProductionTransferDetail = {
                'ID': 0, 'FK_tbl_Pro_ProductionTransferMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null, 'ReferenceNo': '',
                'Quantity': 0, 'Remarks': '', 'QACleared': null, 'QAClearedBy': null, 'QAClearedDate': null,
                'Received': false, 'ReceivedBy': null, 'ReceivedDate': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {

            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_ProductionTransferDetail };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_ProductionTransferDetail = data;
            if ($scope.tbl_Pro_ProductionTransferDetail.QAClearedDate != null
                ||
                $scope.tbl_Pro_ProductionTransferDetail.QAClearedDate != ''
                ||
                typeof $scope.tbl_Pro_ProductionTransferDetail.QAClearedDate != 'undefined'
            ) {
                $scope.tbl_Pro_ProductionTransferDetail.QAClearedDate = new Date(data.QAClearedDate);
            };
            if ($scope.tbl_Pro_ProductionTransferDetail.ReceivedDate != null
                ||
                $scope.tbl_Pro_ProductionTransferDetail.ReceivedDate != ''
                ||
                typeof $scope.tbl_Pro_ProductionTransferDetail.ReceivedDate != 'undefined'
            ) {
                $scope.tbl_Pro_ProductionTransferDetail.ReceivedDate = new Date(data.ReceivedDate);
            };
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };        

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    