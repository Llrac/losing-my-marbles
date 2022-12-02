using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RåttaProperties : Movement
{
    float tim = 1f;
    private void Start()
    {
        Movement.enemies.Add(this);
    }

    private void Update()
    {
        //tim -= Time.deltaTime;
        //if (tim < 0f)
        //{
        //    DoAMove(1);
        //    tim = 1f;
        //}
    }
    public override char ChangeTag()
    {
        return 'E';
    }

    public override void DoAMove(int inc)
    {
        TryMove(gameObject, 0, inc);
    }
}