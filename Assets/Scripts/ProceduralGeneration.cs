using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] Vector2 holeSizeMinMax;

    [SerializeField] int nbOfBuilding;
    [SerializeField] Building[] buildings;

    [SerializeField] GameObject[] obstacles;
    [SerializeField] float minimumDistanceBetweenObstacles;
    [SerializeField] float minDistanceBetweenEdgesAndObstacles = 5;

    [SerializeField] EnemyScript enemyPrefab;

    float spawnPosition = 0;
    int currentBuildingNumber = 1;

    Transform buildingsParent;
    Transform obstaclesParent;

    Player player;

    private void Start()
    {
        player = PlayerHelper.instance.GetComponent<Player>();

        // making the hierarchy clear by creating a parent for every buildings
        buildingsParent = new GameObject("Buildings").transform;
        buildingsParent.SetParent(transform);

        // making the hierarchy clear by creating a parent for every obstacles
        obstaclesParent = new GameObject("Obstacles").transform;
        obstaclesParent.SetParent(transform);

        SpawnBuilding();

        //Generate();
    }

    private void Update()
    {
        if(player.transform.position.x + 30 >= spawnPosition)
        {
            if (player.isInBuilding) { SpawnBuilding(player.transform.position.y + 5f); }
            else SpawnBuilding();
        }
    }

    private void SpawnBuilding(float? maxHeight = null)
    {
        Building buildingPrefab = buildings[Random.Range(0, buildings.Length)];
        if (maxHeight != null)
        {
            while(buildingPrefab.height > maxHeight)
            {
                buildingPrefab = buildings[Random.Range(0, buildings.Length)];
            }
        }
        Building building = Instantiate(buildingPrefab, new Vector2(spawnPosition, 0), Quaternion.identity, buildingsParent);
        building.name = "Building " + currentBuildingNumber.ToString() + "( " + buildingPrefab.name + " )";

        // Decide if we want obstacles on this building
        if (Random.Range(0,3) != 0) 
        {
            // Spawn some obstacles on the building
            SpawnObstacles(new Vector2(spawnPosition + minDistanceBetweenEdgesAndObstacles, spawnPosition + building.width - minDistanceBetweenEdgesAndObstacles), building.height, currentBuildingNumber, obstaclesParent);
        }

        // Decide if we want an enemy on this building
        if(Random.Range(0,3) == 0)
        {
            //Ins
        }

        // Decide if we want a hole
        // If yes push the next building spawn position even more
        if (Random.Range(0, 2) == 0) // temp
            spawnPosition += Random.Range(holeSizeMinMax.x, holeSizeMinMax.y);

        // Pushing next building spawn position
        spawnPosition += building.width;
        currentBuildingNumber++;
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
