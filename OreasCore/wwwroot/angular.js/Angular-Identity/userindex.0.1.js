MainModule
    .controller("UserIndexCtlr", function ($scope, $http) {
        $scope.DivHideShow = function (v, itm, div_hide, div_show, scope) {
            if (typeof v !== 'undefined' && v !== '' && v !== null) {
                $scope.$broadcast(v, itm);
            }
            if (typeof scope !== 'undefined' && scope !== '' && scope !== null && typeof scope.$parent.pageNavigation === 'function') {
                scope.$parent.pageNavigation('Load');
            }

            $("#" + div_hide).hide('slow');
            $("#" + div_show).show('slow');
        };



        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/Identity/Account/UserLoad', //--v_Load
            '/Identity/Account/UserGet', // getrow
            '/Identity/Account/UserPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Identity/Account/GetInitializedUser');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'UserIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'UserIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'UserIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        init_EmployeeSearchModalGeneral($scope, $http);

        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.UserViewModel.FK_tbl_WPT_Employee_ID = item.ID;
                $scope.UserViewModel.FK_tbl_WPT_Employee_IDName = '[' + item.ATEnrollmentNo_Default  + '] ' +item.EmployeeName;
            }
            else {
                $scope.UserViewModel.FK_tbl_WPT_Employee_ID = null;
                $scope.UserViewModel.FK_tbl_WPT_Employee_IDName = null;
            }
        };

        $scope.UserViewModel = {
            'Id': '', 'Email': '', 'UserName': '', 'EmailConfirmed': true, 'PhoneNumber': '', 'PhoneNumberConfirmed': true, 'TwoFactorEnabled': false,
            'LockoutEnd': null, 'LockoutEnabled': false, 'AccessFailedCount': 0,
            'FK_AspNetOreasAuthorizationScheme_ID': null, 'FK_AspNetOreasAuthorizationScheme_IDName': '', 'MyID': 0,
            'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
            'PurchaseRequestApprover': false, 'AcVoucherApprover': false, 'EmailSignature': null
        };

        //for list model which will be coming as as data in pageddata
        $scope.UserViewModels = [$scope.UserViewModel];


        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.UserViewModel = {
                'Id': '', 'Email': '', 'UserName': '', 'EmailConfirmed': true, 'PhoneNumber': '', 'PhoneNumberConfirmed': true, 'TwoFactorEnabled': false,
                'LockoutEnd': null, 'LockoutEnabled': false, 'AccessFailedCount': 0,
                'FK_AspNetOreasAuthorizationScheme_ID': null, 'FK_AspNetOreasAuthorizationScheme_IDName': '', 'MyID': 0,
                'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
                'PurchaseRequestApprover': false, 'AcVoucherApprover': false, 'EmailSignature': null
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.UserViewModel }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.UserViewModel = data;
            //if (data.LockoutEnd !== null) { $scope.UserViewModel.LockoutEnd = new Date(data.LockoutEnd);  }
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

        $scope.OpenSchemeSearchModal = function () {
            $scope.SchemeResult = [];
            $scope.LoadValueByTextSchemeSearchModal = 'byName';
            $scope.FilterValueByTextSchemeSearchModal = '';
            $('#SchemeSearchModal').modal('show');
        };

        $scope.SearchForm = function () {
            var successcallback = function (response) {
                $scope.SchemeResult = response.data;
            };
            var errorcallback = function (error) {
            };
            $http({ method: "GET", url: "/Identity/Account/AuthorizationSchemeList", async: true, params: { FilterByText: $scope.LoadValueByTextSchemeSearchModal, FilterValueByText: $scope.FilterValueByTextSchemeSearchModal }, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
        };

        $scope.SelectedScheme = function (item) {
            $scope.UserViewModel.FK_AspNetOreasAuthorizationScheme_ID = item.ID;
            $scope.UserViewModel.FK_AspNetOreasAuthorizationScheme_IDName = item.Name;
        };

        $(function () {
            $('#SchemeSearchModal').on('shown.bs.modal', function () {
                $('#LoadValueByTextSchemeSearchModal').focus();
            });
        });

        $(function () {
            $('#SchemeSearchModal').on('hidden.bs.modal', function () {
                $('[name="UserViewModel.FK_AspNetOreasAuthorizationScheme_IDName"]').focus();
            });
        });

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


