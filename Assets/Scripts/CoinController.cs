using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CoinController : MonoBehaviour
{
    public float rotationSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        // distance in angles to rotate 
        float angle = rotationSpeed * Time.deltaTime;

        //rotate on Y
        transform.Rotate(Vector3.up, angle, Space.World);

        //rotate on Y

    }
}