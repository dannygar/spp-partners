/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.


(function () {
    'use strict';

    angular.module('app')
        .config(setupRouter);

    setupRouter.$inject = ['$stateProvider', '$urlRouterProvider', '$httpProvider', 'adalAuthenticationServiceProvider', 'config'];

    function setupRouter($stateProvider, $urlRouterProvider, $httpProvider, adalProvider, config) {
        $stateProvider
            .state('login', {
                url: '/login',
                templateUrl: '/Client/Views/login.html',
                controller: 'LoginCtrl as vm'
            })
            .state('error', {
                url: '/error',
                templateUrl: '/Client/Views/error.html'
            })
            .state('app', {
                url: '/app',
                abstract: true,
                resolve: {
                    currentUser: getCurrentUser,
                    team: getTeamPlayers
                },
                templateUrl: '/Client/Views/master.html',
                controller: 'MainCtrl as main',
                requireADLogin: true
            })
            .state('app.users', {
                url: '/users',
                views: {
                    nestedHolder: {
                        resolve: {
                            team: getTeamPlayers,
                            options: getUserOptions,
                            teams: getAllTeams
                        },
                        templateUrl: '/Client/Views/users.html',
                        controller: 'UsersCtrl as vm'
                    }
                },
                requireADLogin: true
            })
            .state('app.user-new', {
                url: '/users/new',
                views: {
                    nestedHolder: {
                        resolve: {
                            options: getUserOptions,
                            team: getTeamPlayers,
                            playerPositions: getPlayerPositions,
                            playerSubpositions: getPlayerSubpositions
                        },
                        templateUrl: '/Client/Views/user-new.html',
                        controller: 'UserNewCtrl as vm'
                    }
                },
                requireADLogin: true
            })
            .state('app.user-detail', {
                url: '/users/:id',
                params: {user: null},
                views: {
                    nestedHolder: {
                        resolve: {
                            user: getUser,
                            team: getTeamPlayers,
                            options: getUserOptions,
                            playerPositions: getPlayerPositions,
                            playerSubpositions: getPlayerSubpositions
                        },
                        templateUrl: '/Client/Views/user-detail.html',
                        controller: 'UserDetailCtrl as vm'
                    }
                },
                requireADLogin: true
            })
            .state('app.sessions-day', {
                url: '/sessions/day',
                params: { date: null },
                views: {
                    nestedHolder: {
                        resolve: {
                            sessions: getDaySessions,
                            date: getDate,
                            wellness: getDayWellness
                        },
                        templateUrl: '/Client/Views/sessions-day.html',
                        controller: 'SessionsDayCtrl as vm'
                    }
                },
                requireADLogin: true
            })
            .state('app.sessions-week', {
                url: '/sessions/week',
                params: { date: null },
                views: {
                    nestedHolder: {
                        resolve: {
                            sessions: getWeekSessions,
                            selectedPeriod: getSelectedPeriod,
                            wellness: getWeekWellness
                        },
                        templateUrl: '/Client/Views/sessions-week.html',
                        controller: 'SessionsWeekCtrl as vm'
                    }
                },
                requireADLogin: true
            })
            .state('app.session-detail', {
                url: '/sessions/:id',
                params: { session: null },
                views: {
                    nestedHolder: {
                        resolve: {
                            session: getSession,
                            options: getSessionOptions,
                            sessionType: getSessionTypes,
                            drills: getDrills,
                            practices: getPractices,
                            team: getTeamPlayers,
                            locations: getLocations
                        },
                        templateUrl: '/Client/Views/session-detail.html',
                        controller: 'SessionDetailCtrl as vm'
                    }
                },
                requireADLogin: true
            })
            .state('app.settings', {
                url: '/settings',
                views: {
                    nestedHolder: {
                        resolve: {
                            personalImages: getPersonalImages,
                            sessionTypes: getSessionTypes,
                            sessionSubtypes: getSessionSubtypes,
                            sessionDayTypes: getSessionDayTypes,
                            sessionMesocycles: getSessionMesocycles,
                            sessionMicrocycles: getSessionMicrocycles,
                            playerPositions: getPlayerPositions,
                            playerSubpositions: getPlayerSubpositions,
                            team: getTeamPlayers,
                            teams: getAllTeams
                        },
                        templateUrl: '/Client/Views/settings.html',
                        controller: 'SettingsCtrl as vm'
                    }
                },
                requireADLogin: true
            })
            .state('app.about', {
                url: '/about',
                views: {
                    nestedHolder: {
                        templateUrl: '/Client/Views/about.html'
                    }
                }
            });

        $urlRouterProvider.otherwise('/login');

        adalProvider.init(
        {
            instance: config.instance,
            tenant: config.tenant,
            clientId: config.client_id,
            extraQueryParameter: 'nux=1',
            cacheLocation: 'localStorage', 
            redirectUri: window.location.protocol + "//" + window.location.host
        },
        $httpProvider
        );

        function getCurrentUser($state, $timeout, AuthService, ResourceService) {
            if(JSON.parse(localStorage.getItem("currentUser"))){
                return JSON.parse(localStorage.getItem("currentUser"));
            }
            else {
                $state.go('login');
            }
        }

        function getAllTeams(ResourceService) {
            return ResourceService.getTeams();
        }
        function getTeamPlayers() {
            return JSON.parse(localStorage.getItem("currentTeam"));
        }
        function getLocations(ResourceService){
            return ResourceService.getLocations();
        }

        function getUserOptions() {
            return JSON.parse('{"Positions":[{"SubPositionIds":[1,2,3],"Id":1,"Name":"Offensive Line","Description":null},{"SubPositionIds":[4,5],"Id":2,"Name":"Tight End","Description":null},{"SubPositionIds":[6,7],"Id":3,"Name":"Running Back","Description":null},{"SubPositionIds":[8,9],"Id":4,"Name":"Defensive Line","Description":null},{"SubPositionIds":[10,11],"Id":5,"Name":"Linebackers","Description":null},{"SubPositionIds":[12,13],"Id":6,"Name":"Defensive Back","Description":null},{"SubPositionIds":[14,15,16],"Id":7,"Name":"Specialists","Description":null},{"SubPositionIds":[],"Id":8,"Name":"Wide Receiver","Description":null},{"SubPositionIds":[],"Id":9,"Name":"Quarterback","Description":null}],"SubPositions":[{"Id":1,"Name":"Offensive Tackle","Description":null},{"Id":2,"Name":"Offensive Guard","Description":null},{"Id":3,"Name":"Offensive Center","Description":null},{"Id":4,"Name":"Running Tight End","Description":null},{"Id":5,"Name":"Blocking Tight End","Description":null},{"Id":6,"Name":"Running Back","Description":null},{"Id":7,"Name":"Full Back","Description":null},{"Id":8,"Name":"Defensive Tackle","Description":null},{"Id":9,"Name":"Defensive End","Description":null},{"Id":10,"Name":"Inside Linebacker","Description":null},{"Id":11,"Name":"Outside Linebacker","Description":null},{"Id":12,"Name":"Corner Back","Description":null},{"Id":13,"Name":"Safety","Description":null},{"Id":14,"Name":"Kicker","Description":null},{"Id":15,"Name":"Punter","Description":null},{"Id":16,"Name":"Long Snapper","Description":null}],"Squads":[{"PlayerIds":[4,5],"Id":1,"Name":"Offensive","Description":""},{"PlayerIds":[2,6,7],"Id":2,"Name":"Defensive","Description":null}],"Roles":[{"Id":1,"Name":"Coach"},{"Id":2,"Name":"Player"}, {"Id":3, "Name":"Administrator"}],"Genders":["Male","Female"]}');
        }

        function getUser(ResourceService, $stateParams, $state) {
            if (!parseInt($stateParams.id)) {
                $state.go('app.users', null, {reload: true});
            }

            return $stateParams.user || ResourceService.getUserInfo($stateParams.id);
        }

        function getPersonalImages(ResourceService) {
            return ResourceService.getAllPersonalImages();
        }

        function getSessionTypes(ResourceService) {
            return ResourceService.getAllSessionTypes();
            //return JSON.parse('[{"SubTypeIds":[1,2,3],"Id":1,"Name":"Strength training","Description":null},{"SubTypeIds":[4,5,6,7],"Id":2,"Name":"Conditioning","Description":null},{"SubTypeIds":[],"Id":3,"Name":"Practice","Description":null},{"SubTypeIds":[],"Id":4,"Name":"Game","Description":null},{"SubTypeIds":[],"Id":5,"Name":"Recovery","Description":null}]');
        }

        function getSessionSubtypes(ResourceService) {
            return JSON.parse('[{"Id":1,"Name":"Total body","Description":null},{"Id":2,"Name":"Upper body","Description":null},{"Id":3,"Name":"Lower body","Description":null},{"Id":4,"Name":"Running","Description":null},{"Id":5,"Name":"Cardio-Vascular","Description":null},{"Id":6,"Name":"Agility","Description":null},{"Id":7,"Name":"Flexibility","Description":null}]');
        }

        function getSessionDayTypes(ResourceService) {
            return JSON.parse('[{"Id":1,"Name":"Competition Wed","Description":null},{"Id":2,"Name":"Turnover Thurs","Description":null},{"Id":3,"Name":"Bonus","Description":null},{"Id":4,"Name":"No Repeat Fri","Description":null}]');
        }

        function getSessionMesocycles(ResourceService) {
            return ResourceService.getAllSessionMesocycles();
        }

        function getSessionMicrocycles(ResourceService) {
            return ResourceService.getAllSessionMicrocycles();
        }

        function getPlayerPositions(ResourceService) {
            return ResourceService.getAllPositions();
        }

        function getPlayerSubpositions(ResourceService) {
            return ResourceService.getAllSubpositions();
        }

        function getPractices(ResourceService) {
            return ResourceService.getAllPractices();
        }        

        function getDaySessions(ResourceService, UtilsService, $stateParams) {
            var date = $stateParams.date || UtilsService.getTodayDateAsString();
            return ResourceService.getAllSessions(date);
        }

        function getDate($stateParams) {
            return $stateParams.date || new Date();
        }

        function getDayWellness(ResourceService, $stateParams) {
            var dateFrom = $stateParams.date ? new Date($stateParams.date) : new Date();
            dateFrom.toMidnight();

            var dateTo = new Date(dateFrom).nextDate();

            //return ResourceService.getWellnessForPeriod(dateFrom, dateTo);
            //Temporarily returns JSON object 
            return JSON.parse('[{"Id":362,"SessionId":null,"SessionType":null,"StartDateTime":"2017-03-15T00:00:00Z","DurationMinutes":null,"AlertDateTime":"2017-03-15T09:00:00Z","RandomizeQuestionOrder":false,"SequenceNumberPerDay":0,"Type":null,"TypeId":1,"Questions":[{"Id":1,"Text":"Energy","ValueCaptions":{"Full":["Exhausted","Somewhat Energetic","High Energy"],"Short":[]},"Enabled":true,"Answers":null},{"Id":2,"Text":"Sleep","ValueCaptions":{"Full":["Poor","Restless","Outstanding"],"Short":[]},"Enabled":true,"Answers":null},{"Id":3,"Text":"Soreness","ValueCaptions":{"Full":["Extremely Sore","Moderately Sore","No Soreness"],"Short":[]},"Enabled":true,"Answers":null},{"Id":4,"Text":"Stress","ValueCaptions":{"Full":["High Stress","Moderate Stress","Stress Free"],"Short":[]},"Enabled":true,"Answers":null}]}]');
        }

        function getWeekSessions(ResourceService, UtilsService, $stateParams) {
            var period = UtilsService.getWeekForDate($stateParams.date || new Date());
             return ResourceService.getAllSessions(
               period[0].date,
             period[period.length - 1].date.nextDate()
            );
        }

        function getSelectedPeriod($stateParams, UtilsService) {
            return UtilsService.getWeekForDate($stateParams.date || new Date());
        }

        function getWeekWellness(ResourceService, $stateParams, UtilsService) {
            var period = UtilsService.getWeekForDate($stateParams.date || new Date());

            var startDate = period[0].date;
            var endDate = period[period.length - 1].date.nextDate();

            //return ResourceService.getWellnessForPeriod(startDate, endDate);
            // TODO temporarily returning JSON object
            return JSON.parse('[{"Id":360,"SessionId":null,"SessionType":null,"StartDateTime":"2017-03-13T00:00:00Z","DurationMinutes":null,"AlertDateTime":"2017-03-13T09:00:00Z","RandomizeQuestionOrder":false,"SequenceNumberPerDay":0,"Type":null,"TypeId":1,"Questions":[{"Id":1,"Text":"Energy","ValueCaptions":{"Full":["Exhausted","Somewhat Energetic","High Energy"],"Short":[]},"Enabled":true,"Answers":null},{"Id":2,"Text":"Sleep","ValueCaptions":{"Full":["Poor","Restless","Outstanding"],"Short":[]},"Enabled":true,"Answers":null},{"Id":3,"Text":"Soreness","ValueCaptions":{"Full":["Extremely Sore","Moderately Sore","No Soreness"],"Short":[]},"Enabled":true,"Answers":null},{"Id":4,"Text":"Stress","ValueCaptions":{"Full":["High Stress","Moderate Stress","Stress Free"],"Short":[]},"Enabled":true,"Answers":null}]},{"Id":361,"SessionId":null,"SessionType":null,"StartDateTime":"2017-03-14T00:00:00Z","DurationMinutes":null,"AlertDateTime":"2017-03-14T09:00:00Z","RandomizeQuestionOrder":false,"SequenceNumberPerDay":0,"Type":null,"TypeId":1,"Questions":[{"Id":1,"Text":"Energy","ValueCaptions":{"Full":["Exhausted","Somewhat Energetic","High Energy"],"Short":[]},"Enabled":true,"Answers":null},{"Id":2,"Text":"Sleep","ValueCaptions":{"Full":["Poor","Restless","Outstanding"],"Short":[]},"Enabled":true,"Answers":null},{"Id":3,"Text":"Soreness","ValueCaptions":{"Full":["Extremely Sore","Moderately Sore","No Soreness"],"Short":[]},"Enabled":true,"Answers":null},{"Id":4,"Text":"Stress","ValueCaptions":{"Full":["High Stress","Moderate Stress","Stress Free"],"Short":[]},"Enabled":true,"Answers":null}]},{"Id":362,"SessionId":null,"SessionType":null,"StartDateTime":"2017-03-15T00:00:00Z","DurationMinutes":null,"AlertDateTime":"2017-03-15T09:00:00Z","RandomizeQuestionOrder":false,"SequenceNumberPerDay":0,"Type":null,"TypeId":1,"Questions":[{"Id":1,"Text":"Energy","ValueCaptions":{"Full":["Exhausted","Somewhat Energetic","High Energy"],"Short":[]},"Enabled":true,"Answers":null},{"Id":2,"Text":"Sleep","ValueCaptions":{"Full":["Poor","Restless","Outstanding"],"Short":[]},"Enabled":true,"Answers":null},{"Id":3,"Text":"Soreness","ValueCaptions":{"Full":["Extremely Sore","Moderately Sore","No Soreness"],"Short":[]},"Enabled":true,"Answers":null},{"Id":4,"Text":"Stress","ValueCaptions":{"Full":["High Stress","Moderate Stress","Stress Free"],"Short":[]},"Enabled":true,"Answers":null}]},{"Id":363,"SessionId":null,"SessionType":null,"StartDateTime":"2017-03-16T00:00:00Z","DurationMinutes":null,"AlertDateTime":"2017-03-16T09:00:00Z","RandomizeQuestionOrder":false,"SequenceNumberPerDay":0,"Type":null,"TypeId":1,"Questions":[{"Id":1,"Text":"Energy","ValueCaptions":{"Full":["Exhausted","Somewhat Energetic","High Energy"],"Short":[]},"Enabled":true,"Answers":null},{"Id":2,"Text":"Sleep","ValueCaptions":{"Full":["Poor","Restless","Outstanding"],"Short":[]},"Enabled":true,"Answers":null},{"Id":3,"Text":"Soreness","ValueCaptions":{"Full":["Extremely Sore","Moderately Sore","No Soreness"],"Short":[]},"Enabled":true,"Answers":null},{"Id":4,"Text":"Stress","ValueCaptions":{"Full":["High Stress","Moderate Stress","Stress Free"],"Short":[]},"Enabled":true,"Answers":null}]},{"Id":364,"SessionId":null,"SessionType":null,"StartDateTime":"2017-03-17T00:00:00Z","DurationMinutes":null,"AlertDateTime":"2017-03-17T09:00:00Z","RandomizeQuestionOrder":false,"SequenceNumberPerDay":0,"Type":null,"TypeId":1,"Questions":[{"Id":1,"Text":"Energy","ValueCaptions":{"Full":["Exhausted","Somewhat Energetic","High Energy"],"Short":[]},"Enabled":true,"Answers":null},{"Id":2,"Text":"Sleep","ValueCaptions":{"Full":["Poor","Restless","Outstanding"],"Short":[]},"Enabled":true,"Answers":null},{"Id":3,"Text":"Soreness","ValueCaptions":{"Full":["Extremely Sore","Moderately Sore","No Soreness"],"Short":[]},"Enabled":true,"Answers":null},{"Id":4,"Text":"Stress","ValueCaptions":{"Full":["High Stress","Moderate Stress","Stress Free"],"Short":[]},"Enabled":true,"Answers":null}]},{"Id":365,"SessionId":null,"SessionType":null,"StartDateTime":"2017-03-18T00:00:00Z","DurationMinutes":null,"AlertDateTime":"2017-03-18T09:00:00Z","RandomizeQuestionOrder":false,"SequenceNumberPerDay":0,"Type":null,"TypeId":1,"Questions":[{"Id":1,"Text":"Energy","ValueCaptions":{"Full":["Exhausted","Somewhat Energetic","High Energy"],"Short":[]},"Enabled":true,"Answers":null},{"Id":2,"Text":"Sleep","ValueCaptions":{"Full":["Poor","Restless","Outstanding"],"Short":[]},"Enabled":true,"Answers":null},{"Id":3,"Text":"Soreness","ValueCaptions":{"Full":["Extremely Sore","Moderately Sore","No Soreness"],"Short":[]},"Enabled":true,"Answers":null},{"Id":4,"Text":"Stress","ValueCaptions":{"Full":["High Stress","Moderate Stress","Stress Free"],"Short":[]},"Enabled":true,"Answers":null}]},{"Id":366,"SessionId":null,"SessionType":null,"StartDateTime":"2017-03-19T00:00:00Z","DurationMinutes":null,"AlertDateTime":"2017-03-19T09:00:00Z","RandomizeQuestionOrder":false,"SequenceNumberPerDay":0,"Type":null,"TypeId":1,"Questions":[{"Id":1,"Text":"Energy","ValueCaptions":{"Full":["Exhausted","Somewhat Energetic","High Energy"],"Short":[]},"Enabled":true,"Answers":null},{"Id":2,"Text":"Sleep","ValueCaptions":{"Full":["Poor","Restless","Outstanding"],"Short":[]},"Enabled":true,"Answers":null},{"Id":3,"Text":"Soreness","ValueCaptions":{"Full":["Extremely Sore","Moderately Sore","No Soreness"],"Short":[]},"Enabled":true,"Answers":null},{"Id":4,"Text":"Stress","ValueCaptions":{"Full":["High Stress","Moderate Stress","Stress Free"],"Short":[]},"Enabled":true,"Answers":null}]}]');
        }

        function getSession(ResourceService, $stateParams, $state, config) {
            if ($stateParams.id === config.idForNewItems) {
                return $stateParams.session;
            }

            if (!parseInt($stateParams.id)) {
                $state.go('app.sessions-day', null, { reload: true });
            }

            return $stateParams.session || ResourceService.getSession($stateParams.id);
        }

        function getSessionOptions(ResourceService) {
            //return JSON.parse(ResourceService.getAllPractices());
            return JSON.parse('{"SessionTypes":[{"SubTypeIds":[1,2,3],"Id":1,"Name":"Metabolic","Description":null},{"SubTypeIds":[4,5,6,7],"Id":2,"Name":"Speed","Description":null},{"SubTypeIds":[],"Id":3,"Name":"Recovery","Description":null},{"SubTypeIds":[],"Id":4,"Name":"Gym","Description":null}],"SessionSubTypes":[{"Id":1,"Name":"Total body","Description":null},{"Id":2,"Name":"Upper body","Description":null},{"Id":3,"Name":"Lower body","Description":null},{"Id":4,"Name":"Running","Description":null},{"Id":5,"Name":"Cardio-Vascular","Description":null},{"Id":6,"Name":"Agility","Description":null},{"Id":7,"Name":"Flexibility","Description":null}],"SessionDayTypes":[{"Id":1,"Name":"Competition Wed","Description":null},{"Id":2,"Name":"Turnover Thurs","Description":null},{"Id":3,"Name":"Bonus","Description":null},{"Id":4,"Name":"No Repeat Fri","Description":null}],"SessionMesocycles":[{"Id":1,"Name":"Training Camp","Description":null},{"Id":2,"Name":"Quarter 1","Description":null},{"Id":3,"Name":"Quarter 2","Description":null}],"SessionMicrocycles":[{"Id":1,"Name":"Training camp week 1","Description":null},{"Id":2,"Name":"Training camp week 2","Description":null},{"Id":3,"Name":"Training camp week 3","Description":null}],"Squads":[{"PlayerIds":[4,5],"Id":1,"Name":"Offensive","Description":""},{"PlayerIds":[2,6,7],"Id":2,"Name":"Defensive","Description":null}],"Players":[{"Id":2,"Name":"Cristiano Ronaldo","LastName":"dos Santos Aveiro","DateOfBirth":"2016-05-26T13:39:05.168Z","AadEmail":"Cristiano.Aveiro@rmteamperformancetest.onmicrosoft.com","Email":"player@rm.com","Position":{"SubPositionIds":null,"Id":8,"Name":"Wide Receiver","Description":null},"SubPosition":null,"ShirtNumber":10,"Height":null,"Weight":null,"Gender":"Male","PathToPhoto":"https://rmteamphotostest.blob.core.windows.net/public/users/photo/cropped/c6c834a151d14babab5ede483c3fb2b7.png","GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":4,"Name":"Navas","LastName":"Keylor","DateOfBirth":"2016-06-01T14:41:17.978Z","AadEmail":"Navas.Keylor@rmteamperformancetest.onmicrosoft.com","Email":"Keylor@rmteamperformancetest.onmicrosoft.com","Position":{"SubPositionIds":null,"Id":7,"Name":"Specialists","Description":null},"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":"Male","PathToPhoto":"https://rmteamphotostest.blob.core.windows.net/public/users/photo/cropped/999daed42dec49f59ddf6e2fb4b5bca3.png","GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":5,"Name":"Karim","LastName":"Benzema","DateOfBirth":"2016-06-01T14:41:31.063Z","AadEmail":"Karim.Benzema@rmteamperformancetest.onmicrosoft.com","Email":"Karim.Benzema@rmteamperformancetest.onmicrosoft.com","Position":{"SubPositionIds":null,"Id":1,"Name":"Offensive Line","Description":null},"SubPosition":null,"ShirtNumber":null,"Height":74.40945,"Weight":207.2347,"Gender":"Male","PathToPhoto":"https://rmteamphotostest.blob.core.windows.net/public/users/photo/cropped/4e21786e133e4b9f99169570a029c8a9.png","GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":6,"Name":"K�pler Laver�n","LastName":"Lima Ferreira","DateOfBirth":"2016-06-01T14:41:47.13Z","AadEmail":"Kepler.Ferreira@rmteamperformancetest.onmicrosoft.com","Email":"player2@rm.com","Position":{"SubPositionIds":null,"Id":4,"Name":"Defensive Line","Description":null},"SubPosition":null,"ShirtNumber":null,"Height":65,"Weight":121.254341,"Gender":"Male","PathToPhoto":"https://rmteamphotostest.blob.core.windows.net/public/users/photo/cropped/48d5e3b14a14468391aa6d0363a9f8c1.png","GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":7,"Name":"Sergio","LastName":"Ramos","DateOfBirth":"2016-06-01T14:41:52.755Z","AadEmail":"Sergio.Ramos@rmteamperformancetest.onmicrosoft.com","Email":"Ramos@rmteamperformancetest.onmicrosoft.com","Position":{"SubPositionIds":null,"Id":4,"Name":"Defensive Line","Description":null},"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":"Male","PathToPhoto":"https://rmteamphotostest.blob.core.windows.net/public/users/photo/cropped/16a077375b994a8bb42c1d36429f85be.png","GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":8,"Name":"�lvaro","LastName":"Arbeloa","DateOfBirth":null,"AadEmail":"Alvaro.Arbeloa@rmteamperformancetest.onmicrosoft.com","Email":"Alvaro.Arbeloa@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":"https://rmteamphotostest.blob.core.windows.net/public/users/photo/cropped/eb17c7bdf0984597866da4918c71f3ec.png","GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":9,"Name":"Borja","LastName":"Mayoral Moya","DateOfBirth":null,"AadEmail":"Borja.Mayoral@rmteamperformancetest.onmicrosoft.com","Email":"Borja.Mayoral@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":10,"Name":"Carlos Henrique Jos�","LastName":"Francisco Ven�ncio","DateOfBirth":"2016-06-29T14:47:12.788Z","AadEmail":"Carlos.Venancio@rmteamperformancetest.onmicrosoft.com","Email":"Carlos@email.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":"Male","PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":11,"Name":"Daniel","LastName":"Carvajal","DateOfBirth":null,"AadEmail":"Daniel.Carvajal@rmteamperformancetest.onmicrosoft.com","Email":"Daniel.Carvajal@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":12,"Name":"Danilo Luiz","LastName":"da Silva","DateOfBirth":null,"AadEmail":"Danilo.Silva@rmteamperformancetest.onmicrosoft.com","Email":"Danilo.Silva@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":13,"Name":"Denis","LastName":"Cheryshev","DateOfBirth":null,"AadEmail":"Denis.Cheryshev@rmteamperformancetest.onmicrosoft.com","Email":"Denis.Cheryshev@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":14,"Name":"Francisco","LastName":"Casilla Cort�s","DateOfBirth":null,"AadEmail":"Francisco.Cortes@rmteamperformancetest.onmicrosoft.com","Email":"Francisco.Cortes@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":15,"Name":"Francisco Rom�n","LastName":"Alarc�n Su�rez","DateOfBirth":"2017-03-13T01:22:13.95Z","AadEmail":"Francisco.Suarez@rmteamperformancetest.onmicrosoft.com","Email":"Francisco.Suarez@rmteamperformancetest.onmicrosoft.com","Position":{"SubPositionIds":null,"Id":3,"Name":"Running Back","Description":null},"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":"Male","PathToPhoto":"https://rmteamphotostest.blob.core.windows.net/public/users/photo/cropped/1c4593b60136439ea990d180d34362a9.png","GetsAlerts":false,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":false,"InGameDays":null},{"Id":16,"Name":"Gareth","LastName":"Bale","DateOfBirth":null,"AadEmail":"Gareth.Bale@rmteamperformancetest.onmicrosoft.com","Email":"Gareth.Bale@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":17,"Name":"James","LastName":"Rodr�guez","DateOfBirth":null,"AadEmail":"James.Rodriguez@rmteamperformancetest.onmicrosoft.com","Email":"James.Rodriguez@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":18,"Name":"Jes� Rodr�guez","LastName":"Ru�z","DateOfBirth":null,"AadEmail":"Jese.Rodriguez@rmteamperformancetest.onmicrosoft.com","Email":"Jese.Rodriguez@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":19,"Name":"Jos� Ignacio","LastName":"Fern�ndez Iglesias","DateOfBirth":null,"AadEmail":"Jose.Iglesias@rmteamperformancetest.onmicrosoft.com","Email":"Jose.Iglesias@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":20,"Name":"Lucas","LastName":"V�zquez Iglesias","DateOfBirth":null,"AadEmail":"Lucas.Iglesias@rmteamperformancetest.onmicrosoft.com","Email":"Lucas.Iglesias@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":21,"Name":"Luka","LastName":"Modric","DateOfBirth":null,"AadEmail":"Luka.Modric@rmteamperformancetest.onmicrosoft.com","Email":"Luka.Modric@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":22,"Name":"Marcelo Vieira","LastName":"Da Silva Junior","DateOfBirth":null,"AadEmail":"Marcelo.daSilva@rmteamperformancetest.onmicrosoft.com","Email":"Marcelo.daSilva@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":23,"Name":"Marcos","LastName":"Llorente Moreno","DateOfBirth":null,"AadEmail":"Marcos.Moreno@rmteamperformancetest.onmicrosoft.com","Email":"Marcos.Moreno@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":24,"Name":"Mateo","LastName":"Kovacic","DateOfBirth":null,"AadEmail":"Mateo.Kovacic@rmteamperformancetest.onmicrosoft.com","Email":"Mateo.Kovacic@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":25,"Name":"Raphael","LastName":"Varane","DateOfBirth":null,"AadEmail":"Raphael.Varane@rmteamperformancetest.onmicrosoft.com","Email":"Raphael.Varane@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":null,"GetsAlerts":false,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":26,"Name":"Toni","LastName":"Kroos","DateOfBirth":null,"AadEmail":"Toni.Kroos@rmteamperformancetest.onmicrosoft.com","Email":"Toni.Kroos@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":27,"Name":"Rub�n","LastName":"Y��ez","DateOfBirth":null,"AadEmail":"Ruben.Yanez@rmteamperformancetest.onmicrosoft.com","Email":"Ruben.Yanez@rmteamperformancetest.onmicrosoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":null,"PathToPhoto":"https://rmteamphotostest.blob.core.windows.net/public/users/photo/cropped/d1ce3a132bce46fc86a40c83a92168e2.png","GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Player","Roles":[{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null},{"Id":28,"Name":"Test","LastName":"To show","DateOfBirth":"2016-06-28T08:42:52.508Z","AadEmail":"test.test@rmteamperformancetest.onmicrosoft.com","Email":"sugopa@microsoft.com","Position":null,"SubPosition":null,"ShirtNumber":null,"Height":null,"Weight":null,"Gender":"Male","PathToPhoto":null,"GetsAlerts":true,"HasEmailNotifications":false,"CertifiedForAccess":true,"AccountType":"Administrator","Roles":[{"Id":1,"Name":"Administrator"},{"Id":2,"Name":"Player"}],"Active":true,"InGameDays":null}]}');
        }

        function getDrills(ResourceService) {
            return ResourceService.getAllDrills();
        }
    }
})();