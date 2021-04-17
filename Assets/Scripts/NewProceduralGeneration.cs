using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewProceduralGeneration : MonoBehaviour
{
    [SerializeField] Vector2 holeSizeMinMax;

    [SerializeField] int nbOfBuilding;
    [SerializeField] Building[] buildings;

    [SerializeField] GameObject[] obstacles;
    [SerializeField] float minimumDistanceBetweenObstacles;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        // making the hierarchy clear by creating a parent for every buildings
        Transform buildingsParent = new GameObject("Buildings").transform;
        buildingsParent.SetParent(transform);

        // making the hierarchy clear by creating a parent for every obstacles
        Transform obstaclesParent = new GameObject("Obstacles").transform;
        obstaclesParent.SetParent(transform);



        // Creating a float to store the x value of the position of the next building
        float buildingX = 0;
        for(int i = 0; i < nbOfBuilding; i++)
        {
            // Spawning building
            Building building = Instantiate(buildings[Random.Range(0, buildings.Length)], new Vector2(buildingX, 0), Quaternion.identity, buildingsParent); ;
            building.name = "Building " + i.ToString();

            // Pushing next building spawn position
            buildingX += building.width;

            SpawnObstacles(new Vector2(buildingX - building.width + 5, buildingX - 5), building.height, i, obstaclesParent);

            // Decide if we want a hole
                // If yes push the next building spawn position even more
            if (Random.Range(0, 2) == 0) // temp
                buildingX += Random.Range(holeSizeMinMax.x, holeSizeMinMax.y);
        }
    }

    private void SpawnObstacles(Vector2 xMinMax, float y, int buildingNumber, Transform parent)
    {
        if(Random.Range(0, 3) == 0)
        {
            // don't spawn for this building
            return;
        }
        else
        {
            int random = Random.Range(0, 2) + 1;

            List<Transform> currentObstacles = new List<Transform>();
            // Spawn obstacles
            for (int i = 0; i < random; i++)
            {
                Vector2 spawnPos = new Vector2(Random.Range(xMinMax.x, xMinMax.y), y);
                while (true) 
                {
                    bool positionFound = true;
                    foreach(Transform temp in currentObstacles)
                    {
                        if(Mathf.Abs(spawnPos.x - temp.position.x) < minimumDistanceBetweenObstacles)
                        {
                            positionFound = false;
                            if (spawnPos.x > temp.position.x) spawnPos.x += 0.5f;
                            else spawnPos.x -= 0.5f;
                        }
                    }

                    if (positionFound) break;
                }
                if(spawnPos.x < xMinMax.x || spawnPos.x > xMinMax.y) { continue;}

                GameObject obstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length)], spawnPos, Quaternion.identity, parent);
                obstacle.name = "Obstacle " + i + " | Building " + buildingNumber;

                currentObstacles.Add(obstacle.transform);
            }
        }
    }
}
