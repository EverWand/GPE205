using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f; //How long the Projectile is in world
    public float damage = 10f;  // The Damage that the projectile has
    public GameObject shooter;  // The GameObject that shot this Projectile

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime); //destroy self after the the set lifetime
    }

    //Destroy self once running into a collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != shooter) //is the collider ran into not the object that shot the projectile?
        {
            HealthSystem health = other.GetComponent<HealthSystem>(); //get a health component from the collided object
            if (health != null)  //is the health component there?
            {
                health.TakeDamage(damage); //YES: have the collided object take damage
            }

            Destroy(gameObject); //destroy self
        }
    }
}