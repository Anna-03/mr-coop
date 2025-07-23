using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.ImmersiveDebugger;


using Oculus;

public class MovePillarsWithRightJoystick : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float radius = 1f;
    bool anchorInitialized = false;
    float delaySeconds = 1f;
    string rootName = "PillarsRoot";
    Vector3 offsetPos = new Vector3(0, 0, 0);

    void Start()
    {



        placePillars();
        if (OVRManager.isHmdPresent && !Application.isEditor)
        {
            StartCoroutine(DelayedAnchorSetup());
            Debug.Log("Spatial Anchor Coroutine started");
        }
    }
    private IEnumerator DelayedAnchorSetup()
    {
        yield return new WaitForSeconds(delaySeconds);
        InitializeAnchor();
    }

    private void InitializeAnchor()
    {
        if (anchorInitialized) return; // Prevent duplicate runs
        anchorInitialized = true;

        // Create root at current position/rotation
        GameObject pillarsRoot = new GameObject(rootName);
        pillarsRoot.transform.SetPositionAndRotation(transform.position, transform.rotation);

        // Add runtime anchor
        pillarsRoot.AddComponent<OVRSpatialAnchor>();

        // Re-parent this object (Pillars) under the new anchored root
        transform.SetParent(pillarsRoot.transform, worldPositionStays: true);

        Debug.Log("Pillars anchored under '" + rootName + "'");
    }
    void Update()
    {
        // InputRight vom rechten Joystick holen
        Vector2 inputRight = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        Vector2 inputLeft = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Transform cameraTransform = Camera.main.transform;
            Vector3 cameraXY = new Vector3(cameraTransform.position.x, 0, cameraTransform.position.z);
            Vector3 selfXY = new Vector3(transform.position.x, 0, transform.position.z);
            offsetPos = cameraXY - selfXY;
        }
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            AlignToHeadset();
        }


        if (inputRight.magnitude > 0.1f)
        {
            Vector3 inputRightDir = new Vector3(inputRight.x, 0, inputRight.y);

            // Kamera-Forward auf XZ-Ebene (Y=0) projizieren und normalisieren
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            // Rechts-Vektor von Kamera berechnen
            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            // Bewegung in Kamera-Raum umrechnen
            Vector3 moveDir = cameraRight * inputRightDir.x + cameraForward * inputRightDir.z;

            // Bewegung anwenden
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        if (Mathf.Abs(inputLeft.x) > 0.7f)
        {
            // Bewegung anwenden
            transform.Rotate(0, inputLeft.x * moveSpeed * 30.0f * Time.deltaTime, 0);
        }
        if (Mathf.Abs(inputLeft.y) > 0.7f)
        {
            // Bewegung anwenden
            radius += inputLeft.y * moveSpeed * Time.deltaTime / 2.0f;
            placePillars();
        }

    }

    void placePillars()
    {
        int childCount = transform.childCount;
        float angleStep = 360f / childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // Calculate angle in radians
            float angle = i * angleStep * Mathf.Deg2Rad;

            // Position on circle
            Vector3 position = new Vector3(
                Mathf.Sin(angle) * radius, // X
                0f,
                Mathf.Cos(angle) * radius  // Z
            );

            child.localPosition = position;

            // Make the pillar face the center (0,0,0 relative to parent)
            child.LookAt(transform.position);
        }
    }
    void AlignToHeadset()
    {
        Transform cameraTransform = Camera.main.transform;
        Vector3 cameraXY = new Vector3(cameraTransform.position.x, 0, cameraTransform.position.z);
        Vector3 selfXY = new Vector3(transform.position.x, 0, transform.position.z);

        transform.position = cameraXY - offsetPos;

        // Match Y-axis (yaw) rotation, ignore pitch and roll
        float cameraYaw = cameraTransform.eulerAngles.y;
        // transform.rotation = Quaternion.Euler(0, cameraYaw, 0);
        // Get headset transform

    }

}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// using Oculus;

// public class MovePillarsWithRightJoystick : MonoBehaviour
// {
//     public float moveSpeed = 1.5f;
//     public float radius = 1f;
//     void Start()
//     {
//         placePillars();
//     }
//     void Update()
//     {
//         // InputRight vom rechten Joystick holen
//         Vector2 inputRight = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
//         Vector2 inputLeft = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

//         if (inputRight.magnitude > 0.1f)
//         {
//             Vector3 inputRightDir = new Vector3(inputRight.x, 0, inputRight.y);

//             // Kamera-Forward auf XZ-Ebene (Y=0) projizieren und normalisieren
//             Vector3 cameraForward = Camera.main.transform.forward;
//             cameraForward.y = 0;
//             cameraForward.Normalize();

//             // Rechts-Vektor von Kamera berechnen
//             Vector3 cameraRight = Camera.main.transform.right;
//             cameraRight.y = 0;
//             cameraRight.Normalize();

//             // Bewegung in Kamera-Raum umrechnen
//             Vector3 moveDir = cameraRight * inputRightDir.x + cameraForward * inputRightDir.z;

//             // Bewegung anwenden
//             transform.position += moveDir * moveSpeed * Time.deltaTime;
//         }
//         if (Mathf.Abs(inputLeft.x) > 0.7f)
//         {
//             // Bewegung anwenden
//             transform.Rotate(0, inputLeft.x * moveSpeed * 30.0f * Time.deltaTime, 0);
//         }
//         if (Mathf.Abs(inputLeft.y) > 0.7f)
//         {
//             // Bewegung anwenden
//             radius += inputLeft.y * moveSpeed * Time.deltaTime / 2.0f;
//             placePillars();
//         }

//     }
//     void placePillars()
//     {
//                 int childCount = transform.childCount;
//         float angleStep = 360f / childCount;

//         for (int i = 0; i < childCount; i++)
//         {
//             Transform child = transform.GetChild(i);

//             // Calculate angle in radians
//             float angle = i * angleStep * Mathf.Deg2Rad;

//             // Position on circle
//             Vector3 position = new Vector3(
//                 Mathf.Cos(angle) * radius,
//                 0f,
//                 Mathf.Sin(angle) * radius
//             );

//             child.localPosition = position;

//             // Make the pillar face the center (0,0,0 relative to parent)
//             child.LookAt(transform.position);
//         }
//     }
// }