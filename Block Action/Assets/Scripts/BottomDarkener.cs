using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomDarkener : MonoBehaviour
{
    private static BottomDarkener _bottomDarkener;
    public static BottomDarkener bottomDarkener
    {
        get
        {
            if (_bottomDarkener == null)
            {
                _bottomDarkener = FindObjectOfType<BottomDarkener>();
            }
            return _bottomDarkener;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        float bottomEdge = -1 * Camera.main.orthographicSize;
        float height = (-1 * bottomEdge * 2) * 0.3875f;
        float y = bottomEdge + (height / 2);
        transform.position = new Vector3(0, y, 0);
        float width = 2 * Camera.main.orthographicSize * Screen.width / Screen.height;
        float scaleX = 40;
        float scaleY = 6.2f;
        transform.localScale = new Vector3(scaleX, scaleY, 1);
        _bottomDarkener = this;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void DarkenBottom()
    {
        bottomDarkener.gameObject.SetActive(true);
    }

    public static void UndarkenBottom()
    {
        bottomDarkener.gameObject.SetActive(false);
    }
}
