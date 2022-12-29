using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDesktop : MonoBehaviour
{
    [Header("In game")]
    public Transform playLogTransform;

    public GameObject playerOrderPrefab;
    public Sprite[] playerSprite;

    public List<GameObject> playerBags = new();
    public List<GameObject> playerUIPosition = new();
    public List<Animator> playerBagsAnimator = new();
    
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

        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().playerReady);
    }

    public void ToggleReadyShine(int playerID, bool isShining)
    {
        GameObject child = playerUIPosition[playerID - 1].transform.GetChild(0).gameObject;
        
        child.SetActive(isShining);
        //Debug.Log(child);
    }
    
    //TODO Add a function that toggles ALL ready shines

    public void TogglePlayerBags(bool showBags)
    {
        if (showBags)
        {
            foreach (GameObject playerBag in playerBags)
            {
                playerBag.GetComponent<Image>().enabled = true;
            }
        }
        else
        {
            foreach (GameObject playerBag in playerBags)
            {
                playerBag.GetComponent<Image>().enabled = false;
            }
        }
    }
    
    

}
