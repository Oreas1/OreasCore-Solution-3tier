MainModule
    .controller("IncrementIndexCtlr", function ($scope, $window, $http) {
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
            '/WPT/Increment/IncrementCalendarLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Increment/GetInitializedIncrement');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'IncrementIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'IncrementIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'IncrementIndexCtlr').WildCard, null, null, null);

                if (data.find(o => o.Controller === 'IncrementIndexCtlr').Otherdata === null) {
                    $scope.DesignationList = [];
                    $scope.DepartmentList = [];
                }
                else {
                    $scope.DesignationList = data.find(o => o.Controller === 'IncrementIndexCtlr').Otherdata.DesignationList;
                    $scope.DepartmentList = data.find(o => o.Controller === 'IncrementIndexCtlr').Otherdata.DepartmentList;
                }
                $scope.pageNavigation('first');
            }


            if (data.find(o => o.Controller === 'IncrementMasterCtlr') != undefined) {
                $scope.$broadcast('init_IncrementMasterCtlr', data.find(o => o.Controller === 'IncrementMasterCtlr'));
            }
            if (data.find(o => o.Controller === 'IncrementDetailEmployeeCtlr') != undefined) {
                $scope.$broadcast('init_IncrementDetailEmployeeCtlr', data.find(o => o.Controller === 'IncrementDetailEmployeeCtlr'));
            }
        };

        init_EmployeeSearchModalGeneral($scope, $http);
        init_MonthSearchModal($scope, $http);

        $scope.tbl_WPT_CalendarYear = {
            'ID': 0, 'CalendarYear': new Date().getFullYear() + 1, 'NoOfIncrementDoc': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_CalendarYears = [$scope.tbl_WPT_CalendarYear];
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("IncrementMasterCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('IncrementMasterCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_IncrementMasterCtlr', function (e, itm) {           
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/Increment/IncrementMasterLoad', //--v_Load
            '/WPT/Increment/IncrementMasterGet', // getrow
            '/WPT/Increment/IncrementMasterPost' // PostRow
        );

        $scope.tbl_WPT_IncrementMaster = {
            'ID': 0, 'DocNo': null, 'DocDate': new Date(), 'FK_tbl_WPT_CalendarYear_ID': $scope.MasterObject.ID,
            'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_IncrementMasters = [$scope.tbl_WPT_IncrementMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_IncrementMaster = {
                'ID': 0, 'DocNo': null, 'DocDate': new Date(), 'FK_tbl_WPT_CalendarYear_ID': $scope.MasterObject.ID,
                'Remarks': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {   return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_IncrementMaster };   };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_IncrementMaster = data;
            $scope.tbl_WPT_IncrementMaster = data; $scope.tbl_WPT_IncrementMaster.DocDate = new Date(data.DocDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("IncrementDetailEmployeeCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('IncrementDetailEmployeeCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });
        $scope.$on('init_IncrementDetailEmployeeCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/WPT/Increment/GetReport');
            $scope.IncrementByList = itm.Otherdata === null ? [] : itm.Otherdata.IncrementByList;
        });

        init_Operations($scope, $http,
            '/WPT/Increment/IncrementDetailEmployeeLoad', //--v_Load
            '/WPT/Increment/IncrementDetailEmployeeGet', // getrow
            '/WPT/Increment/IncrementDetailEmployeePost' // PostRow
        );

        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_IncrementDetail.FK_tbl_WPT_Employee_ID = item.ID;
                $scope.tbl_WPT_IncrementDetail.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_IncrementDetail.FK_tbl_WPT_Employee_ID = null;
                $scope.tbl_WPT_IncrementDetail.FK_tbl_WPT_Employee_IDName = null;
            }
        };

        $scope.MonthSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_IncrementDetail.FK_tbl_WPT_CalendarYear_Months_ID_ApplyArrear = item.ID;
                $scope.tbl_WPT_IncrementDetail.FK_tbl_WPT_CalendarYear_Months_ID_ApplyArrearName = item.MonthStart + ' TO ' + item.MonthEnd;
            }
            else {
                $scope.tbl_WPT_IncrementDetail.FK_tbl_WPT_CalendarYear_Months_ID_ApplyArrear = null;
                $scope.tbl_WPT_IncrementDetail.FK_tbl_WPT_CalendarYear_Months_ID_ApplyArrearName = null;
            }

        };

        $scope.tbl_WPT_IncrementDetail = {
            'ID': 0, 'FK_tbl_WPT_IncrementMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Employee_ID': 0, 'FK_tbl_WPT_Employee_IDName': '', 'EffectiveDate': new Date(),
            'IncrementValue': 0, 'FK_tbl_WPT_IncrementBy_ID': 2, 'FK_tbl_WPT_IncrementBy_IDName': '',
            'Arrear': 0, 'FK_tbl_WPT_CalendarYear_Months_ID_ApplyArrear': null, 'FK_tbl_WPT_CalendarYear_Months_ID_ApplyArrearName': '', 'Remarks': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_IncrementDetails = [$scope.tbl_WPT_IncrementDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_IncrementDetail = {
                'ID': 0, 'FK_tbl_WPT_IncrementMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Employee_ID': 0, 'FK_tbl_WPT_Employee_IDName': '', 'EffectiveDate': new Date(),
                'IncrementValue': 0, 'FK_tbl_WPT_IncrementBy_ID': 2, 'FK_tbl_WPT_IncrementBy_IDName': '',
                'Arrear': 0, 'FK_tbl_WPT_CalendarYear_Months_ID_ApplyArrear': null, 'FK_tbl_WPT_CalendarYear_Months_ID_ApplyArrearName': '', 'Remarks': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
            if ($scope.AddBulk === true) {
                $scope.FK_tbl_WPT_Designation_ID = null;
                $scope.FK_tbl_WPT_Department_ID = null;
                $scope.JoiningDateTill = new Date();
            }
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText, MasterID: $scope.MasterObject.ID, DesignationID: $scope.FK_tbl_WPT_Designation_ID, DepartmentID: $scope.FK_tbl_WPT_Department_ID, JoiningDate: new Date($scope.JoiningDateTill).toLocaleString('en-US')  }, data: $scope.tbl_WPT_IncrementDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_IncrementDetail = data;
            $scope.tbl_WPT_IncrementDetail.EffectiveDate = new Date(data.EffectiveDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        //-----------------------Excel Upload----------------------//
        $scope.LoadFileData = function (files) {
            var formData = new FormData();
            formData.append("IncrementExcelFile", files[0]);

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
                method: "POST", url: "/WPT/Increment/IncrementDetailEmployeeUploadExcelFile", params: { MasterID: $scope.MasterObject.ID, operation: 'Save New' }, data: formData, headers: { 'Content-Type': undefined, 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }, transformRequest: angular.identity
            }).then(successcallback, errorcallback);
        };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    