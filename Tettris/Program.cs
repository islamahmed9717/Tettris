using System;
using System.Security.Cryptography;
internal class Tettris
{
    static int rows = 20;
    static int cols = 10;
    static Random rnd = new Random();
    static Shape CurrentShape;
    static char[,] board = new char[rows, cols];
    static bool isGameOver = false;
    static bool isGamePaused = false;
    static ConsoleKey? lastKeyPressed = null;


    static void Main(string[] args)
    {
        Intializeboard();

        Thread thread = new Thread(GameLoop);
        thread.Start();

        // also make a thread for input user

    }


    static void Intializeboard()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                board[i, j] = ' ';
            }
        }
    }

    static void GameLoop()
    {
        while (!isGameOver)
        {
            if (isGamePaused)
            {
                Thread.Sleep(1000);
                continue;
            }

            // logic of game loop

            Console.Clear();
            Printboard();

            if (CurrentShape == null || !CanMove(CurrentShape, 1, 0))
            {
                if (CurrentShape != null)
                {
                    mergeShape();
                    clearRows();
                }

                CurrentShape = GetRandomShape();

                if (!CanMove(CurrentShape, 0, 0))
                {
                    isGameOver = true;
                    Console.WriteLine("GameOver");
                    return;
                }
            }

            else
            {
                CanMove(CurrentShape, 1, 0);
            }

            Thread.Sleep(500);


        }
    }

    private static void clearRows()
    {
        for (int i = rows - 1; i >= 0; i--)
        {
            bool isFull = true;
            for (int j = 0; j < cols; j++)
            {
                if (board[i, j] == ' ')
                {
                    isFull = false;
                    break;
                }
            }

            if (isFull)
            {
                for (int K = i; K >= 0; K--)
                {
                    for (int J = 0; J < cols; J++)
                    {
                        board[K, J] = board[K - 1, J];
                    }
                }


                for (int J = 0; J < cols; J++)
                {
                    board[0, J] = ' ';
                }

                i++;

            }
        }

    }

    private static void mergeShape()
    {
        foreach(var block in CurrentShape.blocks)
        {
            int newRow = block.row + CurrentShape.Baserow;
            int newCol = block.col + CurrentShape.Basecol;

            board[newRow, newCol] = '#';
        }
    }

    private static bool CanMove(Shape currentShape, int row, int col)
    {
        foreach (var block in CurrentShape.blocks)
        {
            int newRow = block.row + currentShape.Baserow + row;
            int newCol = block.col + currentShape.Basecol + col;

            if (newRow >= rows || newCol >= cols)
            {
                return false;
            }
        }
        return true;
    }

    static void Printboard()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (CurrentShape != null)
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(board[i, j]);
                }
            }
            Console.WriteLine();
        }
    }


    static Shape GetRandomShape()
    {
        int typeOfShape = rnd.Next(3);

        switch (typeOfShape)
        {
            case 0:
                return new Squareshape(0, 4);
            case 1:
                return new Lshape(0, 4);
            case 2:
                return new Tshape(0, 4);
            default:
                return null;
        }
    }

}


public class Block
{
    public int row { get; set; }
    public int col { get; set; }
    public char Symbol { get; set; } = '#';
    public Block(int row, int col)
    {
        this.row = row;
        this.col = col;
    }
}

public class Shape
{
    public List<Block> blocks { get; set; } = new List<Block>();
    public int Baserow { get; set; }
    public int Basecol { get; set; }

    public Shape(int row, int col)
    {
        Baserow = row;
        Basecol = col;
    }

    public void Move(int drow, int dcol)
    {
        Baserow += drow;
        Basecol += dcol;
    }

    public void Rotate()
    {
        // logic of rotation
    }


}

public class Tshape : Shape
{
    public Tshape(int row, int col) : base(row, col)
    {
        blocks.Add(new Block(0, -1));
        blocks.Add(new Block(0, 0));
        blocks.Add(new Block(0, 1));
        blocks.Add(new Block(1, 0));
    }
}

public class Lshape : Shape
{
    public Lshape(int row, int col) : base(row, col)
    {
        blocks.Add(new Block(0, -1));
        blocks.Add(new Block(0, 0));
        blocks.Add(new Block(0, 1));
        blocks.Add(new Block(1, 1));
    }
}

public class Squareshape : Shape
{
    public Squareshape(int row, int col) : base(row, col)
    {
        blocks.Add(new Block(0, 0));
        blocks.Add(new Block(0, 1));
        blocks.Add(new Block(1, 0));
        blocks.Add(new Block(1, 1));
    }
}

