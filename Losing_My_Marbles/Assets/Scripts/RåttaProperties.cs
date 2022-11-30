using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RåttaProperties : Movement
{
    public int id = 0;
    private Vector2[] moves1;
    float timer = 10f;
    private void Start()
    {
        Movement.enemies.Add(this);
        moves1 = new Vector2[8]
        {new Vector2(0,1),new Vector2(0,1),new Vector2(0,1),new Vector2(0,1),new Vector2(0,1),
         new Vector2(0,1),new Vector2(0,1),new Vector2(1,2),
        };
    }
    void Update()
    {
        // Diagonal movement
        for(int i = 0; i < moves1.Length; i++)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                TryMove(enemies[1].gameObject, (int)moves1[i].x, (int)moves1[i].y);
                timer = 100f;
            }
        }
        
    }
    public override char ChangeTag()
    {
        return 'E';
    }
}