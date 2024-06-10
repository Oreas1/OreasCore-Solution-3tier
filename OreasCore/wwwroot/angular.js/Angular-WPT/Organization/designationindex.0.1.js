MainModule
    .controller("DesignationIndexCtlr", function ($scope, $window, $http) {
       
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/WPT/Organization/DesignationLoad', //--v_Load
            '/WPT/Organization/DesignationGet', // getrow
            '/WPT/Organization/DesignationPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Organization/GetInitializedDesignation');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'DesignationIndexCtlr') != undefined) {          
                $scope.Privilege = data.find(o => o.Controller === 'DesignationIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'DesignationIndexCtlr').WildCard, null, null, null);
                init_Report($scope, data.find(o => o.Controller === 'DesignationIndexCtlr').Reports, '/WPT/Organization/GetReport');
                $scope.pageNavigation('first');
            }
        };    

        $scope.tbl_WPT_Designation = {
            'ID': 0, 'Designation': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };


        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_Designation = {
                'ID': 0, 'Designation': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_Designation };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_Designation = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: 0 }; };


    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


