using System.Text;

namespace HangMan
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HangMan(); /*Krøre hangman som spillet liger i*/
        }
        static void HangMan() //Metoden som køre hangman spillet
        {
            Console.WriteLine("Welcome to Hangman, what is the word?");//beder spilleren om at indtaste et ord som skal gættes
            string guessWord = Console.ReadLine();//læser ordet som spilleren indtaster
            StringBuilder hiddenWord = new StringBuilder(new string('_', guessWord.Length));//gemmer ordet som spilleren skal gætte
            int life = 10;//antal forsøg spilleren har til at gætte
            Console.WriteLine("Guess the word, you have " + life + " tries.");//beder spilleren om at gætte ordet
            Console.WriteLine(hiddenWord.ToString());//viser spilleren hvor mange bogstaver der er i ordet som skal gættes

            while (life > 0) //så længe spilleren har liv tilbage køre spillet
            {
                bool correctGuess = false; //variabel som holder styr på om spilleren har gættet rigtigt
                string guess = Console.ReadLine();//læser spilleren gæt
                
                if (guess.Length == 1)//hvis spilleren gætter et bogstav
                {
                    for (int i = 0; i < guessWord.Length; i++)//går igennem ordet som spilleren skal gætte
                    {
                        if (guessWord[i].ToString().ToLower() == guess[0].ToString().ToLower())//hvis spilleren gætter rigtigt
                        {
                            hiddenWord[i] = guess[0];//gemmer det rigtige gæt i hiddenWord
                            correctGuess = true;//sætter correctGuess til true
                        }
                    }
                    if (correctGuess)//hvis spilleren gættede rigtigt
                    {
                        Console.WriteLine("Good guess! You have " + life + " lives left. What is the next letter?");//beder spilleren om at gætte igen
                        Console.WriteLine(hiddenWord.ToString());// viser hvor mange bogstaver der er i ordet dem som er belvet gætte
                    }
                    else //hvis spilleren gættede forkert
                    {
                        life -= 1; //trækker et liv fra spilleren
                        Console.WriteLine("That was wrong you have " + life + " lives left. What is the next letter?"); //beder spilleren om at gætte igen
                        Console.WriteLine(hiddenWord.ToString()); // viser hvor mange bogstaver der er i ordet dem som er belvet gætte
                    }
                }
                
                else if (guess.Length > 1) //hvis spilleren gætter et ord
                {
                    for (int i = 0; i < guessWord.Length; i++)//går igennem ordet som spilleren skal gætte
                    {
                        if (guessWord[i].ToString().ToLower() == guess[0].ToString().ToLower())//hvis spilleren gætter rigtigt
                        {
                            hiddenWord[i] = guess[0];//gemmer det rigtige gæt i hiddenWord
                            correctGuess = true;//sætter correctGuess til true
                        }
                    }

                    if (correctGuess) //
                    {
                        Console.WriteLine("That was right!"); //beder spilleren om at gætte igen
                        Console.WriteLine(hiddenWord.ToString());// viser hvor mange bogstaver der er i ordet dem som er belvet gætte
                    }
                    else
                    {
                        life -= 2;
                        Console.WriteLine("That was wrong " + life);
                        Console.WriteLine(hiddenWord.ToString());
                    }
                
                }

                if (hiddenWord.ToString() == guessWord)
                {
                    Console.WriteLine("You guessed the word!");
                    Console.WriteLine(hiddenWord.ToString());// viser hvor mange bogstaver der er i ordet dem som er belvet gætte
                    Console.WriteLine("want to play again? press y for yes or n for no");
                    string playAgain = Console.ReadLine();
                    if (playAgain.ToLower() == "y")
                    {
                        HangMan();
                    }
                    else if (playAgain.ToLower() == "n")
                    {
                        Console.WriteLine("Thanks for playing!");
                        break;
                    }
                }
                else if (life <= 0)
                {
                    Console.WriteLine("You ran out of lives!");
                    Console.WriteLine("The word was: " + guessWord);
                    Console.WriteLine("want to play again? press y for yes or n for no");
                    string playAgain = Console.ReadLine();
                    if (playAgain.ToLower() == "y")
                    {
                        HangMan();
                    }
                    else if (playAgain.ToLower() == "n")
                    {
                        Console.WriteLine("Thanks for playing!");
                        break;
                    }
                }
            }
        }
    }
}
