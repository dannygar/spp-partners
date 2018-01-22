/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Microsoft.ProjectOxford.Common.Contract;
using Spp.Presentation.User.Shared.CognitiveServiceHelpers;
using System.Linq;
using Windows.UI.Xaml.Controls;


namespace Spp.Presentation.User.Client.UserControls
{
    /// <summary>
    /// Interaction logic for EmotionEmojiControl.xaml
    /// </summary>
    public partial class EmotionEmojiControl : UserControl
    {
        public EmotionEmojiControl()
        {
            InitializeComponent();
        }

        public void UpdateEmotion(EmotionScores scores)
        {
            EmotionData topEmotion = EmotionServiceHelper.ScoresToEmotionData(scores).OrderByDescending(d => d.EmotionScore).First();
            string label = "", emoji = "";

            switch (topEmotion.EmotionName)
            {
                case "Anger":
                    label = "Angry";
                    emoji = "\U0001f620";
                    break;
                case "Contempt":
                    label = "Contemptuous";
                    emoji = "\U0001f612";
                    break;
                case "Disgust":
                    label = "Disgusted";
                    emoji = "\U0001f627";
                    break;
                case "Fear":
                    label = "Afraid";
                    emoji = "\U0001f628";
                    break;
                case "Happiness":
                    label = "Happy";
                    emoji = "\U0001f60a";
                    break;
                case "Neutral":
                    label = "Neutral";
                    emoji = "\U0001f614";
                    break;
                case "Sadness":
                    label = "Sad";
                    emoji = "\U0001f622";
                    break;
                case "Surprise":
                    label = "Surprised";
                    emoji = "\U0001f632";
                    break;
                default:
                    break;
            }

            this.emotionEmoji.Text = emoji;
            this.emotionText.Text = label;
        }
    }
}
