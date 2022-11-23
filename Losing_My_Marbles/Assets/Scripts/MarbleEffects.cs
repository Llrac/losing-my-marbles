using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleEffects : MonoBehaviour
{
    public List<GameObject> marblesToTriggerList = new();

    PlayerMovement pMovement;

    // Start is called before the first frame update
    void Start()
    {
        pMovement = FindObjectOfType<PlayerMovement>();
    }

    public void TriggerMarbles()
    {
        for (int i = 0; i < marblesToTriggerList.Count; i++)
        {
            Debug.Log(i);
        }
    }
}
