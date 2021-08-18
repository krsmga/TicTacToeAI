using System;
using UnityEngine;

public class MouseEvents : MonoBehaviour
{
    // Private variables
    private Controller _controller;
    private ColorManager _colorManager;
    private GameObject _piece;
    private Square _square;

    private void Awake()
    {
        _controller = GameObject.Find("/Controller").GetComponent<Controller>();
        _piece = transform.parent.gameObject;
        _square = _piece.GetComponent<Square>();
        _colorManager = GetComponent<ColorManager>();
    }

    private void OnMouseEnter()
    {
        if (!_square.isChecked && !_colorManager.IsSquareNoColor())
        {
            _colorManager.SetColor(_colorManager.baseColor);
        }
    }

    private void OnMouseExit()
    {
        if (!_square.isChecked && !_colorManager.IsSquareNoColor())
        {
            _colorManager.SetColor(_colorManager.alphaColor);
        }
    }

    private void OnMouseDown()
    {
        if (!_colorManager.IsEqualsColors(_colorManager.noColor, _colorManager.GetColor()))
        {
            if (!_square.isChecked)
            {
                SetHumanChoice();
                if (Controller.humanChoice == name)
                {
                    SetSquareChecked(gameObject);
                    _controller.IncrementScoreSquare(_piece.name, _controller.boardHuman);
                }
                _controller.IncrementScoreSquare(_controller.boardIntToStr[_controller.PlayAi(gameObject)], _controller.boardAI);
                _controller.IsGameOver();
            }
        }
    }

    public void SetSquareChecked(GameObject __square)
    {
        __square.GetComponent<ColorManager>().SetColor(__square.GetComponent<ColorManager>().baseColor);
        __square.transform.parent.gameObject.GetComponent<Square>().isChecked = true;
    }

    private void SetHumanChoice()
    {
        if (String.IsNullOrEmpty(Controller.humanChoice))
        {
            Controller.humanChoice = name;
            _colorManager.ClearBoardOfNonePlayerAvailableChoices();
        }
    }
}
