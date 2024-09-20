MainModule
    .controller("CredentialCtlr", function ($scope, $http) {
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


        $scope.CredentialsInfo = {
            'Id': null, 'LoginUserName': null, 'LoginEmailID': null, 'LoginPhoneNo': null
        };

        $scope.LoadCredentialsInfo = function () {
            var successcallback = function (response) {
                $scope.CredentialsInfo = response.data;
            };
            var errorcallback = function (error) {
            };
            $http({ method: "GET", url: "/DashBoard/LoadCredentialsInfo", async: true, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

        };
        $scope.LoadCredentialsInfo();

        $scope.Logout = function () {
            var successcallback = function (response) {
                if (response.data.redirectUrl != null || response.data.redirectUrl != '' || response.data.redirectUrl != undefined)
                    window.open(response.data.redirectUrl, '_self');
            };

            var errorcallback = function (error) { console.log(error); };
            $http({
                method: "POST", url: '/Identity/Account/Logout', async: true, params: {}, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
            }).then(successcallback, errorcallback);
        };

        $scope.PostChangedKey = function () {
            if ($scope.ChangedKey1 != $scope.ChangedKey2)
            {
                alert('Passwords do not match');
                return;

            }
            var successcallback = function (response) {
                if (response.data === 'OK') {
                    alert('Sucessfully change Password');
                    $scope.Logout();
                }
                else {
                    alert(response.data);
                }
            };

            var errorcallback = function (error) { alert('Some thing went wrong'); console.log(error); };
            $http({
                method: "POST", url: '/DashBoard/PostChangedKey', async: true, params: { ChangedKey: $scope.ChangedKey1, UserId: $scope.CredentialsInfo.Id }, headers: { 'X-Requested-With': 'XMLHttpRequest', 'RequestVerificationToken': $scope.antiForgeryToken }
            }).then(successcallback, errorcallback);
        };

        $scope.ChangedKeyEye = function (id)
        {         
            var x = document.getElementById("ChangedKey"+id);
            var y = document.getElementById("ChangedKey" + id + "Eye");

            if (x.type === "password") {
                x.type = "text"; 
                y.classList.remove('fa-eye-slash');
                y.classList.add('fa-eye');       
 
            } else {
                x.type = "password";
                y.classList.remove('fa-eye');
                y.classList.add('fa-eye-slash');      
            }
        }

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


