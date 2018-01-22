/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Spp.Presentation.User.Client.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
{

    public sealed partial class MatchPerformanceUserControl : UserControl
    {
        private List<MatchViewModel> DummyDataList = new List<MatchViewModel>();
        private List<string> DummyStatList = new List<string>();
        private MatchViewModel TempResult1, TempResult2, TempResult3;

        public MatchPerformanceUserControl()
        {
            this.InitializeComponent();
            TempResult1 = new MatchViewModel();
            TempResult1.Date = "Oct 22, 2017";
            TempResult1.Match = "SEA @ NY Giants";
            TempResult1.Result = "W 24-7";
            TempResult1.Appearance = "1556";
            TempResult1.Mins = "138";
            TempResult1.Goals = "11";
            TempResult1.Assists = "1";
            TempResult1.Shots = "3";
            TempResult1.Sog = "0";

            TempResult2 = new MatchViewModel();
            TempResult2.Date = "Oct 8, 2017";
            TempResult2.Match = "SEA @ LA Rams";
            TempResult2.Result = "W 16-10";
            TempResult2.Appearance = "1240";
            TempResult2.Mins = "111";
            TempResult2.Goals = "7";
            TempResult2.Assists = "2";
            TempResult2.Shots = "0";
            TempResult2.Sog = "0";

            TempResult3 = new MatchViewModel();
            TempResult3.Date = "Oct 1, 2017";
            TempResult3.Match = "SEA @ Colts";
            TempResult3.Result = "W 46-18";
            TempResult3.Appearance = "1621";
            TempResult3.Mins = "146";
            TempResult3.Goals = "16";
            TempResult3.Assists = "4";
            TempResult3.Shots = "1";
            TempResult3.Sog = "0";

            DummyStatList = new List<string>();
            DummyStatList.Add("DATE");
            DummyStatList.Add("MATCH");
            DummyStatList.Add("RESULT");
            DummyStatList.Add("YARDS");
            DummyStatList.Add("PASSING");
            DummyStatList.Add("TD ATTEMPTS");
            DummyStatList.Add("TACKLES");
            DummyStatList.Add("INT");
            DummyStatList.Add("FUMBLES");
        }

        private void MatchPerformanceUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // adding dummy data
            DummyDataList = new List<MatchViewModel>();
            DummyDataList.Add(TempResult1);
            DummyDataList.Add(TempResult2);
            DummyDataList.Add(TempResult3);


            this.Bindings.Update();
        }


    }
}
