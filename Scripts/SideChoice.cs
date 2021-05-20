using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SideChoice : MonoBehaviour
{
    GameObject  sideChoiceAnchor = GameObject.Find("SideChoiseAnchor");
    GameObject result;
    GameObject gameChoice;
    SideChoice ChoiceW;
    SideChoice ChoiceB;
    SideChoice(string _name, Vector2 _scale, Vector2 _position)
    {
        gameChoice = new GameObject();
        gameChoice.name = _name;
        gameChoice.transform.SetParent(sideChoiceAnchor.transform);
        gameChoice.transform.localPosition = _position;
        gameChoice.transform.localScale = _scale;
        gameChoice.AddComponent<Image>();
        gameChoice.GetComponent<Image>().sprite = Resources.Load<Sprite>(@"Chess\wSquare");
        gameChoice.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        gameChoice.AddComponent<Button>();
        var colors = gameChoice.GetComponent<Button>().colors;
        switch (gameChoice.name) {
            case "White":
                colors.normalColor = Color.white;
                colors.highlightedColor = Color.blue;
                colors.selectedColor = Color.white;
                colors.pressedColor = Color.white;
                break;
            case "Black":
                colors.normalColor = Color.black;
                colors.highlightedColor = Color.red;
                colors.selectedColor = Color.black;
                colors.pressedColor = Color.black;
                break;
        }
        gameChoice.GetComponent<Button>().colors = colors;
    }
    void Start()
    {
        result = GameObject.Find("ResultSideChoice");
        Debug.Log(result.name);
        ChoiceW = new SideChoice("White",new Vector2(2.5f, 4.5f), new Vector2(-132.79f, 12.06f));
        ChoiceB = new SideChoice("Black",new Vector2(2.5f, 4.5f), new Vector2(114.3f, 12.06f));
        ChoiceW.gameChoice.GetComponent<Button>().onClick.AddListener(WhiteChoice);
        ChoiceB.gameChoice.GetComponent<Button>().onClick.AddListener(BlackChoice);

    }

    void WhiteChoice()
    {
        result.GetComponent<Text>().text = "White";
        Destroy(ChoiceB.gameChoice);
        Destroy(ChoiceW.gameChoice);
        
    }
    void BlackChoice()
    {
        result.GetComponent<Text>().text = "Black";
        Destroy(ChoiceB.gameChoice);
        Destroy(ChoiceW.gameChoice);
    }
}
