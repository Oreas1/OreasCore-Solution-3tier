MainModule
    .controller("LoanTypeIndexCtlr", function ($scope, $window, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/WPT/Organization/LoanTypeLoad', //--v_Load
            '/WPT/Organization/LoanTypeGet', // getrow
            '/WPT/Organization/LoanTypePost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Organization/GetInitializedLoanType');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'LoanTypeIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'LoanTypeIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'LoanTypeIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_WPT_LoanType = {
            'ID': 0, 'LoanType': '', 'Prefix': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_LoanTypes = [$scope.tbl_WPT_LoanType];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_LoanType = {
                'ID': 0, 'LoanType': '', 'Prefix': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_LoanType }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_LoanType = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


