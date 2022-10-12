using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBlock : MonoBehaviour
{
    Vector3 prevMousePosition = new Vector3(0, 0, 0);
    public float selectedTime = 0;
    public bool placed;
    public bool mouseTouching;
    public int squareCount;
    public Collider2D blockCollider;
    public float relX;
    public float relY;

    // Start is called before the first frame update
    void Start()
    {
        blockCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        mouseTouching = true;
    }

    void OnMouseExit()
    {
        mouseTouching = false;    
    }
}
