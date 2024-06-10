MainModule
    .controller("CurrencyAndCountryIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/Accounts/SetUp/CurrencyAndCountryLoad', //--v_Load
            '/Accounts/SetUp/CurrencyAndCountryGet', // getrow
            '/Accounts/SetUp/CurrencyAndCountryPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/Accounts/SetUp/GetInitializedCurrencyAndCountry');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'CurrencyAndCountryIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'CurrencyAndCountryIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'CurrencyAndCountryIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_Ac_CurrencyAndCountry = {
            'ID': 0, 'CountryName': null, 'CurrencyCode': null, 'CurrencySymbol': null, 'IsDefault': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Ac_CurrencyAndCountrys = [$scope.tbl_Ac_CurrencyAndCountry];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Ac_CurrencyAndCountry = {
                'ID': 0, 'CountryName': null, 'CurrencyCode': null, 'CurrencySymbol': null, 'IsDefault': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Ac_CurrencyAndCountry }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Ac_CurrencyAndCountry = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


