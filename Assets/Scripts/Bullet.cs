using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 velocity;
    private int collisionCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collisionCount < 1)
        {
            if (collision.gameObject.GetComponent<BaseInvader>() != null)
            {
                
                collision.gameObject.GetComponent<BaseInvader>().Kill();
                collisionCount++;
                Destroy(this.gameObject);
                
            }
        }
    }


}
