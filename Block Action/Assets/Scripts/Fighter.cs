using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    public int health;
    public double buff;
    public List<BuffCounter> buffLeft;
    public int maxHealth;

    void Start() {
        
    }

    void update() { 
    
    }
}
