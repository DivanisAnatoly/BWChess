using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class HelpBox : MonoBehaviour
{
    int milliseconds = 0;
    int counter = 0;
    const int arrSize = 4;
    GameObject helpBox;
    GameObject imageField;
    GameObject tempImages;
    List<Sprite> images = new List<Sprite>() { };// Start is called before the first frame update
    private void Start()
    {
        DownloadingImages(arrSize);
        helpBox = GameObject.Find("HelpBox");
        tempImages = GameObject.Find("Images");
        imageField = new GameObject();
        imageField.transform.SetParent((helpBox.transform.GetChild(0)).transform.GetChild(0));
        imageField.AddComponent<Image>();
        imageField.GetComponent<Image>().sprite = images[0];
    }
    private void Update()
    {
        if (helpBox.activeSelf)
        {
            milliseconds++;
            if (milliseconds == 3000)
            {
                if (counter == arrSize - 1) counter = 0;
                else counter++;
                imageField.GetComponent<Image>().sprite = images[counter];
                milliseconds = 0;
            }
        }
    }
    void DownloadingImages(int _arrSize)
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
