using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class HelpBox : MonoBehaviour
{
    int milliseconds = 0;
    const int timeUntilDeath = 3000;
    int counter = 0;
    float alpha = 0f;
    const int arrSize = 4;
    GameObject helpBox;
    GameObject imageField;
    List<Sprite> images = new List<Sprite>() { };

    private void Start()
    {
        DownloadingImages(arrSize);
        helpBox = GameObject.Find("HelpBox");
        imageField = new GameObject();
        imageField.transform.SetParent((helpBox.transform.GetChild(0)));
        imageField.transform.localPosition = new Vector2(0, 0);
        imageField.transform.localScale = new Vector2(2.7f, 3);
        imageField.AddComponent<Image>();
        imageField.GetComponent<Image>().sprite = images[0];
        imageField.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0);
    }
    private void Update()
    {
        if (helpBox.activeSelf)
        {
            milliseconds++;
            if (milliseconds < timeUntilDeath / 2)
                alpha += (float)10 / timeUntilDeath;
            else
                alpha -= (float)10 / timeUntilDeath;

            imageField.GetComponent<Image>().color = new Color(1f,1f,1f,alpha);
            if (milliseconds == timeUntilDeath)
            {
                alpha = 0f;
                if (counter == arrSize - 1) counter = 0;
                else counter++;
                imageField.GetComponent<Image>().sprite = images[counter];
                milliseconds = 0;
            }    
        }
    }
    private void DownloadingImages(int _arrSize)
    {
        string path = "HelpImages/";
        for (int i = 0; i < _arrSize; i++)
        {
            path += i;
            images.Add(Resources.Load<Sprite>(path));
            path = path.Remove(path.Length - 1);
        }
    }
    public void HelpBoxOnoff(GameObject helpBox)
    {
        if (helpBox.activeSelf)
        {
            helpBox.SetActive(false);
        }
        else helpBox.SetActive(true);
    }
}
