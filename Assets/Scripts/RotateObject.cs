using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private bool spinRight;
    [SerializeField] private float spinSpeed;
    private float zRotation;
    void Update()
    {
        if (spinRight) zRotation -= Time.deltaTime * spinSpeed;
        else zRotation += Time.deltaTime * spinSpeed;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
    }
}