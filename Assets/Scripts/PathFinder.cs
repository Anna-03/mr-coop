using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public NodeMCUManagerThread nodeMCUManagerThread;
    //public GameObject nodeMCUManagerObj;
    int[,] statesArray;
    public GameObject[] stationObjectsFlat; // 25 elements = 5x5 Grid
    public GameObject[] path = new GameObject[5];
    public bool pathFound;
    public bool buttonIsPressed;

    public int[,] maskRobot1 = new int[5, 5];
    GameObject[] path1 = new GameObject[5];
    public int[,] maskRobot2 = new int[5, 5];
    GameObject[] path2 = new GameObject[5];
    public int[,] maskRobot3 = new int[5, 5];
    GameObject[] path3 = new GameObject[5];

    void Start()
    {
        statesArray = nodeMCUManagerThread.statesArray;
        // initialize Robot Masks 
        for (int row = 0; row < 5; row++)
            for (int column = 0; column < 5; column++)
            {
                maskRobot1[row, column] = -1;
                maskRobot2[row, column] = -1;
                maskRobot3[row, column] = -1;
            }
        // Mask 1
        maskRobot1[0, 2] = 0;
        maskRobot1[1, 2] = 0;
        maskRobot1[2, 0] = 0;
        maskRobot1[3, 0] = 0;
        maskRobot1[4, 4] = 0;
        path1[0] = GetStationAt(0, 2);
        path1[1] = GetStationAt(1, 2);
        path1[2] = GetStationAt(2, 0);
        path1[0] = GetStationAt(3, 0);
        path1[0] = GetStationAt(4, 4);

        // Mask 2
        maskRobot2[0, 1] = 0;
        maskRobot2[1, 1] = 0;
        maskRobot2[2, 4] = 0;
        maskRobot2[3, 4] = 0;
        maskRobot2[4, 2] = 0;
        path2[0] = GetStationAt(0, 1);
        path2[1] = GetStationAt(1, 1);
        path2[2] = GetStationAt(2, 4);
        path2[3] = GetStationAt(3, 4);
        path2[4] = GetStationAt(4, 2);

        // Mask 3
        maskRobot3[0, 4] = 0;
        maskRobot3[1, 4] = 0;
        maskRobot3[2, 3] = 0;
        maskRobot3[3, 3] = 0;
        maskRobot3[4, 1] = 0;
        path3[0] = GetStationAt(0, 4);
        path3[1] = GetStationAt(1, 4);
        path3[2] = GetStationAt(2, 3);
        path3[3] = GetStationAt(3, 3);
        path3[4] = GetStationAt(4, 1);

        buttonIsPressed = false;
        pathFound = false;
    }


    void Update()
    {
        if (buttonIsPressed)
        {
            bool validConnection = true;

            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    int val1 = maskRobot1[row, column];
                    int val2 = statesArray[row, column];

                    if (val1 == 0)
                    {
                        if (val2 == -1)
                        {
                            validConnection = false;
                        }
                    }
                    else if (val1 == -1)
                    {
                        if (val2 != -1)
                        {
                            validConnection = false;
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Warnung bei ({row},{column}): Unerwarteter Wert in array1: {val1}");
                    }
                }
            }

            if (validConnection)
            {
                path = path1;
                pathFound = true;
            }
            else
            {
                Debug.Log("Invalid Connection");
            }
        }
    }
    
    private GameObject GetStationAt(int row, int column)
    {
        return stationObjectsFlat[column * 5 + row];
    }
}
