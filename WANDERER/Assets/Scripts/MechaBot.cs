using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaBot : MonoBehaviour
{
    public float flightSpeed = 3f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone mechaAttackZone;
    public List<Transform> waypoints;


    Animator animator;
    Rigidbody rb;
    DamageAble DamageAble;

    Transform nextWaypoint;
    int waypointNum = 0;

    public bool _hasTarget = false;
    

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        DamageAble = GetComponent<DamageAble>();
        
    }

    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = mechaAttackZone.detectedCollinders.Count > 0;
    }

    private void FixedUpdate()
    {
        if (DamageAble.IsAlive)
        {
            if(CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void Flight()
    {
        //fly to next waypoints
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        //check if we have reached the waypoint already
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;

        //see if we need to switch waypoint
        if(distance < waypointReachedDistance)
        {
            //switch to next waypoint
            waypointNum++;

            if(waypointNum >= waypoints.Count)
            {
                //loop back to original waypoint
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }
}
