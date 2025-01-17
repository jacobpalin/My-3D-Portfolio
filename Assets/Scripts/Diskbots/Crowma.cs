using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowma : Interactable
{
    // Public variables to adjust min and max wait time in the Inspector
    public float minWaitTime = 1f;
    public float maxWaitTime = 5f;

    // Animator component reference
    private Animator animator;

    // Internal timer and delay
    private float timer = 0f;
    private float delay = 0f;

    void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();

        // Set the initial delay to a random value
        delay = Random.Range(minWaitTime, maxWaitTime);
    }

    void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // Check if the timer exceeds the current delay
        if (timer >= delay)
        {
            // Trigger the animation
            animator.SetTrigger("IdleAction");

            // Reset the timer
            timer = 0f;

            // Pick a new random delay
            delay = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    public override void OnInteract()
    {
        animator.SetTrigger("Ability");
    }
    public override void OnHover()
    {
        base.OnHover();
    }
    public override void OnHoverExit()
    {
        base.OnHoverExit();
    }
}