using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewProceduralGeneration : MonoBehaviour
{
    [SerializeField] Vector2 holeSizeMinMax;

    [SerializeField] int nbOfBuilding;
    [SerializeField] Building[] buildings;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        // making the hierarchy clear by creating a parent for every buildings
        Transform buildingsParent = new GameObject("Buildings").transform;
        buildingsParent.SetParent(transform);

        // Creating a float to store the x value of the position of the next building
        float buildingX = 0;
        for(int i = 0; i < nbOfBuilding; i++)
        {
            // Spawning building
            Building building = buildings[Random.Range(0, buildings.Length)];
            if (i != 0) buildingX += building.size;
            GameObject obj = Instantiate(building.obj, new Vector2(buildingX, 0), Quaternion.identity);
            obj.name = "Building " + i.ToString();
            obj.transform.SetParent(buildingsParent);

            // Pushing next building spawn position
            buildingX += building.size;


            // Decide if we want a hole
                // If yes push the next building spawn position even more
            if (Random.Range(0, 2) == 0) // temp
                buildingX += Random.Range(holeSizeMinMax.x, holeSizeMinMax.y);
        }
    }
}
[System.Serializable] public class Building
{
    public GameObject obj;
    public float size;

    public Building(GameObject obj, float size)
    {
        this.obj = obj;
        this.size = size;
    }
}
