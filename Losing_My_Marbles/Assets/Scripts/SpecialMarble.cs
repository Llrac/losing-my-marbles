using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMarble : MonoBehaviour
{
    public static void Daze(PlayerProperties user)
    {
        for (int i = 0; i < TurnManager.players.Count; i++)
        {
            if(TurnManager.players[i] != user)
            {
                int turn = Random.Range(1, 3);
                TurnManager.players[i].GetComponent<Movement>().TryMove(TurnManager.players[i].gameObject, 1,turn);
            }
        }
    }
    public static void Earthquake(PlayerProperties user)
    {
        for (int i = 0; i < TurnManager.players.Count; i++)
        {
            if (TurnManager.players[i] != user)
            {
                TurnManager.players[i].GetComponent<PlayerProperties>().Pushed(user.currentDirectionID);
            }
            // maybe adda a way to not actually play an animation
        }
    }
    public static void Magnet(PlayerProperties user)
    {
        if(user.hasKey == false)
        {
            for (int i = 0; i < TurnManager.players.Count; i++)
            {
                if (TurnManager.players[i] != user && TurnManager.players[i].hasKey == true)
                {
                    TurnManager.players[i].GetComponent<PlayerProperties>().GetComponent<Animation>().DropKey(TurnManager.players[i].gameObject);
                    return;
                }   
            }
            //maybe add a particle effect indicating you didnt steal a key
        }
    }
    public static void Swap(PlayerProperties user)
    {
        if (TurnManager.players.Count <= 0)
        {
            Debug.Log("hig");
            return;
        }

        int p = Random.Range(0, TurnManager.players.Count);

        if (TurnManager.players[p] == user)
        {
            while (TurnManager.players[p] == user)
            {
                p = Random.Range(0, TurnManager.players.Count);
                if (TurnManager.players[p] != user)
                {
                    Vector2 targetDestination = TurnManager.players[p].GetComponent<PlayerProperties>().gridPosition;
                    Vector2 myPosition = user.gridPosition;
                    Vector2 targetWorldPosition = TurnManager.players[p].gameObject.transform.position;
                    Vector2 worldPosition = user.gameObject.transform.position;

                    user.gridPosition = targetDestination;
                    user.gameObject.transform.position = targetWorldPosition;
                    TurnManager.players[p].gridPosition = myPosition;
                    TurnManager.players[p].transform.position = worldPosition;
                    return;
                }
            }
        }
        else
        {
            Vector2 targetDestination = TurnManager.players[p].GetComponent<PlayerProperties>().gridPosition;
            Vector2 myPosition = user.gridPosition;
            Vector2 targetWorldPosition = TurnManager.players[p].gameObject.transform.position;
            Vector2 worldPosition = user.gameObject.transform.position;

            user.gridPosition = targetDestination;
            user.gameObject.transform.position = targetWorldPosition;
            TurnManager.players[p].gridPosition = myPosition;
            TurnManager.players[p].transform.position = worldPosition;
            return;
        }
    }
    public static IEnumerator Bomb(PlayerProperties user)
    {
        GridManager gm = FindObjectOfType<GridManager>().GetComponent<GridManager>();
        PlayerProperties player = user;
        float zones = 1;
        for(int i = 0; i < 3; i++)
        {
            for(int j = 2; j < 6; j++)
            {
                char result = 's';
                if (j % 2 == 0)
                {
                    result = gm.GetNexTile(user.gameObject, new Vector2(zones, 0));
                    
                    if (result == GridManager.PLAYER)
                    {
                        for (int k = 0; k < TurnManager.players.Count; k++)
                        {
                            if (TurnManager.players[k].gridPosition == user.gridPosition + new Vector2(zones, 0))
                            {
                                player = TurnManager.players[k];
                            }
                        }
                        switch (zones > 0)
                        {
                            case true:
                                player.Pushed(1);
                                //push upward
                                break;
                            case false:
                                player.Pushed(3);
                                //push downward
                                break;
                        }
                    }
                    zones *= -1;
                }
                else
                {
                    result = gm.GetNexTile(user.gameObject, new Vector2(0, zones)); // not working get back to it
                    if (result == GridManager.PLAYER)
                    {
                        for (int k = 0; k < TurnManager.players.Count; k++)
                        {
                            if (TurnManager.players[k].gridPosition == user.gridPosition + new Vector2(zones, 0))
                            {
                                player = TurnManager.players[k];
                            }
                        }
                        switch (zones > 0)
                        {
                            case true:
                                player.Pushed(0);
                                //push right
                                break;
                            case false:
                                player.Pushed(2);
                                //push left
                                break;
                        }
                    }
                }
                
            }
            
            zones++;
        }
        yield return new WaitForSeconds(2f);
    }

}
