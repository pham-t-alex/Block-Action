using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Calls animation
    // @param trigger the trigger connected to the animation
    public void SetTrigger(string trigger)
    {
        //Starts animation 
        animator.SetTrigger(trigger);
    }

    //Animation event
    public void SwitchTurn()
    {
        Battle.b.bs = BattleState.EnemyAction;
        Debug.Log("Enemy Turn");
    }
}