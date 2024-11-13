using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockARPlane : MonoBehaviour
{
    private GameObject mockPlane;

    void Awake()
    {
#if UNITY_EDITOR
        // Create a mock plane when testing in Play Mode in Unity Editor
        mockPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        mockPlane.transform.position = new Vector3(0, 0, 2); // Place it 2 units in front of the camera
        mockPlane.transform.localScale = Vector3.one * 0.5f; // Scale it down
        mockPlane.name = "Mock AR Plane";
#endif
    }

    void Update()
    {
        // Here you can simulate detection logic, for instance:
#if UNITY_EDITOR
        if (mockPlane != null)
        {
            Debug.Log("Simulated AR Plane Detected in Editor.");
        }
#endif
    }
}
