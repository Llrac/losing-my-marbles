using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RåttaProperties : Movement
{
    public int id = 0;
    private List<Vector2> moves1 = new();
    float timer = 5f;
    private void Start()
    {
        Movement.enemies.Add(this);
       


    }

    void Update()
    {
        moves1 = new List<Vector2>
        {
            new Vector2(0, 1),new Vector2(0, 1),new Vector2(0, 1),new Vector2(0, 1),new Vector2(0, 1),
            new Vector2(1, 2)
        };
        for (int i = 0; i < moves1.Count; i++)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                TryMove(gameObject, (int)moves1[i].x, (int)moves1[i].y);
            
                timer = 5f;
            }
        }
       
        
    }
    public override char ChangeTag()
    {
        return 'E';
    }
}