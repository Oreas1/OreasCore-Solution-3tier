MainModule
    .controller("AccountsDashBoardCtlr", function ($scope, $http) {



        $scope.GetDashBoardData = function () {

            var successcallback = function (response) {
                $scope.data = response.data;   
                if ($scope.data != null) {
                    new Chart(document.getElementById('SalePurchase'), {
                        type: 'bar',
                        data: {
                            labels: ['Sales', 'Sales Return', 'Purchase', 'Purchase Return'],
                            datasets: [
                                {
                                    label: "In Last six Months",
                                    backgroundColor: ["#39ac39", "#8cd98c", "#4da6ff", "#99ccff"],
                                    data: [$scope.data.SP.Sales_L6M, $scope.data.SalesReturn_L6M, $scope.data.SP.Purchase_L6M, $scope.data.SP.PurchaseReturn_L6M]
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
                                text: 'Sale / Purchase'
                            }
                        }
                    });
                }             
            };
            var errorcallback = function (error) {
                console.log(error);
            };
            $http({ method: "GET", url: "/Accounts/Home/DashBoardGet", async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
        };

        $scope.GetDashBoardData();

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


