MainModule
    .controller("HolidayIndexCtlr", function ($scope, $window, $http) {
   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/WPT/Organization/HolidayLoad', //--v_Load
            '/WPT/Organization/HolidayGet', // getrow
            '/WPT/Organization/HolidayPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Organization/GetInitializedHoliday');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'HolidayIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'HolidayIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'HolidayIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_WPT_Holiday = {
            'ID': 0, 'HolidayName': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_Holidays = [$scope.tbl_WPT_Holiday];


        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_Holiday = {
                'ID': 0, 'HolidayName': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };

        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_Holiday }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_Holiday = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


