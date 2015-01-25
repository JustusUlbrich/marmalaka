﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    private const string GRASS = "grass";
    private const string WATER = "water";
    private const string STREET = "street";
    private const string PATH = "path";
    public GameObject pathTile;
    public List<GameObject> floorGO;
    public List<GameObject> middleGO;
    public List<GameObject> topGO;
    public List<List<GameObject>> floorGrid;
    public GameObject[,,] top3DGrid;
    public int levelSize;
    public int seed;
    public float distanceProbability;
    public float forwardProbability;
    public int maxNumberOfStreets;
    public int maxNumberOfWater;
    public int maxNumberOfHills;
    public int hillDensity;
    public int maxHillHeight;
    public List<float> floorPropability;
    public List<float> middlePropability;
    public List<float> topPropability;
    private List<Vector2> playerPath;
    private List<List<Vector2>> streetPaths;
    private List<List<Vector2>> waterPaths;
    private Vector2 p1Start;
    private Vector2 target;

    // Use this for initialization
    void Start()
    {
        setupLevel();
    }

    void clearPath()
    {
        foreach (Vector2 tile in playerPath)
        {
            GameObject.Instantiate(pathTile, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
            floorGrid [(int)tile.x] [(int)tile.y].tag = PATH;
        }
    }

    void drawStreets()
    {
        foreach (List<Vector2> street in streetPaths)
        {
            foreach (Vector2 tile in street)
            {
                GameObject.DestroyObject(floorGrid [(int)tile.x] [(int)tile.y]);
                floorGrid [(int)tile.x] [(int)tile.y] = GameObject.Instantiate(middleGO [0], new Vector3(tile.x, 0, tile.y), Quaternion.identity) as GameObject;
            }
        }
    }

    void drawWater()
    {
        foreach (List<Vector2> water in waterPaths)
        {
            foreach (Vector2 tile in water)
            {
                GameObject.DestroyObject(floorGrid [(int)tile.x] [(int)tile.y]);
                floorGrid [(int)tile.x] [(int)tile.y] = GameObject.Instantiate(middleGO [1], new Vector3(tile.x, 0, tile.y), Quaternion.identity) as GameObject;
            }
        }
    }

    void setupLevel()
    {
        p1Start.Set(0, 0);//Random.value * levelSize, Random.value * levelSize);
        target.Set(levelSize - 1, levelSize - 1);//Random.value * levelSize, Random.value * levelSize);
        Random.seed = seed;
        floorGrid = new List<List<GameObject>>();
        computePathForPlayer();
        generateTiles();
        clearPath();

        generateStreets();
        drawStreets();

        generateWater();
        drawWater();

        top3DGrid = new GameObject[levelSize, maxHillHeight, levelSize];
        generateHills();
    }

    int computeDirection(int numTiles)
    {
        int direction = 0;
        if (numTiles > 0)
        {
            direction = 1;
        } else if (numTiles < 0)
        {
            direction = -1;
        }
        return direction;
    }

    float computePathProbability(Vector2 current, Vector2 neighbor, Vector2 target)
    {
        float probability = 0;
        if ((Mathf.Abs(current.x - target.x) + Mathf.Abs(current.y - target.y)) 
            > (Mathf.Abs(neighbor.x - target.x) + Mathf.Abs(neighbor.y - target.y)))
        {
            probability = distanceProbability;
        } else
        {
            probability = 1 - distanceProbability;
        }
        if (neighbor.x < 0 || neighbor.x >= levelSize || neighbor.y < 0 || neighbor.y >= levelSize)
        {
            probability = 0;
        }
        return probability;
    }

    void computePathForPlayer()
    {
        //compute diagonal path
        playerPath = computePathBetweenPoints(p1Start, target); 
    }

    List<Vector2> computePathBetweenPoints(Vector2 start, Vector2 target)
    {
        Vector2 currentPosition = start;
        List<Vector2> path = new List<Vector2>();
        while (((int)(currentPosition.x - target.x)) != 0 || ((int)(currentPosition.y - target.y)) != 0)
        {
            Vector2 left = new Vector2(currentPosition.x - 1, currentPosition.y);
            Vector2 right = new Vector2(currentPosition.x + 1, currentPosition.y);
            Vector2 top = new Vector2(currentPosition.x, currentPosition.y + 1);
            Vector2 bottom = new Vector2(currentPosition.x, currentPosition.y - 1);
            float probabilityLeft = computePathProbability(currentPosition, left, target);
            float probabilityRight = computePathProbability(currentPosition, right, target);
            float probabilityTop = computePathProbability(currentPosition, top, target);
            float probabilityBottom = computePathProbability(currentPosition, bottom, target);

            float sum = probabilityLeft + probabilityRight + probabilityTop + probabilityBottom;
            probabilityLeft /= sum;
            probabilityRight = probabilityRight / sum + probabilityLeft;
            probabilityTop = probabilityTop / sum + probabilityRight;
            probabilityBottom = probabilityBottom / sum + probabilityTop;

            float randomNumber = Random.value;

            if (randomNumber < probabilityLeft)
            {
                path.Add(left);
            } else if (randomNumber < probabilityRight)
            {
                path.Add(right);
            } else if (randomNumber < probabilityTop)
            {
                path.Add(top);
            } else
            {
                path.Add(bottom);
            }
            currentPosition = path [path.Count - 1];
        }

        return path;
    }

    List<float> computeDensity(List<float> probabilityList)
    {
        List<float> densityList = new List<float>();
        float sum = 0;
        for (int i = 0; i < probabilityList.Count; i++)
        {
            sum += probabilityList [i];
            densityList.Add(sum);
        }
        return densityList;
    }

    void generateTiles()
    {
        floorPropability = computeDensity(floorPropability);

        Random.Range(0f, 1f);
        for (int x = 0; x < this.levelSize; x++)
        {
            floorGrid.Add(new List<GameObject>());
            for (int z = 0; z < this.levelSize; z++)
            {
                float randomValue = Random.value;
                for (int i = 0; i < floorPropability.Count; i++)
                {
                    if (randomValue < floorPropability [i])
                    {
                        floorGrid [x].Add(GameObject.Instantiate(floorGO [i], new Vector3(x, 0, z), Quaternion.identity) as GameObject);
                        break;
                    }
                }
          
            }
        }

    }

    void generateStreets()
    {
        streetPaths = new List<List<Vector2>>();
        int numberOfStreets = (int)(maxNumberOfStreets * Random.value);
        for (int i = 0; i < numberOfStreets; i++)
        {
            Vector2 start = new Vector2((int)((levelSize - 1) * Random.value), (int)((levelSize - 1) * Random.value));
            Vector2 end = new Vector2((int)((levelSize - 1) * Random.value), (int)((levelSize - 1) * Random.value));
            streetPaths.Add(computePathBetweenPoints(start, end));
        }
    }

    void generateWater()
    {
        waterPaths = new List<List<Vector2>>();
        int numberOfWater = (int)(maxNumberOfWater * Random.value);
        for (int i = 0; i < numberOfWater; i++)
        {
            Vector2 start = new Vector2((int)((levelSize - 1) * Random.value), (int)((levelSize - 1) * Random.value));
            Vector2 end = new Vector2((int)((levelSize - 1) * Random.value), (int)((levelSize - 1) * Random.value));
            waterPaths.Add(computePathBetweenPoints(start, end));
        }
    }

    bool validatePosition(Vector3 currentPosition)
    {
        return currentPosition.x >= 0 && currentPosition.x < levelSize 
            && currentPosition.z >= 0 && currentPosition.z < levelSize 
            && floorGrid [(int)currentPosition.x] [(int)currentPosition.z].tag == GRASS;
    }

    void placeHillTile(Vector3 position, Vector3 currentPosition, int iteration)
    {
        if (validatePosition(currentPosition))
        {
            for (int k = 1; k <= Random.value * position.y * Mathf.Pow(0.8f, iteration) + 1; k++)
            {
                currentPosition.y = k;
                GameObject.DestroyObject(top3DGrid [(int)currentPosition.x, (int)currentPosition.y - 1, (int)currentPosition.z]);
                top3DGrid [(int)currentPosition.x, (int)currentPosition.y - 1, (int)currentPosition.z] = GameObject.Instantiate(topGO [0], currentPosition, Quaternion.identity) as GameObject;
            }
        }
    }

    void computeHill(Vector3 position)
    {
        if (floorGrid [(int)position.x] [(int)position.z].tag.Equals(GRASS))
        {
            for (int i = 0; i < (int)position.y; i++)
            {
                top3DGrid [(int)position.x, i, (int)position.z] = GameObject.Instantiate(topGO [0], new Vector3(position.x, i + 1, position.z), Quaternion.identity) as GameObject;
            }

            Vector3 topRight = position;
            Vector3 bottomLeft = position;

            if (position.y == 1)
            {
                GameObject.DestroyObject(top3DGrid [(int)position.x, (int)position.y - 1, (int)position.z]);
                top3DGrid [(int)position.x, (int)position.y - 1, (int)position.z] = GameObject.Instantiate(topGO [0], position, Quaternion.identity) as GameObject;
            } else
            {
                for (int y = 0; y < hillDensity; y++)
                {
                    topRight.x += 1;
                    topRight.z += 1;
                    bottomLeft.x -= 1;
                    bottomLeft.z -= 1;
                   
                    for (int i = (int)bottomLeft.x; i <= topRight.x; i++)
                    {
                        Vector3 currentPosition = new Vector3(i, 0, (int)bottomLeft.z);
                        placeHillTile(position, currentPosition, y);
                    }
                    
                    for (int i = (int)bottomLeft.z + 1; i <= topRight.z; i++)
                    {
                        Vector3 currentPosition = new Vector3((int)topRight.x, 0, i);
                        placeHillTile(position, currentPosition, y);
                    }
                    
                    for (int i = (int)topRight.x - 1; i >=  bottomLeft.x; i--)
                    {
                        Vector3 currentPosition = new Vector3(i, 0, topRight.z);
                        placeHillTile(position, currentPosition, y);
                    }
                    
                    for (int i = (int)topRight.z - 1; i >= bottomLeft.z + 1; i--)
                    {
                        Vector3 currentPosition = new Vector3((int)bottomLeft.x, 0, i);
                        placeHillTile(position, currentPosition, y);
                    }
                }
                
                
            }
        }
    }

    void generateHills()
    {
        int numberOfHills = (int)(maxNumberOfHills * Random.value);
        for (int i = 0; i < numberOfHills; i++)
        {
            Vector3 position = new Vector3((int)((levelSize - 1) * Random.value),
                                           (int)((maxHillHeight) * Random.value) + 1,
                                           (int)((levelSize - 1) * Random.value));
            computeHill(position);
        }
    }

    // Update is called once per frame
    void Update()
    {
  
    }
}
