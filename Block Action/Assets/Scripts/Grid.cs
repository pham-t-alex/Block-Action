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
    static float b;
    public static float scale;

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

    LevelData levelData;
    public int levelNumber;
    void Start()
    {
        /*using (FileStream fs = File.OpenRead(path)) {
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
        }*/
        b = -3f; // top of the grid start

        levelData = Resources.Load<LevelData>($"Levels/Level {levelNumber}");

        string grid = levelData.gridAsString;
        StringReader s = new StringReader(grid);
        string line = s.ReadLine();

        while (line != null)
        {
            for (a = 0; a < line.Length; a++) // a is each column
            {
                if (line[a] == '~') // if it is a proper tile
                {
                    // place tile
                    GameObject tile = Instantiate(myPrefab, new Vector3((a * scale) - ((levelData.gridWidth * scale) / 2), b, 0), Quaternion.identity);
                    tile.transform.localScale = new Vector3(scale, scale, 0); // set scale
                    tiles.Add(tile); // add it to arraylist
                }
            }
            b -= scale; // move down a line
            line = s.ReadLine();
        }
    }

    void Update()
    {

    }

    public static void SetScale() {
        // open file

        /*
        using (FileStream fs = File.OpenRead(path)) {
            lnLength = 0; // grid length
            foreach (string l in System.IO.File.ReadLines(path)) { // go through each line in text file
                ++lnCount;
                if (lnLength < l.Length) lnLength = l.Length; // find max row size
            }
            scale = (lnCount <= lnLength) ? (5 / lnLength) : (5 / lnCount); // find which is longer
        }
        GridFitter.gridFitter.scale = scale; // assign scale for blocks
        */
        float scaleMaxSize = 4.5f; // the max length that the grid can be
        
        LevelData gridScLvl = Resources.Load<LevelData>($"Levels/Level {Battle.b.levelNumber}"); // grab level data because static method
        scale = (gridScLvl.gridHeight <= gridScLvl.gridWidth) ? (scaleMaxSize / gridScLvl.gridWidth) : (scaleMaxSize / gridScLvl.gridHeight); // assign scale for grid
        GridFitter.gridFitter.scale = scale; // assign scale for blocks
        
    }
}
