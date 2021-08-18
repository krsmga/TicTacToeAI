using UnityEngine;

public class Square : MonoBehaviour
{
    private bool _isChecked;
    public bool isChecked
    {
        get { return _isChecked; }
        set 
        { 
            _isChecked = value;
        }
    }
}
