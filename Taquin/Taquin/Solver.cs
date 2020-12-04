using System;
using System.Collections.Generic;

namespace Taquin
{
    public class PuzzleSolver
    {
        public PuzzleSolver(int[,] Puzzle)
        {
            
        }

        public static int[,] swapTilesOnPuzzle(int[,] puzzle, int[] firstIndex, int [] secondIndex)
        {
            int[,] newPuzzle = new int[4,4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    newPuzzle[i,j] = puzzle[i,j];
                }
            }

            int temp = newPuzzle[firstIndex[0], firstIndex[1]];
            newPuzzle[firstIndex[0], firstIndex[1]] = newPuzzle[secondIndex[0], secondIndex[1]];
            newPuzzle[secondIndex[0], secondIndex[1]] = temp;

            return newPuzzle;
        }

        public int[][] MoveableTileIndexes(int[] emptyTileIndex)
        {
            List<int[]> moveableTileIndexes = new List<int[]>();

            int line = emptyTileIndex[0];
            int row = emptyTileIndex[1];

            if (row > 0)
            {
                moveableTileIndexes.Add(new int[2] { line, row - 1 });
            }
            if (row < 3)
            {
                moveableTileIndexes.Add(new int[2] { line, row + 1 });
            }
            if (line > 0)
            {
                moveableTileIndexes.Add(new int[2] { line - 1, row });
            }
            if (line < 3)
            {
                moveableTileIndexes.Add(new int[2] { line + 1,  row });
            }
            return moveableTileIndexes.ToArray();

        }
    }


    public class Node
    {
        public Node parent;
        public Node[] childrens;
        public int[] moves;
        public int[] puzzle;
        public int heuristic;
        public int[] emptyTileIndex;

        public Node(Node parent, int[] moves, int nextMove, int[] previousBoard, int[] emptyTileIndex)
        {
            this.parent = parent;

            this.moves = new int[moves.Length + 1];
            for(int i = 0; i < moves.Length; i++)
            {
                this.moves[i] = moves[i];
            }
            this.moves[moves.Length] = nextMove;

            this.heuristic = 0;
        }
    }
}
