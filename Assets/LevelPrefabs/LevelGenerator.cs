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

    // Use this for initialization
    void Start()
    {

        Random.seed = seed;
        List<List<GameObject>> objectTypes = new List<List<GameObject>>();
        objectTypes.Add(floor);
        objectTypes.Add(middle);
        objectTypes.Add(top);
        levelGrid3d = new List<List<List<GameObject>>>();
        
        List<GameObject> currentObjects = new List<GameObject>();
        for (int y = 0; y <= 2; y++)
        {
            currentObjects.Clear();
            levelGrid3d.Add(new List<List<GameObject>>());
            currentObjects.AddRange(objectTypes[y]);
            Random.Range(-0.499999999f, currentObjects.Count - 0.50000001f);
            for (int x = 0; x < levelSize; x++)
            {
                levelGrid3d[y].Add(new List<GameObject>());
                for (int z = 0; z < levelSize; z++)
                {
                    levelGrid3d[y][x].Add((GameObject)GameObject.Instantiate(currentObjects[(int)Mathf.Round(Random.value)], new Vector3(x, y, z), Quaternion.identity));
                }
            }
        }
    }
  
    // Update is called once per frame
    void Update()
    {
  
    }
}
