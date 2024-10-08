using UnityEngine;
using UnityEngine.Audio;

public class Projectile : MonoBehaviour
{
    public AudioSource hitSFX;

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
        if (other.gameObject != shooter.gameObject) //is the collider ran into not the object that shot the projectile?
        {
            
            HealthSystem health = other.GetComponent<HealthSystem>(); //get a health component from the collided object
            Pawn source = null;
            try
            {
                source = shooter?.GetComponent<Pawn>();
            }
            catch { };
            
            //is the health component there?
            if (health != null)
            {
                //PLAY HIT SOUND!
                GameManager.instance.PlaySFX(hitSFX);

                //Is there a pawn that is the source of the projectile?
                if (source != null) { health.TakeDamage(damage, source); }  //Take Damage from that source
                else { health.TakeDamage(damage); }                         //Take damage without a direct source
            }
        }
    }

}