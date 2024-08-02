using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    //Structure of Volumes used for different noises the player makes

    public float movementVolume;
    public float turnVolume;
    public float primaryFireVolume;

    public float noiseDistance; //Tracks the noise's radius
    public float noiseTime;     //Time it takes until noise resets
    // Coroutine to reset noiseDistance after noiseTime
    private IEnumerator NoiseTimer()
    {
        yield return new WaitForSeconds(noiseTime);
        noiseDistance = 0;
    }

    // Method to make noise with specified volume
    public void MakeNoise(float noiseVolume)
    {
        // Set noiseDistance to the given volume
        noiseDistance = noiseVolume;

        // Start the NoiseTimer coroutine to reset noiseDistance after noiseTime
        StartCoroutine(NoiseTimer());
    }
}