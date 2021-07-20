using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Assignment_5_Math_Game
{
    /// <summary>
    /// Interaction logic for FinalScoreWindow.xaml
    /// </summary>
    public partial class FinalScoreWindow : Window
    {
        /// <summary>
        /// User variable for name and age.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Number of correct answers.
        /// </summary>
        public int CorrectAnswers { get; set; }

        /// <summary>
        /// Time taken to complete the game.
        /// </summary>
        public string TimeToCompletion { get; set; }

        /// <summary>
        /// Image brush to set background based on score.
        /// </summary>
        public ImageBrush Brush { get; set; }

        public FinalScoreWindow()
        {
            InitializeComponent();
            SoundPlayer simpleSound = new SoundPlayer("podracing.wav");
            simpleSound.Play();
        }

        /// <summary>
        /// Set all the labels on the Final score window.
        /// </summary>
        public void SetLabels()
        {
            Name_Label.Content = "Name: " + User.Name;
            Age_Label.Content = "Age: " + User.Age.ToString();
            Correct_Answers_Label.Content = "Correct Answers: " + CorrectAnswers.ToString();
            Incorrect_Answers_Label.Content = "Incorrect Answers: " + (10 - CorrectAnswers).ToString();
            Time_Completion_Label.Content = "Time to Completion: " + TimeToCompletion.ToString();
            SetBackground();
        }

        /// <summary>
        /// Set the background image based on the score.
        /// </summary>
        private void SetBackground()
        {
            try
            {
                if (CorrectAnswers < 4)
                {
                    Brush = new ImageBrush(new BitmapImage(new Uri(@"Images/throne_room.jpg", UriKind.Relative)));
                }
                else if (CorrectAnswers > 4 && CorrectAnswers < 8)
                {
                    Brush = new ImageBrush(new BitmapImage(new Uri(@"Images/pod_racing.jpg", UriKind.Relative)));
                }
                else
                {
                    Brush = new ImageBrush(new BitmapImage(new Uri(@"Images/naboo_celebration.jpg", UriKind.Relative)));
                }

                Final_Score_Window.Background = Brush;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Handle the error.
        /// </summary>
        /// <param name="sClass">The class in which the error occurred in.</param>
        /// <param name="sMethod">The method in which the error occurred in.</param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                //Would write to a file or database here.
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
            }
        }
    }
}
