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


    [SerializeField]
    float startHeight = 2.2f;
    float progress = 0f;
    int atWire = 0;
    Vector3 startPosition;
    Vector3 endPosition;
    WireStart wireStart;
    Wire wire1;
    Wire wire2;
    int row3Column = -1;
    int row4Column = -1;
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
        sockets = gameLogicManager.sockets;

        materialLookup = new Material[] { null, null, robotBlackMaterial, robotWhiteMaterial, robotRedMaterial };
        itemRendererLookup = new SkinnedMeshRenderer[] { clubMesh, humanMaskMesh, jetpackMesh, monocleMesh, umbrellaMesh };

    }

    // Update is called once per frame
    void Update()
    {
        // progress = animator.GetFloat("progress");
        // transform.position = Vector3.Lerp(startPosition, endPosition, progress);
    }

    public void StartBuildingRobot(WireStart wS, Wire w1, Wire w2)
    {
        wireStart = wS;
        wire1 = w1;
        wire2 = w2;
        TransformRobot();
    }

    public void TransformRobot()
    {
        switch (atWire)
        {
            case 0: // from start to row 0
                transform.position = pillarsObj.transform.position + new Vector3(0f, 2.2f, 0f);
                startPosition = transform.position;
                endPosition = sockets[0, wireStart.connectedColumn].transform.position;
                ingotMesh.enabled = true;
                break;
            case 1: // from row 1 to row 2
                if (wire1.connection1[0] < wire1.connection2[0]) // make sure robot moves down the line
                {
                    startPosition = sockets[wire1.connection1[0], wire1.connection1[1]].transform.position;
                    endPosition = sockets[wire1.connection2[0], wire1.connection2[1]].transform.position;
                }
                else
                {
                    startPosition = sockets[wire1.connection2[0], wire1.connection2[1]].transform.position;
                    endPosition = sockets[wire1.connection1[0], wire1.connection1[1]].transform.position;
                }
                ingotMesh.enabled = false;
                bodyMesh.enabled = true;
                break;
            case 2: // from row 3 to row 4
                if (wire2.connection1[0] < wire2.connection2[0]) // make sure robot moves down the line
                {
                    startPosition = sockets[wire2.connection1[0], wire2.connection1[1]].transform.position;
                    endPosition = sockets[wire2.connection2[0], wire2.connection2[1]].transform.position;
                    row3Column = wire2.connection1[1];
                    row4Column = wire2.connection2[1];
                }
                else
                {
                    startPosition = sockets[wire2.connection2[0], wire2.connection2[1]].transform.position;
                    endPosition = sockets[wire2.connection1[0], wire2.connection1[1]].transform.position;
                    row3Column = wire2.connection2[1];
                    row4Column = wire2.connection1[1];
                }
                bodyMesh.material = materialLookup[row3Column];
                break;
            case 3:
                startPosition = sockets[4, row4Column].transform.position;
                endPosition = startPosition + new Vector3(0, -1, 0);
                itemRendererLookup[row4Column].enabled = true;
                itemRendererLookup[row4Column].material = bodyMesh.material;
                break;
        }
        animator.Play("RobotEaseToOne", 0, 0f);
    }
    public void ProgressFinished()
    {
        isAnimationDone = true;
        // atWire++;
        // if (atWire <= 3)
        // {
        //     TransformRobot();
        // }
    }
    public IEnumerator MoveTo(Vector3 startPosition, Vector3 endPosition)
    {
        // Start the animation from the beginning
        animator.Play("RobotEaseToOne", 0, 0f);

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
