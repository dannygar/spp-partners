/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Configuration;
using System.Globalization;
using SportsDataLoader.Shared.Extensions;
using SportsDataLoader.Web.Interfaces;

namespace SportsDataLoader.Web.Configuration
{
    public class LocalWebConfiguration : IWebConfiguration
    {
        public const string DefaultCultureCodeKey = "DefaultCultureCode";
        public const string DefaultTenantIdKey = "DefaultTenantId";

        public LocalWebConfiguration()
        {
            ConfigureDefaultCulture();
            ConfigureDefaultTenantId();
        }

        public CultureInfo DefaultCulture { get; private set; }
        public Guid DefaultTenantId { get; private set; }

        private void ConfigureDefaultCulture()
        {
            var defaultCultureCode = ConfigurationManager.AppSettings[DefaultCultureCodeKey];

            if (string.IsNullOrEmpty(defaultCultureCode))
            {
                DefaultCulture = CultureInfo.CurrentCulture;
            }
            else
            {
                var defaultCulture = defaultCultureCode.TryParseCultureInfo();

                if (defaultCulture == null)
                {
                    throw new ConfigurationErrorsException($"[{DefaultCultureCodeKey}] is invalid. " +
                                                           $"[{defaultCultureCode}] is not a valid culture code.");
                }

                DefaultCulture = defaultCulture;
            }
        }

        private void ConfigureDefaultTenantId()
        {
            var defaultTenantId = ConfigurationManager.AppSettings[DefaultTenantIdKey];

            if (string.IsNullOrEmpty(defaultTenantId))
                throw new ConfigurationErrorsException($"[{DefaultTenantIdKey}] not configured.");

            Guid defaultTenantGuid;

            if (Guid.TryParse(defaultTenantId, out defaultTenantGuid) == false)
            {
                throw new ConfigurationErrorsException($"[{DefaultTenantIdKey}] is invalid." +
                                                       $"[{defaultTenantId}] is not a Guid.");
            }

            DefaultTenantId = defaultTenantGuid;
        }
    }
}