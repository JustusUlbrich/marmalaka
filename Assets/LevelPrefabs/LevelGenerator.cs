using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public GameObject pathTile;
    public List<GameObject> floorGO;
    public List<GameObject> middleGO;
    public List<GameObject> topGO;
    public List<List<GameObject>> floorGrid;
    public int levelSize;
    public int seed;
    public float distanceProbability;
    public float forwardProbability;
    public int maxNumberOfStreets;
    public List<float> floorPropability;
    public List<float> middlePropability;
    public List<float> topPropability;
    private List<Vector2> playerPath;
    private List<List<Vector2>> streetPaths;
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
            currentPosition = path[path.Count - 1];
        }

        return path;
    }

    List<float> computeDensity(List<float> probabilityList)
    {
        List<float> densityList = new List<float>();
        float sum = 0;
        for (int i = 0; i < probabilityList.Count; i++)
        {
            sum += probabilityList[i];
            densityList.Add(sum);
        }
        return densityList;
    }

    void generateTiles()
    {

        List<List<float>> objectDensities = new List<List<float>>();
        floorPropability = computeDensity(floorPropability);
//        objectDensities.Add(computeDensity(middlePropability));
//        objectDensities.Add(computeDensity(topPropability));
//
//        objectTypes.Add(middle);
//        objectTypes.Add(top);
//        levelGrid3d = new List<List<List<GameObject>>>();
//
//        List<GameObject> currentObjects = new List<GameObject>();
//        List<float> curDensity = new List<float>();

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

    // Update is called once per frame
    void Update()
    {
  
    }
}
