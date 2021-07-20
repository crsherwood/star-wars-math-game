using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assignment_5_Math_Game
{
    /// <summary>
    /// Enum used for game types.
    /// </summary>
    public enum GameTypes
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    };

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Variable to hold user name and age.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Variable to hold the game type selected.
        /// </summary>
        public GameTypes GameType { get; set; }

        /// <summary>
        /// Constructor to initialize variables and clear labels.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            ClearLabels();
            User = new();
        }

        /// <summary>
        /// Start button click validates user input and then opens game window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateName() && ValidateAge() && ValidateGame())
                {
                    ClearLabels();
                    GameWindow newGameWindow = new(GameType);
                    newGameWindow.User = User;
                    Visibility = Visibility.Hidden;
                    newGameWindow.ShowDialog();
                    Visibility = Visibility.Visible;
                }
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
        private void AgeTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
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
        /// Check whether Name box is empty or set the name.
        /// </summary>
        /// <returns>bool</returns>
        private bool ValidateName()
        {
            if (string.IsNullOrEmpty(NameTextBox.Text))
            {
                NameValidationLabel.Content = "Please Enter Your Name to Continue.";
                return false;
            }
            else
            {
                User.Name = NameTextBox.Text;
                return true;
            }
        }

        /// <summary>
        /// Check whether age is valid and set the age.
        /// </summary>
        /// <returns>bool</returns>
        private bool ValidateAge()
        {
            bool isAgeValid = int.TryParse(AgeTextBox.Text, out int ageValue);

            if (string.IsNullOrEmpty(AgeTextBox.Text)){
                AgeValidationLabel.Content = "Please Enter Your Age To Continue. (3-10)";
                return false;
            }
            else if (isAgeValid == false)
            {
                AgeValidationLabel.Content = "Please Enter Your Age To Continue. (3-10)";
                return false;
            }
            else if (ageValue < 3 || ageValue > 10)
            {
                AgeValidationLabel.Content = "Please Enter Your Age To Continue. (3-10)";
                return false;
            }
            else
            {
                User.Age = ageValue;
                return true;
            }
        }

        /// <summary>
        /// Check whether a game type was selected and set the type.
        /// </summary>
        /// <returns>bool</returns>
        private bool ValidateGame()
        {
            // add a game type variable and save
            if (Add_Radio.IsChecked == true)
            {
                GameType = GameTypes.Addition;
                return true;
            }
            else if (Subtract_Radio.IsChecked == true)
            {
                GameType = GameTypes.Subtraction;
                return true;
            }
            else if (Mult_Radio.IsChecked == true)
            {
                GameType = GameTypes.Multiplication;
                return true;
            }
            else if (Division_Radio.IsChecked == true)
            {
                GameType = GameTypes.Division;
                return true;
            }
            else
            {
                GameValidationLabel.Content = "Please Select A Game Type To Continue.";
                return false;
            }
        }

        /// <summary>
        /// Clear the error labels from the window.
        /// </summary>
        public void ClearLabels()
        {
            NameValidationLabel.Content = "";
            AgeValidationLabel.Content = "";
            GameValidationLabel.Content = "";
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
