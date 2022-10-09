using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    //variable that defines the hp of the player
    public static int health;
    public static bool playerTurn;
    public static bool gameOver;

    public int[] attacks;


    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        gameOver = false;
        //the set of attacks the player has. the elements in the array should be the damage it deals
        attacks = new int[] {1, 2, 3, 4, 5};

        //change this later
        playerTurn = false;
    }

    // Update is called once per frame
    void Update()
    {
        //this condition should check if the player hit the end turn button instead, but for now do this
        if (playerTurn && !gameOver) {
            //attack animation
            //take attack power from attacks[]
            //enemy takes damage
            Enemy.health -= attacks[1] * 10;
            Debug.Log("Enemy hp: " + Enemy.health);
            //enemy's turn
            playerTurn = false;
            Enemy.enemyTurn = true;
        }

        if (health <= 0 && !gameOver) {
            //game over
            gameOver = true;
            Debug.Log("game over");
        }
    }
}
