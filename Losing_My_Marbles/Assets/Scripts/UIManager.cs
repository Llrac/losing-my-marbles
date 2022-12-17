using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    public PlayerID playerID;
    
    [Header("Marbles & Slots")]
    public Transform[] marbleSlotsTop = new Transform[7];
    public Transform[] marbleSlotsBottom = new Transform[3];
    public List<Marble> marbleBag = new();
    public Transform marbleBagTransform;
    public Image[] marbleLights;

    [Header("Background & Lights")]
    public GameObject background = null;
    public Button confirmButton = null;
    public Image insertAlert = null;

    [Header("Timer")] 
    public Timer timer;
    
    [HideInInspector] public bool[] availableMarbleSlotsTop = new bool[7];
    public bool[] availableMarbleSlotsBottom = new bool[3];
    [HideInInspector] public List<Marble> discardBag = new();
    public int[] orderID = new int[3];

    private void Start()
    {
        // TODO connect this to matchmaking etc
        if (background != null)
        {
            background.GetComponent<Image>().sprite = background.GetComponent<PlayerColor>().backgroundColor[playerID.playerID - 1];
        }

        for (int i = 0; i < availableMarbleSlotsTop.Length; i++)
        {
            availableMarbleSlotsTop[i] = true;
        }
        
        for (int i = 0; i < availableMarbleSlotsBottom.Length; i++)
        {
            availableMarbleSlotsBottom[i] = true;
        }
        
        if (insertAlert != null)
            insertAlert.enabled = true;
        
        if (confirmButton != null)
            confirmButton.image.enabled = false;
        
        FillHandWithMarbles();
    }

    public void FillHandWithMarbles()
    {
        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().newMarbles);
        SetAllSlotsToAvailable();
        
        for (int i = 0; i < availableMarbleSlotsTop.Length; i++)
        {
            if (marbleBag.Count <= 0)
            {
                Shuffle();
                i--;
                
                if (marbleBag.Count <= 0)
                {
                    return;
                }
            }
            else if (availableMarbleSlotsTop[i])
            {
                Marble randomMarble = marbleBag[Random.Range(0, marbleBag.Count)];
                randomMarble.topRowIndex = i;
                
                if (marbleSlotsTop[i] != null)
                    randomMarble.transform.position = marbleSlotsTop[i].position;
                
                randomMarble.isInHand = true;
                availableMarbleSlotsTop[i] = false;
                marbleBag.Remove(randomMarble);
            }
        }
    }

    public bool MoveMarbleToBottomRow(GameObject marble)
    {
        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().selectMarble);
        for (int i = 0; i < availableMarbleSlotsBottom.Length; i++)
        {
            if (availableMarbleSlotsBottom[i])
            {
                Marble currentMarble = marble.GetComponent<Marble>();
                currentMarble.transform.position = marbleSlotsBottom[i].position;
                currentMarble.bottomRowIndex = i;
                availableMarbleSlotsBottom[i] = false;
                orderID[i] = currentMarble.marbleID;
                availableMarbleSlotsTop[currentMarble.topRowIndex] = true;
                
                if (confirmButton != null)
                    confirmButton.interactable = BottomRowFull();
                
                if (confirmButton != null)
                    confirmButton.image.enabled = BottomRowFull();
                
                marbleLights[i].enabled = true;
                
                return true;
            }
        }
        
        return false;
    }
    
    public bool MoveMarbleToTopRow(GameObject marble)
    {
        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().selectMarble);
        for (int i = 0; i < availableMarbleSlotsTop.Length; i++)
        {
            if (availableMarbleSlotsTop[i])
            {
                Marble currentMarble = marble.GetComponent<Marble>();
                currentMarble.transform.position = marbleSlotsTop[i].position;
                currentMarble.topRowIndex = i;
                availableMarbleSlotsTop[i] = false;
                availableMarbleSlotsBottom[currentMarble.bottomRowIndex] = true;
                
                if (confirmButton != null)
                    confirmButton.interactable = BottomRowFull();
                
                if (confirmButton != null)
                    confirmButton.image.enabled = BottomRowFull();
                
                marbleLights[currentMarble.bottomRowIndex].enabled = false;
                
                return false;
            }
        }
        
        return true;
    }

    private bool BottomRowFull()
    {
        for (int i = 0; i < availableMarbleSlotsBottom.Length; i++)
        {
            if (availableMarbleSlotsBottom[i])
            {
                if (insertAlert != null)
                    insertAlert.enabled = true;
                
                return false;
            }
        }
        
        if (insertAlert != null)
            insertAlert.enabled = false;
        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().marblesReady);
        return true;
    }

    public void Shuffle()
    {
        if (discardBag.Count >= 1)
        {
            foreach (Marble marble in discardBag)
            {
                marbleBag.Add(marble);
            }
            
            discardBag.Clear();
        }
    }

    public void DiscardMarblesFromHand()
    {
        Marble[] marblesInScene = FindObjectsOfType<Marble>();

        foreach (Marble marble in marblesInScene)
        {
            if (marble.isInHand)
            {
                discardBag.Add(marble);
                marble.isInHand = false;
                marble.isOnBottomRow = false;
                marble.gameObject.transform.position = new Vector2(-5000, -5000);
            }
        }
    }

    private void SetAllSlotsToAvailable()
    {
        for (int i = 0; i < availableMarbleSlotsTop.Length; i++)
        {
            availableMarbleSlotsTop[i] = true;
        }
        
        for (int i = 0; i < availableMarbleSlotsBottom.Length; i++)
        {
            availableMarbleSlotsBottom[i] = true;
            //marbleLights[i].enabled = false;
            
            if (confirmButton != null)
                confirmButton.interactable = false;
        }
    }

    public void PlayerCanInteractWithMarbles(bool canPickMarbles)
    {
        Marble[] marblesInScene = FindObjectsOfType<Marble>();
        
        foreach (Marble marble in marblesInScene)
        {
            marble.GetComponent<Button>().interactable = canPickMarbles;
        }
    }

    public void OnConfirmButtonClick()
    {
        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<AudioManager>().pressGo);
    }
}
