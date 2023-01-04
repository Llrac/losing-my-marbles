using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;

public class UIDesktop : MonoBehaviour
{
    [Header("Transition")]
    public Sprite[] tooltips;
    public static int orderInLevel = 0;
    readonly static List<int> tooltipOrder = new();
    [HideInInspector] public GameObject transitionScreen;
    [HideInInspector] public GameObject winScreen;
    [HideInInspector] public SkeletonGraphic skeleton;
    GameObject randomTooltip;
    GameObject loadingIcon;

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
                transitionScreen.SetActive(true);
            }
            
        }
        foreach (Transform child in desktopCanvas.transform)
        {
            if (child.gameObject.name == "WinScreen")
            {
                winScreen = child.gameObject;
            }
        }
        foreach (Transform child in winScreen.transform)
        {
            if (child.gameObject.name == "Win_Player")
            {
                skeleton = child.GetComponent<SkeletonGraphic>();
            }
        }
        foreach (Transform child in transitionScreen.transform)
        {
            if (child.gameObject.name == "Loading_Icon")
            {
                loadingIcon = child.gameObject;
            }
            else if (child.gameObject.name == "Random_Tooltip")
            {
                randomTooltip = child.gameObject;

                while (tooltipOrder.Count < tooltips.Length)
                {
                    int randomTooltip = UnityEngine.Random.Range(0, tooltips.Length);
                    if (!tooltipOrder.Contains(randomTooltip))
                    {
                        tooltipOrder.Add(randomTooltip);
                    }
                }
                randomTooltip.GetComponent<Image>().sprite = tooltips[tooltipOrder[orderInLevel - 1]];
            }
        }

        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        yield return new WaitForSeconds(3f);
        loadingIcon.GetComponent<Animator>().SetTrigger("fade_out");
        yield return new WaitForSeconds(0.75f);
        transitionScreen.GetComponent<Animator>().SetTrigger("go_away");
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
        
        if (playerWin == false) // if no mystery marbles in scene, do ...
        {
            StartCoroutine(NewLevel());
            playerWin = false;
            // insert load next scene function here
        }
    }
    private IEnumerator NewLevel()
    {
        yield return new WaitForSeconds(.5f);
        if (FindObjectOfType<MysteryMarble>() == null && playerWin == false) //should not happen if there is someone who won
        {
            FindObjectOfType<ResetManager>().NextLevel();
        }
    }
}
