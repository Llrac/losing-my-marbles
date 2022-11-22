using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleManager : MonoBehaviour
{
    GameObject highlight;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Highlight")
            {
                highlight = child.gameObject;
            }
        }
    }

    public void GetHighlight(GameObject marbleToHighlight)
    {
        Marble marbleScript = marbleToHighlight.GetComponent<Marble>();
        GameObject newHighlight = Instantiate(highlight, marbleToHighlight.transform);
        if (!marbleScript.hasHighlight)
        {
            newHighlight.transform.position = marbleToHighlight.transform.position;
        }
        else
        {

        }
    }
}
