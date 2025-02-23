using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FireBall : MonoBehaviour
{
    private float spawnTimer = 5f;
    public bool canBeDestroyed = false;
    private AudioSource audioSource;

    private void Start()
    {
        /*
        audioSource= GetComponent<AudioSource>();
        audioSource.rolloffMode= AudioRolloffMode.Linear;
        audioSource.minDistance = 1;
        audioSource.minDistance = 15;
        */
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer < 0)
        {
            canBeDestroyed= true;
        }
    }
}
