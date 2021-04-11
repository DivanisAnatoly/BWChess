using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enlarge : MonoBehaviour
{

    public void Hovered()
    {
            gameObject.transform.localScale = new Vector2(1.15f, 1.15f);  
    }

    public void Exited()
    {
        gameObject.transform.localScale = new Vector2(1.0f, 1.0f);
    }

    public void Downed()
    {
        gameObject.transform.localScale = new Vector2(0.85f, 0.85f);
    }
}
