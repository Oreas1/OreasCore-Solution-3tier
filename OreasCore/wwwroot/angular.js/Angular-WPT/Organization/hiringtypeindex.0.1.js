﻿MainModule
    .controller("HiringTypeIndexCtlr", function ($scope, $window, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/WPT/Organization/HiringTypeLoad', //--v_Load
            '/WPT/Organization/HiringTypeGet', // getrow
            '/WPT/Organization/HiringTypePost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Organization/GetInitializedHiringType');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'HiringTypeIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'HiringTypeIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'HiringTypeIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_WPT_HiringType = {
            'ID': 0, 'TypeName': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_HiringTypes = [$scope.tbl_WPT_HiringType];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_HiringType = {
                'ID': 0, 'TypeName': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_HiringType }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_HiringType = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


