/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace FaceAPITrainer
{
    public class Defines
    {
        public static string API_BASE_URL = "https://sppdevapi.azurewebsites.net/api/";

        public static bool SkipAuthentication = false; //ONLY FOR TEST & DEBUG PURPOSES

        public static string API_AUTHENTICATION_ENDPOINT = "v1/auth";
        public static string API_COGNITIVESERVICES_ENDPOINT = "v1/cs";
        public static string API_TEAM_ENDPOINT = "v1/teams/players";

        //B2C AAD Settings
        public const string B2CTenant = "tppusers.onmicrosoft.com";
        public const string AADInstance = "https://login.microsoftonline.com/{0}";
        public const string B2CClientId = "610aa26c-9b77-4038-9954-64b856c50016";
        public const string SignInPolicy = "B2C_1_TPP_SignIn";
        public const string PasswordResetPolicy = "B2C_1_tpp_passwordreset";
        //B2B AAD Settings
        public const string B2BTenant = "tppadmin.onmicrosoft.com";
        public const string B2BClientId = "4c5c9914-00eb-4079-bd2f-28fd846ab556";
        public const string B2BClientKey = "uQ5UWwNjxP29TxdEh5c/ffBeE5fsaSdnTP0kCsHEY4I=";
        public const string Audience = "https://tppadmin.onmicrosoft.com/tppapi";


    }
}
