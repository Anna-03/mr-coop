using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    public GameObject hook;

    public GameObject ingot;
    public GameObject body;
    public GameObject item;

    public MeshRenderer hookMesh;
    public MeshRenderer ingotMesh;
    public SkinnedMeshRenderer bodyMesh;

    public SkinnedMeshRenderer itemMesh;

    public Material ingotMaterial;

    public Material robotMaterialRaw;
    public Material robotMaterialPainted;
    public Material itemMaterial;

    public GameObject pillarsObj;
    public GameLogicManager gameLogicManager;
    GameObject[,] sockets;
    Animator animator;

    private bool isAnimationDone = false;


    // Start is called before the first frame update
    void Start()
    {
        ingotMesh = ingot.GetComponent<MeshRenderer>();
        bodyMesh = body.GetComponent<SkinnedMeshRenderer>();
        itemMesh = item.GetComponent<SkinnedMeshRenderer>();
        hookMesh = hook.GetComponent<MeshRenderer>();

        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ProgressFinished()
    {
        isAnimationDone = true;
    }
    public IEnumerator MoveTo(Vector3 startPosition, Vector3 endPosition)
    {
        // Start the animation from the beginning
        animator.Play("RobotEaseToOne", 0, 0f);
        // Wait one frame so Animator updates
        yield return null;
        // Keep updating position while the animation is running
        while (!isAnimationDone)
        {
            float progress = animator.GetFloat("progress");
            transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            Vector3 lookAtPositionFlat = endPosition;
            lookAtPositionFlat.y = transform.position.y;
            transform.LookAt(lookAtPositionFlat);
            yield return null; // Wait for next frame
        }

        // Ensure final position is exact
        transform.position = endPosition;

        // Reset the flag so it's ready for next time
        isAnimationDone = false;
        Debug.Log("Robot moving done!");

    }
}
