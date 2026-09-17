using System.Text;

namespace HangMan
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; /*Sætter encoding til UTF-8 så man kan*/

            HangMan(); /*Krøre hangman som spillet liger i*/
        }
        static void HangMan() //Metoden som køre hangman spillet
        {
            Console.Clear(); //rydder konsollen for tekst
            Console.Title = "Hangman"; //sætter titlen på konsollen til Hangman
            Console.WriteLine("Welcome to Hangman, what is the word?");//beder spilleren om at indtaste et ord som skal gættes
            
            string guessWord = Console.ReadLine();//læser ordet som spilleren indtaster
            Console.SetCursorPosition(0, Console.CursorTop - 1); //flytter cursoren op en linje

            StringBuilder hiddenWord = new StringBuilder(new string('_', guessWord.Length));//gemmer ordet som spilleren skal gætte
            int life = 8;//antal forsøg spilleren har til at gætte
            if (guessWord.Contains(" "))
            {
                for (int i = 0; i < guessWord.Length; i++) //går igennem ordet som spilleren skal gætte
                {
                    if (guessWord[i] == ' ') //hvis der er et mellemrum i ordet som spilleren skal gætte
                    {
                        hiddenWord[i] = ' ';//gemmer mellemrum i hiddenWord
                    }
                }
            }

            Console.WriteLine("Guess the word, you have " + life + " tries.");//beder spilleren om at gætte ordet
            DisplayHangman(life);
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
                        Console.Clear();
                        Console.WriteLine("Good guess! You have " + life + " lives left. What is the next letter?");//beder spilleren om at gætte igen
                        DisplayHangman(life);
                        Console.WriteLine(hiddenWord.ToString());// viser hvor mange bogstaver der er i ordet dem som er belvet gætte

                    }
                    else //hvis spilleren gættede forkert
                    {
                        Console.Clear();
                        life -= 1; //trækker et liv fra spilleren
                        Console.Beep(1000, 500); //spiller en lyd når spilleren gætter forkert
                        Console.WriteLine("That was wrong you have " + life + " lives left. What is the next letter?"); //beder spilleren om at gætte igen
                        DisplayHangman(life);
                        Console.WriteLine(hiddenWord.ToString()); // viser hvor mange bogstaver der er i ordet dem som er belvet gætte

                    }
                }
                
                else if (guess.Length >= 1) //hvis spilleren gætter et ord
                {
                    for (int i = 0; i < guessWord.Length; i++)//går igennem ordet som spilleren skal gætte
                    {
                        for (int j = 0; j < guess.Length; j++)//går igennem spilleren gæt)
                        { 
                            if (guessWord[i].ToString().ToLower() == guess[j].ToString().ToLower())
                            {
                            hiddenWord[i] = guess[j];//gemmer det rigtige gæt i hiddenWord
                            correctGuess = true;//sætter correctGuess til true
                            }
                        }

                    }

                    if (correctGuess) //hvis spilleren gættede rigtigt
                    {
                        Console.Clear();
                        Console.WriteLine("Some of the letters match! You have " + life + " lives left. What is the next letter?");//beder spilleren om at gætte igen
                        DisplayHangman(life);
                        Console.WriteLine(hiddenWord.ToString());// viser hvor mange bogstaver der er i ordet dem som er belvet gætte

                    }
                    else //hvis spilleren gættede forkert
                    {
                        Console.Clear();
                        life -= 2;
                        Console.Beep(1000, 500); //spiller en lyd når spilleren gætter forkert
                        Console.WriteLine("None of the letters match. You have " + life + " lives left. What is the next letter?"); //beder spilleren om at gætte igen
                        DisplayHangman(life);
                        Console.WriteLine(hiddenWord.ToString()); // viser hvor mange bogstaver der er i ordet dem som er belvet gætte
                    }


                
                }

                if (hiddenWord.ToString().ToLower() == guessWord.ToLower()) //hvis spilleren har gættet ordet
                {
                    Console.WriteLine("You guessed the word!");
                    Console.WriteLine(hiddenWord.ToString());// viser hvor mange bogstaver der er i ordet dem som er belvet gætte
                    Console.WriteLine("want to play again? press y for yes or n for no");
                    string playAgain = Console.ReadLine();
                    if (playAgain.ToLower() == "y")
                    {
                        HangMan();
                    }
                    else if (playAgain.ToLower() == "n") // hvis spilleren ikke vil spille igen og vil tilbage til menuen
                    {
                        Console.WriteLine("Thanks for playing!");
                        break;
                    }
                }
                else if (life <= 0) //hvis spilleren har mistet alle liv
                {
                    Console.WriteLine("You ran out of lives!");
                    Console.WriteLine("The word was: " + guessWord);
                    Console.WriteLine("want to play again? press y for yes or n for no");
                    string playAgain = Console.ReadLine();
                    if (playAgain.ToLower() == "y")
                    {
                        HangMan();
                    }
                    else if (playAgain.ToLower() == "n") //hvis spilleren ikke vil spille igen og vil tilbage til menuen
                    {
                        Console.WriteLine("Thanks for playing!");
                        break;
                    }
                }
            }

            void DisplayHangman(int life) //metoden som viser tegner den hangman som spilleren har gættet forkert
            {
                string[] stages =
                {
                    // final state: hele kroppen og boksen skuppes væk
                    @"
             💴💴💴💴💴
             🚪       |
             🚪       💀
             🚪      \👕/
             🚪       👖
             🚪       / \
            🧱🧱   🪜    [--]
                        ",
                    // hoved, krop, begge arme og begge ben
                    @"
             💴💴💴💴💴
             🚪       |
             🚪       😱
             🚪      \👕/
             🚪       👖
             🚪       / \
            🧱🧱   🪜[--]
                        ",
                    // hoved, krop, begge arme og et ben
                    @"
             💴💴💴💴💴
             🚪       |
             🚪       😨
             🚪      \👕/
             🚪       👖
             🚪       / 
            🧱🧱   🪜[--]
                        ",
                    // hoved, krop og begge arme
                    @"
             💴💴💴💴💴
             🚪       |
             🚪       😧
             🚪      \👕/
             🚪       👖
             🚪      
            🧱🧱   🪜[--]
                        ",
                    // hoved, krop og en arm
                    @"
             💴💴💴💴💴
             🚪        |
             🚪        😦
             🚪       \👕
             🚪        👖
             🚪      
            🧱🧱    🪜[--]
                        ",
                    // hoved og krop
                    @"
             💴💴💴💴💴
             🚪        |
             🚪        🙁
             🚪        👕
             🚪        👖
             🚪      
            🧱🧱    🪜[--]
                        ",
                    // hoved
                    @"
             💴💴💴💴💴
             🚪        |
             🚪        😐
             🚪     
             🚪     
             🚪     
            🧱🧱    🪜[--]
                        ",
                    // stolpe
                    @"
             💴💴💴💴💴
             🚪        |
             🚪      
             🚪     
             🚪      
             🚪      
            🧱🧱    🪜[--]
                        ",
                    
                    // initial empty state bakke og boks
                    @"
                          
                          
                          
                          
                          
                          
            🧱🧱    🪜[--]
                        ",
                };

                Console.WriteLine(stages[life]);
            }
        }
    }
}
