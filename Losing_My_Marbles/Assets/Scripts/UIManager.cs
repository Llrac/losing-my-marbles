using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    public PlayerID playerID;
    [Header("Marbles & Slots")]
    public Transform[] marbleSlotsTop = new Transform[7];
    public Transform[] marbleSlotsBottom = new Transform[5];
    public List<Marble> marbleBag = new();
    public Transform marbleBagTransform;
    public Image[] marbleLights;

    [Header("Background & Lights")]
    public GameObject background;

    public Button confirmButton;
    public Image insertAlert;

    [HideInInspector] public bool[] availableMarbleSlotsTop = new bool[7];
    [HideInInspector] public bool[] availableMarbleSlotsBottom = new bool[5];
    [HideInInspector] public List<Marble> discardBag = new();
    [HideInInspector] public int[] orderID = new int[5];

    private void Start()
    {
        // TODO connect this to matchmaking etc
        if (background != null)
        {
            background.GetComponent<Image>().sprite = background.GetComponent<PlayerColor>().backgroundColor[playerID.playerID];
        }
        
        for (int i = 0; i < availableMarbleSlotsTop.Length; i++)
        {
            availableMarbleSlotsTop[i] = true;
        }
        
        for (int i = 0; i < availableMarbleSlotsBottom.Length; i++)
        {
            availableMarbleSlotsBottom[i] = true;
        }

        insertAlert.enabled = true;
        confirmButton.image.enabled = false;
        FillHandWithMarbles();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            FillHandWithMarbles();
        }
    }

    public void FillHandWithMarbles()
    {
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
                confirmButton.interactable = BottomRowFull();
                confirmButton.image.enabled = BottomRowFull();
                marbleLights[i].enabled = true;
                return true;
            }
        }
        
        return false;
    }
    
    public bool MoveMarbleToTopRow(GameObject marble)
    {
        for (int i = 0; i < availableMarbleSlotsTop.Length; i++)
        {
            if (availableMarbleSlotsTop[i])
            {
                Marble currentMarble = marble.GetComponent<Marble>();
                currentMarble.transform.position = marbleSlotsTop[i].position;
                currentMarble.topRowIndex = i;
                availableMarbleSlotsTop[i] = false;
                availableMarbleSlotsBottom[currentMarble.bottomRowIndex] = true;
                confirmButton.interactable = BottomRowFull();
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
                insertAlert.enabled = true;
                return false;
            }
        }

        insertAlert.enabled = false;
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
            marbleLights[i].enabled = false;
            if (confirmButton != null)
                confirmButton.interactable = false;
        }
    }

}
