using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] ZombieArray;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = 100;
        QualitySettings.vSyncCount = 0;
    }


    void PlayZombieDeath()
    {
        var printRandom = (Random.Range(0, 3));
        source.PlayOneShot(ZombieArray[printRandom], 1);

    }
}
