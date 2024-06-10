MainModule
    .controller("MachineIndexCtlr", function ($scope, $window, $http) {
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
            '/WPT/Machine/MachineLoad', //--v_Load
            '/WPT/Machine/MachineGet', // getrow
            '/WPT/Machine/MachinePost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Machine/GetInitializedMachine');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'MachineIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'MachineIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'MachineIndexCtlr').WildCard, null, null, null);                
                $scope.pageNavigation('first');               
            }
        };

        $scope.tbl_WPT_Machine = {
            'ID': 0, 'Name': '', 'No': 1, 'IP': '192.168.0.1', 'PortNo': 4370, 'AutoClearLogAfterDownload': false,
            'LastATLogDownloanded': null, 'LastATLogClear': null, 'LastATLogCount': 0,
            'ScheduledDownloadDailyAT': null, 'ScheduledDownloadDailyAT2': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_Machines = [$scope.tbl_WPT_Machine];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_Machine = {
                'ID': 0, 'Name': '', 'No': 1, 'IP': '192.168.0.1', 'PortNo': 4370, 'AutoClearLogAfterDownload': false,
                'LastATLogDownloanded': null, 'LastATLogClear': null, 'LastATLogCount': 0,
                'ScheduledDownloadDailyAT': null, 'ScheduledDownloadDailyAT2': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            if ($scope.ScheduledDownloadDailyAT == null)
                $scope.tbl_WPT_Machine.ScheduledDownloadDailyAT = null
            else
                $scope.tbl_WPT_Machine.ScheduledDownloadDailyAT = ($scope.ScheduledDownloadDailyAT.getHours() + ':' + $scope.ScheduledDownloadDailyAT.getMinutes() + ':' + $scope.ScheduledDownloadDailyAT.getSeconds()).toString();

            if ($scope.ScheduledDownloadDailyAT2 == null)
                $scope.tbl_WPT_Machine.ScheduledDownloadDailyAT2 = null
            else
                $scope.tbl_WPT_Machine.ScheduledDownloadDailyAT2 = ($scope.ScheduledDownloadDailyAT2.getHours() + ':' + $scope.ScheduledDownloadDailyAT2.getMinutes() + ':' + $scope.ScheduledDownloadDailyAT2.getSeconds()).toString();


            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_Machine };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_Machine = data;
            if (data.ScheduledDownloadDailyAT != null)
                $scope.ScheduledDownloadDailyAT = new Date(data.ScheduledDownloadDailyAT);
            else
                $scope.ScheduledDownloadDailyAT = null;

            if (data.ScheduledDownloadDailyAT2 != null)
                $scope.ScheduledDownloadDailyAT2 = new Date(data.ScheduledDownloadDailyAT2);
            else
                $scope.ScheduledDownloadDailyAT2 = null;
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    