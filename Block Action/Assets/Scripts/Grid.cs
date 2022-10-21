using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Grid : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> tiles;
    public GameObject myPrefab;
    public List<SoulObject> soulObjectsInGrid;
    int i, a, lnCount, lnLength;
    float b;

    // text file
    public string path = "Assets/TextFiles/gridtest.txt";
    /*
    Hi, dunno where to put this, so here's the current key.

    First off, one important thing to note is that it reads the bottom line first.
    So, that line will be the top. Will that get patched? probably.

    Key:
    _: empty
    ~: regular tile

    */
    void Start()
    {
        using (FileStream fs = File.OpenRead(path)) {
            // VERSION 1
            /*
            i = 0;
            aMax = 5;
            bMax = 5;
            for (a = 0; a < aMax; a++) {
                for (b = 0; b < bMax; b++) {
                    tiles.Add(Instantiate(myPrefab, new Vector3(a * 2, b * 2 - 4, 0), Quaternion.identity));
                    i++;
                }
            }
            */

            // VERSION 2
            // count number of lines & length
            foreach (string l in System.IO.File.ReadLines(path)) {
                ++lnCount;
            }

            // variable for center
            b = (float) lnCount / 2;

            // grid alignment/placing
            foreach (string line in System.IO.File.ReadLines(path)) {  
                for (a = 0; a < line.Length; a++){
                    if (line[a] == '~') {
                        tiles.Add(Instantiate(myPrefab, new Vector3(-12 + a, b - (float) 0.5, 0), Quaternion.identity));
                    }
                }
                b--;
            } 
        }
    }

    void Update()
    {

    }
}
