/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Spp.Presentation.User.Client.Data;

namespace Spp.Presentation.User.Client.ViewModels
{
    public class AthleteExerciseViewModel : NotificationBase<AthleteExercise>
    {
        public bool Completed { get; set; }
        private bool _isActive;
        private List<AthleteExerciseSetViewModel> _setModels;
        private ObservableCollection<AthleteExerciseSetViewModel> _sets;

        public AthleteExerciseViewModel(AthleteExercise exercise) : base(exercise)
        {
            //FOR DEMO PURPOSES ONLY! 
            //TODO: Change per customer's requirements
            _setModels = new List<AthleteExerciseSetViewModel>();
            for (int i = 1; i <= (exercise.Sets.Sets + 1); i++)
            {
                _setModels.Add(new AthleteExerciseSetViewModel(exercise.Sets, i));
            }

            Completed = false;
            IsActive = false;
            _sets = new ObservableCollection<AthleteExerciseSetViewModel>(_setModels);
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                RaisePropertyChanged("IsActive");
            }
        }

        public override Task Load() => null;

        public int Id => This.Id;

        public string Category => This.Category;
        public ObservableCollection<AthleteExerciseSetViewModel> Sets
        {
            get { return _sets; }
            set { SetProperty(Sets, value, () => _sets = value); }
        }

        public int Duration
        {
            get { return This.Duration; }
            set { SetProperty(This.Duration, value, () => This.Duration = value); }
        }

        public string Description
        {
            get { return This.Description; }
            set { SetProperty(This.Description, value, () => This.Description = value); }
        }
        public string Name
        {
            get { return This.Name; }
            set { SetProperty(This.Name, value, () => This.Name = value); }
        }
        public string ImageUrl
        {
            get { return This.ImageUrl; }
            set { SetProperty(This.ImageUrl, value, () => This.ImageUrl = value); }
        }
        public string Notes
        {
            get { return This.Note.Text; }
            set { SetProperty(This.Note.Text, value, () => This.Note.Text = value); }
        }
        public int NumberOfSets
        {
            get { return This.Sets.Sets; }
            set { SetProperty(This.Sets.Sets, value, () => This.Sets.Sets = value); }
        }

        public string TrainingLoad => This.Sets.TrainingLoad.ToString();
        public int Order => This.Sets.Order;
        public int Reps
        {
            get { return This.Sets.Reps; }
            set
            {
                SetProperty(This.Sets.Reps, value, () => This.Sets.Reps = value);
            }
        }
        public float Weight
        {
            get { return This.Sets.Weight; }
            set
            {
                SetProperty(This.Sets.Weight, value, () => This.Sets.Weight = value);
            }
        }
        public int RecoveryTimeInMin
        {
            get { return This.Sets.RecoveryTimeInMin ?? 0; }
            set { SetProperty(This.Sets.RecoveryTimeInMin, value, () => This.Sets.RecoveryTimeInMin = value); }
        }
    }
}
