using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{ 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown() {
        if (Battle.b.bs.Equals(BattleState.PlayerGrid))
        {
            Debug.Log("Beginning Battle Sequence\nPlayer Turn");
            Battle.b.bs = BattleState.PlayerAction;
        }
    }
}
