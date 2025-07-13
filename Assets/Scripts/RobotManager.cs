using System;
using System.Collections;
using System.Collections.Generic;
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

    MeshRenderer ingotMesh;
    SkinnedMeshRenderer bodyMesh;
    SkinnedMeshRenderer clubMesh;
    SkinnedMeshRenderer humanMaskMesh;
    SkinnedMeshRenderer jetpackMesh;
    SkinnedMeshRenderer monocleMesh;
    SkinnedMeshRenderer umbrellaMesh;

    public Material ingotMaterial;
    public Material robotBlackMaterial;
    public Material robotWhiteMaterial;
    public Material robotRedMaterial;

    public GameObject pillarsObj;
    Animator animator;


    [SerializeField]
    float startHeight = 2.2f;
    float progress = 0f;


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
        
    }

    // Update is called once per frame
    void Update()
    {
        progress = animator.GetFloat("progress");
    }

    public void StartBuildingRobot(WireStart wireStart, Wire wire1, Wire wire2)
    {
        animator.Play("RobotEaseToOne", 0, 0f);
        transform.position = pillarsObj.transform.position + new Vector3(0f, 2.2f, 0f);
        
    }

    public void ProgressFinished()
    {
        Debug.Log("animation done!");
    }
}
