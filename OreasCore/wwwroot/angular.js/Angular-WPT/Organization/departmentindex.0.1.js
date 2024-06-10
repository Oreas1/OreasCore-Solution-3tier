MainModule
    .controller("DepartmentIndexCtlr", function ($scope, $window, $http) {
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
            '/WPT/Organization/DepartmentLoad', //--v_Load
            '/WPT/Organization/DepartmentGet', // getrow
            '/WPT/Organization/DepartmentPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Organization/GetInitializedDepartment');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'DepartmentIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'DepartmentIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'DepartmentIndexCtlr').WildCard, null, null, null);
                $scope.DepartmentList = data.find(o => o.Controller === 'DepartmentIndexCtlr').Otherdata === null ? [] : data.find(o => o.Controller === 'DepartmentIndexCtlr').Otherdata.DepartmentList;
                $scope.pageNavigation('first');
                $scope.LoadNode();
            }
            if (data.find(o => o.Controller === 'DepartmentDesignationCtlr') != undefined) {
                $scope.$broadcast('init_DepartmentDesignationCtlr', data.find(o => o.Controller === 'DepartmentDesignationCtlr'));
            }
            if (data.find(o => o.Controller === 'DepartmentSectionsDetailCtlr') != undefined) {
                $scope.$broadcast('init_DepartmentSectionsDetailCtlr', data.find(o => o.Controller === 'DepartmentSectionsDetailCtlr'));
            }
            if (data.find(o => o.Controller === 'DepartmentSectionDetailHOSCtlr') != undefined) {
                $scope.$broadcast('init_DepartmentSectionDetailHOSCtlr', data.find(o => o.Controller === 'DepartmentSectionDetailHOSCtlr'));
            }
        };

        init_EmployeeSearchModalGeneral($scope, $http);

        $scope.tbl_WPT_Department = {
            'ID': 0, 'FK_tbl_WPT_Department_ID_Head': null, 'FK_tbl_WPT_Department_ID_HeadName': '', 'DepartmentName': '', 
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'ChildCount': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_Departments = [$scope.tbl_WPT_Department];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_Department = {
                'ID': 0, 'FK_tbl_WPT_Department_ID_Head': null, 'FK_tbl_WPT_Department_ID_HeadName': '', 'DepartmentName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '', 'ChildCount': 0
            };

        };

        $scope.postRowParam = function () {

            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_Department };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_Department = data;
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

        ////////////////////////////////////////tree view////////////////////////
        $scope.SelectedNode = '';
        $scope.nodedata = [{ 'sign': '+', 'ID': null, 'DepartmentName': '', 'ParentID': null, 'ChildCount': 0, 'spacing': '' }];

        $scope.LoadNode = function () {
         
            var successcallback = function (response) {
                $scope.nodedata.length = 0;
                $scope.nodedata = response.data;
            };
            var errorcallback = function (error) {
                
            };
            $http({ method: "GET", url: "/WPT/Organization/GetNodes", params: { PID: 0 }, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
            
        };     

        $scope.range = function (n) {
            return new Array(n);
        };

        $scope.getchild = function (Value_ID) {            

            var index = $scope.nodedata.findIndex(x => x.ID === Value_ID);
           

            var successcallback = function (response) {

                for (var i = 0; i < response.data.length; i++) {
                    $scope.nodedata.splice(index + i + 1, 0, response.data[i]);
                    $scope.nodedata[index + i + 1].spacing = $scope.nodedata[index].spacing + '_';
                    if ($scope.nodedata[index + i + 1].ChildCount > 0)
                        $scope.nodedata[index + i + 1].sign = '+';
                    else
                        $scope.nodedata[index + i + 1].sign = '-';
                    $scope.nodedata[index].sign = "-";
                }
            };
            var errorcallback = function (error) { };

            if ($scope.nodedata[index].ChildCount > 0 && $scope.nodedata.findIndex(x => x.ParentID === Value_ID) === -1) {
                $http({ method: "GET", url: "/WPT/Organization/getNodes", params: { PID: Value_ID }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'Privilege': $scope.Privilege.CanView } }).then(successcallback, errorcallback);
            }
            else {
                setOperationMessage('Please Wait while unloading data...', 0);
                $scope.RemoveChildNotes(Value_ID);
                setOperationMessage('', 0);
            }



        };

        $scope.RemoveChildNotes = function (Value_ID) {

            var index = $scope.nodedata.findIndex(x => x.ID === Value_ID);
            if ($scope.nodedata[index].ChildCount > 0)
                $scope.nodedata[index].sign = '+';
            else
                $scope.nodedata[index].sign = '-';
            for (var i = index + 1; i < $scope.nodedata.length; i++) {
                if ($scope.nodedata.findIndex(x => x.ParentID === $scope.nodedata[i].ID) > 0 && $scope.nodedata[i].ParentID === Value_ID) {
                    $scope.RemoveChildNotes($scope.nodedata[i].ID);
                    i = i - 1;
                }
                else {
                    if ($scope.nodedata[i].ParentID === Value_ID) {
                        $scope.nodedata.splice(i, 1);
                        i = i - 1;
                    }

                }
            }
        };

        $scope.SelectFromTree = function (Value_ID) {
            $scope.GetRow(Value_ID, 'Edit');           
        };
       
    })
    .controller("DepartmentDesignationCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('DepartmentDesignationCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_DepartmentDesignationCtlr', function (e, itm) {
            $scope.DesignationList = itm.Otherdata === null ? [] : itm.Otherdata.DesignationList;
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/Organization/DepartmentDesignationLoad', //--v_Load
            '/WPT/Organization/DepartmentDesignationGet', // getrow
            '/WPT/Organization/DepartmentDesignationPost' // PostRow
        );

        $scope.tbl_WPT_DepartmentDetail = {
            'ID': 0, 'FK_tbl_WPT_Department_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Designation_ID': null, 'FK_tbl_WPT_Designation_IDName': '', 'NoOfEmployees': 1,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
            'NoOfActiveEmployees': 0
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_DepartmentDetails = [$scope.tbl_WPT_DepartmentDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_DepartmentDetail = {
                'ID': 0, 'FK_tbl_WPT_Department_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Designation_ID': null, 'FK_tbl_WPT_Designation_IDName': '', 'NoOfEmployees': 1,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
                'NoOfActiveEmployees': 0
            };

        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_DepartmentDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_DepartmentDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };
    })
    .controller("DepartmentSectionsDetailCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('DepartmentSectionsDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_DepartmentSectionsDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });
        init_Operations($scope, $http,
            '/WPT/Organization/DepartmentSectionLoad', //--v_Load
            '/WPT/Organization/DepartmentSectionGet', // getrow
            '/WPT/Organization/DepartmentSectionPost' // PostRow
        );

        $scope.tbl_WPT_DepartmentDetail_Section = {
            'ID': 0, 'FK_tbl_WPT_Department_ID': $scope.MasterObject.ID,
            'SectionName': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_DepartmentDetail_Sections = [$scope.tbl_WPT_DepartmentDetail_Section];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_DepartmentDetail_Section = {
                'ID': 0, 'FK_tbl_WPT_Department_ID': $scope.MasterObject.ID,
                'SectionName': '', 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_DepartmentDetail_Section }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_DepartmentDetail_Section = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("DepartmentSectionDetailHOSCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('DepartmentSectionDetailHOSCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_DepartmentSectionDetailHOSCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });
        init_Operations($scope, $http,
            '/WPT/Organization/DepartmentSectionHOSLoad', //--v_Load
            '/WPT/Organization/DepartmentSectionHOSGet', // getrow
            '/WPT/Organization/DepartmentSectionHOSPost' // PostRow
        );

        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_DepartmentDetail_Section_HOS.FK_tbl_WPT_Employee_ID = item.ID;
                $scope.tbl_WPT_DepartmentDetail_Section_HOS.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_DepartmentDetail_Section_HOS.FK_tbl_WPT_Employee_ID = null;
                $scope.tbl_WPT_DepartmentDetail_Section_HOS.FK_tbl_WPT_Employee_IDName = null;
            }            
        };
        ////////////data structure define//////////////////

        $scope.tbl_WPT_DepartmentDetail_Section_HOS = {
            'ID': 0, 'FK_tbl_WPT_DepartmentDetail_Section_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Employee_ID': '', 'FK_tbl_WPT_Employee_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_DepartmentDetail_HODs = [$scope.tbl_WPT_DepartmentDetail_Section_HOS];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_WPT_DepartmentDetail_Section_HOS = {
                'ID': 0, 'FK_tbl_WPT_DepartmentDetail_Section_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Employee_ID': '', 'FK_tbl_WPT_Employee_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };

        };


        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_DepartmentDetail_Section_HOS }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_DepartmentDetail_Section_HOS = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    