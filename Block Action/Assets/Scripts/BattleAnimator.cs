using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    private static BattleAnimator _battleAnimator;
    public static BattleAnimator battleAnimator
    {
        get
        {
            if (_battleAnimator == null)
            {
                _battleAnimator = FindObjectOfType<BattleAnimator>();
            }
            return _battleAnimator;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
