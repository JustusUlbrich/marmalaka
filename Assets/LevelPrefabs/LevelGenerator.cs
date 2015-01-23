using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
    public GameObject levelBaseCube;
    public int levelSize;

	// Use this for initialization
	void Start () {
        for (int x = -levelSize / 2; x < levelSize / 2; x++)
        {
            for (int z = -levelSize / 2; z < levelSize / 2; z++)
            {
                GameObject.Instantiate(levelBaseCube, new Vector3(x, 0, z), Quaternion.identity);
            }
        } 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
