/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.UI.Xaml.Data;

namespace Spp.Presentation.User.Client.Data
{
    [Bindable]
    public class PhotosDataSource
    {
        private static ObservableCollection<PhotoDataItem> _photos;
        private static ObservableCollection<IEnumerable<PhotoDataItem>> _groupedPhotos;
        private static bool _isOnlineCached;

        public async Task<ObservableCollection<PhotoDataItem>> GetItemsAsync(bool online = true, int maxCount = -1)
        {
            CheckCacheState(online);

            if (_photos == null)
            {
                await LoadAsync(online, maxCount);
            }

            return _photos;
        }

        public async Task<ObservableCollection<IEnumerable<PhotoDataItem>>> GetGroupedItemsAsync(bool online = true, int maxCount = -1)
        {
            CheckCacheState(online);

            if (_groupedPhotos == null)
            {
                await LoadAsync(online, maxCount);
            }

            return _groupedPhotos;
        }

        private static async Task LoadAsync(bool online, int maxCount)
        {
            _isOnlineCached = online;
            _photos = new ObservableCollection<PhotoDataItem>();
            _groupedPhotos = new ObservableCollection<IEnumerable<PhotoDataItem>>();

            foreach (var item in await GetPhotosAsync(online))
            {
                _photos.Add(item);

                if (maxCount != -1)
                {
                    maxCount--;

                    if (maxCount == 0)
                    {
                        break;
                    }
                }
            }

            foreach (var group in _photos.GroupBy(x => x.Category))
            {
                _groupedPhotos.Add(group);
            }
        }

        private static async Task<IEnumerable<PhotoDataItem>> GetPhotosAsync(bool online)
        {
            var photos = new List<PhotoDataItem>()
            {
                new PhotoDataItem()
                {
                    Title = "A green sea turtle shows off its shell (© Sergi Garcia Fernandez/Minden Pictures)",
                    Category = "Animal",
                    Thumbnail = "http://az619822.vo.msecnd.net/files/CanaryIslandsTurtle_EN-US8274101746_1366x768.jpg",
                },
                new PhotoDataItem()
                {
                    Title = "An elevated walkway in the Lujiazui district of Shanghai, China (© Mark Harris/Getty Images)",
                    Category = "City",
                    Thumbnail = "http://az619822.vo.msecnd.net/files/ShanghaiElevatedWalkway_EN-US8623422930_1366x768.jpg"
                },
                new PhotoDataItem()
                {
                    Title = "Fox kits playing in the Rocky Mountain foothills near Cascade, Montana (© Jason Savage/Tandem Stills + Motion)",
                    Category = "Animal",
                    Thumbnail = "http://az619519.vo.msecnd.net/files/RockyMtFox_EN-US11902018005_1366x768.jpg"
                },
                new PhotoDataItem()
                {
                    Title = "Burano, in the Venetian Lagoon, Italy (© Digitaler Lumpensammler/Getty Images)",
                    Category = "City",
                    Thumbnail = "http://az608707.vo.msecnd.net/files/Burano_EN-US12610622868_1366x768.jpg"
                },
                new PhotoDataItem()
                {
                    Title = "A green sea turtle shows off its shell (© Sergi Garcia Fernandez/Minden Pictures)",
                    Category = "Animal",
                    Thumbnail = "http://az619822.vo.msecnd.net/files/CanaryIslandsTurtle_EN-US8274101746_1366x768.jpg"
                },
                new PhotoDataItem()
                {
                    Title = "An elevated walkway in the Lujiazui district of Shanghai, China (© Mark Harris/Getty Images)",
                    Category = "City",
                    Thumbnail = "http://az619822.vo.msecnd.net/files/ShanghaiElevatedWalkway_EN-US8623422930_1366x768.jpg",
                },
                new PhotoDataItem()
                {
                    Title = "Fox kits playing in the Rocky Mountain foothills near Cascade, Montana (© Jason Savage/Tandem Stills + Motion)",
                    Category = "Animal",
                    Thumbnail = "http://az619519.vo.msecnd.net/files/RockyMtFox_EN-US11902018005_1366x768.jpg",
                },
                new PhotoDataItem()
                {
                    Title = "Burano, in the Venetian Lagoon, Italy (© Digitaler Lumpensammler/Getty Images)",
                    Category = "City",
                    Thumbnail = "http://az608707.vo.msecnd.net/files/Burano_EN-US12610622868_1366x768.jpg"
                },
            };

            return photos;

            //var prefix = online ? "Online" : string.Empty;
            //var uri = new Uri($"ms-appx:///Assets/Photos/{prefix}Photos.json");
            //StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            //IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();

            //using (StreamReader r = new StreamReader(randomStream.AsStreamForRead()))
            //{
            //    return Parse(await r.ReadToEndAsync());
            //}
        }

        private static IEnumerable<PhotoDataItem> Parse(string jsonData)
        {
            return JsonConvert.DeserializeObject<IList<PhotoDataItem>>(jsonData);
        }

        private static void CheckCacheState(bool online)
        {
            if (_isOnlineCached != online)
            {
                _photos = null;
                _groupedPhotos = null;
            }
        }
    }
}
