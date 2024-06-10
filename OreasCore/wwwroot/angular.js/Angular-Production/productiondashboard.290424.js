MainModule
    .controller("ProductionDashBoardCtlr", function ($scope, $http) {



        $scope.GetDashBoardData = function () {

            var successcallback = function (response) {
                $scope.data = response.data;

                if ($scope.data != null) {
                    new Chart(document.getElementById('OrderNote'), {
                        type: 'doughnut',
                        data: {
                            labels: ['Order Qty', 'Mfg Qty'],
                            datasets: [
                                {
                                    label: "Active Orders: " + $scope.data.OrderNote.ON_NoOfProd,
                                    backgroundColor: ["#3266cd", "#ff8533", "#59b300"],
                                    data: [$scope.data.OrderNote.ON_TotalOrderQty, $scope.data.OrderNote.ON_TotalMfgQty]
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
                                text: 'Order Note'
                            },
                            responsive: true,
                            maintainAspectRatio: true
                        }
                    });
                    new Chart(document.getElementById('BMR'), {
                        type: 'doughnut',
                        data: {
                            labels: ['Open Batches', 'Closed Batches'],
                            datasets: [
                                {
                                    label: "Active/ In-Active Batches ",
                                    backgroundColor: ["#FFB6C1", "#b0c4de"],
                                    data: [$scope.data.BMR.BMR_NoOfOpen, $scope.data.BMR.BMR_NoOfClosed]
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
                                text: 'Active/InActive'
                            },
                            responsive: true,
                            maintainAspectRatio: true
                        }
                    });
                    new Chart(document.getElementById('BMRDispensing'), {
                        type: 'doughnut',
                        data: {
                            labels: ['Open Raw', 'Open Packaging'],
                            datasets: [
                                {
                                    label: "Active BMR: " + $scope.data.BMR.BMR_NoOfOpen,
                                    backgroundColor: ["#3266cd", "#59b300"],
                                    data: [$scope.data.BMR.BMR_R_DisPending, $scope.data.BMR.BMR_P_DisPending]
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
                                text: 'Dispensing'
                            },
                            responsive: true,
                            maintainAspectRatio: true
                        }
                    });
                }
                
            };
            var errorcallback = function (error) {
                console.log(error);
            };
            $http({ method: "GET", url: "/Production/Home/DashBoardGet", async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
        };

        $scope.GetDashBoardData();

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


