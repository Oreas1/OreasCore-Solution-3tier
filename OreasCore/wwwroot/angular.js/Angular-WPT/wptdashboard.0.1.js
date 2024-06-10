MainModule
    .controller("WPTDashBoardCtlr", function ($scope, $http) {



        $scope.GetDashBoardData = function () {

            var successcallback = function (response) {
                $scope.data = response.data;  

                if ($scope.data.Comparison.length > 0) {
                    new Chart(document.getElementById('Employees'), {
                        type: 'bar',
                        data: {
                            labels: Object.keys($scope.data.Comparison).map(function (k) { return $scope.data.Comparison[k].Month }),
                            datasets: [
                                {
                                    label: "Employees ",
                                    backgroundColor: ["#3266cd"],
                                    data: Object.keys($scope.data.Comparison).map(function (k) { return $scope.data.Comparison[k].Employees })
                                }
                            ]
                        },
                        options: {
                            plugins: {
                                legend: {
                                    display: true
                                }
                            },
                            title: {
                                display: false,
                                text: 'Employees'
                            }
                        }
                    });                    
                    new Chart(document.getElementById('Wage'), {
                        type: 'bar',
                        data: {
                            labels: Object.keys($scope.data.Comparison).map(function (k) { return $scope.data.Comparison[k].Month }),
                            datasets: [
                                {
                                    label: "Wage ",
                                    backgroundColor: ["#2a753c"],
                                    data: Object.keys($scope.data.Comparison).map(function (k) { return $scope.data.Comparison[k].Wage })
                                }
                            ]
                        },
                        options: {
                            plugins: {
                                legend: {
                                    display: true
                                }
                            },
                            title: {
                                display: false,
                                text: 'Wage'
                            }
                        }
                    });
                    new Chart(document.getElementById('Attendance'), {
                        type: 'bar',
                        data: {
                            labels: Object.keys($scope.data.Comparison).map(function (k) { return $scope.data.Comparison[k].Month }),
                            datasets: [
                                {
                                    label: "Attendance Weightage",
                                    backgroundColor: ["#e0e84a"],
                                    data: Object.keys($scope.data.Comparison).map(function (k) { return $scope.data.Comparison[k].PresentPercentage })
                                }
                            ]
                        },
                        options: {
                            plugins: {
                                legend: {
                                    display: true
                                }
                            },
                            title: {
                                display: false,
                                text: 'Attendance'
                            }
                        }
                    });
                    new Chart(document.getElementById('OT'), {
                        type: 'bar',
                        data: {
                            labels: Object.keys($scope.data.Comparison).map(function (k) { return $scope.data.Comparison[k].Month }),
                            datasets: [
                                {
                                    label: "Overtime In Hours ",
                                    backgroundColor: ["#d4632f"],
                                    data: Object.keys($scope.data.Comparison).map(function (k) { return $scope.data.Comparison[k].OT })
                                }
                            ]
                        },
                        options: {
                            plugins: {
                                legend: {
                                    display: true
                                }
                            },
                            title: {
                                display: false,
                                text: 'Overtime'
                            }
                        }
                    });
                    new Chart(document.getElementById('WD'), {
                        type: 'bar',
                        data: {
                            labels: Object.keys($scope.data.Comparison).map(function (k) { return $scope.data.Comparison[k].Month }),
                            datasets: [
                                {
                                    label: "Paid Days",
                                    backgroundColor: ["#255c5b"],
                                    data: Object.keys($scope.data.Comparison).map(function (k) { return $scope.data.Comparison[k].WD })
                                }
                            ]
                        },
                        options: {
                            plugins: {
                                legend: {
                                    display: true
                                }
                            },
                            title: {
                                display: false,
                                text: 'Paid Days'
                            }
                        }
                    });
                }
            };
            var errorcallback = function (error) {
                console.log(error);
            };
            $http({ method: "GET", url: "/WPT/Home/DashBoardGet", async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
        };

        $scope.GetDashBoardData();

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


