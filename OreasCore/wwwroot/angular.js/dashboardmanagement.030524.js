MainModule
    .controller("ManagementDashBoardCtlr", function ($scope, $http, $interval) {
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

        $scope.IsFor = '';
        $scope.ActivatedSidePanel = null;
        $scope.SidePenalLoad = function (IsFor, ActivateSidePanel) {
  
            if ($scope.ActivatedSidePanel != null)
                $("#" + $scope.ActivatedSidePanel).hide();

            $scope.ActivatedSidePanel = ActivateSidePanel;
            $scope.IsFor = IsFor;            
            $scope.$broadcast($scope.ActivatedSidePanel);
            $("#" + $scope.ActivatedSidePanel).show('slow');
        };  

        $scope.ClosePanel = function (ClosingSidePanel)
        {
            $("#" + ClosingSidePanel).hide();
        };

        init_ViewSetup($scope, $http, '/DashBoard/GetInitializedManagementDashBoard');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'ManagementDashBoardCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'ManagementDashBoardCtlr').Privilege;
                $scope.data = data.find(o => o.Controller === 'ManagementDashBoardCtlr').Otherdata;  

                $scope.Init_OrderNoteChart($scope.data.OrderNote);
                $scope.Init_PayableReceivableChart($scope.data.INV.Payables_Supplier, $scope.data.INV.Receivables_Customer);
            }
            if (data.find(o => o.Controller === 'ManagementBankDocCtlr') != undefined) {
                $scope.$broadcast('init_ManagementBankDocCtlr', data.find(o => o.Controller === 'ManagementBankDocCtlr'));
            }
            if (data.find(o => o.Controller === 'ManagementCashDocCtlr') != undefined) {
                $scope.$broadcast('init_ManagementCashDocCtlr', data.find(o => o.Controller === 'ManagementCashDocCtlr'));
            }
            if (data.find(o => o.Controller === 'ManagementJournalDocCtlr') != undefined) {
                $scope.$broadcast('init_ManagementJournalDocCtlr', data.find(o => o.Controller === 'ManagementJournalDocCtlr'));
            }
            if (data.find(o => o.Controller === 'ManagementJournalDoc2Ctlr') != undefined) {
                $scope.$broadcast('init_ManagementJournalDoc2Ctlr', data.find(o => o.Controller === 'ManagementJournalDoc2Ctlr'));
            }
            if (data.find(o => o.Controller === 'ManagementPNCtlr') != undefined) {
                $scope.$broadcast('init_ManagementPNCtlr', data.find(o => o.Controller === 'ManagementPNCtlr'));
            }
            if (data.find(o => o.Controller === 'ManagementPNDetailCtlr') != undefined) {
                $scope.$broadcast('init_ManagementPNDetailCtlr', data.find(o => o.Controller === 'ManagementPNDetailCtlr'));
            }
            if (data.find(o => o.Controller === 'ManagementPRNCtlr') != undefined) {
                $scope.$broadcast('init_ManagementPRNCtlr', data.find(o => o.Controller === 'ManagementPRNCtlr'));
            }
            if (data.find(o => o.Controller === 'ManagementSNCtlr') != undefined) {
                $scope.$broadcast('init_ManagementSNCtlr', data.find(o => o.Controller === 'ManagementSNCtlr'));
            }
            if (data.find(o => o.Controller === 'ManagementSRNCtlr') != undefined) {
                $scope.$broadcast('init_ManagementSRNCtlr', data.find(o => o.Controller === 'ManagementSRNCtlr'));
            }
        };
        $scope.Init_OrderNoteChart = function (data)
        {          
          new Chart(document.getElementById('OrderNote'), {
                    type: 'bar',
                    data: {
                        labels: ['Order Qty', 'Mfg Qty', 'Sold Qty'],
                        datasets: [
                            {
                                label: "Active Orders: " + data.ON_NoOfProd,
                                backgroundColor: ["#3266cd", "#ff8533", "#59b300"],
                                data: [data.ON_TotalOrderQty, data.ON_TotalMfgQty, data.ON_TotalSoldQty]
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
        };
        $scope.Init_PayableReceivableChart = function (data1, data2) {
            new Chart(document.getElementById('PayableReceivable'), {
                type: 'pie',
                data: {
                    labels: ['Payables', 'Receivables'],
                    datasets: [
                        {
                            label: null,
                            backgroundColor: ["#E75480 ", "#59b300"],
                            data: [data1, data2]
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
                        text: 'Payables & Receivables'
                    },
                    responsive: true,
                    maintainAspectRatio: true
                }
            });
        };

        //----------------Page Auto Reload--------------//
        $scope.idleTime = 0;

        $scope.resetIdleTime = function () {
            $scope.idleTime = 0;
        };

        $scope.checkIdleTime = function () {
            $scope.idleTime += 1;
            if ($scope.idleTime >= 600) { // 10 Minutes 
                location.reload();
            }
        };

        // Reset idle time on mouse click or keypress
        angular.element(document).on('click keydown', function () {
            $scope.$apply($scope.resetIdleTime);
        });

        // Check idle time every second
        $interval($scope.checkIdleTime, 1000);

    })
    .controller("ManagementBankDocCtlr", function ($scope, $http) {
              
        $scope.$on('ManagementBankDocCtlr', function (e, itm) {
            $scope.FilterByText = null;
            $scope.FilterValueByText = '';
            $scope.pageNavigation('first');
        });

        $scope.$on('init_ManagementBankDocCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, itm.WildCardDateRange, null);
        });


        init_Operations($scope, $http,
            '/DashBoard/BankDocumentLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.pageNavigatorParam = function () { return { IsFor: $scope.IsFor }; };
        $scope.GetpageNavigationResponse = function (data)
        {
            $scope.pageddata = data.pageddata;
            if ($scope.IsFor === 'Payment')
                $scope.data.Pen.BPVPendingSupervised = data.pageddata.otherdata;   
            else if ($scope.IsFor === 'Received')
                $scope.data.Pen.BRVPendingSupervised = data.pageddata.otherdata; 
        };

        $scope.Supervised = function (id) {

            var successcallback = function (response) {
                if (response.data === 'OK') {
                    $scope.pageNavigation('Load');
                }
                else {
                    alert(response.data);
                }
            };
            var errorcallback = function (error) {
                alert('Some thing went Wrong, Contact Administrator');
                console.log('Post error', error);
            };

            $http({
                method: "POST", url: '/DashBoard/BankDocumentSupervised', async: true, params: {ID: id}, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
            }).then(successcallback, errorcallback);
        };

    })
    .controller("ManagementCashDocCtlr", function ($scope, $http) {

        $scope.$on('ManagementCashDocCtlr', function (e, itm) {
            $scope.FilterByText = null;
            $scope.FilterValueByText = '';
            $scope.pageNavigation('first');
        });

        $scope.$on('init_ManagementCashDocCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, itm.WildCardDateRange, null);
        });


        init_Operations($scope, $http,
            '/DashBoard/CashDocumentLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.pageNavigatorParam = function () { return { IsFor: $scope.IsFor }; };
        $scope.GetpageNavigationResponse = function (data) {
            $scope.pageddata = data.pageddata;
            if ($scope.IsFor === 'Payment')
                $scope.data.Pen.CPVPendingSupervised = data.pageddata.otherdata;
            else if ($scope.IsFor === 'Received')
                $scope.data.Pen.CRVPendingSupervised = data.pageddata.otherdata;
        };

        $scope.Supervised = function (id) {

            var successcallback = function (response) {
                if (response.data === 'OK') {
                    $scope.pageNavigation('Load');
                }
                else {
                    alert(response.data);
                }
            };
            var errorcallback = function (error) {
                alert('Some thing went Wrong, Contact Administrator');
                console.log('Post error', error);
            };

            $http({
                method: "POST", url: '/DashBoard/CashDocumentSupervised', async: true, params: { ID: id }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
            }).then(successcallback, errorcallback);
        };

    })
    .controller("ManagementJournalDocCtlr", function ($scope, $http) {

        $scope.$on('ManagementJournalDocCtlr', function (e, itm) {
            $scope.FilterByText = null;
            $scope.FilterValueByText = '';
            $scope.pageNavigation('first');
        });

        $scope.$on('init_ManagementJournalDocCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, itm.WildCardDateRange, null);
        });


        init_Operations($scope, $http,
            '/DashBoard/JournalDocumentLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.pageNavigatorParam = function () { return { }; };

        $scope.GetpageNavigationResponse = function (data) {
            $scope.pageddata = data.pageddata;
            $scope.data.Pen.JVPendingSupervised = data.pageddata.otherdata;
        };

        $scope.Supervised = function (id) {

            var successcallback = function (response) {
                if (response.data === 'OK') {
                    $scope.pageNavigation('Load');
                }
                else {
                    alert(response.data);
                }
            };
            var errorcallback = function (error) {
                alert('Some thing went Wrong, Contact Administrator');
                console.log('Post error', error);
            };

            $http({
                method: "POST", url: '/DashBoard/JournalDocumentSupervised', async: true, params: { ID: id }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
            }).then(successcallback, errorcallback);
        };

    })
    .controller("ManagementJournalDoc2Ctlr", function ($scope, $http) {

        $scope.$on('ManagementJournalDoc2Ctlr', function (e, itm) {
            $scope.FilterByText = null;
            $scope.FilterValueByText = '';
            $scope.pageNavigation('first');
        });

        $scope.$on('init_ManagementJournalDoc2Ctlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, itm.WildCardDateRange, null);
        });


        init_Operations($scope, $http,
            '/DashBoard/JournalDocument2Load', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.pageNavigatorParam = function () { return {}; };

        $scope.GetpageNavigationResponse = function (data) {
            $scope.pageddata = data.pageddata;
            $scope.data.Pen.JV2PendingSupervised = data.pageddata.otherdata;
        };

        $scope.Supervised = function (id) {

            var successcallback = function (response) {
                if (response.data === 'OK') {
                    $scope.pageNavigation('Load');
                }
                else {
                    alert(response.data);
                }
            };
            var errorcallback = function (error) {
                alert('Some thing went Wrong, Contact Administrator');
                console.log('Post error', error);
            };

            $http({
                method: "POST", url: '/DashBoard/JournalDocument2Supervised', async: true, params: { ID: id }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
            }).then(successcallback, errorcallback);
        };

    })
    .controller("ManagementPNCtlr", function ($scope, $http) {

        $scope.$on('ManagementPNCtlr', function (e, itm) {
            $scope.FilterByText = null;
            $scope.FilterValueByText = '';
            $scope.pageNavigation('first');
        });

        $scope.$on('init_ManagementPNCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/DashBoard/PurchaseNoteLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.pageNavigatorParam = function () { return { IsFor: $scope.IsFor }; };

        $scope.GetpageNavigationResponse = function (data) {
            $scope.pageddata = data.pageddata;
            $scope.data.Pen.PNPendingSupervised = data.pageddata.otherdata.PNPendingSupervised;
            $scope.data.Pen.PNPendingProcessed = data.pageddata.otherdata.PNPendingProcessed;
        };

        $scope.Supervised = function (id) {

            var successcallback = function (response) {
                if (response.data === 'OK') {
                    $scope.pageNavigation('Load');
                }
                else {
                    alert(response.data);
                }
            };
            var errorcallback = function (error) {
                alert('Some thing went Wrong, Contact Administrator');
                console.log('Post error', error);
            };

            $http({
                method: "POST", url: '/DashBoard/PurchaseNoteSupervised', async: true, params: { ID: id }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
            }).then(successcallback, errorcallback);
        };

    })
    .controller("ManagementPNDetailCtlr", function ($scope, $http) {

        $scope.$on('ManagementPNDetailCtlr', function (e, itm) {   
            $scope.MasterObject = itm;
            $scope.FilterByText = null;
            $scope.FilterValueByText = '';
            $scope.pageNavigation('first');
        });

        $scope.$on('init_ManagementPNDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            
        });

        init_Operations($scope, $http,
            '/DashBoard/PurchaseNoteDetailLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        $scope.GetpageNavigationResponse = function (data) {
            $scope.pageddata = data.pageddata;
            
        };
        
        $scope.OpenPODetailModal = function (itm) {
            $('#PODetailModal').modal('show');   
            $scope.PODetailModalitems = itm;
        };
      

    })
    .controller("ManagementPRNCtlr", function ($scope, $http) {

        $scope.$on('ManagementPRNCtlr', function (e, itm) {
            $scope.FilterByText = null;
            $scope.FilterValueByText = '';
            $scope.pageNavigation('first');
        });

        $scope.$on('init_ManagementPRNCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });


        init_Operations($scope, $http,
            '/DashBoard/PurchaseReturnNoteLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.pageNavigatorParam = function () { return { IsFor: $scope.IsFor }; };

        $scope.GetpageNavigationResponse = function (data) {
            $scope.pageddata = data.pageddata;
            $scope.data.Pen.PRNPendingSupervised = data.pageddata.otherdata.PRNPendingSupervised;
            $scope.data.Pen.PRNPendingProcessed = data.pageddata.otherdata.PRNPendingProcessed;
        };

        $scope.Supervised = function (id) {

            var successcallback = function (response) {
                if (response.data === 'OK') {
                    $scope.pageNavigation('Load');
                }
                else {
                    alert(response.data);
                }
            };
            var errorcallback = function (error) {
                alert('Some thing went Wrong, Contact Administrator');
                console.log('Post error', error);
            };

            $http({
                method: "POST", url: '/DashBoard/PurchaseReturnNoteSupervised', async: true, params: { ID: id }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
            }).then(successcallback, errorcallback);
        };

    })
    .controller("ManagementSNCtlr", function ($scope, $http) {

        $scope.$on('ManagementSNCtlr', function (e, itm) {
            $scope.FilterByText = null;
            $scope.FilterValueByText = '';
            $scope.pageNavigation('first');
        });

        $scope.$on('init_ManagementSNCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });


        init_Operations($scope, $http,
            '/DashBoard/SalesNoteLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.pageNavigatorParam = function () { return { IsFor: $scope.IsFor }; };

        $scope.GetpageNavigationResponse = function (data) {
            $scope.pageddata = data.pageddata;
            $scope.data.Pen.SNPendingSupervised = data.pageddata.otherdata.SNPendingSupervised;
            $scope.data.Pen.SNPendingProcessed = data.pageddata.otherdata.SNPendingProcessed;
        };

        $scope.Supervised = function (id) {

            var successcallback = function (response) {
                if (response.data === 'OK') {
                    $scope.pageNavigation('Load');
                }
                else {
                    alert(response.data);
                }
            };
            var errorcallback = function (error) {
                alert('Some thing went Wrong, Contact Administrator');
                console.log('Post error', error);
            };

            $http({
                method: "POST", url: '/DashBoard/SalesNoteSupervised', async: true, params: { ID: id }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
            }).then(successcallback, errorcallback);
        };

    })
    .controller("ManagementSRNCtlr", function ($scope, $http) {

        $scope.$on('ManagementSRNCtlr', function (e, itm) {
            $scope.FilterByText = null;
            $scope.FilterValueByText = '';
            $scope.pageNavigation('first');
        });

        $scope.$on('init_ManagementSRNCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });


        init_Operations($scope, $http,
            '/DashBoard/SalesReturnNoteLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.pageNavigatorParam = function () { return { IsFor: $scope.IsFor }; };

        $scope.GetpageNavigationResponse = function (data) {
            $scope.pageddata = data.pageddata;
            $scope.data.Pen.SRNPendingSupervised = data.pageddata.otherdata.SRNPendingSupervised;
            $scope.data.Pen.SRNPendingProcessed = data.pageddata.otherdata.SRNPendingProcessed;
        };

        $scope.Supervised = function (id) {

            var successcallback = function (response) {
                if (response.data === 'OK') {
                    $scope.pageNavigation('Load');
                }
                else {
                    alert(response.data);
                }
            };
            var errorcallback = function (error) {
                alert('Some thing went Wrong, Contact Administrator');
                console.log('Post error', error);
            };

            $http({
                method: "POST", url: '/DashBoard/SalesReturnNoteSupervised', async: true, params: { ID: id }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
            }).then(successcallback, errorcallback);
        };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


