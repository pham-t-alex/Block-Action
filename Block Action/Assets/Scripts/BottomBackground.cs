using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomBackground : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float bottomEdge = -1 * Camera.main.orthographicSize;
        float height = (-1 * bottomEdge * 2) * 0.3875f;
        float y = bottomEdge + (height / 2);
        transform.position = new Vector3(0, y, 0);
        float width = 2 * Camera.main.orthographicSize * Screen.width / Screen.height;
        float scaleX = width / 28f;
        float scaleY = height / 6.2f;
        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
