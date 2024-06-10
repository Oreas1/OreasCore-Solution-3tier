MainModule
    .controller("EmployeeLevelIndexCtlr", function ($scope, $window, $http) {
   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/WPT/Organization/EmployeeLevelLoad', //--v_Load
            '/WPT/Organization/EmployeeLevelGet', // getrow
            '/WPT/Organization/EmployeeLevelPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Organization/GetInitializedEmployeeLevel');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'EmployeeLevelIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'EmployeeLevelIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'EmployeeLevelIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_WPT_EmployeeLevel = {
            'ID': 0, 'LevelName': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_EmployeeLevels = [$scope.tbl_WPT_EmployeeLevel];


        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_EmployeeLevel = {
                'ID': 0, 'LevelName': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };

        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_EmployeeLevel }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_EmployeeLevel = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


