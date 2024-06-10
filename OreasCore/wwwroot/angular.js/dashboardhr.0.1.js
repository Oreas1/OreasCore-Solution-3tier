MainModule
    .controller("HRCtlr", function ($scope, $http) {
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

        init_MonthSearchModal($scope, $http);

        $scope.UserInfo = {
            'ID': 0, 'Photo160X210': null, 'HasFaceTemplate': false,
            'f0': false, 'f1': false, 'f2': false, 'f3': false, 'f4': false,
            'f5': false, 'f6': false, 'f7': false, 'f8': false, 'f9': false
        };

        $scope.LoadUserInfo = function () {
            var successcallback = function (response) {
                $scope.UserInfo = response.data;
            };
            var errorcallback = function (error) {
            };
            $http({ method: "GET", url: "/DashBoard/LoadUserInfo", async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

        };
        $scope.LoadUserInfo();



    })
    .controller("HRSalaryCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('HRSalaryCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.$broadcast('HRSalaryStructureCtlr', itm);
            $scope.$broadcast('HRLoanDetailCtlr', itm);
        });

        init_Operations($scope, $http,
            '/DashBoard/LoadSalaryDetail', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID > 0 ? $scope.MasterObject.ID : 0 }; };

    })
    .controller("HRSalaryStructureCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('HRSalaryStructureCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });


        init_Operations($scope, $http,
            '/DashBoard/LoadSalaryStructureDetail', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID > 0 ? $scope.MasterObject.ID : 0 }; };

    })
    .controller("HRLoanDetailCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('HRLoanDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });


        init_Operations($scope, $http,
            '/DashBoard/LoadLoanDetail', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID > 0 ? $scope.MasterObject.ID : 0 }; };

    })
    .controller("HRAttendanceCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('HRAttendanceCtlr', function (e, itm) {
            $scope.MonthID = 0;
            $scope.MasterObject = itm;
            $scope.LoadAT();
        });


        $scope.LoadAT = function () {

            var successcallback = function (response) {
                $scope.AT = response.data;
                
                $scope.createCalendar(new Date($scope.AT.MonthObject.MonthStart), new Date($scope.AT.MonthObject.MonthEnd));  
            };
            var errorcallback = function (error) {
            };
            $http({ method: "GET", url: "/DashBoard/LoadAttendance?EmpID=" + $scope.MasterObject.ID + '&MonthID=' + $scope.MonthID, async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
        };

        $scope.MonthSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.MonthID = item.ID; 
                $scope.LoadAT();
            }
            else {
                $scope.MonthID = 0;
            }

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
                        $scope.week.push({ 'ID': '', 'Day': '', 'AT': null });
                        continue;
                    }
                    //id = loopDate.getFullYear().toString() + (loopDate.getMonth() + 1).toString() + loopDate.getDate().toString();
                    id = new Date(loopDate.getTime() - loopDate.getTimezoneOffset() * 60 * 1000).toISOString().slice(0, 10).toString();
                            
                    $scope.week.push({ 'ID': id, 'Day': loopDate.getDate(), 'AT': $scope.AT.GetATOutComeOfEmployees.find(x => x.InstanceDate === id) });
                    loopDate.setDate(loopDate.getDate() + 1);
                }
                $scope.weeks.push($scope.week);
            }
            //------now load data
        };
        

    })
    .controller("HRTeamCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('HRTeamCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.ATDate = new Date();
            $scope.LoadTeamAT();
        });

        
        $scope.ATDateChange = function () {
            $scope.LoadTeamAT();
        };

        $scope.DrawChart = function (id, dataarray) {
            new Chart(document.getElementById(id), {
                type: 'pie',
                data: {
                    labels: ['Present','Absent'],
                    datasets: [
                        {
                            label: "Attendance",
                            backgroundColor: ["#3cba9f", "#c45850"],
                            data: dataarray
                        }
                    ]
                },
                options: {
                    plugins: {
                        legend: {
                            display: false
                        }
                    },
                    title: {
                        display: false,
                        text: 'xxx'
                    }
                }
            });

        };

        $scope.LoadTeamAT = function () {
            var successcallback = function (response) {
                $scope.TeamAT = response.data;  


                setTimeout(function () {
                    $scope.TeamAT.forEach(function myFunction(item) {
                       
                        $scope.DrawChart(item.DepartmentName, [item.TotalP, item.TotalA]);
                    });                  
                    

                }, 1);


                //const canvas = document.createElement("canvas");
                //canvas.id = 'mychart';
             
                //element.appendChild(canvas);
             


            };
            var errorcallback = function (error) {   };
            $http({ method: "GET", url: "/DashBoard/LoadTeamAT?EmpID=" + $scope.MasterObject.ID + "&ATDate=" + new Date($scope.ATDate).toLocaleString('en-US'), async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
        };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


