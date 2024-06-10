MainModule
    .controller("AuthorizationSchemeCtlr", function ($scope, $window, $http) {
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
            '/Identity/Account/AuthorizationSchemeLoad', //--v_Load
            '/Identity/Account/AuthorizationSchemeGet', // getrow
            '/Identity/Account/AuthorizationSchemePost' // PostRow
        );
        init_WHMSearchModalGeneral($scope, $http);

        init_ViewSetup($scope, $http, '/Identity/Account/GetInitializedScheme');
        $scope.init_ViewSetup_Response = function (data) {

            if (data.find(o => o.Controller === 'AuthorizationSchemeCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'AuthorizationSchemeCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'AuthorizationSchemeCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'AuthorizationSchemeAreaCtlr') != undefined) {
                $scope.$broadcast('init_AuthorizationSchemeAreaCtlr', data.find(o => o.Controller === 'AuthorizationSchemeAreaCtlr'));
            }
            if (data.find(o => o.Controller === 'AuthorizationSchemeSectionCtlr') != undefined) {
                $scope.$broadcast('init_AuthorizationSchemeSectionCtlr', data.find(o => o.Controller === 'AuthorizationSchemeSectionCtlr'));
            }
            if (data.find(o => o.Controller === 'AuthorizationSchemeWHMCtlr') != undefined) {
                $scope.$broadcast('init_AuthorizationSchemeWHMCtlr', data.find(o => o.Controller === 'AuthorizationSchemeWHMCtlr'));
            }
            if (data.find(o => o.Controller === 'AuthorizationSchemeAreaFormCtlr') != undefined) {
                $scope.$broadcast('init_AuthorizationSchemeAreaFormCtlr', data.find(o => o.Controller === 'AuthorizationSchemeAreaFormCtlr'));
            }
        };


        $scope.AspNetOreasAuthorizationScheme = {
            'ID': 0, 'Name': '', 'IsCredentialsDashBoard': true, 'IsWPTDashBoard': false, 'IsManagementDashBoard': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.AspNetOreasAuthorizationSchemes = [$scope.AspNetOreasAuthorizationScheme];


        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.AspNetOreasAuthorizationScheme = {
                'ID': 0, 'Name': '', 'IsCredentialsDashBoard': true, 'IsWPTDashBoard': false, 'IsManagementDashBoard': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };           
           
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.AspNetOreasAuthorizationScheme }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.AspNetOreasAuthorizationScheme = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };  


    })
    .controller("AuthorizationSchemeSectionCtlr", function ($scope, $window, $http) {

        $scope.MasterObject = {};
        $scope.$on('AuthorizationSchemeSectionCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.MasterID = $scope.MasterObject.ID;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_AuthorizationSchemeSectionCtlr', function (e, itm) {         
            init_Filter($scope, itm.WildCard, null, null, null);
            $scope.SectionList = itm.Otherdata === null ? [] : itm.Otherdata.SectionList;
        });

        init_Operations($scope, $http,
            '/Identity/Account/AuthorizationSchemeSectionLoad', //--v_Load
            '/Identity/Account/AuthorizationSchemeSectionGet', // getrow
            '/Identity/Account/AuthorizationSchemeSectionPost' // PostRow
        );

        $scope.AspNetOreasAuthorizationScheme_Section = {
            'ID': 0, 'FK_AspNetOreasAuthorizationScheme_ID': $scope.MasterID, 'FK_tbl_WPT_DepartmentDetail_Section_ID': null, 'FK_tbl_WPT_DepartmentDetail_Section_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.AspNetOreasAuthorizationScheme_Sections = [$scope.AspNetOreasAuthorizationScheme_Section];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.AspNetOreasAuthorizationScheme_Section = {
                'ID': 0, 'FK_AspNetOreasAuthorizationScheme_ID': $scope.MasterID, 'FK_tbl_WPT_DepartmentDetail_Section_ID': null, 'FK_tbl_WPT_DepartmentDetail_Section_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.AspNetOreasAuthorizationScheme_Section }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.AspNetOreasAuthorizationScheme_Section = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    })
    .controller("AuthorizationSchemeWHMCtlr", function ($scope, $window, $http) {

        $scope.MasterObject = {};
        $scope.$on('AuthorizationSchemeWHMCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.MasterID = $scope.MasterObject.ID;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_AuthorizationSchemeWHMCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/Identity/Account/AuthorizationSchemeWHMLoad', //--v_Load
            '/Identity/Account/AuthorizationSchemeWHMGet', // getrow
            '/Identity/Account/AuthorizationSchemeWHMPost' // PostRow
        );



        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.AspNetOreasAuthorizationScheme_WareHouse.FK_tbl_Inv_WareHouseMaster_ID = item.ID;
                $scope.AspNetOreasAuthorizationScheme_WareHouse.FK_tbl_Inv_WareHouseMaster_IDName = item.WareHouseName;
            }
            else {
                $scope.AspNetOreasAuthorizationScheme_WareHouse.FK_tbl_Inv_WareHouseMaster_ID = null;
                $scope.AspNetOreasAuthorizationScheme_WareHouse.FK_tbl_Inv_WareHouseMaster_IDName = null;
            }
        };

        $scope.AspNetOreasAuthorizationScheme_WareHouse = {
            'ID': 0, 'FK_AspNetOreasAuthorizationScheme_ID': $scope.MasterID, 'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.AspNetOreasAuthorizationScheme_WareHouses = [$scope.AspNetOreasAuthorizationScheme_WareHouse];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.AspNetOreasAuthorizationScheme_WareHouse = {
                'ID': 0, 'FK_AspNetOreasAuthorizationScheme_ID': $scope.MasterID, 'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.AspNetOreasAuthorizationScheme_WareHouse }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.AspNetOreasAuthorizationScheme_WareHouse = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    })
    .controller("AuthorizationSchemeAreaCtlr", function ($scope, $window, $http) {

        $scope.MasterObject = {};
        $scope.$on('AuthorizationSchemeAreaCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.MasterID = $scope.MasterObject.ID;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_AuthorizationSchemeAreaCtlr', function (e, itm) {
            $scope.AreaList = itm.Otherdata === null ? [] : itm.Otherdata.AreaList;
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/Identity/Account/AuthorizationSchemeAreaLoad', //--v_Load
            '/Identity/Account/AuthorizationSchemeAreaGet', // getrow
            '/Identity/Account/AuthorizationSchemeAreaPost' // PostRow
        );

        $scope.AspNetOreasAuthorizationScheme_Area = {
            'ID': 0, 'FK_AspNetOreasAuthorizationScheme_ID': $scope.MasterID, 'FK_AspNetOreasArea_ID': null, 'FK_AspNetOreasArea_IDName': '',
            'CanView': true, 'CanAdd': true, 'CanEdit': true, 'CanDelete': true, 'CanViewReport': true, 'CanViewOnlyOwnData': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.AspNetOreasAuthorizationScheme_Areas = [$scope.AspNetOreasAuthorizationScheme_Area];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.AspNetOreasAuthorizationScheme_Area = {
                'ID': 0, 'FK_AspNetOreasAuthorizationScheme_ID': $scope.MasterID, 'FK_AspNetOreasArea_ID': null, 'FK_AspNetOreasArea_IDName': '',
                'CanView': true, 'CanAdd': true, 'CanEdit': true, 'CanDelete': true, 'CanViewReport': true, 'CanViewOnlyOwnData': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.AspNetOreasAuthorizationScheme_Area }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.AspNetOreasAuthorizationScheme_Area = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    })
    .controller("AuthorizationSchemeAreaFormCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('AuthorizationSchemeAreaFormCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.MasterID = $scope.MasterObject.ID;
            $scope.pageNavigation('first');

        });
        $scope.$on('init_AuthorizationSchemeAreaFormCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        $scope.AspNetOreasAuthorizationScheme_Area_Form = {
            'ID': 0, 'FK_AspNetOreasAuthorizationScheme_Area_ID': $scope.MasterID, 'FK_AspNetOreasArea_Form_ID': null,'FK_AspNetOreasArea_Form_IDName':'',
            'CanView': true, 'CanAdd': true, 'CanEdit': true, 'CanDelete': true, 'CanViewReport': true, 'CanViewOnlyOwnData': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.AspNetOreasAuthorizationScheme_Area_Forms = [$scope.AspNetOreasAuthorizationScheme_Area_Form];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.AspNetOreasAuthorizationScheme_Area_Form = {
                'ID': 0, 'FK_AspNetOreasAuthorizationScheme_Area_ID': $scope.MasterID, 'FK_AspNetOreasArea_Form_ID': null, 'FK_AspNetOreasArea_Form_IDName': '',
                'CanView': true, 'CanAdd': true, 'CanEdit': true, 'CanDelete': true, 'CanViewReport': true, 'CanViewOnlyOwnData': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.AspNetOreasAuthorizationScheme_Area_Form };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.AspNetOreasAuthorizationScheme_Area_Form = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

        init_Operations($scope, $http,
            '/Identity/Account/AuthorizationSchemeAreaFormLoad', //--v_Load
            '/Identity/Account/AuthorizationSchemeAreaFormGet', // getrow
            '/Identity/Account/AuthorizationSchemeAreaFormPost' // PostRow
        );

        $scope.OpenFormSearchModal = function () {
            $scope.FormResult = [];
            $scope.FilterByTextFormSearchModal = 'byFormName';
            $scope.FilterValueByTextFormSearchModal = '';
            $('#FormSearchModal').modal('show');
        };

        $scope.SearchForm = function () {
            var successcallback = function (response) {
                $scope.FormResult = response.data;                
            };
            var errorcallback = function (error) {
            };
            $http({ method: "GET", url: "/Identity/Account/FormList", async: true, params: { LoadValueByText: $scope.MasterObject.FK_AspNetOreasArea_IDName, FilterByText: $scope.FilterByTextFormSearchModal, FilterValueByText: $scope.FilterValueByTextFormSearchModal  }, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
        };

        $scope.SelectedForm = function (item) {
            $scope.AspNetOreasAuthorizationScheme_Area_Form.FK_AspNetOreasArea_Form_ID = item.ID;
            $scope.AspNetOreasAuthorizationScheme_Area_Form.FK_AspNetOreasArea_Form_IDName = item.FormName;
        };

        $(function () {
            $('#FormSearchModal').on('shown.bs.modal', function () {
                $('#FilterValueByTextFormSearchModal').focus();
            });
        });

        $(function () {
            $('#FormSearchModal').on('hidden.bs.modal', function () {
                $('[name="AspNetOreasAuthorizationScheme_Area_Form.FK_AspNetOreasArea_Form_IDName"]').focus();
            });
        });
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


