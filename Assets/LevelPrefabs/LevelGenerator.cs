using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> floor;
    public List<GameObject> middle;
    public List<GameObject> top;
    public List<List<List<GameObject>>> levelGrid3d;
    public int levelSize;
    public int seed;

    public List<float> floorPropability;
    public List<float> middlePropability;
    public List<float> topPropability;

    // Use this for initialization

    void Start()
    {
        List<List<float>> objectDensities = new List<List<float>>();
        objectDensities.Add(computeDensity(floorPropability));
        objectDensities.Add(computeDensity(middlePropability));
        objectDensities.Add(computeDensity(topPropability));
        
        Random.seed = seed;
        List<List<GameObject>> objectTypes = new List<List<GameObject>>();
        objectTypes.Add(floor);
        objectTypes.Add(middle);
        objectTypes.Add(top);
        Random.Range(0f,1f);
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
                    for(int i = 0; i < curDensity.Count; i++)
                    {
                        levelGrid3d[y][x].Add(null);
                        if(randomNumber < curDensity[i])
                        {
                            if(y > 0) {
                                if(levelGrid3d[y-1][x][z] != null) {
                                    levelGrid3d[y][x][z] = ((GameObject)GameObject.Instantiate(currentObjects[i], new Vector3(x, y, z), Quaternion.identity));
                                    break;
                                }
                            } else 
                            {
                                levelGrid3d[y][x][z] = ((GameObject)GameObject.Instantiate(currentObjects[i], new Vector3(x, y, z), Quaternion.identity));
                            }

                        }
                    }
                }
            }
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
  
    // Update is called once per frame
    void Update()
    {
  
    }
}
