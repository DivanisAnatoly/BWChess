using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpBox : MonoBehaviour
{
    // Start is called before the first frame update
    public void HelpBoxOnoff(GameObject HelpBox)
    {
        if (HelpBox.activeSelf)
        {
            HelpBox.SetActive(false);
        }
        else HelpBox.SetActive(true);
    }
}
