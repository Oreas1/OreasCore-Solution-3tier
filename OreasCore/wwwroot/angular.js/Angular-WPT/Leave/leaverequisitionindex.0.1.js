MainModule
    .controller("LeaveRequisitionMasterCtlr", function ($scope, $window, $http) {
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
            '/WPT/Leave/LeaveRequisitionMasterLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );
        init_ViewSetup($scope, $http, '/WPT/Leave/GetInitializedLeaveRequisition');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'LeaveRequisitionMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'LeaveRequisitionMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'LeaveRequisitionMasterCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'LeaveRequisitionDetailCtlr') != undefined) {
                $scope.$broadcast('init_LeaveRequisitionDetailCtlr', data.find(o => o.Controller === 'LeaveRequisitionDetailCtlr'));
            }
        };

        init_EmployeeSearchModalGeneral($scope, $http);
        init_LeavePolicySearchModal($scope, $http);

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_CalendarYear_Monthss = [$scope.tbl_WPT_CalendarYear_Months];
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };       
    })
    .controller("LeaveRequisitionDetailCtlr", function ($scope, $window, $http) {
        $scope.IsMonthClosed = true;
        $scope.MasterObject = {};
        $scope.$on('LeaveRequisitionDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.MinRange = new Date($scope.MasterObject.CalendarMonthStartDate);
            $scope.MaxRange = new Date($scope.MasterObject.CalendarMonthEndDate);
            $scope.IsMonthClosed = $scope.MasterObject.IsMonthClosed;
            
        });
        $scope.$on('init_LeaveRequisitionDetailCtlr', function (e, itm) {
            $scope.ActionList = itm.Otherdata === null ? [] : itm.Otherdata.ActionList;
            init_Filter($scope, itm.WildCard, null, null, itm.LoadByCard);
        });

        init_Operations($scope, $http,
            '/WPT/Leave/LeaveRequisitionDetailLoad', //--v_Load
            '/WPT/Leave/LeaveRequisitionDetailGet', // getrow
            '/WPT/Leave/LeaveRequisitionDetailPost' // PostRow
        );

        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_Employee_ID = item.ID;
                $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_Employee_IDName = item.EmployeeName;                
            }
            else {
                $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_Employee_ID = null;
                $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_Employee_IDName = null;
            }
            $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_LeavePolicyNonPaid_ID = null;
            $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_LeavePolicy_ID = null;
            $scope.tbl_WPT_LeaveRequisition.LeavePolicyName = null;
        };

        $scope.LeavePolicySearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.LeaveType === 'NP') {
                $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_LeavePolicyNonPaid_ID = item.ID;
                $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_LeavePolicy_ID = null;
            }
            else if (item.LeaveType === 'P') {
                $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_LeavePolicy_ID = item.ID;
                $scope.tbl_WPT_LeaveRequisition.FK_tbl_WPT_LeavePolicyNonPaid_ID = null;

            }
            $scope.tbl_WPT_LeaveRequisition.LeavePolicyName = item.PolicyName;
        };

        $scope.tbl_WPT_LeaveRequisition = {
            'ID': 0, 'DocNo': null, 'DocDate': new Date,
            'LeaveValue': 1, 'FK_tbl_WPT_CalendarYear_Months_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
            'FK_tbl_WPT_LeavePolicy_ID': null, 'FK_tbl_WPT_LeavePolicyNonPaid_ID': '', 'LeavePolicyName': '',
            'LeaveFrom': new Date($scope.MasterObject.MonthStart), 'LeaveTill': new Date($scope.MasterObject.MonthStart), 'Reason': '',
            'FK_tbl_WPT_ActionList_ID_HOS': 3, 'FK_tbl_WPT_ActionList_ID_HOSName': 'FK_tbl_WPT_Employee_ID_HOS', '': null, 'FK_tbl_WPT_Employee_ID_HOSName': '',
            'FK_tbl_WPT_ActionList_ID_HR': 1, 'FK_tbl_WPT_ActionList_ID_HRName': '', 'FK_tbl_WPT_Employee_ID_HR': null, 'FK_tbl_WPT_Employee_ID_HRName': '',
            'FK_tbl_WPT_ActionList_ID_Final': 3, 'FK_tbl_WPT_ActionList_ID_FinalName': '',
            'RequesterFormCode': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_LeaveRequisitions = [$scope.tbl_WPT_LeaveRequisition];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_LeaveRequisition = {
                'ID': 0, 'DocNo': null, 'DocDate': new Date,
                'LeaveValue': 1, 'FK_tbl_WPT_CalendarYear_Months_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
                'FK_tbl_WPT_LeavePolicy_ID': null, 'FK_tbl_WPT_LeavePolicyNonPaid_ID': '', 'LeavePolicyName': '',
                'LeaveFrom': new Date($scope.MasterObject.MonthStart), 'LeaveTill': new Date($scope.MasterObject.MonthStart), 'Reason': '',
                'FK_tbl_WPT_ActionList_ID_HOS': 3, 'FK_tbl_WPT_ActionList_ID_HOSName': 'FK_tbl_WPT_Employee_ID_HOS', '': null, 'FK_tbl_WPT_Employee_ID_HOSName': '',
                'FK_tbl_WPT_ActionList_ID_HR': 1, 'FK_tbl_WPT_ActionList_ID_HRName': '', 'FK_tbl_WPT_Employee_ID_HR': null, 'FK_tbl_WPT_Employee_ID_HRName': '',
                'FK_tbl_WPT_ActionList_ID_Final': 3, 'FK_tbl_WPT_ActionList_ID_FinalName': '',
                'RequesterFormCode': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_LeaveRequisition }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_LeaveRequisition = data;
            $scope.tbl_WPT_LeaveRequisition.DocDate = new Date(data.DocDate);
            $scope.tbl_WPT_LeaveRequisition.LeaveFrom = new Date(data.LeaveFrom);
            $scope.tbl_WPT_LeaveRequisition.LeaveTill = new Date(data.LeaveTill);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    