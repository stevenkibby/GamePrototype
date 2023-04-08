using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineMovement : MonoBehaviour
{
    Vector3 startingPosition;
    const float tau = Mathf.PI * 2f;
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) {return;}

        float cycles = Time.time / period;
        float rawSineWave = Mathf.Sin(cycles * tau);
        float movementFactor = rawSineWave;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
