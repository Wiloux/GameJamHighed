using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class differentSprite : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
    }


}
