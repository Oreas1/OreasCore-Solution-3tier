MainModule
    .controller("RequestMasterCtlr", function ($scope, $http) {
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
        //////////////////////////////entry panel/////////////////////////
        init_Operations($scope, $http,
            '/PD/Request/RequestMasterLoad', //--v_Load
            '/PD/Request/RequestMasterGet', // getrow
            '/PD/Request/RequestMasterPost' // PostRow
        );

        init_ViewSetup($scope, $http, '/PD/Request/GetInitializedRequest');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'RequestMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'RequestMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'RequestMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'RequestMasterCtlr').LoadByCard);
                if (data.find(o => o.Controller === 'RequestMasterCtlr').Otherdata === null) {
                    $scope.WareHouseList = []; $scope.ProProcedureList = []; $scope.CompositionFilterList = [];
                }
                else {
                    $scope.WareHouseList = data.find(o => o.Controller === 'RequestMasterCtlr').Otherdata.WareHouseList;
                    $scope.ProProcedureList = data.find(o => o.Controller === 'RequestMasterCtlr').Otherdata.ProProcedureList;
                    $scope.CompositionFilterList = data.find(o => o.Controller === 'RequestMasterCtlr').Otherdata.CompositionFilterList;
                }
                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'RequestDetailTRCtlr') != undefined) {
                $scope.$broadcast('init_RequestDetailTRCtlr', data.find(o => o.Controller === 'RequestDetailTRCtlr'));
            }
            if (data.find(o => o.Controller === 'RequestDetailTRProcedureCtlr') != undefined) {
                $scope.$broadcast('init_RequestDetailTRProcedureCtlr', data.find(o => o.Controller === 'RequestDetailTRProcedureCtlr'));
            }
            if (data.find(o => o.Controller === 'RequestDetailTRCFPCtlr') != undefined) {
                $scope.$broadcast('init_RequestDetailTRCFPCtlr', data.find(o => o.Controller === 'RequestDetailTRCFPCtlr'));
            }
            if (data.find(o => o.Controller === 'RequestDetailTRCFPItemCtlr') != undefined) {
                $scope.$broadcast('init_RequestDetailTRCFPItemCtlr', data.find(o => o.Controller === 'RequestDetailTRCFPItemCtlr'));
            }
        };

        init_ProductSearchModalGeneral($scope, $http);
        init_WHMSearchModalGeneral($scope, $http);

        $scope.tbl_PD_RequestMaster = {
            'ID': 0, 'DocNo': null, 'DocDate': new Date(),
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'FK_tbl_Inv_ProductRegistrationDetail_ID_Primary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName': '', 'MeasurementUnitPrimary': '',
            'SampleProductExpiryMonths': 1, 'SampleProductMRP': 0, 'SampleProductPhoto': null, 'Remarks': '', 'FinalStatus': null, 'IsDispensedAll': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                if ($scope.ProductSearch_CallerName === 'tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_IDName') {
                    $scope.tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                    $scope.tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                    $scope.tbl_PD_RequestMaster.MeasurementUnit = item.MeasurementUnit;

                    //------when primary changed then de-select secondary
                    $scope.tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary = null;
                    $scope.tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = null;
                }
                else if ($scope.ProductSearch_CallerName === 'tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName') {
                    $scope.tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary = item.ID;
                    $scope.tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = item.MeasurementUnit + ' [' + item.Split_Into + 's]';
                }
            }
            else {
                if ($scope.ProductSearch_CallerName === 'tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_IDName') {
                    $scope.tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                    $scope.tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                    $scope.tbl_PD_RequestMaster.MeasurementUnit = null;

                    //------when primary changed then de-select secondary
                    $scope.tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary = null;
                    $scope.tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = null;
                }
                else if ($scope.ProductSearch_CallerName === 'tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName') {
                    $scope.tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary = null;
                    $scope.tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = null;
                }
            }
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_PD_RequestMasters = [$scope.tbl_PD_RequestMaster];

        $scope.clearEntryPanel = function () {
            $scope.ImageUploadingProgress = '';
            document.getElementById('SampleProductPhoto').value = '';
            //rededine to orignal values
            $scope.tbl_PD_RequestMaster = {
                'ID': 0, 'DocNo': null, 'DocDate': new Date(),
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'FK_tbl_Inv_ProductRegistrationDetail_ID_Primary': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName': '', 'MeasurementUnitPrimary': '',
                'SampleProductExpiryMonths': 1, 'SampleProductMRP': 0, 'SampleProductPhoto': null, 'Remarks': '', 'FinalStatus': null, 'IsDispensedAll': 0,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_PD_RequestMaster };
        };

        $scope.GetRowResponse = function (data, operation) {
            $scope.ImageUploadingProgress = '';
            $scope.tbl_PD_RequestMaster = data;
            $scope.tbl_PD_RequestMaster.DocDate = new Date(data.DocDate);

        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };



        $scope.LoadFileData = function (files) {
            $scope.ng_DisabledBtnAll = true;

            $scope.ImageUploadingProgress = 'Image is capturing! Please Wait';
            $scope.tbl_PD_RequestMaster.SampleProductPhoto = '';
            $scope.UploadProgressValue = 0;  // Initialize progress to 0
            $scope.$digest();

            const file = files[0];

            if (file) {
                const filesizeKB = Math.round(file.size / 1024);
                if (filesizeKB > 100) {
                    $scope.ng_DisabledBtnAll = false;
                    $scope.ImageUploadingProgress = 'Image is capturing terminated';
                    alert('Maximum 100KBs file size is allowed but uploaded file is size is: ' + filesizeKB + 'KBs');
                    $scope.$apply(); // ye es liye kiya hai alert ki waja se ye  $scope.ng_DisabledBtnAll = false; code work nahi karta tou zabardasti apply karwa k run kiya hai
                    return;
                }

                const reader = new FileReader();

                // Handle the progress event to show the uploading progress
                reader.onprogress = function (e) {
                    if (e.lengthComputable) {
                        const percentLoaded = Math.round((e.loaded / e.total) * 100);
                        $scope.UploadProgressValue = percentLoaded;
                        $scope.ImageUploadingProgress = 'Uploading: ' + percentLoaded + '%';
                        $scope.$digest(); // Update the UI
                    }
                };

                reader.onload = function (e) {

                    const base64String = e.target.result.split(',')[1];

                    $scope.tbl_PD_RequestMaster.SampleProductPhoto = base64String;
                    $scope.ng_DisabledBtnAll = false;
                    $scope.ImageUploadingProgress = 'Image is captured Sucessfully Please save the record';
                    $scope.$digest();
                    console.log('o', $scope.ng_DisabledBtnAll);
                };

                reader.onerror = function () {
                    $scope.ImageUploadingProgress = 'Error uploading image!';
                    $scope.ng_DisabledBtnAll = false;
                    $scope.$digest();
                };

                reader.readAsDataURL(file);

            }
        };



    })
    .controller("RequestDetailTRCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('RequestDetailTRCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_RequestDetailTRCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, itm.LoadByCard);
        });

        init_Operations($scope, $http,
            '/PD/Request/RequestDetailTRLoad', //--v_Load
            '/PD/Request/RequestDetailTRGet', // getrow
            '/PD/Request/RequestDetailTRPost' // PostRow
        );

        $scope.tbl_PD_RequestDetailTR = {
            'ID': 0, 'FK_tbl_PD_RequestMaster_ID': $scope.MasterObject.ID, 'DocNo': null, 'DocDate': new Date(),
            'MfgDate': new Date(), 'TrialBatchNo': '#', 'TrialBatchSizeInSemiUnits': 1, 'TrialStatus': null, 'IsDispensedAll': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_PD_RequestDetailTRs = [$scope.tbl_PD_RequestDetailTR];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_PD_RequestDetailTR = {
                'ID': 0, 'FK_tbl_PD_RequestMaster_ID': $scope.MasterObject.ID, 'DocNo': null, 'DocDate': new Date(),
                'MfgDate': new Date(), 'TrialBatchNo': '#', 'TrialBatchSizeInSemiUnits': 1, 'TrialStatus': null, 'IsDispensedAll': 0,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_PD_RequestDetailTR }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_PD_RequestDetailTR = data;
            $scope.tbl_PD_RequestDetailTR.DocDate = new Date(data.DocDate);
            $scope.tbl_PD_RequestDetailTR.MfgDate = new Date(data.MfgDate);
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        $scope.EditStatus = function (itm) {
            if ((itm.TrialStatus === null) && (!$scope.Privilege.CanAdd || !$scope.Privilege.CanEdit)) {
                alert('Dont have privilege to add TrialStatus');
                return;
            }
            else if (itm.TrialStatus != null && !$scope.Privilege.CanEdit) {
                alert('Dont have privilege to change TrialStatus');
                return;
            }
            else {
                $scope.GetRow(itm.ID, 'Edit');
                $scope.StatusOperation = true;
            }

        };

    })
    .controller("RequestDetailTRProcedureCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('RequestDetailTRProcedureCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');

        });

        $scope.$on('init_RequestDetailTRProcedureCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/PD/Request/RequestDetailTRProcedureLoad', //--v_Load
            '/PD/Request/RequestDetailTRProcedureGet', // getrow
            '/PD/Request/RequestDetailTRProcedurePost' // PostRow
        );

        $scope.tbl_PD_RequestDetailTR_Procedure = {
            'ID': 0, 'FK_tbl_PD_RequestDetailTR_ID': $scope.MasterObject.ID,
            'FK_tbl_Pro_Procedure_ID': null, 'FK_tbl_Pro_Procedure_IDName': '',
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_PD_RequestDetailTR_Procedures = [$scope.tbl_PD_RequestDetailTR_Procedure];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_PD_RequestDetailTR_Procedure = {
                'ID': 0, 'FK_tbl_PD_RequestDetailTR_ID': $scope.MasterObject.ID,
                'FK_tbl_Pro_Procedure_ID': null, 'FK_tbl_Pro_Procedure_IDName': '',
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_PD_RequestDetailTR_Procedure }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_PD_RequestDetailTR_Procedure = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("RequestDetailTRCFPCtlr", function ($scope, $http) {
        $scope.MasterObject = {};
        $scope.$on('RequestDetailTRCFPCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_RequestDetailTRCFPCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
            init_Report($scope, itm.Reports, '/PD/Request/GetPDRequestReport');
        });

        init_Operations($scope, $http,
            '/PD/Request/RequestDetailTRCFPLoad', //--v_Load
            '/PD/Request/RequestDetailTRCFPGet', // getrow
            '/PD/Request/RequestDetailTRCFPPost' // PostRow
        );

        $scope.tbl_PD_RequestDetailTR_CFP = {
            'ID': 0, 'FK_tbl_PD_RequestDetailTR_ID': $scope.MasterObject.ID,
            'FK_tbl_Pro_CompositionFilterPolicyDetail_ID': null, 'FK_tbl_Pro_CompositionFilterPolicyDetail_IDName': '',
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '', 'IsDispensedAll': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_PD_RequestDetailTR_CFP.FK_tbl_Inv_WareHouseMaster_ID = item.ID;
                $scope.tbl_PD_RequestDetailTR_CFP.FK_tbl_Inv_WareHouseMaster_IDName = item.WareHouseName;
            }
            else {
                $scope.tbl_PD_RequestDetailTR_CFP.FK_tbl_Inv_WareHouseMaster_ID = null;
                $scope.tbl_PD_RequestDetailTR_CFP.FK_tbl_Inv_WareHouseMaster_IDName = null;
            }
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_PD_RequestDetailTR_CFPs = [$scope.tbl_PD_RequestDetailTR_CFP];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_PD_RequestDetailTR_CFP = {
                'ID': 0, 'FK_tbl_PD_RequestDetailTR_ID': $scope.MasterObject.ID,
                'FK_tbl_Pro_CompositionFilterPolicyDetail_ID': null, 'FK_tbl_Pro_CompositionFilterPolicyDetail_IDName': '',
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '', 'IsDispensedAll': 0,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_PD_RequestDetailTR_CFP }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_PD_RequestDetailTR_CFP = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .controller("RequestDetailTRCFPItemCtlr", function ($scope, $http, $window) {
        $scope.MasterObject = {};
        $scope.$on('RequestDetailTRCFPItemCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');

        });

        $scope.$on('init_RequestDetailTRCFPItemCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);

        });

        init_Operations($scope, $http,
            '/PD/Request/RequestDetailTRCFPItemLoad', //--v_Load
            '/PD/Request/RequestDetailTRCFPItemGet', // getrow
            '/PD/Request/RequestDetailTRCFPItemPost' // PostRow
        );

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_PD_RequestDetailTR_CFP_Item.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_PD_RequestDetailTR_CFP_Item.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_PD_RequestDetailTR_CFP_Item.MeasurementUnit = item.MeasurementUnit;
            }
            else {
                $scope.tbl_PD_RequestDetailTR_CFP_Item.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_PD_RequestDetailTR_CFP_Item.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_PD_RequestDetailTR_CFP_Item.MeasurementUnit = null;
            }
            if (item.IsDecimal) { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+(\.[0-9]{1,4})?$"); }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9]+$"); }
        };

        $scope.tbl_PD_RequestDetailTR_CFP_Item = {
            'ID': 0, 'FK_tbl_PD_RequestDetailTR_CFP_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'RequiredTrue_ReturnFalse': true, 'Remarks': '', 'IsDispensed': 0,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_PD_RequestDetailTR_CFP_Items = [$scope.tbl_PD_RequestDetailTR_CFP_Item];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_PD_RequestDetailTR_CFP_Item = {
                'ID': 0, 'FK_tbl_PD_RequestDetailTR_CFP_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'Quantity': 0, 'RequiredTrue_ReturnFalse': true, 'Remarks': '', 'IsDispensed': 0,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_PD_RequestDetailTR_CFP_Item }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_PD_RequestDetailTR_CFP_Item = data;
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };


        $scope.GotoReport = function (id) {
            $window.open('/PD/Request/GetPDRequestReport?rn=Single Item Request&id=' + id);
        };
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });
