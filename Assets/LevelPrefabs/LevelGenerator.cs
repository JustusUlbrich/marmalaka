using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    private const string GRASS = "grass";
    private const string WATER = "water";
    private const string STREET = "street";
    private const string ROCK = "ROCK";
    private const string PATH = "path";
    private const string HOUSE = "house";
    public GameObject pathTile;
    public List<GameObject> floorGO;
    public List<GameObject> middleGO;
    public List<GameObject> topGO;
    public GameObject house;
    public List<List<GameObject>> floorGrid;
    public GameObject[,,] top3DGrid;
    public int levelSize;
    public int seed;
    public float distanceProbability;
    public float forwardProbability;
    public List<float> floorprobability;
    public List<float> middleprobability;
    public List<float> topprobability;
    public float houseprobability;
    public int maxNumberOfStreets;
    public int maxNumberOfWater;
    public int maxNumberOfHills;
    public int hillDensity;
    public int maxHillHeight;
    private List<Vector2> playerPath;
    private List<List<Vector2>> streetPaths;
    private List<List<Vector2>> waterPaths;
    private Vector2 p1Start;
    public Vector2 target;

    private string[,,] futureTiles;


    // Use this for initialization
    void Start()
    {
        setupLevel();
        GameManager.singleton.levelGen = this;
    }

    void clearPath()
    {
        foreach (Vector2 tile in playerPath)
        {
//            GameObject.Instantiate(pathTile, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
            floorGrid [(int)tile.x] [(int)tile.y].tag = PATH;
        }
    }

    void drawStreets()
    {
        foreach (List<Vector2> street in streetPaths)
        {
            foreach (Vector2 tile in street)
            {
                futureTiles[(int)tile.x, 0, (int)tile.y] = STREET;
            }
        }
    }

    void drawWater()
    {
        foreach (List<Vector2> water in waterPaths)
        {
            foreach (Vector2 tile in water)
            {
                futureTiles[(int)tile.x, 0, (int)tile.y] = WATER;
            }
        }
    }

    void setupLevel()
    {
        p1Start.Set(0, 0);//Random.value * levelSize, Random.value * levelSize);
        target.Set(levelSize - 1, levelSize - 1);//Random.value * levelSize, Random.value * levelSize);
        Random.seed = seed;
        floorGrid = new List<List<GameObject>>();
        top3DGrid = new GameObject[levelSize, maxHillHeight, levelSize];
        top3DGrid.Initialize();
        futureTiles = new string[levelSize, maxHillHeight + 1, levelSize];


        computePathForPlayer();
        generateTiles();
        clearPath();

        generateStreets();
        drawStreets();

        generateWater();
        drawWater();
        generateHouses();
        generateHills();

        instantiateGameObjects();
    }

    void instantiateGameObjects()
    {
        for (int x = 0; x < levelSize; x++) {
            for (int y = 0; y < maxHillHeight; y++) {
                for (int z = 0; z < levelSize; z++) {
                    switch (futureTiles[x, y, z]) {
                        case WATER:
                            floorGrid [x][z] = GameObject.Instantiate(middleGO [1], new Vector3(x, y, z), Quaternion.identity) as GameObject;
                            break;
                        case STREET:
                            floorGrid [x][z] = GameObject.Instantiate(middleGO [0], new Vector3(x, y, z), Quaternion.identity) as GameObject;
                            break;
                        case ROCK:
                            placeRockObject(x, y, z);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    void placeRockObject(int x, int y, int z)
    {
        if(inBounds(x, y + 1, z) && futureTiles[x, y + 1, z] != null && futureTiles[x, y + 1, z] == ROCK) {
            top3DGrid [x, y, z] = GameObject.Instantiate(topGO[4], 
                                                         new Vector3(x, y, z), Quaternion.Euler(-90,0,0)) as GameObject;

        } else {
            int rockCount = 0;
            bool left = false;
            bool right = false;
            bool top = false;
            bool bottom = false;

            if(inBounds(x+1, y, z) && futureTiles[x+1, y, z] != null && futureTiles[x+1, y, z] == ROCK) {
                rockCount++;
                right = true;
            }

            if(inBounds(x-1, y, z) && futureTiles[x-1, y, z] != null && futureTiles[x-1, y, z] == ROCK) {
                rockCount++;
                left = true;
            }

            if(inBounds(x, y, z+1) && futureTiles[x, y, z+1] != null && futureTiles[x, y, z+1] == ROCK) {
                rockCount++;
                top = true;
            }

            if(inBounds(x, y, z-1) && futureTiles[x, y, z-1] != null && futureTiles[x, y, z-1] == ROCK) {
                rockCount++;
                bottom = true;
            }
            int angle = getAngle(right, left, top, bottom, rockCount);
            switch (rockCount) {
                case 0:
                    top3DGrid [x, y, z] = GameObject.Instantiate(topGO[0], 
                                                                 new Vector3(x, y, z), Quaternion.Euler(-90,angle,0)) as GameObject;
                    break;
                case 1:
                    top3DGrid [x, y, z] = GameObject.Instantiate(topGO[1], 
                                                                 new Vector3(x, y, z), Quaternion.Euler(-90,angle,0)) as GameObject;
                    break;
                case 2:
                    top3DGrid [x, y, z] = GameObject.Instantiate(topGO[2], 
                                                                 new Vector3(x, y, z), Quaternion.Euler(-90,angle,0)) as GameObject;
                    break;
                case 3:
                    top3DGrid [x, y, z] = GameObject.Instantiate(topGO[3], 
                                                                 new Vector3(x, y, z), Quaternion.Euler(-90,angle,0)) as GameObject;
                    break;
                case 4:
                    top3DGrid [x, y, z] = GameObject.Instantiate(topGO[4], 
                                                                 new Vector3(x, y, z), Quaternion.Euler(-90,angle,0)) as GameObject;
                    break;
                default:
                    break;
            }
        }

    }

    int getAngle(bool right, bool left, bool top, bool bottom, int rockCount)
    {
        if (rockCount == 1) {
            if (right)
            {
                return 90;
            }
            
            if (left)
            {
                return 270;
            }
            
            if (top) {
                return 0;
            }
            
            if (bottom) {
                return 90;
            }
        } else if (rockCount == 2) {
            if (top && right)
            {
                return 90;
            }
            
            if (top && left)
            {
                return 0;
            }
            
            if (bottom && right) {
                return 180;
            }
            
            if (bottom && left) {
                return -90;
            }
        } else if (rockCount == 3) {
            if (top && right && left)
            {
                return -90;
            }
            
            if (top && right && bottom)
            {
                return 0;
            }
            
            if (bottom && left && right) {
                return 90;
            }
            
            if (bottom && left && top) {
                return 180;
            }
        }

        return 0;
    }

    bool inBounds(int x, int y, int z)
    {
        return 0 <= x && x < levelSize 
            && 0 <= y && y < maxHillHeight + 1 
            && 0 <= z && z < levelSize;
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
        floorprobability = computeDensity(floorprobability);

        Random.Range(0f, 1f);
        for (int x = 0; x < this.levelSize; x++)
        {
            floorGrid.Add(new List<GameObject>());
            for (int z = 0; z < this.levelSize; z++)
            {
                float randomValue = Random.value;
                for (int i = 0; i < floorprobability.Count; i++)
                {
                    if (randomValue < floorprobability [i])
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

    void generateHouses() {
        foreach (List<Vector2> path in streetPaths)
        {
            foreach (Vector2 position in path) {
                createHouse(new Vector3(position.x, Random.value*maxHillHeight*0.5f, position.y));
            }
        }
    }

    Vector2 getDirection(Vector2 position)
    {
        Vector2 direction = new Vector2(0,0);

            float randomDirection = Random.value;
            if (randomDirection < 0.5) {
                direction = new Vector2(1,0);
            } else 
            {
                direction = new Vector2(-1,0);
            }
            Vector3 currentPosition = new Vector3(position.x + direction.x, 0, position.y + direction.y);
            if (validatePosition(currentPosition)) {
                return direction;
            } else
            {
                float dirX = direction.x;
                direction.x = 0;
                direction.y = dirX;
                currentPosition = new Vector3(position.x + direction.x, 0, position.y + direction.y);
                if(validatePosition(currentPosition))
                {
                    return direction;
                }
            } 
        return new Vector2(0, 0);
    }

    void createHouse(Vector3 position) {
        float randomValue = Random.value;
        if (randomValue < houseprobability) {
            Vector2 direction = getDirection(new Vector2(position.x, position.z));
            if(Vector2.SqrMagnitude(direction) > 0)
            {
                for(int i = 0; i < position.y; i++)
                {
                    top3DGrid[(int)(position.x + direction.x), i, (int)(position.z + direction.y)] = GameObject.Instantiate(house, new Vector3(position.x + direction.x, i + 1, position.z + direction.y), Quaternion.identity) as GameObject;
                }
            }
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
            && floorGrid [(int)currentPosition.x] [(int)currentPosition.z].tag == GRASS
            && futureTiles[(int) currentPosition.x, 0, (int) currentPosition.z] == null;
    }

    void placeHillTile(Vector3 position, Vector3 currentPosition, int iteration)
    {
        if (validatePosition(currentPosition))
        {
            if (top3DGrid [(int)currentPosition.x, 0, (int)currentPosition.z] == null) {
    
                for (int k = 1; k <= Random.value * position.y * Mathf.Pow(0.9f, iteration) + 1; k++)
                {
                    currentPosition.y = k;
                    futureTiles[(int)currentPosition.x, (int)currentPosition.y, (int)currentPosition.z] = ROCK;
                }
            }
        }
    }

    void computeHill(Vector3 position)
    {
        if (top3DGrid[(int)position.x, 0, (int)position.z] != null)
        {
            return;
        }
        if (validatePosition(position))
        {
            for (int i = 0; i < (int)position.y; i++)
            {
                futureTiles[(int)position.x, i + 1, (int)position.z] = ROCK;
            }


            Vector3 topRight = position;
            Vector3 bottomLeft = position;

            if (position.y == 1)
            {
                return;
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
