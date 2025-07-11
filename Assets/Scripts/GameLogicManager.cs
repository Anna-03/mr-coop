using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    int[,] maskRobot1 = new int[,]{
            {-1, -1,  1, -1, -1},
            {-1, -1,  1, -1, -1},
            {-1, -1, -1, -1,  1},
            {-1, -1, -1, -1,  1},
            { 1, -1, -1, -1, -1},
        };
    int[,] maskRobot2 = new int[,]{
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
        };
    int[,] maskRobot3 = new int[,]{
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
        };


    public NodeMCUManagerThread nodeMCUManager;
    public FXManager fxManager;

    int robotCount = 0;

    bool currentButtonState = false;
    bool previousButtonState = false;

    int[] cableStart = new int[] { -1, 0 };
    int[,] cable1 = {
            {-1, -1, 0},
            {-1, -1, 0}
        };
    int[,] cable2 = {
            {-1, -1, 0},
            {-1, -1, 0}
        };
    public StartWire wireStart = new StartWire();
    public Wire wire1 = new Wire();
    public Wire wire2 = new Wire();
    public bool allWiresValid = false;

    // Start is called before the first frame update
    void Start()
    {
        //currentValues = nodeMCUManager.statesArray;
        // TEST
        currentValues = new int[,]{
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
        };
        currentButtonState = nodeMCUManager.buttonState;
        // TODO: initialise robot masks with correct connections
    }

    // Update is called once per frame
    void Update()
    {
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
                    previousValues[row, column] = currentValues[row, column];
                }
            }
        }
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
                    previousValues[row, column] = currentValues[row, column];
                }
            }
        }
        // TESTING
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log(validateConnections());
        }
        // connect
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            currentValues = new int[,]{
            {-1, -1,  0, -1, -1},
            {-1, -1,  1, -1, -1},
            {-1, -1, -1, -1,  2},
            {-1, -1, -1, -1,  4},
            { 3, -1, -1, -1, -1},
            };
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            currentValues = new int[,]{
            {-1, -1, -1, -1, -1},
            {-1, -1, -1,  2, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1,  1, -1, -1, -1},
            };
        }
        // Disconnect
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            currentValues = new int[,]{
            {-1, -1, -1, -1, -1},
            {-1, -1, -1,  2, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1},
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
        if (currentButtonState == true && previousButtonState == false)
        {
            if (validateConnections())
            {

            }
        }

    }

    void disconnectSocket(int disconnectedRow, int disconnectedColumn, int id)
    {
        int otherRow = -1;
        int otherColumn = -1;
        bool otherEndIsConnected = false;

        switch (id)
        {
            case 0:
                wireStart.connectedRow = -1;
                break;
            case 1:
                wire1.connection1[0] = -1;
                wire1.connection1[1] = -1;
                break;
            case 2:
                wire1.connection2[0] = -1;
                wire1.connection2[1] = -1;
                break;
            case 3:
                wire2.connection1[0] = -1;
                wire2.connection1[1] = -1;
                break;
            case 4:
                wire2.connection2[0] = -1;
                wire2.connection2[1] = -1;
                break;

        }

        for (int row = 0; row < 5; row++)
        {
            for (int column = 0; column < 5; column++)
            {
                if (id == 0)
                {
                    fxManager.disconnectStartWire(column);
                    break;
                }
                else if (id % 2 == 0) // id is even
                {
                    if ((currentValues[row, column] == id - 1))
                    {
                        otherEndIsConnected = true;
                        otherRow = row;
                        otherColumn = column;
                        break;
                    }
                }
                else
                {
                    if ((currentValues[row, column] == id + 1))
                    {
                        otherEndIsConnected = true;
                        otherRow = row;
                        otherColumn = column;
                        break;
                    }
                }

            }
        }
        if (otherEndIsConnected)
        {
            fxManager.disconnectFullWire(disconnectedRow, disconnectedColumn, otherRow, otherColumn);
        }
        fxManager.changeSocketState(disconnectedRow, disconnectedColumn, 0);
    }

    void connectSocket(int connectedRow, int connectedColumn, int id)
    {
        int otherRow = -1;
        int otherColumn = -1;
        bool otherEndIsConnected = false;

        switch (id)
        {
            case 0:
                wireStart.connectedRow = connectedColumn;
                break;
            case 1:
                wire1.connection1[0] = connectedRow;
                wire1.connection1[1] = connectedColumn;
                break;
            case 2:
                wire1.connection2[0] = connectedRow;
                wire1.connection2[1] = connectedColumn;
                break;
            case 3:
                wire2.connection1[0] = connectedRow;
                wire2.connection1[1] = connectedColumn;
                break;
            case 4:
                wire2.connection2[0] = connectedRow;
                wire2.connection2[1] = connectedColumn;
                break;
        }

        for (int row = 0; row < 5; row++)
        {
            for (int column = 0; column < 5; column++)
            {
                if (id == 0)
                {
                    fxManager.connectStartWire(column);
                    break;
                }
                else if (id % 2 == 0) // id is even
                {
                    if ((currentValues[row, column] == id - 1))
                    {
                        otherEndIsConnected = true;
                        otherRow = row;
                        otherColumn = column;
                        break;
                    }
                }
                else
                {
                    if ((currentValues[row, column] == id + 1))
                    {
                        otherEndIsConnected = true;
                        otherRow = row;
                        otherColumn = column;
                        break;
                    }
                }

            }
        }
        if (otherEndIsConnected)
        {
            fxManager.connectFullWire(connectedRow, connectedColumn, otherRow, otherColumn);
        }
        fxManager.changeSocketState(connectedRow, connectedColumn, 1);
    }

    bool validateConnections()
    {
        int connection1 = -1;

        int[,] connection2 = {
            {-1, -1, -1},
            {-1, -1, -1}
        };
        int[,] connection3 = {
            {-1, -1, -1},
            {-1, -1, -1}
        };

        int[,] currentMask;
        int rowToCheck = -1;
        int correctConnections = 0;
        // TODO add robotCount
        switch (robotCount)
        {
            case 0:
                currentMask = maskRobot1;
                break;
            case 1:
                currentMask = maskRobot2;
                break;
            case 2:
                currentMask = maskRobot3;
                break;
            default:
                currentMask = maskRobot1;
                break;
        }

        for (int row = 0; row < 5; row++)
        {
            for (int column = 0; column < 5; column++)
            {
                int id = currentValues[row, column];

                if (id == 0) // id is 0 so it has to be the starting wire
                {
                    connection1 = column;
                    continue;
                }

                if (id != -1) // a normal wire is connected
                {
                    if (connection2[0, 0] == -1) // neither connection 2 nor 3 have an entry yet, so the first normal connection has been found
                    {
                        connection2[0, 0] = row;
                        connection2[0, 1] = column;
                        connection2[0, 2] = id;
                        continue;
                    }
                    // connection2 already has an entry
                    if (connection2[0, 0] != -1)
                    {
                        if (id % 2 == 0) // id is even
                        {
                            if (connection2[0, 2] == id - 1) // current id is the partner id for connection2
                            {
                                connection2[1, 0] = row;
                                connection2[1, 1] = column;
                                connection2[1, 2] = id;
                                continue;
                            }
                        }
                        else
                        {
                            if (connection2[0, 2] == id + 1) // current id is the partner id for connection2
                            {
                                connection2[1, 0] = row;
                                connection2[1, 1] = column;
                                connection2[1, 2] = id;
                                continue;
                            }
                        }
                    }
                    // connection 2 has an entry and current id is not the partner id for connection2

                    if (connection3[0, 0] == -1) // connection 3 has no entry 
                    {
                        connection3[0, 0] = row;
                        connection3[0, 1] = column;
                        connection3[0, 2] = id;
                        continue;
                    }

                    if (connection3[0, 0] != -1) // connection 3 has an entry check for partner id
                    {
                        if (id % 2 == 0) // id is even
                        {
                            if (connection3[0, 2] == id - 1) // current id is the partner id for connection2
                            {
                                connection3[1, 0] = row;
                                connection3[1, 1] = column;
                                connection3[1, 2] = id;
                                continue;
                            }
                        }
                        else
                        {
                            if (connection3[0, 2] == id + 1) // current id is the partner id for connection2
                            {
                                connection3[1, 0] = row;
                                connection3[1, 1] = column;
                                connection3[1, 2] = id;
                                continue;
                            }
                        }
                    }




                }
            }

        }
        Debug.Log($"Connection1:\n" + connection1);
        Debug.Log("Connection2:\n" + connection2[0, 0] + ", " + connection2[0, 1] + ", " + connection2[0, 2] + "     " + connection2[1, 0] + ", " + connection2[1, 1] + ", " + connection2[1, 2]);
        Debug.Log("Connection2:\n" + connection3[0, 0] + ", " + connection3[0, 1] + ", " + connection3[0, 2] + "     " + connection3[1, 0] + ", " + connection3[1, 1] + ", " + connection3[1, 2]);



        //check if connected sockets are one row apart each and not crossed
        wireStart.isValid = false;
        wire1.isValid = false;
        wire2.isValid = false;
        allWiresValid = false;

        //if (wireStart.connectedRow == -1)
        //{
        //    wireStart.isValid = false;
        //    allWiresValid = false;

        //}

        //if (Mathf.Abs(wire1.connection1[0] - wire1.connection2[0]) != 1)
        //{
        //    wire1.isValid = false;
        //    allWiresValid = false;
        //}

        //if (Mathf.Abs(wire2.connection1[0] - wire2.connection2[0]) != 1)
        //{
        //    wire2.isValid = false;
        //    allWiresValid = false;
        //}

        //if (wireStart.connectedRow == -1)
        //{
        //    wireStart.isValid = false;
        //    allWiresValid = false;
        //}
        //else
        //{

        //    if (currentMask[0, wireStart.connectedRow] != 1)
        //    {
        //        wireStart.isValid = false;
        //        allWiresValid = false;
        //    }
        //}

        if (wireStart.connectedRow != -1)
        {
            if (currentMask[0, wireStart.connectedRow] == 1)
            {
                wireStart.isValid = true;
            }
        }

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

        if (Mathf.Abs(wire1.connection1[0] - wire1.connection2[0]) != 1)
        {
            wire1.isValid = false;
        }

        if (Mathf.Abs(wire2.connection1[0] - wire2.connection2[0]) != 1)
        {
            wire2.isValid = false;
        }

        if (wireStart.isValid && wire1.isValid && wire2.isValid)
        {
            allWiresValid = true;
        }
        Debug.Log("wire1 c1 row: " + wire1.connection1[0] + ", wire1 c1 column: " + wire1.connection1[1]);
        Debug.Log("wire1 c2 row: " + wire1.connection2[0] + ", wire1 c2 column: " + wire1.connection2[1]);

        Debug.Log("wireStart is valid: ");
        Debug.Log(wireStart.isValid);

        Debug.Log("wire1 is valid: ");
        Debug.Log(wire1.isValid);

        Debug.Log("wire2 is valid: ");
        Debug.Log(wire2.isValid);

        //if (wire1.connection1[0] == -1 ||
        //    wire1.connection2[0] == -1
        //    )
        //{
        //    wire1.isValid = false;
        //    allWiresValid = false;
        //}
        //else
        //{
        //    if (currentMask[wire1.connection1[0], wire1.connection1[1]] != 1)
        //    {
        //        wire1.isValid = false;
        //        allWiresValid = false;
        //    }
        //    if (currentMask[wire1.connection2[0], wire1.connection2[1]] != 1)
        //    {
        //        wire1.isValid = false;
        //        allWiresValid = false;
        //    }
        //}

        //if (
        //    wire2.connection1[0] == -1 ||
        //    wire2.connection2[0] == -1
        //)
        //{
        //    wire2.isValid = false;
        //    allWiresValid = false;
        //}
        //else
        //{

        //    if (currentMask[wire2.connection1[0], wire2.connection1[1]] != 1)
        //    {
        //        wire2.isValid = false;
        //        allWiresValid = false;
        //    }
        //    if (currentMask[wire2.connection2[0], wire2.connection2[1]] != 1)
        //    {
        //        wire2.isValid = false;
        //        allWiresValid = false;
        //    }
        //}

        return allWiresValid;

    }
}

public class Wire
{
    public int[] connection1 = new int[] { -1, -1 };
    public int[] connection2 = new int[] { -1, -1 };
    public bool isValid = false;

}
public class StartWire
{
    public int connectedRow = -1;
    public bool isValid = false;
}