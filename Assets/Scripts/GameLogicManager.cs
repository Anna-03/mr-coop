using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Oculus;


public class GameLogicManager : MonoBehaviour
{
    int[,] previousValues = new int[,]{
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
        };
    int[,] currentValues = new int[,]{
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
        };

    // TODO: initialise robot masks with correct connections
    int[,] maskRobot1 = new int[,]{
            {-1,  1, -1, -1, -1},
            {-1,  1, -1, -1, -1},
            {-1, -1, -1,  1, -1},
            {-1, -1, -1,  1, -1},
            { 1, -1, -1, -1, -1},
        };
    int[,] maskRobot2 = new int[,]{
            {-1, -1, -1,  1, -1},
            {-1, -1, -1,  1, -1},
            {-1,  1, -1, -1, -1},
            {-1,  1, -1, -1, -1},
            {-1, -1, -1, -1,  1},
        };
    // int[,] maskRobot3 = new int[,]{
    //         {-1,  1, -1, -1, -1},
    //         {-1,  1, -1, -1, -1},
    //         {-1, -1, -1,  1, -1},
    //         {-1, -1, -1,  1, -1},
    //         { 1, -1, -1, -1, -1},
    //     };


    public NodeMCUManagerThread nodeMCUManager;
    public FXManager fxManager;
    public RobotManager robotManager;
    public SoundManager soundManager;
    AudioSource audioSourceClick;

    public int robotCount = 0;
    public bool isBuilding = false;

    bool currentButtonState = false;
    bool previousButtonState = false;
    public bool gameIsFinished = false;


    public WireStart wireStart = new WireStart();
    public Wire wire1 = new Wire();
    public Wire wire2 = new Wire();
    public bool allWiresValid = false;

    public GameObject[] pillars;
    public GameObject[] unorderedPillars;
    public GameObject[] stationsFlat;
    public GameObject[] unorderedStations;
    public GameObject[] socketsFlat;
    public GameObject[] unorderedSockets;
    public GameObject[,] sockets = new GameObject[5, 5];
    public GameObject[,] stations = new GameObject[3, 5];

    // Start is called before the first frame update
    void Start()
    {
        unorderedPillars = GameObject.FindGameObjectsWithTag("Pillar");
        pillars = unorderedPillars.OrderBy(pi => pi.name).ToArray();
        unorderedStations = GameObject.FindGameObjectsWithTag("Station");
        stationsFlat = unorderedStations.OrderBy(st => st.name).ToArray();
        unorderedSockets = GameObject.FindGameObjectsWithTag("Socket");
        socketsFlat = unorderedSockets.OrderBy(so => so.name).ToArray();

        audioSourceClick = soundManager.audioSourceClick;


        for (int column = 0; column < 5; column++)
        {
            for (int row = 0; row < 5; row++)
            {
                int index = column * 5 + row; // Column-major order
                sockets[row, column] = socketsFlat[index];
                fxManager.changeSocketState(row, column, 0);
            }
        }
        for (int column = 0; column < 5; column++)
        {
            for (int row = 0; row < 3; row++)
            {
                int index = column * 3 + row; // Column-major order
                stations[row, column] = stationsFlat[index];
            }
        }

        currentValues = nodeMCUManager.statesArray;
        // TEST
        currentValues = new int[,]{
             {-1, -1, -1, -1, -1},
             {-1, -1, -1, -1, -1},
             {-1, -1, -1, -1, -1},
             {-1, -1, -1, -1, -1},
             {-1, -1, -1, -1, -1},
         };
        currentButtonState = nodeMCUManager.buttonState;
    }

    // Update is called once per frame
    void Update()
    {
        currentButtonState = nodeMCUManager.buttonState;
        currentValues = nodeMCUManager.statesArray;



        for (int row = 0; row < 5; row++)
        {
            for (int column = 0; column < 5; column++)
            {
                if (previousValues[row, column] != currentValues[row, column])
                {
                    if (currentValues[row, column] == -1)
                    {
                        disconnectSocket(row, column, previousValues[row, column]);
                    }
                }
            }
        }
        for (int row = 0; row < 5; row++)
        {
            for (int column = 0; column < 5; column++)
            {
                if (previousValues[row, column] != currentValues[row, column])
                {
                    if (currentValues[row, column] != -1)
                    {
                        connectSocket(row, column, currentValues[row, column]);
                    }
                }
                previousValues[row, column] = currentValues[row, column];
            }
        }


        // TESTING
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log(validateConnections());
        }
        // connect
        if (Keyboard.current.digit1Key.wasPressedThisFrame || OVRInput.GetDown(OVRInput.Button.One))
        {
            currentValues = new int[,]{
            {-1,  0, -1, -1, -1},
            {-1,  2, -1, -1, -1},
            {-1, -1, -1,  1, -1},
            {-1, -1, -1,  4, -1},
            { 3, -1, -1, -1, -1},
            };

        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame || OVRInput.GetDown(OVRInput.Button.Two))
        {
            currentValues = new int[,]{
            { 0, -1, -1, -1, -1},
            { 3, -1, -1, -1, -1},
            {-1, -1,  4, -1, -1},
            {-1, -1,  1, -1, -1},
            {-1, -1, -1, -1,  2},
            };

        }
        // Disconnect
        if (Keyboard.current.digit3Key.wasPressedThisFrame || OVRInput.GetDown(OVRInput.Button.Three))
        {
            currentValues = new int[,]{
            {-1, -1, -1, -1,  0},
            {-1, -1, -1, -1,  2},
            {-1, -1, -1,  1, -1},
            {-1, -1, -1,  3, -1},
            {-1, -1,  4, -1, -1},
            };

        }

        // Reconnect
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            currentValues = new int[,]{
            {-1, -1,  0, -1, -1},
            {-1, -1, -1,  2, -1},
            {-1, -1, -1,  1, -1},
            {-1, -1, -1, -1,  3},
            { 4, -1, -1, -1, -1},
            };

        }
        if (Keyboard.current.enterKey.wasPressedThisFrame || OVRInput.GetDown(OVRInput.Button.Four))
        {
            currentButtonState = true;
        }
        // END TESTING
        if (currentButtonState && !previousButtonState)
        {
            if (gameIsFinished)
            {
                // TODO: change for end game 
                fxManager.ResetRobots();
                gameIsFinished = false;
            }
            else if (validateConnections() && !isBuilding)
            {
                // start robot
                // robotManager.StartBuildingRobot(wireStart, wire1, wire2);
                StartCoroutine(fxManager.runBuildSequence());
                isBuilding = true;
            }
            currentButtonState = false;
        }
        previousButtonState = currentButtonState;
    }

    void disconnectSocket(int disconnectedRow, int disconnectedColumn, int id)
    {
        switch (id)
        {
            case 0:
                wireStart.connectedColumn = -1;
                fxManager.disconnectStartWire(disconnectedColumn);

                break;
            case 1:
                wire1.connection1[0] = -1;
                wire1.connection1[1] = -1;
                if (wire1.connection2[0] != -1)
                {
                    fxManager.disconnectFullWire(disconnectedRow, disconnectedColumn, wire1.connection2[0], wire1.connection2[1], id);
                }
                break;
            case 2:
                wire1.connection2[0] = -1;
                wire1.connection2[1] = -1;
                if (wire1.connection1[0] != -1)
                {
                    fxManager.disconnectFullWire(disconnectedRow, disconnectedColumn, wire1.connection1[0], wire1.connection1[1], id);
                }
                break;
            case 3:
                wire2.connection1[0] = -1;
                wire2.connection1[1] = -1;
                if (wire2.connection2[0] != -1)
                {
                    fxManager.disconnectFullWire(disconnectedRow, disconnectedColumn, wire2.connection2[0], wire2.connection2[1], id);
                }
                break;
            case 4:
                wire2.connection2[0] = -1;
                wire2.connection2[1] = -1;
                if (wire2.connection1[0] != -1)
                {
                    fxManager.disconnectFullWire(disconnectedRow, disconnectedColumn, wire2.connection1[0], wire2.connection1[1], id);
                }
                break;
        }
        fxManager.changeSocketState(disconnectedRow, disconnectedColumn, 0);
    }

    void connectSocket(int connectedRow, int connectedColumn, int id)
    {

        switch (id)
        {
            case 0:
                wireStart.connectedColumn = connectedColumn;
                fxManager.connectStartWire(connectedColumn);

                break;
            case 1:
                wire1.connection1[0] = connectedRow;
                wire1.connection1[1] = connectedColumn;
                if (wire1.connection2[0] != -1)
                {
                    fxManager.connectFullWire(connectedRow, connectedColumn, wire1.connection2[0], wire1.connection2[1], id);
                }
                break;
            case 2:
                wire1.connection2[0] = connectedRow;
                wire1.connection2[1] = connectedColumn;
                if (wire1.connection1[0] != -1)
                {
                    fxManager.connectFullWire(connectedRow, connectedColumn, wire1.connection1[0], wire1.connection1[1], id);
                }
                break;
            case 3:
                wire2.connection1[0] = connectedRow;
                wire2.connection1[1] = connectedColumn;
                if (wire2.connection2[0] != -1)
                {
                    fxManager.connectFullWire(connectedRow, connectedColumn, wire2.connection2[0], wire2.connection2[1], id);
                }
                break;
            case 4:
                wire2.connection2[0] = connectedRow;
                wire2.connection2[1] = connectedColumn;
                if (wire2.connection1[0] != -1)
                {
                    fxManager.connectFullWire(connectedRow, connectedColumn, wire2.connection1[0], wire2.connection1[1], id);
                }
                break;
        }
        fxManager.changeSocketState(connectedRow, connectedColumn, 1);
        audioSourceClick.PlayOneShot(soundManager.click);
    }

    bool validateConnections()
    {
        int[,] currentMask;
        switch (robotCount)
        {
            case 0:
                currentMask = maskRobot1;
                break;
            case 1:
                currentMask = maskRobot2;
                break;
            // case 2:
            //     currentMask = maskRobot3;
            //     break;
            default:
                currentMask = maskRobot1;
                break;
        }

        wireStart.isValid = false;
        wire1.isValid = false;
        wire2.isValid = false;
        allWiresValid = false;

        // check if wireStart has is connected and is valid
        if (wireStart.connectedColumn != -1)
        {
            if (currentMask[0, wireStart.connectedColumn] == 1)
            {
                wireStart.isValid = true;
            }
        }
        // check if wires are connected and a potential correct position
        if (wire1.connection1[0] != -1 && wire1.connection2[0] != -1)
        {
            if (currentMask[wire1.connection1[0], wire1.connection1[1]] == 1 && currentMask[wire1.connection2[0], wire1.connection2[1]] == 1)
            {
                wire1.isValid = true;
            }
        }

        if (wire2.connection1[0] != -1 && wire2.connection2[0] != -1)
        {
            if (currentMask[wire2.connection1[0], wire2.connection1[1]] == 1 && currentMask[wire2.connection2[0], wire2.connection2[1]] == 1)
            {
                wire2.isValid = true;
            }
        }

        // check if wires are one row apart to make sure they are valid
        if (Mathf.Abs(wire1.connection1[0] - wire1.connection2[0]) != 1)
        {
            wire1.isValid = false;
        }

        if (Mathf.Abs(wire2.connection1[0] - wire2.connection2[0]) != 1)
        {
            wire2.isValid = false;
        }

        // check if all wires are valid
        if (wireStart.isValid && wire1.isValid && wire2.isValid)
        {
            allWiresValid = true;
        }

        // change states of the sockets a wire is connected to
        if (wireStart.isValid)
        {
            fxManager.changeSocketState(0, wireStart.connectedColumn, 2);
        }
        else if (wireStart.connectedColumn != -1)
        {
            fxManager.changeSocketState(0, wireStart.connectedColumn, 3);
        }
        else if (wire1.connection1[0] != -1 && wire1.connection2[0] != -1)
        {
        }
        if (wire1.isValid)
        {
            fxManager.changeSocketState(wire1.connection1[0], wire1.connection1[1], 2);
            fxManager.changeSocketState(wire1.connection2[0], wire1.connection2[1], 2);
        }
        else
        {
            if (wire1.connection1[0] != -1)
            {
                fxManager.changeSocketState(wire1.connection1[0], wire1.connection1[1], 3);
            }
            if (wire1.connection2[0] != -1)
            {
                fxManager.changeSocketState(wire1.connection2[0], wire1.connection2[1], 3);
            }
        }

        if (wire2.isValid)
        {
            fxManager.changeSocketState(wire2.connection1[0], wire2.connection1[1], 2);
            fxManager.changeSocketState(wire2.connection2[0], wire2.connection2[1], 2);
        }
        else
        {
            if (wire2.connection1[0] != -1)
            {
                fxManager.changeSocketState(wire2.connection1[0], wire2.connection1[1], 3);
            }
            if (wire2.connection2[0] != -1)
            {
                fxManager.changeSocketState(wire2.connection2[0], wire2.connection2[1], 3);
            }
        }

        Debug.Log(
            "wireS: " + wireStart.connectedColumn +
            "\nwire1: " + wire1.connection1[0] + ", " + wire1.connection1[1] + "   " + wire1.connection2[0] + ", " + wire1.connection2[1] +
            "\nwire2: " + wire2.connection1[0] + ", " + wire2.connection1[1] + "   " + wire2.connection2[0] + ", " + wire2.connection2[1]
            );

        Debug.Log("wireStart is valid: ");
        Debug.Log(wireStart.isValid);

        Debug.Log("wire1 is valid: ");
        Debug.Log(wire1.isValid);

        Debug.Log("wire2 is valid: ");
        Debug.Log(wire2.isValid);

        Debug.Log("all wires valid:: ");
        Debug.Log(allWiresValid);

        if (allWiresValid)
        {
            soundManager.PlayVoice("correct");
        }
        else
        {
            soundManager.PlayVoice("incorrect");
        }
        return allWiresValid;

    }
}

public class Wire
{
    public int[] connection1 = new int[] { -1, -1 };
    public int[] connection2 = new int[] { -1, -1 };
    public bool isValid = false;

}
public class WireStart
{
    public int connectedColumn = -1;
    public bool isValid = false;
}