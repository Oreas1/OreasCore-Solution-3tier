MainModule
    .controller("PayRunIndexCtlr", function ($scope, $http) {
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
            '/WPT/PayRun/PayRunCalendarLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );
        init_ViewSetup($scope, $http, '/WPT/PayRun/GetInitializedPayRun');

        $scope.init_ViewSetup_Response = function (data) {

            if (data.find(o => o.Controller === 'PayRunIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'PayRunIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'PayRunIndexCtlr').WildCard, null, null, null);
                if (data.find(o => o.Controller === 'PayRunIndexCtlr').Otherdata === null) {
                    $scope.TransactionModeList = [];
                    $scope.DesignationList = [];
                    $scope.DepartmentList = [];
                    $scope.HolidayList = [];
                    $scope.ShiftList = [];
                }
                else {
                    $scope.TransactionModeList = data.find(o => o.Controller === 'PayRunIndexCtlr').Otherdata.TransactionModeList;
                    $scope.DesignationList = data.find(o => o.Controller === 'PayRunIndexCtlr').Otherdata.DesignationList;
                    $scope.DepartmentList = data.find(o => o.Controller === 'PayRunIndexCtlr').Otherdata.DepartmentList;
                    $scope.HolidayList = data.find(o => o.Controller === 'PayRunIndexCtlr').Otherdata.HolidayList;
                    $scope.ShiftList = data.find(o => o.Controller === 'PayRunIndexCtlr').Otherdata.ShiftList;
                }

                $scope.pageNavigation('first');

            }
            if (data.find(o => o.Controller === 'PayRunToDoCtlr') != undefined) {
                $scope.$broadcast('init_PayRunToDoCtlr', data.find(o => o.Controller === 'PayRunToDoCtlr'));
            }
            if (data.find(o => o.Controller === 'PayRunExemptCtlr') != undefined) {
                $scope.$broadcast('init_PayRunExemptCtlr', data.find(o => o.Controller === 'PayRunExemptCtlr'));
            }
            if (data.find(o => o.Controller === 'PayRunExemptEmployeeCtlr') != undefined) {
                $scope.$broadcast('init_PayRunExemptEmployeeCtlr', data.find(o => o.Controller === 'PayRunExemptEmployeeCtlr'));
            }
            if (data.find(o => o.Controller === 'PayRunHolidayCtlr') != undefined) {
                $scope.$broadcast('init_PayRunHolidayCtlr', data.find(o => o.Controller === 'PayRunHolidayCtlr'));
            }
            if (data.find(o => o.Controller === 'PayRunLeaveRequisitionCtlr') != undefined) {
                $scope.$broadcast('init_PayRunLeaveRequisitionCtlr', data.find(o => o.Controller === 'PayRunLeaveRequisitionCtlr'));
            }
            if (data.find(o => o.Controller === 'PayRunMasterDetailEmployeeCtlr') != undefined) {
                $scope.$broadcast('init_PayRunMasterDetailEmployeeCtlr', data.find(o => o.Controller === 'PayRunMasterDetailEmployeeCtlr'));
            }
            if (data.find(o => o.Controller === 'PayRunMasterDetailPaymentCtlr') != undefined) {
                $scope.$broadcast('init_PayRunMasterDetailPaymentCtlr', data.find(o => o.Controller === 'PayRunMasterDetailPaymentCtlr'));
            }
            if (data.find(o => o.Controller === 'PayRunMasterDetailPaymentEmployeeCtlr') != undefined) {
                $scope.$broadcast('init_PayRunMasterDetailPaymentEmployeeCtlr', data.find(o => o.Controller === 'PayRunMasterDetailPaymentEmployeeCtlr'));
            }
            if (data.find(o => o.Controller === 'PayRunRosterMasterCtlr') != undefined) {
                $scope.$broadcast('init_PayRunRosterMasterCtlr', data.find(o => o.Controller === 'PayRunRosterMasterCtlr'));
            }
            if (data.find(o => o.Controller === 'PayRunRosterDetailShiftCtlr') != undefined) {
                $scope.$broadcast('init_PayRunRosterDetailShiftCtlr', data.find(o => o.Controller === 'PayRunRosterDetailShiftCtlr'));
            }
            if (data.find(o => o.Controller === 'PayRunRosterDetailEmployeeCtlr') != undefined) {
                $scope.$broadcast('init_PayRunRosterDetailEmployeeCtlr', data.find(o => o.Controller === 'PayRunRosterDetailEmployeeCtlr'));
            }
        };

        init_EmployeeSearchModalGeneral($scope, $http);
        init_LeavePolicySearchModal($scope, $http);

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_CalendarYear_Monthss = [$scope.tbl_WPT_CalendarYear_Months];

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

        $scope.MonthOpenClose = function (id, IsClosed) {            
            var successcallback = function (response) {
                if (response.data === 'OK') {
                    $scope.pageNavigation('Load');
                }
                else {
                    alert(response.data);
                }
            };
            var errorcallback = function (error) {
                console.log(response.data);
            };

            if (confirm("Are you sure! you want to " + (IsClosed ? 'Close' : 'Open') + " the month") === true) {           
                    $http({
                        method: "POST", url: "/WPT/PayRun/PayRunMonthOpenClose", params: { operation: 'Save Update', MonthID: id, MonthIsClosed: IsClosed }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
                    }).then(successcallback, errorcallback);                
            }
        };

    })
    .controller("PayRunToDoCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('PayRunToDoCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_PayRunToDoCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/PayRun/PayRunToDoLoad', //--v_Load
            '/WPT/PayRun/PayRunToDoGet', // getrow
            '/WPT/PayRun/PayRunToDoPost' // PostRow
        );

        $scope.tbl_WPT_PayRunToDo = {
            'ID': 0, 'FK_tbl_WPT_CalendarYear_Months_ID': $scope.MasterObject.ID, 'ToDo': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_PayRunToDos = [$scope.tbl_WPT_PayRunToDo];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_PayRunToDo = {
                'ID': 0, 'FK_tbl_WPT_CalendarYear_Months_ID': $scope.MasterObject.ID, 'ToDo': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_PayRunToDo }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_PayRunToDo = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("PayRunExemptCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('PayRunExemptCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_PayRunExemptCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            $scope.DeductibleTypeList = itm.Otherdata === null ? [] : itm.Otherdata.DeductibleTypeList;
            $scope.LoanTypeList = itm.Otherdata === null ? [] : itm.Otherdata.LoanTypeList;
        });

        init_Operations($scope, $http,
            '/WPT/PayRun/PayRunExemptLoad', //--v_Load
            '/WPT/PayRun/PayRunExemptGet', // getrow
            '/WPT/PayRun/PayRunExemptPost' // PostRow
        );

        $scope.tbl_WPT_PayRunExemption = {
            'ID': 0, 'FK_tbl_WPT_CalendarYear_Months_ID': $scope.MasterObject.ID, 'ApplyToAll': true, 'ExemptionPercentage': 100,
            'FK_tbl_WPT_DeductibleType_ID': null, 'FK_tbl_WPT_DeductibleType_IDName': '',
            'FK_tbl_WPT_LoanType_ID': null, 'FK_tbl_WPT_LoanType_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'NoOfEmp': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_PayRunExemptions = [$scope.tbl_WPT_PayRunExemption];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_PayRunExemption = {
                'ID': 0, 'FK_tbl_WPT_CalendarYear_Months_ID': $scope.MasterObject.ID, 'ApplyToAll': true, 'ExemptionPercentage': 100,
                'FK_tbl_WPT_DeductibleType_ID': null, 'FK_tbl_WPT_DeductibleType_IDName': '',
                'FK_tbl_WPT_LoanType_ID': null, 'FK_tbl_WPT_LoanType_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'NoOfEmp': 0
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_PayRunExemption }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_PayRunExemption = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("PayRunExemptEmployeeCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('PayRunExemptEmployeeCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_PayRunExemptEmployeeCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/PayRun/PayRunExemptEmployeeLoad', //--v_Load
            '/WPT/PayRun/PayRunExemptEmployeeGet', // getrow
            '/WPT/PayRun/PayRunExemptEmployeePost' // PostRow
        );

        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_PayRunExemption_Emp.FK_tbl_WPT_Employee_ID = item.ID;
                $scope.tbl_WPT_PayRunExemption_Emp.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_PayRunExemption_Emp.FK_tbl_WPT_Employee_ID = null;
                $scope.tbl_WPT_PayRunExemption_Emp.FK_tbl_WPT_Employee_IDName = null;
            }
        };

        $scope.tbl_WPT_PayRunExemption_Emp = {
            'ID': 0, 'FK_tbl_WPT_PayRunExemption_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '', 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_PayRunExemption_Emps = [$scope.tbl_WPT_PayRunExemption_Emp];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_PayRunExemption_Emp = {
                'ID': 0, 'FK_tbl_WPT_PayRunExemption_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '', 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_PayRunExemption_Emp }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_PayRunExemption_Emp = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("PayRunHolidayCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('PayRunHolidayCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.createCalendar(new Date(itm.CalendarMonthStartDate), new Date(itm.CalendarMonthEndDate));           
        });

        $scope.$on('init_PayRunHolidayCtlr', function (e, itm) {
            
        });

        init_Operations($scope, $http,
            '/WPT/PayRun/PayRunHolidayLoad', //--v_Load
            '/WPT/PayRun/PayRunHolidayGet', // getrow
            '/WPT/PayRun/PayRunHolidayPost' // PostRow
        );        
        
        $scope.GetpageNavigationResponse = function (data) {
            $scope.pageddata = data.pageddata;

            var DayFound = null;
            $scope.weeks.forEach(function (w) {
                w.forEach(function (d) {
                    DayFound = $scope.pageddata.Data.find(x => x.HolidayDate === d.ID);
                    if (DayFound != null) {
                        d.Remarks = DayFound.FK_tbl_WPT_Holiday_IDName;
                    }
                    else {
                        d.Remarks = '';
                    }
                })
            });          

        };
   
        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_CalendarYear_Months_Holidays }; };

        $scope.PostRowResponse = function (data) {
            if (data === 'OK')
            {
                $('#PayRunHolidayModal').modal('hide');
                $scope.pageNavigation('first');
            }
                
            else
                alert(data);

        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_CalendarYear_Months_Holidays = {
                'ID': 0, 'FK_tbl_WPT_CalendarYear_Months_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Holiday_ID': null, 'FK_tbl_WPT_Holiday_IDName': '', 'HolidayDate': new Date(),
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_CalendarYear_Months_Holidays = data;
            $scope.tbl_WPT_CalendarYear_Months_Holidays.HolidayDate = new Date(data.HolidayDate);
        };

        $scope.createCalendar = function (startdate, enddate) {
            $scope.MonthName = startdate.toLocaleString('default', { month: 'long' }) + '-' + startdate.getFullYear();
            const StartBlankDays = startdate.getDay();

            var id = '0';
            const diffTime = Math.abs(enddate - startdate);
            const TotalDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24)) + 1;

            const TotalWeeks = Math.ceil((TotalDays+StartBlankDays)/7);

            let loopDate = new Date(startdate);

            $scope.weeks = [];
            for (let i = 0; i <= TotalWeeks; i++) {
                $scope.week = [];
                for (let j = 0; j < 7; j++) {

                    if (i == 0 && j < StartBlankDays || enddate < loopDate) {
                        $scope.week.push({ 'ID': '', 'Day': '', 'Remarks': ''});
                        continue;
                    }
                    //id = loopDate.getFullYear().toString() + (loopDate.getMonth() + 1).toString() + loopDate.getDate().toString();
                    id=new Date(loopDate.getTime() - loopDate.getTimezoneOffset() * 60 * 1000).toISOString().slice(0, 10).toString()
                    $scope.week.push({ 'ID': id, 'Day': loopDate.getDate(), 'Remarks': ''});
                    loopDate.setDate(loopDate.getDate() + 1);
                }
                $scope.weeks.push($scope.week);
            }  
            
            $scope.pageNavigation('first');
        };

        $scope.DateClickEvent = function (operation, item) {
            $scope.showEntryPanel();

            $scope.ng_entryPanelSubmitBtnText = operation;

            
            let founddata = $scope.pageddata.Data.find(x => x.HolidayDate === item);
   

            if (operation === 'Save New') {                
                $scope.tbl_WPT_CalendarYear_Months_Holidays.HolidayDate = new Date(item);
            }
            else if (operation === 'Save Update') {
                if (founddata != null) {
                    $scope.GetRow(founddata.ID, 'Edit');
                }
                else
                    return;

            }
            else if (operation === 'Save Delete') {
                if (founddata != null) {
                    $scope.GetRow(founddata.ID, 'Delete');
                }
                else
                    return;
            }
            else
                return;             

            $('#PayRunHolidayModal').modal('show');

        };
        
       
    })
    .controller("PayRunLeaveRequisitionCtlr", function ($scope, $http) {
        $scope.IsMonthClosed = true;
        $scope.MasterObject = {};
        $scope.$on('PayRunLeaveRequisitionCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.MinRange = new Date($scope.MasterObject.CalendarMonthStartDate);
            $scope.MaxRange = new Date($scope.MasterObject.CalendarMonthEndDate);
            $scope.IsMonthClosed = $scope.MasterObject.IsMonthClosed;
        });
        $scope.$on('init_PayRunLeaveRequisitionCtlr', function (e, itm) {
            $scope.ActionList = itm.Otherdata === null ? [] : itm.Otherdata.ActionList;
            init_Filter($scope, itm.WildCard, null, null, itm.LoadByCard);
        });

        init_Operations($scope, $http,
            '/WPT/PayRun/PayRunLeaveRequisitionLoad', //--v_Load
            '/WPT/PayRun/PayRunLeaveRequisitionGet', // getrow
            '/WPT/PayRun/PayRunLeaveRequisitionPost' // PostRow
        );

        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_Employee_ID = item.ID;
                $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_Employee_ID = null;
                $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_Employee_IDName = null;
            }
            $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_LeavePolicyNonPaid_ID = null;
            $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_LeavePolicy_ID = null;
            $scope.tbl_WPT_LeaveRequisition.LeavePolicyName = null;
        };

        $scope.LeavePolicySearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.LeaveType === 'NP') {
                $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_LeavePolicyNonPaid_ID = item.ID;
                $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_LeavePolicy_ID = null;
            }
            else if (item.LeaveType === 'P') {
                $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_LeavePolicy_ID = item.ID;
                $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_LeavePolicyNonPaid_ID = null;

            }
            $scope.tbl_WPT_LeaveRequisition.LeavePolicyName = item.PolicyName;
        };

        $scope.tbl_WPT_LeaveRequisition = {
            'ID': 0, 'DocNo': null, 'DocDate': new Date,
            'LeaveValue': 1, 'FK_tbl_WPT_CalendarYear_Months_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
            'FK_tbl_WPT_LeavePolicy_ID': null, 'FK_tbl_WPT_LeavePolicyNonPaid_ID': '', 'LeavePolicyName': '',
            'LeaveFrom': new Date($scope.MasterObject.MonthStart), 'LeaveTill': new Date($scope.MasterObject.MonthStart), 'Reason': '',
            'FK_tbl_WPT_ActionList_ID_HOS': 3, 'FK_tbl_WPT_ActionList_ID_HOSName': 'FK_tbl_WPT_Employee_ID_HOS', '': null, 'FK_tbl_WPT_Employee_ID_HOSName': '',
            'FK_tbl_WPT_ActionList_ID_HR': 1, 'FK_tbl_WPT_ActionList_ID_HRName': '', 'FK_tbl_WPT_Employee_ID_HR': null, 'FK_tbl_WPT_Employee_ID_HRName': '',
            'FK_tbl_WPT_ActionList_ID_Final': 3, 'FK_tbl_WPT_ActionList_ID_FinalName': '',
            'RequesterFormCode': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_LeaveRequisitions = [$scope.tbl_WPT_LeaveRequisition];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_LeaveRequisition = {
                'ID': 0, 'DocNo': null, 'DocDate': new Date,
                'LeaveValue': 1, 'FK_tbl_WPT_CalendarYear_Months_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
                'FK_tbl_WPT_LeavePolicy_ID': null, 'FK_tbl_WPT_LeavePolicyNonPaid_ID': '', 'LeavePolicyName': '',
                'LeaveFrom': new Date($scope.MasterObject.MonthStart), 'LeaveTill': new Date($scope.MasterObject.MonthStart), 'Reason': '',
                'FK_tbl_WPT_ActionList_ID_HOS': 3, 'FK_tbl_WPT_ActionList_ID_HOSName': 'FK_tbl_WPT_Employee_ID_HOS', '': null, 'FK_tbl_WPT_Employee_ID_HOSName': '',
                'FK_tbl_WPT_ActionList_ID_HR': 1, 'FK_tbl_WPT_ActionList_ID_HRName': '', 'FK_tbl_WPT_Employee_ID_HR': null, 'FK_tbl_WPT_Employee_ID_HRName': '',
                'FK_tbl_WPT_ActionList_ID_Final': 3, 'FK_tbl_WPT_ActionList_ID_FinalName': '',
                'RequesterFormCode': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_LeaveRequisition }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_LeaveRequisition = data;
            $scope.tbl_WPT_LeaveRequisition.DocDate = new Date(data.DocDate);
            $scope.tbl_WPT_LeaveRequisition.LeaveFrom = new Date(data.LeaveFrom);
            $scope.tbl_WPT_LeaveRequisition.LeaveTill = new Date(data.LeaveTill);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };
    })
    .controller("PayRunMasterCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('PayRunMasterCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.MinRange = new Date($scope.MasterObject.CalendarMonthStartDate);
            $scope.MaxRange = new Date($scope.MasterObject.CalendarMonthEndDate);
            $scope.pageNavigation('first');
        });
        $scope.$on('init_PayRunMasterCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/PayRun/PayRunMasterLoad', //--v_Load
            '/WPT/PayRun/PayRunMasterGet', // getrow
            '/WPT/PayRun/PayRunMasterPost' // PostRow
        );

        $scope.tbl_WPT_PayRunMaster = {
            'ID': 0, 'FK_tbl_WPT_CalendarYear_Months_ID': $scope.MasterObject.ID,
            'StartDate': new Date($scope.MasterObject.CalendarMonthStartDate),
            'EndDate': new Date($scope.MasterObject.CalendarMonthEndDate),
            'NoOfActiveEmployees': 0, 'NoOfEmployees': 0,
            'NoOfZeroATEmp': 0, 'NoOfZeroWageEmp': 0, 'NoOfDefaulterWageEmp': 0, 'TotalWage': 0, 'NoOf2LessThenEqualATEmp': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_PayRunMasters = [$scope.tbl_WPT_PayRunMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_PayRunMaster = {
                'ID': 0, 'FK_tbl_WPT_CalendarYear_Months_ID': $scope.MasterObject.ID,
                'StartDate': new Date($scope.MasterObject.CalendarMonthStartDate),
                'EndDate': new Date($scope.MasterObject.CalendarMonthEndDate),
                'NoOfActiveEmployees': 0, 'NoOfEmployees': 0,
                'NoOfZeroATEmp': 0, 'NoOfZeroWageEmp': 0, 'NoOfDefaulterWageEmp': 0, 'TotalWage': 0, 'NoOf2LessThenEqualATEmp': 0,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_PayRunMaster };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_PayRunMaster = data;
            $scope.tbl_WPT_PayRunMaster.StartDate = new Date(data.StartDate);
            $scope.tbl_WPT_PayRunMaster.EndDate = new Date(data.EndDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        $scope.PayRunAll = function (id) {

            if (confirm("Are you sure! you want to Start PayRun ") === true) {

                var successcallback = function (response) {
                    if (response.data === 'OK') { $scope.pageNavigation('Load'); }
                    else
                        alert(response.data);
                };
                var errorcallback = function (error) { };

                $http({ method: "GET", url: "/WPT/PayRun/PayRunDetailEmpPayRunAll", params: { id: id }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'Privilege': $scope.Privilege.CanView } }).then(successcallback, errorcallback);

            }

        };

        $scope.PayRunAllRemove = function (id) {

            if (confirm("Are you sure! you want to Rollback PayRun ") === true) {

                var successcallback = function (response) {
                    if (response.data === 'OK') { $scope.pageNavigation('Load'); }
                    else
                        alert(response.data);
                };
                var errorcallback = function (error) { };

                $http({ method: "GET", url: "/WPT/PayRun/PayRunDetailEmpPayRunAllDelete", params: { id: id }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'Privilege': $scope.Privilege.CanView } }).then(successcallback, errorcallback);

            }

        };

    })
    .controller("PayRunMasterDetailEmployeeCtlr", function ($scope, $http, $window) {
        $scope.MasterObject = {};
        $scope.$on('PayRunMasterDetailEmployeeCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.rptID = itm.ID;           
            $scope.pageNavigation('first');
           
        });
        $scope.$on('init_PayRunMasterDetailEmployeeCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);     
            init_Report($scope, itm.Reports, '/WPT/PayRun/GetReport?_for=Emp');
        });

        init_Operations($scope, $http,
            '/WPT/PayRun/PayRunDetailEmployeeLoad', //--v_Load
            '/WPT/PayRun/PayRunDetailEmployeeGet', // getrow
            '/WPT/PayRun/PayRunDetailEmployeePost' // PostRow
        );
        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_PayRunDetail_Emp.FK_tbl_WPT_Employee_ID = item.ID;
                $scope.tbl_WPT_PayRunDetail_Emp.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_PayRunDetail_Emp.FK_tbl_WPT_Employee_ID = null;
                $scope.tbl_WPT_PayRunDetail_Emp.FK_tbl_WPT_Employee_IDName = null;
            }

        };

        $scope.GetPaySlip = function (id) {
            $window.open('/WPT/PayRun/GetReport?rn=PayRun Salary Slip Individual&_for=Emp&id=' + id);
        };

        $scope.tbl_WPT_PayRunDetail_Emp = {
            'ID': 0, 'FK_tbl_WPT_PayRunMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '', 'Email': '', 'CellPhoneNo': '',
            'WD_AT': 0, 'WD_Manual': 0, 'WD': 0, 'OT_AT': 0, 'OT_Manual': 0, 'OT': 0, 'Wage': 0,
            'WagePrimary': 0, 'FK_tbl_WPT_TransactionMode_ID_Primary': null, 'FK_tbl_WPT_TransactionMode_ID_PrimaryName': '',
            'WageSecondary': 0, 'FK_tbl_WPT_TransactionMode_ID_Secondary': null, 'FK_tbl_WPT_TransactionMode_ID_SecondaryName': '',
            'FK_tbl_WPT_PayRunDetail_Payment_ID_Primary': null, 'FK_tbl_WPT_PayRunDetail_Payment_ID_PrimaryName': '',
            'FK_tbl_WPT_PayRunDetail_Payment_ID_Secondary': null, 'FK_tbl_WPT_PayRunDetail_Payment_ID_SecondaryName': '',
            'FK_tbl_WPT_EmployeeSalaryStructure_ID': null, 'FK_tbl_WPT_EmployeeSalaryStructure_IDName': '',
            'FK_tbl_WPT_EmployeeBankDetail_ID': null, 'FK_tbl_WPT_EmployeeBankDetail_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
            'IsWagedGenerated': false
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_PayRunDetail_Emps = [$scope.tbl_WPT_PayRunDetail_Emp];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_PayRunDetail_Emp = {
                'ID': 0, 'FK_tbl_WPT_PayRunMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '', 'Email': '', 'CellPhoneNo': '',
                'WD_AT': 0, 'WD_Manual': 0, 'WD': 0, 'OT_AT': 0, 'OT_Manual': 0, 'OT': 0, 'Wage': 0,
                'WagePrimary': 0, 'FK_tbl_WPT_TransactionMode_ID_Primary': null, 'FK_tbl_WPT_TransactionMode_ID_PrimaryName': '',
                'WageSecondary': 0, 'FK_tbl_WPT_TransactionMode_ID_Secondary': null, 'FK_tbl_WPT_TransactionMode_ID_SecondaryName': '',
                'FK_tbl_WPT_PayRunDetail_Payment_ID_Primary': null, 'FK_tbl_WPT_PayRunDetail_Payment_ID_PrimaryName': '',
                'FK_tbl_WPT_PayRunDetail_Payment_ID_Secondary': null, 'FK_tbl_WPT_PayRunDetail_Payment_ID_SecondaryName': '',
                'FK_tbl_WPT_EmployeeSalaryStructure_ID': null, 'FK_tbl_WPT_EmployeeSalaryStructure_IDName': '',
                'FK_tbl_WPT_EmployeeBankDetail_ID': null, 'FK_tbl_WPT_EmployeeBankDetail_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
                'IsWagedGenerated': false
            };
        };

        $scope.postRowParam = function () {
            if ($scope.tbl_WPT_PayRunDetail_Emp.FK_tbl_WPT_TransactionMode_ID_Primary === $scope.tbl_WPT_PayRunDetail_Emp.FK_tbl_WPT_TransactionMode_ID_Secondary) {
                alert('Both Transaction mode cannot be identical');
                return { validate: false, params: { operation: $scope.entryPanelSubmitBtnText }, data: $scope.tbl_WPT_EmployeeSalaryStructure };
            }
            if ($scope.tbl_WPT_PayRunDetail_Emp.WagePrimary === 0) $scope.tbl_WPT_PayRunDetail_Emp.FK_tbl_WPT_PayRunDetail_Payment_ID_Primary = null;
            if ($scope.tbl_WPT_PayRunDetail_Emp.WageSecondary === 0) $scope.tbl_WPT_PayRunDetail_Emp.FK_tbl_WPT_PayRunDetail_Payment_ID_Secondary = null;
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_PayRunDetail_Emp };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_PayRunDetail_Emp = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };


        $scope.PayRunIndividual = function (id, payrun) {         
            if (confirm("Are you sure! you want to start PayRun process") === true) {

                var successcallback = function (response) {
                    if (response.data === 'OK') { $scope.pageNavigation('Load'); }
                    else
                        alert(response.data);
                };
                var errorcallback = function (error) { };

                $http({ method: "POST", url: "/WPT/PayRun/PayRunProcessDetailEmployeePost", params: { operation: 'Save Update', ID: id, PayRun: payrun, MasterID: $scope.MasterObject.ID}, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken } }).then(successcallback, errorcallback);
            }
        };

        $scope.BalanceWage = function (v) {

            if ($scope.tbl_WPT_PayRunDetail_Emp.WagePrimary > $scope.tbl_WPT_PayRunDetail_Emp.Wage)
                $scope.tbl_WPT_PayRunDetail_Emp.WagePrimary = $scope.tbl_WPT_PayRunDetail_Emp.Wage;

            if ($scope.tbl_WPT_PayRunDetail_Emp.WageSecondary > $scope.tbl_WPT_PayRunDetail_Emp.Wage)
                $scope.tbl_WPT_PayRunDetail_Emp.WageSecondary = $scope.tbl_WPT_PayRunDetail_Emp.Wage;

            if (v === 'primary') {
                $scope.tbl_WPT_PayRunDetail_Emp.WageSecondary = $scope.tbl_WPT_PayRunDetail_Emp.Wage - $scope.tbl_WPT_PayRunDetail_Emp.WagePrimary;
            }
            else if (v === 'secondary')
                $scope.tbl_WPT_PayRunDetail_Emp.WagePrimary = $scope.tbl_WPT_PayRunDetail_Emp.Wage - $scope.tbl_WPT_PayRunDetail_Emp.WageSecondary;

        };

        $scope.EmailPaySlip = function (id,empName,Email) {
          
            if (confirm("Are you sure! you want to Email PaySlip ") === true) {
                var successcallback = function (response) {
                    if (response.data === 'OK') { alert('Sucessfully Sent'); }
                    else
                        alert(response.data);
                };
                var errorcallback = function (error) { alert(error); };

                $http({ method: "GET", url: "/WPT/PayRun/EmailPaySlip", params: { ID: id, EmpName: empName, EmpEmail: Email, MonthEnd: $scope.MasterObject.EndDate }, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
            }
        };

    })
    .controller("PayRunMasterDetailEmployeeATCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('PayRunMasterDetailEmployeeATCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_PayRunMasterDetailEmployeeATCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/PayRun/PayRunDetailEmployeeATLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.tbl_WPT_PayRunDetail_EmpDetail_AT = {
            'ID': 0, 'FK_tbl_WPT_WageDetail_Emp_ID': $scope.MasterObject.ID, 'DayName': '', 'InstanceDate': '', 'CheckIn': '', 'CheckOut': '',
            'Present': false, 'Absent': false, 'AbsentInHoliday': false, 'AbsentPenalty': false, 'Holiday': false,
            'LateIn': false, 'EarlyOut': false, 'HalfShit': false, 'HalfShitPenalty': false, 'OT': 0, 'ShiftMinutes': 0, 'BeforeShiftMinutes': 0, 'AfterShiftMinutes': 0,
            'FK_tbl_WPT_LeaveRequisition_ID': null, 'FK_tbl_WPT_LeaveRequisition_IDName': '', 'ShiftPrefix': '',
            'ShiftWorkingMinutes': 0, 'WDValue': 0, 'LeaveValue': 0,
            'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_PayRunDetail_EmpDetail_ATs = [$scope.tbl_WPT_PayRunDetail_EmpDetail_AT];

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("PayRunMasterDetailEmployeeWageCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('PayRunMasterDetailEmployeeWageCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_PayRunMasterDetailEmployeeWageCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/PayRun/PayRunDetailEmployeeWageLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.tbl_WPT_PayRunDetail_EmpDetail_Wage = {
            'ID': 0, 'FK_tbl_WPT_WageDetail_Emp_ID': $scope.MasterObject.ID, 'Rate': 0, 'Qty': 0, 'Debit': 0, 'Credit': 0,
            'FK_tbl_WPT_EmployeeSalaryStructure_ID_Basic': null, 'FK_tbl_WPT_EmployeeSalaryStructure_ID_BasicName': '',
            'FK_tbl_WPT_PayRunDetail_EmpDetail_Wage_ID': null, 'FK_tbl_WPT_PayRunDetail_EmpDetail_Wage_IDName': '',
            'FK_tbl_WPT_EmployeeSalaryStructureDeductible_ID': null, 'FK_tbl_WPT_EmployeeSalaryStructureDeductible_IDName': '',
            'FK_tbl_WPT_tbl_OTPolicy_ID': null, 'FK_tbl_WPT_tbl_OTPolicy_IDName': '',
            'FK_tbl_WPT_IncentivePolicy_ID': null, 'FK_tbl_WPT_IncentivePolicy_IDName': '',
            'FK_tbl_WPT_LoanDetail_ID': null, 'FK_tbl_WPT_LoanDetail_IDName': '',
            'FK_tbl_WPT_IncrementDetail_ID_Arrear': null, 'FK_tbl_WPT_IncrementDetail_ID_ArrearName': '',
            'FK_tbl_WPT_ShiftRosterDetail_Employee_ID': null, 'FK_tbl_WPT_ShiftRosterDetail_Employee_IDName': '',
            'FK_tbl_WPT_LeavePolicy_ID_EL': null, 'FK_tbl_WPT_LeavePolicy_ID_ELName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_PayRunDetail_EmpDetail_Wages = [$scope.tbl_WPT_PayRunDetail_EmpDetail_Wage];

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("PayRunMasterDetailPaymentCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('PayRunMasterDetailPaymentCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_PayRunMasterDetailPaymentCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            $scope.CompanyBankAcList = itm.Otherdata === null ? [] : itm.Otherdata.CompanyBankAcList;
        });

        init_Operations($scope, $http,
            '/WPT/PayRun/PayRunDetailPaymentLoad', //--v_Load
            '/WPT/PayRun/PayRunDetailPaymentGet', // getrow
            '/WPT/PayRun/PayRunDetailPaymentPost' // PostRow
        );


        $scope.tbl_WPT_PayRunDetail_Payment = {
            'ID': 0, 'FK_tbl_WPT_PayRunMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_CompanyBankDetail_ID': null, 'FK_tbl_WPT_CompanyBankDetail_IDName': '',
            'FK_tbl_WPT_TransactionMode_ID': null, 'FK_tbl_WPT_TransactionMode_IDName': '', 'InstrumentNo': '', 'TransactionDate': new Date(), 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'Amount': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_PayRunDetail_Payments = [$scope.tbl_WPT_PayRunDetail_Payment];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_PayRunDetail_Payment = {
                'ID': 0, 'FK_tbl_WPT_PayRunMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_CompanyBankDetail_ID': null, 'FK_tbl_WPT_CompanyBankDetail_IDName': '',
                'FK_tbl_WPT_TransactionMode_ID': null, 'FK_tbl_WPT_TransactionMode_IDName': '', 'InstrumentNo': '', 'TransactionDate': new Date(), 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'Amount': 0
            };

        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_PayRunDetail_Payment };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_PayRunDetail_Payment = data;
            $scope.tbl_WPT_PayRunDetail_Payment.TransactionDate = new Date(data.TransactionDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("PayRunMasterDetailPaymentEmployeeCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('PayRunMasterDetailPaymentEmployeeCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.rptID = itm.ID;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_PayRunMasterDetailPaymentEmployeeCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/WPT/PayRun/GetReport?_for=Pay');
        });

        init_Operations($scope, $http,
            '/WPT/PayRun/PayRunDetailPaymentEmployeeLoad', //--v_Load
            '/WPT/PayRun/PayRunDetailPaymentEmployeeGet', // getrow
            '/WPT/PayRun/PayRunDetailPaymentEmployeePost' // PostRow
        );
        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_PayRunDetail_Emp.ID = item.ID;
                $scope.tbl_WPT_PayRunDetail_Emp.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_PayRunDetail_Emp.ID = 0;
                $scope.tbl_WPT_PayRunDetail_Emp.FK_tbl_WPT_Employee_IDName = '';
            }
        };

        $scope.tbl_WPT_PayRunDetail_Emp = {
            'ID': 0, 'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
            'FK_tbl_WPT_PayRunDetail_Payment_ID_Primary': null, 'FK_tbl_WPT_PayRunDetail_Payment_ID_Secondary': null,
            'WagePrimary': 0, 'WageSecondary': 0,
            'FK_tbl_WPT_EmployeeBankDetail_ID': null, 'FK_tbl_WPT_EmployeeBankDetail_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_PayRunDetail_Emps = [$scope.tbl_WPT_PayRunDetail_Emp];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_PayRunDetail_Emp = {
                'ID': 0, 'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
                'FK_tbl_WPT_PayRunDetail_Payment_ID_Primary': null, 'FK_tbl_WPT_PayRunDetail_Payment_ID_Secondary': null,
                'WagePrimary': 0, 'WageSecondary': 0,
                'FK_tbl_WPT_EmployeeBankDetail_ID': null, 'FK_tbl_WPT_EmployeeBankDetail_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
            $scope.FK_tbl_WPT_Designation_ID = null;
            $scope.FK_tbl_WPT_Department_ID = null;
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText, tbl_WPT_PayRunDetail_EmpID: $scope.tbl_WPT_PayRunDetail_Emp.ID, PayRunPaymentID: $scope.MasterObject.ID, DesignationID: $scope.FK_tbl_WPT_Designation_ID, DepartmentID: $scope.FK_tbl_WPT_Department_ID }, data: $scope.tbl_WPT_PayRunDetail_Emp };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_PayRunDetail_Emp = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("PayRunOperationHubCtlr", function ($scope, $http, $rootScope) {

        $scope.MasterObject = {};
        $scope.$on('PayRunOperationHubCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            //$scope.pageNavigation('first');
            $scope.StartHub();
        });

        $scope.$on('init_PayRunOperationHubCtlr', function (e, itm) {
            //init_Filter($scope, itm.WildCard, null, null, null);
        });
        $scope.Operations = [
            { n: 'Employees Accumulating', v: 'Employees Accumulating' },
            { n: 'Employees PayRun', v: 'Employees PayRun' },
            { n: 'Employees PayRun Reversal', v: 'Employees PayRun Reversal' },
            { n: 'Emloyees PaySlip Mailing', v: 'Emloyees PaySlip Mailing' },
            { n: 'test', v: 'test' }
        ];
        //--------xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx--------//
        //---------------------------------------SignalR------------------------------------------------------------//
        "use strict";

        var connection = new signalR.HubConnectionBuilder().withUrl("/payrunOperationHub").build();

        connection.on("ClientAcknowledgment", function (message) {
            $rootScope.$apply(function () {
                $scope.ServerAcknowledgment = message;
                console.log(message);
            });
        });

        connection.on("ReceiveProgressUpdate", function (message) {
            $rootScope.$apply(function () {
                $scope.ServerAcknowledgment = message;
                console.log($scope.ServerAcknowledgment);
            });
        });
        //---------------------------------------------------------------SignalR Hub events---------------//
        $scope.StartHub = function () {
            if (connection.state === signalR.HubConnectionState.Disconnected)
                connection.start().then(function () {
                    $rootScope.$apply(function () {
                        $scope.ServerConnectionStatus = 'Connected';
                    });
                }).catch(function (err) {
                    return console.error(err.toString());
                });
        };
        $scope.StopHub = function () {
            connection.stop().then(function () {
                $rootScope.$apply(function () {
                    $scope.ServerConnectionStatus = 'Disconnected';
                });

            }).catch(function (err) {
                return console.error(err.toString());
            });
        };

        connection.onclose(() => {
            $rootScope.$apply(function () {
                $scope.ServerConnectionStatus = 'Disconnected';
                $scope.BtnOptText = 'Start Operation';
            });
        });
        //--------xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx--------//
        //---------------------------------------Angular------------------------------------------------------------//

        $scope.BtnOptText = 'Start Operation';
        $scope.ServerConnectionStatus = 'Disconnected';
        $scope.IsAborted = false;
        $scope.InProcess = false;


        $scope.OperationCalling = function () {

            if ($scope.BtnOptText === 'Start Operation' && connection.state === signalR.HubConnectionState.Connected) {

                $scope.BtnOptText = 'Stop Operation';
                $scope.InProcess = true;

                connection.invoke("StartOperation", parseInt($scope.MasterObject.ID), $scope.MachineOpt)
                    .then(function () {
                        $rootScope.$apply(function () {

                        });
                    })
                    .catch(function (err) {
                        $rootScope.$apply(function () {
                            $scope.ServerAcknowledgment = 'Some thing went wrong while processing the Operation: ' + $scope.MachineOpt;
                        });
                        return console.error(err.toString());
                    })
                    .finally(function () {
                        $rootScope.$apply(function () {
                            $scope.BtnOptText = 'Start Operation';
                            $scope.InProcess = false;
                            if ($scope.IsAborted === true) {
                                $scope.ServerAcknowledgment = 'Process Aborted for Operation : ' + $scope.MachineOpt;
                                $scope.IsAborted = false;
                            }
                        });
                    });
            }
            else if ($scope.BtnOptText === 'Stop Operation' && connection.state === signalR.HubConnectionState.Connected) {

                $scope.BtnOptText = 'Start Operation';

                connection.invoke("CancelOperation")
                    .then(function () {
                        $rootScope.$apply(function () {
                            $scope.ServerAcknowledgment = 'Process Aborted for Operation: ' + $scope.MachineOpt;
                            $scope.IsAborted = true;
                            $scope.InProcess = false;
                        });
                    })
                    .catch(function (err) {
                        return console.error(err.toString());
                    })
                    .finally(function () {
                        $rootScope.$apply(function () {
                            $scope.ServerAcknowledgment = 'Process Aborted for Operation: ' + $scope.MachineOpt;
                        });
                    });
            }
            else if (connection.state === signalR.HubConnectionState.Disconnected) {
                alert('Connection has been Disconnected from server');
            }

        };
        $scope.RequestToConnectOrDisconnect = function () {
            if (connection.state === signalR.HubConnectionState.Disconnected)
                $scope.StartHub();
            else if (connection.state === signalR.HubConnectionState.Connected)
                $scope.StopHub();
        };
        $scope.GetOperatorDetail = function () {
            if (connection.state === signalR.HubConnectionState.Connected) {
                connection.invoke("GetOperatorDetail", parseInt($scope.MasterID))
                    .then(function (response) {
                        $rootScope.$apply(function () {
                            $scope.OperatorDetail = response;
                        });
                    })
                    .catch(function (err) {
                        $rootScope.$apply(function () {

                        });
                        return console.error(err.toString());
                    })
                    .finally(function () {
                        $rootScope.$apply(function () {
                        });
                    });
            }
            else {
                alert('Connection has been Disconnected from server');
            }

        };
        

    })
    .controller("PayRunRosterMasterCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('PayRunRosterMasterCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_PayRunRosterMasterCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/PayRun/PayRunRosterMasterLoad', //--v_Load
            '/WPT/PayRun/PayRunRosterMasterGet', // getrow
            '/WPT/PayRun/PayRunRosterMasterPost' // PostRow
        );

        $scope.tbl_WPT_ShiftRosterMaster = {
            'ID': 0, 'FK_tbl_WPT_CalendarYear_Months_ID': $scope.MasterObject.ID,
            'RosterName': '', 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_ShiftRosterMasters = [$scope.tbl_WPT_ShiftRosterMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_ShiftRosterMaster = {
                'ID': 0, 'FK_tbl_WPT_CalendarYear_Months_ID': $scope.MasterObject.ID,
                'RosterName': '', 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_ShiftRosterMaster }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_ShiftRosterMaster = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("PayRunRosterDetailShiftCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('PayRunRosterDetailShiftCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            
            $scope.createCalendar(new Date(itm.MasterObject.CalendarMonthStartDate), new Date(itm.MasterObject.CalendarMonthEndDate));
        });

        $scope.$on('init_PayRunRosterDetailShiftCtlr', function (e, itm) {

        });

        init_Operations($scope, $http,
            '/WPT/PayRun/PayRunRosterDetailShiftLoad', //--v_Load
            '/WPT/PayRun/PayRunRosterDetailShiftGet', // getrow
            '/WPT/PayRun/PayRunRosterDetailShiftPost' // PostRow
        );

        $scope.GetpageNavigationResponse = function (data) {
            $scope.pageddata = data.pageddata;

            var DayFound = null;
            var defaultHolidayFound = null;
            $scope.weeks.forEach(function (w) {
                w.forEach(function (d) {


                    d.RosterID = 0;
                    d.ShiftName = '~Shift';
                    d.HolidayName = '';

                    defaultHolidayFound = $scope.pageddata.otherdata.find(x => x.HolidayDate === d.ID);
                    if (defaultHolidayFound != null && d.HolidayName === '') {
                        d.HolidayName = '~Holiday';
                    }


                    DayFound = $scope.pageddata.Data.find(x => x.RosterDate === d.ID);
                    
                    if (DayFound != null) {
                        d.RosterID = DayFound.RosterID;

                        if (DayFound.FK_tbl_WPT_Shift_IDName.length > 0)
                            d.ShiftName = DayFound.FK_tbl_WPT_Shift_IDName;

                        if (DayFound.FK_tbl_WPT_Holiday_IDName.length > 0)
                            d.HolidayName = DayFound.FK_tbl_WPT_Holiday_IDName;
                        else
                        {
                            if (DayFound.ApplyDefaultHoliday === false)
                                d.HolidayName = '';
                        }

                        
                    }

                        

                })
            });



        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_ShiftRosterDetail }; };

        $scope.PostRowResponse = function (data) {
            if (data === 'OK') {
                $('#PayRunRosterShiftModal').modal('hide');
                $scope.pageNavigation('first');
            }                
            else
                alert(data);

        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_ShiftRosterDetail = {
                'ID': 0, 'FK_tbl_WPT_ShiftRosterMaster_ID': $scope.MasterObject.ID, 'RosterDate': new Date(),
                'FK_tbl_WPT_Shift_ID': null, 'FK_tbl_WPT_Shift_IDName': '', 'FK_tbl_WPT_Holiday_ID': null, 'FK_tbl_WPT_Holiday_IDName': '',
                'ApplyDefaultHoliday': true,'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_ShiftRosterDetail = data;
            $scope.tbl_WPT_ShiftRosterDetail.RosterDate = new Date(data.RosterDate);
        };

        $scope.createCalendar = function (startdate, enddate) {
            $scope.MonthName = startdate.toLocaleString('default', { month: 'long' }) + '-' + startdate.getFullYear();
            const StartBlankDays = startdate.getDay();

            var id = '0';
            const diffTime = Math.abs(enddate - startdate);
            const TotalDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24)) + 1;
            const TotalWeeks = Math.ceil((TotalDays + StartBlankDays) / 7);            

            let loopDate = new Date(startdate);

            $scope.weeks = [];
            for (let i = 0; i <= TotalWeeks; i++) {
                $scope.week = [];
                for (let j = 0; j < 7; j++) {

                    if (i == 0 && j < StartBlankDays || enddate < loopDate) {
                        $scope.week.push({ 'ID': '', 'Day': '', 'ShiftName': '', 'HolidayName': '', 'RosterID': 0 });
                        continue;
                    }

                    id = new Date(loopDate.getTime() - loopDate.getTimezoneOffset() * 60 * 1000).toISOString().slice(0, 10).toString(); // 'yyyy-MM-dd'
                    $scope.week.push({ 'ID': id, 'Day': loopDate.getDate(), 'ShiftName': '', 'HolidayName': '', 'RosterID': 0 });
                    loopDate.setDate(loopDate.getDate() + 1);
                }
                $scope.weeks.push($scope.week);
            }

            $scope.pageNavigation('first');
        };

        $scope.DateClickEvent = function (operation, item) {
            $scope.showEntryPanel();

            $scope.ng_entryPanelSubmitBtnText = operation;

            if (operation === 'Save New') {
                $scope.tbl_WPT_ShiftRosterDetail.RosterDate = new Date(item.ID);
            }
            else if (operation === 'Save Update') {
                if (item != null) {
                    $scope.GetRow(item.RosterID, 'Edit');
                }
                else
                    return;

            }
            else if (operation === 'Save Delete') {
                if (item != null) {
                    $scope.GetRow(item.RosterID, 'Delete');
                }
                else
                    return;
            }
            else
                return;

            $('#PayRunRosterShiftModal').modal('show');

        };

   
        $scope.DateArr = [];
        $scope.SelectDate = function (id) {



            const objWithIdIndex = $scope.DateArr.findIndex(x => x.date === new Date(id).toISOString());
    

            if (objWithIdIndex < 0) {
                document.getElementById(id).style.backgroundColor = 'red';
                $scope.DateArr.push({ 'date': new Date(id).toISOString() });


                
            }
            else {
                document.getElementById(id).style.backgroundColor = 'darkslategrey';
                $scope.DateArr.splice(objWithIdIndex,1);
            }

            
            console.log($scope.DateArr);
        };
    })
    .controller("PayRunRosterDetailEmployeeCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('PayRunRosterDetailEmployeeCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_PayRunRosterDetailEmployeeCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/PayRun/PayRunRosterDetailEmployeeLoad', //--v_Load
            '/WPT/PayRun/PayRunRosterDetailEmployeeGet', // getrow
            '/WPT/PayRun/PayRunRosterDetailEmployeePost' // PostRow
        );

        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_ShiftRosterDetail_Employee.FK_tbl_WPT_Employee_ID = item.ID;
                $scope.tbl_WPT_ShiftRosterDetail_Employee.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_ShiftRosterDetail_Employee.FK_tbl_WPT_Employee_ID = null;
                $scope.tbl_WPT_ShiftRosterDetail_Employee.FK_tbl_WPT_Employee_IDName = null;
            }
        };

        $scope.tbl_WPT_ShiftRosterDetail_Employee = {
            'ID': 0, 'FK_tbl_WPT_ShiftRosterMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Employee_ID': 0, 'FK_tbl_WPT_Employee_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_ShiftRosterDetail_Employees = [$scope.tbl_WPT_ShiftRosterDetail_Employee];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_ShiftRosterDetail_Employee = {
                'ID': 0, 'FK_tbl_WPT_ShiftRosterMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Employee_ID': 0, 'FK_tbl_WPT_Employee_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
            if ($scope.AddBulk === true) {

                $scope.FK_tbl_WPT_Designation_ID = null;
                $scope.FK_tbl_WPT_Department_ID = null;
                $scope.JoiningDateTill = new Date();
            }
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText, MasterID: $scope.MasterObject.ID, DesignationID: $scope.FK_tbl_WPT_Designation_ID, DepartmentID: $scope.FK_tbl_WPT_Department_ID, JoiningDate: new Date($scope.JoiningDateTill).toLocaleString('en-US') }, data: $scope.tbl_WPT_ShiftRosterDetail_Employee };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_ShiftRosterDetail_Employee = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


