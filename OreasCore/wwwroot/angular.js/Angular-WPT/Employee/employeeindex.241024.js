MainModule
    .controller("EmployeeIndexCtlr", function ($scope, $window, $http) {
        $scope.DivHideShow = function (v, itm, div_hide, div_show, scope) {
            if (typeof v !== 'undefined' && v !== '' && v !== null) {
                $scope.$broadcast(v, itm);
            }
            if (typeof scope !== 'undefined' && scope !== '' && scope !== null && typeof scope.$parent.pageNavigation === 'function') {
                scope.$parent.pageNavigation('Load');
            }

            $("#" + div_hide).hide('slow');
            $("#" + div_show).show('slow');
        };

        //////////////////////////////entry panel/////////////////////////
        init_Operations($scope, $http,
            '/WPT/Employee/EmployeeLoad', //--v_Load
            '/WPT/Employee/EmployeeGet', // getrow
            '/WPT/Employee/EmployeePost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Employee/GetInitializedEmployee');

        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'EmployeeIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'EmployeeIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'EmployeeIndexCtlr').WildCard, null, null, null);
                init_Report($scope, data.find(o => o.Controller === 'EmployeeIndexCtlr').Reports, '/WPT/Employee/GetReport');

                if (data.find(o => o.Controller === 'EmployeeIndexCtlr').Otherdata === null) {
                    $scope.InActiveTypeList = [];
                    $scope.EmployeeLevelList = [];
                    $scope.EmploymentTypeList = [];
                    $scope.ShiftList = [];
                    $scope.SectionList = [];
                    $scope.DesignationList = [];
                    $scope.EducationalTypeList = [];
                    $scope.ATTypeList = [];
                    $scope.OTPolicyList = [];
                    $scope.TransactionModeList = [];
                    $scope.BankList = [];
                    $scope.EmployeeLetterList = [];
                }
                else {
                    $scope.InActiveTypeList = data.find(o => o.Controller === 'EmployeeIndexCtlr').Otherdata.InActiveTypeList;
                    $scope.EmployeeLevelList = data.find(o => o.Controller === 'EmployeeIndexCtlr').Otherdata.EmployeeLevelList;
                    $scope.EmploymentTypeList = data.find(o => o.Controller === 'EmployeeIndexCtlr').Otherdata.EmploymentTypeList;
                    $scope.ShiftList = data.find(o => o.Controller === 'EmployeeIndexCtlr').Otherdata.ShiftList;
                    $scope.SectionList = data.find(o => o.Controller === 'EmployeeIndexCtlr').Otherdata.SectionList;
                    $scope.DesignationList = data.find(o => o.Controller === 'EmployeeIndexCtlr').Otherdata.DesignationList;
                    $scope.EducationalTypeList = data.find(o => o.Controller === 'EmployeeIndexCtlr').Otherdata.EducationalTypeList;
                    $scope.ATTypeList = data.find(o => o.Controller === 'EmployeeIndexCtlr').Otherdata.ATTypeList;
                    $scope.OTPolicyList = data.find(o => o.Controller === 'EmployeeIndexCtlr').Otherdata.OTPolicyList;
                    $scope.TransactionModeList = data.find(o => o.Controller === 'EmployeeIndexCtlr').Otherdata.TransactionModeList;
                    $scope.BankList = data.find(o => o.Controller === 'EmployeeIndexCtlr').Otherdata.BankList;
                    $scope.EmployeeLetterList = data.find(o => o.Controller === 'EmployeeIndexCtlr').Otherdata.EmployeeLetterList;
                }

                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'EmployeeFFCPTemplateCtlr') != undefined) {
                $scope.$broadcast('init_EmployeeFFCPTemplateCtlr', data.find(o => o.Controller === 'EmployeeFFCPTemplateCtlr'));
            }
            if (data.find(o => o.Controller === 'EmployeeSalaryCtlr') != undefined) {
                $scope.$broadcast('init_EmployeeSalaryCtlr', data.find(o => o.Controller === 'EmployeeSalaryCtlr'));
            }
        };


        $scope.tbl_WPT_Employee = {
            'ID': 0, 'EmployeeNo': null,
            'FK_tbl_WPT_EmploymentType_ID': null, 'FK_tbl_WPT_EmploymentType_IDName': '',
            'FK_tbl_WPT_DepartmentDetail_Section_ID': null, 'FK_tbl_WPT_DepartmentDetail_Section_IDName': '',
            'FK_tbl_WPT_Designation_ID': null, 'FK_tbl_WPT_Designation_IDName': '',
            'FK_tbl_WPT_EmployeeLevel_ID': null, 'FK_tbl_WPT_EmployeeLevel_IDName': '',
            'FK_tbl_WPT_Shift_ID_Default': null, 'FK_tbl_WPT_Shift_ID_DefaultName': '', 'ATEnrollmentNo_Default': null,
            'FK_tbl_WPT_ATType_ID': 1, 'FK_tbl_WPT_ATType_IDName': '',
            'JoiningDate': new Date(), 'InactiveDate': null, 'FK_tbl_WPT_InActiveType_ID': null, 'FK_tbl_WPT_InActiveType_IDName': '', 'IsPensionActive': false, 'Remarks': '',
            'Name': '', 'FatherORHusbandName': '', 'Gender': null, 'MaritalStatus': null,
            'CNIC': '', 'DateOfBirth': null, 'CellPhoneNo': '', 'HomeAddress': '',
            'Email': '', 'SendEmailAs_OfficialTrue_UnofficialFalse_NoneNull': null, 'BloodGroup': '', 'EmergencyNo': '',
            'FK_tbl_WPT_EducationalLevelType_ID': null, 'FK_tbl_WPT_EducationalLevelType_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 
            'BasicWage': 0, 'OTPolicy': '', 'TotalAllowances': 0, 'TotalDeductibles': 0, 'WageEffective': null, 'TransactionMode': ''
        };

        $scope.tbl_WPT_EmployeeSalaryStructure = {
            'ID': 0, 'FK_tbl_WPT_Employee_ID': $scope.MasterID, 'EffectiveDate': new Date(), 'BasicWage': 1,
            'FK_tbl_WPT_tbl_OTPolicy_ID': $scope.OTPolicyList === undefined ? null : $scope.OTPolicyList[$scope.OTPolicyList.length - 1].ID,
            'FK_tbl_WPT_TransactionMode_ID': null, 'FK_tbl_WPT_TransactionMode_IDName': '', 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
            'TotalAllowances': 0, 'TotalDeductibles': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.VM_EmployeeEnrollment = { 'tbl_WPT_Employee': $scope.tbl_WPT_Employee, 'tbl_WPT_EmployeeSalaryStructure': $scope.tbl_WPT_EmployeeSalaryStructure };

        $scope.clearEntryPanel = function () {

            $('[href="#Main"]').tab('show');
            //rededine to orignal values
            $scope.tbl_WPT_Employee = {
                'ID': 0, 'EmployeeNo': null,
                'FK_tbl_WPT_EmploymentType_ID': null, 'FK_tbl_WPT_EmploymentType_IDName': '',
                'FK_tbl_WPT_DepartmentDetail_Section_ID': null, 'FK_tbl_WPT_DepartmentDetail_Section_IDName': '',
                'FK_tbl_WPT_Designation_ID': null, 'FK_tbl_WPT_Designation_IDName': '',
                'FK_tbl_WPT_EmployeeLevel_ID': null, 'FK_tbl_WPT_EmployeeLevel_IDName': '',
                'FK_tbl_WPT_Shift_ID_Default': null, 'FK_tbl_WPT_Shift_ID_DefaultName': '', 'ATEnrollmentNo_Default': null,
                'FK_tbl_WPT_ATType_ID': 1, 'FK_tbl_WPT_ATType_IDName': '',
                'JoiningDate': new Date(), 'InactiveDate': null, 'FK_tbl_WPT_InActiveType_ID': null, 'FK_tbl_WPT_InActiveType_IDName': '', 'IsPensionActive': false, 'Remarks': '',
                'Name': '', 'FatherORHusbandName': '', 'Gender': null, 'MaritalStatus': null,
                'CNIC': '', 'DateOfBirth': null, 'CellPhoneNo': '', 'HomeAddress': '',
                'Email': '', 'SendEmailAs_OfficialTrue_UnofficialFalse_NoneNull': null, 'BloodGroup': '', 'EmergencyNo': '',
                'FK_tbl_WPT_EducationalLevelType_ID': null, 'FK_tbl_WPT_EducationalLevelType_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 
                'BasicWage': 0, 'OTPolicy': '', 'TotalAllowances': 0, 'TotalDeductibles': 0, 'WageEffective': null, 'TransactionMode': ''
            };

            $scope.tbl_WPT_EmployeeSalaryStructure = {
                'ID': 0, 'FK_tbl_WPT_Employee_ID': $scope.MasterID, 'EffectiveDate': new Date(), 'BasicWage': 1,
                'FK_tbl_WPT_tbl_OTPolicy_ID': $scope.OTPolicyList[$scope.OTPolicyList.length - 1].ID,
                'FK_tbl_WPT_TransactionMode_ID': null, 'FK_tbl_WPT_TransactionMode_IDName': '', 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
                'TotalAllowances': 0, 'TotalDeductibles': 0
            };

            $scope.VM_EmployeeEnrollment = { 'tbl_WPT_Employee': $scope.tbl_WPT_Employee, 'tbl_WPT_EmployeeSalaryStructure': $scope.tbl_WPT_EmployeeSalaryStructure };

        };

        $scope.JoiningDateChange = function () {
            if ($scope.ng_entryPanelSubmitBtnText === 'Save New')
                $scope.tbl_WPT_EmployeeSalaryStructure.EffectiveDate = $scope.tbl_WPT_Employee.JoiningDate;
        };

        $scope.postRowParam = function () {            
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.VM_EmployeeEnrollment };
        };

        $scope.GetRowResponse = function (data, operation) {

            $scope.tbl_WPT_Employee = data.tbl_WPT_Employee;
            $scope.tbl_WPT_Employee.JoiningDate = new Date(data.tbl_WPT_Employee.JoiningDate);

            if (data.tbl_WPT_Employee.InactiveDate !== null) { $scope.tbl_WPT_Employee.InactiveDate = new Date(data.tbl_WPT_Employee.InactiveDate); }
            if (data.tbl_WPT_Employee.DateOfBirth !== null) { $scope.tbl_WPT_Employee.DateOfBirth = new Date(data.tbl_WPT_Employee.DateOfBirth); }

            $scope.tbl_WPT_EmployeeSalaryStructure = data.tbl_WPT_EmployeeSalaryStructure;
            $scope.tbl_WPT_EmployeeSalaryStructure.EffectiveDate = new Date(data.tbl_WPT_EmployeeSalaryStructure.EffectiveDate);

            $scope.VM_EmployeeEnrollment = { 'tbl_WPT_Employee': $scope.tbl_WPT_Employee, 'tbl_WPT_EmployeeSalaryStructure': $scope.tbl_WPT_EmployeeSalaryStructure };

            $scope.EmailSendAsDisabled();
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

        //-----------------------Excel Upload----------------------//
        $scope.LoadFileData = function (files) {
            var formData = new FormData();
            formData.append("EmpExcelFile", files[0]);

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
                method: "POST", url: "/WPT/Employee/EmployeeUploadExcelFile", params: { operation: 'Save New' }, data: formData, headers: { 'Content-Type': undefined, 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }, transformRequest: angular.identity
            }).then(successcallback, errorcallback);
        };

        $scope.SendAsDisabled = true;
        var emailPattern = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;
        $scope.EmailSendAsDisabled = function ()
        {
            if ($scope.tbl_WPT_Employee &&
                typeof $scope.tbl_WPT_Employee.Email === 'string' &&
                $scope.tbl_WPT_Employee.Email.trim() !== '' &&
                emailPattern.test($scope.tbl_WPT_Employee.Email)) 
            {
                $scope.SendAsDisabled = false;
                
            }
            else
            {
                $scope.tbl_WPT_Employee.SendEmailAs_OfficialTrue_UnofficialFalse_NoneNull = null;
                $scope.SendAsDisabled = true;
            }

        };
    })
    .controller("EmployeeFFCPTemplateCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('EmployeeFFCPTemplateCtlr', function (e, itm) {
            $scope.MasterObject = itm;            
            $scope.GetRow($scope.MasterObject.ID, 'View');
            $scope.rptID = itm.ID;
        });

        $scope.RemoveFace = false;
        $scope.RemoveFinger = false;
        $scope.RemovePhoto = false;

        $scope.$on('init_EmployeeFFCPTemplateCtlr', function (e, itm) {
            init_Report($scope, itm.Reports, '/WPT/Employee/GetReport');
        });

        //////////////////////////////entry panel/////////////////////////
        init_Operations($scope, $http,
            '', //--v_Load
            '/WPT/Employee/EmployeeFFCPGet', // getrow
            '/WPT/Employee/EmployeeFFCPPost' // PostRow
        );

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_Employee_PFF = data;
        };       

        $scope.postRowParam = function () {            
            $scope.ng_entryPanelSubmitBtnText = 'Save Update';
            return { validate: true, params: { EmpID: $scope.MasterObject.ID, CardNo: $scope.tbl_WPT_Employee_PFF.CardNumber, Paswd: $scope.tbl_WPT_Employee_PFF.Password, Privilege: $scope.tbl_WPT_Employee_PFF.Privilege, Enabled: $scope.tbl_WPT_Employee_PFF.Enabled, RemoveFace: $scope.RemoveFace, RemoveFinger: $scope.RemoveFinger, RemovePhoto: $scope.RemovePhoto, operation: $scope.ng_entryPanelSubmitBtnText }, data: null };
        };

        $scope.PostRowResponse = function (result) {
            if (result === 'OK') {
                alert('Successfully Updated');
                $scope.RemoveFace = false;
                $scope.RemoveFinger = false;
                $scope.RemovePhoto = false;
                $scope.GetRow($scope.MasterObject.ID, 'View');
            }                
            else {
                console.log(result);
            }
        };
        
        $scope.LoadFileData = function (files) {
            var formData = new FormData();
            formData.append("UserPhoto", files[0]);

            var successcallback = function (response) {
                if (response.data === 'OK') {
                    $scope.GetRow($scope.MasterObject.ID, 'View');
                    document.getElementById('UserPhoto').value = '';
                    alert('Successfully Updated');
                }       
                else {
                    console.log(response.data);
                }
            };
            var errorcallback = function (error) {
            };

            $http({
                method: "POST", url: "/WPT/Employee/SetEmployeePhoto", params: { PhotoTableID: $scope.tbl_WPT_Employee_PFF.ID, EmpID: $scope.tbl_WPT_Employee_PFF.FK_tbl_WPT_Employee_ID, operation: 'Save Update' }, data: formData, headers: { 'Content-Type': undefined, 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }, transformRequest: angular.identity
            }).then(successcallback, errorcallback);
        };

    })
    .controller("EmployeeSalaryCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('EmployeeSalaryCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.MasterID = $scope.MasterObject.ID;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_EmployeeSalaryCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            if (itm.Otherdata === null) {
                $scope.WageCalculationTypeList = [];
                $scope.AllowanceTypeList = [];
                $scope.DeductibleTypeList = [];
            }
            else {
                
                $scope.WageCalculationTypeList = itm.Otherdata.WageCalculationTypeList;
                $scope.AllowanceTypeList = itm.Otherdata.AllowanceTypeList;
                $scope.DeductibleTypeList = itm.Otherdata.DeductibleTypeList;
            }
        });

        init_Operations($scope, $http,
            '/WPT/Employee/EmployeeSalaryLoad', //--v_Load
            '/WPT/Employee/EmployeeSalaryGet', // getrow
            '/WPT/Employee/EmployeeSalaryPost' // PostRow
        );

        $scope.tbl_WPT_EmployeeSalaryStructure = {
            'ID': 0, 'FK_tbl_WPT_Employee_ID': $scope.MasterObject.ID, 'EffectiveDate': new Date(), 'BasicWage': 1,
            'FK_tbl_WPT_tbl_OTPolicy_ID': null, 'FK_tbl_WPT_tbl_OTPolicy_IDName': '',
            'FK_tbl_WPT_TransactionMode_ID': null, 'FK_tbl_WPT_TransactionMode_IDName': '', 'MaxTransactionLimit': 0,
            'FK_tbl_WPT_TransactionMode_ID_Secondary': null, 'FK_tbl_WPT_TransactionMode_ID_SecondaryName': '',
            'Remarks': '', 'FK_tbl_WPT_IncrementDetail_ID': null, 'FK_tbl_WPT_IncrementDetail_IDName':'', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
            'TotalAllowances': 0, 'TotalDeductibles': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_EmployeeSalaryStructures = [$scope.tbl_WPT_EmployeeSalaryStructure];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_EmployeeSalaryStructure = {
                'ID': 0, 'FK_tbl_WPT_Employee_ID': $scope.MasterObject.ID, 'EffectiveDate': new Date(), 'BasicWage': 1,
                'FK_tbl_WPT_tbl_OTPolicy_ID': null, 'FK_tbl_WPT_tbl_OTPolicy_IDName': '',
                'FK_tbl_WPT_TransactionMode_ID': null, 'FK_tbl_WPT_TransactionMode_IDName': '', 'MaxTransactionLimit': 0,
                'FK_tbl_WPT_TransactionMode_ID_Secondary': null, 'FK_tbl_WPT_TransactionMode_ID_SecondaryName': '',
                'Remarks': '', 'FK_tbl_WPT_IncrementDetail_ID': null, 'FK_tbl_WPT_IncrementDetail_IDName': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
                'TotalAllowances': 0, 'TotalDeductibles': 0
            };
        };

        $scope.postRowParam = function () {
            if ($scope.tbl_WPT_EmployeeSalaryStructure.MaxTransactionLimit > 0 && $scope.tbl_WPT_EmployeeSalaryStructure.FK_tbl_WPT_TransactionMode_ID_Secondary === null) {
                alert('Secondary should be selected if Maximum Limit Apply');
                return { validate: false, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_EmployeeSalaryStructure };
            }
            else {
                return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_EmployeeSalaryStructure };
            }
                
            
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_EmployeeSalaryStructure = data;
            $scope.tbl_WPT_EmployeeSalaryStructure.EffectiveDate = new Date(data.EffectiveDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    })
    .controller("EmployeeSalaryAllowanceCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('EmployeeSalaryAllowanceCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.MasterID = $scope.MasterObject.ID;
            $scope.pageNavigation('first');
        });

        init_Operations($scope, $http,
            '/WPT/Employee/EmployeeSalaryAllowanceLoad', //--v_Load
            '/WPT/Employee/EmployeeSalaryAllowanceGet', // getrow
            '/WPT/Employee/EmployeeSalaryAllowancePost' // PostRow
        );

        $scope.tbl_WPT_EmployeeSalaryStructureAllowance = {
            'ID': 0, 'FK_tbl_WPT_EmployeeSalaryStructure_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_AllowanceType_ID': null, 'FK_tbl_WPT_AllowanceType_IDName': '', 'Amount': 0,
            'FK_tbl_WPT_WageCalculationType_ID': null, 'FK_tbl_WPT_WageCalculationType_IDName': '', 'Min_WD_Per': 1, 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_EmployeeSalaryStructureAllowances = [$scope.tbl_WPT_EmployeeSalaryStructureAllowance];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_EmployeeSalaryStructureAllowance = {
                'ID': 0, 'FK_tbl_WPT_EmployeeSalaryStructure_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_AllowanceType_ID': null, 'FK_tbl_WPT_AllowanceType_IDName': '', 'Amount': 0,
                'FK_tbl_WPT_WageCalculationType_ID': null, 'FK_tbl_WPT_WageCalculationType_IDName': '', 'Min_WD_Per': 1, 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_EmployeeSalaryStructureAllowance }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_EmployeeSalaryStructureAllowance = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    })
    .controller("EmployeeSalaryDeductibleCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('EmployeeSalaryDeductibleCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.MasterID = $scope.MasterObject.ID;
            $scope.pageNavigation('first');
        });

        init_Operations($scope, $http,
            '/WPT/Employee/EmployeeSalaryDeductibleLoad', //--v_Load
            '/WPT/Employee/EmployeeSalaryDeductibleGet', // getrow
            '/WPT/Employee/EmployeeSalaryDeductiblePost' // PostRow
        );

        $scope.tbl_WPT_EmployeeSalaryStructureDeductible = {
            'ID': 0, 'FK_tbl_WPT_EmployeeSalaryStructure_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_DeductibleType_ID': null, 'FK_tbl_WPT_DeductibleType_IDName': '', 'Amount': 0,
            'FK_tbl_WPT_WageCalculationType_ID': null, 'FK_tbl_WPT_WageCalculationType_IDName': '', 'Min_WD_Per': 1, 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_EmployeeSalaryStructureDeductibles = [$scope.tbl_WPT_EmployeeSalaryStructureDeductible];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_EmployeeSalaryStructureDeductible = {
                'ID': 0, 'FK_tbl_WPT_EmployeeSalaryStructure_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_DeductibleType_ID': null, 'FK_tbl_WPT_DeductibleType_IDName': '', 'Amount': 0,
                'FK_tbl_WPT_WageCalculationType_ID': null, 'FK_tbl_WPT_WageCalculationType_IDName': '', 'Min_WD_Per': 1, 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_EmployeeSalaryStructureDeductible }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_EmployeeSalaryStructureDeductible = data;

        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    })
    .controller("EmployeePensionCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('EmployeePensionCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.MasterID = $scope.MasterObject.ID;
            $scope.pageNavigation('first');
        });

        init_Operations($scope, $http,
            '/WPT/Employee/EmployeePensionLoad', //--v_Load
            '/WPT/Employee/EmployeePensionGet', // getrow
            '/WPT/Employee/EmployeePensionPost' // PostRow
        );

        $scope.tbl_WPT_EmployeePensionStructure = {
            'ID': 0, 'FK_tbl_WPT_Employee_ID': $scope.MasterObject.ID, 'EffectiveDate': new Date(), 'PensionWage': 1,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_EmployeePensionStructures = [$scope.tbl_WPT_EmployeePensionStructure];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_EmployeePensionStructure = {
                'ID': 0, 'FK_tbl_WPT_Employee_ID': $scope.MasterObject.ID, 'EffectiveDate': new Date(), 'PensionWage': 1,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_EmployeePensionStructure }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_EmployeePensionStructure = data;
            $scope.tbl_WPT_EmployeePensionStructure.EffectiveDate = new Date(data.EffectiveDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    })
    .controller("EmployeeBankCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('EmployeeBankCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.MasterID = $scope.MasterObject.ID;
            $scope.pageNavigation('first');
        });

        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_EmployeeBankDetail.FK_tbl_WPT_Employee_ID = item.ID;
                $scope.tbl_WPT_EmployeeBankDetail.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_EmployeeBankDetail.FK_tbl_WPT_Employee_ID = null;
                $scope.tbl_WPT_EmployeeBankDetail.FK_tbl_WPT_Employee_IDName = null;
            }
        };

        init_Operations($scope, $http,
            '/WPT/Employee/EmployeeBankLoad', //--v_Load
            '/WPT/Employee/EmployeeBankGet', // getrow
            '/WPT/Employee/EmployeeBankPost' // PostRow
        );

        $scope.tbl_WPT_EmployeeBankDetail = {
            'ID': 0, 'FK_tbl_WPT_Employee_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Bank_Branch_ID': null, 'FK_tbl_WPT_Bank_Branch_IDName': null,
            'BankAccountNo': null, 'BankAccountTitle': null, 'IsDefaultForBank':true,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_EmployeeBankDetails = [$scope.tbl_WPT_EmployeeBankDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_EmployeeBankDetail = {
                'ID': 0, 'FK_tbl_WPT_Employee_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Bank_Branch_ID': null, 'FK_tbl_WPT_Bank_Branch_IDName': null,
                'BankAccountNo': null, 'BankAccountTitle': null, 'IsDefaultForBank': true,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_EmployeeBankDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_EmployeeBankDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    