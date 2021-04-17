using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTrigger : MonoBehaviour
{
    public enum Mode { entry, exit};
    public Mode mode;

    [Space(10)]
    public int floorIndex;

    private void Start()
    {
        //if(toActivate != null) toActivate.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Building building = GetComponentInParent<Building>();
            if (building == null) Debug.LogError("There is no buildings in the parents");
            else { building.RegisterTrigger(this); }

            PlayerHelper.instance.AddWallDestructionScore();
            Instantiate(GameHandler.instance.destructionParticles, transform.position, Quaternion.Euler(new Vector3(0,0,180)));

            gameObject.SetActive(false);
        }
    }
}
