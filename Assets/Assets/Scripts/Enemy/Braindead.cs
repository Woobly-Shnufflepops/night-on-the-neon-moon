﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
     * Todo: 
     * Add public properties: GameObject target, bool chasingPlayer, float buildingAttackCooldown
     * Add public methods: void Aggro(), void TargetNearestBuilding()
     * On update:

        If state is entering:
        Move towards target. If at target, acquire nearest building as target (TargetNearestBuilding) and enter moving state.

        If state is dying:
        Disable collider and rigidbody. Play death animation. Destroy object.

        Check health. If 0, enter dying state.

        If state is moving:
            If not ChasingPlayer:
                Check if building is in range. If so, enter attacking state.
        If state is attacking:
            If attack is not on cooldown, damage building. Otherwise, do nothing.
        On collision:
            If collision target is tagged as player, make them receive damage (IDamageable)   
    */
public class Braindead : MonoBehaviour{

    //Properties
    public GameObject Target { get; set; }
    public bool ChasingPlayer { get; set; }
    public float BuildingAttackCooldown { get; set; }
    public float AggroRange { get; set; }
    public override int Health { get; set; }
    public override float MoveSpeed { get; set; }
    public override EnemyState CurrentState { get; set; }

    //THIS FIELD IS ONLY FOR TESTING UNTIL GAMEMANAGER IS COMPLETE
    public GameObject[] buildings = new GameObject[] { new GameObject("b1"),
        new GameObject("b2"), new GameObject("b3"), new GameObject("b4") };
    //END TESTING FIELD


    // Use this for initialization
    void Start () {
        BuildingAttackCooldown = 2f;
        AggroRange = .5f;
        MoveSpeed = 2f;

	}
	
	// Update is called once per frame
	void Update () {
        //check status
        CheckStatus();
        //check aggro
        Aggro();
        //move toward target
        //if at target, attack target
	}

    private void CheckStatus()
    {
        if(CurrentState == EnemyState.Entering)
        {

        }
        else if(CurrentState == EnemyState.Moving)
        {
            //move towards target
            Vector3.MoveTowards(this.gameObject.transform.position, Target.transform.position, MoveSpeed * Time.deltaTime);
        }
        else if (CurrentState == EnemyState.Attacking)
        {

        }
        else if (CurrentState == EnemyState.Dying)
        {
            //disable rigidbody and collider
            if (this.GetComponent<Rigidbody2D>() != null && this.GetComponent<BoxCollider2D>() != null)
            {
                Destroy(this.GetComponent<Rigidbody2D>());
                Destroy(this.GetComponent<BoxCollider2D>());
            }
            //play death anim
                //can skip for now
            //destroy gameobject
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            // Syntax:
            // ScriptName variableName = other.GetComponent<ScriptName>();
            // if(variableName != null)
            //     variableName.TakeDamage(damageAmount);
            //---------------------------------------------------------------
            // Player player = other.GetComponent<Player>();
            // if(player != null)
            //     player.TakeDamage(1);
        }
    }

    public void Aggro()
    {
        //if the difference between the player position and this gameObject position is <= .5f, target the player 
        if (Vector3.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= AggroRange)
        {
            Target = GameObject.FindGameObjectWithTag("Player");
            ChasingPlayer = true;
        }
        else
        {
            TargetNearestBuilding();
        }
    }

    public void TargetNearestBuilding()
    {
        GameObject closestBuilding = buildings[0];
        foreach (GameObject bldg in buildings)
        {
            if (Vector3.Distance(this.transform.position, bldg.transform.position) < Vector3.Distance(this.transform.position, closestBuilding.transform.position))
            {
                closestBuilding = bldg;
            }
        }
        Target = closestBuilding;
    }

    public override void TakeDamage(int amount)
    {
        this.Health -= amount;
        if(this.Health <= 0)
        {       
             CurrentState = EnemyState.Dying;
        }
    }
}


