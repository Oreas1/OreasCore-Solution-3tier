var MainModule = angular.module("MainModule", ['ngAnimate']);

function scrollSmoothToBottom(id) {
    var div = document.getElementById(id);
    $('#' + id).animate({
        scrollTop: div.scrollHeight - div.clientHeight
    }, 500);
}
MainModule
    .controller("HeaderCtlr", function ($scope, $http, $animate) {

        //$(document).ready(function () {
        //    $(loginform['LoginModel.UserName']).focus();
        //});

        $scope.Login = function () {
            loadingstart();
            $scope.AreaList = [];
            $scope.DashBoardList = [];

            var successcallback = function (response) { 
                $scope.AreaList = Array.isArray(response.data.AuthorizedAreaList.Areas) ? response.data.AuthorizedAreaList.Areas : [];
                $scope.DashBoardList = Array.isArray(response.data.AuthorizedAreaList.Dashboards) ? response.data.AuthorizedAreaList.Dashboards : [];

                localStorage.removeItem('AuthorizedArea');
                localStorage.setItem('AuthorizedArea', JSON.stringify($scope.AreaList));    

                localStorage.removeItem('AuthorizedDashBoard');
                localStorage.setItem('AuthorizedDashBoard', JSON.stringify($scope.DashBoardList)); 
                
                loadingstop();
                if (response.data.redirectUrl == null || response.data.redirectUrl == '' || response.data.redirectUrl == undefined)
                    window.open('/', '_self');
                else
                    window.open(response.data.redirectUrl, '_self');
                
            };
            var errorcallback = function (error) { loadingstop(); alert(error.data); console.log(error);  };

            $http({
                method: "POST", url: '/Identity/Account/Login', async: true, params: { }, data: $scope.LoginModel, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
                }).then(successcallback, errorcallback); 
        };

        $scope.AreaList = JSON.parse(localStorage.getItem("AuthorizedArea") || "[]");
        $scope.DashBoardList = JSON.parse(localStorage.getItem("AuthorizedDashBoard") || "[]");
        
        $scope.clearAuthorizedAreaStorage = function () {          
            localStorage.removeItem('AuthorizedArea');
            localStorage.removeItem('AuthorizedDashBoard');
        };
        $scope.Logout = function () {
            loadingstart();
            $scope.AreaList = [];
            $scope.DashBoardList = [];
            var successcallback = function (response) {
                loadingstop();
                if (response.data.redirectUrl != null || response.data.redirectUrl != '' || response.data.redirectUrl != undefined)
                    window.open(response.data.redirectUrl, '_self');
            };

            var errorcallback = function (error) { loadingstop(); };
            $http({
                method: "POST", url: '/Identity/Account/Logout', async: true, params: { }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
            }).then(successcallback, errorcallback);
        };
    });

function init_Operations ($scope, $http, pageNavigationURL, v_GetRowURL, v_PostRowURL) {
 
    $scope.ng_entryPanelBtnText = 'Add New';
    $scope.ng_entryPanelHide = true;
    $scope.ng_entryPanelSubmitBtnText = 'Save New';
    $scope.ng_readOnly = false;
    $scope.ng_readOnlyOnEdit = false;
    $scope.ng_DisabledBtnAll = false;
    $scope.ng_DisabledBtnPageN = true;
    $scope.ng_DisabledBtnPageL = true;
    $scope.ng_DisabledBtnPageP = true;
    $scope.ng_DisabledBtnPageF = true;
    $scope.AddBulk = false;
    $scope.IsPosting = false;

    //----------------------------------------Paging functions----------------------------------------//

    $scope.pageddata = { 'Data': [], 'TotalPages': 0, 'CurrentPage': 1 };

    $scope.NavigationControlDisabled = function () {

        $scope.ng_DisabledBtnAll = true;
        $scope.ng_DisabledBtnPageN = true;
        $scope.ng_DisabledBtnPageL = true;
        $scope.ng_DisabledBtnPageP = true;
        $scope.ng_DisabledBtnPageF = true;

    };

    $scope.NavigationControlEnabled = function () {

        $scope.NavigationControlDisabled();
        $scope.ng_DisabledBtnAll = false;
        //deciding on the basis of total pages which button need to be enabled
        if ($scope.pageddata.TotalPages > 0) {
            if ($scope.pageddata.CurrentPage < $scope.pageddata.TotalPages) {
                $scope.ng_DisabledBtnPageN = false;
                $scope.ng_DisabledBtnPageL = false;
            }
            if ($scope.pageddata.CurrentPage > 1) {
                $scope.ng_DisabledBtnPageP = false;
                $scope.ng_DisabledBtnPageF = false;
            }
        }

    };

    //----------------------------------------xxxxxxxxxxxxxx----------------------------------------//
    $scope.showEntryPanel = function () {
        if ($scope.ng_entryPanelHide) {
            $scope.ng_entryPanelHide = false;
            $scope.ng_entryPanelBtnText = 'Cancel';
            if ($scope.AddBulk)
                $scope.ng_entryPanelSubmitBtnText = 'Save New Bulk';
            else
                $scope.ng_entryPanelSubmitBtnText = 'Save New';
        }
        else {
            $scope.ng_entryPanelHide = true;
            $scope.ng_entryPanelBtnText = 'Add New';
        }
        $scope.clearEntryPanel();
        $scope.ng_readOnly = false;
        $scope.ng_readOnlyOnEdit = false;
    };

    //-------For get Function initializer------------//
    $scope.GetInit = function () {
        $scope.ng_entryPanelHide = false;
        $scope.ng_entryPanelBtnText = 'Cancel';
        $scope.clearEntryPanel();
    };

    if (typeof pageNavigationURL !== undefined && pageNavigationURL !== '' && pageNavigationURL !== null) {
        
        $scope.pageddata = { 'Data': [], 'TotalPages': 0, 'CurrentPage': 1 };
        $scope.pageNavigation = function (action) {         
            
            let promise = new Promise((resolve, reject) => {
                $scope.NavigationControlDisabled();
                //$scope.pageddata.Data.length = 0;

                $scope.ng_entryPanelHide = true;
                $scope.ng_entryPanelBtnText = 'Add New';

                var successcallback = function (response) {               

                    if (typeof $scope.GetpageNavigationResponse === "function") {
                        $scope.GetpageNavigationResponse(response.data);
                    }
                    else {
                        $scope.pageddata = response.data.pageddata;
                    }
                    
                    $scope.NavigationControlEnabled();
                    resolve();
                };
                var errorcallback = function (error) {
                    console.log('PageLoad',error);
                    $scope.NavigationControlEnabled();
                    reject();

                };
                loadingstart();

                $scope.CurrentPage = 1;
                if (action === 'Load')
                    $scope.CurrentPage = $scope.pageddata.CurrentPage;
                else if (action === 'next' && $scope.pageddata.TotalPages > $scope.pageddata.CurrentPage)
                    $scope.CurrentPage = $scope.pageddata.CurrentPage + 1;
                else if (action === 'last' && $scope.pageddata.CurrentPage < $scope.pageddata.TotalPages)
                    $scope.CurrentPage = $scope.pageddata.TotalPages;
                else if (action === 'back' && $scope.pageddata.CurrentPage > 1)
                    $scope.CurrentPage = $scope.pageddata.CurrentPage - 1;
                else if (action === 'first' && $scope.pageddata.CurrentPage >= 1)
                    $scope.CurrentPage = 1;

             
                //---------------------------------------setting default parameter---------------------//
                let myParam = $scope.pageNavigatorParam();
                myParam['CurrentPage'] = $scope.CurrentPage;

                myParam['FilterByText'] = $scope.FilterByText; myParam['FilterValueByText'] = $scope.FilterValueByText;

                myParam['FilterByNumberRange'] = $scope.FilterByNumberRange;
                myParam['FilterValueByNumberRangeFrom'] = $scope.FilterValueByNumberRangeFrom;
                myParam['FilterValueByNumberRangeTill'] = $scope.FilterValueByNumberRangeTill;

                myParam['FilterByDateRange'] = $scope.FilterByDateRange;
                myParam['FilterValueByDateRangeFrom'] = new Date($scope.FilterValueByDateRangeFrom).toLocaleString('en-US'); 
                myParam['FilterValueByDateRangeTill'] = new Date($scope.FilterValueByDateRangeTill).toLocaleString('en-US');

                //myParam['FilterValueByDateRangeFrom'] = typeof $scope.FilterValueByDateRangeFrom === undefined ? null : $scope.FilterValueByDateRangeFrom;
                //myParam['FilterValueByDateRangeTill'] = typeof $scope.FilterValueByDateRangeTill === undefined ? null : $scope.FilterValueByDateRangeTill;

                myParam['FilterByLoad'] = $scope.FilterByLoad;
               

                $http({ method: "GET", url: pageNavigationURL, async: true, params: myParam, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

            });
            // return of a promise is required
            return promise.catch(function (reason) { });


        };
    }

    if (typeof v_GetRowURL !== undefined && v_GetRowURL !== '' && v_GetRowURL !== null) {

        $scope.GetRow = function (id, operation) {
            $scope.NavigationControlDisabled();
            $scope.AddBulk = false;
            if (operation === 'Copy' || operation === 'Add') { $scope.ng_entryPanelSubmitBtnText = 'Save New'; $scope.ng_readOnly = false; $scope.ng_readOnlyOnEdit = false; }
            else if (operation === 'Edit') { $scope.ng_entryPanelSubmitBtnText = 'Save Update'; $scope.ng_readOnly = false; $scope.ng_readOnlyOnEdit = true; }
            else if (operation === 'Delete') { $scope.ng_entryPanelSubmitBtnText = 'Save Delete'; $scope.ng_readOnly = true; $scope.ng_readOnlyOnEdit = true; }
            else if (operation === 'View') { $scope.ng_entryPanelSubmitBtnText = 'Close'; $scope.ng_readOnly = true; $scope.ng_HideSubmitBtn = true; $scope.ng_readOnlyOnEdit = true; }

            var successcallback = function (response) {
         
                $scope.GetInit();
                if (typeof $scope.GetRowResponse === "function") {
                    if (response.data === 'Not Authenticated, Please Sign-In into the application ')
                    {
                        alert(response.data);
                        window.location.href = '/';
                    }
                    else
                        $scope.GetRowResponse(response.data, operation);
                 
                }                
                $scope.NavigationControlEnabled();    
            
            };
            var errorcallback = function (error) { $scope.NavigationControlEnabled(); console.log('Get',error); $scope.showEntryPanel(); setOperationMessage(error.statusText, 0); };

            $http({ method: "GET", url: v_GetRowURL, async: true, params: { id: id }, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
        };
    }

    if (typeof v_PostRowURL !== undefined && v_PostRowURL !== '' && v_PostRowURL !== null) {

        $scope.PostRow = function (pageNavigationBy) {       
            if ($scope.IsPosting)
            {
                alert('Aborted! Already one Request is in progress, therefore multiple submissions not allowed.');
                return;
            };

            let para = $scope.postRowParam();           

            if (para.validate === false)
                return;

            if ($scope.ng_entryPanelSubmitBtnText === 'Close') {
                $scope.showEntryPanel();
                return;
            }

            var successcallback = function (response) {
                $scope.IsPosting = false;
                if (typeof $scope.PostRowResponse === "function") {
                    $scope.PostRowResponse(response.data);
                }
                else {
                    if (response.data === 'OK') {
                        $scope.showEntryPanel();
                        if (typeof pageNavigationBy !== 'undefined') {
                            $scope.pageNavigation(pageNavigationBy);                            
                        }
                        //---------if new record then goto first page to see effect-------------//
                        else if ($scope.ng_entryPanelSubmitBtnText === 'Save New')
                            $scope.pageNavigation('first');
                        else
                            $scope.pageNavigation('Load');
                    }
                    else if (response.data === 'Not Authenticated, Please Sign-In into the application ') {
                        alert(response.data);
                        window.location.href = '/';
                    }
                    else {
                        alert(response.data);
                    }
                }                
                
            };
            var errorcallback = function (error) {
                $scope.IsPosting = false;
                if ($scope.ng_entryPanelSubmitBtnText === 'Save Delete')
                    $scope.ng_readOnly = true;
                console.log('Post error', error);
       
            };
            
            if (confirm("Are you sure! you want to " + $scope.ng_entryPanelSubmitBtnText) === true) {
                $scope.IsPosting = true;
                $http({
                    method: "POST", url: v_PostRowURL, async: true, params: para.params, data: para.data, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
                }).then(successcallback, errorcallback);
            }
        };
    }

}

function init_ViewSetup($scope,$http, pageUrl)
{
    if (typeof pageUrl !== undefined && pageUrl !== '' && pageUrl !== null) {
        var successcallback = function (response) {
            
            if (typeof $scope.init_ViewSetup_Response === "function") {
                $scope.init_ViewSetup_Response(response.data);
            }
        };
        var errorcallback = function (error) { };

        $http({ method: "GET", url: pageUrl, async: true, params: {}, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

    }
}
///-----------------fiscal year-------------------//
function init_FiscalYear($scope, obj)
{
    $scope.FYS = null;
    $scope.FYE = null;

    if (obj != undefined) {
        $scope.FYS = obj.FYS != undefined ? new Date(obj.FYS) : null;
        $scope.FYE = obj.FYE != undefined ? new Date(obj.FYE) : null;
    }
}
///--------------------------Filter----------------------------------------//
function init_Filter
    ($scope, _FilterByTextList, _FilterByNumberRangeList, _FilterByDateRangeList, _FilterByLoadList) {


    $scope.FilterByText = null; $scope.FilterValueByText = '';
    $scope.FilterByNumberRange = null; $scope.FilterValueByNumberRangeFrom = 1; $scope.FilterValueByNumberRangeTill = 1;
    $scope.FilterValueByDateRangeFrom = new Date(); $scope.FilterValueByDateRangeTill = new Date();
    var today = new Date();
    $scope.FilterValueByDateRangeFrom = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 0, 0, 0, 0);
    $scope.FilterValueByDateRangeTill = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 23, 59, 0, 0);
    $scope.FilterByLoad = null;

    $scope.FilterByTextList = _FilterByTextList;
    $scope.FilterByNumberRangeList = _FilterByNumberRangeList;
    $scope.FilterByDateRangeList = _FilterByDateRangeList;
    $scope.FilterByLoadList = _FilterByLoadList;

    $scope.ReloadbyFilter = function (callerDomID) {
        if ($scope.FilterByNumberRange !== null) {
            if (!$scope.FilterValueByNumberRangeFrom >= 0)
                $scope.FilterValueByNumberRangeFrom = 0;

            if (!$scope.FilterValueByNumberRangeTill >= 0)
                $scope.FilterValueByNumberRangeTill = 0;
        }

        if ($scope.FilterByDateRange !== null && (isNaN(Date.parse($scope.FilterValueByDateRangeFrom)) || isNaN(Date.parse($scope.FilterValueByDateRangeTill)))) {
            alert('Invalid Date in Filter DateRange');
            return;
        }

        $scope.pageNavigation('first').then(() => { document.getElementById(callerDomID).focus(); }, () => { console.log('rejected'); });

    };

}

///--------------------------Report----------------------------------------//
function init_Report
    ($scope, rptList, Url_CallReport) {

    if (typeof rptList !== undefined && rptList !== '' && rptList !== null
        &&
        typeof Url_CallReport !== undefined && Url_CallReport !== '' && Url_CallReport !== null) {
        var today = new Date();
        $scope._Report_DateFrom = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 0, 0, 0, 0);
        $scope._Report_DateTill = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 23, 59, 0, 0);
        $scope.SerialDiv = true; $scope.DateDiv = true; $scope.GroupByDiv = true; $scope.OrderByDiv = true; $scope.SeekByDiv = true;
        $scope.rptList = rptList; $scope.rptID = 0; $scope.ReportName = null;   
        $scope.DisableDateFrom = true; $scope.DisableDateTill = true; 

        $scope.onSelectReport = function (reportName) {
            
            $scope.SerialDiv = true; $scope.DateDiv = true; $scope.GroupByDiv = true; $scope.OrderByDiv = true; $scope.SeekByDiv = true;
            $scope.DisableDateFrom = true; $scope.DisableDateTill = true; 
            $scope._Report_SerialNoFrom = 1; $scope._Report_SerialNoTill = 1;
            $scope._Report_GroupBy = null; $scope._Report_OrderBy = null; $scope._Report_SeekBy = null;
            $scope.groupbyList = {}; $scope.orderbyList = {}; $scope.seekbyList = {};

            if (reportName !== undefined) {
                var rpt = $scope.rptList.find(e => e.ReportName === reportName);

                if (rpt.ReportType === 0) {
                    $scope.DateDiv = false;
                    $scope.DisableDateFrom = false; $scope.DisableDateTill = false; 
                }                    

                if (rpt.ReportType === 1)
                    $scope.SerialDiv = false;

                if (rpt.ReportType === 2) {
                    $scope.DateDiv = true;
                    $scope.SerialDiv = true;
                }
                if (rpt.ReportType === 4) {
                    $scope.DateDiv = false;
                    $scope.DisableDateFrom = true; $scope.DisableDateTill = false; 
                }
                    

                //----------------------------------------------------------------------//


                if (rpt.GroupBy !== null) {
                    $scope.GroupByDiv = false;
                    $scope.groupbyList = rpt.GroupBy;
                }


                if (rpt.OrderBy !== null) {
                    $scope.OrderByDiv = false;
                    $scope.orderbyList = rpt.OrderBy;
                }


                if (rpt.SeekBy !== null) {
                    $scope.SeekByDiv = false;
                    $scope.seekbyList = rpt.SeekBy;
                }




                //----------------------------------------------------------------------//
            }

        };

        $scope.ReportCalling = function () {
            var url = Url_CallReport;
            
            if (url.indexOf("?") > -1)
                url = url + '&rn=' + $scope.ReportName;
            else 
                url = url + '?rn=' + $scope.ReportName;
                
            

            if (typeof $scope.rptID === undefined)
                url = url + "&id=0";
            else
                url = url + "&id=" + $scope.rptID;


            if (typeof $scope._Report_DateFrom === undefined)
                url = url + "&datefrom=" + new Date();
            else
                url = url + '&datefrom=' + new Date($scope._Report_DateFrom).toLocaleString('en-US');

            if (typeof $scope._Report_DateTill === undefined)
                url = url + "&datetill=" + new Date();
            else
                url = url + '&datetill=' + new Date($scope._Report_DateTill).toLocaleString('en-US');

            if (typeof $scope._Report_GroupBy === undefined || $scope.groupbyList === null || $scope._Report_GroupBy === null)
                url = url + "&GroupBy=" + '';
            else
                url = url + '&GroupBy=' + $scope._Report_GroupBy;

            if (typeof $scope._Report_SeekBy === undefined || $scope.seekbyList === null || $scope._Report_SeekBy === null)
                url = url + "&SeekBy=" + '';
            else
                url = url + '&SeekBy=' + $scope._Report_SeekBy;

            if (typeof $scope._Report_OrderBy === undefined || $scope.orderbyList === null || $scope._Report_OrderBy === null)
                url = url + "&OrderBy=" + '';
            else
                url = url + '&OrderBy=' + $scope._Report_OrderBy;


            window.open(url, '_blank');

        };

        $scope.SetSerialNo = function () {
            $scope._Report_SerialNoTill = $scope._Report_SerialNoFrom;
        };

    }
}

///--------------------------Employee Modal----------------------------------------//
function init_EmployeeSearchModalGeneral($scope, $http) {
  
    $scope.EmployeeSearchResult = [{ 'ID': null, 'EmployeeNo': null, 'EmployeeName': '', 'ATEnrollmentNo_Default': '', 'Department': '', 'Designation': '', 'LevelName': '', 'Photo': '', 'OtherInfo': '' }];
    $scope.EmployeeSearch_QueryName = ''; $scope.EmployeeSearch_FormID = 0; $scope.EmployeeSearch_CallerName = ''; $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection0 = null;


    $scope.OpenEmployeeSearchModalGeneral = function (QueryName, FormID, callerName, FunctionOnInvokeSelection) {
        if ($scope.EmployeeFilterBy === undefined || $scope.EmployeeFilterBy === null) {
            $scope.EmployeeFilterBy = 'EmployeeName';
        }
        $scope.EmployeeSearchResult.length = 0;
        $scope.EmployeeFilterValue = '';

        $scope.EmployeeSearch_QueryName = QueryName;
        $scope.EmployeeSearch_FormID = FormID;
        $scope.EmployeeSearch_CallerName = callerName;
        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection0 = FunctionOnInvokeSelection;
        $('#EmployeeSearchModalGeneral').modal('show');
    };


    $scope.General_EmployeeSearch = function () {
        
        var successcallback = function (response) {            
            $scope.EmployeeSearchResult = response.data;         
        };
        var errorcallback = function (error) {
        };
        $http({ method: "GET", url: "/WPT/Employee/GetEmployeeList?QueryName=" + $scope.EmployeeSearch_QueryName + "&EmployeeFilterBy=" + $scope.EmployeeFilterBy + "&EmployeeFilterValue=" + $scope.EmployeeFilterValue + "&FormID=" + $scope.EmployeeSearch_FormID, async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

    };

    $scope.General_EmployeeSelectedAc = function (item) {        
        if (typeof $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection0 === 'function')
            $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection0(item);     
    };

    $(function () {
        $('#EmployeeSearchModalGeneral').on('shown.bs.modal', function () {
            $('#EmployeeFilterBy').focus();
        });
    });

    $(function () {
        $('#EmployeeSearchModalGeneral').on('hidden.bs.modal', function () {
            if (typeof $scope.EmployeeSearch_CallerName !== undefined && $scope.EmployeeSearch_CallerName !== null && $scope.EmployeeSearch_CallerName !== '')
                $('[name="' + $scope.EmployeeSearch_CallerName + '"]').focus();
        });
    });

}
///--------------------------Leave Policy Search Modal----------------------------------------//
function init_LeavePolicySearchModal($scope, $http) {

    $scope.PolicySearchResult = [{ 'ID': null, 'PolicyName': '', 'PolicyPrefix': '', 'MonthBalance': 0, 'AnnualBalance': 0, 'LeaveType': '' }];

    $scope.LeavePolicySearch_QueryName = '';
    $scope.LeavePolicySearch_EmpID = 0; $scope.LeavePolicySearch_MonthID = 0;
    $scope.LeavePolicySearch_CallerName = ''; $scope.LeavePolicySearch_CtrlFunction_Ref_InvokeOnSelection0 = null;
    $scope.LeavePolicySearchModalEmployeeName = '';

    $scope.OpenLeavePolicySearchModalGeneral = function (QueryName, EmpID, MonthID, EmpName, callerName, FunctionOnInvokeSelection) {
        $scope.PolicyStatus = 'Please While Policy is loading from server';
        $scope.PolicySearchResult.length = 0;

        $scope.LeavePolicySearchModalEmployeeName = EmpName;

        $scope.LeavePolicySearch_QueryName = QueryName;
        $scope.LeavePolicySearch_EmpID = EmpID;
        $scope.LeavePolicySearch_MonthID = MonthID;
        $scope.LeavePolicySearch_CallerName = callerName;
        $scope.LeavePolicySearch_CtrlFunction_Ref_InvokeOnSelection0 = FunctionOnInvokeSelection;
        $scope.General_PolicySearch();
    };

    $scope.General_PolicySearch = function () {
        var successcallback = function (response) {
            $scope.PolicySearchResult = response.data;
            $scope.PolicyStatus = 'Loaded sucessfully';
            $('#LeavePolicySearchModalGeneral').modal('show');
        };
        var errorcallback = function (error) {
            $scope.PolicyStatus = 'Some thing went wrong while loading policy';
        };
        $http({ method: "GET", url: "/WPT/Leave/GetLeavePoliciesList?QueryName=" + $scope.LeavePolicySearch_QueryName + "&EmployeeID=" + $scope.LeavePolicySearch_EmpID + "&MonthID=" + $scope.LeavePolicySearch_MonthID, async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

    };


    $scope.General_PolicySelectedAc = function (item) {
        if (typeof $scope.LeavePolicySearch_CtrlFunction_Ref_InvokeOnSelection0 === 'function')
            $scope.LeavePolicySearch_CtrlFunction_Ref_InvokeOnSelection0(item);
    };

    $(function () {
        $('#LeavePolicySearchModalGeneral').on('shown.bs.modal', function () {
            $('#LeavePolicySearchModalGeneralClosed').focus();
        });
    });

    $(function () {
        $('#LeavePolicySearchModalGeneral').on('hidden.bs.modal', function () {
            if (typeof $scope.LeavePolicySearch_CallerName !== undefined && $scope.LeavePolicySearch_CallerName !== null && $scope.LeavePolicySearch_CallerName !== '')
                $('[name="' + $scope.LeavePolicySearch_CallerName + '"]').focus();
        });
    });

}
///--------------------------Month Modal----------------------------------------//
function init_MonthSearchModal($scope, $http) {

    $scope.MonthSearchResult = [{ 'ID': null, 'MonthStart': '', 'MonthEnd': '', 'IsClosed': false, 'CalendarYearID': null, 'CalendarYear': null }];

    $scope.MonthSearch_QueryName = ''; $scope.MonthSearch_FormID = 0; $scope.MonthSearch_CallerName = ''; $scope.MonthSearch_CtrlFunction_Ref_InvokeOnSelection0 = null;

    $scope.OpenMonthSearchModalGeneral = function (QueryName, FormID, callerName, FunctionOnInvokeSelection) {

        $scope.MonthSearchResult.length = 0;
        $scope.SearchMonth = (new Date().getMonth() + 1).toString();
        $scope.SearchYear = new Date().getFullYear();

        $scope.MonthSearch_QueryName = QueryName;
        $scope.MonthSearch_FormID = FormID;
        $scope.MonthSearch_CallerName = callerName;
        $scope.MonthSearch_CtrlFunction_Ref_InvokeOnSelection0 = FunctionOnInvokeSelection;
        $('#MonthSearchModalGeneral').modal('show');
    };

    $scope.General_MonthSearch = function () {
        var successcallback = function (response) {
            $scope.MonthSearchResult = response.data;
        };
        var errorcallback = function (error) {
        };
        $http({ method: "GET", url: "/WPT/Calendar/GetMonthList?QueryName=" + $scope.MonthSearch_QueryName + "&SearchMonth=" + $scope.SearchMonth + "&SearchYear=" + $scope.SearchYear + "&FormID=" + $scope.MonthSearch_FormID, async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);
    };

    $scope.General_MonthSelectedAc = function (item) {

        if (typeof $scope.MonthSearch_CtrlFunction_Ref_InvokeOnSelection0 === 'function')
            $scope.MonthSearch_CtrlFunction_Ref_InvokeOnSelection0(item);

    };

    $(function () {
        $('#MonthSearchModalGeneral').on('shown.bs.modal', function () {
            $('#SearchMonth').focus();
        });
    });

    $(function () {
        $('#MonthSearchModalGeneral').on('hidden.bs.modal', function () {
            if (typeof $scope.MonthSearch_CallerName !== undefined && $scope.MonthSearch_CallerName !== null && $scope.MonthSearch_CallerName !== '')
                $('[name="' + $scope.MonthSearch_CallerName + '"]').focus();
        });
    });

}

///--------------------------COA Modal----------------------------------------//
function init_COASearchModalGeneral($scope, $http) {
    $scope.COASearchResult = [{ 'ID': null, 'AccountCode': null, 'AccountName': '' }];
    $scope.COASearch_QueryName = ''; $scope.COASearch_FormID = 0; $scope.COASearch_CallerName = ''; $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection0 = null;
    

    $scope.OpenCOASearchModalGeneral = function (QueryName, FormID, callerName, FunctionOnInvokeSelection) {
        if ($scope.COAFilterBy === undefined || $scope.COAFilterBy === null) {
            $scope.COAFilterBy = 'AccountName';
        }
        $scope.COASearchResult.length = 0;
        $scope.COAFilterValue = '';

        $scope.COASearch_QueryName = QueryName;
        $scope.COASearch_FormID = FormID;
        $scope.COASearch_CallerName = callerName;
        $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection0 = FunctionOnInvokeSelection;
        $('#COASearchModalGeneral').modal('show');
    };


    $scope.General_COASearch = function () {
   
        var successcallback = function (response) {
            $scope.COASearchResult = response.data;
        };
        var errorcallback = function (error) {
        };
        $http({ method: "GET", url: "/Accounts/ChartOfAccounts/GetCOAList?QueryName=" + $scope.COASearch_QueryName + "&COAFilterBy=" + $scope.COAFilterBy + "&COAFilterValue=" + $scope.COAFilterValue + "&FormID=" + $scope.COASearch_FormID, async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

    };

    $scope.General_COASelectedAc = function (item) {
        if (typeof $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection0 === 'function')
            $scope.COASearch_CtrlFunction_Ref_InvokeOnSelection0(item);

    };

    $(function () {
        $('#COASearchModalGeneral').on('shown.bs.modal', function () {
            $('#COAFilterBy').focus();
        });
    });

    $(function () {
        $('#COASearchModalGeneral').on('hidden.bs.modal', function () {
            if (typeof $scope.COASearch_CallerName !== undefined && $scope.COASearch_CallerName !== null && $scope.COASearch_CallerName !== '')
                $('[name="' + $scope.COASearch_CallerName + '"]').focus();
        });
    });

}

///--------------------------WHM Modal----------------------------------------//
function init_WHMSearchModalGeneral($scope, $http) {
    $scope.WHMSearchResult = [{ 'ID': null, 'WareHouseName': null, 'Prefix': '' }];
    $scope.WHMSearch_QueryName = ''; $scope.WHMSearch_FormID = 0; $scope.WHMSearch_CallerName = ''; $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection0 = null;
 
    $scope.OpenWHMSearchModalGeneral = function (QueryName, FormID, callerName, FunctionOnInvokeSelection) {
        
        if ($scope.WHMFilterBy === undefined || $scope.WHMFilterBy === null) {
            $scope.WHMFilterBy = 'WareHouseName';
        }

        $scope.WHMSearchResult.length = 0;
        $scope.WHMFilterValue = '';

        $scope.WHMSearch_QueryName = QueryName;
        $scope.WHMSearch_FormID = FormID;
        $scope.WHMSearch_CallerName = callerName;
        $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection0 = FunctionOnInvokeSelection;
        $('#WHMSearchModalGeneral').modal('show');
    };


    $scope.General_WHMSearch = function () {

        var successcallback = function (response) {
            $scope.WHMSearchResult = response.data;
        };
        var errorcallback = function (error) {
        };
        $http({ method: "GET", url: "/Inventory/SetUp/GetWHMList?QueryName=" + $scope.WHMSearch_QueryName + "&WHMFilterBy=" + $scope.WHMFilterBy + "&WHMFilterValue=" + $scope.WHMFilterValue + "&FormID=" + $scope.WHMSearch_FormID, async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

    };

    $scope.General_WHMSelectedAc = function (item) {
        if (typeof $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection0 === 'function')
            $scope.WHMSearch_CtrlFunction_Ref_InvokeOnSelection0(item);
    };

    $(function () {
        $('#WHMSearchModalGeneral').on('shown.bs.modal', function () {
            $('#WHMFilterBy').focus();
        });
    });

    $(function () {
        $('#WHMSearchModalGeneral').on('hidden.bs.modal', function () {
            if (typeof $scope.WHMSearch_CallerName !== undefined && $scope.WHMSearch_CallerName !== null && $scope.WHMSearch_CallerName !== '')
                $('[name="' + $scope.WHMSearch_CallerName + '"]').focus();
        });
    });

}

///--------------------------Product Modal----------------------------------------//
function init_ProductSearchModalGeneral($scope, $http) {

    $scope.ProductSearchResult = [
        { 'ID': null, 'MasterProdID': null, 'ProductName': '', 'CategoryName': '', 'MeasurementUnit': '', 'Description': '', 'IsDecimal': false, 'Split_Into': 1, 'OtherDetail': '' }];   
    

    $scope.ProductSearch_QueryName = ''; $scope.ProductSearch_FormID = 0; $scope.ProductSearch_CallerName = ''; $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection0 = null;
    $scope.ProductSearch_TillDate = null; $scope.ProductSearch_DetailID = null;

    $scope.OpenProductSearchModalGeneral = function (QueryName, FormID, callerName, FunctionOnInvokeSelection, productSearch_tillDate, productSearch_detailID) {

        if ($scope.ProductFilterBy === undefined || $scope.ProductFilterBy === null)
            $scope.ProductFilterBy = 'ProductName';
           
        $scope.ProductSearchResult.length = 0;
        $scope.ProductFilterValue = '';

        $scope.ProductSearch_QueryName = QueryName;
        $scope.ProductSearch_FormID = FormID;
        $scope.ProductSearch_CallerName = callerName;
        $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection0 = FunctionOnInvokeSelection;
        $scope.ProductSearch_TillDate = productSearch_tillDate;
        $scope.ProductSearch_DetailID = productSearch_detailID;       
        $('#ProductSearchModalGeneral').modal('show');
    };


    $scope.General_ProductSearch = function () {        
        var successcallback = function (response) {
            $scope.ProductSearchResult = response.data;
        };
        var errorcallback = function (error) {
        };
        $http({ method: "GET", url: "/Inventory/SetUp/GetProductList?QueryName=" + $scope.ProductSearch_QueryName + "&ProductFilterBy=" + $scope.ProductFilterBy + "&ProductFilterValue=" + $scope.ProductFilterValue + "&masterid=" + $scope.ProductSearch_FormID + "&tillDate=" + $scope.productSearch_tillDate + "&detailid=" + $scope.ProductSearch_DetailID, async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

    };

    $scope.General_ProductSelectedAc = function (item) {
        if (typeof $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection0 === 'function')
            $scope.ProductSearch_CtrlFunction_Ref_InvokeOnSelection0(item);
    };

    $(function () {
        $('#ProductSearchModalGeneral').on('shown.bs.modal', function () {            
            $('#ProductFilterBy').focus();
        });
    });

    $(function () {
        $('#ProductSearchModalGeneral').on('hidden.bs.modal', function () {
            if (typeof $scope.ProductSearch_CallerName !== undefined && $scope.ProductSearch_CallerName !== null && $scope.ProductSearch_CallerName !== '')
                $('[name="' + $scope.ProductSearch_CallerName + '"]').focus();
        });
    });

}

///--------------------------Reference Modal----------------------------------------//
function init_ReferenceSearchModalGeneral($scope, $http) {

    $scope.ReferenceSearchResult = [
        { 'ReferenceNo': '', 'Balance': 0, 'FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo': null, 'FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo': null, 'OtherDetail': '' }];


    $scope.ReferenceSearch_QueryName = ''; $scope.ReferenceSearch_FormID = 0; $scope.ReferenceSearch_CallerName = ''; $scope.ReferenceSearch_CtrlFunction_Ref_InvokeOnSelection0 = null;
    $scope.ReferenceSearch_TillDate = null; $scope.ReferenceSearch_DetailID = null;

    $scope.OpenReferenceSearchModalGeneral = function (QueryName, FormID, callerName, FunctionOnInvokeSelection, ReferenceSearch_tillDate, ReferenceSearch_detailID) {
    
        if ($scope.ReferenceFilterBy === undefined || $scope.ReferenceFilterBy === null)
            $scope.ReferenceFilterBy = 'ReferenceNo';

        $scope.ReferenceSearchResult.length = 0;
        $scope.ReferenceFilterValue = '';

        $scope.ReferenceSearch_QueryName = QueryName;
        $scope.ReferenceSearch_FormID = FormID;
        $scope.ReferenceSearch_CallerName = callerName;
        $scope.ReferenceSearch_CtrlFunction_Ref_InvokeOnSelection0 = FunctionOnInvokeSelection;
        $scope.ReferenceSearch_TillDate = ReferenceSearch_tillDate;
        $scope.ReferenceSearch_DetailID = ReferenceSearch_detailID;
        $('#ReferenceSearchModalGeneral').modal('show');
    };


    $scope.General_ReferenceSearch = function () {
        var successcallback = function (response) {
            $scope.ReferenceSearchResult = response.data;
        };
        var errorcallback = function (error) {
        };
        $http({ method: "GET", url: "/Inventory/SetUp/GetReferenceList?QueryName=" + $scope.ReferenceSearch_QueryName + "&ReferenceFilterBy=" + $scope.ReferenceFilterBy + "&ReferenceFilterValue=" + $scope.ReferenceFilterValue + "&masterid=" + $scope.ReferenceSearch_FormID + "&tillDate=" + $scope.ReferenceSearch_tillDate + "&detailid=" + $scope.ReferenceSearch_DetailID, async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

    };

    $scope.General_ReferenceSelectedAc = function (item) {
        if (typeof $scope.ReferenceSearch_CtrlFunction_Ref_InvokeOnSelection0 === 'function')
            $scope.ReferenceSearch_CtrlFunction_Ref_InvokeOnSelection0(item);
    };

    $(function () {
        $('#ReferenceSearchModalGeneral').on('shown.bs.modal', function () {
            $('#ReferenceFilterBy').focus();
        });
    });

    $(function () {
        $('#ReferenceSearchModalGeneral').on('hidden.bs.modal', function () {
            if (typeof $scope.ReferenceSearch_CallerName !== undefined && $scope.ReferenceSearch_CallerName !== null && $scope.ReferenceSearch_CallerName !== '')
                $('[name="' + $scope.ReferenceSearch_CallerName + '"]').focus();
        });
    });

}

///--------------------------Applicant Modal----------------------------------------//
function init_ApplicantSearchModal($scope, $http) {

    $scope.ApplicantSearchResult = [{ 'ID': null, 'ApplicationNo': null, 'FullName': '', 'CNIC': '', 'Education': '' }];

    $scope.ApplicantSearch_QueryName = ''; $scope.ApplicantSearch_FormID = 0; $scope.ApplicantSearch_CallerName = ''; $scope.ApplicantSearch_CtrlFunction_Ref_InvokeOnSelection = null;


    $scope.OpenApplicantSearchModalGeneral = function (QueryName, FormID, callerName, FunctionOnInvokeSelection) {
        if (typeof $scope.ApplicantFilterBy === undefined || $scope.ApplicantFilterBy === null) {
            $scope.ApplicantFilterBy = 'ApplicantName';
        }
        $scope.ApplicantSearchResult.length = 0;
        $scope.ApplicantFilterValue = '';

        $scope.ApplicantSearch_QueryName = QueryName;
        $scope.ApplicantSearch_FormID = FormID;
        $scope.ApplicantSearch_CallerName = callerName;
        $scope.ApplicantSearch_CtrlFunction_Ref_InvokeOnSelection = FunctionOnInvokeSelection;
        $('#ApplicantSearchModalGeneral').modal('show');
    };
    

    $scope.General_ApplicantSearch = function () {

        var successcallback = function (response) {
            $scope.ApplicantSearchResult = response.data;
        };
        var errorcallback = function (error) {
        };
        $http({ method: "GET", url: "/WPT/Hiring/General_LoadApplicant?QueryName=" + $scope.ApplicantSearch_QueryName + "&ApplicantFilterBy=" + $scope.ApplicantFilterBy + "&ApplicantFilterValue=" + $scope.ApplicantFilterValue + "&FormID=" + $scope.ApplicantSearch_FormID, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

    };

    $scope.General_ApplicantSelectedAc = function (item) {
        
        if (typeof $scope.ApplicantSearch_CtrlFunction_Ref_InvokeOnSelection === 'function')
            $scope.ApplicantSearch_CtrlFunction_Ref_InvokeOnSelection(item);

    };


    $(function () {
        $('#ApplicantSearchModalGeneral').on('shown.bs.modal', function () {
            $('#ApplicantFilterBy').focus();
        });
    });

    $(function () {
        $('#ApplicantSearchModalGeneral').on('hidden.bs.modal', function () {
            if (typeof $scope.ApplicantSearch_CallerName !== undefined && $scope.ApplicantSearch_CallerName !== null && $scope.ApplicantSearch_CallerName !== '')
                $('[name="' + $scope.ApplicantSearch_CallerName + '"]').focus();
        });
    });

}

///--------------------------SubDistributor Modal----------------------------------------//
function init_SDSearchModalGeneral($scope, $http) {
    $scope.SDSearchResult = [{ 'ID': null, 'Name': '', 'Address': '', 'ContactNo': '', 'ContactPerson': null }];
    $scope.SDSearch_QueryName = ''; $scope.SDSearch_FormID = 0; $scope.SDSearch_CallerName = ''; $scope.SDSearch_CtrlFunction_Ref_InvokeOnSelection0 = null;


    $scope.OpenSDSearchModalGeneral = function (QueryName, FormID, callerName, FunctionOnInvokeSelection) {
        if ($scope.SDFilterBy === undefined || $scope.SDFilterBy === null) {
            $scope.SDFilterBy = 'byName';
        }
        $scope.SDSearchResult.length = 0;
        $scope.SDFilterValue = '';

        $scope.SDSearch_QueryName = QueryName;
        $scope.SDSearch_FormID = FormID;
        $scope.SDSearch_CallerName = callerName;
        $scope.SDSearch_CtrlFunction_Ref_InvokeOnSelection0 = FunctionOnInvokeSelection;
        $('#SDSearchModalGeneral').modal('show');
    };


    $scope.General_SDSearch = function () {

        var successcallback = function (response) {
            $scope.SDSearchResult = response.data;
        };
        var errorcallback = function (error) {
        };
        $http({ method: "GET", url: "/Inventory/Orders/GetSubDistributorList?QueryName=" + $scope.SDSearch_QueryName + "&SDFilterBy=" + $scope.SDFilterBy + "&SDFilterValue=" + $scope.SDFilterValue + "&FormID=" + $scope.SDSearch_FormID, async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

    };

    $scope.General_SDSelectedAc = function (item) {
        if (typeof $scope.SDSearch_CtrlFunction_Ref_InvokeOnSelection0 === 'function')
            $scope.SDSearch_CtrlFunction_Ref_InvokeOnSelection0(item);
    };

    $(function () {
        $('#SDSearchModalGeneral').on('shown.bs.modal', function () {
            $('#SDFilterBy').focus();
        });
    });

    $(function () {
        $('#SDSearchModalGeneral').on('hidden.bs.modal', function () {
            if (typeof $scope.SDSearch_CallerName !== undefined && $scope.SDSearch_CallerName !== null && $scope.SDSearch_CallerName !== '')
                $('[name="' + $scope.SDSearch_CallerName + '"]').focus();
        });
    });

}

//-------------------------------------------------------------General Functions------------------------------------//
//these functions are called out side of init_Normal() functions as well like in http_interceptor_loading etc


// interceptor when HTTP request is processing
var http_interceptor_loading = function ($q) {

    return {
        request: function (config) {
            //capture when request started [request started...]  
            setOperationMessage('Please Wait while system is being processing', 0);
   

            if ((config.headers['NOSpinner']) === undefined || config.headers['NOSpinner'] === false) {
                loadingstart();
            }           
            
            if (config.headers['Privilege'] !== undefined) {
                if (config.headers['Privilege'] !== true) {
                    setOperationMessage('You Dont have sufficient privileges to perform this operation', 1);
                    config.aborted = true;
                    return;
                }
            }

            return config;

        },
        requestError: function (rejection) {
            // Contains the data about the error on the request and return the promise rejection. 
            console.log('Interceptor request error',rejection);
            //exceptionAjax(rejection);
            loadingstop();
            return $q.reject(rejection);
        },
        response: function (result) {
            //If some manipulation of result is required before assigning to scope.  [request completed]
            setOperationMessage('Sucessfully Processed', 0);           
            loadingstop();
            return result;
        },
        responseError: function (response) {
            //Check different response status and do the necessary actions 400, 401, 403,401, or 500 eror [response error started...] 
            //-if response.data is not null
            if (response.data) {
                alert(response.data);
            } else {
                alert('Not responding');
            }
            console.log('Interceptor response error', response);
            if (typeof response.headers !== undefined) {
                //exceptionAjax(response);
            }
            loadingstop();
            return $q.reject(response);
        }
    };
};

//pure java function to catch exception when ajax request
function exceptionAjax(error) {
    console.log('error', error);

    //when user is not log in 
    if (error.status === 999) { window.location.href = '/account/login?returnUrl=/Accounts/ChartOfAccountsType/Index'; }
    //when logged in but not authorized by role
    else if (error.status === 998) { window.location.href = '/account/Denied'; }
    //authorized by role but not have permission to use that action
    else if (error.status === 997) { setOperationMessage('You Dont have sufficient privileges to perform this operation', 1); }
    //any other exception raise backend                 
    else if (error.status === 996) { setOperationMessage(error.statusText, 1); }
    //any other exception raise like 404 not found etc
    else { setOperationMessage('Some thing went wrong', 3); }

}

//general function to set message
function setOperationMessage(msg, priority) {
    
    if (priority === 0) document.getElementById("operationalMessage").style.color = 'black';
    if (priority === 1) document.getElementById("operationalMessage").style.color = 'pink';
    if (priority === 2) document.getElementById("operationalMessage").style.color = 'lightpink';
    if (priority === 3) document.getElementById("operationalMessage").style.color = 'yellow';

    //document.getElementById("operationalMessage").innerHTML = msg;

    //$("#myToast").toast("dispose");
    //$("#myToast").toast({ delay: 2000 });
    //$("#myToast").toast("show");
}

function loadingstart() {
    document.getElementById("loadingOnMainPage").style.display = "block";
}

function loadingstop() {

    document.getElementById("loadingOnMainPage").style.display = "none";
}

function sortBy(key, propertytype) {
    if (propertytype === "string") {
        return function (a, b) {
            return a[key].toLowerCase().localeCompare(b[key].toLowerCase());
        };
    }
    else if (propertytype === "number") {
        return function (a, b) {
            if (a[key] < b[key]) { return -1; }
            if (a[key] > b[key]) { return 1; }
            return 0;
        };
    }

}











