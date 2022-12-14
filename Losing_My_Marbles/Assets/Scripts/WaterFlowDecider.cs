using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlowDecider : MonoBehaviour
{
    public Vector2 gridPos = Vector2.zero;
    public int flowDirection = 0;
    private void Awake()
    {
        Environment.waterFlowDeciders.Add(this);
    }
}
