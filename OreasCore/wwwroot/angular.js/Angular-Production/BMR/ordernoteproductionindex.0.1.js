MainModule
    .controller("OrderNoteProductionCtlr", function ($scope, $http) {
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
            '/Production/BMR/OrderNoteProductionLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/Production/BMR/GetInitializedOrderNoteProduction');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'OrderNoteProductionCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'OrderNoteProductionCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'OrderNoteProductionCtlr').WildCard, null, null, data.find(o => o.Controller === 'OrderNoteProductionCtlr').LoadByCard);
                $scope.FilterByLoad = 'byManufacturingPending';
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'OrderNoteProductionBMRDetailCtlr') != undefined) {
                $scope.$broadcast('init_OrderNoteProductionBMRDetailCtlr', data.find(o => o.Controller === 'OrderNoteProductionBMRDetailCtlr'));
            }
            if (data.find(o => o.Controller === 'OrderNoteProductionOrderCtlr') != undefined) {
                $scope.$broadcast('init_OrderNoteProductionOrderCtlr', data.find(o => o.Controller === 'OrderNoteProductionOrderCtlr'));
            }
        };
        
        $scope.tbl_Inv_OrderNoteDetail = {
            'ID': 0, 'FK_tbl_Inv_OrderNoteMaster_ID': null,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID': null, 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_IDName': '', 
            'Quantity': 0, 'CustomMRP': 0, 'FK_AspNetOreasPriority_ID': null, 'FK_AspNetOreasPriority_IDName': '', 'Remarks': '',
            'MfgQtyLimit': 0, 'ManufacturingQty': 0, 'SoldQty': 0, 'ClosedTrue_OpenFalse': false, 'CustomerName': '', 'DocNo': '', 'TargetDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_OrderNoteMasters = [$scope.tbl_Inv_OrderNoteMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_OrderNoteDetail = {
                'ID': 0, 'FK_tbl_Inv_OrderNoteMaster_ID': null,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID': null, 'FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_IDName': '',
                'Quantity': 0, 'CustomMRP': 0, 'FK_AspNetOreasPriority_ID': null, 'FK_AspNetOreasPriority_IDName': '', 'Remarks': '',
                'MfgQtyLimit': 0, 'ManufacturingQty': 0, 'SoldQty': 0, 'ClosedTrue_OpenFalse': false, 'CustomerName': '', 'DocNo': '', 'TargetDate': ''
            };
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

       
    })
    .controller("OrderNoteProductionOrderCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('OrderNoteProductionOrderCtlr', function (e, itm) {
            $scope.BMRAvailabilityData = [];
            $scope.MaxRawBatchSizeObj = null;
            $scope.MaxPackagingBatchSizeObj = null;

            $scope.MasterObject = itm;    
            $scope.pageNavigation('first');
        });

        $scope.$on('init_OrderNoteProductionOrderCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/Production/BMR/OrderNoteProductionOrderLoad', //--v_Load
            '/Production/BMR/OrderNoteProductionOrderGet', // getrow
            '/Production/BMR/OrderNoteProductionOrderPost' // PostRow
        );

        $scope.tbl_Inv_OrderNoteDetail_ProductionOrder = {
            'ID': 0, 'FK_tbl_Inv_OrderNoteDetail_ID': $scope.MasterObject.ID, 'RequestedBatchSize': 1,
            'RequestedBatchNo': '#', 'RequestedMfgDate': new Date(),
            'RequestedBy': '', 'RequestedDate': null, 'ProcessedBy': '', 'ProcessedDate': null
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_OrderNoteDetail_ProductionOrders = [$scope.tbl_Inv_OrderNoteDetail_ProductionOrder];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_OrderNoteDetail_ProductionOrder = {
                'ID': 0, 'FK_tbl_Inv_OrderNoteDetail_ID': $scope.MasterObject.ID, 'RequestedBatchSize': 1,
                'RequestedBatchNo': '#', 'RequestedMfgDate': new Date(),
                'RequestedBy': '', 'RequestedDate': null, 'ProcessedBy': '', 'ProcessedDate': null
            };
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };


        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_OrderNoteDetail_ProductionOrder };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Inv_OrderNoteDetail_ProductionOrder = data;
            $scope.tbl_Inv_OrderNoteDetail_ProductionOrder.RequestedMfgDate = new Date(data.RequestedMfgDate);
        };

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

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    