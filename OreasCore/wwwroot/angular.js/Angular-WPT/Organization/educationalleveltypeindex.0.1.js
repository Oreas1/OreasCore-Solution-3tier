MainModule
    .controller("EducationalLevelIndexCtlr", function ($scope, $window, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/WPT/Organization/EducationalLevelTypeLoad', //--v_Load
            '/WPT/Organization/EducationalLevelTypeGet', // getrow
            '/WPT/Organization/EducationalLevelTypePost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Organization/GetInitializedEducationalLevelType');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'EducationalLevelIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'EducationalLevelIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'EducationalLevelIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_WPT_EducationalLevelType = {
            'ID': 0, 'LevelName': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_EducationalLevelTypes = [$scope.tbl_WPT_EducationalLevelType];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_EducationalLevelType = {
                'ID': 0, 'LevelName': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_EducationalLevelType }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_EducationalLevelType = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


