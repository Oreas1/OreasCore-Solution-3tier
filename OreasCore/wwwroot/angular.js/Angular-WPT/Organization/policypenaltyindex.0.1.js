MainModule
    .controller("PolicyPenaltyIndexCtlr", function ($scope, $window, $http) {
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
            '/WPT/Organization/PolicyPenaltyLoad', //--v_Load
            '/WPT/Organization/PolicyPenaltyGet', // getrow
            '/WPT/Organization/PolicyPenaltyPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Organization/GetInitializedPolicyPenalty');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'PolicyPenaltyIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'PolicyPenaltyIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'PolicyPenaltyIndexCtlr').WildCard, null, null, null);                
                $scope.pageNavigation('first');               
            }
            if (data.find(o => o.Controller === 'PolicyPenaltyDesignationCtlr') != undefined) {
                $scope.$broadcast('init_PolicyPenaltyDesignationCtlr', data.find(o => o.Controller === 'PolicyPenaltyDesignationCtlr'));
            }
        };

        init_EmployeeSearchModalGeneral($scope, $http);

        $scope.tbl_WPT_PolicyPenaltyOnWT = {
            'ID': 0, 'PolicyName': '',
            'PenaltyAbsentAtEvery_LI': 0, 'PenaltyHalfShiftFromEvery_LI': 0,
            'MonthlyLateInGraceLimit_Minutes_MGLI': 0,'PenaltyHalfShiftFromEveryMinutes_MGLI': 0,
            'PenaltyAbsentAtEvery_EO': 0, 'PenaltyHalfShiftFromEvery_EO': 0,
            'PenaltyAbsentAtEvery_HS': 0, 'KeepHalfShiftFromEvery_HS': 0,
            'PenaltyAbsentOnMissingINorOUT': false, 'PenaltyHalfShiftOnMissingINorOUT': false, 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_PolicyPenaltyOnWTs = [$scope.tbl_WPT_PolicyPenaltyOnWT];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_PolicyPenaltyOnWT = {
                'ID': 0, 'PolicyName': '',
                'PenaltyAbsentAtEvery_LI': 0, 'PenaltyHalfShiftFromEvery_LI': 0,
                'MonthlyLateInGraceLimit_Minutes_MGLI': 0, 'PenaltyHalfShiftFromEveryMinutes_MGLI': 0,
                'PenaltyAbsentAtEvery_EO': 0, 'PenaltyHalfShiftFromEvery_EO': 0,
                'PenaltyAbsentAtEvery_HS': 0, 'KeepHalfShiftFromEvery_HS': 0,
                'PenaltyAbsentOnMissingINorOUT': false, 'PenaltyHalfShiftOnMissingINorOUT': false, 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {

            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_PolicyPenaltyOnWT };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_PolicyPenaltyOnWT = data;
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

        $scope.ValidatePolicy = function ()
        {
            if ($scope.tbl_WPT_PolicyPenaltyOnWT.PenaltyAbsentAtEvery_LI > 0)
            {
                $scope.tbl_WPT_PolicyPenaltyOnWT.PenaltyHalfShiftFromEvery_LI = 0;
                $scope.tbl_WPT_PolicyPenaltyOnWT.PenaltyMinutesAfterMonthlyGraceLI_LI = 0;
            }

        };
       
    })
    .controller("PolicyPenaltyDesignationCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('PolicyPenaltyDesignationCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_PolicyPenaltyDesignationCtlr', function (e, itm) {
            $scope.DesignationList = itm.Otherdata === null ? [] : itm.Otherdata.DesignationList;
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/Organization/PolicyPenaltyDesignationLoad', //--v_Load
            '/WPT/Organization/PolicyPenaltyDesignationGet', // getrow
            '/WPT/Organization/PolicyPenaltyDesignationPost' // PostRow
        );

        $scope.tbl_WPT_PolicyPenaltyOnWTDetail_Designation = {
            'ID': 0, 'FK_tbl_WPT_PolicyPenaltyOnWT_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Designation_ID': null, 'FK_tbl_WPT_Designation_IDName': '',
            'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_PolicyPenaltyOnWTDetail_Designations = [$scope.tbl_WPT_PolicyPenaltyOnWTDetail_Designation];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_PolicyPenaltyOnWTDetail_Designation = {
                'ID': 0, 'FK_tbl_WPT_PolicyPenaltyOnWT_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Designation_ID': null, 'FK_tbl_WPT_Designation_IDName': '',
                'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_PolicyPenaltyOnWTDetail_Designation }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_PolicyPenaltyOnWTDetail_Designation = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    