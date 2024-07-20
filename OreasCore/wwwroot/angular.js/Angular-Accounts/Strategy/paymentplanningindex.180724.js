MainModule
    .controller("PaymentPlanningCtlr", function ($scope, $http) {
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
            '/Accounts/Strategy/PaymentPlanningLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        init_COASearchModalGeneral($scope, $http);      

        init_ViewSetup($scope, $http, '/Accounts/Strategy/GetInitializedPaymentPlanning');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'PaymentPlanningCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'PaymentPlanningCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'PaymentPlanningCtlr').WildCard, null, null, data.find(o => o.Controller === 'PaymentPlanningCtlr').LoadByCard);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'PaymentPlanningMasterCtlr') != undefined) {
                $scope.$broadcast('init_PaymentPlanningMasterCtlr', data.find(o => o.Controller === 'PaymentPlanningMasterCtlr'));
            }
            if (data.find(o => o.Controller === 'PaymentPlanningDetailCtlr') != undefined) {
                $scope.$broadcast('init_PaymentPlanningDetailCtlr', data.find(o => o.Controller === 'PaymentPlanningDetailCtlr'));
            }
        };       
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("PaymentPlanningMasterCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('PaymentPlanningMasterCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.rptID = itm.ID;
            $scope.pageNavigation('first');             
        });

        $scope.$on('init_PaymentPlanningMasterCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);   
            $scope.MonthList = itm.Otherdata === null ? [] : itm.Otherdata.MonthList; 
        });

        init_Operations($scope, $http,
            '/Accounts/Strategy/PaymentPlanningMasterLoad', //--v_Load
            '/Accounts/Strategy/PaymentPlanningMasterGet', // getrow
            '/Accounts/Strategy/PaymentPlanningMasterPost' // PostRow
        );

        $scope.tbl_Ac_PaymentPlanningMaster = {
            'ID': 0, 'FK_tbl_Ac_FiscalYear_ID': $scope.MasterObject.ID, 'MonthNo': null, 'MonthStart': null, 'MonthEnd': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_PaymentPlanningMasters = [$scope.tbl_Ac_PaymentPlanningMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_PaymentPlanningMaster = {
                'ID': 0, 'FK_tbl_Ac_FiscalYear_ID': $scope.MasterObject.ID, 'MonthNo': null, 'MonthStart': null, 'MonthEnd': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_PaymentPlanningMaster }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_PaymentPlanningMaster = data;
            $scope.tbl_Ac_PaymentPlanningMaster.MonthStart = new Date(data.MonthStart);
            $scope.tbl_Ac_PaymentPlanningMaster.MonthEnd = new Date(data.MonthEnd);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; }; 

        //------------------Month selection-----------//
        $scope.setMonthDates= function () {
            let start = new Date($scope.MasterObject.PeriodStart);
            let end = new Date($scope.MasterObject.PeriodEnd);
            let monthStart, monthEnd;

            // Loop through months to find the required month
            while (start <= end) {
                if ((start.getMonth() + 1) === $scope.tbl_Ac_PaymentPlanningMaster.MonthNo) {
                    $scope.tbl_Ac_PaymentPlanningMaster.MonthStart = new Date(start.getFullYear(), start.getMonth(), 1);
                    $scope.tbl_Ac_PaymentPlanningMaster.MonthEnd = new Date(start.getFullYear(), start.getMonth() + 1, 0,23,59,59,990);
                    break;
                }
                start.setMonth(start.getMonth() + 1);
            }
        }

    })
    .controller("PaymentPlanningDetailCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('PaymentPlanningDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.rptID = itm.ID;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_PaymentPlanningDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/Accounts/Strategy/GetPaymentPlanningReport'); 
        });

        init_Operations($scope, $http,
            '/Accounts/Strategy/PaymentPlanningDetailLoad', //--v_Load
            '/Accounts/Strategy/PaymentPlanningDetailGet', // getrow
            '/Accounts/Strategy/PaymentPlanningDetailPost' // PostRow
        );

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Ac_PaymentPlanningDetail.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                $scope.tbl_Ac_PaymentPlanningDetail.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;
                
            }
            else {
                $scope.tbl_Ac_PaymentPlanningDetail.FK_tbl_Ac_ChartOfAccounts_ID = null;
                $scope.tbl_Ac_PaymentPlanningDetail.FK_tbl_Ac_ChartOfAccounts_IDName = null;
            }
            $scope.GetOutStanding(item.ID);
        };

        $scope.tbl_Ac_PaymentPlanningDetail = {
            'ID': 0, 'FK_tbl_Ac_PaymentPlanningMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
            'Amount': 0, 'Restricted': true, 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_PaymentPlanningDetails = [$scope.tbl_Ac_PaymentPlanningDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Ac_PaymentPlanningDetail = {
                'ID': 0, 'FK_tbl_Ac_PaymentPlanningMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
                'Amount': 0, 'Restricted': true, 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
            $scope.outStanding = null;
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_PaymentPlanningDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_PaymentPlanningDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; }; 

        $scope.GetOutStanding = function (acID) {
            $scope.outStanding = null;
            setOperationMessage('Please Wait while Loading Outstanding...', 0);
            var successcallback = function (response) {
                $scope.outStanding = response.data;
            };
            var errorcallback = function (error) { console.log(error) };
            $http({ method: "GET", url: "/Accounts/Strategy/PaymentPlanningDetailOutStandingGet", params: { AcID: acID, MasterID: $scope.MasterObject.ID }, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

        };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    