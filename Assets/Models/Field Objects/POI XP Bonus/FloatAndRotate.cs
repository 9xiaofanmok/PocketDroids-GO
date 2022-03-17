using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAndRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 50;
    [SerializeField] private float floatAmplitude = 2.0f;
    [SerializeField] private float floatFrequency = 0.5f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        Debug.Log(startPosition);
        Debug.Log("hello!");
    }

    void Update()
    {
        Debug.Log("its working!");
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        Vector3 tempPosition = startPosition;
        tempPosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI * floatFrequency) * floatAmplitude;
        transform.position = tempPosition;
    }
}
