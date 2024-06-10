MainModule
    .controller("MachineOperationsCtlr", function ($scope, $window, $http, $rootScope) {
        var pathArray = window.location.pathname.split('/');

        $scope.MasterID = pathArray[pathArray.length - 1];
        $scope.SelectedEmp = {
            'ID': 0, 'EmployeeNo': null, 'EmployeeName': null, 'ATEnrollmentNo_Default': '', 'DepartmentName': '', 'Designation': '',
            'Photo160X210': '', 'FaceTemplate': false, 'FingerCount': 0, 'CardNumber': '', 'Password': '', 'Privilege': null, 'Enabled': true
        };

        init_EmployeeSearchModalGeneral($scope, $http);
        $scope.EmployeeSearch_CtrlFunction_Ref_InvokeOnSelection = function (item) {
            if (item.ID > 0) {
                $scope.SelectedEmp.ID = item.ID;
                $scope.SelectedEmp.EmployeeNo = item.EmployeeNo;
                $scope.SelectedEmp.EmployeeName = '[' + item.ATEnrollmentNo_Default + '] ' + item.EmployeeName;
                $scope.SelectedEmp.ATEnrollmentNo_Default = item.ATEnrollmentNo_Default;
                $scope.SelectedEmp.DepartmentName = item.DepartmentName;
                $scope.SelectedEmp.Designation = item.Designation;
                $scope.GetEmployeePFF();
            }
            else {
                $scope.SelectedEmp = {
                    'ID': 0, 'EmployeeNo': null, 'EmployeeName': null, 'ATEnrollmentNo_Default': '', 'DepartmentName': '', 'Designation': '',
                    'Photo160X210': '', 'FaceTemplate': false, 'FingerCount': 0, 'CardNumber': '', 'Password': '', 'Privilege': 0, 'Enabled': true
                };
            }
            
        };
        $scope.GetEmployeePFF = function () {
            var successcallback = function (response) {
                if (response.data != null) {
                    $scope.SelectedEmp.Photo160X210 = response.data.Photo160X210;
                    $scope.SelectedEmp.FaceTemplate = response.data.HasFaceTemplate;
                    $scope.SelectedEmp.FingerCount = response.data.FingerCount;
                    $scope.SelectedEmp.CardNumber = response.data.CardNumber;
                    $scope.SelectedEmp.Password = response.data.Password;
                    $scope.SelectedEmp.Privilege = response.data.Privilege;
                    $scope.SelectedEmp.Enabled = response.data.Enabled;
                }
                

            };
            var errorcallback = function (error) { console.log(error); };
            $http({ method: "GET", url: "/WPT/Machine/GetEmployeePFF?EmpID=" + $scope.SelectedEmp.ID, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(successcallback, errorcallback);

        }

        $scope.OpenProgressModal = function (OperationName)
        {
            $scope.OperationName = OperationName;
            $scope.BtnOptText = 'Stop Operation';

            if (connection.state === signalR.HubConnectionState.Connected) {

                connection.invoke("StartOperation", parseInt($scope.MasterID), OperationName, parseInt($scope.SelectedEmp.ID))
                    .then(function () {
                        $rootScope.$apply(function () { });
                    })
                    .catch(function (err) {
                        $rootScope.$apply(function () {
                            $scope.ServerAcknowledgment = 'Some thing went wrong while processing the Operation: ' + OperationName;
                        });
                        return console.error(err.toString());
                    })
                    .finally(function () {
                        $rootScope.$apply(function () {
                            $scope.BtnOptText = 'Close';
                            if ($scope.IsAborted === true) {
                                $scope.ServerAcknowledgment = 'Process Aborted for Operation : ' + OperationName;
                                $scope.IsAborted = false;
                            }
                        });
                    });
            }
            else if (connection.state === signalR.HubConnectionState.Disconnected) {
                alert('Connection has been Disconnected from server');
            }

            $('#ProgressModal').modal('show');
        };
        $scope.CloseProgressModal = function () {

            if ($scope.BtnOptText === 'Stop Operation' && connection.state === signalR.HubConnectionState.Connected) {

                $scope.ServerAcknowledgment = 'Process Aborted is called';

                connection.invoke("CancelOperation")
                    .then(function () {
                        $rootScope.$apply(function () {
                            $scope.ServerAcknowledgment = 'Process Aborted for Operation: ' + $scope.OperationName;
                        });
                    })
                    .catch(function (err) {
                        return console.error(err.toString());
                    })
                    .finally(function () {
                        $rootScope.$apply(function () {
                            $scope.ServerAcknowledgment = 'Process Aborted for Operation: ' + $scope.OperationName;
                            $scope.BtnOptText = 'Close'; 
                        });
                    });
            }
            else if ($scope.BtnOptText === 'Stop Operation' && connection.state === signalR.HubConnectionState.Disconnected) {
                alert('Connection has been Disconnected from server');
            }
            $scope.BtnOptText = 'Close'; 
            $('#ProgressModal').modal('hide');
        };

        //--------xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx--------//
        //---------------------------------------SignalR------------------------------------------------------------//
        "use strict";

        var connection = new signalR.HubConnectionBuilder().withUrl("/machineOperationHub").build();

        connection.on("ClientAcknowledgment", function (message) {
            $rootScope.$apply(function () {
                $scope.ServerAcknowledgment = message;
               console.log(message);
            });
        });

       
       //---------------------------------------------------------------SignalR Hub events---------------//
        $scope.StartHub = function () {
            connection.start().then(function () {
                $rootScope.$apply(function () {
                    $scope.ServerConnectionStatus = 'Connected';
                });

            }).catch(function (err) {
                return console.error(err.toString());
            });
        };
        $scope.StopHub = function () {
            connection.stop().then(function () {
                $rootScope.$apply(function () {
                    $scope.ServerConnectionStatus = 'Disconnected';
                });

            }).catch(function (err) {
                return console.error(err.toString());
            });
        };
        

        connection.onclose(() => {           
            $rootScope.$apply(function () {
                $scope.ServerConnectionStatus = 'Disconnected';
                $scope.BtnOptText = 'Start Operation';
            });            
        });

        $scope.StartHub();
        //--------xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx--------//
        //---------------------------------------Angular------------------------------------------------------------//
       
        $scope.BtnOptText = 'Start Operation';
        $scope.ServerConnectionStatus = 'Disconnected';
        $scope.IsAborted = false;
        $scope.InProcess = false;

       

        $scope.OperationCalling = function () {
            
            if ($scope.BtnOptText === 'Start Operation' && connection.state === signalR.HubConnectionState.Connected) {

                $scope.BtnOptText = 'Stop Operation';
                $scope.InProcess = true;
                
                connection.invoke("StartOperation", parseInt($scope.MasterID), $scope.MachineOpt, parseInt($scope.SelectedEmp.ID))
                    .then(function () {
                        $rootScope.$apply(function () {
                                                        
                        });
                    })
                    .catch(function (err) {
                        $rootScope.$apply(function () {
                            $scope.ServerAcknowledgment = 'Some thing went wrong while processing the Operation: ' + $scope.MachineOpt;
                        });
                        return console.error(err.toString());
                    })
                    .finally(function () {                        
                        $rootScope.$apply(function () {
                            $scope.BtnOptText = 'Start Operation';
                            $scope.InProcess = false;
                            if ($scope.IsAborted === true) {
                                $scope.ServerAcknowledgment = 'Process Aborted for Operation : ' + $scope.MachineOpt;
                                $scope.IsAborted = false;
                            }
                        });
                    });
            }
            else if ($scope.BtnOptText === 'Stop Operation' && connection.state === signalR.HubConnectionState.Connected) {
                
                $scope.BtnOptText = 'Start Operation';
                
                connection.invoke("CancelOperation")
                    .then(function () {
                        $rootScope.$apply(function () {
                            $scope.ServerAcknowledgment = 'Process Aborted for Operation: ' + $scope.MachineOpt;
                            $scope.IsAborted = true;
                            $scope.InProcess = false;
                        });
                    })
                    .catch(function (err) {
                        return console.error(err.toString());
                    })
                    .finally(function () {
                        $rootScope.$apply(function () {
                            $scope.ServerAcknowledgment = 'Process Aborted for Operation: ' + $scope.MachineOpt;                            
                        });
                    });
            }
            else if (connection.state === signalR.HubConnectionState.Disconnected) {
                alert('Connection has been Disconnected from server');
            }

        };
        $scope.RequestToConnectOrDisconnect = function () {
            if (connection.state === signalR.HubConnectionState.Disconnected)
                $scope.StartHub();
            else if (connection.state === signalR.HubConnectionState.Connected)
                $scope.StopHub();
        };
        $scope.GetOperatorDetail = function () {
            if (connection.state === signalR.HubConnectionState.Connected) {
                connection.invoke("GetOperatorDetail", parseInt($scope.MasterID))
                    .then(function (response) {
                        $rootScope.$apply(function () {
                            $scope.OperatorDetail = response;
                        });
                    })
                    .catch(function (err) {
                        $rootScope.$apply(function () {

                        });
                        return console.error(err.toString());
                    })
                    .finally(function () {
                        $rootScope.$apply(function () {
                        });
                    });
            }
            else {
                alert('Connection has been Disconnected from server');
            }
            
        };

    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(http_interceptor_loading);
    });


    