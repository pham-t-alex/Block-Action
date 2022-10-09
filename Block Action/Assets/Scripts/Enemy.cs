using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //variable that defines the hp of the enemy
    public static int health;
    public static bool enemyTurn;

    public int[] attacks;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        //the set of attacks the enemy has. the elements in the array should be the damage it deals
        attacks = new int[] { 1, 2, 3, 4, 5};
        enemyTurn = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (enemyTurn && !Player.gameOver) {
            //attack animation
            //player takes damage
            Player.health -= attacks[1] * 10;
            Debug.Log("Player hp: " + Player.health);
            //player's turn
            enemyTurn = false;
        }

        if (health <= 0) { 
            //destory obj
        }
    }
}
