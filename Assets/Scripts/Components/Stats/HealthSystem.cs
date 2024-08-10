using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;


public class HealthSystem : MonoBehaviour
{
    //AUDIO
    public AudioSource dieSFX;

    //HEALTH VARIABLES
    public float maxHealth = 100; //Sets the Max Health of the owner
    public float currHealth;      //tracks the current Objects Health

    //Events
    public UnityEvent OnHealed;            //Triggered when Healed
    public UnityEvent ReachedMaxHealth;    //Triggered when the Health reaches the Max [Not when the health is at max]
    public UnityEvent OnDamaged;           //Triggered when Damaged
    public UnityEvent OnUpdateHealth;      //Triggered whenever the Health is Updated
    public event Action OnDeath;           //Triggers when gameobject dies

    //====| SCHEDULES |====
    // Start is called before the first frame update
    void Start()
    {
        InitHealth(); //Initialize Health
    }

    //====| HEALTH FUNCTIONS |====
    private void InitHealth() //Initializes Health 
    {
        currHealth = maxHealth;     //Reinitialize Health
        OnUpdateHealth?.Invoke();   //signal that the health refreshed
    }
    public void Heal(float amount, Pawn source = null) //Heal health by amount
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
    public void TakeDamage(float amount, Pawn source = null) //Lose health by amount
    {
        //Checks if Health is not empty already
        if (currHealth > 0)
        {
            currHealth = Mathf.Clamp(currHealth - amount, 0, maxHealth); //subtract health to the current health, keeping it between 0 and its max health
            OnDamaged?.Invoke(); //invoke the Damaged Event
            OnUpdateHealth?.Invoke(); //invoke the Update Health Event
        }

        if (currHealth <= 0)
        {
            if (source != null) { Die(source); } // Die when there's a source of death
            else { Die(); } //Still die without a specific source of death
        }

    }

    //====| GENERAL FUNCTIONS |====
    //When the Object Dies
    public void Die(Pawn source = null)
    {
        GameManager.instance.PlaySFX(dieSFX);


        OnDeath?.Invoke(); //Signal Death

        Pawn pawn = GetComponent<Pawn>(); //This GameObject's Pawn

        //Debug.Log(pawn.gameObject.name + " Has Died!");

        //====| DEATH PENALTY |====
        if (pawn != null)
        {
            //Debug.Log(pawn.gameObject.name + " has a Pawn");

            //Is there a specific Source of death given?
            if (source != null)
            {
                pawn.DealScoreReward(source?.controller);   //Deal out score to the sourced controller
            }

            pawn.controller?.RemoveLives(1); //Remove a life from owner pawn
            //Debug.Log("CONTROLLER: " + pawn.controller.name);

            //Does the Pawn still have lives?
            if (pawn.controller?.lives > 0)
            {
                //Debug.Log("Respawning " + pawn.gameObject.name);
                
                //===| RESPAWN |===
                //Spawn in new location
                gameObject.transform.position = GameManager.instance.mapGenerator.getRandPawnSpawn().transform.position;
                //Initialize Health
                InitHealth();
            }
        }
    }
}
