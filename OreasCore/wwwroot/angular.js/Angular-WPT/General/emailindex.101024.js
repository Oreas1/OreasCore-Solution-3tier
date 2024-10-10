MainModule
    .controller("EmailIndexCtlr", function ($scope, $window, $http) {   

        init_ViewSetup($scope, $http, '/WPT/General/GetInitializedEmail');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'EmailIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'EmailIndexCtlr').Privilege;
                if (data.find(o => o.Controller === 'EmailIndexCtlr').Otherdata === null) {                    
                    $scope.SectionList = [];
                    $scope.DesignationList = [];
                }
                else {

                    $scope.SectionList = data.find(o => o.Controller === 'EmailIndexCtlr').Otherdata.SectionList;
                    $scope.DesignationList = data.find(o => o.Controller === 'EmailIndexCtlr').Otherdata.DesignationList;
                }                
            }
        };


        $scope.EmailTo = { 'Index': null, 'EmailAddress': null, 'EmailType': 'To' };
        $scope.MailBody = ''; $scope.Subject = '';
        //for list model which will be coming as as data in pageddata
        $scope.EmailTos = [];
        $scope.WithFooter = true;
        $scope.AsUnknown = false;
      

        $scope.clear = function () {
            $scope.EmailTo = { 'Index': null, 'EmailAddress': null, 'EmailType': 'To' };
        };

        $scope.ClearList = function () {
            $scope.EmailTos = [];
        };
        $scope.DeleteRow = function (id) {
            var index = $scope.EmailTos.findIndex(x => x.Index === id);
            $scope.EmailTos.splice(index, 1);
        };

        $scope.WithFooterClick = function () {
            if ($scope.WithFooter === true)
                $scope.WithFooter = false;
            else
                $scope.WithFooter = true;
        };

        $scope.AsUnknownClick = function () {
            if ($scope.AsUnknown === true)
                $scope.AsUnknown = false;
            else
                $scope.AsUnknown = true;
        };

        $scope.PostRow = function (op) {            
            if ($scope.EmailTo.EmailAddress.length > 3 && $scope.EmailTos.findIndex(x => x.EmailAddress === $scope.EmailTo.EmailAddress) === -1) {
                if ($scope.EmailTo.Index === null) { $scope.EmailTo.Index = $scope.EmailTos.length; };
                $scope.EmailTos.push($scope.EmailTo);
                $scope.clear();
            };
        };


        $scope.AddGroup = function () {

            var successcallback = function (response) {
                angular.forEach(response.data, function (value, key) {
                    if (value.Email.length > 3 && $scope.EmailTos.findIndex(x => x.EmailAddress === value.Email) === -1) {
                        $scope.EmailTos.push({ 'Index': $scope.EmailTos.length, 'EmailAddress': value.Email, 'EmailType': $scope.EmailTo.EmailType });
                    };
                });
            };

            var errorcallback = function (error) {       };
            $http({ method: "GET", url: "/WPT/General/GetGroupEmailList?DesignationID=" + $scope.FK_tbl_WPT_Designation_ID + "&DepartmentID=" + $scope.FK_tbl_WPT_Department_ID, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

        };


        $scope.SendMail = function (mailBody) {
            $scope.MailBody = mailBody;
            if (typeof $scope.Subject === 'undefined' || $scope.Subject === '' || typeof $scope.MailBody === 'undefined' || $scope.MailBody === ''  || $scope.EmailTos.length <= 0) {
                alert("Provide All information correctly");
                return;
            }

            $scope.VM_Email = { 'Subject': $scope.Subject, 'MailBody': $scope.MailBody, 'WithFooter': $scope.WithFooter, 'VM_EmailAddresses': $scope.EmailTos }
            var successcallback = function (response) {
                if (response.data === 'Failed') {
                    alert('Failed');
                }                    
                else {
                    alert("Successfully");
                    location.reload(true);
                }
                
            };
            var errorcallback = function (error) { console.log(error); };
            if (confirm("Are you sure! you want to Send Mail") === true) {
                $http({
                    method: "POST", url: '/WPT/General/SendMail', async: true, params: { operation :'Save New', Unknown: $scope.AsUnknown }, data: $scope.VM_Email, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
                }).then(successcallback, errorcallback);
            }
        };

        (function () {
            const editorInstance = new FroalaEditor('#edit', {
                enter: FroalaEditor.ENTER_P,
                placeholderText: null,
                events: {
                    initialized: function () {
                        const editor = this
                        this.el.closest('form').addEventListener('submit', function (e) {   
                            console.log(editor.$oel.val());
                            
                            e.preventDefault();
                            
                            $scope.SendMail(editor.$oel.val());
                            
                        })
                    }
                }
            })
        })();

    }).config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


