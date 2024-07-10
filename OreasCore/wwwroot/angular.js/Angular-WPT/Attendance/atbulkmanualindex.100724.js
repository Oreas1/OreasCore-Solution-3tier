MainModule
    .controller("ATBulkManualMasterCtlr", function ($scope, $window, $http) {
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
            '/WPT/Attendance/ATBulkManualMasterLoad', //--v_Load
            '/WPT/Attendance/ATBulkManualMasterGet', // getrow
            '/WPT/Attendance/ATBulkManualMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Attendance/GetInitializedATBulkManual');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'ATBulkManualMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'ATBulkManualMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'ATBulkManualMasterCtlr').WildCard, null, null, null);                
                if (data.find(o => o.Controller === 'ATBulkManualMasterCtlr').Otherdata === null) {
                    $scope.ATInOutModeList = [];
                }
                else {
                    $scope.ATInOutModeList = data.find(o => o.Controller === 'ATBulkManualMasterCtlr').Otherdata.ATInOutModeList;
                }

                $scope.pageNavigation('first');              
            }
            if (data.find(o => o.Controller === 'ATBulkManualDetailEmployeeCtlr') != undefined) {
                $scope.$broadcast('init_ATBulkManualDetailEmployeeCtlr', data.find(o => o.Controller === 'ATBulkManualDetailEmployeeCtlr'));
            }
        };

        init_EmployeeSearchModalGeneral($scope, $http);

        $scope.tbl_WPT_ATBulkManualMaster = {
            'ID': 0, 'DocNo': '', 'ATDateTime': new Date(), 
            'FK_tbl_WPT_ATInOutMode_ID': null, 'FK_tbl_WPT_ATInOutMode_IDName': '', 'Reason': null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_ATBulkManualMasters = [$scope.tbl_WPT_ATBulkManualMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_ATBulkManualMaster = {
                'ID': 0, 'DocNo': '', 'ATDateTime': new Date(),
                'FK_tbl_WPT_ATInOutMode_ID': null, 'FK_tbl_WPT_ATInOutMode_IDName': '', 'Reason': null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_ATBulkManualMaster };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_ATBulkManualMaster = data;
            $scope.tbl_WPT_ATBulkManualMaster.ATDateTime = new Date(data.ATDateTime);
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("ATBulkManualDetailEmployeeCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('ATBulkManualDetailEmployeeCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_ATBulkManualDetailEmployeeCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/Attendance/ATBulkManualDetailLoad', //--v_Load
            '/WPT/Attendance/ATBulkManualDetailGet', // getrow
            '/WPT/Attendance/ATBulkManualDetailPost' // PostRow
        );

        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_ATBulkManualDetail_Employee.FK_tbl_WPT_Employee_ID = item.ID;
                $scope.tbl_WPT_ATBulkManualDetail_Employee.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_ATBulkManualDetail_Employee.FK_tbl_WPT_Employee_ID = null;
                $scope.tbl_WPT_ATBulkManualDetail_Employee.FK_tbl_WPT_Employee_IDName = null;
            }
        };

        $scope.tbl_WPT_ATBulkManualDetail_Employee = {
            'ID': 0, 'FK_tbl_WPT_ATBulkManualMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_ATBulkManualDetail_Employees = [$scope.tbl_WPT_ATBulkManualDetail_Employee];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_ATBulkManualDetail_Employee = {
                'ID': 0, 'FK_tbl_WPT_ATBulkManualMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_ATBulkManualDetail_Employee }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_ATBulkManualDetail_Employee = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        //-----------------------Excel Upload----------------------//
        $scope.LoadFileData = function (files) {
            var formData = new FormData();
            formData.append("ELExcelFile", files[0]);

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
                method: "POST", url: "/WPT/Attendance/ATBulkManualDetailUploadExcelFile", params: { MasterID: $scope.MasterObject.ID, operation: 'Save New' }, data: formData, headers: { 'Content-Type': undefined, 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }, transformRequest: angular.identity
            }).then(successcallback, errorcallback);
        };
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    