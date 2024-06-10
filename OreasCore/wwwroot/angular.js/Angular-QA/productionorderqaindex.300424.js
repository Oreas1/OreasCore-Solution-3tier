MainModule
    .controller("ProductionOrderCtlr", function ($scope, $http) {
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
            '/QA/ProductionOrder/BMRBPRMasterLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/QA/ProductionOrder/GetInitializedProductionOrder');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'ProductionOrderCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'ProductionOrderCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'ProductionOrderCtlr').WildCard, null, null, data.find(o => o.Controller === 'ProductionOrderCtlr').LoadByCard);
                $scope.FilterByLoad = 'byUnProcessed';
                $scope.pageNavigation('first');
            }

            if (data.find(o => o.Controller === 'ProductionOrderBatchCtlr') != undefined) {
                $scope.$broadcast('init_ProductionOrderBatchCtlr', data.find(o => o.Controller === 'ProductionOrderBatchCtlr'));
            }
        };

        $scope.tbl_Pro_BatchMaterialRequisitionMaster = {
            'ID': 0, 'AccountName': '', 'OrderNo': '', 'OrderDate': new Date(), 'Priority': '',
            'ProductName': '', 'MeasurementUnit': '', 'Quantity': 0, 'ManufacturingQty': 0, 'SoldQty': 0, 
            'RequestedBatchSize': 0, 'RequestedBatchNo': '', 'RequestedMfgDate': new Date(),
            'RequestedBy': '', 'RequestedDate': '', 'ProcessedBy': '', 'ProcessedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_BatchMaterialRequisitionMasters = [$scope.tbl_Pro_BatchMaterialRequisitionMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_BatchMaterialRequisitionMaster = {
                'ID': 0, 'AccountName': '', 'OrderNo': '', 'OrderDate': new Date(), 'Priority': '',
                'ProductName': '', 'MeasurementUnit': '', 'Quantity': 0, 'ManufacturingQty': 0, 'SoldQty': 0,
                'RequestedBatchSize': 0, 'RequestedBatchNo': '', 'RequestedMfgDate': new Date(),
                'RequestedBy': '', 'RequestedDate': '', 'ProcessedBy': '', 'ProcessedDate': ''
            };
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("ProductionOrderBatchCtlr", function ($scope, $http, $window) {
        $scope.MasterObject = {};
        $scope.$on('ProductionOrderBatchCtlr', function (e, itm) {
            $scope.MasterObject = itm;

            $scope.BMRAvailabilityData = [];
            $scope.MaxRawBatchSizeObj = null;
            $scope.MaxPackagingBatchSizeObj = null;
            $scope.BtnHide = false;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_ProductionOrderBatchCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/QA/ProductionOrder/ProductionOrderBatchLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );
        $scope.pageNavigatorParam = function () { return { BatchNo: $scope.MasterObject.RequestedBatchNo }; };

        $scope.LoadAvailability = function () {

            var successcallback = function (response) {

                $scope.BMRAvailabilityData = [];
                $scope.MaxRawBatchSizeObj = null;
                $scope.MaxPackagingBatchSizeObj = null;

                response.data.forEach((item, index) => {
                    item['MaxUnits'] = Math.trunc((item.AvailableQty * item.FormulaBatchSize) / (item.FormulaQty * item.FormulaPackSize));
                    $scope.BMRAvailabilityData.push(item);

                    if (item.FormulaFor === 'r' && $scope.MaxRawBatchSizeObj === null)
                        $scope.MaxRawBatchSizeObj = item;
                    else if (item.FormulaFor === 'r' && $scope.MaxRawBatchSizeObj.MaxUnits > item.MaxUnits)
                        $scope.MaxRawBatchSizeObj = item;
                    if (item.FormulaFor === 'p' && $scope.MaxPackagingBatchSizeObj === null)
                        $scope.MaxPackagingBatchSizeObj = item;
                    else if (item.FormulaFor === 'p' && $scope.MaxPackagingBatchSizeObj.MaxUnits > item.MaxUnits)
                        $scope.MaxPackagingBatchSizeObj = item;

                });

            };
            var errorcallback = function (error) {
                console.log('BMR Availability Error: ', error);
            };

            $http({ method: "GET", url: '/Production/BMR/BMRAvailabilityGet?id=' + $scope.MasterObject.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID, async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
        };

        $scope.GenerateBMR = function (SPOpt) {

            var successcallback = function (response) {
                if (response.data === 'OK') {   

                    $scope.BtnHide = true;
                    setTimeout(function () {
                        $window.open('/QA/BMR/BMRIndex?byBatchNo=' + $scope.MasterObject.RequestedBatchNo,'_blank');
                    }, 1000);  
                    
                }
                else {
                    alert(response.data);
                }
            };
            var errorcallback = function (error) { console.log('Post error', error); };

            if (confirm("Are you sure! you want to Generate BMR") === true) {
                $http({
                    method: "POST", url: '/QA/ProductionOrder/GenerateBMR', async: true,
                    params: { operation: 'Save New', 'SPOperation': SPOpt, 'FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID': $scope.MasterObject.ID }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
                }).then(successcallback, errorcallback);
            }


        };
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });
