using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public Animator animator;
    public bool attackDone;
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

    public void Animate(Animator a)
    {
        animator = a;
        attackDone = false;
    }

    // Calls animation
    // @param trigger the trigger connected to the animation
    public void SetTrigger(string trigger)
    {
        //Starts animation 
        animator.SetTrigger(trigger);
    }

    public void EnemyAttackDone()
    {
        attackDone = true;
        Debug.Log("pls");
    }
}
