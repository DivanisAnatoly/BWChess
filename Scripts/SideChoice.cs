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
        gameChoice.GetComponent<Image>().sprite = (_name == "Black") ? Resources.Load<Sprite>(@"Fonts/Dark_Side") : Resources.Load<Sprite>(@"Fonts/Light_Side");
        gameChoice.AddComponent<Button>();
        gameChoice.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
        var sprites = gameChoice.GetComponent<Button>().spriteState;
        switch (gameChoice.name) {
            case "White":
                sprites.highlightedSprite = Resources.Load<Sprite>(@"Fonts/Light_Side_High");
                sprites.disabledSprite = Resources.Load<Sprite>(@"Fonts/Light_Side");
                sprites.selectedSprite = Resources.Load<Sprite>(@"Fonts/Light_Side");
                sprites.disabledSprite = Resources.Load<Sprite>(@"Fonts/Light_Side");
                break;
            case "Black":
                sprites.highlightedSprite = Resources.Load<Sprite>(@"Fonts/Dark_Side_High");
                sprites.disabledSprite = Resources.Load<Sprite>(@"Fonts/Dark_Side");
                sprites.selectedSprite = Resources.Load<Sprite>(@"Fonts/Dark_Side");
                sprites.disabledSprite = Resources.Load<Sprite>(@"Fonts/Dark_Side");
                break;
        }
        gameChoice.GetComponent<Button>().spriteState = sprites;
    }
    void Start()
    {
        Vector2 scaleW = new Vector2(2.5f, 4.5f);
        Vector2 positionW = new Vector2(-132.79f, 12.06f);
        Vector2 scaleB = new Vector2(2.5f, 4.5f);
        Vector2 positionB = new Vector2(114.3f, 12.06f);

        result = GameObject.Find("ResultSideChoice");
        Debug.Log(result.name);
        ChoiceW = new SideChoice("White",scaleW, positionW);
        ChoiceB = new SideChoice("Black",scaleB, positionB);
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
