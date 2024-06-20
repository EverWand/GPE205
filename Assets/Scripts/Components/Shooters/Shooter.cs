using UnityEngine;

public abstract class Shooter : MonoBehaviour
{

    //Variables
    public float damage = 10f;          //Sets the Shooters Damage

    public float fireForce = 1000f;     //sets the force of the projectile force
    public float fireRate = 1f;         //The rate the cooldown fills up
    private float fireCoolDown = 0f;    //cool down timer

    public GameObject projectilePrefab; //the projectile that the Shooter Shoots
    public Transform firePoint;         //The transform where to fire the projectile from

    public bool canShoot()
    {
        return Time.time >= fireCoolDown;
    }

    public void Shoot()
    {
        if (!canShoot()) return; //Return if the Shooter is still on cooldown

        fireCoolDown = Time.time + 1f / fireRate; //begin cooldown

        //Spawn Projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation); //Instantiate the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();    //Get the rigidbody of the projectile
        Projectile projectileScript = projectile.GetComponent<Projectile>(); // get the Projectile component of the projectile

        projectileScript.damage = damage;   //set the damage of the projectile
        projectileScript.shooter = gameObject;  //set the projectile's Shooter object to the Shooter object that spawned it

        rb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse); //add an impulse to the projectile's rigidbody to shoot it
    }
}
