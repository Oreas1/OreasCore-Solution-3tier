MainModule
    .controller("ShiftIndexCtlr", function ($scope, $window, $http) {
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
            '/WPT/Shift/ShiftLoad', //--v_Load
            '/WPT/Shift/ShiftGet', // getrow
            '/WPT/Shift/ShiftPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Shift/GetInitializedShift');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'ShiftIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'ShiftIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'ShiftIndexCtlr').WildCard, null, null, null);                
                $scope.pageNavigation('first');               
            }
            if (data.find(o => o.Controller === 'DefaultEmployeeShiftCtlr') != undefined) {
                $scope.$broadcast('init_DefaultEmployeeShiftCtlr', data.find(o => o.Controller === 'DefaultEmployeeShiftCtlr'));
            }
        };

        init_EmployeeSearchModalGeneral($scope, $http);

        $scope.tbl_WPT_Shift = {
            'ID': 0, 'ShiftName': '', 'Prefix': '',
            'StartTime': null, 'LateInTime': null,
            'EarlyOutTime': null, 'EndTime': null,
            'CheckInStartTime': null, 'CheckOutEndTime': null,
            'HalfShiftLimit_Minutes': 300, 'ShiftLimit_Minutes': 0, 'OTMargin_Minutes': 0,
            'LI': true, 'EO': true, 'OT_HD': true, 'OT_BeforeShift_NON_HD': true, 'OT_AfterShift_NON_HD': true,
            'HS': true, 'HD': true, 'ShiftLimit': true,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_Shifts = [$scope.tbl_WPT_Shift];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_Shift = {
                'ID': 0, 'ShiftName': '', 'Prefix': '',
                'StartTime': null, 'LateInTime': null,
                'EarlyOutTime': null, 'EndTime': null,
                'CheckInStartTime': null, 'CheckOutEndTime': null,
                'HalfShiftLimit_Minutes': 300, 'ShiftLimit_Minutes': 0, 'OTMargin_Minutes': 0,
                'LI': true, 'EO': true, 'OT_HD': true, 'OT_BeforeShift_NON_HD': true, 'OT_AfterShift_NON_HD': true,
                'HS': true, 'HD': true, 'ShiftLimit': true,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };

        };
        $scope.postRowParam = function () {
            //---------------------convert datetime to only timespan before return back data to save on server-------------------//
            $scope.tbl_WPT_Shift.StartTime = ($scope.StartTime.getHours() + ':' + $scope.StartTime.getMinutes() + ':' + $scope.StartTime.getSeconds()).toString();
            $scope.tbl_WPT_Shift.EndTime = ($scope.EndTime.getHours() + ':' + $scope.EndTime.getMinutes() + ':' + $scope.EndTime.getSeconds()).toString();
            $scope.tbl_WPT_Shift.LateInTime = ($scope.LateInTime.getHours() + ':' + $scope.LateInTime.getMinutes() + ':' + $scope.LateInTime.getSeconds()).toString();
            $scope.tbl_WPT_Shift.EarlyOutTime = ($scope.EarlyOutTime.getHours() + ':' + $scope.EarlyOutTime.getMinutes() + ':' + $scope.EarlyOutTime.getSeconds()).toString();
            $scope.tbl_WPT_Shift.CheckInStartTime = ($scope.CheckInStartTime.getHours() + ':' + $scope.CheckInStartTime.getMinutes() + ':00').toString();
            $scope.tbl_WPT_Shift.CheckOutEndTime = ($scope.CheckOutEndTime.getHours() + ':' + $scope.CheckOutEndTime.getMinutes() + ':00').toString();
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_Shift };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_Shift = data;

            //-------------convert timespan to datetime completely to show in html Input<time> --------------------------//
            $scope.StartTime = new Date(data.StartTime);//new Date(1900, 1, 1, data.StartTime.Hours, data.StartTime.Minutes, data.StartTime.Seconds, 0);
            $scope.EndTime = new Date(data.EndTime);//new Date(1900, 1, 1, data.EndTime.Hours, data.EndTime.Minutes, data.EndTime.Seconds, 0);
            $scope.LateInTime = new Date(data.LateInTime);//new Date(1900, 1, 1, data.LateInTime.Hours, data.LateInTime.Minutes, data.LateInTime.Seconds, 0);
            $scope.EarlyOutTime = new Date(data.EarlyOutTime);//new Date(1900, 1, 1, data.EarlyOutTime.Hours, data.EarlyOutTime.Minutes, data.EarlyOutTime.Seconds, 0);
            $scope.CheckInStartTime = new Date(data.CheckInStartTime);//new Date(1900, 1, 1, data.CheckInStartTime.Hours, data.CheckInStartTime.Minutes, data.CheckInStartTime.Seconds, 0);
            $scope.CheckOutEndTime = new Date(data.CheckOutEndTime);//new Date(1900, 1, 1, data.CheckOutEndTime.Hours, data.CheckOutEndTime.Minutes, data.CheckOutEndTime.Seconds, 0);
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("DefaultEmployeeShiftCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('DefaultEmployeeShiftCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_DefaultEmployeeShiftCtlr', function (e, itm) {
            $scope.SectionList = itm.Otherdata === null ? [] : itm.Otherdata.SectionList;
            $scope.DesignationList = itm.Otherdata === null ? [] : itm.Otherdata.DesignationList;
            $scope.BulkByList = itm.Otherdata === null ? [] : itm.Otherdata.BulkByList;
            init_Filter($scope, itm.WildCard, null, null, itm.LoadByCard);
        });

        init_Operations($scope, $http,
            '/WPT/Shift/DefaultEmployeeShiftLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        $scope.UpdateBulk = function () {

            var successcallback = function (response) {
                if (response.data === 'OK') { $scope.pageNavigation('Load'); }
                else { alert(response.data); }
            };
            var errorcallback = function (error) { };

            if (confirm("Are you sure! you want to Allocate Defualt Shifts to bulk employees ") === true) {
                $http({
                    method: "POST", url: "/WPT/Shift/DefaultEmployeeShiftBulkPost", params: { ShiftID: $scope.MasterObject.ID, SectionID: $scope.FK_tbl_WPT_DepartmentDetail_Section_ID, DesignationID: $scope.FK_tbl_WPT_Designation_ID, BulkBy: $scope.BulkBy, operation: 'Save Update' }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken}
                }).then(successcallback, errorcallback);
            }
        };
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    