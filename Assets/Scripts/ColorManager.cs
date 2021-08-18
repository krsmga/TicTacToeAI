using UnityEngine;

public class ColorManager : MonoBehaviour
{
    private Controller _controller;

    private Color32 _baseColor;
    public Color32 baseColor
    {
        get { return _baseColor; }
        set 
        {
            _baseColor = value;
        }
    }

    private Color32 _alphaColor;
    public Color32 alphaColor
    {
        get { return _alphaColor; }
        set 
        {
            _alphaColor = value;
        }
    }
    
    private Color32 _noColor;
    public Color32 noColor
    {
        get { return _noColor; }
        set 
        {
            _noColor = value;
        }
    }    

    private void Awake()
    {
        _controller = GameObject.Find("/Controller").GetComponent<Controller>();
        _baseColor = GetComponent<Renderer>().material.color;
        _alphaColor = new Color(1.0f, 1.0f, 1.0f, 0.1f);         
        _noColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        GetComponent<Renderer>().material.color = _alphaColor;
    }

    public void SetColor(Color32 __color)
    {
        GetComponent<Renderer>().material.color = __color;
    }

    public Color32 GetColor()
    {
        return GetComponent<Renderer>().material.color;
    }

    public bool IsSquareNoColor()
    {
        return IsEqualsColors(GetColor(), _noColor);
    }

    public void ClearBoardOfNonePlayerAvailableChoices()
    {
        for (int i = 0; i < _controller.squaresArray.Length; i++)
        {
            for (int j = 0; j < _controller.squaresArray[i].transform.childCount; j++)
            {
                GameObject xOrCircle = _controller.squaresArray[i].transform.GetChild(j).gameObject;
                if (Controller.humanChoice != xOrCircle.name)
                {
                    xOrCircle.GetComponent<ColorManager>().SetColor(_noColor);
                }
            }
        }
    }

    public void SetAiOtherChoiceNoColor(GameObject __square)
    {
        Color32 __pieceColor;
        for (int __i = 0; __i < __square.transform.childCount; __i++)
        {
            GameObject __piece = __square.transform.GetChild(__i).gameObject;
            __pieceColor = __piece.GetComponent<ColorManager>().GetColor();
            if (__pieceColor.ToString().Equals(_alphaColor.ToString()))
            {
                __piece.GetComponent<ColorManager>().SetColor(_noColor);
            }
        }
    }

    public bool IsEqualsColors(Color32 __color1, Color32 __color2)
    {
        return __color1.ToString().Equals(__color2.ToString());
    }
}
