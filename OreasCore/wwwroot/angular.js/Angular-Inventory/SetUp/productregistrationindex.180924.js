MainModule
    .controller("ProductRegistrationMasterCtlr", function ($scope, $http) {
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
            '/Inventory/SetUp/ProductRegistrationMasterLoad', //--v_Load
            '/Inventory/SetUp/ProductRegistrationMasterGet', // getrow
            '/Inventory/SetUp/ProductRegistrationMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Inventory/SetUp/GetInitializedProductRegistration');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'ProductRegistrationMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'ProductRegistrationMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'ProductRegistrationMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'ProductRegistrationMasterCtlr').LoadByCard);
                init_Report($scope, data.find(o => o.Controller === 'ProductRegistrationMasterCtlr').Reports, '/Inventory/SetUp/GetProductRegistrationReport');
                $scope.rptID = 0;

                if (data.find(o => o.Controller === 'ProductRegistrationMasterCtlr').Otherdata === null) {
                    $scope.ProductClassificationList = [];
                }
                else {
                    $scope.ProductClassificationList = data.find(o => o.Controller === 'ProductRegistrationMasterCtlr').Otherdata.ProductClassificationList;
                }
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'ProductRegistrationDetailCtlr') != undefined) {
                $scope.$broadcast('init_ProductRegistrationDetailCtlr', data.find(o => o.Controller === 'ProductRegistrationDetailCtlr'));
            }
        };

        init_ProductSearchModalGeneral($scope, $http);

        $scope.tbl_Inv_ProductRegistrationMaster = {
            'ID': 0, 'FK_tbl_Inv_ProductClassification_ID': null, 'FK_tbl_Inv_ProductClassification_IDName': '',
            'ProductName': null, 'Description': null, 'ControlProcedureNo': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'NoOfUnits': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_ProductRegistrationMasters = [$scope.tbl_Inv_ProductRegistrationMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_ProductRegistrationMaster = {
                'ID': 0, 'FK_tbl_Inv_ProductClassification_ID': null, 'FK_tbl_Inv_ProductClassification_IDName': '',
                'ProductName': null, 'Description': null, 'ControlProcedureNo': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'NoOfUnits': 0
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_ProductRegistrationMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Inv_ProductRegistrationMaster = data;
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

        //-----------------------Excel Upload----------------------//
        $scope.LoadFileData = function (files) {
            var formData = new FormData();
            formData.append("PDExcelFile", files[0]);

            var successcallback = function (response) {
                if (response.data === 'OK') {
                    document.getElementById('UploadExcelFile').value = '';
                    $scope.pageNavigation('first');
                    alert('Successfully Updated');
                }
                else {
                    console.log(response.data);
                }
            };
            var errorcallback = function (error) {
            };

            $http({
                method: "POST", url: "/Inventory/SetUp/ProductRegistrationUploadExcelFile", params: { operation: 'Save New' }, data: formData, headers: { 'Content-Type': undefined, 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }, transformRequest: angular.identity
            }).then(successcallback, errorcallback);
        };
    })
    .controller("ProductRegistrationDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('ProductRegistrationDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.LoadNode();            

        });

        $scope.$on('init_ProductRegistrationDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
            $scope.ProductTypeCategoryList = itm.Otherdata === null ? [] : itm.Otherdata.ProductTypeCategoryList;
            $scope.MeasurementUnitList = itm.Otherdata === null ? [] : itm.Otherdata.MeasurementUnitList;
        });

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_ProductRegistrationDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Inv_ProductRegistrationDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.MeasurementUnit;
            }
            else {
                $scope.tbl_Inv_ProductRegistrationDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Inv_ProductRegistrationDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
            }
            
        };

        init_Operations($scope, $http,
            '/Inventory/SetUp/ProductRegistrationDetailLoad', //--v_Load
            '/Inventory/SetUp/ProductRegistrationDetailGet', // getrow
            '/Inventory/SetUp/ProductRegistrationDetailPost' // PostRow
        );


        $scope.tbl_Inv_ProductRegistrationDetail = {
            'ID': 0, 'FK_tbl_Inv_ProductRegistrationMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductType_Category_ID': null, 'FK_tbl_Inv_ProductType_Category_IDName': '', 'Description': '',
            'ProductCode': '', 'FK_tbl_Inv_MeasurementUnit_ID': null, 'FK_tbl_Inv_MeasurementUnit_IDName': '', 'ReorderLevel': 0, 'ReorderAlert': false,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '',
            'Split_Into': 1, 'IsInventory': true, 'IsDiscontinue': false,
            'HarmonizedCode': null, 'GTINCode': null, 'StandardMRP': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'ChildCount': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_ProductRegistrationDetails = [$scope.tbl_Inv_ProductRegistrationDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_ProductRegistrationDetail = {
                'ID': 0, 'FK_tbl_Inv_ProductRegistrationMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductType_Category_ID': null, 'FK_tbl_Inv_ProductType_Category_IDName': '', 'Description': '',
                'ProductCode': '', 'FK_tbl_Inv_MeasurementUnit_ID': null, 'FK_tbl_Inv_MeasurementUnit_IDName': '', 'ReorderLevel': 0, 'ReorderAlert': false,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '',
                'Split_Into': 1, 'IsInventory': true, 'IsDiscontinue': false,
                'HarmonizedCode': null, 'GTINCode': null, 'StandardMRP': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'ChildCount': 0
            };

        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_ProductRegistrationDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_ProductRegistrationDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        ////////////////////////////////////////-------------tree view----------------------/////////////////////////////////////////////////////

        $scope.SelectedNode = '';
        $scope.nodedata = [{
            'sign': '+', 'ID': null, 'Description': '', 'Unit': '', 'Split_Into': 1, 'ParentID': null, 'ChildCount': 0, 'spacing': '', 'IsParent': false }];

        $scope.LoadNode = function () {
            setOperationMessage('Please Wait while Loading Tree...', 0);
            var successcallback = function (response) {
                $scope.nodedata.length = 0;
                $scope.nodedata = response.data;
            };
            var errorcallback = function (error) { };
            $http({ method: "GET", url: "/Inventory/SetUp/GetNodes", params: { PID: 0, MasterID: $scope.MasterObject.ID }, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

        };

        $scope.range = function (n) {
            return new Array(n);
        };

        $scope.getchild = function (Value_ID) {
            var index = $scope.nodedata.findIndex(x => x.ID === Value_ID);


            var successcallback = function (response) {

                for (var i = 0; i < response.data.length; i++) {
                    $scope.nodedata.splice(index + i + 1, 0, response.data[i]);
                    $scope.nodedata[index + i + 1].spacing = $scope.nodedata[index].spacing + '_';
                    if ($scope.nodedata[index + i + 1].ChildCount > 0)
                        $scope.nodedata[index + i + 1].sign = '+';
                    else
                        $scope.nodedata[index + i + 1].sign = '-';
                    $scope.nodedata[index].sign = "-";
                }
            };
            var errorcallback = function (error) { };

            if ($scope.nodedata[index].ChildCount > 0 && $scope.nodedata.findIndex(x => x.ParentID === Value_ID) === -1) {
                $http({ method: "GET", url: "/Inventory/SetUp/GetNodes", params: { PID: Value_ID, MasterID: $scope.MasterObject.ID }, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
            }
            else {
                setOperationMessage('Please Wait while unloading data...', 0);
                $scope.RemoveChildNotes(Value_ID);
                setOperationMessage('', 0);
            }



        };

        $scope.RemoveChildNotes = function (Value_ID) {

            var index = $scope.nodedata.findIndex(x => x.ID === Value_ID);
            if ($scope.nodedata[index].ChildCount > 0)
                $scope.nodedata[index].sign = '+';
            else
                $scope.nodedata[index].sign = '-';
            for (var i = index + 1; i < $scope.nodedata.length; i++) {
                if ($scope.nodedata.findIndex(x => x.ParentID === $scope.nodedata[i].ID) > 0 && $scope.nodedata[i].ParentID === Value_ID) {
                    $scope.RemoveChildNotes($scope.nodedata[i].ID);
                    i = i - 1;
                }
                else {
                    if ($scope.nodedata[i].ParentID === Value_ID) {
                        $scope.nodedata.splice(i, 1);
                        i = i - 1;
                    }

                }
            }
        };

        $scope.SelectFromTree = function (Value_ID) {
            $scope.GetRow(Value_ID, 'Edit');
        };

      //////////////////////////////end/////////////////////////

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    