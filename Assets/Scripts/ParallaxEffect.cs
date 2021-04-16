using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{

    float length;
    Vector2 startPos;
    Camera cam;
    [SerializeField] bool axisY;
    [SerializeField] float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        cam = Camera.main;

        if(transform.parent != null)
        {
            transform.parent.SetParent(cam.transform);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos.x + dist, axisY ? transform.position.y : startPos.y, transform.position.z);

        if (temp > startPos.x + length) startPos.x += length;
        else if (temp < startPos.x - length) startPos.x -= length;
    }
}
