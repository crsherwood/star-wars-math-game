using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_5_Math_Game
{
    /// <summary>
    /// Game class that handles the game data.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Type of game chosen. (Add, Subtract, Multiply, Divide)
        /// </summary>
        public GameTypes GameType { get; set; }

        /// <summary>
        /// Variable to keep track of player's score.
        /// </summary>
        public int UserScore { get; set; }

        /// <summary>
        /// Random number generator.
        /// </summary>
        private readonly Random rand;

        /// <summary>
        /// Question number.
        /// </summary>
        public int QuestionNumber { get; set; }

        /// <summary>
        /// Get the correct symbol for the game type.
        /// </summary>
        public string GameTypeSymbol { get; set; }

        /// <summary>
        /// First number in the question.
        /// </summary>
        public int FirstNumber { get; set; }

        /// <summary>
        /// Second Number in the question.
        /// </summary>
        public int SecondNumber { get; set; }

        /// <summary>
        /// Answer to the question.
        /// </summary>
        public int Answer { get; set; }

        /// <summary>
        /// Constructor that gets the game type from Main window
        /// and sets the data for a fresh game.
        /// </summary>
        /// <param name="gameType"></param>
        public Game(GameTypes gameType)
        {
            rand = new Random();
            UserScore = 0;
            QuestionNumber = 1;
            GameType = gameType;
            GameTypeSymbol = GetGameTypeSymbol(gameType);
        }

        /// <summary>
        /// Generates a question for the game.
        /// </summary>
        /// <returns></returns>
        public string GenerateQuestion()
        {
            FirstNumber = GenerateFirstNumber();
            SecondNumber = GenerateSecondNumber(FirstNumber);
            Answer = CalculateAnswer(FirstNumber, SecondNumber);
            return FirstNumber.ToString() + " " + GameTypeSymbol.ToString() + " " + SecondNumber.ToString() + " = ";
        }

        /// <summary>
        /// Generate a random number between 1 and 10.
        /// </summary>
        /// <returns></returns>
        public int GenerateFirstNumber()
        {
            int number = rand.Next(1, 11);
            return number;
        }

        /// <summary>
        /// Generate a second number that is smaller than the first number
        /// if the game type is subtraction or is evenly divisible by first number
        /// if the game type is division, otherwise return a random number.
        /// </summary>
        /// <param name="firstNum"></param>
        /// <returns></returns>
        public int GenerateSecondNumber(int firstNum)
        {
            int number;

            if (GameType == GameTypes.Subtraction)
            {
                number = rand.Next(1, firstNum);
            }
            else if (GameType == GameTypes.Division)
            {
                number = rand.Next(1, 11);
                while (firstNum % number != 0)
                {
                    number = rand.Next(1, 11);
                }
            }
            else
            {
                number = rand.Next(1, 11);
            }

            return number;
        }

        /// <summary>
        /// Calculate the answer to the current question.
        /// </summary>
        /// <param name="firstNum"></param>
        /// <param name="secondNum"></param>
        /// <returns></returns>
        public int CalculateAnswer(int firstNum, int secondNum)
        {
            if (GameType == GameTypes.Addition)
            {
                return firstNum + secondNum;
            }
            else if (GameType == GameTypes.Subtraction)
            {
                return firstNum - secondNum;
            }
            else if (GameType == GameTypes.Multiplication)
            {
                return firstNum * secondNum;
            }
            else
            {
                return firstNum / secondNum;
            }
        }

        /// <summary>
        /// Return the symbol of the game type.
        /// </summary>
        /// <param name="gameType"></param>
        /// <returns></returns>
        public string GetGameTypeSymbol(GameTypes gameType)
        {
            if (GameType == GameTypes.Addition)
            {
                return "+";
            }
            else if (GameType == GameTypes.Subtraction)
            {
                return "-";
            }
            else if (GameType == GameTypes.Multiplication)
            {
                return "*";
            }
            else
            {
                return "/";
            }
        }
    }
}
