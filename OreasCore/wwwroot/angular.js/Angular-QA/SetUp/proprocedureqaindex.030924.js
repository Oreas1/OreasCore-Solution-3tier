MainModule
    .controller("ProProcedureIndexCtlr", function ($scope, $http) { 
        const urlParams = new URLSearchParams(window.location.search);
       
        if (urlParams.get('by') != null) {
            $scope.caller = urlParams.get('by');
        }
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/QA/SetUp/ProProcedureLoad', //--v_Load
            '/QA/SetUp/ProProcedureGet', // getrow
            '/QA/SetUp/ProProcedurePost' // PostRow
        );

        init_ViewSetup($scope, $http, '/QA/SetUp/GetInitializedProProcedure');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'ProProcedureIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'ProProcedureIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'ProProcedureIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
           

        };

        $scope.tbl_Pro_Procedure = {
            'ID': 0, 'ProcedureName': null, 'ForRaw1_Packaging0': $scope.caller === 'BMR' ? true : $scope.caller === 'BPR' ? false : null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_Procedures = [$scope.tbl_Pro_Procedure];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Pro_Procedure = {
                'ID': 0, 'ProcedureName': null, 'ForRaw1_Packaging0': $scope.caller === 'BMR' ? true : $scope.caller === 'BPR' ? false : null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_Procedure }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_Procedure = data;
        };

        $scope.pageNavigatorParam = function () {
            return { MasterID: $scope.MasterID, Caller: $scope.caller };
        };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


