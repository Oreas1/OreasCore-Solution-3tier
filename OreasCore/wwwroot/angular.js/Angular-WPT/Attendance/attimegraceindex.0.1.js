MainModule
    .controller("ATTimeGraceIndexCtlr", function ($scope, $window, $http) {
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
            '/WPT/Attendance/ATTimeGraceLoad', //--v_Load
            '/WPT/Attendance/ATTimeGraceGet', // getrow
            '/WPT/Attendance/ATTimeGracePost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Attendance/GetInitializedATTimeGrace');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'ATTimeGraceIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'ATTimeGraceIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'ATTimeGraceIndexCtlr').WildCard, null, null, null);                
                $scope.pageNavigation('first');              
            }
            if (data.find(o => o.Controller === 'ATTimeGraceEmployeeCtlr') != undefined) {
                $scope.$broadcast('init_ATTimeGraceEmployeeCtlr', data.find(o => o.Controller === 'ATTimeGraceEmployeeCtlr'));
            }
        };

        init_EmployeeSearchModalGeneral($scope, $http);

        $scope.tbl_WPT_ATTimeGrace = {
            'ID': 0, 'DocNo': '', 'DateFrom': new Date(), 'DateTill': new Date(),
            'Ignore_LI': false, 'Ignore_EO': false, 'Ignore_HS': false,
            'Ignore_OT': false, 'Ignore_Present': false, 'Ignore_Absent': false, 'Ignore_AutoLeaves': false, 'Ignore_Holidays': false,
            'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
            'TotalEmployees': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_ATTimeGraces = [$scope.tbl_WPT_ATTimeGrace];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_ATTimeGrace = {
                'ID': 0, 'DocNo': '', 'DateFrom': new Date(), 'DateTill': new Date(),
                'Ignore_LI': false, 'Ignore_EO': false, 'Ignore_HS': false,
                'Ignore_OT': false, 'Ignore_Present': false, 'Ignore_Absent': false, 'Ignore_AutoLeaves': false, 'Ignore_Holidays': false,
                'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
                'TotalEmployees': 0
            };

        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_ATTimeGrace };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_ATTimeGrace = data;
            $scope.tbl_WPT_ATTimeGrace.DateFrom = new Date(data.DateFrom);
            $scope.tbl_WPT_ATTimeGrace.DateTill = new Date(data.DateTill);
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("ATTimeGraceEmployeeCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('ATTimeGraceEmployeeCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_ATTimeGraceEmployeeCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/Attendance/ATTimeGraceEmployeeLoad', //--v_Load
            '/WPT/Attendance/ATTimeGraceEmployeeGet', // getrow
            '/WPT/Attendance/ATTimeGraceEmployeePost' // PostRow
        );

        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_ATTimeGraceEmployeeLink.FK_tbl_WPT_Employee_ID = item.ID;
                $scope.tbl_WPT_ATTimeGraceEmployeeLink.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_ATTimeGraceEmployeeLink.FK_tbl_WPT_Employee_ID = null;
                $scope.tbl_WPT_ATTimeGraceEmployeeLink.FK_tbl_WPT_Employee_IDName = null;
            }
        };

        $scope.tbl_WPT_ATTimeGraceEmployeeLink = {
            'ID': 0, 'FK_tbl_WPT_ATTimeGrace_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_ATTimeGraceEmployeeLinks = [$scope.tbl_WPT_ATTimeGraceEmployeeLink];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_ATTimeGraceEmployeeLink = {
                'ID': 0, 'FK_tbl_WPT_ATTimeGrace_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };

        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_ATTimeGraceEmployeeLink }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_ATTimeGraceEmployeeLink = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        //-----------------------Excel Upload----------------------//
        $scope.LoadFileData = function (files) {
            var formData = new FormData();
            formData.append("ATGraceExcelFile", files[0]);

            var successcallback = function (response) {
                if (response.data === 'OK') {
                    document.getElementById('UploadExcelFile').value = '';
                    $scope.pageNavigation('first');
                    alert('Successfully Updated');
                }
                else {
                    console.log(response.data);
                }
            };
            var errorcallback = function (error) {
            };

            $http({
                method: "POST", url: "/WPT/Attendance/ATTimeGraceEmployeeUploadExcelFile", params: { MasterID: $scope.MasterObject.ID, operation: 'Save New' }, data: formData, headers: { 'Content-Type': undefined, 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }, transformRequest: angular.identity
            }).then(successcallback, errorcallback);
        };
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    