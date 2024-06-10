MainModule
    .controller("DeductibleTypeIndexCtlr", function ($scope, $window, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/WPT/Organization/DeductibleTypeLoad', //--v_Load
            '/WPT/Organization/DeductibleTypeGet', // getrow
            '/WPT/Organization/DeductibleTypePost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Organization/GetInitializedDeductibleType');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'DeductibleTypeIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'DeductibleTypeIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'DeductibleTypeIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_WPT_DeductibleType = {
            'ID': 0, 'DeductibleName': '', 'Prefix': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_DeductibleTypes = [$scope.tbl_WPT_DeductibleType];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_DeductibleType = {
                'ID': 0, 'DeductibleName': '', 'Prefix': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_DeductibleType }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_DeductibleType = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


