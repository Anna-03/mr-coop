using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public PathFinder pathFinder;
    bool buttonPressed;
    public NodeMCUManagerThread nodeMCUManagerThread;
    int[,] statesArray;
    public GameObject[] testPath;
    void Start()
    {
        buttonPressed = pathFinder.buttonIsPressed;
        statesArray = nodeMCUManagerThread.statesArray;

        pathFinder.path = testPath;
        pathFinder.pathFound = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
