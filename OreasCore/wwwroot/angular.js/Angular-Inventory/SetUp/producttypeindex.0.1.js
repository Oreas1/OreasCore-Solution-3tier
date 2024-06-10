MainModule
    .controller("ProductTypeMasterCtlr", function ($scope, $http) {
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
            '/Inventory/SetUp/ProductTypeMasterLoad', //--v_Load
            '/Inventory/SetUp/ProductTypeMasterGet', // getrow
            '/Inventory/SetUp/ProductTypeMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/SetUp/GetInitializedProductType');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'ProductTypeMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'ProductTypeMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'ProductTypeMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'ProductTypeMasterCtlr').LoadByCard);
                if (data.find(o => o.Controller === 'ProductTypeMasterCtlr').Otherdata === null) {
                    $scope.ActionList = [];
                }
                else {
                    $scope.ActionList = data.find(o => o.Controller === 'ProductTypeMasterCtlr').Otherdata.ActionList;
                }
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'ProductTypeDetailCtlr') != undefined) {
                $scope.$broadcast('init_ProductTypeDetailCtlr', data.find(o => o.Controller === 'ProductTypeDetailCtlr'));
            }
        };

        $scope.tbl_Inv_ProductType = {
            'ID': 0, 'ProductType': '', 'Prefix': '', 'FK_tbl_Qc_ActionType_ID_PurchaseNote': null, 'FK_tbl_Qc_ActionType_ID_PurchaseNoteName': '',
            'PurchaseNoteDetailRateAutoInsertFromPO': false, 'PurchaseNoteDetailWithOutPOAllowed': true,
            'SalesNoteDetailRateAutoInsertFromON': false, 'SalesNoteDetailWithOutONAllowed': true,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'NoOfCategories': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_ProductTypes = [$scope.tbl_Inv_ProductType];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_ProductType = {
                'ID': 0, 'ProductType': '', 'Prefix': '', 'FK_tbl_Qc_ActionType_ID_PurchaseNote': null, 'FK_tbl_Qc_ActionType_ID_PurchaseNoteName': '',
                'PurchaseNoteDetailRateAutoInsertFromPO': false, 'PurchaseNoteDetailWithOutPOAllowed': true,
                'SalesNoteDetailRateAutoInsertFromON': false, 'SalesNoteDetailWithOutONAllowed': true,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'NoOfCategories': 0
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_ProductType };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Inv_ProductType = data;
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("ProductTypeDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('ProductTypeDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_ProductTypeDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
            init_Report($scope, itm.Reports, '/Inventory/SetUp/GetProductTypeReport'); 
        });


        init_Operations($scope, $http,
            '/Inventory/SetUp/ProductTypeDetailLoad', //--v_Load
            '/Inventory/SetUp/ProductTypeDetailGet', // getrow
            '/Inventory/SetUp/ProductTypeDetailPost' // PostRow
        );


        $scope.tbl_Inv_ProductType_Category = {
            'ID': 0, 'FK_tbl_Inv_ProductType_ID': $scope.MasterObject.ID, 'CategoryName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_ProductType_Categorys = [$scope.tbl_Inv_ProductType_Category];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_ProductType_Category = {
                'ID': 0, 'FK_tbl_Inv_ProductType_ID': $scope.MasterObject.ID, 'CategoryName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_ProductType_Category }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_ProductType_Category = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    