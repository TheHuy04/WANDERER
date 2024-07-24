using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaBot : MonoBehaviour
{
    public DetectionZone mechaAttackZone;
    Animator animator;
    Rigidbody rb;

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

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void Update()
    {
        HasTarget = mechaAttackZone.detectedCollinders.Count > 0;
    }
}
