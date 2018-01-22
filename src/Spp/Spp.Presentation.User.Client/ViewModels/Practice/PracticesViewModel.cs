/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Spp.Presentation.User.Client.Data;
using Spp.Presentation.User.Client.Models;

namespace Spp.Presentation.User.Client.ViewModels
{
    public class PracticesViewModel : NotificationBase
    {
        private readonly AthletePracticeModel _practiceModel;
        private List<AthletePractice> _practices;

        public PracticesViewModel()
        {
            _practiceModel = SimpleIoc.Default.GetInstance<AthletePracticeModel>();
        }

        public List<AthletePracticeViewModel> Practices { get; } = new List<AthletePracticeViewModel>();

        public override async Task Load()
        {
            _logService.Info("Attempting to load practices", this);
            _practices = (List<AthletePractice>)await _practiceModel.GetAthletePractices();

            foreach (var practice in _practices)
            {
                var np = new AthletePracticeViewModel(practice);
                Practices.Add(np);
            }
        }
    }
}
