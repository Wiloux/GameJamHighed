using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject obj;
    public float width;
    public float height;

    [SerializeField] Floor[] floors;

    [SerializeField] FloorTrigger[] triggers;
    bool triggersEnabled = true;

    private Transform player;
    bool playerPassed;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerHelper.instance.transform;
        if (triggers.Length == 0) triggersEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerPassed)
        {
            if (player.position.x > transform.position.x + width + 2f) playerPassed = true;

            if (player.transform.position.y < height)
            {
                if(!triggersEnabled) EnableTriggers(true);
            }
            else if(triggersEnabled) { EnableTriggers(false); }
        }
    }

    private void EnableTriggers(bool enable)
    {
        foreach(FloorTrigger trigger in triggers)
        {
            trigger.gameObject.SetActive(enable);
        }
        triggersEnabled = enable;
    }

    public void RegisterTrigger(FloorTrigger trigger)
    {
        GameHandler gameHandler = GameHandler.instance;
        Floor chosenFloor = floors[trigger.floorIndex];

        for(int i = 0; i < floors.Length; i++)
        {
            Floor floor = floors[i];
            if (floor == chosenFloor) continue;

            if(floor.obj.GetComponentInChildren<Renderer>().material.color.a < 1)
            {
                if (floor.bg != null) StartCoroutine(gameHandler.Fade(floor.bg, 0.1f, "out"));
                StartCoroutine(gameHandler.Fade(floor.obj, 0.2f, "in"));
            }

            floor.obj.GetComponent<Collider2D>().enabled = true;
        }

        if(trigger.mode == FloorTrigger.Mode.entry)
        {
            StartCoroutine(gameHandler.Fade(chosenFloor.obj, 0.5f, "out"));
            if(chosenFloor.bg != null) StartCoroutine(gameHandler.Fade(chosenFloor.bg, 0.2f, "in"));
            chosenFloor.obj.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            StartCoroutine(gameHandler.Fade(chosenFloor.obj, 0.2f, "in"));
            if (chosenFloor.bg != null) StartCoroutine(gameHandler.Fade(chosenFloor.bg, 0.5f, "out"));
            chosenFloor.obj.GetComponent<Collider2D>().enabled = true;
        }
    }
}
[System.Serializable] public class Floor
{
    public GameObject obj;
    public GameObject bg;
}
