using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseInvader : MonoBehaviour
{
    public int enemyTypeIndex;

    public int xPos;
    public int yPos;

    public UnityAction<BaseInvader> InvaderDied;

    public int destinationX = 0;
    public int destinationY = 0;

    public WaveManager waveManager;
    public SpriteRenderer SpriteRenderer;
    public GameObject cascadeColliderObj;

    int cascadeColliderFrameCount = 0;

    bool setToDestroy = false;
    bool checkChainingFlagNextFrame = false;

    private float movementSpeed = 1f;

    bool isMoving = false;

    public Camera cameraShake;

    public GameObject SoundSource;

    // Start is called before the first frame update
    void Start()
    {
        SoundSource = GameObject.FindGameObjectWithTag("Sound");
        //cameraShake = GetComponent<Camera>();
        cameraShake = GameObject.FindObjectOfType<Camera>();
        cascadeColliderObj.GetComponent<CascadeColliderRelay>().OnCollisionEnterAction += CascadeColliderOnCollisionEnter;

        //DEBUG - SET COLOR BY ENEMY TYPE
        /*
        Color color;
        switch (enemyTypeIndex)
        {
            case 0: color = Color.red; break;
            case 1: color = Color.green; break;
            case 2: color = Color.blue; break;
            default: color = Color.yellow; break;
        }

        SpriteRenderer.material.color = color;
        */
    }

    // Update is called once per frame
    void Update()
    {
        //CHECK FOR CHAINING
        if (checkChainingFlagNextFrame)
        {
            waveManager.CheckChaining();
            checkChainingFlagNextFrame = false;
        }



        if(waveManager.waveGrid[destinationX, destinationY] == null)
        {
            Debug.Log("it's null! X: " + destinationX + "Y: " + destinationY);
        }

        if (destinationX != xPos || destinationY != yPos)
        {
            //FIRST MOVEMENT FRAME
            if (!isMoving)
            {
                //Update WaveManager grid
                isMoving = true;
                waveManager.waveGrid[xPos, yPos].gameObj = null;
                
            }

            //NOT ARRIVED YET
            if (gameObject.transform.position != waveManager.waveGrid[destinationX, destinationY].positionObj.transform.position)
            {
                float step = movementSpeed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, waveManager.waveGrid[destinationX, destinationY].positionObj.transform.position, step);
            }
            else
            {
                //ARRIVED
                xPos = destinationX;
                yPos = destinationY;

                //Update WaveManager grid
                isMoving = false;
                waveManager.waveGrid[xPos, yPos].gameObj = this.gameObject;

                checkChainingFlagNextFrame = true;
                
            }
        }

        if (setToDestroy)
        {
            if (cascadeColliderFrameCount > 1)
            {
                cascadeColliderObj.SetActive(false);
                InvaderDied.Invoke(this);
                waveManager.waveGrid[xPos, yPos].gameObj = null;
                //ZombieDeath.Play(ZombieDeathSFX,1);
                Destroy(this.gameObject);
                SoundSource.SendMessage("PlayZombieDeath");
                cameraShake.SendMessage("ScreenShake");


            }
            else
            {
                cascadeColliderFrameCount++;
            }

        }


    }


    public void Kill()
    {
        setToDestroy = true;
        cascadeColliderObj.gameObject.SetActive(true);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }

    private void CascadeColliderOnCollisionEnter(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BaseInvader>() != null)
        {
            int destinationX = collision.gameObject.GetComponent<BaseInvader>().xPos;
            if (collision.gameObject.GetComponent<BaseInvader>().yPos > 0)
            {
                if (collision.gameObject.GetComponent<BaseInvader>().destinationY > 0)
                {
                    destinationY = collision.gameObject.GetComponent<BaseInvader>().destinationY - 1;
                }
                else
                {
                    destinationY = collision.gameObject.GetComponent<BaseInvader>().yPos - 1;
                }
            }

            collision.gameObject.GetComponent<BaseInvader>().SetDestination(destinationX, destinationY);

        }

    }


    public void SetDestination(int x, int y)
    {
        if(destinationX < waveManager.gridWidth && destinationY > 0)
        {
            destinationX = x;
        }
        if (destinationY < waveManager.gridHeight && destinationY > 0)
        {
            destinationY = y;
        }

        //WaveManager.waveGrid[xPos, yPos]. = new GridObject();
    }

}
