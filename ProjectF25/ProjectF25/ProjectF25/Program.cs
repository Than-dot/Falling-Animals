using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ProjectF25
{
    internal class Program
    {
        // Synchronisation (FPS)
        static Int32 SYNC_FPS = 0;
        static Int32 SYNC = 3000;
        // Life
        static Int32 life = 3;
        static Int32 defaultlife = 3;
        static string heart = "Heart ";
        static Int32 heartX = 0;
        static Int32 heartY = 0;
        //SCORE
        static Int32 score = 0;
        static string point = "SCORE :";
        static Int32 scoreX = Console.WindowWidth - 10;
        static Int32 scoreY = 0;
        //Player(Paddle)
        static Int32 paddleX;
        static Int32 paddleY;
        static Int32 OldpaddleX;
        static Int32 paddlewidth = 11;
        static string paddle = "===========";
        //Key
        static ConsoleKey key;
        //Difficulty

        // Words
        static string[] words = { "Dog", "Cat", "Pig"};
        static Int32[] wordsX = new int[words.Length];
        static Int32[] wordsY = new int[words.Length];
        static Int32[] OldwordsX = new int[words.Length];
        static Int32[] OldwordsY = new int[words.Length];
        static Int32[] OffsetwordsX = new int[words.Length];
        static Int32[] OffsetwordsY = new int[words.Length];
        //Clearing
        static string[] ClearWords = new string[words.Length];
        static string ClearHearts = new string(' ', defaultlife * heart.Length);
        static string ClearPaddle = new string(' ', paddle.Length);
        //MISC
        static Random r = new Random();
        static string mode = "Default";
        static bool caught;
        static bool continueornot;
        //MAIN
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            do
            {
                WordsStartingPosition();
                PaddleStartingPosition();
                mainmenu();
                Console.Clear();
                while (!gameover())
                {
                    SYNC_FPS++;
                    lifesystem();
                    scoresystem();
                    Paddle();
                    Words();
                    draw();
                }
                continueornot = endmenu();
                score = 0; //reset the score
            }
            
            while (continueornot);
            Console.Clear();
            Console.WriteLine("Thank you for playing the game!");
        }
        //MAIN MENU
        static void mainmenu()
        {
            bool moveon = true;
            while (moveon == true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Falling Animals");
                Console.WriteLine("1. Start");
                Console.WriteLine("2. Instruction");
                Console.WriteLine("3. Settings");
                Console.WriteLine("4. Quit");
                ConsoleKeyInfo key = Console.ReadKey(true); // Key Variable for input without pressing enter, true make the choice doesn't appear on the screen
                switch (key.KeyChar) //Read the key and not string or int
                {
                    case '1':
                        return;
                    case '2':
                        Console.Clear();
                        Console.WriteLine("Welcome to Falling Animals");
                        Console.WriteLine("In this game, animals will fall from the sky and you need to catch it before it reachs the ground");
                        Console.WriteLine("In order to catch it, using left and right arrows or A and D to move the paddle to the animals before it reachs the ground");
                        Console.WriteLine("The game will technically run forever until you lose all the hearts");
                        Console.WriteLine("This game will consist of 3 difficulties: Default, Hard and Impossible");
                        Console.WriteLine("You will be starting with 3 hearts in Default and each difficulties will reduce it by 1");
                        Console.WriteLine("The higher the difficulty, the stronger the gravity and the better the rewards!");
                        Console.WriteLine("In order to change difficulty, go into Settings and change it from there");
                        Console.WriteLine("If you're ready, press any key to return to menu");
                        Console.ReadKey(true);
                        break;
                    case '3':
                        Console.Clear();
                        Console.WriteLine("Please choose the difficulty");
                        Console.WriteLine("1. Default");
                        Console.WriteLine("2. Hard");
                        Console.WriteLine("3. Impossible");
                        ConsoleKeyInfo Modekey = Console.ReadKey(true);
                        switch (Modekey.KeyChar)
                        {
                            case '1':
                                mode = "Default";
                                defaultlife = 3;
                                life = defaultlife;
                                SYNC = 2000;
                                break;
                            case '2':
                                mode = "Hard";
                                defaultlife = 2;
                                life = defaultlife;
                                SYNC = 1000;
                                break;
                            case '3':
                                mode = "Impossible";
                                defaultlife = 1;
                                life = defaultlife;
                                SYNC = 1000;
                                break;
                            default:
                                break;
                        }
                        Console.Clear();
                        Console.WriteLine("You've choose " + mode + " as your difficulty");
                        Console.WriteLine("Press any key to return to main menu");
                        Console.ReadKey(true);
                        break;
                    case '4':
                        moveon = false;
                        break;
                    default:
                        return;

                }
            }
        }
        static bool gameover()
        {
            return life <= 0; //return true if life <= 0
        }
        static void WordsStartingPosition()
        {
            for (int i = 0; i < words.Length; i++)
            {
                wordsX[i] = r.Next(0, Console.WindowWidth - words.Length); //Random the horizontal position of the word + can't go beyond the screen
                wordsY[i] = 2;
                //For Clearing the Old words position nicely without hardcode and cut word in half
                ClearWords[i] = new string(' ', words[i].Length);
            }
        }
        static void PaddleStartingPosition()
        {
            paddleX = Console.WindowWidth / 2 - paddlewidth / 2; //Make the paddle starts in the center
            paddleY = Console.WindowHeight - 2;
        }

        static void lifesystem()
        {
            //Previous heart
            Console.SetCursorPosition(heartX, heartY);
            //Clear the old one
            Console.Write(ClearHearts);
            //Present
            Console.SetCursorPosition(heartX, heartY);
            for (int i = 0; i < life; i++)
            {
                Console.Write(heart);
            }
        }
        static void scoresystem()
        {
            Console.SetCursorPosition(scoreX, scoreY);
            Console.Write(point + score + "   "); //Display score and give space for the score to not overlap the screen
        }
        static void Paddle()
        {
            if (Console.KeyAvailable)
            {
                key = Console.ReadKey(true).Key;//read from the keyboard, true hides it
                //Move left
                if (key == ConsoleKey.LeftArrow | key == ConsoleKey.A)
                {
                    paddleX--;
                    //Limits
                    if (paddleX < 0)
                    {
                        paddleX = 0;
                    }
                }

                //Move right
                if (key == ConsoleKey.RightArrow | key == ConsoleKey.D)
                {
                    paddleX++;
                    //Limits
                    if (paddleX + paddlewidth > Console.WindowWidth)
                    {
                        paddleX = Console.WindowWidth - paddlewidth;
                    }
                }
            }
        }
        static void Words()
        {
            //Speed
            if (SYNC_FPS % SYNC != 0)
            {
                return;
            }
            for (int i = 0; i < words.Length; i++)
            {
             //Remember Old positions
                OldwordsX[i] = wordsX[i];
                OldwordsY[i] = wordsY[i];
            //Falling section
                OffsetwordsY[i] = r.Next(1, 5);
                wordsY[i] += OffsetwordsY[i];
            //Collision
                if (wordsY[i] >= paddleY)
                {
                    //Catching it, the word is inside the paddle. Point gain!
                    caught = wordsX[i] + words[i].Length >= paddleX && wordsX[i] <= paddleX + paddlewidth;
                    if (caught)
                    {
                        //Point gain x2 each time difficulty goes up
                        if (mode == "Default")
                        {
                            score += 10;
                        }
                        else if (mode == "Hard")
                        {
                            score += 20;
                        }
                        else if (mode == "Impossible")
                        {
                            score += 40;
                        }
                    }
                    else if (!caught)
                    {
                        life--; //miss =  - 1 life
                    }
                    if (life <= 0)
                    {
                        life = 0; //make life unable to be negative, just in case
                    }
                    wordsY[i] = 2; //GO BACK!!!
                    wordsX[i] = r.Next(0, Console.WindowWidth - words.Length); // random the new position
                }
            } 
            }
        static void draw()
        {
            for (int i = 0; i < words.Length; i++)
            {
                //ANIMALS
                  //get rid of the old animals
                  Console.SetCursorPosition(OldwordsX[i], OldwordsY[i]);
                  Console.Write(ClearWords[i]);
                  //draw the new animals
                  Console.SetCursorPosition(wordsX[i], wordsY[i]);
                Console.Write(words[i]);
            }
            //PADDLE
            //get rid of the old paddle
            Console.SetCursorPosition(OldpaddleX, paddleY);
            Console.Write(ClearPaddle);
            // draw the new paddle
            Console.SetCursorPosition(paddleX, paddleY);
            Console.Write(paddle);
            OldpaddleX = paddleX;


        }
        static bool endmenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Game's over, you've got " + score + " points");
                Console.WriteLine("Do you wish to retry?");
                Console.WriteLine("1. Return to main menu");
                Console.WriteLine("2. Quit");
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.KeyChar)
                {
                    case '1':
                        return true;
                    case '2':
                        return false;
                    default:
                        break;
                }
            }
        }
    }
}

