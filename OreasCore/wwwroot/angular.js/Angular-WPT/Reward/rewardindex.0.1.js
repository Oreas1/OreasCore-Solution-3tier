MainModule
    .controller("RewardIndexCtlr", function ($scope, $http) {
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
            '/WPT/Reward/RewardMasterLoad', //--v_Load
            '/WPT/Reward/RewardMasterGet', // getrow
            '/WPT/Reward/RewardMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Reward/GetInitializedReward');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'RewardIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'RewardIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'RewardIndexCtlr').WildCard, null, null, null);
                if (data.find(o => o.Controller === 'RewardIndexCtlr').Otherdata === null) {
                    $scope.RewardTypeList = [];
                    $scope.DesignationList = [];
                    $scope.DepartmentList = [];
                }
                else {
                    $scope.RewardTypeList = data.find(o => o.Controller === 'RewardIndexCtlr').Otherdata.RewardTypeList;
                    $scope.DesignationList = data.find(o => o.Controller === 'RewardIndexCtlr').Otherdata.DesignationList;
                    $scope.DepartmentList = data.find(o => o.Controller === 'RewardIndexCtlr').Otherdata.DepartmentList;
                }

                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'RewardDetailEmployeeCtlr') != undefined) {
                $scope.$broadcast('init_RewardDetailEmployeeCtlr', data.find(o => o.Controller === 'RewardDetailEmployeeCtlr'));
            }
            if (data.find(o => o.Controller === 'RewardDetailPaymentCtlr') != undefined) {
                $scope.$broadcast('init_RewardDetailPaymentCtlr', data.find(o => o.Controller === 'RewardDetailPaymentCtlr'));
            }
            if (data.find(o => o.Controller === 'RewardDetailPaymentEmployeeCtlr') != undefined) {
                $scope.$broadcast('init_RewardDetailPaymentEmployeeCtlr', data.find(o => o.Controller === 'RewardDetailPaymentEmployeeCtlr'));
            }
        };

        init_EmployeeSearchModalGeneral($scope, $http);
        init_MonthSearchModal($scope, $http);

        $scope.MonthSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_RewardMaster.FK_tbl_WPT_CalendarYear_Months_ID = item.ID;
                $scope.tbl_WPT_RewardMaster.FK_tbl_WPT_CalendarYear_Months_IDName = item.MonthStart + ' TO ' + item.MonthEnd;
            }
            else {
                $scope.tbl_WPT_RewardMaster.FK_tbl_WPT_CalendarYear_Months_ID = null;
                $scope.tbl_WPT_RewardMaster.FK_tbl_WPT_CalendarYear_Months_IDName = null;
            }

        };
        $scope.tbl_WPT_RewardMaster = {
            'ID': 0, 'FK_tbl_WPT_CalendarYear_Months_ID': null, 'FK_tbl_WPT_CalendarYear_Months_IDName': '',
            'FK_tbl_WPT_RewardType_ID': null, 'FK_tbl_WPT_RewardType_IDName': '', 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_RewardMasters = [$scope.tbl_WPT_RewardMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_RewardMaster = {
                'ID': 0, 'FK_tbl_WPT_CalendarYear_Months_ID': null, 'FK_tbl_WPT_CalendarYear_Months_IDName': '',
                'FK_tbl_WPT_RewardType_ID': null, 'FK_tbl_WPT_RewardType_IDName': '', 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_RewardMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_WPT_RewardMaster = data;
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("RewardDetailEmployeeCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('RewardDetailEmployeeCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_RewardDetailEmployeeCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);  
            init_Report($scope, itm.Reports, '/WPT/Reward/GetReport');
        });

        init_Operations($scope, $http,
            '/WPT/Reward/RewardDetailEmployeeLoad', //--v_Load
            '/WPT/Reward/RewardDetailEmployeeGet', // getrow
            '/WPT/Reward/RewardDetailEmployeePost' // PostRow
        );

        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_RewardDetail.FK_tbl_WPT_Employee_ID = item.ID;
                $scope.tbl_WPT_RewardDetail.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_RewardDetail.FK_tbl_WPT_Employee_ID = null;
                $scope.tbl_WPT_RewardDetail.FK_tbl_WPT_Employee_IDName = null;
            }
        };        

        $scope.tbl_WPT_RewardDetail = {
            'ID': 0, 'FK_tbl_WPT_RewardMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '', 'RewardAmount': 1, 'WithSalary': true,
            'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_RewardDetails = [$scope.tbl_WPT_RewardDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_RewardDetail = {
                'ID': 0, 'FK_tbl_WPT_RewardMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Employee_ID': 0, 'FK_tbl_WPT_Employee_IDName': '', 'RewardAmount': 1, 'WithSalary': true,
                'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
            if ($scope.AddBulk === true) {
                
                $scope.FK_tbl_WPT_Designation_ID = null;
                $scope.FK_tbl_WPT_Department_ID = null;
                $scope.JoiningDateTill = new Date();
            }
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText, MasterID: $scope.MasterObject.ID, DesignationID: $scope.FK_tbl_WPT_Designation_ID, DepartmentID: $scope.FK_tbl_WPT_Department_ID, JoiningDate: new Date($scope.JoiningDateTill).toLocaleString('en-US') }, data: $scope.tbl_WPT_RewardDetail };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_RewardDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        //-----------------------Excel Upload----------------------//
        $scope.LoadFileData = function (files) {
            var formData = new FormData();
            formData.append("RewardExcelFile", files[0]);

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
                method: "POST", url: "/WPT/Reward/RewardDetailEmployeeUploadExcelFile", params: { MasterID: $scope.MasterObject.ID, operation: 'Save New' }, data: formData, headers: { 'Content-Type': undefined, 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }, transformRequest: angular.identity
            }).then(successcallback, errorcallback);
        };

    })
    .controller("RewardDetailPaymentCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('RewardDetailPaymentCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_RewardDetailPaymentCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            $scope.CompanyBankAcList = itm.Otherdata === null ? [] : itm.Otherdata.CompanyBankAcList;
            $scope.TransactionModeList = itm.Otherdata === null ? [] : itm.Otherdata.TransactionModeList;
        });

        init_Operations($scope, $http,
            '/WPT/Reward/RewardDetailPaymentLoad', //--v_Load
            '/WPT/Reward/RewardDetailPaymentGet', // getrow
            '/WPT/Reward/RewardDetailPaymentPost' // PostRow
        );


        $scope.tbl_WPT_RewardDetail_Payment = {
            'ID': 0, 'FK_tbl_WPT_RewardMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_CompanyBankDetail_ID': null, 'FK_tbl_WPT_CompanyBankDetail_IDName': '',
            'FK_tbl_WPT_TransactionMode_ID': null, 'FK_tbl_WPT_TransactionMode_IDName': '', 'InstrumentNo': '', 'TransactionDate': new Date(), 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'Amount': 0
        };


        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_RewardDetail_Payments = [$scope.tbl_WPT_RewardDetail_Payment];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_RewardDetail_Payment = {
                'ID': 0, 'FK_tbl_WPT_RewardMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_CompanyBankDetail_ID': null, 'FK_tbl_WPT_CompanyBankDetail_IDName': '',
                'FK_tbl_WPT_TransactionMode_ID': null, 'FK_tbl_WPT_TransactionMode_IDName': '', 'InstrumentNo': '', 'TransactionDate': new Date(), 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'Amount': 0
            };

        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_RewardDetail_Payment };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_RewardDetail_Payment = data;
            $scope.tbl_WPT_RewardDetail_Payment.TransactionDate = new Date(data.TransactionDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("RewardDetailPaymentEmployeeCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('RewardDetailPaymentEmployeeCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_RewardDetailPaymentEmployeeCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/WPT/Reward/GetReport');
        });

        init_Operations($scope, $http,
            '/WPT/Reward/RewardDetailPaymentEmployeeLoad', //--v_Load
            '/WPT/Reward/RewardDetailPaymentEmployeeGet', // getrow
            '/WPT/Reward/RewardDetailPaymentEmployeePost' // PostRow
        );
        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_RewardDetail.ID = item.ID;
                $scope.tbl_WPT_RewardDetail.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_RewardDetail.ID = 0;
                $scope.tbl_WPT_RewardDetail.FK_tbl_WPT_Employee_IDName = '';
            }
        };

        $scope.tbl_WPT_RewardDetail = {
            'ID': 0, 'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
            'FK_tbl_WPT_RewardDetail_Payment_ID': null,
            'RewardAmount': 0, 'WithSalary': false,
            'FK_tbl_WPT_EmployeeBankDetail_ID': null, 'FK_tbl_WPT_EmployeeBankDetail_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_RewardDetails = [$scope.tbl_WPT_RewardDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_RewardDetail = {
                'ID': 0, 'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
                'FK_tbl_WPT_RewardDetail_Payment_ID': null,
                'RewardAmount': 0, 'WithSalary': false,
                'FK_tbl_WPT_EmployeeBankDetail_ID': null, 'FK_tbl_WPT_EmployeeBankDetail_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
            $scope.FK_tbl_WPT_Designation_ID = null;
            $scope.FK_tbl_WPT_Department_ID = null;
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText, tbl_WPT_RewardDetailID: $scope.tbl_WPT_RewardDetail.ID, RewardPaymentID: $scope.MasterObject.ID, DesignationID: $scope.FK_tbl_WPT_Designation_ID, DepartmentID: $scope.FK_tbl_WPT_Department_ID }, data: $scope.tbl_WPT_RewardDetail };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_RewardDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    