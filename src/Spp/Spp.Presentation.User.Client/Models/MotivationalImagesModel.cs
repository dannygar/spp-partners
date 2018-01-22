/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spp.Presentation.User.Client.Data;
using Spp.Presentation.User.Client.Services;

namespace Spp.Presentation.User.Client.Models
{
    public class MotivationalImagesModel : BaseModel
    {
        private IHttpClientService _dataService;
        private ILogService _logService;
        private ICacheService _cacheService;

        public MotivationalImagesModel(IHttpClientService dataService, ILogService logService, ICacheService cacheService)
        {
            _dataService = dataService;
            _logService = logService;
            _cacheService = cacheService;
        }

        public async Task<List<TeamImage>> GetImages()
        {
            // Check cache
            if (_cacheService.IsCached(Defines.CACHE_KEY_MOTIVATIONAL_IMAGES))
            {
                var cache = _cacheService.GetItem<List<TeamImage>>(Defines.CACHE_KEY_MOTIVATIONAL_IMAGES);
                if (cache != null)
                    return cache;
            }

            _logService.Info($"Getting the images for the team using: {_dataService.GetType()}", this);
            var result = await _dataService.GetItemAsync<List<TeamImage>>(Defines.API_SETTINGS_IMAGES);

            if (result != null)
            {
                var filtered = result.Where(i => i.Tags != null && i.Tags.ToLower() == "motivational");
                if (filtered != null)
                    CacheImages(filtered.ToList());
                return filtered.ToList();
            }
            else
            {
                return null;
            }

        }

        public void CacheImages(List<TeamImage> images)
        {
            _cacheService.SetItem<List<TeamImage>>(Defines.CACHE_KEY_MOTIVATIONAL_IMAGES, images);
        }



    }
}
