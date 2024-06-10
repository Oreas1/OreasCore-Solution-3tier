MainModule
    .controller("InventoryDashBoardCtlr", function ($scope, $http) {



        $scope.GetDashBoardData = function () {

            var successcallback = function (response) {
                $scope.data = response.data;
                if ($scope.data != null) {
                    new Chart(document.getElementById('OrderNote'), {
                        type: 'bar',
                        data: {
                            labels: ['Order Qty', 'Mfg Qty', 'Sold Qty'],
                            datasets: [
                                {
                                    label: "Active Orders: " + $scope.data.OrderNote.ON_NoOfProd,
                                    backgroundColor: ["#3266cd", "#ff8533", "#59b300"],
                                    data: [$scope.data.OrderNote.ON_TotalOrderQty, $scope.data.OrderNote.ON_TotalMfgQty, $scope.data.OrderNote.ON_TotalSoldQty]
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
                            }
                        }
                    });
                    new Chart(document.getElementById('PurchaseOrder'), {
                        type: 'bar',
                        data: {
                            labels: ['PO Qty', 'Received Qty'],
                            datasets: [
                                {
                                    label: "Active PO: " + $scope.data.PurchaseOrder.PO_NoOfProd,
                                    backgroundColor: ["#3266cd", "#59b300"],
                                    data: [$scope.data.PurchaseOrder.PO_Qty, $scope.data.PurchaseOrder.PO_RecQty]
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
                            }
                        }
                    });
                }
                
            };
            var errorcallback = function (error) {
                console.log(error);
            };
            $http({ method: "GET", url: "/Inventory/Home/DashBoardGet", async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
        };

        $scope.GetDashBoardData();

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


