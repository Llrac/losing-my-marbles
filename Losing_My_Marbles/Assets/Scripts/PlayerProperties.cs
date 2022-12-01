using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : Movement
{
    public GameObject move1;
    public GameObject move2;
    public GameObject move3;
    public GameObject left1;
    public GameObject right1;

    TurnManager tm;

    private void Start()
    {
        tm = FindObjectOfType<TurnManager>();
    }

    void Update()
    {
        // Diagonal movement
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            tm.globalOrderID++;
            move1.GetComponent<Marble>().orderID += tm.globalOrderID;
            tm.selectedMarbles.Add(move1);
            tm.OrderSelectedMarbles();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            tm.globalOrderID++;
            move2.GetComponent<Marble>().orderID += tm.globalOrderID;
            tm.selectedMarbles.Add(move2);
            tm.OrderSelectedMarbles();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            tm.globalOrderID++;
            move3.GetComponent<Marble>().orderID += tm.globalOrderID;
            tm.selectedMarbles.Add(move3);
            tm.OrderSelectedMarbles();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            tm.globalOrderID++;
            left1.GetComponent<Marble>().orderID += tm.globalOrderID;
            tm.selectedMarbles.Add(left1);
            tm.OrderSelectedMarbles();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            tm.globalOrderID++;
            right1.GetComponent<Marble>().orderID += tm.globalOrderID;
            tm.selectedMarbles.Add(right1);
            tm.OrderSelectedMarbles();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            TryMove(gameObject, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            //TryMove(gameObject, 0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            TryMove(gameObject, 1, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            TryMove(gameObject, 1, 1);
        }
    }
    public override char ChangeTag()
    {
        return 'P';
    }
}
