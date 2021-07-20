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
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        /// <summary>
        /// User variable to save user name and age.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Game variable to handle questions and score.
        /// </summary>
        public Game Game { get; set; }

        /// <summary>
        /// GameType variable to be able to set the created games type.
        /// </summary>
        public GameTypes GameType { get; set; }

        /// <summary>
        /// Dispatcher timer variable.
        /// </summary>
        public DispatcherTimer Timer { get; set; }

        /// <summary>
        /// Sound player for correct answers.
        /// </summary>
        public SoundPlayer CorrectAnswerSound { get; set; }

        /// <summary>
        /// Sound player for correct answers.
        /// </summary>
        public SoundPlayer IncorrectAnswerSound { get; set; }

        /// <summary>
        /// Time when the game starts
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Constructor that creates a new game based on game type.
        /// </summary>
        public GameWindow(GameTypes gameType)
        {
            InitializeComponent();
            GameType = gameType;
            Game = new(GameType);
            Game_Type_Title.Content = GameType.ToString();
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += new EventHandler(Timer_Tick);
            StartTime = DateTime.Now;
            CorrectAnswerSound = new SoundPlayer("the-force-is-strong-with-this-one.wav");
            IncorrectAnswerSound = new SoundPlayer("i-can't-shake-him.wav");
        }

        /// <summary>
        /// Formats the timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                TimeSpan TimeDifference = new();
                TimeDifference = DateTime.Now.Subtract(StartTime);
                string TimeElapsed = ((int)TimeDifference.TotalSeconds).ToString();
                Timer_Label.Content = TimeElapsed;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Cancel the current game and return to main window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Timer.Stop();
                Game_Window.Close();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Start the current game and timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Start_Button.Visibility = Visibility.Hidden;
                Start_Button.IsEnabled = false;
                Submit_Button.Visibility = Visibility.Visible;
                Submit_Button.IsEnabled = true;
                Submit_Button.IsDefault = true;
                Answer_TextBox.Visibility = Visibility.Visible;
                DisplayQuestion();
                Timer.Start();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Check if answer is correct and display label.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isValid = int.TryParse(Answer_TextBox.Text, out int answer);
                if (isValid)
                {
                    if (answer == Game.CalculateAnswer(Game.FirstNumber, Game.SecondNumber))
                    {
                        Check_Answer_Label.Foreground = Brushes.Teal;
                        Check_Answer_Label.Content = "You answered wisely!";
                        Game.UserScore++;
                        CorrectAnswerSound.Play();
                    }
                    else
                    {
                        Check_Answer_Label.Foreground = Brushes.Red;
                        Check_Answer_Label.Content = "Your answered foolishly.";
                        IncorrectAnswerSound.Play();
                    }
                }
                else
                {
                    Check_Answer_Label.Foreground = Brushes.Red;
                    Check_Answer_Label.Content = "Invalid Answer.";
                }

                if (Game.QuestionNumber == 10)
                {
                    Timer.Stop();
                    FinalScoreWindow finalScoreWindow = new();
                    finalScoreWindow.User = User;
                    finalScoreWindow.CorrectAnswers = Game.UserScore;
                    finalScoreWindow.TimeToCompletion = Timer_Label.Content.ToString();
                    Visibility = Visibility.Hidden;
                    finalScoreWindow.SetLabels();
                    finalScoreWindow.ShowDialog();
                    Close();
                }
                else
                {
                    Game.QuestionNumber++;
                }

                DisplayQuestion();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Display the current question.
        /// </summary>
        public void DisplayQuestion()
        {
            try
            {
                Question_Number.Content = "#" + Game.QuestionNumber.ToString();
                Answer_TextBox.Text = "";
                Question_Label.Content = Game.GenerateQuestion();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Only allow user to enter numbers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Answer_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Submit_Button_Click(sender, e);
                e.Handled = true;
                return;
            }

            //Only allow numbers to be entered
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                  e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                //Allow the user to use the backspace and delete keys
                if (!(e.Key == Key.Back || e.Key == Key.Delete))
                {
                    //No other keys allowed besides numbers, backspace, and delete
                    e.Handled = true;
                }
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
