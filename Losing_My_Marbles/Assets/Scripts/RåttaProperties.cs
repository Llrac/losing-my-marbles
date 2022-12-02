using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RåttaProperties : Movement
{
    int savedDir;
    private void Awake()
    {
        Movement.enemies.Add(this);
    }

    public override char ChangeTag()
    {
        return 'E';
    }

    public override void DoAMove(int inc, int dir)
    {
        savedDir = gameObject.GetComponent<RåttaProperties>().currentDirectionID;
        gameObject.GetComponent<RåttaProperties>().currentDirectionID = dir;
        TryMove(gameObject, 0, inc);
        gameObject.GetComponent<RåttaProperties>().currentDirectionID = savedDir;
        // 
    }
}