using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileUI : MonoBehaviour
{
    public PlayerID playerID;
    public GameObject background;
    public Button confirmButton;
    public Image insertAlert;
    
    // Start is called before the first frame update
    void Start()
    {
        background.GetComponent<Image>().sprite = background.GetComponent<PlayerColor>().backgroundColor[PlayerID.playerID];
        
        insertAlert.enabled = true;
        confirmButton.image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
