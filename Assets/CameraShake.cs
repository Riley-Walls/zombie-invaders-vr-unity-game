using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float ShakeAmount = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ScreenShake()
    {
        iTween.ShakePosition(gameObject, Vector3.one * ShakeAmount, 0.2f);
    }
}
