using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public enum WinPossibleScenarios { firstRow, secondRow, thirdRow, firstColumn, secondColumn, thirdColumn, forwardDiagonal, backDiagonal };

public class Controller : MonoBehaviour
{
    //Inspector variables
    [SerializeField] private string _tagSquare;

    //Public variables
    [HideInInspector]
    public GameObject[] squaresArray;
    public Dictionary<WinPossibleScenarios, int> boardHuman = new Dictionary<WinPossibleScenarios, int>();
    public Dictionary<WinPossibleScenarios, int> boardAI = new Dictionary<WinPossibleScenarios, int>();

    public Dictionary<int, string> boardIntToStr = new Dictionary<int, string>
    {
        {0, "[0,0]"}, {1, "[0,1]"}, {2,  "[0,2]" },
        {3, "[1,0]" }, {4, "[1,1]" }, {5, "[1,2]" },
        {6, "[2,0]" }, {7, "[2,1]" }, {8, "[2,2]" },
    };

    //Private variables
    private Minimax minimaxAlgo = new Minimax();
    private Dictionary<string, int> _boardStrToInt = new Dictionary<string, int>
    {
        { "[0,0]", 0 }, { "[0,1]", 1 }, { "[0,2]", 2 },
        { "[1,0]", 3 }, { "[1,1]", 4 }, { "[1,2]", 5 },
        { "[2,0]", 6 }, { "[2,1]", 7 }, { "[2,2]", 8 },
    };

    //Getters and setters
    private static string _humanChoice;
    public static string humanChoice
    {
        get { return _humanChoice; }
        set
        {
            if (_humanChoice != value)
            {
                _humanChoice = value;
            }
        }
    }

    void Awake()
    {
        squaresArray = GameObject.FindGameObjectsWithTag(_tagSquare);
        
        ClearBoard();

        IEnumerable<int> __humanRange = Enumerable.Range(0, 50);
        System.Random __value = new System.Random();

        if (__humanRange.Contains(__value.Next(0, 100)))
        {
            Debug.Log("Humano");
        }
        else
        {
            Debug.Log("AI");
        }
    }

    public void IsGameOver()
    {
        bool __isGameOver = false;

        int __tieCount = 0;
        string __line = null;
        foreach (GameObject __object in squaresArray)
        {
            Square __parent = __object.GetComponent<Square>();
            __tieCount = __parent.isChecked ? __tieCount + 1 : __tieCount;
        }

        if (__tieCount == 9)
        {
            Debug.Log("Empate");
            __isGameOver = true;

        } 
        else
        {
            
            foreach (KeyValuePair<WinPossibleScenarios, int> __boardSquare in boardHuman)
            {
                if (boardHuman[__boardSquare.Key].Equals(3))
                {
                    Debug.Log("Você ganhou");
                    __isGameOver = true;
                    __line = __boardSquare.Key.ToString();
                }
            }
            foreach (KeyValuePair<WinPossibleScenarios, int> __boardSquare in boardAI)
            {
                if (boardAI[__boardSquare.Key].Equals(3))
                {
                    Debug.Log("A AI Ganhou");
                    __isGameOver = true;
                    __line = __boardSquare.Key.ToString();
                }
            }
        }

        if (__isGameOver)
        {
       //     GameObject.Find(__line).SetActive(true);
            //RestartGame();
        }
    }

    private void ClearBoard()
    {
        foreach (WinPossibleScenarios __winPossibles in Enum.GetValues(typeof(WinPossibleScenarios)))
        {
            boardHuman[__winPossibles] = 0;
            boardAI[__winPossibles] = 0;
        }        
    }

    public void RestartGame()
    {
        Debug.Log("Reiniciando o game");
        _humanChoice = "";
        ClearBoard();

        for (int __i = 0; __i < squaresArray.Length; __i++)
        {
            squaresArray[__i].GetComponent<Square>().isChecked = false;
            for (int __j = 0; __j < squaresArray[__i].transform.childCount; __j++)
            {
                GameObject __pieces = squaresArray[__i].transform.GetChild(__j).gameObject;
                __pieces.GetComponent<ColorManager>().SetColor(__pieces.GetComponent<ColorManager>().alphaColor);
            }
        }
    }

    public void IncrementScoreSquare(string __increment, Dictionary<WinPossibleScenarios, int> __board)
    {
        switch (__increment)
        {
            case "[0,0]":
                __board[WinPossibleScenarios.firstRow]++;
                __board[WinPossibleScenarios.firstColumn]++;
                __board[WinPossibleScenarios.backDiagonal]++;
                break;
            case "[0,1]":
                __board[WinPossibleScenarios.firstRow]++;
                __board[WinPossibleScenarios.secondColumn]++;
                break;
            case "[0,2]":
                __board[WinPossibleScenarios.firstRow]++;
                __board[WinPossibleScenarios.thirdColumn]++;
                __board[WinPossibleScenarios.forwardDiagonal]++;
                break;
            case "[1,0]":
                __board[WinPossibleScenarios.secondRow]++;
                __board[WinPossibleScenarios.firstColumn]++;
                break;
            case "[1,1]":
                __board[WinPossibleScenarios.secondRow]++;
                __board[WinPossibleScenarios.secondColumn]++;
                __board[WinPossibleScenarios.forwardDiagonal]++;
                __board[WinPossibleScenarios.backDiagonal]++;
                break;
            case "[1,2]":
                __board[WinPossibleScenarios.secondRow]++;
                __board[WinPossibleScenarios.thirdColumn]++;
                break;
            case "[2,0]":
                __board[WinPossibleScenarios.thirdRow]++;
                __board[WinPossibleScenarios.firstColumn]++;
                __board[WinPossibleScenarios.forwardDiagonal]++;
                break;
            case "[2,1]":
                __board[WinPossibleScenarios.thirdRow]++;
                __board[WinPossibleScenarios.secondColumn]++;
                break;
            case "[2,2]":
                __board[WinPossibleScenarios.thirdRow]++;
                __board[WinPossibleScenarios.thirdColumn]++;
                __board[WinPossibleScenarios.backDiagonal]++;
                break;
        }
    }

    public int PlayAi(GameObject __object)
    {
        int __aiPlayChoice = Controller._humanChoice.Equals("X") ? -1 : 1;
        minimaxAlgo.SetAiChoicePlayer(__aiPlayChoice);
        
        TurnSquare __bestSquareEndTurn = minimaxAlgo.GetBestSquare(ConvertBoardToInt(__object), __aiPlayChoice, 0);
        int __newBestSquare = __bestSquareEndTurn.square;
        
        foreach (GameObject __boardSquare in squaresArray)
        {
            foreach (KeyValuePair<string, int> __intStrEntry in _boardStrToInt)
            {
                if (__intStrEntry.Key == __boardSquare.name && __intStrEntry.Value == __newBestSquare)
                {
                    if (Controller._humanChoice != __boardSquare.transform.GetChild(0).gameObject.name)
                    {
                        __object.GetComponent<MouseEvents>().SetSquareChecked(__boardSquare.transform.GetChild(0).gameObject);
                        __object.GetComponent<ColorManager>().SetAiOtherChoiceNoColor(__boardSquare);
                    }
                    else
                    {
                        __object.GetComponent<MouseEvents>().SetSquareChecked(__boardSquare.transform.GetChild(1).gameObject);
                        __object.GetComponent<ColorManager>().SetAiOtherChoiceNoColor(__boardSquare);
                    }
                }
            }
        }
        return __newBestSquare;
    }

    private int[] ConvertBoardToInt(GameObject __object)
    {
        int[] __boardInt = new int[9];
        Color32 __noColor = __object.GetComponent<ColorManager>().noColor;
        foreach (GameObject __square in this.squaresArray)
        {
            foreach (KeyValuePair<string, int> __intStrEntry in _boardStrToInt)
            {
                if (__intStrEntry.Key == __square.name)
                {
                    if (!__square.GetComponent<Square>().isChecked)
                    {
                        __boardInt[__intStrEntry.Value] = 0;
                    }
                    else
                    {
                        if (!__square.transform.GetChild(0).GetComponent<ColorManager>().GetColor().Equals(__noColor) && __square.transform.GetChild(0).gameObject.name == "X")
                        {
                            __boardInt[__intStrEntry.Value] = 1;

                        }
                        else
                        {
                            __boardInt[__intStrEntry.Value] = -1;
                        }
                    }
                }
            }
        }
        return __boardInt;
    }
}