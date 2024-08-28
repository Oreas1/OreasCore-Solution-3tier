MainModule
    .controller("QcTestIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/QC/SetUp/QcTestLoad', //--v_Load
            '/QC/SetUp/QcTestGet', // getrow
            '/QC/SetUp/QcTestPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/QC/SetUp/GetInitializedQcTest');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'QcTestIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'QcTestIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'QcTestIndexCtlr').WildCard, null, null, null);

                if (data.find(o => o.Controller === 'QcTestIndexCtlr').Otherdata === null) {
                    $scope.QcLabList = [];
                }
                else {
                    $scope.QcLabList = data.find(o => o.Controller === 'QcTestIndexCtlr').Otherdata.QcLabList;
                }

                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_Qc_Test = {
            'ID': 0, 'TestName': null, 'FK_tbl_Qc_Lab_ID': null, 'FK_tbl_Qc_Lab_IDName': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Qc_Tests = [$scope.tbl_Qc_Test];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Qc_Test = {
                'ID': 0, 'TestName': null, 'FK_tbl_Qc_Lab_ID': null, 'FK_tbl_Qc_Lab_IDName': null, 'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Qc_Test }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Qc_Test = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


