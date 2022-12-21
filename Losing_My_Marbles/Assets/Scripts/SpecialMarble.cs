using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMarble : MonoBehaviour
{
    private GridGenerator gg;
    // sorted players 
    public static void Daze(PlayerProperties user)
    {
        for (int i = 0; i < TurnManager.players.Count; i++)
        {
            if(TurnManager.players[i] != user)
            {
                int turn = Random.Range(1, 3);
                TurnManager.players[i].GetComponent<Movement>().TryMove(TurnManager.players[i].gameObject, 1, turn);
            }
        }
    }
    public static void Earthquake(PlayerProperties user, float extraEarthquakes = 1)
    {
        for (int j = 0; j < extraEarthquakes; j++)
        {
            for (int i = 0; i < TurnManager.players.Count; i++)
            {
                if (TurnManager.players[i] != user)
                {
                    TurnManager.players[i].GetComponent<PlayerProperties>().Pushed(user.currentDirectionID);
                }
                // maybe add a way to not actually play an animation
            }

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
        if (TurnManager.players.Count <= 1)
        {
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
    public IEnumerator Bomb(PlayerProperties user, float area = 3)
    {
        GridManager gm = FindObjectOfType<GridManager>().GetComponent<GridManager>();
        GridGenerator gg = FindObjectOfType<GridGenerator>();
        PlayerProperties player = user;
        float zones = 1;
        Vector2 blastDir = user.gridPosition;
        for(int i = 0; i < area; i++)
        {
            for(int j = 2; j < 6; j++)
            {
                char result = 's';
                if (j % 2 == 0)
                {
                    result = gm.GetNexTile(user.gameObject, new Vector2(zones, 0));
                    blastDir = new Vector2(zones, 0);
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
                    blastDir = new Vector2(0, zones);
                    if (result == GridManager.PLAYER)
                    {
                        for (int k = 0; k < TurnManager.players.Count; k++)
                        {
                            if (TurnManager.players[k].gridPosition == user.gridPosition + new Vector2(0, zones))
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
                //add some texture
                gg.DeployBomb(user.gridPosition + blastDir);
            }
            yield return new WaitForSeconds(.5f);
            
            zones++;
        }
        
    }
    public static void BlockMove(PlayerProperties user, int currentTurn) // needs the current turn in turnmanager.
    {
        PlayerProperties victim = RandomizedPlayer(user); // needs to be sorted players
        int myIndex = 0;
        int opponentIndex = 0;
        for(int i = 0; i < TurnManager.players.Count; i++)
        {
            if(TurnManager.players[i] == user)
            {
                myIndex = i;
            }
            if (TurnManager.players[i] == victim)
            {
                opponentIndex = i;
            }
        }

        if(opponentIndex > myIndex)
        {
            victim.marbleEffect[currentTurn] = new Vector2(1, 0); // adds a scrap marble this turn
        }
        else if (opponentIndex < myIndex && currentTurn!= 2) 
        {
            victim.marbleEffect[currentTurn + 1] = new Vector2(1, 0);
        }
    }
    public static void RollerSkates(PlayerProperties user)
    {
        for (int i = 0; i < user.marbleEffect.Count; i++)
        {
            if(user.marbleEffect[i] != null && user.marbleEffect[i].x == 0)
            {
                user.marbleEffect[i] += new Vector2(0,3);
            }
        }
    }
    public static void Amplifier(PlayerProperties user)
    {
        for(int i = 0; i < user.marbleEffect.Count; i++)
        {
            switch (user.marbleEffect[i].x)
            {
                case 2: // blink
                    user.marbleEffect[i] += new Vector2(0, 3);
                    return;
                case 3: // earthquake
                    user.marbleEffect[i] += new Vector2(0, 2);
                    return;
                case 4: // bomb
                    user.marbleEffect[i] += new Vector2(0, 2);
                    return;
               
            }
        }
    }
    private static PlayerProperties RandomizedPlayer (PlayerProperties user)
    {
        if (TurnManager.players.Count <= 0)
        {
            return null;
        }

        int p = Random.Range(0, TurnManager.players.Count);

        if (TurnManager.players[p] == user)
        {
            while (TurnManager.players[p] == user)
            {
                p = Random.Range(0, TurnManager.players.Count);
                if (TurnManager.players[p] != user)
                {
                    return TurnManager.players[p];
                }
            }
        }
        else
        {
            return TurnManager.players[p];
        }
        return null; // something went wrong return null
    }
    public void ExecuteSpecialMarble(PlayerProperties user, float type, float amount = 1) // turnkeeper
    {
        switch (type)
        {
            case 3:
                Earthquake(user, amount);
                break;
            case 4:
                StartCoroutine(Bomb(user, amount));
                break;
            case 5:
                Daze(user);
                break;
            case 6:
                Magnet(user);
                break;
            case 7:
                Amplifier(user);
                break;
            case 8:
                BlockMove(user, 2); //default blocks the last move
                break;
            case 9:
                Swap(user);
                break;
        }
    }
}
