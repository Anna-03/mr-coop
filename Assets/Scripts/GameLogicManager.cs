using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicManager : MonoBehaviour
{
    int[,] previousValues = new int[5, 5];
    int[,] currentValues = new int[5, 5];
    int[,] maskRobot1 = new int[5, 5];
    int[,] maskRobot2 = new int[5, 5];
    int[,] maskRobot3 = new int[5, 5];


    public NodeMCUManagerThread nodeMCUManager;
    public FXManager fxManager;

    int robotCount = 0;

    bool currentButtonState = false;
    bool previousButtonState = false;
    // Start is called before the first frame update
    void Start()
    {
        // initialize latest Values and robot masks with -1
        for (int row = 0; row < 5; row++)
        {
            for (int column = 0; column < 5; column++)
            {
                previousValues[row, column] = -1;
                maskRobot1[row, column] = -1;
                maskRobot2[row, column] = -1;
                maskRobot3[row, column] = -1;

            }
        }
        currentValues = nodeMCUManager.statesArray;
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
                    if (currentValues[row, column] == -1)
                    {
                        disconnectWire(row, column, previousValues[row, column]);
                    }
                    else
                    {
                        connectWire(row, column, currentValues[row, column]);

                    }
                }
            }
        }

        if(currentButtonState == true && previousButtonState == false)
        {
            if (connectionsAreCorrect())
            {

            }
        }
    }

    void disconnectWire(int disconnectedRow, int disconnectedColumn, int id)
    {
        int otherRow = -1;
        int otherColumn = -1;
        bool otherEndIsConnected = false;
        for (int row = 0; row < 5; row++)
        {
            for (int column = 0; column < 5; column++)
            {
                if(id == 0)
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
            fxManager.disconnectWire(disconnectedRow, disconnectedColumn, otherRow, otherColumn);
        }
        fxManager.changeSocketState(disconnectedRow, disconnectedColumn, 0);
    }

    void connectWire(int connectedRow, int connectedColumn, int id)
    {
        int otherRow = -1;
        int otherColumn = -1;
        bool otherEndIsConnected = false;
        for (int row = 0; row < 5; row++)
        {
            for (int column = 0; column < 5; column++)
            {
                if(id == 0)
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
            fxManager.connectWire(connectedRow, connectedColumn, otherRow, otherColumn);
        }
        fxManager.changeSocketState(connectedRow, connectedColumn, 1);
    }

    bool connectionsAreCorrect()
    {
        int[,] currentMask;
        int idToCheck;
        switch (robotCount)
        {
            case 0:
                currentMask = maskRobot1;
            case 1:
                currentMask = maskRobot2;
            case 2:
                currentMask = maskRobot3;
            default:
                break;
        }
        for (int row = 0; row < 5; row++)
        {
            for (int column = 0; column < 5; column++)
            {
                if(row == 0)
                {
                    if (currentValues[row, column] == 0  && currentMask[row, column] == 1)
                    {
                    
                    }
                }
                else
                {
                    if (currentValues[row, column] != -1 && currentMask[row, column] == 1)
                    {
                        idToCheck = currentValues[row, column];

                    }
                }

            }
        }
        return true;
    }
}
