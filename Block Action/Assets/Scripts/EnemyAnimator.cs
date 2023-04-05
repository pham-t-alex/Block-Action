using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public static Animator animator;
    public static bool attackDone;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        attackDone = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Calls animation
    // @param trigger the trigger connected to the animation
    public static void SetTrigger(string trigger)
    {
        //Starts animation 
        animator.SetTrigger(trigger);
    }

    public void AttackDone()
    {
        attackDone = true;
    }
}
