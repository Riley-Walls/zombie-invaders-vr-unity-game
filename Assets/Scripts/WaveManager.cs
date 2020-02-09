using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public float xShift = 1.5f;
    public float yShift = .2f;
    public float speed = .01f;
    public float xPos = 0;
    public float yPos = 0;
    bool moveRight = true;
    bool moveDown = false;

    public List<GameObject> enemyPrefabs;

    public GameObject basicSpritePrefab;
    public GameObject enemiesContainerObj;
    public GameObject gridPositionPrefab;
    public GameObject gridPositionContainerObj;

    public GridObject[,] waveGrid;

    public int gridWidth = 10;
    public int gridHeight = 5;
    private float gridHeightGap = .2f;
    private float gridWidthGap = .2f;

    private int minimumChainLength = 3;

    private float accumulatedSpawnOffsetX = 0;
    private float accumulatedSpawnOffsetY = 0;

    private void Awake()
    {
        waveGrid = new GridObject[gridWidth, gridHeight];

        for (int i = 0; i < gridHeight; i++)
        {

            accumulatedSpawnOffsetX = 0;
            accumulatedSpawnOffsetY += gridHeightGap;

            for (int j = 0; j < gridWidth; j++)
            {
                waveGrid[j, i] = new GridObject();


                Vector3 spawnPosition = transform.position;

                accumulatedSpawnOffsetX += gridWidthGap;

                spawnPosition.x = transform.position.x + j + accumulatedSpawnOffsetX;
                spawnPosition.y = transform.position.y + i + accumulatedSpawnOffsetY;

                //SPAWN INVADER
                //Random Spawn
                int prefabIndexToSpawn;
                float randomNum = Random.Range(0f, 1f);
                if (randomNum < .5f)
                {
                    
                    prefabIndexToSpawn = 0;
                }
                else
                {
                    prefabIndexToSpawn = Random.Range(1, enemyPrefabs.Count);
                }


                GameObject newInvaderObj = Instantiate(enemyPrefabs[prefabIndexToSpawn], spawnPosition, Quaternion.identity);
                newInvaderObj.transform.parent = enemiesContainerObj.transform;
                newInvaderObj.GetComponent<BaseInvader>().xPos = j;
                newInvaderObj.GetComponent<BaseInvader>().yPos = i;
                newInvaderObj.GetComponent<BaseInvader>().destinationX = j;
                newInvaderObj.GetComponent<BaseInvader>().destinationY = i;
                newInvaderObj.GetComponent<BaseInvader>().InvaderDied += OnInvaderDeath;
                newInvaderObj.GetComponent<BaseInvader>().waveManager = this;
                //newInvaderObj.GetComponent<BaseInvader>().enemyTypeIndex = 0;

                waveGrid[j, i].gameObj = newInvaderObj;

                //DEBUG - TEST CHAINING-----------------------------
                /*
                if (i == 1 || i == 2)
                {
                    if (j == 1 || j == 2)
                    {
                        newInvaderObj.GetComponent<BaseInvader>().enemyTypeIndex = 1;
                    }
                }
                if (i == 2)
                {
                    if (j == 3)
                    {
                        newInvaderObj.GetComponent<BaseInvader>().enemyTypeIndex = 1;
                    }
                }
                */
                //-------------------------------------
                /*
                if(Random.Range(0, 100) > 50)
                {
                    newInvaderObj.GetComponent<BaseInvader>().enemyTypeIndex = 1;
                }
                */

                //SPAWN GRID POSITION
                GameObject newGridPosition = Instantiate(gridPositionPrefab, spawnPosition, Quaternion.identity);
                waveGrid[j, i].positionObj = newGridPosition;
                newGridPosition.transform.parent = gridPositionContainerObj.transform;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {


    }


    // Update is called once per frame
    void Update()
    {
        //UPDATE WAVE MOVEMENT
        if (!moveDown)
        {
            if (moveRight)
            {
                if (xPos >= xShift)
                {
                    moveRight = false;
                    moveDown = true;
                }
                transform.Translate(new Vector3(1 * speed, 0, 0));
                xPos += 1 * speed;
            }
            else if (!moveRight)
            {
                if (xPos <= 0)
                {
                    moveRight = true;
                    moveDown = true;
                }
                transform.Translate(new Vector3(-1 * speed, 0, 0));
                xPos += -1 * speed;
            }
        }
        else
        {

            //MOVE DOWN
            if(yPos <= yShift)
            {
                transform.Translate(new Vector3(0, -1 * speed, 0));
                yPos += 1 * speed;
            }
            else
            {
                yPos = 0;
                moveDown = false;
            }

        }

        //Debug.Log(xPos.ToString());

    }

    void OnInvaderDeath(BaseInvader invader)
    {
        //Destroy(invader.gameObject);
    }


    public void CheckChaining()
    {
        int chainLength = 0;
        int currentChainInvaderTypeIndex = 99;
        ArrayList enemiesInChain = new ArrayList();

        for (int i = 0; i < gridHeight; i++)
        {
            currentChainInvaderTypeIndex = 99;

            for (int j = 0; j < gridWidth; j++)
            {
                //DEBUG - RESET COLORS
                /*
                if(waveGrid[j, i].gameObj != null)
                {
                    Color color;
                    waveGrid[j, i].gameObj.GetComponent<BaseInvader>().SpriteRenderer.material.color = Color.red;
                    switch (waveGrid[j, i].gameObj.GetComponent<BaseInvader>().enemyTypeIndex)
                    {
                        case 0: color = Color.red; break;
                        case 1: color = Color.green; break;
                        case 2: color = Color.blue; break;
                        default: color = Color.yellow; break;
                    }
                    waveGrid[j, i].gameObj.GetComponent<BaseInvader>().SpriteRenderer.material.color = color;
                }
                */
                //-----------------------


                if (waveGrid[j, i].gameObj != null && waveGrid[j, i].gameObj.GetComponent<BaseInvader>() != null)
                {
                    //IF THE INDEX MATCHES...ELSE
                    if (waveGrid[j, i].gameObj.GetComponent<BaseInvader>().enemyTypeIndex == currentChainInvaderTypeIndex)
                    {

                        enemiesInChain.Add(waveGrid[j, i].gameObj);

                    }
                    else
                    {

                        //CLEAR THE LIST AND START AGAIN
                        enemiesInChain.Clear();
                        currentChainInvaderTypeIndex = 99;
                        //UPDATE THE CURRENT CHAIN TYPE
                        currentChainInvaderTypeIndex = waveGrid[j, i].gameObj.GetComponent<BaseInvader>().enemyTypeIndex;
                        //ADD THE NEW ENEMY TO THE LIST
                        enemiesInChain.Add(waveGrid[j, i].gameObj);
                    }


                    //IF A THE CHAIN IS LARGE ENOUGH...
                    if (enemiesInChain.Count >= minimumChainLength)
                    {
                        if (((GameObject)enemiesInChain[0]).GetComponent<BaseInvader>().enemyTypeIndex != 0)
                            Debug.Log("Destroying chain of size: " + enemiesInChain.Count);


                        foreach (GameObject inv in enemiesInChain)
                        {
                            //DEBUG - IGNORE TYPE 0 (RED) FOR NOW
                            if (((GameObject)enemiesInChain[0]).GetComponent<BaseInvader>().enemyTypeIndex != 0)
                            {
                                //inv.GetComponent<BaseInvader>().SpriteRenderer.material.color = Color.yellow;

                                GameObject tempObj = waveGrid[j, i].gameObj;
                                waveGrid[j, i].gameObj = null;
                                inv.GetComponent<BaseInvader>().Kill();
                                //waveGrid[j, i].gameObj = null;
                            }

                            //--------------------------------------------

                            //inv.GetComponent<BaseInvader>().Kill();
                        }
                    }
                    

                }
                else
                {
                    enemiesInChain.Clear();
                    currentChainInvaderTypeIndex = 99;
                }
            }
        }
        
        
    }


}

public class GridObject
{
    public GameObject positionObj;
    public GameObject gameObj;
}
