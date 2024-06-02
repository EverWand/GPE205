using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    //HEALTH VARIABLES
    public float maxHealth = 100; //Sets the Max Health of the owner
    public float currHealth;      //tracks the current Objects Health

    //References
    private GameObject bodyObject; //Game Object that owns this component

    //Events
    public UnityEvent OnHealed;            //Triggered when Healed
    public UnityEvent ReachedMaxHealth;    //Triggered when the Health reaches the Max [Not when the health is at max]
    public UnityEvent OnDamaged;           //Triggered when Damaged
    public UnityEvent OnUpdateHealth;      //Triggered whenever the Health is Updated

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth; //Intialize current health to the set health
    }

    public void Heal(float amount) //Heal health by amount
    {
        bool atMax; //Boolean to track if the healt is initially at max

        //Check if the Health is lower than the Max health
        if (currHealth < maxHealth)
        {
            atMax = false; //Not initialy at Max Health: Update Health by Healing
            
            currHealth = Mathf.Clamp(currHealth + amount, 0, maxHealth); //add health to the current health, keeping it between 0 and its max health
            OnHealed?.Invoke(); //invoke the Damaged Event
            OnUpdateHealth?.Invoke(); //invoke the Update Health Event
        }
        //Initially Maxed Out: No health Updates Happen
        else
        {
            atMax = true;
        }
        //Does Health reach the Max Health after healing?
        if (!atMax && currHealth >= maxHealth)
        {
            ReachedMaxHealth?.Invoke(); //invoke the max reached Event
        }
    }

    public void TakeDamage(float amount) //Lose health by amount
    {
        //Checks if Damages is not empty already
        if (currHealth > 0) 
        {
            currHealth = Mathf.Clamp(currHealth - amount, 0, maxHealth); //subtract health to the current health, keeping it between 0 and its max health
            OnDamaged?.Invoke(); //invoke the Damaged Event
            OnUpdateHealth?.Invoke(); //invoke the Update Health Event
        }
    }

    //When there needs to check if the object is dead
    public void CheckForDeath() 
    {
        //Die if at 0 health
        if (currHealth <= 0)
        {
            Die(); //Run the Death Method
        } 
    }

    public void Die() //When the Object Dies
    {
        Object.Destroy(gameObject); //Destroy the Owning Object
    }
}
