/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Spp.Presentation.User.Client.Models;

namespace Spp.Presentation.User.Client.ViewModels
{
    public class RecommendedLoadViewModel : NotificationBase
    {
        private RecommendedLoadModel _loadModel;

        public RecommendedLoadViewModel() : base()
        {
            _loadModel = SimpleIoc.Default.GetInstance<RecommendedLoadModel>();
        }

        private int _recommendedLoad;
        public int RecommendedLoad
        {
            get { return _recommendedLoad; }
            set { SetProperty(ref _recommendedLoad, value); }
        }

        public override async Task Load()
        {
            RecommendedLoad = (int)Math.Round(await _loadModel.InvokeRequestResponseService());
        }
    }
}
