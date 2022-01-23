using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public float radius = 500f;
    public float viewRadius = 3f;
    public float viewAngle = 120f;

    public LayerMask foodLayer;
    public int takenFoodLayer = 8;
    public LayerMask wallLayer;
    public Transform targetFood;

    public float foodPickupRadius = 0.2f; // how big the radius to check for food and pick up

    public Transform head;

    public float antSpeed = 20f;
    // YO - might add a min direction change range and max, and select random float in between
    public float randomDirectionChangeRange = 1f;

    public float wallCheckRadius = 0.1f;

    public GameObject HomePheromone;
    public GameObject FoodPheromone;

    public float sinceLastPheromoneDrop = 0f;

    public float pheromoneDropFrames = 1f; // every frame drop a pheromone

    public int type = PheromoneConstants.FOOD; // compare with pheromone constants

    void Start() {
        head = transform.GetChild(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // just move the ant randomly around for now
        RandomMove();
        HandleFood();
        PushFromWall();

        biasMoveToPheromoneType();

        if (sinceLastPheromoneDrop >= pheromoneDropFrames) {
            dropPheromone();
            sinceLastPheromoneDrop = 0f;
        }

        else {
            sinceLastPheromoneDrop += Time.fixedDeltaTime;
        }
    }

    void dropPheromone() {
        GameObject droppedPheromone;
        
        if (type == PheromoneConstants.HOME) {
            droppedPheromone = Instantiate(HomePheromone, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
        }

        else if (type == PheromoneConstants.FOOD) {
            droppedPheromone = Instantiate(FoodPheromone, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
            Debug.Log("i dropped food off??");
        }
    }

    void HandleFood() {
        // if the ant has not found food yet
        if (targetFood == null) {
            Collider2D[] allFood = Physics2D.OverlapCircleAll(transform.position, viewRadius, foodLayer);

            if (allFood.Length > 0) {
                Transform food = allFood[Random.Range(0, allFood.Length)].transform;

                Vector2 directionToFood = (food.position - head.position).normalized;

                if (Vector2.Angle(Vector2.up, directionToFood) < viewAngle / 2) { // the angle between forward and the point where food is is BELOW the max angle 
                    // basically imagine a 120 degree angle in front of the ant that represents its radar, and code above checks if point is inside radar

                    food.gameObject.layer = takenFoodLayer;
                    targetFood = food;

                    type = PheromoneConstants.FOOD;
                    Debug.Log("set type as food");
                }
            }
        }

        else { // ant has found food
            Vector2 desiredDirection = (targetFood.position - head.position).normalized; // direction to get to the target food

            transform.position += new Vector3(desiredDirection.x, desiredDirection.y, 0) * antSpeed * Time.fixedDeltaTime;

            if (Vector2.Distance(targetFood.position, head.position) < foodPickupRadius) { // ant is within distance to pick up the food
                targetFood.localScale *= 0.9f;

                targetFood.position = head.position + new Vector3(0f, 0.9f, 0f);
                targetFood.parent = head;
                targetFood = null;

                // once we've found food, go back home
                type = PheromoneConstants.HOME;
            }
        }
    }

    void biasMoveToPheromoneType() {
        // sample all pheromones in view angle, and select the one that has the strongest intensity

    }

    void dropOffFood() {
        // drop food off and feed the generation of new ants

        type = PheromoneConstants.FOOD;
    }

    void RandomMove() {
        // add pheromone sensor

        float direction_push = Random.Range(-randomDirectionChangeRange, randomDirectionChangeRange);

        transform.Rotate(new Vector3(0, 0, direction_push));

        transform.position += transform.up * antSpeed * Time.fixedDeltaTime;
    }

    void PushFromWall() {
        Collider2D wall = Physics2D.OverlapCircle(transform.position, wallCheckRadius, wallLayer);

        if (wall != null) {
            Vector3 normal = wall.bounds.center.normalized;

            Vector3 newDirection;

            if (normal == Vector3.left || normal == Vector3.down) {
                newDirection = new Vector3(0, 0, Random.Range(-150f, -210f));
            }

            else {
                newDirection = new Vector3(0, 0, Random.Range(150f, 210f));
            }

            transform.Rotate(newDirection);

            transform.position += transform.up * antSpeed * Time.fixedDeltaTime;
        }
    }

    float SenseFoodPheromoneIntensity() {
        // returns the intensity of the nearest food pheromone
        float intensity = 0f;

        return intensity;
    }

    float SenseHomePheromoneIntensity() {
        float intensity = 0f;

        return intensity;
    }
}
