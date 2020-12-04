using System;
using System.IO;

namespace Taquin
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "";
            string directoryPath = Path.GetFullPath("/Users/vduong/projetsLBC/taquin_cs/ascii-images");

            Console.Clear();
            while (true)
            {
                try
                {
                    Console.WriteLine("Menu: \n");
                    if (filePath == "")
                    {
                        Console.WriteLine("Pas de fichier selectionné\n");
                        Console.WriteLine("Dossier d'images: \n\"{0}\"\n", directoryPath);
                    }
                    else
                    {
                        Console.WriteLine("Fichier \"${0}\" sélectionné\n\n", filePath);
                    }
                    Console.WriteLine("1. Quitter");
                    Console.WriteLine("2. Selectionner un fichier pour jouer");
                    if (filePath == "")
                    {
                        Console.WriteLine("3. Jouer -- Veuillez sélectionner un fichier pour jouer(2)\n");
                    }
                    else
                    {
                        Console.WriteLine("3. Jouer\n");
                    }
                    Console.Write("Choix: ");
                    int menuChoice = Convert.ToInt32(Console.ReadLine());

                    switch (menuChoice)
                    {
                        case 1:
                            Console.WriteLine("Quitting....");
                            System.Environment.Exit(1);
                            break;
                        case 2:
                            Console.Clear();
                            filePath = filesMenu(directoryPath);
                            Console.Clear();
                            break;
                        case 3:
                            if (filePath != "")
                            {
                                Console.Clear();
                                boardMenu(filePath);
                                Console.Clear();
                            }
                            else
                            {
                                Console.Clear();
                            }
                           break;
                        default:
                            throw new Exception(Convert.ToString(menuChoice));
                    }

                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("ERROR: Wrong input ({0}) ! \n", e.Message);
                }
            }

        }

        static string filesMenu(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Directory {0} does not exists", directoryPath);
            }

            while (true)
            {

                string[] files = Directory.GetFiles(directoryPath);

                Console.WriteLine("Menu > File selection: ");
                Console.WriteLine("0. Retourner au menu");

                for (int i = 1; i <= files.Length; i++)
                {
                    Console.WriteLine("{0}. {1}", i, files[i - 1]);
                }

                Console.Write("Select file number or press 0 to return to menu: ");

                try
                {
                    int fileChoice = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Your input: {0}", fileChoice);

                    if (fileChoice < 0 || fileChoice > files.Length)
                    {
                        throw new OverflowException(Convert.ToString(fileChoice));
                    }

                    if (fileChoice == 0)
                    {
                        return "";
                    }
                    else
                    {
                        return files[fileChoice - 1];

                    }

                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("ERROR: Wrong input ({0}) ! \n", e.Message);
                }
            }
        }
        static void boardMenu(string filePath)
        {
            Board board = new Board(filePath);
            while (true)
            {
                try
                {
                    Console.WriteLine("Voici le puzzle à résoudre: ");
                    board.Show();
                    Console.WriteLine("Menu > Partie: \n");
                    Console.WriteLine("1. Retourner au menu principal");
                    Console.WriteLine("2. Mélanger");
                    if (board.IsShuffled)
                    {
                        Console.WriteLine("3. Jouer avec un nombre de coups limité (20)");
                        Console.WriteLine("4. Jouer avec un nombre de coups illimité");
                        Console.WriteLine("5. Résoudre avec l'algorithme A*");
                    }
                    Console.Write('\n');
                    Console.Write("Choix: ");

                    int menuChoice = Convert.ToInt32(Console.ReadLine());

                    switch (menuChoice)
                    {
                        case 1:
                            return;
                        case 2:
                            for (int i = 0; i < 100; i++)
                            {
                                Console.Clear();
                                Console.WriteLine("En cours de mélange...");
                                board.Shuffle();
                                board.Show();
                                System.Threading.Thread.Sleep(50);
                            }
                            Console.Clear();

                            break;
                        case 3:
                            Console.Clear();
                            playGame(board, 20);
                            Console.Clear();
                            board = new Board(filePath);
                            break;
                        case 4:
                            Console.Clear();
                            playGame(board, -1);
                            Console.Clear();
                            board = new Board(filePath);
                            break;
                        case 5:
                            Console.Clear();
                            solveGame(board);
                            break;
                        default:
                            throw new Exception(Convert.ToString(menuChoice));
                    }
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("ERROR: Wrong input ({0}) ! \n", e.Message);
                }
            }
        }

        static void playGame(Board board, int roundLimit)
        {
            for (int roundNumber = 0; ;roundNumber++)
            {
                try
                {
                    Console.WriteLine("Vous êtes à {0} coups", roundNumber);

                    if (roundLimit > -1)
                    {
                        Console.WriteLine("Il vous reste {0} courps", roundLimit - roundNumber);
                    }

                    board.Show();

                    int[] moveableTileIndexes = board.MoveableTileIndexes();

                    Console.WriteLine("Vous pouvez bouger les panneaux suivants:\n");

                    foreach (int index in moveableTileIndexes)
                    {
                        Console.WriteLine("- {0}", index + 1);
                    }
                    Console.WriteLine("Pour quitter: 0\n");
                    Console.Write("Choix: ");
                    int choice = Convert.ToInt32(Console.ReadLine());

                    if (choice == 0)
                    {
                        return;
                    }

                    bool choiceIsAvailable = false;

                    foreach (int index in moveableTileIndexes)
                    {
                        if (index + 1 == choice) { choiceIsAvailable = true; }
                    }

                    if (!choiceIsAvailable) { throw new Exception(Convert.ToString(choice)); }

                    board.MoveTile(choice - 1);

                    if (board.PuzzleSolved())
                    {
                        Console.Clear();
                        Console.WriteLine();
                        board.Show();

                        Console.WriteLine("\n\n🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉");
                        Console.WriteLine("🎉🎉 Bravo, vous avez résolu le puzzle en {0} coups ! 🎉🎉", roundNumber + 1);
                        Console.WriteLine("🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉🎉");

                        Console.WriteLine("\nAppuyez sur Enter pour continuer...");

                        Console.ReadLine();
                        return;
                    }

                    if (roundNumber == roundLimit)
                    {
                        Console.Clear();
                        Console.WriteLine();
                        board.Show();

                        Console.WriteLine("\n\n💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀");
                        Console.WriteLine("💀💀💀 Dommage vous avez perdu 💀💀💀");
                        Console.WriteLine("💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀💀");

                        Console.WriteLine("\nAppuyez sur Enter pour continuer...");

                        Console.ReadLine();
                        return;
                    }
                    Console.Clear();
                }
                catch(Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("Veuillez choisir un numéro de choix de la liste! \n", e.Message);
                    roundNumber--;
                }
            }
        }
        static void solveGame(Board board)
        {
            int[,] puzzle = board.Puzzle();

            PuzzleSolver solver = new PuzzleSolver(puzzle);


            return;
        }
    }
}
