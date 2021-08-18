/**
 * @author Kleber Ribeiro da Silva
 * @email krsmga@gmail.com
 * @create 2021-08-15 12:02:58
 * @modify 2021-08-15 12:02:58
 * @desc Algorithm based on Minimax (and minimax) architecture. 
 *       Objective to minimize a possible maximum loss, or, the maximization of the minimum gain (maximin).
 * @git https://github.com/krsmga/tictactoe
 */

using System; 
using UnityEngine;

public class TurnSquare
{
    public int score;
    public int square;

    public TurnSquare()
    {
        score = 0;
        square = 0;
    }

    public TurnSquare(int __score)
    {
        score = __score;
    }
}

public class Minimax
{
    // Constants
    private const int _WIN = 10;
    private const int _LOSE = -10;
    private const int _TIE = 0;

    // Private variables
    private int _winBoardSquares = 3;
    private int _aiChoicePlayer;

    // Getters and Setters
    private int _winCondition
    {
        get 
        {
            if (_aiChoicePlayer == -1)
            {
                return _winBoardSquares * _aiChoicePlayer;
            }
            else
            {
                return _winBoardSquares;
            }
        }
    }

    private int _loseCondition
    {
        get
        {
            return _winCondition * -1;
        }
    }

    public void SetAiChoicePlayer(int __value)
    {
        _aiChoicePlayer = __value;
    }    

    private int IsGameOver(int[] __board)
    {
        int __rowSize = (int)Math.Sqrt(__board.Length);
        int __rowNumber = 1;
        int __winHorizontal = 0;
        int[] __winVertical = new int[__rowSize];
        int __winDiagonalOne = 0;
        int __winDiagonalTwo = 0;        

        for (int __i = 0; __i < __board.Length; __i++)
        {
            if (__i != 0 && (__i % __rowSize) == 0)
            {
                ++__rowNumber;
                __winHorizontal = 0;
            }

            __winHorizontal += __board[__i];
            __winVertical[__i % __rowSize] += __board[__i];

            if (__i != 0 && __i == ((__rowSize - 1) * __rowNumber))
            {
                __winDiagonalOne += __board[__i];
            }

            if ((__i == ((__rowSize + 1) * (__rowNumber -1))))
            {
                __winDiagonalTwo += __board[__i];
            }

            if (__winHorizontal == _winCondition || __winVertical[__i % __rowSize] == _winCondition || __winDiagonalOne == _winCondition || __winDiagonalTwo == _winCondition)
            {
                return _WIN;
            }
            else 
            if (__winHorizontal == _loseCondition || __winVertical[__i % __rowSize] == _loseCondition || __winDiagonalOne == _loseCondition || __winDiagonalTwo == _loseCondition)
            {
                return _LOSE;
            }
        }
        return _TIE;        
    }

    public TurnSquare GetBestSquare(int[] __board, int __bestRoute, int __depth)
    {
        int __statusGameOver = IsGameOver(__board);
        
        if (__statusGameOver == _WIN)
        {
            return new TurnSquare(10 - __depth);
        }
        else if (__statusGameOver == _LOSE)
        {
            return new TurnSquare(__depth - 10);
        }
        else if (IsFullBoard(__board) && __statusGameOver == 0)
        {
            return new TurnSquare(0);
        }
        
        __depth++;

        TurnSquare __levelBestSquare = SetSquareForLevel(__bestRoute);

        for (int __i = 0; __i < __board.Length; __i++)
        {
            if (__board[__i] == 0)
            {
                __board[__i] = __bestRoute;
                TurnSquare __newSquare = GetBestSquare((int[])__board.Clone(), __bestRoute * -1, __depth);

                if (_aiChoicePlayer != __bestRoute)
                {
                    if (__newSquare.score <= __levelBestSquare.score)
                    {
                        __levelBestSquare.square = __i;
                        __levelBestSquare.score = __newSquare.score;
                    }
                }
                else
                {
                    if (__newSquare.score >= __levelBestSquare.score)
                    {
                        __levelBestSquare.square = __i;
                        __levelBestSquare.score = __newSquare.score;
                    }
                }
                
                __board[__i] = 0;
            }
        }
        return __levelBestSquare;
    }

    private TurnSquare SetSquareForLevel(int __value)
    {
        TurnSquare __tempSquare = new TurnSquare();
        if (_aiChoicePlayer != __value) 
        {
            __tempSquare.score = int.MaxValue;
        }
        else
        {    
            __tempSquare.score = int.MinValue;        
        }
        return __tempSquare;
    }

    private bool IsFullBoard(int[] __board)
    {
        foreach (int __node in __board)
        {
            if (__node == 0)
            {
                return false;
            }
        }
        return true;
    }
}