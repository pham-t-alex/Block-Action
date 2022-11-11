using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDarkener : MonoBehaviour
{
    private static ScreenDarkener _screenDarkener;
    public static ScreenDarkener screenDarkener
    {
        get
        {
            if (_screenDarkener == null)
            {
                _screenDarkener = FindObjectOfType<ScreenDarkener>();
            }
            return _screenDarkener;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void DarkenScreen()
    {
        screenDarkener.gameObject.SetActive(true);
    }

    public static void UndarkenScreen()
    {
        screenDarkener.gameObject.SetActive(false);
    }
}
