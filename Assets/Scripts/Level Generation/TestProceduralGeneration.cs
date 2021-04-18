using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProceduralGeneration : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] Vector2Int heightMinMax;
    [SerializeField] Vector2Int heigthTresholdMinMax;

    [SerializeField] GameObject dirt, grass;

    [SerializeField] Vector2Int partLength;
    int nextPart;

    [SerializeField] Vector2Int holeSizeMinMax;

    // Start is called before the first frame update
    void Start()
    {
        Generate();        
    }

    void Generate()
    {
        nextPart = Random.Range(partLength.x, partLength.y);

        for(int x = 0; x < width; x++)
        {
            if(x == nextPart)
            {
                int random = Random.Range(0, 2);
                Debug.Log(random);
                switch (random)
                {
                    case 0:
                        height += Random.Range(heigthTresholdMinMax.x, heigthTresholdMinMax.y);
                        if (height > heightMinMax.y) height = heightMinMax.y;
                        if (height < heightMinMax.x) height = heightMinMax.x;
                        break;
                    case 1:
                        int holeSize = Random.Range(holeSizeMinMax.x, holeSizeMinMax.y);
                        x += holeSize;
                        nextPart += holeSize;
                        break;
                }
                nextPart += Random.Range(partLength.x, partLength.y);
            }

            for(int y = 0; y < height; y++)
            {
                if (y + 1 == height) SpawnTile(grass, x, y);
                else SpawnTile(dirt, x, y);
            }
        }
    }

    void SpawnTile(GameObject tile, int x, int y)
    {
        GameObject obj = Instantiate(tile, new Vector2(x, y), Quaternion.identity);
        obj.name = tile.name + " (" + x.ToString() + ", " + y.ToString() + ")";
        obj.transform.SetParent(transform);
    }
}
