using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbomMovement : MonoBehaviour
{
    const float tau = Mathf.PI * 2f;
    float framePosition1;
    float framePosition2;
    
    bool isStopping = false;
    bool hasRotated = false;
    float movementFactor = 0f;

    Vector3 rotationVector = new Vector3(0f, 180f, 0f);
    
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ApplySineWave();
        DetermineStopping();
        DetermineRotation();
        TurnCreature();
    }

    void ApplySineWave()
    {
        if (period <= Mathf.Epsilon) { return; } //Avoids dividing by a period of 0

        float cycles = Time.time / period; //Applies Sine Wave to creature's movement.
        float rawSineWave = Mathf.Sin(cycles * tau);
        movementFactor = -Mathf.Abs(rawSineWave);
        Vector3 offset = movementVector * movementFactor * Time.deltaTime;
        transform.Translate(offset, Space.Self);
    }

    void DetermineStopping()
    {
        if (Mathf.Abs(movementFactor) < 0.01)
        {
            isStopping = true;
        }

        else
        {
            isStopping = false;
        }
    }

    void DetermineRotation()
    {
        if (Mathf.Abs(movementFactor) > 0.5)
        {
            hasRotated = false;
        }
    }

    void TurnCreature()
    {
        if (isStopping == true & hasRotated == false)
        {
            transform.Rotate(rotationVector, Space.Self);
            hasRotated = true;
        }
    }
}