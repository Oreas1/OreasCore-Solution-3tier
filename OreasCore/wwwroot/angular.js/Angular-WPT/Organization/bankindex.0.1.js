MainModule
    .controller("BanktIndexCtlr", function ($scope, $window, $http) {
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
            '/WPT/Organization/BankLoad', //--v_Load
            '/WPT/Organization/BankGet', // getrow
            '/WPT/Organization/BankPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/WPT/Organization/GetInitializedBank');
        $scope.init_ViewSetup_Response = function (data) {

            if (data.find(o => o.Controller === 'BanktIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'BanktIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'BanktIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'BankDetailBranchCtlr') != undefined) {
                $scope.$broadcast('init_BankDetailBranchCtlr', data.find(o => o.Controller === 'BankDetailBranchCtlr'));
            }
            if (data.find(o => o.Controller === 'BankDetailBranchCompanyAcCtlr') != undefined) {
                $scope.$broadcast('init_BankDetailBranchCompanyAcCtlr', data.find(o => o.Controller === 'BankDetailBranchCompanyAcCtlr'));
            }
            if (data.find(o => o.Controller === 'BankDetailBranchEmployeeAcCtlr') != undefined) {
                $scope.$broadcast('init_BankDetailBranchEmployeeAcCtlr', data.find(o => o.Controller === 'BankDetailBranchEmployeeAcCtlr'));
            }
        };

        init_EmployeeSearchModalGeneral($scope, $http);

        $scope.tbl_WPT_Bank = {
            'ID': 0, 'BankName': '', 'Branches': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_Banks = [$scope.tbl_WPT_Bank];


        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_Bank = {
                'ID': 0, 'BankName': '', 'Branches': 0, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_Bank }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_Bank = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    })
    .controller("BankDetailBranchCtlr", function ($scope, $window, $http) {

        $scope.MasterObject = {};
        $scope.$on('BankDetailBranchCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.MasterID = $scope.MasterObject.ID;
            $scope.pageNavigation('first');
        });
        $scope.$on('init_BankDetailBranchCtlr', function (e, itm) {         
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/Organization/BankDetailBranchLoad', //--v_Load
            '/WPT/Organization/BankDetailBranchGet', // getrow
            '/WPT/Organization/BankDetailBranchPost' // PostRow
        );

        $scope.tbl_WPT_Bank_Branch = {
            'ID': 0, 'FK_tbl_WPT_Bank_ID': $scope.MasterObject.ID,
            'BranchName': '', 'BranchCode': '', 'City': '', 'PostalAddress': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_Bank_Branchs = [$scope.tbl_WPT_Bank_Branch];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_Bank_Branch = {
                'ID': 0, 'FK_tbl_WPT_Bank_ID': $scope.MasterObject.ID,
                'BranchName': '', 'BranchCode': '', 'City': '', 'PostalAddress': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_Bank_Branch }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_Bank_Branch = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    })
    .controller("BankDetailBranchCompanyAcCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('BankDetailBranchCompanyAcCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.MasterID = $scope.MasterObject.ID;
            $scope.pageNavigation('first');

        });

        $scope.$on('init_BankDetailBranchCompanyAcCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/Organization/BankDetailBranchCompanyAcLoad', //--v_Load
            '/WPT/Organization/BankDetailBranchCompanyAcGet', // getrow
            '/WPT/Organization/BankDetailBranchCompanyAcPost' // PostRow
        );
        

        $scope.tbl_WPT_CompanyBankDetail = {
            'ID': 0, 'FK_tbl_WPT_Bank_Branch_ID': $scope.MasterObject.ID,
            'BankAccountNo': '', 'BankAccountTitle': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_CompanyBankDetails = [$scope.tbl_WPT_CompanyBankDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_CompanyBankDetail = {
                'ID': 0, 'FK_tbl_WPT_Bank_Branch_ID': $scope.MasterObject.ID,
                'BankAccountNo': '', 'BankAccountTitle': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_CompanyBankDetail };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_CompanyBankDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };


    })
    .controller("BankDetailBranchEmployeeAcCtlr", function ($scope, $window, $http) {
        $scope.MasterObject = {};
        $scope.$on('BankDetailBranchEmployeeAcCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.MasterID = $scope.MasterObject.ID;
            $scope.pageNavigation('first');

        });
        $scope.$on('init_BankDetailBranchEmployeeAcCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/WPT/Organization/BankDetailBranchEmployeeAcLoad', //--v_Load
            '/WPT/Organization/BankDetailBranchEmployeeAcGet', // getrow
            '/WPT/Organization/BankDetailBranchEmployeeAcPost' // PostRow
        );

        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_WPT_EmployeeBankDetail.FK_tbl_WPT_Employee_ID = item.ID;
                $scope.tbl_WPT_EmployeeBankDetail.FK_tbl_WPT_Employee_IDName = item.EmployeeName;
            }
            else {
                $scope.tbl_WPT_EmployeeBankDetail.FK_tbl_WPT_Employee_ID = null;
                $scope.tbl_WPT_EmployeeBankDetail.FK_tbl_WPT_Employee_IDName = null;
            }
        };

        $scope.tbl_WPT_EmployeeBankDetail = {
            'ID': 0, 'FK_tbl_WPT_Bank_Branch_ID': $scope.MasterObject.ID,
            'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
            'BankAccountNo': '', 'BankAccountTitle': '', 'IsDefaultForBank': true,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_WPT_EmployeeBankDetails = [$scope.tbl_WPT_EmployeeBankDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_WPT_EmployeeBankDetail = {
                'ID': 0, 'FK_tbl_WPT_Bank_Branch_ID': $scope.MasterObject.ID,
                'FK_tbl_WPT_Employee_ID': null, 'FK_tbl_WPT_Employee_IDName': '',
                'BankAccountNo': '', 'BankAccountTitle': '', 'IsDefaultForBank': true,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_WPT_EmployeeBankDetail };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_WPT_EmployeeBankDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


