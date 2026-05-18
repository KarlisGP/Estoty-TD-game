using UnityEngine;


public class Goblin : EnemyBase
{
    protected override void Start()
    {
        base.Start(); // Runs the pathfinding setup in the base script
        health = 1;   // Overwrite attributes
        speed = 2;    
    }
}

