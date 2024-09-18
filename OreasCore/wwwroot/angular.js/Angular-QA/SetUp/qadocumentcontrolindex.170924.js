MainModule
    .controller("QaDocumentControlIndexCtlr", function ($scope, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model
        init_Operations($scope, $http,
            '/QA/SetUp/QaDocumentControlLoad', //--v_Load
            '/QA/SetUp/QaDocumentControlGet', // getrow
            '/QA/SetUp/QaDocumentControlPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/QA/SetUp/GetInitializedQaDocumentControl');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'QaDocumentControlIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'QaDocumentControlIndexCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'QaDocumentControlIndexCtlr').WildCard, null, null, null);
                $scope.pageNavigation('first');
            }
        };

        $scope.tbl_Qa_DocumentControl = {
            'ID': 0, 'DocumentNo': null, 'DocumentName': null, 'IssuedDate': new Date(), 'RevisionNo': 0,
            'ReportCodes': null, 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Qa_DocumentControls = [$scope.tbl_Qa_DocumentControl];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values            
            $scope.tbl_Qa_DocumentControl = {
                'ID': 0, 'DocumentNo': null, 'DocumentName': null, 'IssuedDate': new Date(), 'RevisionNo': 0,
                'ReportCodes': null, 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Qa_DocumentControl }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Qa_DocumentControl = data;
            $scope.tbl_Qa_DocumentControl.IssuedDate = new Date(data.IssuedDate); 
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


