/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
namespace MicrosoftSportsScience
{
    public class Defines
    {
        public static string API_BASE_URL;

        public static string API_AUTHENTICATION_ENDPOINT = "api/v1/auth/user";
        public static string API_COGNITIVESERVICES_ENDPOINT = "api/v1/cs/keys";
        public static string API_AUTH_RECOGNIZED_ENDPOINT = "api/v1/cs/faceid";

        public static string API_TEAM_ENDPOINT = "api/v1/teams";
        public static string API_TEAM_WITH_PLAYERS_ENDPOINT = "api/v1/teams/players/{0}";
        public static string API_TEAM_TRENDING_ENDPOINT = "api/v1/teams/{0}/teamtrending";
        public static string API_TEAM_READINESS_ENDPOINT = "api/v1/teams/{0}/readiness?from={1}&to={2}";
        public static string API_TEAM_NEXT_MATCH = "api/v1/teams/{0}/nextmatch";

        public static string API_SETTINGS_IMAGES = "api/v1/images";

        public static string API_PLAYERS_ENDPOINT = "api/v1/teams/{0}";
        public static string API_PLAYER_ENDPOINT = "api/v1/players/{0}";
        public static string API_PLAYER_READINESS_ENDPOINT = "api/v1/players/{0}/readiness?from={1}&to={2}";
        public static string API_PLAYER_WELLNESS_ENDPOINT = "api/v1/players/{0}/wellness?dx={1}";
        public static string API_PLAYER_MATCH_STATS_ENDPOINT = "api/v1/players/{0}/statistics";
        public static string API_PLAYER_RESTING_ENDPOINT = "api/v1/players/{0}/resting";
        public static string API_PLAYER_RESPONDED = "api/v1/responses/{0}/{1}";

        public static string API_COACHES_ENDPOINT = "api/v1/coaches";
        public static string API_COACH_NOTES_ENDPOINT = "api/v1/coachesnotes/{0}?textvalue={1}";

        public static string API_QUESTIONS_ENDPOINT = "api/v1/questionnaires/{0}";
        public static string API_QUESTIONS_RESPONSE_ENDPOINT = "api/v1/questionnaires/response/{0}/{1}";

        public static string API_SESSION_ENDPOINT = "api/v1/sessions/{0}";
        public static string API_SESSION_USERS_SAVE_ENDPOINT = "api/v1/sessions/addusers";

        public static string API_MESSAGE_ENDPOINT = "api/v1/messages/{0}";

        public static string API_WORKOUTS_ENDPOINT = "api/v1/workouts/{0}";
        public static string API_WORKOUT_SAVE_ENDPOINT = "api/v1/workouts/exercises/update";
        public static string API_WORKOUT_GET_EXERCISES_ENDPOINT = "api/v1/workouts/exercises";
        public static string API_WORKOUTS_COMPLETE_ENDPOINT = "api/v1/workouts/{0}?isCompleted={1}&isModified={2}";

        public static string API_PRACTICES_ENDPOINT = "api/v1/practices";
        public static string API_PRACTICES_UPDATE_ENDPOINT = "api/v1/practices/update";

        public static string API_PRACTICE_ENDPOINT = "api/v1/practices/{0}";
        public static string API_PRACTICE_BYID_ENDPOINT = "api/v1/practices/id/{0}";
        public static string API_ALLPRACTICEDRILLS_ENDPOINT = "api/v1/practices/drills";
        public static string API_PRACTICEDRILLS_ENDPOINT = "api/v1/practices/drills/{0}";
        public static string API_DRILLS_ENDPOINT = "api/v1/drills";

        public static string API_RESPONSE_ENDPOINT = "api/v1/responses";

        public static string API_PERFORMANCE_METRICS_ENDPOINT = "api/v1/metrics";
        public static string API_PERFORMANCE_DATA_ENDPOINT = "api/v1/metrics/data";

        public static string API_SKILLS_ENDPOINT = "api/v1/skills";

        public static string CACHE_KEY_TEAM = "cache_team";
        public static string CACHE_KEY_SESSION = "cache_session";
        public static string CACHE_KEY_ATHLETESESSIONS = "cache_athlete_sessions";
        public static string CACHE_KEY_ATHLETEMESSAGES = "cache_athlete_messages";
        public static string CACHE_KEY_ATHLETEHISTORY = "cache_athlete_history";
        public static string CACHE_KEY_ATHLETEQUESTIONS = "cache_athlete_questions";
        public static string CACHE_KEY_ATHLETES = "cache_athletes";
        public static string CACHE_KEY_ATHLETEPRACTICES = "cache_athletes_practices";
        public static string CACHE_KEY_ATHLETEALLPRACTICES = "cache_athletes_all_practices";
        public static string CACHE_KEY_ATHLETEPRACTICES_BYID = "cache_athletes_practices_byid";
        public static string CACHE_KEY_PRACTICEDRILLS = "cache_practice_drills";
        public static string CACHE_KEY_PRACTICEALLDRILLS = "cache_practice_alldrills";
        public static string CACHE_KEY_DRILLS = "cache_drills";
        public static string CACHE_KEY_COACHES = "cache_athletes_coaches";
        public static string CACHE_KEY_ATHLETEWORKOUTS = "cache_athlete_workouts";
        public static string CACHE_KEY_EXERCISES = "cache_athlete_exercises";
        public static string CACHE_KEY_SESSIONWORKOUTS = "cache_session_workouts";
        public static string CACHE_KEY_METRICS = "cache_metrics";
        public static string CACHE_KEY_TEAM_TRENDING = "cache_team_trending";
        public static string CACHE_KEY_TEAM_READINESS = "cache_team_readiness";
        public static string CACHE_KEY_PLAYER_READINESS = "cache_player_readiness";
        public static string CACHE_KEY_PLAYER_RESPONDED = "cache_player_responses_{0}";
        public static string CACHE_KEY_PLAYER_WELLNESS = "cache_player_wellness";
        public static string CACHE_KEY_PLAYER_MATCH_STATS = "cache_player_match_stats";
        public static string CACHE_KEY_METRICSETTINGS = "cache_metric_settings";
        public static string CACHE_KEY_NEXT_MATCH_SUMMARY = "cache_next_match_summary";
        public static string CACHE_KEY_WELLNESS_SETTINGS = "cache_wellness_settings";
        public static string CACHE_KEY_SKILLS = "cache_skills";
        public static string CACHE_KEY_MOTIVATIONAL_IMAGES = "cache_motivational_images";

        //ADv2 Settings
        //Below is the clientId of your app registration. 
        //You have to replace the below with the Application Id for your app registration
        //public const string ClientId = "436b73b7-8f59-45e7-9449-580fb9a5d90d";
        // Tenant is the tenant ID (e.g. contoso.onmicrosoft.com, or 'common' for multi-tenant)
        //public const string TenantId = "0802107c-ed17-4b41-9f91-96250f0b54d4"; //tppusers.onmicrosoft.com


        //B2C AAD Settings
        //public const string B2CTenant = "tppusers.onmicrosoft.com";
        //public const string AADInstance = "https://login.microsoftonline.com/{0}";
        //public const string B2CClientId = "10596739-c12e-46be-8dc4-32ecaf7aeaac";
        //public const string SignInPolicy = "B2C_1_TPP_login";
        //public const string PasswordResetPolicy = "B2C_1_tpp_passwordreset";
        //B2B AAD Settings
        //public const string B2BTenant = "tppadmin.onmicrosoft.com";
        //public const string B2BClientId = "4c5c9914-00eb-4079-bd2f-28fd846ab556";
        //public const string B2BClientKey = "uQ5UWwNjxP29TxdEh5c/ffBeE5fsaSdnTP0kCsHEY4I=";
        //public const string Audience = "https://tppadmin.onmicrosoft.com/tppapi";


        //public static string ML_CLIENT_KEY = "4vCGbQhsWvt1mlm8RzkvDeJ9vEq1Z6I71v4Wm8wA6vv+SDXIEwqj6Zg8vA76O68fFFb14idf4577LqoQQxWlEg==";
        //public static string ML_URL = "https://ussouthcentral.services.azureml.net/workspaces/320fa3d96f05432184e98eaff316509b/services/8126537b7f1e43d993d968a469907b9f/execute?api-version=2.0&details=true";

        public static string URLPARAMETER_DATE_FORMAT = "MM-yyyy";

        public enum GraphMode { Line, Bar };


        public static double FACE_RECOGNITION_MIN_SCORE = 0.5;
        public static bool FACE_RECOGNITION_DEBUG = false;
        public static int FACE_RECOGNITION_MESSAGE_DELAY = 500; //milliseconds

    }
}
