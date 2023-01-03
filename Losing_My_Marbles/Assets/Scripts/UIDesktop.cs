using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDesktop : MonoBehaviour
{
    [Header("Transition")]
    public GameObject transitionScreen = null;

    [Header("In game")]
    public Transform playLogTransform;
    public GameObject playerOrderPrefab;
    public Sprite[] playerSprite;

    public List<GameObject> playerBags = new();
    public List<GameObject> playerUIPosition = new();
    public List<Animator> playerBagsAnimator = new();
    public List<GameObject> playerPickupMarbles = new();

    private bool playerWin = false;
    // references
    //TurnManager tm;

    void Start()
    {
        GameObject desktopCanvas = FindObjectOfType<Canvas>().gameObject;
        foreach (Transform child in desktopCanvas.transform)
        {
            if (child.gameObject.name == "Transition")
            {
                transitionScreen = child.gameObject;
            }
        }

        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        yield return new WaitForSeconds(4f);
        Debug.Log("fade out");
        transitionScreen.GetComponent<Animator>().SetTrigger("fade_out");
        yield return null;
    }
    
    public void InstantiatePlayerOrder(int playerId)
    {
       var newPlayerPosition = Instantiate(playerOrderPrefab, transform.position, Quaternion.identity);
       newPlayerPosition.transform.SetParent(playLogTransform, false);
       newPlayerPosition.GetComponent<Image>().sprite = playerSprite[playerId - 1];
    }

    public void ClearPlayerOrder()
    {
        foreach (Transform child in playLogTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public void TurnOnMarbleBagAnimation()
    {
        foreach (Animator animator in playerBagsAnimator)
        {
            animator.SetBool("chosen marbles", false);
        }
        
    }

    public void TurnOffMarbleBagAnimation(int playerID)
    {
        playerBags[playerID - 1].GetComponent<Animator>().SetBool("chosen marbles", true);
        
        ToggleReadyShine(playerID, true);

       
    }

    public void ToggleReadyShine(int playerID, bool isShining)
    {
        GameObject child = playerUIPosition[playerID - 1].transform.GetChild(0).gameObject;
        
        child.SetActive(isShining);

        if(isShining == true)
        {
            if (GetComponent<AudioSource>() != null)
                GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().playerReady);
        }
       
        //Debug.Log(child);
    }
    
    //TODO Add a function that toggles ALL ready shines

    public void TogglePlayerBags(bool showBags, int playerID = 0)
    {
        if (showBags)
        {// this code should run at once
            foreach (GameObject playerBag in playerBags)
            {
                playerBag.GetComponent<Image>().enabled = true;
            }
        }
        else
        {//this code should run asynchronolsly
            playerUIPosition[playerID - 1].transform.GetChild(1).GetComponent<Image>().enabled = false;
            //foreach (GameObject playerBag in playerBags)
            //{
            //    playerBag.GetComponent<Image>().enabled = false;
            //}
        }
    }
    
    public void UpdatePickupMarbles(GameObject player)
    {
        for (int i = 0; i < player.GetComponent<PlayerProperties>().specialMarbleCount; i++)
        {
            GameObject child = playerPickupMarbles[player.GetComponent<PlayerProperties>().playerID - 1].transform.GetChild(i).gameObject;
            child.SetActive(true);
            if (player.GetComponent<PlayerProperties>().specialMarbleCount >= 3)
            {
                ResetManager.PlayerWin(player.GetComponent<PlayerProperties>().playerID);
                playerWin = true;
            }
        }
        
        if (FindObjectOfType<MysteryMarble>() == null && playerWin == false) // if no mystery marbles in scene, do ...
        {
           
         // insert load next scene function here
        }
    }
    
    

}
