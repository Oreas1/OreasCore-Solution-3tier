MainModule
    .controller("QcLabIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/QC/SetUp/QcLabLoad', //--v_Load
            '/QC/SetUp/QcLabGet', // getrow
            '/QC/SetUp/QcLabPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/QC/SetUp/GetInitializedQcLab');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'QcLabIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'QcLabIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'QcLabIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_Qc_Lab = {
            'ID': 0, 'LabName': null, 'Prefix': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Qc_Labs = [$scope.tbl_Qc_Lab];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Qc_Lab = {
                'ID': 0, 'LabName': null, 'Prefix': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Qc_Lab }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Qc_Lab = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


