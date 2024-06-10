MainModule
    .controller("AcLedgerIndexCtlr", function ($scope,$window, $http) {
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

        $scope.ParaAcID = null; $scope.ParaAcName = null;     
        $scope.ParaDateFrom = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate(), 0, 0, 0, 0);
        $scope.ParaDateTill = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate(), 23, 59, 59, 999);
       

        //////////////////////////////entry panel/////////////////////////
        init_Operations($scope, $http,
            '/Accounts/Proceedings/AcLedgerLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/Proceedings/GetInitializedAcLedger');

        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'AcLedgerIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'AcLedgerIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'AcLedgerIndexCtlr').WildCard, null, null, null);

                // $scope.pageNavigation('first');              
            }
        };

        init_COASearchModalGeneral($scope, $http);

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.ParaAcID = item.ID;
                $scope.ParaAcName = item.AccountName;
            }
            else {
                $scope.ParaAcID = null;
                $scope.ParaAcName = null;
            }

        };

      
        

        $scope.tbl_Ac_Ledger = {
            'ID': 0, 'FK_tbl_Ac_ChartOfAccounts_ID': $scope.ParaAcID, 'FK_tbl_Ac_ChartOfAccounts_IDName': $scope.ParaAcName,
            'Debit': 0, 'Credit': 0, 'Narration': '', 'Posted': true, 'PostingDate': new Date(), 'PostingNo': 0, 'Ref': null
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_Ledgers = [$scope.tbl_Ac_Ledger];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_Ledger = {
                'ID': 0, 'FK_tbl_Ac_ChartOfAccounts_ID': $scope.ParaAcID, 'FK_tbl_Ac_ChartOfAccounts_IDName': $scope.ParaAcName,
                'Debit': 0, 'Credit': 0, 'Narration': '', 'Posted': true, 'PostingDate': new Date(), 'PostingNo': 0, 'Ref': null
            };  
        };

        $scope.pageNavigationParameterChanged = function () {
            $scope.ng_entryPanelHide = true;
            $scope.pageddata.Data.length = null;
            $scope.pageddata.TotalPages = 0;
            $scope.pageddata.CurrentPage = 1;
        };
      
        $scope.pageNavigatorParam = function () {
            $scope.FilterValueByDateRangeFrom = new Date($scope.ParaDateFrom).toLocaleString('en-US');
            $scope.FilterValueByDateRangeTill = new Date($scope.ParaDateTill).toLocaleString('en-US');
            return { MasterID: $scope.ParaAcID, TStatus: $scope.ParaTStatus };
        }; 

        $scope.OpenReport = function (name) {
            if (name === 'Ledger' && $scope.ParaAcID > 0)
                $window.open('/Accounts/Proceedings/GetAcLedgerReport?rn=Ledger&id=' + $scope.ParaAcID + "&datefrom=" + new Date($scope.ParaDateFrom).toLocaleString('en-US') + "&datetill=" + new Date($scope.ParaDateTill).toLocaleString('en-US'));
            else if (name === 'Trial3')
                $window.open('/Accounts/Proceedings/GetAcLedgerReport?rn=Trial3&datefrom=' + new Date($scope.ParaDateFrom).toLocaleString('en-US') + "&datetill=" + new Date($scope.ParaDateTill).toLocaleString('en-US'));
        };
       
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    