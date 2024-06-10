MainModule
    .controller("WareHouseMasterCtlr", function ($scope, $http) {
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
            '/Inventory/SetUp/WareHouseMasterLoad', //--v_Load
            '/Inventory/SetUp/WareHouseMasterGet', // getrow
            '/Inventory/SetUp/WareHouseMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/SetUp/GetInitializedWareHouse');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'WareHouseMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'WareHouseMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'WareHouseMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'WareHouseMasterCtlr').LoadByCard);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'WareHouseDetailCtlr') != undefined) {
                $scope.$broadcast('init_WareHouseDetailCtlr', data.find(o => o.Controller === 'WareHouseDetailCtlr'));
            }
        };

        $scope.tbl_Inv_WareHouseMaster = {
            'ID': 0, 'WareHouseName': '', 'Prefix': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'NoOfCategories': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_WareHouseMasters = [$scope.tbl_Inv_WareHouseMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_WareHouseMaster = {
                'ID': 0, 'WareHouseName': '', 'Prefix': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'NoOfCategories': 0
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_WareHouseMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Inv_WareHouseMaster = data;
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("WareHouseDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('WareHouseDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            

        });

        $scope.$on('init_WareHouseDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
            $scope.ProductTypeCategoryList = itm.Otherdata === null ? [] : itm.Otherdata.ProductTypeCategoryList;
        });


        init_Operations($scope, $http,
            '/Inventory/SetUp/WareHouseDetailLoad', //--v_Load
            '/Inventory/SetUp/WareHouseDetailGet', // getrow
            '/Inventory/SetUp/WareHouseDetailPost' // PostRow
        );


        $scope.tbl_Inv_WareHouseDetail = {
            'ID': 0, 'FK_tbl_Inv_WareHouseMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductType_Category_ID': null, 'FK_tbl_Inv_ProductType_Category_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_WareHouseDetails = [$scope.tbl_Inv_WareHouseDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_WareHouseDetail = {
                'ID': 0, 'FK_tbl_Inv_WareHouseMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductType_Category_ID': null, 'FK_tbl_Inv_ProductType_Category_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_WareHouseDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_WareHouseDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    