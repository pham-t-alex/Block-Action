using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    public float spaceBetweenEnemies;
    public float bottomOffset;
    public float minRightOffset;

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
        PlacePlayer();
        PlaceEnemies();
    }

    static void PlacePlayer()
    {
        float y = -1 * Camera.main.orthographicSize;
        y += fighterController.bottomOffset;
        SpriteRenderer playerSprite = Player.player.GetComponent<SpriteRenderer>();
        y += playerSprite.bounds.size.y / 2;
        Player.player.transform.position = new Vector3(-7, y, 0);
    }

    static void PlaceEnemies()
    {
        float y = -1 * Camera.main.orthographicSize;
        y += fighterController.bottomOffset;
        float totalWidth = 0;
        for (int i = 0; i < Battle.b.enemies.Count; i++)
        {
            if (i > 0)
            {
                totalWidth += fighterController.spaceBetweenEnemies;
            }
            totalWidth += Battle.b.enemies[i].GetComponent<SpriteRenderer>().bounds.size.x;
        }
        float rightOffset = 0;
        if (totalWidth < (Camera.main.orthographicSize * Screen.width / Screen.height) - 2 * (fighterController.minRightOffset))
        {
            rightOffset = ((Camera.main.orthographicSize * Screen.width / Screen.height) - totalWidth) / 2;
        } else
        {
            rightOffset = fighterController.minRightOffset;
        }
        float x = (Camera.main.orthographicSize * Screen.width / Screen.height) - rightOffset;
        for (int i = Battle.b.enemies.Count - 1; i >= 0; i--)
        {
            SpriteRenderer spriteRenderer = Battle.b.enemies[i].GetComponent<SpriteRenderer>();
            x -= spriteRenderer.bounds.size.x / 2;
            y += spriteRenderer.bounds.size.y / 2;
            Battle.b.enemies[i].transform.position = new Vector3(x, y, 0);
            x -= spriteRenderer.bounds.size.x / 2;
            y -= spriteRenderer.bounds.size.y / 2;
            x -= fighterController.spaceBetweenEnemies;
        }
    }
}
