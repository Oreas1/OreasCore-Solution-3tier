MainModule
    .controller("EmploymentTypeIndexCtlr", function ($scope, $window, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/WPT/Organization/EmploymentTypeLoad', //--v_Load
            '/WPT/Organization/EmploymentTypeGet', // getrow
            '/WPT/Organization/EmploymentTypePost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Organization/GetInitializedEmploymentType');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'EmploymentTypeIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'EmploymentTypeIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'EmploymentTypeIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_WPT_EmploymentType = {
            'ID': 0, 'TypeName': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_EmploymentTypes = [$scope.tbl_WPT_EmploymentType];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_EmploymentType = {
                'ID': 0, 'TypeName': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_EmploymentType }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_EmploymentType = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


