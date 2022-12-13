using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] SceneController controller;

    private int _id;
    public int ID
    {
        get { return _id; }
    }

    public void SetBackground(Sprite image)
    {
        GetComponent<SpriteRenderer>().sprite = image;
    }
    
}
