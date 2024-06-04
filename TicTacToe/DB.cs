using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class DB
    {
        // Connection string till databasen
        string connectionString = "datasource=localhost;port=3306;username=root;password=;database=tictactoe";

        public List<Turn> GetTurn() // Hämtar senaste dragen
        {
            // Skapar en lista för att lagra dragen
            List<Turn> turns = new List<Turn>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                // sql fråga som hämtar de senaste dragen
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `playermove`\r\nWHERE ID = (\r\nSELECT MAX(ID) FROM `playermove`)", conn);
                // Läser av svaret från databasen
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Turn turn = new Turn();
                        turn.ID = reader.GetInt32(0);
                        turn.playerone = reader.GetInt32(1);
                        turn.playertwo = reader.GetInt32(2);
                        turns.Add(turn);
                    }
                }
            }
            return turns;
        }

        public List<Turn> SetTurn(Turn turn) // Sätter drag i databasen
        {
            // Skapar en anslutning till databasen
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            // sql fråga som sätter draget i databasen
            MySqlCommand cmd = new MySqlCommand("INSERT INTO playermove (`PlayerOne`, `PlayerTwo`) VALUES (@playerOne, @PlayerTwo)", conn);

            // Variabler som hämtas från programmet och sätts i databasen
            cmd.Parameters.AddWithValue("@PlayerOne", turn.playerone);
            cmd.Parameters.AddWithValue("@PlayerTwo", turn.playertwo);

            cmd.ExecuteNonQuery();
            conn.Close();
            return GetTurn();
        }

        public List<Move> MoveFirst() // Sätter in data om vem som spelas först
        {
            List<Move> moves = new List<Move>();
            GetTurn();
            List<Turn> turns = GetTurn();
            Turn trns = new Turn();
            trns.playerone = turns[0].playerone;
            trns.playertwo = turns[0].playertwo;
            int p1 = trns.playerone;
            int p2 = trns.playertwo;

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            // om det blir lika sätts ett nytt värde
            if (p1 == p2)
            {
                Random random = new Random();
                p1 = random.Next(1, 6);
                p2 = random.Next(1, 6);
                // sql fråga som sätter in i databasen
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `playermove` (`PlayerOne`, `PlayerTwo`) VALUES (@p1, @p2)", conn);
                // variabler från programmet som sätts i databasen
                cmd.Parameters.AddWithValue("@p1", p1);
                cmd.Parameters.AddWithValue("@p2", p2);
                cmd.ExecuteNonQuery();
                conn.Close();
                return MoveFirst();
            }
            else if (p1 > p2)
            {
                // Om spelare 1 spelar först sätts det in i databasen
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `moves` (`P1`, `P2`) VALUES (1, 0)", conn);
                cmd.ExecuteNonQuery();
            }
            else if (p1 < p2)
            {
                // Om spelare 2 spelar först sätts det in i databasen
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `moves` (`P1`, `P2`) VALUES (0, 1)", conn);
                cmd.ExecuteNonQuery();
            }

            conn.Close();
            return moves;
        }

        public List<Move> GetFirstMove() // Hämtar vem som spelar först
        {
            List<Move> moves = new List<Move>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            // sql fråga som hämtar vem som spelar först
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM `moves` WHERE ID = (SELECT MAX(ID) FROM `moves`)", conn);
            // Läser av svaret från databasen
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Move move = new Move();
                    move.ID = reader.GetInt32(0);
                    move.move1 = reader.GetBoolean(1);
                    move.move2 = reader.GetBoolean(2);
                    moves.Add(move);
                }
            }
            conn.Close();
            return moves;
        }

        public List<Player> SetPlayerMove(int x, int y) // Sätter in spelarens drag i databasen
        {
            List<Player> players = new List<Player>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            // sql fråga som sätter in spelarens drag i databasen
            MySqlCommand cmd = new MySqlCommand("INSERT INTO `playerone` (`X`, `Y`) VALUES (@x, @y)", conn);
            // variabler från programmet som sätts i databasen
            cmd.Parameters.AddWithValue("@x", x);
            cmd.Parameters.AddWithValue("@y", y);
            cmd.ExecuteNonQuery();
            conn.Close();
            return players;
        }

        public List<Player> GetPlayerMove() // Hämtar spelarens drag från databasen
        {
            List<Player> players = new List<Player>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            // sql fråga som hämtar spelarens senaste drag från databasen
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM `playertwo` WHERE ID = (SELECT MAX(ID) FROM `playertwo`)", conn);
            // Läser av svaret från databasen
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Player player = new Player();
                    player.ID = reader.GetInt32(0);
                    player.X = reader.GetInt32(1);
                    player.Y = reader.GetInt32(2);
                    players.Add(player);
                }
            }
            conn.Close();
            return players;
        }

        public List<PlayerTurn> GetPlayerTurn() // Hämtar vem som ska spela
        {
            List<PlayerTurn> playerTurns = new List<PlayerTurn>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            // sql fråga som hämtar vem som ska spela
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM `turn` WHERE ID = (SELECT MAX(ID) FROM `turn`)", conn);
            // Läser av svaret från databasen
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    PlayerTurn playerTurn = new PlayerTurn();
                    playerTurn.ID = reader.GetInt32(0);
                    playerTurn.playerone = reader.GetBoolean(1);
                    playerTurn.playertwo = reader.GetBoolean(2);
                    playerTurns.Add(playerTurn);
                }
            }
            conn.Close();
            return playerTurns;
        }

        public List<PlayerTurn> SetPlayerTurn(bool a, bool b) // Sätter vem som ska spela
        {
            List<PlayerTurn> playerTurns = new List<PlayerTurn>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            // sql fråga som sätter vem som ska spela
            MySqlCommand cmd = new MySqlCommand("INSERT INTO `turn` (`P1T`, `P2T`) VALUES (@playerOne, @PlayerTwo)", conn);
            // variabler från programmet som sätts i databasen
            cmd.Parameters.AddWithValue("@PlayerOne", a);
            cmd.Parameters.AddWithValue("@PlayerTwo", b);
            cmd.ExecuteNonQuery();
            conn.Close();
            return playerTurns;
        }

        public void Reset() // Nollställer databasen
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("TRUNCATE TABLE `playermove`", conn);
            cmd.ExecuteNonQuery();
            MySqlCommand cmd1 = new MySqlCommand("TRUNCATE TABLE `moves`", conn);
            cmd1.ExecuteNonQuery();
            MySqlCommand cmd2 = new MySqlCommand("TRUNCATE TABLE `playerone`", conn);
            cmd2.ExecuteNonQuery();
            MySqlCommand cmd3 = new MySqlCommand("TRUNCATE TABLE `playertwo`", conn);
            cmd3.ExecuteNonQuery();
            MySqlCommand cmd4 = new MySqlCommand("TRUNCATE TABLE `turn`", conn);
            cmd4.ExecuteNonQuery();
            // Lägger till att det inte är någons tur
            MySqlCommand cmd5 = new MySqlCommand("INSERT INTO `turn`(`ID`, `P1T`, `P2T`) VALUES ('1','0','0')", conn);
            cmd5.ExecuteNonQuery();
            conn.Close();
        }
    }
}
