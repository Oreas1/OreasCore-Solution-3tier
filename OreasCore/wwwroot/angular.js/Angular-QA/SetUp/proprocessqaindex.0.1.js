MainModule
    .controller("ProProcessMasterCtlr", function ($scope, $http) {

        const urlParams = new URLSearchParams(window.location.search);

        if (urlParams.get('by') != null) {
            $scope.caller = urlParams.get('by');
        }

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
            '/QA/SetUp/ProProcessMasterLoad', //--v_Load
            '/QA/SetUp/ProProcessMasterGet', // getrow
            '/QA/SetUp/ProProcessMasterPost' // PostRow
        );
  
        init_ViewSetup($scope, $http, '/QA/SetUp/GetInitializedProProcess?Caller=' + $scope.caller);

        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'ProProcessMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'ProProcessMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'ProProcessMasterCtlr').WildCard, null, null, null);
                if (data.find(o => o.Controller === 'ProProcessMasterCtlr').Otherdata === null) {
                    $scope.ProProcedureList = [];
                }
                else {
                    $scope.ProProcedureList = data.find(o => o.Controller === 'ProProcessMasterCtlr').Otherdata.ProProcedureList;
                }
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'ProProcessDetailCtlr') != undefined) {
                $scope.$broadcast('init_ProProcessDetailCtlr', data.find(o => o.Controller === 'ProProcessDetailCtlr'));
            }
        };

        $scope.tbl_Pro_ProcessMaster = {
            'ID': 0, 'ProcessName': '', 'ForRaw1_Packaging0': $scope.caller === 'BMR' ? true : $scope.caller === 'BPR' ? false : null,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_ProcessMasters = [$scope.tbl_Pro_ProcessMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_ProcessMaster = {
                'ID': 0, 'ProcessName': '', 'ForRaw1_Packaging0': $scope.caller === 'BMR' ? true : $scope.caller === 'BPR' ? false : null,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_ProcessMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Pro_ProcessMaster = data; 
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID, Caller: $scope.caller }; };
       
    })
    .controller("ProProcessDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('ProProcessDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_ProProcessDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null); 
        });


        init_Operations($scope, $http,
            '/QA/SetUp/ProProcessDetailLoad', //--v_Load
            '/QA/SetUp/ProProcessDetailGet', // getrow
            '/QA/SetUp/ProProcessDetailPost' // PostRow
        );


        $scope.tbl_Pro_ProcessDetail = {
            'ID': 0, 'FK_tbl_Pro_ProcessMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Pro_Procedure_ID': null, 'FK_tbl_Pro_Procedure_IDName': '',  'IsQAClearanceBeforeStart': true,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Pro_ProcessDetails = [$scope.tbl_Pro_ProcessDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Pro_ProcessDetail = {
                'ID': 0, 'FK_tbl_Pro_ProcessMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Pro_Procedure_ID': null, 'FK_tbl_Pro_Procedure_IDName': '', 'IsQAClearanceBeforeStart': true,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Pro_ProcessDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Pro_ProcessDetail = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    