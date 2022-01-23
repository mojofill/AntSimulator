using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromone : MonoBehaviour
{
    public float intensity = 1f;

    public int type; // home or food - from Pheromone Constants

    private float frames_since_placement = 0f;

    void FixedUpdate()
    {
        if (intensity <= 0) {
            Destroy(gameObject);
        }

        else {
            evaporate(frames_since_placement);
            updateEvaporationFrames();
        }
    }

    void setPheromoneType(int type) {
        this.type = type;
    }

    void evaporate(float x) {
        // takes in the frames

        intensity = (-1 / 200) * x + 1;
    }

    void updateEvaporationFrames() {
        frames_since_placement += Time.fixedDeltaTime;
    }
}
