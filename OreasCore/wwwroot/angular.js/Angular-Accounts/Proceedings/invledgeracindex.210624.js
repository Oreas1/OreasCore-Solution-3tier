MainModule
    .controller("InvLedgerAcIndexCtlr", function ($scope,$window, $http) {
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

        $scope.ParaProdID = null; $scope.ParaProdName = null; $scope.ParaProdUnit = null; $scope.ParaWareHouseID = null; $scope.ParaWareHouseName = null;
        $scope.ParaDateFrom = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate(), 0, 0, 0, 0);
        $scope.ParaDateTill = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate(), 23, 59, 59, 999);
       

        //////////////////////////////entry panel/////////////////////////
        init_Operations($scope, $http,
            '/Accounts/Proceedings/InvLedgerLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/Proceedings/GetInitializedInvLedger');

        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'InvLedgerAcIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'InvLedgerAcIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'InvLedgerAcIndexCtlr').WildCard, null, null, null);    
                init_Report($scope, data.find(o => o.Controller === 'InvLedgerAcIndexCtlr').Reports, '/Accounts/Proceedings/GetInvLedgerReport'); 
                // $scope.pageNavigation('first');              
            }
        };

        init_ProductSearchModalGeneral($scope, $http);
        init_WHMSearchModalGeneral($scope, $http);

        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.ParaWareHouseID = item.ID;
                $scope.ParaWareHouseName = item.WareHouseName;
            }
            else {
                $scope.ParaWareHouseID = null;
                $scope.ParaWareHouseName = null;
            }
        };
        $scope.rptID = 0;
        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.ParaProdID = item.ID;
                $scope.ParaProdName = item.ProductName;
                $scope.ParaProdUnit = item.MeasurementUnit;
                $scope.rptID = item.ID;
            }
            else {

                $scope.ParaProdID = null;
                $scope.ParaProdName = null;
                $scope.ParaProdUnit = null;
                $scope.rptID = 0;
            }
        };
        
        

        $scope.tbl_Inv_Ledger = {
            'ID': 0, 'FK_tbl_Inv_ProductRegistrationDetail_ID': $scope.ParaProdID,
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '', 'PostingDate': new Date(),
            'QuantityIn': 0, 'QuantityOut': 0, 'Narration': '', 'PostingNo': 0, 'ReferenceNo': '', 'Ref': null
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_Ledgers = [$scope.tbl_Inv_Ledger];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_Ledger = {
                'ID': 0, 'FK_tbl_Inv_ProductRegistrationDetail_ID': $scope.ParaProdID,
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '', 'PostingDate': new Date(),
                'QuantityIn': 0, 'QuantityOut': 0, 'Narration': '', 'PostingNo': 0, 'Ref': null
            };
        };

        $scope.pageNavigationParameterChanged = function () {
            $scope.ng_entryPanelHide = true;
            $scope.pageddata.Data.length = null;
            $scope.pageddata.TotalPages = 0;
            $scope.pageddata.CurrentPage = 1;
            $scope._Report_DateFrom = $scope.ParaDateFrom;
            $scope._Report_DateTill = $scope.ParaDateTill;
            
        };
      
        $scope.pageNavigatorParam = function () {
            $scope.FilterValueByDateRangeFrom = new Date($scope.ParaDateFrom).toLocaleString('en-US');
            $scope.FilterValueByDateRangeTill = new Date($scope.ParaDateTill).toLocaleString('en-US');
            return { MasterID: $scope.ParaProdID, WareHouseID: $scope.ParaWareHouseID };
        };      

        $scope.OpenReport = function (name) {
            if (name === 'Ledger' && $scope.ParaProdID > 0)
                $window.open('/Accounts/Proceedings/GetInvLedgerReport?rn=Ledger&id=' + $scope.ParaProdID + "&datefrom=" + new Date($scope.ParaDateFrom).toLocaleString('en-US') + "&datetill=" + new Date($scope.ParaDateTill).toLocaleString('en-US'));
            else if (name === 'Stock')
                $window.open('/Accounts/Proceedings/GetInvLedgerReport?rn=Stock&id=' + $scope.ParaWareHouseID + "&datefrom=" + new Date($scope.ParaDateFrom).toLocaleString('en-US') + "&datetill=" + new Date($scope.ParaDateTill).toLocaleString('en-US'));
        };
       
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    