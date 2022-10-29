using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    private static FighterController _fighterController;
    public static FighterController fighterController
    {
        get
        {
            if (_fighterController == null)
            {
                _fighterController = FindObjectOfType<FighterController>();
            }
            return _fighterController;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PlaceFighters()
    {

    }
}
