using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Taquin
{
    public class Board
    {
        public int tileWitdh;
        public int tileHeight;
        public string[] imageLines;
        public Tile[] tiles = new Tile[16];
        public int emptyTileIndex;
        public int previousEmptyTileIndex = -1;
        public bool isShuffled = false;

        public bool IsShuffled
        {
            get { return this.isShuffled; }
        }

        public Board(string filePath)
        {
            try
            {
                string[] fileLines = File
                    .ReadAllText(filePath, Encoding.ASCII)
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries);

                if (fileLines.Length < 4)
                {
                    throw new Exception("Le fichier doit contenir au moins 4 lignes");
                }

                if (fileLines[0].Length < 4)
                {
                    throw new Exception("Les lignes du fichier doivent contenir au moins 4 caractères");
                }

                int linesWidthCheck = fileLines[0].Length;

                foreach (string line in fileLines)
                {
                    if (line.Length != linesWidthCheck)
                    {
                        throw new Exception("Les lignes du fichier doivent contenir la même longueur");
                    }
                }

                this.imageLines = fileLines;
                this.tileWitdh = linesWidthCheck / 4;
                this.tileHeight = fileLines.Length / 4;

                for (int y = 0, tilePosition = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++, tilePosition++)
                    {
                        if (tilePosition < 15) // Skip last element 
                        {
                            this.tiles[tilePosition] = new Tile(
                            tilePosition,
                            x * this.tileWitdh,
                            y * this.tileHeight,
                            false);
                            
                        } else
                        {
                            this.tiles[tilePosition] = new Tile(
                            tilePosition,
                            0,
                            0,
                            true);

                            emptyTileIndex = tilePosition;
                        }
                        
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Une erreur est survenue pendant le chargement de la partie... \nError: \"{0}\"\n", e.Message);
            }
        }

        public void Show()
        {
            for (int y = 0, tileIndex = 0 ; y < 4; y++, tileIndex += 4)
            {
                string[] rowLines = new string[this.tileHeight];

                string header = "| ";

                for (int i = 0; i < 4; i++)
                {
                    int tileDisplayNumber = tileIndex + i + 1;
                    int padding = tileDisplayNumber > 9 ? 1 : 0;
                    
                    int widthHalf = this.tileWitdh / 2;

                    padding -= this.tileWitdh % 2 == 1 ? 1 : 0;
                    
                    header += new String(' ', widthHalf) + tileDisplayNumber + new String(' ', widthHalf - padding) + "| ";
                }
                Console.WriteLine(new String('-', this.imageLines[0].Length + 4 * 3 + 1));

                Console.WriteLine(header);
                for (int lineIndex = 0; lineIndex < tileHeight; lineIndex++)
                {
                    rowLines[lineIndex] = "| ";
                    for (int x = 0; x < 4; x++)
                    {
                        Tile currentTile = this.tiles[tileIndex + x];

                        if (currentTile.IsEmpty)
                        {
                            rowLines[lineIndex] += new String(' ', this.tileWitdh);
                        }
                        else
                        {
                            rowLines[lineIndex] += this.imageLines[currentTile.Y + lineIndex].Substring(currentTile.X, this.tileWitdh);

                        }
                        rowLines[lineIndex] += " | ";
                    }
                }
                Console.WriteLine(string.Join('\n', rowLines));
            }
            Console.WriteLine(new String('-', this.imageLines[0].Length + 4 * 3 + 1));
        }
        public int[] MoveableTileIndexes()
        {
            List<int> moveableTileIndexes = new List<int>();

            if (this.emptyTileIndex > 3)
            {
                moveableTileIndexes.Add(this.emptyTileIndex - 4);
            }
            if (this.emptyTileIndex % 4 != 0)
            {
                moveableTileIndexes.Add(this.emptyTileIndex - 1);
            }
            if (this.emptyTileIndex % 4 != 3)
            {
                moveableTileIndexes.Add(this.emptyTileIndex + 1);
            }
            if (this.emptyTileIndex < 12)
            {
                moveableTileIndexes.Add(this.emptyTileIndex + 4);
            }

            return moveableTileIndexes.ToArray();
        }
        public void MoveTile(int tileIndexToMove)
        {
            Tile temp = this.tiles[this.emptyTileIndex];

            this.tiles[this.emptyTileIndex] = this.tiles[tileIndexToMove];

            this.tiles[tileIndexToMove] = temp;

            this.previousEmptyTileIndex = this.emptyTileIndex;

            this.emptyTileIndex = tileIndexToMove;
        }
        public void Shuffle()
        {


            int[] moveableTileIndexes = this.MoveableTileIndexes();
            Random r = new Random();

            int randomIndex = 0;

            do
            {
                randomIndex = r.Next(0, moveableTileIndexes.Length);
            }
            while (this.previousEmptyTileIndex != -1 && moveableTileIndexes[randomIndex] == this.previousEmptyTileIndex);
            
            this.MoveTile(moveableTileIndexes[randomIndex]);

            this.isShuffled = true;
        }
        public bool PuzzleSolved()
        {
            for(int i = 0; i < 16; i++)
            {
                if (this.tiles[i].Number != i)
                {
                    return false;
                }
            }
            return true;
        }
        public int[,] Puzzle()
        {
            int[,] puzzle = new int[4, 4];

            int row = 0;
            int line = 0;

            foreach (Tile tile in this.tiles)
            {
                puzzle[line, row] = tile.Number;

                if (row == 3)
                {
                    row = 0;
                    line++;
                }
                else
                {
                    row++;
                }
            }

            return puzzle;
        }

    }

    public class Tile
    {
        private int x;
        private int y;
        private int number;
        private bool isEmpty;

        public Tile(int number, int x, int y, bool isEmpty)
        {
            this.x = x;
            this.y = y;
            this.number = number;
            this.isEmpty = isEmpty;
        }

        public int X
        {
            get { return this.x; }
        }
        public int Y
        {
            get { return this.y; }
        }
        public int Number
        {
            get { return this.number; }
        }
        public bool IsEmpty
        {
            get { return this.isEmpty; }
        }
    }

}
