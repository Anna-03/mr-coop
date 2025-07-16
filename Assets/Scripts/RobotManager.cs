using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    public GameObject ingot;
    public GameObject body;
    public GameObject club;
    public GameObject humanMask;
    public GameObject jetpack;
    public GameObject monocle;
    public GameObject umbrella;

    public MeshRenderer ingotMesh;
    public SkinnedMeshRenderer bodyMesh;
    public SkinnedMeshRenderer clubMesh;
    public SkinnedMeshRenderer humanMaskMesh;
    public SkinnedMeshRenderer jetpackMesh;
    public SkinnedMeshRenderer monocleMesh;
    public SkinnedMeshRenderer umbrellaMesh;
    public SkinnedMeshRenderer[] itemRendererLookup;

    public Material ingotMaterial;
    public Material robotBlackMaterial;
    public Material robotWhiteMaterial;
    public Material robotRedMaterial;
    public Material[] materialLookup;

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
        clubMesh = club.GetComponent<SkinnedMeshRenderer>();
        humanMaskMesh = humanMask.GetComponent<SkinnedMeshRenderer>();
        jetpackMesh = jetpack.GetComponent<SkinnedMeshRenderer>();
        monocleMesh = monocle.GetComponent<SkinnedMeshRenderer>();
        umbrellaMesh = umbrella.GetComponent<SkinnedMeshRenderer>();

        animator = GetComponent<Animator>();

        materialLookup = new Material[] { null, robotWhiteMaterial, robotBlackMaterial, robotRedMaterial, null };
        itemRendererLookup = new SkinnedMeshRenderer[] { clubMesh, humanMaskMesh, jetpackMesh, monocleMesh, umbrellaMesh };

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
            yield return null; // Wait for next frame
        }

        // Ensure final position is exact
        transform.position = endPosition;

        // Reset the flag so it's ready for next time
        isAnimationDone = false;
        Debug.Log("Robot moving done!");

    }
}
