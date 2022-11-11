using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisplayer : MonoBehaviour
{
    // Start is called before the first frame update
    private static UIDisplayer _uiDisplayer;
    public static UIDisplayer uiDisplayer {
        get
        {
            if (_uiDisplayer == null)
            {
                _uiDisplayer = FindObjectOfType<UIDisplayer>();
            }
            return _uiDisplayer;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
