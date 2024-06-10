MainModule
    .controller("LeavePolicyNonPaidIndexCtlr", function ($scope, $window, $http) {
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
            '/WPT/Leave/LeavePolicyNonPaidLoad', //--v_Load
            '/WPT/Leave/LeavePolicyNonPaidGet', // getrow
            '/WPT/Leave/LeavePolicyNonPaidPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Leave/GetInitializedLeavePolicyNonPaid');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'LeavePolicyNonPaidIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'LeavePolicyNonPaidIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'LeavePolicyNonPaidIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'LeavePolicyNonPaidDesignationCtlr') != undefined) {
                $scope.$broadcast('init_LeavePolicyNonPaidDesignationCtlr', data.find(o => o.Controller === 'LeavePolicyNonPaidDesignationCtlr'));
            }
        };

        $scope.tbl_WPT_LeavePolicyNonPaid = {
            'ID': 0, 'PolicyName': '', 'PolicyPrefix': '',
            'IsHOSApprovalReq': true, 'IsHRApprovalReq': true, 'FinalGranter': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_LeavePolicyNonPaids = [$scope.tbl_WPT_LeavePolicyNonPaid];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_LeavePolicyNonPaid = {
                'ID': 0, 'PolicyName': '', 'PolicyPrefix': '',
                'IsHOSApprovalReq': true, 'IsHRApprovalReq': true, 'FinalGranter': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_LeavePolicyNonPaid };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_LeavePolicyNonPaid = data;
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };       
    })
    .controller("LeavePolicyNonPaidDesignationCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('LeavePolicyNonPaidDesignationCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_LeavePolicyNonPaidDesignationCtlr', function (e, itm) {
            $scope.DesignationList = itm.Otherdata === null ? [] : itm.Otherdata.DesignationList;
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/Leave/LeavePolicyNonPaidDesignationLoad', //--v_Load
            '/WPT/Leave/LeavePolicyNonPaidDesignationGet', // getrow
            '/WPT/Leave/LeavePolicyNonPaidDesignationPost' // PostRow
        );

        $scope.tbl_WPT_LeavePolicyNonPaid_Designation = {
            'ID': 0, 'FK_tbl_WPT_LeavePolicyNonPaid_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Designation_ID': null, 'FK_tbl_WPT_Designation_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_LeavePolicyNonPaid_Designations = [$scope.tbl_WPT_LeavePolicyNonPaid_Designation];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_LeavePolicyNonPaid_Designation = {
                'ID': 0, 'FK_tbl_WPT_LeavePolicyNonPaid_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Designation_ID': null, 'FK_tbl_WPT_Designation_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_LeavePolicyNonPaid_Designation }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_LeavePolicyNonPaid_Designation = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    