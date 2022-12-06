using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatProperties : Movement
{
    private void Start()
    {
        Movement.enemies.Add(this);
    }

    private void OnDestroy()
    {
        Movement.enemies.Remove(this);
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