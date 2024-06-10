MainModule
    .controller("PurchaseNoteMasterCtlr", function ($scope, $http) {
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
            '/Inventory/Challan/PurchaseNoteMasterLoad', //--v_Load
            '/Inventory/Challan/PurchaseNoteMasterGet', // getrow
            '/Inventory/Challan/PurchaseNoteMasterPost' // PostRow
        );
        

        init_ViewSetup($scope, $http, '/Inventory/Challan/GetInitializedPurchaseNote');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'PurchaseNoteMasterCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'PurchaseNoteMasterCtlr').Privilege;
                init_Filter($scope, data.find(o => o.Controller === 'PurchaseNoteMasterCtlr').WildCard, null, null, data.find(o => o.Controller === 'PurchaseNoteMasterCtlr').LoadByCard);
                init_Report($scope, data.find(o => o.Controller === 'PurchaseNoteMasterCtlr').Reports, '/Inventory/Challan/GetPurchaseNoteReport'); 
                const urlParams = new URLSearchParams(window.location.search);

                if (urlParams.get('byDocNo') != null) {
                    $scope.FilterByText = 'byDocNo';
                    $scope.FilterValueByText = urlParams.get('byDocNo');
                }
          
                // remove url parameter from history so then if user refresh page then no parameter will show
                //window.history.replaceState({}, '', window.location.href.split('?')[0]);               

                $scope.pageNavigation('first');
            }
            if (data.find(o => o.Controller === 'PurchaseNoteDetailCtlr') != undefined) {
                $scope.$broadcast('init_PurchaseNoteDetailCtlr', data.find(o => o.Controller === 'PurchaseNoteDetailCtlr'));
            }
            if (data.find(o => o.Controller === 'PurchaseNoteDetailOfDetailCtlr') != undefined) {
                $scope.$broadcast('init_PurchaseNoteDetailOfDetailCtlr', data.find(o => o.Controller === 'PurchaseNoteDetailOfDetailCtlr'));
            }
        };

        init_ProductSearchModalGeneral($scope, $http);
        init_COASearchModalGeneral($scope, $http);
        init_WHMSearchModalGeneral($scope, $http);

        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_PurchaseNoteMaster.FK_tbl_Inv_WareHouseMaster_ID = item.ID;
                $scope.tbl_Inv_PurchaseNoteMaster.FK_tbl_Inv_WareHouseMaster_IDName = item.WareHouseName;
            }
            else {
                $scope.tbl_Inv_PurchaseNoteMaster.FK_tbl_Inv_WareHouseMaster_ID = null;
                $scope.tbl_Inv_PurchaseNoteMaster.FK_tbl_Inv_WareHouseMaster_IDName = null;
            }
        };

        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_PurchaseNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID = item.ID;
                $scope.tbl_Inv_PurchaseNoteMaster.FK_tbl_Ac_ChartOfAccounts_IDName = item.AccountName;
            }
            else {
                $scope.tbl_Inv_PurchaseNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID = null;
                $scope.tbl_Inv_PurchaseNoteMaster.FK_tbl_Ac_ChartOfAccounts_IDName = null;
            }

        };
        

        $scope.tbl_Inv_PurchaseNoteMaster = {
            'ID': 0, 'DocNo': '', 'DocDate': new Date(),
            'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
            'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
            'SupplierChallanNo': '', 'Remarks': '', 'TotalNetAmount': 0,
            'IsProcessedAll': false, 'IsSupervisedAll': false,
            'IsQCAll': false, 'IsQCSampleAll': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseNoteMasters = [$scope.tbl_Inv_PurchaseNoteMaster];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_PurchaseNoteMaster = {
                'ID': 0, 'DocNo': '', 'DocDate': new Date(),
                'FK_tbl_Inv_WareHouseMaster_ID': null, 'FK_tbl_Inv_WareHouseMaster_IDName': '',
                'FK_tbl_Ac_ChartOfAccounts_ID': null, 'FK_tbl_Ac_ChartOfAccounts_IDName': '',
                'SupplierChallanNo': '', 'Remarks': '', 'TotalNetAmount': 0,
                'IsProcessedAll': false, 'IsSupervisedAll': false,
                'IsQCAll': false, 'IsQCSampleAll': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': ''
            };
        };

        $scope.postRowParam = function () {
            return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseNoteMaster };
        };

        $scope.GetRowResponse = function (data, operation) {            
            $scope.tbl_Inv_PurchaseNoteMaster = data; $scope.tbl_Inv_PurchaseNoteMaster.DocDate = new Date(data.DocDate);  
        };
      
        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterID }; };
       
    })
    .controller("PurchaseNoteDetailCtlr", function ($scope, $http) {
        
        $scope.MasterObject = {};
        $scope.$on('PurchaseNoteDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
            $scope.rptID = itm.ID;
        });

        $scope.$on('init_PurchaseNoteDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, itm.LoadByCard); 
            init_Report($scope, itm.Reports, '/Inventory/Challan/GetPurchaseNoteReport'); 
        });

        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {

            //--------when product change then PO should be selected again with respect to new product
            $scope.tbl_Inv_PurchaseNoteDetail.FK_tbl_Inv_PurchaseOrderDetail_ID = null;
            $scope.tbl_Inv_PurchaseNoteDetail.FK_tbl_Inv_PurchaseOrderDetail_IDName = '';
            $scope.tbl_Inv_PurchaseNoteDetail.Quantity = 0;
            if (item.ID > 0) {
                $scope.tbl_Inv_PurchaseNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = item.ID;
                $scope.tbl_Inv_PurchaseNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = item.ProductName;
                $scope.tbl_Inv_PurchaseNoteDetail.MeasurementUnit = item.MeasurementUnit;                
            }
            else {

                $scope.tbl_Inv_PurchaseNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID = null;
                $scope.tbl_Inv_PurchaseNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_IDName = null;
                $scope.tbl_Inv_PurchaseNoteDetail.MeasurementUnit = null;
            }

            if (item.IsDecimal) { $scope.wholeNumberOrNot = ''; }
            else { $scope.wholeNumberOrNot = new RegExp("^-?[0-9][^\.]*$"); }
            
        };

        init_Operations($scope, $http,
            '/Inventory/Challan/PurchaseNoteDetailLoad', //--v_Load
            '/Inventory/Challan/PurchaseNoteDetailGet', // getrow
            '/Inventory/Challan/PurchaseNoteDetailPost' // PostRow
        );

        $scope.tbl_Inv_PurchaseNoteDetail = {
            'ID': 0, 'FK_tbl_Inv_PurchaseNoteMaster_ID': $scope.MasterObject.ID,
            'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
            'Quantity': 0, 'MfgBatchNo': '', 'MfgDate': null, 'ExpiryDate': null, 'Remarks': '', 'ReferenceNo': '',
            'FK_tbl_Inv_PurchaseOrderDetail_ID': null, 'FK_tbl_Inv_PurchaseOrderDetail_IDName': '',
            'NoOfContainers': null, 'PotencyPercentage': 0, 'IsProcessed': false, 'IsSupervised': false,
            'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
            'FK_tbl_Qc_ActionType_ID': 1, 'FK_tbl_Qc_ActionType_IDName': '', 'QuantitySample': 0,
            'CreatedByQcQa': '', 'CreatedDateQcQa': '', 'ModifiedByQcQa': '', 'ModifiedDateQcQa': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_PurchaseNoteDetails = [$scope.tbl_Inv_PurchaseNoteDetail];

        $scope.clearEntryPanel = function () {
            //rededine to orignal values
            $scope.tbl_Inv_PurchaseNoteDetail = {
                'ID': 0, 'FK_tbl_Inv_PurchaseNoteMaster_ID': $scope.MasterObject.ID,
                'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '',
                'Quantity': 0, 'MfgBatchNo': '', 'MfgDate': null, 'ExpiryDate': null, 'Remarks': '', 'ReferenceNo': '',
                'FK_tbl_Inv_PurchaseOrderDetail_ID': null, 'FK_tbl_Inv_PurchaseOrderDetail_IDName': '',
                'NoOfContainers': null, 'PotencyPercentage': 0, 'IsProcessed': false, 'IsSupervised': false,
                'CreatedBy': '', 'CreatedDate': '', 'ModifiedBy': '', 'ModifiedDate': '',
                'FK_tbl_Qc_ActionType_ID': 1, 'FK_tbl_Qc_ActionType_IDName': '', 'QuantitySample': 0,
                'CreatedByQcQa': '', 'CreatedDateQcQa': '', 'ModifiedByQcQa': '', 'ModifiedDateQcQa': ''
            };
        };

        $scope.postRowParam = function () { return { validate: true, params: { operation: $scope.ng_entryPanelSubmitBtnText }, data: $scope.tbl_Inv_PurchaseNoteDetail }; };

        $scope.GetRowResponse = function (data, operation) {
            $scope.tbl_Inv_PurchaseNoteDetail = data;
            if (data.MfgDate !== null)
                $scope.tbl_Inv_PurchaseNoteDetail.MfgDate = new Date(data.MfgDate);
            if (data.ExpiryDate !== null)
                $scope.tbl_Inv_PurchaseNoteDetail.ExpiryDate = new Date(data.ExpiryDate);  
        };

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

        //-----------------------Excel Upload----------------------//
        $scope.LoadFileData = function (files) {
            var formData = new FormData();
            formData.append("PNDExcelFile", files[0]);

            var successcallback = function (response) {
                if (response.data === 'OK') {                    
                    alert('Successfully Updated');
                }
                else {

                    var textContent = response.data;
                    var blob = new Blob([textContent], { type: "text/plain" });
                    var fileName = "log.txt";
                    var downloadLink = document.createElement("a");
                    downloadLink.href = window.URL.createObjectURL(blob);
                    downloadLink.download = fileName;
                    document.body.appendChild(downloadLink);
                    downloadLink.click();
                    document.body.removeChild(downloadLink);
                }

                document.getElementById('UploadExcelFile').value = '';
                $scope.pageNavigation('first');
            };
            var errorcallback = function (error) {
            };

            $http({
                method: "POST", url: "/Inventory/Challan/PurchaseNoteDetailUploadExcelFile", params: { MasterID: $scope.MasterObject.ID, operation: 'Save New' }, data: formData, headers: { 'Content-Type': undefined, 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }, transformRequest: angular.identity
            }).then(successcallback, errorcallback);
        };

        //------------------------PO Search Modal-------------------------//
        $scope.POSearchResult = [{ 'ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_ID': null, 'FK_tbl_Inv_ProductRegistrationDetail_IDName': '', 'MeasurementUnit': '', 'PONo': null, 'PODate': null, 'Quantity': 0, 'Bal': 0 }];
        
        $scope.OpenPOSearchModalGeneral = function () {
            $scope.POFilterBy = 'byPONo';            
            $scope.POSearchResult.length = 0;
            $scope.POFilterValue = '';
            $('#POSearchModalGeneral').modal('show');
        };

        $scope.General_POSearch = function () {

            var successcallback = function (response) {
                $scope.POSearchResult = response.data;
            };
            var errorcallback = function (error) {
            };
            $http({ method: "GET", url: "/Inventory/Orders/GetPurchaseOrderList?QueryName=PurchaseNote" + "&POFilterBy=" + $scope.POFilterBy + "&POFilterValue=" + $scope.POFilterValue + "&SupplierID=" + $scope.MasterObject.FK_tbl_Ac_ChartOfAccounts_ID + "&ProductID=" + $scope.tbl_Inv_PurchaseNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID, async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

        };

        $scope.General_POSelectedAc = function (item) {
            if (item.ID > 0) {
                $scope.tbl_Inv_PurchaseNoteDetail.FK_tbl_Inv_PurchaseOrderDetail_ID = item.ID;
                $scope.tbl_Inv_PurchaseNoteDetail.FK_tbl_Inv_PurchaseOrderDetail_IDName = item.PONo;
                $scope.tbl_Inv_PurchaseNoteDetail.Quantity = item.Bal;

            }
            else {
                $scope.tbl_Inv_PurchaseNoteDetail.FK_tbl_Inv_PurchaseOrderDetail_ID = null;
                $scope.tbl_Inv_PurchaseNoteDetail.FK_tbl_Inv_PurchaseOrderDetail_IDName = null;
                $scope.tbl_Inv_PurchaseNoteDetail.Quantity = 0;
            }
        };

        $(function () {
            $('#POSearchModalGeneral').on('shown.bs.modal', function () {
                $('#POFilterBy').focus();
            });
        });

        $(function () {
            $('#POSearchModalGeneral').on('hidden.bs.modal', function () {
                $('[name="tbl_Inv_PurchaseNoteDetail.FK_tbl_Inv_PurchaseOrderDetail_IDName"]').focus();
            });
        });

    })
    .controller("PurchaseNoteDetailOfDetailCtlr", function ($scope, $http) {

        $scope.MasterObject = {};
        $scope.$on('PurchaseNoteDetailOfDetailCtlr', function (e, itm) {
            $scope.MasterObject = itm;
            $scope.pageNavigation('first');
        });

        $scope.$on('init_PurchaseNoteDetailOfDetailCtlr', function (e, itm) {
            init_Filter($scope, itm.WildCard, null, null, null);
        });

        init_Operations($scope, $http,
            '/Inventory/Challan/PurchaseNoteDetailOfDetailLoad', //--v_Load
            '', // getrow
            '' // PostRow
        );

        $scope.tbl_Inv_Ledger = {
            'ID': 0, 'PostingDate': new Date(), 'Narration': '', 'QuantityIn': 0, 'QuantityOut': 0, 'Ref': ''
        };

        //for list model which will be coming as as data in pageddata
        $scope.tbl_Inv_Ledgers = [$scope.tbl_Inv_Ledger];

        $scope.pageNavigatorParam = function () { return { MasterID: $scope.MasterObject.ID }; };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    