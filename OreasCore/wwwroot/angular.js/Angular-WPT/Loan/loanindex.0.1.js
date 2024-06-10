MainModule
    .controller("LoanIndexCtlr", function ($scope, $http) {
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
            '/WPT/Loan/LoanMasterLoad', //--v_Load
            '/WPT/Loan/LoanMasterGet', // getrow
            '/WPT/Loan/LoanMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Loan/GetInitializedLoan');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'LoanIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'LoanIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'LoanIndexCtlr').WildCard, null, null, data.find(o => o.Controller === 'LoanIndexCtlr').LoadByCard);
                init_Report($scope, data.find(o => o.Controller === 'LoanIndexCtlr').Reports, '/WPT/Loan/GetReport');

                if (data.find(o => o.Controller === 'LoanIndexCtlr').Otherdata === null) {
                    $scope.LoanTypeList = [];
                    $scope.DesignationList = [];
                    $scope.DepartmentList = [];
                }
                else {
                    $scope.LoanTypeList = data.find(o => o.Controller === 'LoanIndexCtlr').Otherdata.LoanTypeList;
                    $scope.DesignationList = data.find(o => o.Controller === 'LoanIndexCtlr').Otherdata.DesignationList;
                    $scope.DepartmentList = data.find(o => o.Controller === 'LoanIndexCtlr').Otherdata.DepartmentList;
                }

                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'LoanDetailEmployeeCtlr') != undefined) {
                $scope.$broadcast('init_LoanDetailEmployeeCtlr', data.find(o => o.Controller === 'LoanDetailEmployeeCtlr'));
            }
            if (data.find(o => o.Controller === 'LoanDetailPaymentCtlr') != undefined) {
                $scope.$broadcast('init_LoanDetailPaymentCtlr', data.find(o => o.Controller === 'LoanDetailPaymentCtlr'));
            }
            if (data.find(o => o.Controller === 'LoanDetailPaymentEmployeeCtlr') != undefined) {
                $scope.$broadcast('init_LoanDetailPaymentEmployeeCtlr', data.find(o => o.Controller === 'LoanDetailPaymentEmployeeCtlr'));
            }
        };

        init_EmployeeSearchModalGeneral($scope, $http);


        $scope.tbl_WPT_LoanMaster = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
            'FK_tbl_WPT_LoanType_ID': null, 'FK_tbl_WPT_LoanType_IDName': '',
            'TotalLoan': 0, 'TotalBalance': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_LoanMasters = [$scope.tbl_WPT_LoanMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_LoanMaster = {
                'ID': 0, 'DocNo': '', 'DocDate': new Date(),
                'FK_tbl_WPT_LoanType_ID': null, 'FK_tbl_WPT_LoanType_IDName': '',
                'TotalLoan': 0, 'TotalBalance': 0,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_LoanMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_WPT_LoanMaster = data; $scope.tbl_WPT_LoanMaster.DocDate = new Date(data.DocDate);
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("LoanDetailEmployeeCtlr", function ($scope, $http, $window) {
        
        $scope.MasterObject = {};
        $scope.$on('LoanDetailEmployeeCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_LoanDetailEmployeeCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
            init_Report($scope, itm.Reports, '/WPT/Loan/GetReport');
        });


        init_Operations($scope, $http,
            '/WPT/Loan/LoanDetailEmployeeLoad', //--v_Load
            '/WPT/Loan/LoanDetailEmployeeGet', // getrow
            '/WPT/Loan/LoanDetailEmployeePost' // PostRow
        );

        $scope.GotoReport = function (id) {
            $window.open('/WPT/Loan/GetReport?rn=Loan Detail Individual&id=' + id);
        };

        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_LoanDetail.FK_tbl_WPT_Employee_ID = item.ID;
                $scope.tbl_WPT_LoanDetail.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_LoanDetail.FK_tbl_WPT_Employee_ID = null;
                $scope.tbl_WPT_LoanDetail.FK_tbl_WPT_Employee_IDName = null;
            }
        };        

        $scope.tbl_WPT_LoanDetail = {
            'ID': 0, 'FK_tbl_WPT_LoanMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
            'Amount': 0, 'Balance': 0, 'InstallmentRate': 0,
            'EffectiveFrom': new Date($scope.MasterObject.DocDate), 'ReceivingDate': new Date($scope.MasterObject.DocDate), 'Remarks': '', 'IsCompleted':false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_LoanDetails = [$scope.tbl_WPT_LoanDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_LoanDetail = {
                'ID': 0, 'FK_tbl_WPT_LoanMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
                'Amount': 0, 'Balance': 0, 'InstallmentRate': 0,
                'EffectiveFrom': new Date($scope.MasterObject.DocDate), 'ReceivingDate': new Date($scope.MasterObject.DocDate), 'Remarks': '', 'IsCompleted': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
            if ($scope.AddBulk === true) {                
                $scope.FK_tbl_WPT_Designation_ID = null;
                $scope.FK_tbl_WPT_Department_ID = null;
                $scope.JoiningDateTill = new Date();
            }
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText, MasterID: $scope.MasterObject.ID, DesignationID: $scope.FK_tbl_WPT_Designation_ID, DepartmentID: $scope.FK_tbl_WPT_Department_ID, JoiningDate: new Date($scope.JoiningDateTill).toLocaleString('en-US') }, data: $scope.tbl_WPT_LoanDetail };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_LoanDetail = data;
            $scope.tbl_WPT_LoanDetail.EffectiveFrom = new Date(data.EffectiveFrom);
            $scope.tbl_WPT_LoanDetail.ReceivingDate = new Date(data.ReceivingDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        //-----------------------Excel Upload----------------------//
        $scope.LoadFileData = function (files) {
            var formData = new FormData();
            formData.append("LoanExcelFile", files[0]);

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
                method: "POST", url: "/WPT/Loan/LoanDetailEmployeeUploadExcelFile", params: { MasterID: $scope.MasterObject.ID, operation: 'Save New' }, data: formData, headers: { 'Content-Type': undefined, 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }, transformRequest: angular.identity
            }).then(successcallback, errorcallback);
        };

    })
    .controller("LoanDetailPaymentCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('LoanDetailPaymentCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_LoanDetailPaymentCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            $scope.CompanyBankAcList = itm.Otherdata === null ? [] : itm.Otherdata.CompanyBankAcList;
            $scope.TransactionModeList = itm.Otherdata === null ? [] : itm.Otherdata.TransactionModeList;
        });

        init_Operations($scope, $http,
            '/WPT/Loan/LoanDetailPaymentLoad', //--v_Load
            '/WPT/Loan/LoanDetailPaymentGet', // getrow
            '/WPT/Loan/LoanDetailPaymentPost' // PostRow
        );


        $scope.tbl_WPT_LoanDetail_Payment = {
            'ID': 0, 'FK_tbl_WPT_LoanMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_CompanyBankDetail_ID': null, 'FK_tbl_WPT_CompanyBankDetail_IDName': '',
            'FK_tbl_WPT_TransactionMode_ID': null, 'FK_tbl_WPT_TransactionMode_IDName': '', 'InstrumentNo': '', 'TransactionDate': new Date(), 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'Amount': 0
        };


        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_LoanDetail_Payments = [$scope.tbl_WPT_LoanDetail_Payment];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_LoanDetail_Payment = {
                'ID': 0, 'FK_tbl_WPT_LoanMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_CompanyBankDetail_ID': null, 'FK_tbl_WPT_CompanyBankDetail_IDName': '',
                'FK_tbl_WPT_TransactionMode_ID': null, 'FK_tbl_WPT_TransactionMode_IDName': '', 'InstrumentNo': '', 'TransactionDate': new Date(), 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'Amount': 0
            };

        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_LoanDetail_Payment };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_LoanDetail_Payment = data;
            $scope.tbl_WPT_LoanDetail_Payment.TransactionDate = new Date(data.TransactionDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("LoanDetailPaymentEmployeeCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('LoanDetailPaymentEmployeeCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_LoanDetailPaymentEmployeeCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/WPT/Loan/GetReport');
        });

        init_Operations($scope, $http,
            '/WPT/Loan/LoanDetailPaymentEmployeeLoad', //--v_Load
            '/WPT/Loan/LoanDetailPaymentEmployeeGet', // getrow
            '/WPT/Loan/LoanDetailPaymentEmployeePost' // PostRow
        );
        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_LoanDetail.ID = item.ID;
                $scope.tbl_WPT_LoanDetail.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_LoanDetail.ID = 0;
                $scope.tbl_WPT_LoanDetail.FK_tbl_WPT_Employee_IDName = '';
            }
        };

        $scope.tbl_WPT_LoanDetail = {
            'ID': 0, 'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
            'FK_tbl_WPT_LoanDetail_Payment_ID': null,
            'Amount': 0, 'WithSalary': false,
            'FK_tbl_WPT_EmployeeBankDetail_ID': null, 'FK_tbl_WPT_EmployeeBankDetail_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_LoanDetails = [$scope.tbl_WPT_LoanDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_LoanDetail = {
                'ID': 0, 'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
                'FK_tbl_WPT_LoanDetail_Payment_ID': null,
                'Amount': 0, 'WithSalary': false,
                'FK_tbl_WPT_EmployeeBankDetail_ID': null, 'FK_tbl_WPT_EmployeeBankDetail_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
            $scope.FK_tbl_WPT_Designation_ID = null;
            $scope.FK_tbl_WPT_Department_ID = null;
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText, tbl_WPT_LoanDetailID: $scope.tbl_WPT_LoanDetail.ID, LoanPaymentID: $scope.MasterObject.ID, DesignationID: $scope.FK_tbl_WPT_Designation_ID, DepartmentID: $scope.FK_tbl_WPT_Department_ID }, data: $scope.tbl_WPT_LoanDetail };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_LoanDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    