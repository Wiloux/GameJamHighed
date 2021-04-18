using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public static ProceduralGeneration instance;
    private void Awake() { instance = this; }

    [Header("Buildings prefabs")]
    [Space(5)]
    bool generationStarted = false;
    [SerializeField] Building[] buildings;
    float spawnPosition = 0;
    int currentBuildingNumber = 1;
    Transform buildingsParent;
    List<Building> createdBuildings = new List<Building>();

    [SerializeField] float distanceToSpawnNextBuilding = 25f;
    [SerializeField] float maxDifferenceBetweenBuildings;
    
    [Space(10)]
    [SerializeField] Vector2 holeSizeMinMax;

    [Header("Obstacles vars")]
    [Space(30)]
    [SerializeField] GameObject[] obstacles;
    [SerializeField] float minimumDistanceBetweenObstacles;
    [SerializeField] float minDistanceBetweenEdgesAndObstacles = 5;

    
    [Header("Enemies vars")]
    [Space(30)]
    [SerializeField] EnemyScript enemyPrefab;
    [SerializeField] float distanceBetweenEdgeAndEnemy;

    Transform obstaclesParent;
    Transform enemiesParent;

    Player player;

    private void Start()
    {
        player = PlayerHelper.instance.GetComponent<Player>();
    }

    private void Update()
    {
        if (generationStarted)
        {
            if(player.transform.position.x + distanceToSpawnNextBuilding >= spawnPosition)
            {
                if (player.isInBuilding) { SpawnBuilding(player.transform.position.y); Debug.Log("adapted spawn"); }
                else SpawnBuilding();
            }
        }
    }

    public void StartGeneration()
    {
        generationStarted = true;

        // making the hierarchy clear 
        buildingsParent = new GameObject("Buildings").transform;
        buildingsParent.SetParent(transform);
        obstaclesParent = new GameObject("Obstacles").transform;
        obstaclesParent.SetParent(transform);
        enemiesParent = new GameObject("Enemies").transform;
        enemiesParent.SetParent(transform);

        SpawnBuilding();
    }

    private void SpawnBuilding(float? maxHeight = null)
    {
        Debug.Log("spawn");
        Building buildingPrefab = buildings[Random.Range(0, buildings.Length)];

        if(currentBuildingNumber != 1)
        {
            Debug.Log("going to danger");
            if (maxHeight != null)
            {
                while (true)
                {
                    bool buildingsHeightDifferenceNotTooHigh = Building.AbsoluteHeightDifference(buildingPrefab, createdBuildings[currentBuildingNumber - 2]) < maxDifferenceBetweenBuildings;
                    bool isBelowMaxHeight = buildingPrefab.height < maxHeight;

                    if (buildingPrefab.hasWindows && buildingsHeightDifferenceNotTooHigh) { break; }
                    else if (!buildingPrefab.hasWindows && Mathf.Abs((float)maxHeight - buildingPrefab.height) < maxDifferenceBetweenBuildings) break;

                    buildingPrefab = buildings[Random.Range(0, buildings.Length)];
                }

                for (int i = 0; i < buildings.Length; i++)
                {
                    bool isBelowMaxHeight = buildings[i].height < buildingPrefab.height;
                    bool isCloserFromMaxHeight = Mathf.Abs(buildings[i].height - (float)maxHeight) < Mathf.Abs(buildingPrefab.height - (float)maxHeight);

                    if(buildingPrefab.height > maxHeight && isBelowMaxHeight)
                    {
                        buildingPrefab = buildings[i];
                    }
                    else if (isBelowMaxHeight && isCloserFromMaxHeight)
                    {
                        buildingPrefab = buildings[i];
                    }
                }
                Debug.Log("prefab :" + buildingPrefab.name);
            }
            else
            {
                bool buildingNotTooHigh = (createdBuildings[currentBuildingNumber - 2].height - buildingPrefab.height) >= - maxDifferenceBetweenBuildings;
                if (!buildingPrefab.hasWindows || buildingNotTooHigh)
                {
                    while (true)
                    {
                        Debug.Log("crash crash crash");
                        if (Input.GetKeyDown(KeyCode.A)) break;
                        buildingPrefab = buildings[Random.Range(0, buildings.Length)];
                        buildingNotTooHigh = (createdBuildings[currentBuildingNumber - 2].height - buildingPrefab.height) >= - maxDifferenceBetweenBuildings;
                        if (buildingNotTooHigh || buildingPrefab.hasWindows) { break; }
                    }
                }
            }
            Debug.Log("out of danger");
        }

        Building building = Instantiate(buildingPrefab, new Vector2(spawnPosition, 0), Quaternion.identity, buildingsParent);
        building.name = "Building " + currentBuildingNumber.ToString() + "( " + buildingPrefab.name + " )";

        // Decide if we want obstacles on this building
        if (Random.Range(0,2) == 0 && currentBuildingNumber != 1) 
        {
            // Spawn some obstacles on the building
            SpawnObstacles(new Vector2(spawnPosition + minDistanceBetweenEdgesAndObstacles, spawnPosition + building.width - minDistanceBetweenEdgesAndObstacles), building.height, currentBuildingNumber, obstaclesParent);
        }

        // Decide if we want an enemy on this building
        if(Random.Range(0,3) == 0)
        {
            Instantiate(enemyPrefab,
                new Vector2(spawnPosition + building.width - distanceBetweenEdgeAndEnemy, building.height),
                Quaternion.identity,
                enemiesParent);
        }

        // Decide if we want a hole
        // If yes push the next building spawn position even more
        if (Random.Range(0, 2) == 0) // temp
            spawnPosition += Random.Range(holeSizeMinMax.x, holeSizeMinMax.y);

        // Pushing next building spawn position
        spawnPosition += building.width;
        currentBuildingNumber++;
        createdBuildings.Add(building);
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
