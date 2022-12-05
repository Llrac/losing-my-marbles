using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatProperties : Movement
{
    int savedDir;
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

    public override void DoAMove(int inc , int dir)
    {
        savedDir = gameObject.GetComponent<RatProperties>().currentDirectionID;
        gameObject.GetComponent<RatProperties>().currentDirectionID = dir;
        TryMove(gameObject, 0, inc);
        gameObject.GetComponent<RatProperties>().currentDirectionID = savedDir;
    }
}