//PLAYER ONE
using System;

namespace TicTacToe
{
    class Program
    {
        // globala variabler
        static int[,] board = new int[3, 3];
        static string[,] plane = new string[3, 3];
        static int p1;
        static int p2;
        static bool player1;
        static bool player2;
        static bool Win = false;
        static string? winner;

        static void Main()
        {
            DB db = new DB(); // skapar en ny instans av klassen DB som länkar till databasen
            Turns();
            db.MoveFirst();
            List<Turn> turns = db.GetTurn();
            // sätter in värden från databasen i globala variabler
            p1 = turns[0].playerone;
            p2 = turns[0].playertwo;

            Console.WriteLine("Welcome too TickTacToe!!!");
            Thread.Sleep(500);
            Console.WriteLine("Player 1 is X and Player 2 is O!");
            Thread.Sleep(500);
            Console.WriteLine("Lest role a dice to see who goes first!");
            Thread.Sleep(2000);
            Console.WriteLine("Player 1 rolled: " + p1);
            Thread.Sleep(500);
            Console.WriteLine("And...");
            Thread.Sleep(2000);
            Console.WriteLine("Player 2 rolled: " + p2);
            Thread.Sleep(500);

            FirstTurn();
            Playing();
        } // introducerar spelet och kör igång spelet
        static void Playing()
        {
            // Körs så länge ingen har vunnit
            DB db = new DB();
            while (!Win)
            {
                Turn();
                Console.Clear();
                CheckForWin();
            }
        } // Kör spelet
        static void Turns()
        {
            DB db = new DB();
            Random random = new Random();
            // slumpar fram ett tal mellan 1 och 6
            int o = random.Next(1, 6); 
            int t = random.Next(1, 6);
            // sätter in värdena i databasen
            db.SetTurn(new Turn() { playerone = o, playertwo = t }); 
        } // en metod som slumpar fram två tal och sätter in dem i databasen
        static void DrawBoard()
        {
            // Konverterar siffror till X, O eller " "
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == 0)
                    {
                        plane[i, j] = " ";
                    }
                    else if (board[i, j] == 1)
                    {
                        plane[i, j] = "X";
                    }
                    else if (board[i, j] == 2)
                    {
                        plane[i, j] = "O";
                    }
                }
            }
            // Skriver ut spelplanen
            Console.WriteLine("   |  1. |  2. |  3. | ");
            Console.WriteLine("___|_____|_____|_____|");
            Console.WriteLine("   |     |     |     | ");
            Console.WriteLine("1. |  {0}  |  {1}  |  {2}  |", plane[0, 0], plane[0, 1], plane[0, 2]);
            Console.WriteLine("___|_____|_____|_____| ");
            Console.WriteLine("   |     |     |     | ");
            Console.WriteLine("2. |  {0}  |  {1}  |  {2}  |", plane[1, 0], plane[1, 1], plane[1, 2]);
            Console.WriteLine("___|_____|_____|_____| ");
            Console.WriteLine("   |     |     |     | ");
            Console.WriteLine("3. |  {0}  |  {1}  |  {2}  |", plane[2, 0], plane[2, 1], plane[2, 2]);
            Console.WriteLine("___|_____|_____|_____| ");
            Console.WriteLine("Player 1 is X and Player 2 is O! \n");
        } // skriver ut spelplanen
        static void CheckForWin()
        {
            DB db = new DB();

            // Horisontell vinst
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == 1 && board[i, 1] == 1 && board[i, 2] == 1)
                {
                    winner = "Player 1 wins!";
                    Win = true;
                    db.Reset();
                }
                else if (board[i, 0] == 2 && board[i, 1] == 2 && board[i, 2] == 2)
                {
                    winner = "Player 2 wins!";
                    Win = true;
                    db.Reset();
                }
            }

            // Vertical vinst
            for (int i = 0; i < 3; i++)
            {
                if (board[0, i] == 1 && board[1, i] == 1 && board[2, i] == 1)
                {
                    winner = "Player 1 wins!";
                    Win = true;
                    db.Reset();
                }
                else if (board[0, i] == 2 && board[1, i] == 2 && board[2, i] == 2)
                {
                    winner = "Player 2 wins!";
                    Win = true;
                    db.Reset();
                }
            }

            // Diagonal vinst
            if (board[0, 0] == 1 && board[1, 1] == 1 && board[2, 2] == 1)
            {
                winner = "Player 1 wins!";
                Win = true;
                db.Reset();
            }
            else if (board[0, 0] == 2 && board[1, 1] == 2 && board[2, 2] == 2)
            {
                winner = "Player 2 wins!";
                Win = true;
                db.Reset();
            }
            if (board[0, 2] == 1 && board[1, 1] == 1 && board[2, 0] == 1)
            {
                winner = "Player 1 wins!";
                Win = true;
                db.Reset();
            }
            else if (board[0, 2] == 2 && board[1, 1] == 2 && board[2, 0] == 2)
            {
                winner = "Player 2 wins!";
                Win = true;
                db.Reset();
            }

            // Oavgjort
            int count = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] != 0)
                    {
                        count++;
                    }
                }
            }
            if (count == 9)
            {
                winner = "The game is a draw!";
                Win = true;
                db.Reset(); 
            }

            // Slut på spelet
            if (Win)
            {
                Console.Clear();
                player1 = true;
                DrawBoard();
                Console.WriteLine(winner);
                Console.WriteLine("Game Over!");
                Environment.Exit(0);
            }

        } // Kollar om någon har vunnit
        static void PlayerTurn()
        {
            DB dB = new DB();

            try
            {
                // Hämtar andra spelarens tur från databasen
                List<Player> players = dB.GetPlayerMove();
                Player player = new Player();
                player.X = players[0].X;
                player.Y = players[0].Y;
                if (board[player.X, player.Y] == 0)
                {
                    board[player.X, player.Y] = 2;
                }
            }
            catch
            {
            }

            DrawBoard();
            int x = 0;
            int y = 0;
            Console.Write("Enter the row number: ");
            try
            {
                x = int.Parse(Console.ReadLine()!) - 1;
                // Ser till att spelaren spelar inom spelplanen
                if (x > 2 || x < 0) 
                {
                    Console.WriteLine(x + " is not a valid row number. Please enter a number between 0 and 3!");
                    Console.Clear();
                    PlayerTurn();
                }
            }
            catch
            {
                // Om spelaren inte skriver in ett nummer
                Console.WriteLine("Please enter a valid number!");
                Console.Clear();
                PlayerTurn();
            }

            Console.Write("Enter the column number: ");
            try
            {
                y = int.Parse(Console.ReadLine()!) - 1;
                // Ser till att spelaren spelar inom spelplanen
                if (y > 2 || y < 0)
                {
                    Console.WriteLine(y + " is not a valid column number. Please enter a number between 0 and 3!");
                    Console.Clear();
                    PlayerTurn();
                }
            }
            catch
            {
                // Om spelaren inte skriver in ett nummer
                Console.WriteLine("Please enter a valid number!");
                Console.Clear();
                PlayerTurn();
            }

            // Ser till att spelaren inte kan skriva över en annan spelares drag
            if (board[x, y] == 0)
            {
                board[x, y] = 1;
            }
            else
            {
                // Om spelaren spelar på en redan tagen plats får de köra om
                Console.WriteLine($"The spot ({x}, {y}) is already taken!");
                PlayerTurn();
            }

            // Skriver in spelarens drag i databasen
            dB.SetPlayerMove(x, y);
            dB.SetPlayerTurn(false, true);

            Playing();
        } // Spelarens tur
        static void FirstTurn()
        {
            DB db = new DB();
            // Hämtar vem som börjar spelet från databasen
            List<Move> moves = db.GetFirstMove();
            player1 = moves[0].move1;
            if (player1)
            {
                // Om spelaren börjar
                Console.Clear();
                Console.WriteLine("Its your turn :) \n");
                Console.WriteLine(moves[0].move1);
                PlayerTurn();
            }
            else
            {
                // Om andra spelaren börjar
                Console.Clear();
                Console.WriteLine("Its not your turn :( \n");
                Console.WriteLine(moves[0].move1);
                DrawBoard();
                Turn();
            }
        } // Vem som börjar spelet
        static void Turn() 
        {
            DB db = new DB();

            // försöker hämta vems tur det är
            try
            {
                // Hämtar vems tur det är från databasen
                List<PlayerTurn> playerTurns = db.GetPlayerTurn();
                player1 = playerTurns[0].playerone;
                if (player1)
                {
                    // Om det är spelarens tur
                    Thread.Sleep(1000);
                    Console.Clear();
                    Console.WriteLine("Its your turn :) \n");
                    PlayerTurn();
                }
                else
                {
                    // Om det inte är spelarens tur
                    Thread.Sleep(1000);
                    Console.Clear();
                    Console.WriteLine("Its not your turn :( \n");
                    DrawBoard();
                    while (!player1)
                    {
                        try
                        {
                            // försöker hämta andra spelarens drag
                            List<Player> players = db.GetPlayerMove();
                            Player player = new Player();
                            player.X = players[0].X;
                            player.Y = players[0].Y;
                            if (board[player.X, player.Y] == 0)
                            {
                                board[player.X, player.Y] = 2;
                            }
                        }
                        catch
                        {
                        }
                        // Hämtar vems tur det är
                        List<PlayerTurn> playerTurns1 = db.GetPlayerTurn();
                        player1 = playerTurns1[0].playerone;
                        CheckForWin(); // kollar om någon har vunnit
                    }
                }
            }
            catch
            {
                // Om det inte finns något i databasen
                player1 = false;
                Turn();
            }
        } // Vem spelar
    }
}