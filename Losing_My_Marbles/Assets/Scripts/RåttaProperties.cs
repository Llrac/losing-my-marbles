using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RåttaProperties : Movement
{
    private void Start()
    {
        Movement.enemies.Add(this);
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