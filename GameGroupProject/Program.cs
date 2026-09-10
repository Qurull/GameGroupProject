namespace GameGroupProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string guessWord = "Jakob";
            int life = 10;

            Console.WriteLine("Welcome to Hangman, guess the word, you have "+life);


            while (life > 0)
            {
                string guess = Console.ReadLine();

                if (guess == guessWord)
                {
                    Console.WriteLine("That was rigth");
                    

                }
                else if (guess != guessWord)
                {
                    life -= 1;
                    Console.WriteLine("That was wrong " + life);
                    
                }
            }
        
        }
    }
}
