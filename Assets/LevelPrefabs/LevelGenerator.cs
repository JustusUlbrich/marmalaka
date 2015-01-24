using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public GameObject pathTile;
    public List<GameObject> floor;
    public List<GameObject> middle;
    public List<GameObject> top;
    public List<List<List<GameObject>>> levelGrid3d;
    public int levelSize;
    public int seed;
    public float distanceProbability;
    public float forwardProbability;

    public List<float> floorPropability;
    public List<float> middlePropability;
    public List<float> topPropability;
    private List<Vector2> path;
    private Vector2 p1Start;
    private Vector2 target;

    // Use this for initialization
    void Start()
    {
        setupLevel();
    }

    void clearPath()
    {
        foreach (Vector2 tile in path)
        {
            GameObject.DestroyObject(levelGrid3d[2][(int)tile.x][(int)tile.y]);
            GameObject.DestroyObject(levelGrid3d[1][(int)tile.x][(int)tile.y]);
            GameObject.Instantiate(pathTile, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
        }
    }

    void setupLevel()
    {
        p1Start.Set(0, 0);//Random.value * levelSize, Random.value * levelSize);
        target.Set(levelSize-1, levelSize-1);//Random.value * levelSize, Random.value * levelSize);
        Random.seed = seed;
        computePath();
        generateTiles();
        clearPath();
    }

    int computeDirection(int numTiles)
    {
        int direction = 0;
        if (numTiles > 0)
        {
            direction = 1;
        }
        else if (numTiles < 0)
        {
            direction = -1;
        }
        return direction;
    }
    float computePathProbability(Vector2 current, Vector2 neighbor)
    {
        float probability = 0;
        if ((current.x + current.y) > (neighbor.x + neighbor.y))
        {
            probability = distanceProbability;
        }
        else
        {
            probability = 1 - distanceProbability;
        }
		if ( neighbor.x < 0 || neighbor.x >= levelSize || neighbor.y < 0 || neighbor.y >= levelSize)
        {
            probability = 0;
        }
        return probability;
    }

    void computePath()
    {
        //compute diagonal path
        path = new List<Vector2>();
        Vector2 currentPosition = p1Start;
		return;
		while (((int)(currentPosition.x - target.x)) != 0 && ((int)(currentPosition.y - target.y)) != 0)
		{
            Vector2 left =   new Vector2(currentPosition.x - 1, currentPosition.y);
            Vector2 right =  new Vector2(currentPosition.x + 1, currentPosition.y);
            Vector2 top =    new Vector2(currentPosition.x    , currentPosition.y + 1);
            Vector2 bottom = new Vector2(currentPosition.x    , currentPosition.y - 1);
            float probabilityLeft = computePathProbability(currentPosition, left);
            float probabilityRight = computePathProbability(currentPosition, right);
            float probabilityTop = computePathProbability(currentPosition, top);
            float probabilityBottom = computePathProbability(currentPosition, bottom);
            float sum = probabilityLeft + probabilityRight + probabilityTop + probabilityBottom;
            probabilityLeft /= sum;
			probabilityRight = probabilityRight / sum +  probabilityLeft;
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
			currentPosition = path[path.Count -1];
            
        }
        
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
        objectDensities.Add(computeDensity(floorPropability));
        objectDensities.Add(computeDensity(middlePropability));
        objectDensities.Add(computeDensity(topPropability));

        List<List<GameObject>> objectTypes = new List<List<GameObject>>();
        objectTypes.Add(floor);
        objectTypes.Add(middle);
        objectTypes.Add(top);
        Random.Range(0f, 1f);
        levelGrid3d = new List<List<List<GameObject>>>();

        List<GameObject> currentObjects = new List<GameObject>();
        List<float> curDensity = new List<float>();
        for (int y = 0; y <= 2; y++)
        {
            curDensity.Clear();
            curDensity.AddRange(objectDensities[y]);

            currentObjects.Clear();
            levelGrid3d.Add(new List<List<GameObject>>());
            currentObjects.AddRange(objectTypes[y]);

            for (int x = 0; x < levelSize; x++)
            {
                levelGrid3d[y].Add(new List<GameObject>());
                for (int z = 0; z < levelSize; z++)
                {
                    float randomNumber = Random.value;
                    for (int i = 0; i < curDensity.Count; i++)
                    {
                        levelGrid3d[y][x].Add(null);
                        if (randomNumber < curDensity[i])
                        {
                            if (y > 0)
                            {
                                if (levelGrid3d[y - 1][x][z] != null)
                                {
                                    levelGrid3d[y][x][z] = ((GameObject)GameObject.Instantiate(currentObjects[i], new Vector3(x, y, z), Quaternion.identity));
                                    break;
                                }
                            }
                            else
                            {
                                levelGrid3d[y][x][z] = ((GameObject)GameObject.Instantiate(currentObjects[i], new Vector3(x, y, z), Quaternion.identity));
                            }

                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
  
    }
}
