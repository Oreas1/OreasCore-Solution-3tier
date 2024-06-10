MainModule
    .controller("CalendarIndexCtlr", function ($scope, $window, $http) {
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
            '/WPT/Calendar/CalendarLoad', //--v_Load
            '/WPT/Calendar/CalendarGet', // getrow
            '/WPT/Calendar/CalendarPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Calendar/GetInitializedCalendar');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'CalendarIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'CalendarIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'CalendarIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'CalendarMonthCtlr') != undefined) {
                $scope.$broadcast('init_CalendarMonthCtlr', data.find(o => o.Controller === 'CalendarMonthCtlr'));
            }
            if (data.find(o => o.Controller === 'CalendarEmployeeForPLCtlr') != undefined) {
                $scope.$broadcast('init_CalendarEmployeeForPLCtlr', data.find(o => o.Controller === 'CalendarEmployeeForPLCtlr'));
            }
            if (data.find(o => o.Controller === 'CalendarPLOfEmployeeCtlr') != undefined) {
                $scope.$broadcast('init_CalendarPLOfEmployeeCtlr', data.find(o => o.Controller === 'CalendarPLOfEmployeeCtlr'));
            }
        };

        init_EmployeeSearchModalGeneral($scope, $http);

        $scope.tbl_WPT_CalendarYear = {
            'ID': 0, 'CalendarYear': new Date().getFullYear() + 1, 'NoOfOpen': 0, 'NoOfClosed': 0,
            'NoOfLeaveEmp': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_CalendarYears = [$scope.tbl_WPT_CalendarYear];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_CalendarYear = {
                'ID': 0, 'CalendarYear': new Date().getFullYear() + 1, 'NoOfOpen': 0, 'NoOfClosed': 0,
                'NoOfLeaveEmp': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };

        };

        $scope.CloseYear = function (id) {

            var successcallback = function (response) {
                if (response.data === 'OK') {
                    $scope.pageNavigation('Load');
                }
                else {
                    alert(response.data);
                }
            };
            var errorcallback = function (error) {
            };

            if (confirm("Are you sure! you want to close year") === true) {
                $http({
                    method: "POST", url: "/WPT/Calendar/CalendarClose", params: { CalendarID: id, operation: 'Save Update' }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
                }).then(successcallback, errorcallback);
            }
        };

        $scope.postRowParam = function () {

            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_CalendarYear };
        };

        $scope.GetRowResponse = function (data, operation) {
            
            $scope.tbl_WPT_CalendarYear = data;
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("CalendarMonthCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('CalendarMonthCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_CalendarMonthCtlr', function (e, itm) {           
            init_Filter($scope, itm.WildCard, null, null, null);            
        });

        init_Operations($scope, $http,
            '/WPT/Calendar/CalendarMonthLoad', //--v_Load
            '/WPT/Calendar/CalendarMonthGet', // getrow
            '/WPT/Calendar/CalendarMonthPost' // PostRow
        );
        function addDays(date, days) {
            var result = new Date(date);
            result.setDate(result.getDate() + days);
            return result;
        }

        $scope.VM_CalendarYear_Months_Adjustment = {
            'ID_M1': 0, 'FK_tbl_WPT_CalendarYear_ID_M1': null, 'MonthStart_M1': new Date(), 'MonthEnd_M1': new Date(), 'IsClosed_M1': false,
            'ID_M2': 0, 'FK_tbl_WPT_CalendarYear_ID_M2': null, 'MonthStart_M2': new Date(), 'MonthEnd_M2': new Date(), 'IsClosed_M2': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.VM_CalendarYear_Months_Adjustments = [$scope.VM_CalendarYear_Months_Adjustment];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.VM_CalendarYear_Months_Adjustment = {
                'ID_M1': 0, 'FK_tbl_WPT_CalendarYear_ID_M1': null, 'MonthStart_M1': new Date(), 'MonthEnd_M1': new Date(), 'IsClosed_M1': false,
                'ID_M2': 0, 'FK_tbl_WPT_CalendarYear_ID_M2': null, 'MonthStart_M2': new Date(), 'MonthEnd_M2': new Date(), 'IsClosed_M2': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.VM_CalendarYear_Months_Adjustment }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.VM_CalendarYear_Months_Adjustment = data;
            $scope.VM_CalendarYear_Months_Adjustment.MonthStart_M1 = new Date(data.MonthStart_M1);
            $scope.VM_CalendarYear_Months_Adjustment.MonthEnd_M1 = new Date(data.MonthEnd_M1);

            $scope.VM_CalendarYear_Months_Adjustment.MonthStart_M2 = new Date(data.MonthStart_M2);
            $scope.VM_CalendarYear_Months_Adjustment.MonthEnd_M2 = new Date(data.MonthEnd_M2);

            $scope.MinRange = addDays($scope.VM_CalendarYear_Months_Adjustment.MonthStart_M1, 1);
            $scope.MaxRange = addDays($scope.VM_CalendarYear_Months_Adjustment.MonthEnd_M2, -1);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        $scope.OnChangeOfDateTillMonth = function (m) {
            if (m === 1) {
                $scope.VM_CalendarYear_Months_Adjustment.MonthStart_M2 = addDays($scope.VM_CalendarYear_Months_Adjustment.MonthEnd_M1, 1);
            }
            else if (m === 2) {
                $scope.VM_CalendarYear_Months_Adjustment.MonthEnd_M1 = addDays($scope.VM_CalendarYear_Months_Adjustment.MonthStart_M2, -1);
            }
        };
    })
    .controller("CalendarEmployeeForPLCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('CalendarEmployeeForPLCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.CalendarMonthList = itm.MonthList;
            $scope.rptID = itm.ID;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_CalendarEmployeeForPLCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/WPT/Calendar/GetReport');
        });

        $scope.GotoReport = function (id) {           
            $window.open('/WPT/Calendar/GetReport?rn=Annual Employee Leaves Record&id=' + id);
        };

        init_Operations($scope, $http,
            '/WPT/Calendar/CalendarEmployeeForPLLoad', //--v_Load
            '/WPT/Calendar/CalendarEmployeeForPLGet', // getrow
            '/WPT/Calendar/CalendarEmployeeForPLPost' // PostRow
        );

        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_CalendarYear_LeaveEmps.FK_tbl_WPT_Employee_ID = item.ID;
                $scope.tbl_WPT_CalendarYear_LeaveEmps.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_CalendarYear_LeaveEmps.FK_tbl_WPT_Employee_ID = null;
                $scope.tbl_WPT_CalendarYear_LeaveEmps.FK_tbl_WPT_Employee_IDName = null;
            }
        };

        $scope.tbl_WPT_CalendarYear_LeaveEmps = {
            'ID': 0, 'FK_tbl_WPT_CalendarYear_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
            'LeaveCount': 0, 'TopLeaveName': '', 'TopLeaveOpening': '', 'IsClosed': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_CalendarYear_LeaveEmpss = [$scope.tbl_WPT_CalendarYear_LeaveEmps];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_CalendarYear_LeaveEmps = {
                'ID': 0, 'FK_tbl_WPT_CalendarYear_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
                'LeaveCount': 0, 'TopLeaveName': '', 'TopLeaveOpening': '', 'IsClosed': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_CalendarYear_LeaveEmps }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_CalendarYear_LeaveEmps = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("CalendarPLOfEmployeeCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('CalendarPLOfEmployeeCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_CalendarPLOfEmployeeCtlr', function (e, itm) {
            $scope.LeavePolicyList = itm.Otherdata === null ? [] : itm.Otherdata.LeavePolicyList;
            init_Filter($scope, itm.WildCard, null, null, null);
        });
        init_Operations($scope, $http,
            '/WPT/Calendar/CalendarPLOfEmployeeLoad', //--v_Load
            '/WPT/Calendar/CalendarPLOfEmployeeGet', // getrow
            '/WPT/Calendar/CalendarPLOfEmployeePost' // PostRow
        );

       
        ////////////data structure define//////////////////

        $scope.tbl_WPT_CalendarYear_LeaveEmps_Leaves = {
            'ID': 0, 'FK_tbl_WPT_CalendarYear_LeaveEmps_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_LeavePolicy_ID': null, 'FK_tbl_WPT_LeavePolicy_IDName': '',
            'FK_tbl_WPT_CalendarYear_Months_ID_Apply': null, 'FK_tbl_WPT_CalendarYear_Months_ID_ApplyName': '', 'Opening': 0,
            'FK_tbl_WPT_CalendarYear_Months_ID_Expire': null, 'FK_tbl_WPT_CalendarYear_Months_ID_ExpireName': '',
            'AllowedFromNextYear': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_CalendarYear_LeaveEmps_Leavess = [$scope.tbl_WPT_CalendarYear_LeaveEmps_Leaves];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_CalendarYear_LeaveEmps_Leaves = {
                'ID': 0, 'FK_tbl_WPT_CalendarYear_LeaveEmps_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_LeavePolicy_ID': null, 'FK_tbl_WPT_LeavePolicy_IDName': '',
                'FK_tbl_WPT_CalendarYear_Months_ID_Apply': null, 'FK_tbl_WPT_CalendarYear_Months_ID_ApplyName': '', 'Opening': 0,
                'FK_tbl_WPT_CalendarYear_Months_ID_Expire': null, 'FK_tbl_WPT_CalendarYear_Months_ID_ExpireName': '',
                'AllowedFromNextYear': 0,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };


        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_CalendarYear_LeaveEmps_Leaves }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_CalendarYear_LeaveEmps_Leaves = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    