using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    public List<Enemy> enemies;
    public static FighterController fighterController;

    // Start is called before the first frame update
    void Start()
    {
        fighterController = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
