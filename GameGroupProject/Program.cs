

namespace GameGroupProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Jeopardy();
        }

        /// INGE
        /// <summary>
        /// The Jeopardy method opens the Jeopardy menu.
        /// </summary>
        static void Jeopardy()
        {
            while (true)
            {
                // clear console and reset colors
                Console.Clear();
                Console.ResetColor();

                // write out menu options
                Console.WriteLine("Welcome to Jeopardy!\n");
                Console.WriteLine("\t1: Play game");
                Console.WriteLine("\t2: Rules");
                Console.WriteLine("\t3: Credits");
                Console.WriteLine("\t4: Exit Jeopardy");

                // write out instructions
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\nInput a number from the menu and press enter.");
                Console.ResetColor();

                // read input from player
                string menuChoice = Console.ReadLine() ?? "";

                // start the game
                if (menuChoice == "1") ManageRounds();

                // read the rules
                else if (menuChoice == "2")
                {
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Jeopardy rules: \n");
                    Console.ResetColor();

                    // concept
                    Console.WriteLine("Jeopardy is a reversed quiz game where players receive the answers (clues) and then have to ask the correct questions to win points.\n");
                    Console.WriteLine("If they ask the wrong question - or forget to form their answer to the clue as a question (what is/who is/where is/etc.) - they lose points.");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to continue.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();

                    // amount of players and their buzzers
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("The Players: \n");
                    Console.ResetColor();

                    Console.WriteLine("Normally there are 3 players competing in a Jeopardy game, but in this version you can play with 1-3 players.\n\n");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Buzzers: \n");
                    Console.ResetColor();

                    Console.WriteLine("Each player gets assigned a buzzer. When a clue is revealed, whoever presses their buzzer first gets to answer with a question.\n");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\tPlayer 1 ");
                    Console.ResetColor();
                    Console.Write("will be assigned the ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("A key ");
                    Console.ResetColor();
                    Console.WriteLine("as their buzzer.");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\tPlayer 2 ");
                    Console.ResetColor();
                    Console.Write("will be assigned the ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("L key ");
                    Console.ResetColor();
                    Console.WriteLine("as their buzzer.");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("\tPlayer 3 ");
                    Console.ResetColor();
                    Console.Write("will be assigned the ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("G key ");
                    Console.ResetColor();
                    Console.WriteLine("as their buzzer.");

                    Console.WriteLine("\nIf you want to play the game comfortably, please sit according to your buzzer keys.");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to continue.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();

                    // the different rounds
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("The Different Rounds: \n");
                    Console.ResetColor();


                    Console.WriteLine("There are 3 rounds of Jeopardy: Jeopardy, Double Jeopardy and Final Jeopardy.");


                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to continue.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Jeopardy: \n");
                    Console.ResetColor();

                    Console.WriteLine("In Jeopardy, the players are presented with a board of 6 categories with 5 clues each - the clues are represented by the amount of points they're worth (200-1000 points).\n");
                    Console.WriteLine("One of the players are asked to choose a category and a clue.");
                    Console.WriteLine("After this the clue is revealed and the players can buzz in to answer with the, hopefully, correct question. If their answer is incorrect the other players will once again have a chance to answer.\n");
                    Console.WriteLine("Each clue can only be chosen once, and when there are no more clues the round ends.");


                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to continue.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Double Jeopardy: \n");
                    Console.ResetColor();

                    Console.WriteLine("In Double Jeopardy, the players are presented with a similar board and the same rules as in Jeopardy - but this time the clues are worth more points (400-2000).");


                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to continue.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Final Jeopardy: \n");
                    Console.ResetColor();

                    Console.WriteLine("In Final Jeopardy, the rules are different. And only players who have gathered more than 0 points can participate in this round.\n");
                    Console.WriteLine("A random category is chosen and each player wages an amount of their points - at least 1 point, at most all their points.");
                    Console.WriteLine("The final clue is then revealed and each player writes down their answer (in the form of a question).");
                    Console.WriteLine("The players that answer correctly will add the points they wagered to their current amount of points.");
                    Console.WriteLine("The players that answer incorrectly will lose the points they wagered.");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to continue.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();

                    // daily doubles
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Daily Doubles: \n");
                    Console.ResetColor();


                    Console.WriteLine("In the Jeopardy and Double Jeopardy rounds, the players can run into Daily Doubles.\n");
                    Console.WriteLine("At the beginning of the round, one of the clues are chosen to be a Daily Double.\n");
                    Console.WriteLine("This means that, instead of all the players buzzing in to answer, the player who chose the clue gets to choose an amount of points to wager before the clue is revealed.");
                    Console.WriteLine("Once the clue is revealed only that player gets to answer (with a question).\n");
                    Console.WriteLine("If they answer correctly, they earn the amount of points they wagered.");
                    Console.WriteLine("If they answer incorrectly, they lose the amount of points they wagered.\n");
                    Console.WriteLine("In Double Jeopardy, two of the clues will be a Daily Double.");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to go back to the menu.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();
                }

                // credits
                else if (menuChoice == "3")
                {
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Credits: \n");
                    Console.ResetColor();

                    Console.WriteLine("\tProgrammed by Inge DTS.");
                    Console.WriteLine("\tCategories, clues and answers written by Louise and Chris.");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to go back to the menu.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();
                }

                // exit game
                else if (menuChoice == "4")
                {
                    break;
                }

                // invalid input
                else
                {
                    InvalidInput();
                }
            }

            // methods vvv

            /// <summary>
            /// The ManageRounds method sets up the game and manages which round is being played.
            /// </summary>
            static void ManageRounds()
            {
                Console.Clear();
                Console.ResetColor();

                // player information
                string[] playerName = { "Player 1", "Player 2", "Player 3" };
                ConsoleColor[] playerColor = { ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Blue };
                ConsoleKey[] playerBuzzer = { ConsoleKey.A, ConsoleKey.L, ConsoleKey.G };
                bool[] playerAllowed = { false, false, false }; // to allow/disallow players from participating in BuzzIn or FinalJeopardy
                int[] playerPoints = { 0, 0, 0 };

                // game setup
                // ask for the amount of players until getting a valid input
                int amountOfPlayers = 0;

                while (amountOfPlayers == 0)
                {
                    Console.WriteLine("How many will be playing? (1-3)");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\nInput a number from 1-3 and press enter.");
                    Console.ResetColor();

                    if (int.TryParse(Console.ReadLine(), out int amount))
                    {
                        if (amount > 0 && amount < 4) amountOfPlayers = amount;
                    }
                }

                // ask players for their name
                EnterName(ref playerName[0], playerBuzzer[0], playerColor[0]);
                playerAllowed[0] = true;

                if (amountOfPlayers > 1)
                {
                    EnterName(ref playerName[1], playerBuzzer[1], playerColor[1]);
                    playerAllowed[1] = true;

                    if (amountOfPlayers > 2)
                    {
                        EnterName(ref playerName[2], playerBuzzer[2], playerColor[2]);
                        playerAllowed[2] = true;
                    }
                }

                // quiz content for the Jeopardy round
                string[] jeopardyCategories = { "Music", "Science", "Movies", "Sports", "History", "Food" };
                string[,] jeopardyClues =
                {
                {"She’s been called “The Queen of Pop” and had hits like “Like A Prayer” and “Vogue”.", "He composed music for movies like The Pirates of the Carribbeans, The Lion King, Gladiator and Interstellar.", "She played Glinda in Wicked opposite Cynthia Erivo as Elphaba.", "Her full name is Stefani Joanne Angelina Germanotta.", "Beyoncé surprise-released this album in 2016." },
                {"He was credited with the invention of the theory of relativity.", "It makes your voice highpitched when you inhale it, and it makes balloons float.", "Fundamental building blocks of matter.", "Buzz Aldrin and Neil Armstrong landed here in 1969.", "3,14159" },
                {"In this Tim Burton movie five kids visit a factory of a peculiar chocolatier.", "This movie is famous for “the lift” and the naughty dancing.", "In this movie Olivia Newton-John gets a makeover in the end to impress John Travolta.", "John Travolta plays a woman and sings in this movie from 2007.", "Regina George is hit by a bus in this movie from 2004." },
                {"This state has Fenway Park,  the oldest baseball stadium in the US.", "The 2024 Summer Olympics were  held in this city.", "This team has the most Super Bowl wins.", "This football player just got married to Taylor Swift.", "This soccer player married a Spice Girl." },
                {"She was quoted saying: “Let them eat cake”.", "A prominent leader of the civil rights movement from 1955 until his assassination in 1968.", "His middle name was Fitzgerrald and he was the president of the United States from 1961 until his assination in 1963.", "He was stabbed at least 23 times.", "This king was married six times and ordered the beheadings of two of his wives." },
                {"Fries and chips are made out of this vegetable.", "Samwise Gamgee from Lord of the Rings is very passionate about this vegetable.", "Sometimes used to brew alcoholic spirits such as vodka, poitín, akvavit, and brännvin.", "There are at least 16 museums dedicated to this vegetable.", "Mads Mikkelsen tries to plant and harvest this in the movie The Promised Land from 2023." } };
                string[,] jeopardyAnswers =
                {
                {"Who is Madonna?", "Who is Hans Zimmer?", "Who is Ariana Grande?", "Who is Lady Gaga?", "What is Lemonade?" },
                {"Who is Albert Einstein?", "What is helium?", "What are atoms?", "What is the moon?", "What is pi?" },
                {"What is Charlie and the Chocolate Factory?", "What is Dirty Dancing?", "What is Grease?", "What is Hairspray?", "What is Mean Girls?" },
                {"What is Massachusetts?", "What is Paris?", "Who are The New England Patriots?", "Who is Travis Kelce?", "Who is David Beckham?" },
                {"Who is Marie Antoinette?", "Who is Martin Luther King Jr.?", "Who is John F. Kennedy?", "Who is Julius Caesar?", "Who is King Henry VIII?" },
                {"What are potatoes?", "What are potatoes?", "What are potatoes?", "What are potatoes?", "What are potatoes?" } };

                // start the Jeopardy round
                PlayJeopardy(playerName, playerColor, playerBuzzer, ref playerAllowed, ref playerPoints, amountOfPlayers, 200, jeopardyCategories, ref jeopardyClues, jeopardyAnswers);

                // after the Jeopardy round:
                Console.Clear();
                Console.ResetColor();

                // ask if they want to continue with the double jeopardy round
                Console.WriteLine("Do you want to continue and play the Double Jeopardy round?\n");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("If no: press tab.");
                Console.WriteLine("If yes: press any other key.");
                Console.ResetColor();

                if (Console.ReadKey().Key != ConsoleKey.Tab)
                {
                    // quiz content for the Double Jeopardy round
                    string[] doubleJeopardyCategories = { "Lyrics", "Politics", "Television", "Literature", "Famous People", "Christmas" };
                    string[,] doubleJeopardyClues =
                    {
                {"They sang: “I am the walrus”.", "She sang: “I’m a genie in a bottle, you gotta rub me the right way”.", "He sang: “You can tell everybody that this is your song”.", "She sang: “And I will always love you”.", "He sang: “I thought you’d always be mine”." },
                {"Vice president of The United States.", "First black president of The United States of America.", "He was the US president during the 9/11 attacks.", "She was the longest-serving British prime minister of the 20th century and the first woman to hold the office.", "Gated street in Westminster in London that houses the official residences and offices of the Prime Minister of the United Kingdom." },
                {"This guy is yellow and his best friend is a starfish.", "She and her friends, Freddy and Sam make a webshow named after her.", "In this show Jesse and Walter White cook meth.", "Lena Dunham wrote and starred in this show from the 2010s.", "Friend to Ross, Monica, Rachel, Joey and Phoebe." },
                {"His first publication was “Carrie” and he has since published more than 60 titles.", "She published Frankenstein when she was 20 years old.", "She invented the character Mr. Darcy.", "Transylvanian count written about by Bram Stoker.", "In this book 17 year old Bella falls in love with Edward and finds out that he’s a vampire." },
                {"Her Spice Girl name before she got married to David Beckham.", "She is a sister to Bella and  she has a daughter with Zayn Malik.", "She loves auto-tune and she plays Meryl Streep's mother in the movie Mama Mia! Here We Go Again.", "He was in The Social Network and Call Me By Your Name but he also got famous for sexual assault and cannibalism.", "He has been portrayed on screen by Jesse Eisenberg and now Jeremy Strong. He likes blue." },
                {"This song has been played on the radio every Christmas since 1994 and is sung by Mariah Carey.", "“Last Christmas” is sung by this English duo.", "If not a star, this is typically at the top of the christmas tree.", "These animals pull Santa’s sleigh.", "You might get this in your sock if you have been naughty." } };
                    string[,] doubleJeopardyAnswers =
                    {
                {"Who are The Beatles?", "Who is Christina Aguilera?", "Who is Elton John?", "Who is Whitney Houston?", "Who is Justin Bieber?" },
                {"Who is JD Vance?", "Who is Barack Obama?", "Who is George W Bush?", "Who is Margaret Thatcher?", "What is Downing Street?" },
                {"Who is Spongebob Squarepants?", "Who is Carly from iCarly?", "What is Breaking Bad?", "What is Girls?", "Who is Chandler Bing?" },
                {"Who is Stephen King?", "Who is Mary Shelley?", "Who is Jane Austen?", "Who is Dracula?", "What is Twilight?" },
                {"Who is Posh Spice?", "Who is Gigi Hadid?", "Who is Cher?", "Who is Armie Hammer?", "Who is Mark Zuckerberg?" },
                {"What is All I Want for Christmas is You?", "Who is Wham!?", "What is an angel?", "What are reindeers?", "What is coal?" } };

                    // start the Double Jeopardy round
                    PlayJeopardy(playerName, playerColor, playerBuzzer, ref playerAllowed, ref playerPoints, amountOfPlayers, 400, doubleJeopardyCategories, ref doubleJeopardyClues, doubleJeopardyAnswers);
                }

                // after the Double Jeopardy round:
                Console.Clear();
                Console.ResetColor();

                // ask if they want to continue with final jeopardy round
                Console.WriteLine("Do you want to continue with the Final Jeopardy round?\n");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("If no: press tab.");
                Console.WriteLine("If yes: press any other key.");
                Console.ResetColor();

                if (Console.ReadKey().Key != ConsoleKey.Tab)
                {
                    // start Final Jeopardy round
                    PlayFinalJeopardy(playerName, playerColor, ref playerAllowed, ref playerPoints);
                }

                // after the Final Jeopardy round
                Console.Clear();
                Console.ResetColor();

                // display who won the game
                ShowPoints(playerColor, playerName, playerPoints, amountOfPlayers);

                int winner = MostPoints(playerPoints);

                if (winner == -1)
                {
                    Console.WriteLine("\n\nCongratulation! It's a tie!");
                }
                else
                {
                    Console.Write("\nCongratulations, ");
                    Console.ForegroundColor = playerColor[winner];
                    Console.Write(playerName[winner]);
                    Console.ResetColor();
                    Console.WriteLine("! You won!");
                }

                Console.WriteLine("\n\n\nThank you for playing Jeopardy!");

                // go back to jeopardy menu
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\nPress any key to ´return to the menu.");
                Console.ResetColor();
                Console.ReadKey();

                Console.Clear();
            }

            /// <summary>
            /// The PlayJeopardy method contains the Jeopardy and Double Jeopardy game loop.
            /// </summary>
            /// <param name="playerName">
            /// Array of the players' chosen names.
            /// </param>
            /// <param name="playerColor">
            /// Array of the players' colors.
            /// </param>
            /// <param name="playerBuzzer">
            /// Array of the players' assigned buzzer-keys.
            /// </param>
            /// <param name="playerAllowed">
            /// Array of whether each player is allowed to participate in BuzzIn.
            /// </param>
            /// <param name="playerPoints">
            /// Array of the players' points.
            /// </param>
            /// <param name="amountOfPlayers">
            /// The amount of players in the game.
            /// </param>
            /// <param name="lowestCluePointAmount">
            /// The lowest amount of points a clue can give in the round (200 for Jeopardy/400 for Double Jeopardy)
            /// </param>
            /// <param name="categories">
            /// Array of categories for the round.
            /// </param>
            /// <param name="clues">
            /// Array of clues for the round.
            /// </param>
            /// <param name="answers">
            /// Array of correct answers for the round.
            /// </param>
            static void PlayJeopardy(string[] playerName, ConsoleColor[] playerColor, ConsoleKey[] playerBuzzer, ref bool[] playerAllowed, ref int[] playerPoints, int amountOfPlayers, int lowestCluePointAmount, string[] categories, ref string[,] clues, string[,] answers)
            {
                Console.Clear();
                Console.ResetColor();

                // choose daily doubles clue
                Random random = new Random();

                int dailyDoubleCategory = random.Next(0, 6);
                int dailyDoubleClue = random.Next(0, 5);

                int doubleDailyDoubleCategory = -1;
                int doubleDailyDoubleClue = -1;

                // if it's the Double Jeopardy round choose another daily double
                if (lowestCluePointAmount == 400)
                {
                    while (true)
                    {
                        doubleDailyDoubleCategory = random.Next(0, 6);
                        doubleDailyDoubleClue = random.Next(0, 5);

                        if (doubleDailyDoubleCategory == dailyDoubleCategory && doubleDailyDoubleClue == dailyDoubleClue)
                        {
                            continue;
                        }
                        else break;
                    }
                }

                // decide who goes first
                Console.WriteLine("Use your buzzer to choose who goes first!\n\n");

                int clueChooser;

                if (amountOfPlayers > 1)
                {
                    if (lowestCluePointAmount == 200) clueChooser = BuzzIn(playerName, playerBuzzer, playerAllowed, true);
                    else if (lowestCluePointAmount == 400)
                    {
                        // find the player with the least points,
                        // if there's a tie it will be the last player with that amount of points on the list
                        int leastPoints = playerPoints[0];
                        clueChooser = 0;

                        for (int i = 0; i < amountOfPlayers; i++)
                        {
                            if (playerPoints[i] <= leastPoints)
                            {
                                leastPoints = playerPoints[i];
                                clueChooser = i;
                            }
                        }
                    }
                    else clueChooser = 0;
                }
                else clueChooser = 0;

                // Jeopardy/Double Jeopardy game loop
                while (true)
                {
                    Console.Clear();
                    Console.ResetColor();

                    // check if there are any clues left - if not end the round
                    bool cluesLeft = true;

                    foreach (string clue in clues)
                    {
                        if (clue != "")
                        {
                            cluesLeft = true;

                            break;
                        }
                        else cluesLeft = false;
                    }

                    if (!cluesLeft) break;

                    // allow all players to buzz in for the upcoming clue
                    playerAllowed[0] = true;

                    if (amountOfPlayers > 1)
                    {
                        playerAllowed[1] = true;

                        if (amountOfPlayers > 2)
                        {
                            playerAllowed[2] = true;
                        }
                    }

                    // write out points
                    ShowPoints(playerColor, playerName, playerPoints, amountOfPlayers);

                    Console.ResetColor();
                    Console.WriteLine("\n\n");

                    // write out categories
                    for (int i = 0; i < categories.Length; i++)
                    {
                        Console.Write(categories[i] + "       ");
                    }

                    Console.WriteLine("\n");

                    // write out the rest of the board - gray out clues that have been picked before
                    for (int col = 0; col < clues.GetLength(1); col++)
                    {
                        for (int row = 0; row < clues.GetLength(0); row++)
                        {
                            if (clues[row, col] == "") Console.ForegroundColor = ConsoleColor.DarkGray;

                            int cluePointAmount = (col + 1) * lowestCluePointAmount;

                            if (lowestCluePointAmount == 200)
                            {
                                Console.Write(" " + cluePointAmount + (cluePointAmount < 1000 ? "         " : "        "));
                            }
                            if (lowestCluePointAmount == 400)
                            {
                                Console.Write(" " + cluePointAmount + (cluePointAmount < 1000 ? "              " : "             "));
                            }
                            Console.ResetColor();
                        }
                        Console.WriteLine("\n");
                    }

                    Console.WriteLine("\n");

                    // clueChooser chooses category
                    Console.ForegroundColor = playerColor[clueChooser];
                    Console.Write(playerName[clueChooser]);
                    Console.ResetColor();
                    Console.WriteLine(", please choose a category.");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Input the category name or number and press enter.");
                    Console.ResetColor();

                    string chosenCategory = Console.ReadLine() ?? "";

                    int categoryNumber = -1;

                    // check if categories match name
                    for (int i = 0; i < categories.Length; i++)
                    {
                        if (chosenCategory.ToUpper().Trim() == categories[i].ToUpper())
                        {
                            categoryNumber = i;
                            break;
                        }
                    }

                    // check if categories match number
                    if (categoryNumber == -1 && int.TryParse(chosenCategory, out int result))
                    {
                        if (!(result < 1) && !(result > 6))
                        {
                            categoryNumber = result - 1;
                        }
                    }

                    // if categories did not match, make player try again
                    if (categoryNumber == -1)
                    {
                        InvalidInput("category");
                        continue;
                    }

                    // clueChooser chooses clue
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\nChosen category: " + categories[categoryNumber]);
                    Console.ResetColor();

                    Console.ForegroundColor = playerColor[clueChooser];
                    Console.Write(playerName[clueChooser]);
                    Console.ResetColor();
                    Console.WriteLine(", please choose the clue you want.");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Input the point-amount of the clue you want and press enter.");
                    Console.ResetColor();

                    string chosenClue = Console.ReadLine() ?? "";

                    // check if input is valid
                    if (int.TryParse(chosenClue, out int clueNumber))
                    {
                        if (lowestCluePointAmount == 200)
                        {
                            switch (clueNumber)
                            {
                                case 200:
                                    clueNumber = 0;
                                    break;
                                case 400:
                                    clueNumber = 1;
                                    break;
                                case 600:
                                    clueNumber = 2;
                                    break;
                                case 800:
                                    clueNumber = 3;
                                    break;
                                case 1000:
                                    clueNumber = 4;
                                    break;
                                default:
                                    InvalidInput("clue");
                                    continue;
                            }
                        }
                        else if (lowestCluePointAmount == 400)
                        {
                            switch (clueNumber)
                            {
                                case 400:
                                    clueNumber = 0;
                                    break;
                                case 800:
                                    clueNumber = 1;
                                    break;
                                case 1200:
                                    clueNumber = 2;
                                    break;
                                case 1600:
                                    clueNumber = 3;
                                    break;
                                case 2000:
                                    clueNumber = 4;
                                    break;
                                default:
                                    InvalidInput("clue");
                                    continue;
                            }
                        }
                    }
                    else
                    {
                        InvalidInput("clue");
                        continue;
                    }

                    // check if clue has already been picked
                    if (clues[categoryNumber, clueNumber] == "")
                    {
                        InvalidInput("clue");
                        continue;
                    }

                    Console.Clear();

                    // answer "screen"
                    while (true)
                    {
                        int answeringPlayer;

                        // Daily Double
                        if (categoryNumber == dailyDoubleCategory && clueNumber == dailyDoubleClue || lowestCluePointAmount == 400 && categoryNumber == doubleDailyDoubleCategory && clueNumber == doubleDailyDoubleClue)
                        {
                            answeringPlayer = clueChooser;

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Daily Double!\n");
                            Console.ResetColor();

                            // display category
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(categories[categoryNumber] + " for " + chosenClue + "\n\n");
                            Console.ResetColor();

                            // make wager
                            Console.ForegroundColor = playerColor[answeringPlayer];
                            Console.Write(playerName[answeringPlayer]);
                            Console.ResetColor();
                            Console.WriteLine(", you get to make a wager:\n");

                            int dailyDoubleWager = WagePoints(playerName[answeringPlayer], playerColor[answeringPlayer], playerPoints[answeringPlayer], clues.GetLength(1) * lowestCluePointAmount);

                            Console.Clear();
                            Console.ResetColor();

                            // display wager and category
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("You have wagered " + dailyDoubleWager + " points on the category " + categories[categoryNumber] + " for " + chosenClue + ".\n");
                            Console.ResetColor();

                            // display clue
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("Your clue is: ");
                            Console.ResetColor();
                            Console.WriteLine(clues[categoryNumber, clueNumber]);
                        }
                        else
                        {
                            // check if there are more than one person left to answer
                            int buzzersLeft = BuzzersLeft(playerAllowed);

                            if (buzzersLeft < 1)
                            {
                                Console.Clear();

                                // display category
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine(categories[categoryNumber] + " for " + chosenClue + "\n");
                                Console.ResetColor();

                                // display clue
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine("The clue was: ");
                                Console.ResetColor();
                                Console.WriteLine(clues[categoryNumber, clueNumber]);

                                // display answer
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine("\nThe correct answer is: ");
                                Console.ResetColor();
                                Console.WriteLine(answers[categoryNumber, clueNumber]);

                                // go back to board
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine("\n\nPress any key to continue.");
                                Console.ResetColor();
                                Console.ReadKey();
                                break;
                            }

                            Console.Clear();

                            // display category
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(categories[categoryNumber] + " for " + chosenClue + "\n");
                            Console.ResetColor();

                            // display clue
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("The clue is: ");
                            Console.ResetColor();
                            Console.WriteLine(clues[categoryNumber, clueNumber] + "\n");

                            // pick which player who's gonna answer the question
                            if (buzzersLeft >= 2)
                            {
                                answeringPlayer = BuzzIn(playerName, playerBuzzer, playerAllowed);
                            }
                            else
                            {
                                answeringPlayer = LastBuzzer(playerAllowed);

                                // don't force player to answer (unless they are playing alone)
                                if (amountOfPlayers > 1)
                                {
                                    Console.ForegroundColor = playerColor[answeringPlayer];
                                    Console.Write(playerName[answeringPlayer]);
                                    Console.ResetColor();
                                    Console.WriteLine(", do you want to answer?");

                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.WriteLine("Press tab if no, press any other key if yes.");
                                    Console.ResetColor();

                                    if (Console.ReadKey().Key == ConsoleKey.Tab) break;
                                }
                            }
                        }

                        // go back to board if no player has an answer
                        if (answeringPlayer == -1) break;

                        // ask for the player to answer
                        string answerInput = WriteAnswer(playerName[answeringPlayer], playerColor[answeringPlayer]);

                        // check if the answer is correct
                        bool correctAnswer = CorrectAnswer(answerInput, answers[categoryNumber, clueNumber]);

                        // inform player if their answer is correct
                        Console.Clear();

                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("Your answer was: " + answerInput + ".\n");
                        Console.ResetColor();

                        Console.ForegroundColor = correctAnswer ? ConsoleColor.Green : ConsoleColor.Magenta;
                        Console.WriteLine("Your answer is " + (correctAnswer ? "CORRECT" : "WRONG") + "!");
                        Console.ResetColor();

                        // add/subtract points
                        if (correctAnswer)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("+ " + chosenClue);
                            Console.ResetColor();

                            playerPoints[answeringPlayer] += int.Parse(chosenClue);

                            clueChooser = answeringPlayer;

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\n\nPress any key to continue.");
                            Console.ResetColor();

                            Console.ReadKey();

                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("- " + chosenClue);
                            Console.ResetColor();

                            playerPoints[answeringPlayer] -= int.Parse(chosenClue);

                            playerAllowed[answeringPlayer] = false;

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\n\nPress any key to continue.");
                            Console.ResetColor();

                            Console.ReadKey();
                        }

                        // if it was a daily double clue or if there's only one player: go back to the board without repeating anything
                        if (categoryNumber == dailyDoubleCategory && clueNumber == dailyDoubleClue || lowestCluePointAmount == 400 && categoryNumber == doubleDailyDoubleCategory && clueNumber == doubleDailyDoubleClue)
                        {
                            Console.Clear();

                            // display category
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(categories[categoryNumber] + " for " + chosenClue + "\n");
                            Console.ResetColor();

                            // display clue
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("The clue was: ");
                            Console.ResetColor();
                            Console.WriteLine(clues[categoryNumber, clueNumber]);

                            // display answer
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\nThe correct answer is: ");
                            Console.ResetColor();
                            Console.WriteLine(answers[categoryNumber, clueNumber]);

                            // go back to board
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\n\nPress any key to continue.");
                            Console.ResetColor();
                            Console.ReadKey();

                            break;
                        }
                    }

                    // set the clue to be an empty string (grayed out and invalid choice)
                    clues[categoryNumber, clueNumber] = "";

                    Console.Clear();
                    Console.ResetColor();

                    // continue/end game
                    Console.WriteLine("If you want to end the round, press tab.");
                    Console.WriteLine("Press any other key to continue.");

                    if (Console.ReadKey().Key == ConsoleKey.Tab)
                    {
                        break;
                    }
                }
            }

            /// <summary>
            /// The PlayFinalJeopardy method contains the Final Jeopardy round.
            /// </summary>
            /// <param name="playerName">
            /// Array of the players' chosen names.
            /// </param>
            /// <param name="playerColor">
            /// Array of the players' colors.
            /// </param>
            /// <param name="playerAllowed">
            /// Array of whether each player is allowed to participate in BuzzIn.
            /// </param>
            /// <param name="playerPoints">
            /// Array of the players' points.
            /// </param>
            static void PlayFinalJeopardy(string[] playerName, ConsoleColor[] playerColor, ref bool[] playerAllowed, ref int[] playerPoints)
            {
                Console.Clear();

                // check if players have more than 0 points - exclude the ones that don't
                for (int i = 0; i < playerPoints.Length; i++)
                {
                    if (playerPoints[i] > 0)
                    {
                        playerAllowed[i] = true;
                    }
                    else playerAllowed[i] = false;
                }

                // display which players will play in the Final Jeopardy round
                Console.WriteLine("The following players qualify for the Final Jeopardy round: \n");

                int playersLeft = 0;

                for (int i = 0; i < playerAllowed.Length; i++)
                {
                    if (playerAllowed[i])
                    {
                        playersLeft += 1;
                        Console.ForegroundColor = playerColor[i];
                        Console.WriteLine("\t" + playerName[i]);
                        Console.ResetColor();
                    }
                }

                if (playersLeft < 1)
                {
                    Console.WriteLine("\tNo player qualifies.");
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\nPress any key to continue.");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();

                if (playersLeft > 0)
                {
                    // content for Final Jeopardy round
                    string[,] clues = { { "Geography", "Video games", "Language" }, { "Island country east of Australia where The Lord of the Rings were filmed.", "He’s an Italian plumber.", "The Inuit in Greenland are believed to have over 40 words for this weather phenomenon." }, { "What is New Zealand?", "Who is Super Mario?", "What is snow?" } };

                    // choose a random final clue
                    Random random = new Random();
                    int finalClue = random.Next(0, clues.GetLength(1));

                    // let players wager points
                    int[] playerWagers = { 0, 0, 0 };

                    for (int i = 0; i < playerAllowed.Length; i++)
                    {
                        if (playerAllowed[i])
                        {
                            Console.WriteLine("The final category is " + clues[0, finalClue] + ".\n\n");

                            Console.Write("Everyone, except ");
                            Console.ForegroundColor = playerColor[i];
                            Console.Write(playerName[i]);
                            Console.ResetColor();
                            Console.Write(", should look away now.\n");

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(playerName[i] + ", press any key when you are ready to make a wager.");
                            Console.ResetColor();
                            Console.ReadKey();

                            Console.Clear();
                            playerWagers[i] = WagePoints(playerName[i], playerColor[i], playerPoints[i]);

                            Console.Clear();
                            Console.WriteLine("You have wagered " + playerWagers[i] + " points.");

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\n\nPress any key to continue.");
                            Console.ResetColor();

                            Console.ReadKey();

                            Console.Clear();
                        }
                    }

                    // let players answer
                    string[] playerAnswers = { "", "", "" };

                    for (int i = 0; i < playerAllowed.Length; i++)
                    {
                        if (playerAllowed[i])
                        {
                            // display category and clue
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("The final category is " + clues[0, finalClue] + ".\n");
                            Console.ResetColor();

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("The final clue is: ");
                            Console.ResetColor();
                            Console.WriteLine(clues[1, finalClue]);

                            // all players write their answer
                            Console.Write("\n\nEveryone, except ");
                            Console.ForegroundColor = playerColor[i];
                            Console.Write(playerName[i]);
                            Console.ResetColor();
                            Console.Write(", should look away now.\n");

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(playerName[i] + ", press any key when you are ready to write your answer.");
                            Console.ResetColor();
                            Console.ReadKey();

                            playerAnswers[i] = WriteAnswer(playerName[i], playerColor[i]);

                            Console.Clear();
                            Console.WriteLine("You have answered " + playerAnswers[i]);

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\n\nPress any key to continue.");
                            Console.ResetColor();

                            Console.ReadKey();

                            Console.Clear();
                        }
                    }

                    // all the answers and whether they are correct are revealed one by one
                    Console.WriteLine("Now the answers will be revealed: \n");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Press any key to reveal the next player's answer.\n\n");
                    Console.ResetColor();

                    for (int i = 0; i < playerAllowed.Length; i++)
                    {
                        if (playerAllowed[i])
                        {
                            Console.ReadKey();

                            Console.ForegroundColor = playerColor[i];
                            Console.Write(playerName[i]);
                            Console.ResetColor();
                            Console.Write(": " + playerAnswers[i] + ".");

                            Console.ReadKey();

                            if (CorrectAnswer(playerAnswers[i], clues[2, finalClue]))
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("   CORRECT");
                                Console.ResetColor();

                                playerPoints[i] += playerWagers[i];
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine("   INCORRECT");
                                Console.ResetColor();

                                playerPoints[i] -= playerWagers[i];
                            }
                        }
                    }

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to continue.");
                    Console.ResetColor();
                    Console.ReadKey();

                    Console.Clear();

                    // reveal how many points each player wagered on the final clue
                    Console.WriteLine("Points wagered: \n");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Press any key to reveal the next player's wager.\n\n");
                    Console.ResetColor();

                    for (int i = 0; i < playerAllowed.Length; i++)
                    {
                        if (playerAllowed[i])
                        {
                            Console.ReadKey();

                            Console.ForegroundColor = playerColor[i];
                            Console.Write(playerName[i]);
                            Console.ResetColor();
                            Console.WriteLine(" wagered " + playerWagers[i] + " of their points.");
                        }
                    }
                }

                // end final jeopardy
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\nPress any key to end the game and see the results.");
                Console.ResetColor();

                Console.ReadKey();
            }

            /// <summary>
            /// The InvalidInput method informs the player that their input is invalid.
            /// </summary>
            /// <param name="input">
            /// What kind of input that is invalid. Default is "input".
            /// </param>
            static void InvalidInput(string input = "input")
            {
                Console.WriteLine("\nInvalid " + input + ".");

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\n\nPress any key to continue.");
                Console.ResetColor();

                Console.ReadKey();
            }

            /// <summary>
            /// The EnterName method informs the players' of their buzzer-key and asks them to write their name.
            /// </summary>
            /// <param name="player">A specific player's name/alias.</param>
            /// <param name="buzzer">A specific player's assigned buzzer-key.</param>
            /// <param name="color">A specific player's color.</param>
            static void EnterName(ref string player, ConsoleKey buzzer, ConsoleColor color)
            {
                while (true)
                {
                    Console.Clear();

                    // display assigned buzzer
                    Console.ForegroundColor = color;
                    Console.Write(player);
                    Console.ResetColor();
                    Console.Write(" will have the ");
                    Console.ForegroundColor = color;
                    Console.Write(buzzer + "-key");
                    Console.ResetColor();
                    Console.WriteLine(" as their buzzer.");

                    // ask for the player's name
                    Console.WriteLine("\nPlease write the name of " + player + ".");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Write your name and press enter.");

                    string name = Console.ReadLine() ?? "";

                    if (name != "")
                    {
                        player = name;
                        break;
                    }
                    else InvalidInput();
                }
            }

            /// <summary>
            /// The BuzzersLeft method checks how many players are allowed to BuzzIn.
            /// </summary>
            /// <returns>
            /// Returns an integer of how many players are allowed to BuzzIn.
            /// </returns>
            /// <param name="playerAllowed">
            /// An array of booleans determining which players are allowed to BuzzIn.
            /// </param>
            static int BuzzersLeft(bool[] playerAllowed)
            {
                int buzzers = 0;

                foreach (bool buzzerAllowed in playerAllowed)
                {
                    if (buzzerAllowed)
                    {
                        buzzers += 1;
                    }
                }

                return buzzers;
            }

            /// <summary>
            /// The LastBuzzer method checks which player is the last one allowed to BuzzIn.
            /// </summary>
            static int LastBuzzer(bool[] playerAllowed)
            {
                int lastBuzzer = -1;

                for (int i = 0; i < playerAllowed.Length; i++)
                {
                    if (playerAllowed[i])
                    {
                        lastBuzzer = i;
                        return lastBuzzer;
                    }
                }

                return lastBuzzer;
            }

            /// <summary>
            /// The BuzzIn method asks the players to press their buzzer-key and returns the winner.
            /// </summary>
            /// <returns>
            /// Returns the fastest player to press their buzzer-key.
            /// </returns>
            /// <param name="playerName">
            /// Array of the players' names.
            /// </param>
            /// <param name="playerBuzzer">
            /// Array of the players' buzzer-keys.
            /// </param>
            /// <param name="playerAllowed">
            /// Array of whether the players' are allowed to BuzzIn.
            /// </param>
            /// <param name="clueChoice">
            /// Whether the players' are buzzing in to answer a clue or not. Default is false.
            /// </param>
            static int BuzzIn(string[] playerName, ConsoleKey[] playerBuzzer, bool[] playerAllowed, bool clueChoice = false)
            {
                int buzzWinner;

                Console.WriteLine("\nGet ready to buzz in!");

                // remind players of their buzzer-keys
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\nRemember your buzz-keys: \n");
                Console.ResetColor();

                for (int i = 0; i < playerName.Length; i++)
                {
                    if (playerAllowed[i])
                    {
                        Console.Write(playerName[i]);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(", your buzzer-key is ");
                        Console.ResetColor();
                        Console.WriteLine(playerBuzzer[i]);
                    }
                }

                Console.WriteLine("\n\nYou can buzz in after the beep.");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("If you don't hear a beep, you can buzz in when 'BUZZ IN NOW' is written in green.");


                Console.WriteLine("\n\nWhen you are all ready to buzz in, press any key to continue.");
                Console.ResetColor();

                Console.ReadKey();

                // let players buzz in
                Console.Beep(700, 1000);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("BUZZ IN NOW");
                Console.ResetColor();

                // let players skip the question if none of them have an answer
                if (!clueChoice)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("If none of you know the answer, you can skip answering by pressing tab.");
                    Console.ResetColor();
                }

                // find out who buzzed in the fastest
                while (true)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey();

                    if (pressedKey.Key == playerBuzzer[0] && playerAllowed[0])
                    {
                        buzzWinner = 0;
                        break;
                    }
                    else if (pressedKey.Key == playerBuzzer[1] && playerAllowed[1])
                    {
                        buzzWinner = 1;
                        break;
                    }
                    else if (pressedKey.Key == playerBuzzer[2] && playerAllowed[2])
                    {
                        buzzWinner = 2;
                        break;
                    }
                    else if (pressedKey.Key == ConsoleKey.Tab && !clueChoice)
                    {
                        buzzWinner = -1;
                        break;
                    }
                    else continue;

                }

                return buzzWinner;

            }

            /// <summary>
            /// The WriteAnswer method asks the player to write their answer to a clue and outputs their answer.
            /// </summary>
            /// <returns>
            /// Returns the player's answer to the clue.
            /// </returns>
            /// <param name="playerName">
            /// The specific player's name.
            /// </param>
            /// <param name="playerColor">
            /// The specific player's color.
            /// </param>
            static string WriteAnswer(string playerName, ConsoleColor playerColor)
            {
                Console.WriteLine("\n");
                Console.ForegroundColor = playerColor;
                Console.Write(playerName);
                Console.ResetColor();
                Console.WriteLine(", please write your answer and press enter.");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Remember to form your answer as a question (what is/what are/who is/who are/etc).");
                Console.ResetColor();

                return Console.ReadLine() ?? "";
            }

            /// <summary>
            /// The CorrectAnswer method checks whether the player's answer is correct or incorrect.
            /// </summary>
            /// <returns>
            /// Returns true if the player's answer is correct and false if incorrect.
            /// </returns>
            /// <param name="answerInput">
            /// The player's answer.
            /// </param>
            /// <param name="correctAnswer">
            /// The correct answer.
            /// </param>
            static bool CorrectAnswer(string answerInput, string correctAnswer)
            {
                if (answerInput.ToUpper().Trim() == correctAnswer.ToUpper())
                {
                    return true;
                }
                else return false;
            }

            /// <summary>
            /// The ShowPoints method displays how many points each player has earned.
            /// </summary>
            /// <param name="playerColor">
            /// An array of the players' colors.
            /// </param>
            /// <param name="playerName">
            /// An array of the players' names.
            /// </param>
            /// <param name="playerPoints">
            /// An array of the players' points.
            /// </param>
            /// <param name="amountOfPlayers">
            /// The amount of players in the game.
            /// </param>
            static void ShowPoints(ConsoleColor[] playerColor, string[] playerName, int[] playerPoints, int amountOfPlayers)
            {
                Console.ForegroundColor = playerColor[0];
                Console.Write("\t\t" + playerName[0]);
                Console.ResetColor();
                Console.Write(": " + playerPoints[0] + " pts");

                if (amountOfPlayers > 1)
                {
                    Console.ForegroundColor = playerColor[1];
                    Console.Write("\t" + playerName[1]);
                    Console.ResetColor();
                    Console.Write(": " + playerPoints[1] + " pts");
                    if (amountOfPlayers > 2)
                    {
                        Console.ForegroundColor = playerColor[2];
                        Console.Write("\t" + playerName[2]);
                        Console.ResetColor();
                        Console.Write(": " + playerPoints[2] + " pts");
                    }
                }
            }

            /// <summary>
            /// The WagePoints method asks the player to wager an amount of points.
            /// </summary>
            /// <returns>
            /// Returns the player's wager.
            /// </returns>
            /// <param name="playerName">
            /// The specific player's name.
            /// </param>
            /// <param name="playerColor">
            /// The specific player's color.
            /// </param>
            /// <param name="playerPoints">
            /// The specific player's amount of points.
            /// </param>
            /// <param name="mostPointsOnBoard">
            /// The highest amount of points a clue can give on either the Jeopardy or Double Jeopardy board. The default is 0 for use in Final Jeopardy.
            /// </param>
            static int WagePoints(string playerName, ConsoleColor playerColor, int playerPoints, int mostPointsOnBoard = 0)
            {
                Console.WriteLine("\n\n");

                // inform player of the amount of points they have and how much they can wager
                Console.ForegroundColor = playerColor;
                Console.Write(playerName);
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(", you have earned " + playerPoints + " points at this point.\n");
                Console.ResetColor();

                int maxWager = playerPoints > mostPointsOnBoard ? playerPoints : mostPointsOnBoard;
                Console.WriteLine("You can wager between 1 and " + maxWager + ".");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("If your answer is correct, you will add the amount of points you wagered to your current score. \nIf your answer is incorrect, you will lose the amount of points you wagered. \n\n");
                Console.ResetColor();

                // let the player wage their points
                while (true)
                {
                    Console.WriteLine("Please input the amount of points you would like to wager and press enter.");

                    if (int.TryParse(Console.ReadLine(), out int pointsWagered) && pointsWagered > 0 && pointsWagered <= maxWager)
                    {
                        return pointsWagered;
                    }
                    else
                    {
                        InvalidInput("wager");
                        continue;
                    }
                }
            }

            /// <summary>
            /// The MostPoints method checks which player has the most points.
            /// </summary>
            /// <returns>
            /// Returns the player with the most points or -1 if there is a tie.
            /// </returns>
            /// <param name="playerPoints">
            /// An array of the players' points.
            /// </param>
            static int MostPoints(int[] playerPoints)
            {
                int playerWithMostPoints = 0;

                // go through the players' points and set the last player with most points as the player with most points
                for (int i = 0; i < playerPoints.Length; i++)
                {
                    if (playerWithMostPoints == i || playerPoints[playerWithMostPoints] < playerPoints[i])
                    {
                        playerWithMostPoints = i;
                    }
                }

                // check if any other player has the same amount of points
                for (int i = 0; i < playerPoints.Length; i++)
                {
                    if (playerPoints[playerWithMostPoints] == playerPoints[i] && playerWithMostPoints != i)
                    {
                        playerWithMostPoints = -1;
                        break;
                    }
                }

                return playerWithMostPoints;
            }
        }
    }
}
