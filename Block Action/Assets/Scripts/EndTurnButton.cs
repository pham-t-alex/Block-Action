using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    public GameObject endTurnButton;
    Image img;
    public Sprite lantern;
    public Sprite darkLantern;
    // Start is called before the first frame update
    void Start()
    {
        img = endTurnButton.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Battle.b.bs == BattleState.PlayerAction || Battle.b.bs == BattleState.EnemyAction)
        {
            img.sprite = darkLantern;
        }
        else
        {
            img.sprite = lantern;
        }
    }

    public void OnMouseDown() {
        if (Battle.b.bs.Equals(BattleState.PlayerGrid))
        {
            Debug.Log("Beginning Battle Sequence\nPlayer Turn");
            Battle.b.bs = BattleState.PlayerAction;
            BottomDarkener.DarkenBottom();
            ActionController.PlayerTurn();
        }
    }
}
