MainModule
    .controller("AttendanceIndexCtlr", function ($scope, $http) {
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
     
        $scope.ParaDateFrom = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate(), 0, 0, 1, 0);
        $scope.ParaDateTill = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate(), 23, 59, 59, 0);

        //////////////////////////////entry panel/////////////////////////
        init_Operations($scope, $http,
            '/WPT/Attendance/AttendanceIndividualLoad', //--v_Load
            '/WPT/Attendance/AttendanceIndividualGet', // getrow
            '/WPT/Attendance/AttendanceIndividualPost' // PostRow
        );
       
        init_ViewSetup($scope, $http, '/WPT/Attendance/GetInitializedAttendance');

        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'AttendanceIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'AttendanceIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'AttendanceIndexCtlr').WildCard, null, null, null);
                init_Report($scope, data.find(o => o.Controller === 'AttendanceIndexCtlr').Reports, '/WPT/Attendance/GetReport');

                if (data.find(o => o.Controller === 'AttendanceIndexCtlr').Otherdata === null) {
                    $scope.ATInOutModeList = [];
                }
                else {
                    $scope.ATInOutModeList = data.find(o => o.Controller === 'AttendanceIndexCtlr').Otherdata.ATInOutModeList;
                    var LastOpenMonth = data.find(o => o.Controller === 'AttendanceIndexCtlr').Otherdata.LastOpenMonth;
                    if (LastOpenMonth != null) {
                        $scope.FK_tbl_WPT_CalendarYear_Months_ID = LastOpenMonth.ID;
                        $scope.FK_tbl_WPT_CalendarYear_Months_IDName = LastOpenMonth.MonthStart + ' TO ' + LastOpenMonth.MonthEnd;


                        $scope.MonthStart = new Date(LastOpenMonth.MonthStart).setHours(0,0,0); 
                        $scope.MonthEnd = new Date(LastOpenMonth.MonthEnd).setHours(23,59,59);

                        $scope.ParaDateFrom = new Date($scope.MonthStart); 
                        $scope.ParaDateTill = new Date($scope.MonthEnd); 

                        $scope.DateRangeNo();
                      
                     
                    }
                    

                }
               // $scope.pageNavigation('first');              
            }

            if (data.find(o => o.Controller === 'AttendanceTogetherCtlr') != undefined) {
                $scope.$broadcast('init_AttendanceTogetherCtlr', data.find(o => o.Controller === 'AttendanceTogetherCtlr'));
            }
        };

        $scope.DateRangeNo = function () {
            $scope.dateArray = [];
            let currentDate = new Date($scope.MonthStart);

            while (currentDate <= new Date($scope.MonthEnd)) {
                $scope.dateArray.push({ 'DayNo': new Date(currentDate).getDate(), 'DayName': new Date(currentDate).toLocaleString("default", { weekday: "short" }) });
                currentDate.setUTCDate(currentDate.getUTCDate() + 1);
            }
        };

        init_MonthSearchModal($scope, $http);
        init_EmployeeSearchModalGeneral($scope, $http);


        $scope.MonthSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.FK_tbl_WPT_CalendarYear_Months_ID = item.ID;
                $scope.FK_tbl_WPT_CalendarYear_Months_IDName = item.MonthStart + ' TO ' + item.MonthEnd;

                $scope.MonthStart = new Date(item.MonthStart).setHours(0, 0, 0);
                $scope.MonthEnd = new Date(item.MonthEnd).setHours(23, 59, 59);
                $scope.ParaDateFrom = new Date($scope.MonthStart);
                $scope.ParaDateTill = new Date($scope.MonthEnd); 

                $scope.DateRangeNo();

            }
            else {
                $scope.FK_tbl_WPT_CalendarYear_Months_ID = null;
                $scope.FK_tbl_WPT_CalendarYear_Months_IDName = null;
                $scope.MonthStart = new Date();
                $scope.MonthEnd = new Date();
                $scope.ParaDateFrom = new Date();
                $scope.ParaDateTill = new Date();
            }

        };

        $scope.rptID = 0;

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

        $scope.tbl_WPT_AttendanceLog = {
            'ID': 0, 'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
            'FK_tbl_WPT_Machine_ID': null, 'FK_tbl_WPT_Machine_IDName': '', 'ATEnrollmentNo': null,
            'ATInOutMode': 0, 'ATInOutModeName': '', 'ATDateTime': $scope.ParaDateTill, 'Loggedby': 1
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_AttendanceLogs = [$scope.tbl_WPT_AttendanceLog];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_AttendanceLog = {
                'ID': 0, 'FK_tbl_WPT_Employee_ID': $scope.ParaEmpID, 'FK_tbl_WPT_Employee_IDName': $scope.ParaEmpName,
                'FK_tbl_WPT_Machine_ID': null, 'FK_tbl_WPT_Machine_IDName': '', 'ATEnrollmentNo': null,
                'ATInOutMode': 0, 'ATInOutModeName': '', 'ATDateTime': $scope.ParaDateTill, 'Loggedby': 1
            };           
            
        };

        $scope.pageNavigationParameterChanged = function () {
            $scope.ng_entryPanelHide = true;
            $scope.ng_entryPanelBtnText = 'Add New';
            $scope.pageddata.Data.length = null;
            $scope.pageddata.TotalPages = 0;
            $scope.pageddata.CurrentPage = 1;
        };

        $scope.postRowParam = function () {
            if (!$scope.tbl_WPT_AttendanceLog.FK_tbl_WPT_Employee_ID > 0)
                $scope.tbl_WPT_AttendanceLog.FK_tbl_WPT_Employee_ID = $scope.ParaEmpID;

            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_AttendanceLog };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_AttendanceLog = data;
            $scope.tbl_WPT_AttendanceLog.ATDateTime = new Date(data.ATDateTime);
            if (!$scope.tbl_WPT_AttendanceLog.FK_tbl_WPT_Employee_ID > 0) {
                $scope.tbl_WPT_AttendanceLog.FK_tbl_WPT_Employee_ID = $scope.ParaEmpID;
                $scope.tbl_WPT_AttendanceLog.FK_tbl_WPT_Employee_IDName = $scope.ParaEmpName;
            }
                
        };
      
        $scope.pageNavigatorParam = function () {
            $scope.tbl_WPT_AttendanceLog.ATDateTime = new Date();
            $scope.FilterValueByDateRangeFrom = new Date($scope.ParaDateFrom).toLocaleString('en-US');
            $scope.FilterValueByDateRangeTill = new Date($scope.ParaDateTill).toLocaleString('en-US');
            return { MasterID: $scope.ParaEmpID };
        };      
       
    })
    .controller("AttendanceTogetherCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('AttendanceTogetherCtlr', function (e, itm) {        
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_AttendanceTogetherCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/Attendance/ATTogetherLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.pageNavigatorParam = function () {
            return { MonthStart: new Date($scope.MonthStart).toLocaleString('en-US'),MonthEnd: new Date($scope.MonthEnd).toLocaleString('en-US') };
        };

        //$scope.GetpageNavigationResponse = function (data) {
        //    $scope.pageddata = data;
        //    console.log(data);
        //};

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    