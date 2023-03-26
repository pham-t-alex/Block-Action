using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeOutHandler : MonoBehaviour
{
    private static FakeOutHandler _fakeOutHandler;
    public static FakeOutHandler fakeOutHandler
    {
        get
        {
            if (_fakeOutHandler == null)
            {
                _fakeOutHandler = FindObjectOfType<FakeOutHandler>();
            }
            return _fakeOutHandler;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PersistentDataManager.levelNumber == 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoNothing()
    {
        GameText.setText("Nothing happened.");
        Battle.b.bs = BattleState.EnemyAction;
        ActionController.EnemyTurn();
    }
}
