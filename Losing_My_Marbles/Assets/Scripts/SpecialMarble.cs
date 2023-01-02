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
                TurnManager.players[i].GetComponent<Movement>().TryMove(TurnManager.players[i].gameObject, 1, turn);
            }
        }
    }
    public IEnumerator Earthquake(PlayerProperties user, float extraEarthquakes = 1)
    {
        float savedTurnLenght = TurnManager.turnLength; 
        TurnManager.turnLength = extraEarthquakes * .5f;
        for (int j = 0; j < extraEarthquakes; j++)
        {
            for (int i = 0; i < TurnManager.players.Count; i++)
            {
                if (TurnManager.players[i] != user)
                {
                    TurnManager.players[i].GetComponent<PlayerProperties>().Pushed(user.currentDirectionID);
                }
                // maybe add a way to not actually play an animation // make into a coroutine
            }
            
            //TODO add screenshake and call to the server for phone rumble
            yield return new WaitForSeconds(.5f);
        }
        TurnManager.turnLength = savedTurnLenght;
    }
    public static void Magnet(PlayerProperties user)
    {
        //if(user.hasKey == false)
        //{
        //    for (int i = 0; i < TurnManager.players.Count; i++)
        //    {
        //        if (TurnManager.players[i] != user && TurnManager.players[i].hasKey == true)
        //        {
        //            TurnManager.players[i].GetComponent<PlayerProperties>().GetComponent<Animation>().DropKey(TurnManager.players[i].gameObject);
        //            return;
        //        }   
        //    }
        //    //maybe add a particle effect indicating you didnt steal a key
        //}
    }
    public void Swap(PlayerProperties user)
    {
        if (TurnManager.players.Count <= 0)
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
                { //save used variables
                    Vector2 targetDestination = TurnManager.players[p].GetComponent<PlayerProperties>().gridPosition;
                    Vector2 myPosition = user.gridPosition;
                    Vector2 targetWorldPosition = TurnManager.players[p].gameObject.transform.position;
                    Vector2 worldPosition = user.gameObject.transform.position;
                    char mySavedTile = user.savedTile;
                    char opponentSavedTile = TurnManager.players[p].savedTile;
                    //use saved variables
                    user.gridPosition = targetDestination;
                    user.gameObject.transform.position = targetWorldPosition;
                    TurnManager.players[p].gridPosition = myPosition;
                    TurnManager.players[p].transform.position = worldPosition;
                    user.savedTile = opponentSavedTile;
                    TurnManager.players[p].savedTile = mySavedTile;
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
            char mySavedTile = user.savedTile;
            char opponentSavedTile = TurnManager.players[p].savedTile;

            // create effect at your starting position
            if (user.GetComponent<Movement>().swapEffect != null)
            {
                GameObject newSwapEffect = Instantiate(user.GetComponent<Movement>().swapEffect, new Vector2(user.transform.position.x, user.transform.position.y + 0.5f), user.transform.rotation);
                newSwapEffect.GetComponent<Animator>().SetTrigger("swap_away");
                Destroy(newSwapEffect, 1f);
            }
            if (user.GetComponent<Animator>() != null)
            {
                user.GetComponent<Animator>().SetTrigger("shrink");
            }

            // create effect at your mate's position
            if (TurnManager.players[p].GetComponent<Movement>().swapEffect != null)
            {
                GameObject newSwapEffect = Instantiate(TurnManager.players[p].GetComponent<Movement>().swapEffect, new Vector2(TurnManager.players[p].transform.position.x, TurnManager.players[p].transform.position.y + 0.5f), TurnManager.players[p].transform.rotation);
                newSwapEffect.GetComponent<Animator>().SetTrigger("swap_away");
                Destroy(newSwapEffect, 1f);
            }
            if (TurnManager.players[p].GetComponent<Animator>() != null)
            {
                TurnManager.players[p].GetComponent<Animator>().SetTrigger("shrink");
            }

            StartCoroutine(SwapAnimation(user, p, targetDestination, myPosition, targetWorldPosition, worldPosition, mySavedTile, opponentSavedTile));
            return;
        }
    }

    private static IEnumerator SwapAnimation(PlayerProperties user, int p, Vector2 targetDestination, Vector2 myPosition, Vector2 targetWorldPosition, Vector2 worldPosition, char mySavedTile, char opponentSavedTile)
    {
        yield return new WaitForSeconds(0.5f);

        user.gridPosition = targetDestination;
        user.gameObject.transform.position = targetWorldPosition;
        TurnManager.players[p].gridPosition = myPosition;
        TurnManager.players[p].transform.position = worldPosition;
        user.savedTile = opponentSavedTile;
        TurnManager.players[p].savedTile = mySavedTile;

        user.gridPosition = targetDestination;
        user.gameObject.transform.position = targetWorldPosition;
        TurnManager.players[p].gridPosition = myPosition;
        TurnManager.players[p].transform.position = worldPosition;
        user.savedTile = opponentSavedTile;
        TurnManager.players[p].savedTile = mySavedTile;

        // create effect at your destination
        if (user.GetComponent<Movement>().swapEffect != null)
        {
            GameObject newSwapEffect = Instantiate(user.GetComponent<Movement>().swapEffect, new Vector2(user.transform.position.x, user.transform.position.y + 0.5f), user.transform.rotation);
            newSwapEffect.GetComponent<Animator>().SetTrigger("swap_away");
            Destroy(newSwapEffect, 1f);
        }
        if (user.GetComponent<Animator>() != null)
        {
            user.GetComponent<Animator>().SetTrigger("grow");
        }

        // create effect at your mate's destination
        if (TurnManager.players[p].GetComponent<Movement>().swapEffect != null)
        {
            GameObject newSwapEffect = Instantiate(TurnManager.players[p].GetComponent<Movement>().swapEffect, new Vector2(TurnManager.players[p].transform.position.x, TurnManager.players[p].transform.position.y + 0.5f), TurnManager.players[p].transform.rotation);
            newSwapEffect.GetComponent<Animator>().SetTrigger("swap_to");
            Destroy(newSwapEffect, 1f);
        }
        if (TurnManager.players[p].GetComponent<Animator>() != null)
        {
            TurnManager.players[p].GetComponent<Animator>().SetTrigger("grow");
        }
    }

    public IEnumerator Bomb(PlayerProperties user, float area = 3)
    {
        GridManager gm = FindObjectOfType<GridManager>().GetComponent<GridManager>();
        GridGenerator gg = FindObjectOfType<GridGenerator>();

        TurnManager.turnLength = area * .5f; // makes sure the effect gets played before the next person begins their next move

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
                if(result != GridManager.EMPTY)
                {
                    gg.DeployBomb(user.gridPosition + blastDir);
                }
                
            }
            yield return new WaitForSeconds(.5f);
            
            zones++;
        }
        TurnManager.turnLength = .5f;
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
                    break;
                case 3: // earthquake
                    user.marbleEffect[i] += new Vector2(0, 2);
                    Debug.Log(user.marbleEffect[i]);
                    break;
                case 4: // bomb
                    user.marbleEffect[i] += new Vector2(0, 2);
                    break;
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
    public void ExecuteSpecialMarble(PlayerProperties user, float type, float amount = 1, int currentTurn = 0) // turnkeeper
    {
        switch (type)
        {
            case 3:
                StartCoroutine(Earthquake(user, amount));
                if (GetComponent<AudioSource>() != null)
                    GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().triggerEarthquake);
                break;
            case 4:
                StartCoroutine(Bomb(user, amount));
                if (GetComponent<AudioSource>() != null)
                    GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().triggerBomb);
                break;
            case 5:
                Daze(user);
                if (GetComponent<AudioSource>() != null)
                    GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().triggerDaze);
                break;
            case 6:
                Magnet(user);
                break;
            case 7:
                Amplifier(user);
                if (GetComponent<AudioSource>() != null)
                    GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().triggerAmplifier);
                break;
            case 8:
                BlockMove(user, currentTurn);
                if (GetComponent<AudioSource>() != null)
                    GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().triggerBlock);
                break;
            case 9:
                Swap(user);
                if (GetComponent<AudioSource>() != null)
                    GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().triggerSwap);
                break;
            case 10:
                RollerSkates(user);
                if (GetComponent<AudioSource>() != null)
                    GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().triggerRollerskates);
                break;
        }
    }
}
