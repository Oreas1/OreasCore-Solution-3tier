MainModule
    .controller("CompanyProfileIndexCtlr", function ($scope, $window, $http) {   
        ////////////data structure define//////////////////
        //for entrypanel model

        init_ViewSetup($scope, $http, '/Identity/Account/GetInitializedCompanyProfile');
        $scope.init_ViewSetup_Response = function (data) {
            if (data.find(o => o.Controller === 'CompanyProfileIndexCtlr') != undefined) {
                $scope.Privilege = data.find(o => o.Controller === 'CompanyProfileIndexCtlr').Privilege;
                $scope.Load();
            }
        };
        

        $scope.AspNetOreasCompanyProfile = {
            'ID': 0, 'LicenseBy': '', 'LicenseByAddress': '', 'LicenseByCellNo': '', 'LicenseByLogo': '',
            'LicenseToID': '', 'LicenseTo': '', 'LicenseToAddress': '', 'LicenseToContactNo': '', 'LicenseToEmail': '',
            'LicenseToEmailFooter': '', 'LicenseToEmailHostName': '', 'LicenseToEmailPortNo': null, 'LicenseToEmailPswd': null,
            'LicenseToLogo': '', 'LicenseToNTN': '', 'LicenseToSTN': '',
            'LicenseToUnofficialEmail': '', 'LicenseToUnofficialEmailHostName': '', 'LicenseToUnofficialEmailPortNo': null, 'LicenseToUnofficialEmailPswd': null,
            'LicenseToUnofficialEmailFooter' : ''
        };


        $scope.Load = function () {
            
            var successcallback = function (response) {
                $scope.AspNetOreasCompanyProfile = response.data;
            };
            var errorcallback = function (error) { };

            $http({ method: "GET", url: "/Identity/Account/CompanyProfileLoad", headers: { 'X-Requested-With': 'XMLHttpRequest'} }).then(successcallback, errorcallback);

        };

        

        $scope.PostRow = function () {

            var formData = new FormData();


            formData.append("LicenseByLogofile", $scope.LicenseByLogofile ?? null);
            formData.append("LicenseToLogofile", $scope.LicenseToLogofile ?? null);

            formData.append("AspNetOreasCompanyProfile.ID", $scope.AspNetOreasCompanyProfile.ID);
            formData.append("AspNetOreasCompanyProfile.LicenseBy", $scope.AspNetOreasCompanyProfile.LicenseBy);
            formData.append("AspNetOreasCompanyProfile.LicenseByAddress", $scope.AspNetOreasCompanyProfile.LicenseByAddress);
            formData.append("AspNetOreasCompanyProfile.LicenseByCellNo", $scope.AspNetOreasCompanyProfile.LicenseByCellNo);
            formData.append("AspNetOreasCompanyProfile.LicenseToID", $scope.AspNetOreasCompanyProfile.LicenseToID);
            formData.append("AspNetOreasCompanyProfile.LicenseTo", $scope.AspNetOreasCompanyProfile.LicenseTo);
            formData.append("AspNetOreasCompanyProfile.LicenseToAddress", $scope.AspNetOreasCompanyProfile.LicenseToAddress);
            formData.append("AspNetOreasCompanyProfile.LicenseToContactNo", $scope.AspNetOreasCompanyProfile.LicenseToContactNo);
            formData.append("AspNetOreasCompanyProfile.LicenseToEmail", $scope.AspNetOreasCompanyProfile.LicenseToEmail);
            formData.append("AspNetOreasCompanyProfile.LicenseToEmailFooter", $scope.AspNetOreasCompanyProfile.LicenseToEmailFooter);
            formData.append("AspNetOreasCompanyProfile.LicenseToEmailHostName", $scope.AspNetOreasCompanyProfile.LicenseToEmailHostName);
            formData.append("AspNetOreasCompanyProfile.LicenseToEmailPortNo", $scope.AspNetOreasCompanyProfile.LicenseToEmailPortNo);
            formData.append("AspNetOreasCompanyProfile.LicenseToEmailPswd", $scope.AspNetOreasCompanyProfile.LicenseToEmailPswd);

            formData.append("AspNetOreasCompanyProfile.LicenseToNTN", $scope.AspNetOreasCompanyProfile.LicenseToNTN);
            formData.append("AspNetOreasCompanyProfile.LicenseToSTN", $scope.AspNetOreasCompanyProfile.LicenseToSTN);

            formData.append("AspNetOreasCompanyProfile.LicenseToUnofficialEmail", $scope.AspNetOreasCompanyProfile.LicenseToUnofficialEmail);
            formData.append("AspNetOreasCompanyProfile.LicenseToUnofficialEmailFooter", $scope.AspNetOreasCompanyProfile.LicenseToUnofficialEmailFooter);
            formData.append("AspNetOreasCompanyProfile.LicenseToUnofficialEmailHostName", $scope.AspNetOreasCompanyProfile.LicenseToUnofficialEmailHostName);
            formData.append("AspNetOreasCompanyProfile.LicenseToUnofficialEmailPortNo", $scope.AspNetOreasCompanyProfile.LicenseToUnofficialEmailPortNo);
            formData.append("AspNetOreasCompanyProfile.LicenseToUnofficialEmailPswd", $scope.AspNetOreasCompanyProfile.LicenseToUnofficialEmailPswd);

            var successcallback = function (response) {
                if (response.data === 'OK') {
                    document.getElementById('LicenseByLogo').value = '';
                    document.getElementById('LicenseToLogo').value = '';

                    $scope.Load();
                    alert('Updated Successfully');
                }
            };
            var errorcallback = function (error) { };
            if (confirm("Are you sure! you want to update record") === true) {
                $http({
                    method: "POST", url: "/Identity/Account/CompanyProfilePost", params: { operation: 'Save Update' }, data: formData, headers: { 'Content-Type': undefined, 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }, transformRequest: angular.identity
                }).then(successcallback, errorcallback);
            }
        };

        

        $scope.LoadFileData = function (files,logofor) {
            if (logofor === 'LicenseByLogo')
                $scope.LicenseByLogofile = files[0];
            else if (logofor === 'LicenseToLogo')
                $scope.LicenseToLogofile = files[0];
        };


    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


