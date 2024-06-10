MainModule
    .controller("LoanIndividualIndexCtlr", function ($scope, $http) {
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

        $scope.ParaEmpID = null; $scope.ParaEmpName = null;
     
        $scope.ParaDateTill = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate(), 23, 59, 59, 0);
        $scope.ParaLoanTypeID = null;

        //////////////////////////////entry panel/////////////////////////
        init_Operations($scope, $http,
            '/WPT/Loan/LoanIndividualLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Loan/GetInitializedLoanIndividual');

        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'LoanIndividualIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'LoanIndividualIndexCtlr').Privilege;    
                init_Report($scope, data.find(o => o.Controller === 'LoanIndividualIndexCtlr').Reports, '/WPT/Loan/GetReport');
            }
            if (data.find(o => o.Controller === 'LoanIndividualIndexCtlr').Otherdata === null) {
                $scope.LoanTypeList = [];
                $scope.LoadByCardList = [];
            }
            else {
                $scope.LoanTypeList = data.find(o => o.Controller === 'LoanIndividualIndexCtlr').Otherdata.LoanTypeList;
                $scope.LoadByCardList = data.find(o => o.Controller === 'LoanIndividualIndexCtlr').LoadByCard;
            }
        };

        init_EmployeeSearchModalGeneral($scope, $http);

        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.ParaEmpID = item.ID;
                $scope.ParaEmpName = '[' + item.ATEnrollmentNo_Default + '] ' + item.EmployeeName;
                $scope.rptID = item.ID;
            }
            else {
                $scope.ParaEmpID = null;
                $scope.ParaEmpName = null;
                $scope.rptID = 0;
            }
        };

        $scope.tbl_WPT_LoanDetail = {
            'ID': 0, 'FK_tbl_WPT_LoanMaster_ID': null, 'DocNo': null, 'LoanType': '',
            'Amount': 0, 'Balance': 0, 'InstallmentRate': 0,
            'EffectiveFrom': new Date(), 'ReceivingDate': new Date(), 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        $scope.pageNavigationParameterChanged = function () {
            $scope.pageddata.Data.length = null;
            $scope.pageddata.TotalPages = 0;
            $scope.pageddata.CurrentPage = 1;
        };
      
        $scope.pageNavigatorParam = function () {
            return {
                EmployeeID: $scope.ParaEmpID,
                DateTill: new Date($scope.ParaDateTill).toLocaleString('en-US'),
                LoanTypeID: $scope.ParaLoanTypeID,
                 };
        };      
       
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    