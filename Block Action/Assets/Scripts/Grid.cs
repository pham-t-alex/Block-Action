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
    static int i, a, lnCount, lnLength;
    static float b, scale;

    // text file
    public static string path = "Assets/TextFiles/gridtest.txt";
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
            // VERSION 2
            // count number of lines & length

            // variables
            b = -2.5f;

            // grid alignment/placing
            foreach (string line in System.IO.File.ReadLines(path))
            {
                for (a = 0; a < line.Length; a++) // a is each column
                {
                    if (line[a] == '~') // if it is a proper tile
                    {
                        // place tile
                        GameObject tile = Instantiate(myPrefab, new Vector3((a * scale) - ((lnLength * scale) / 2), b, 0), Quaternion.identity);
                        tile.transform.localScale = new Vector3(scale, scale, 0); // set scale
                        tiles.Add(tile); // add it to arraylist
                    }
                }
                b -= scale; // move down a line
            }
        }
    }

    void Update()
    {

    }

    public static void SetScale() {
        // open file
        using (FileStream fs = File.OpenRead(path)) {
            lnLength = 0; // grid length
            foreach (string l in System.IO.File.ReadLines(path)) { // go through each line in text file
                ++lnCount;
                if (lnLength < l.Length) lnLength = l.Length; // find max row size
            }
            scale = (lnCount <= lnLength) ? (5 / lnLength) : (5 / lnCount); // find which is longer
        }
        GridFitter.gridFitter.scale = scale; // assign scale for blocks
    }
}
