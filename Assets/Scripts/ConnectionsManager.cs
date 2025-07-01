using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionsManager : MonoBehaviour
{

    public GameObject[] stationObjectsFlat;
    public GameObject nodeMCUManagerObj;

    int[,] statesArray;
    public int[,] correctStations = new int[5, 2];
    // Start is called before the first frame update
    void Start()
    {
        statesArray = nodeMCUManagerObj.GetComponent<NodeMCUManagerThread>().statesArray;

        // Initialise correctStations
        for (int row = 0; row < correctStations.GetLength(0); row++)
        {
            for (int column = 0; column < correctStations.GetLength(1); column++)
            {
                correctStations[row, column] = 0;
            }
        }

        correctStations[1, 0] = 1;
        correctStations[4, 1] = 1;
    }

    // Update is called once per frame
    void Update()
    {
        for (int row = 0; row < correctStations.GetLength(0); row++) // will be 5 instead of 2 later
        {
            for (int column = 0; column < correctStations.GetLength(1); column++)
            {
                int current = statesArray[row, column];
                // GameObject station;
                GameObject station = stationObjectsFlat[0];
                if (column == 0)
                {
                    if (row == 0)
                    {
                        station = stationObjectsFlat[0];
                    }
                    if (row == 1)
                    {
                        station = stationObjectsFlat[0];
                    }
                    if (row == 2)
                    {
                        station = stationObjectsFlat[1];
                    }
                    if (row == 3)
                    {
                        station = stationObjectsFlat[1];
                    }
                    if (row == 4)
                    {
                        station = stationObjectsFlat[2];
                    }
                }
                if (column == 1)
                {
                    if (row == 0)
                    {
                        station = stationObjectsFlat[3];
                    }
                    if (row == 1)
                    {
                        station = stationObjectsFlat[3];
                    }
                    if (row == 2)
                    {
                        station = stationObjectsFlat[4];
                    }
                    if (row == 3)
                    {
                        station = stationObjectsFlat[4];
                    }
                    if (row == 4)
                    {
                        station = stationObjectsFlat[5];
                    }
                }

                Color targetColor;

                if (current >= 0)
                {
                    targetColor = (correctStations[row, column] == 1) ? Color.green : Color.red;
                    station.transform.localScale = new Vector3(2.8f, 0.08f, 2.8f);
                }
                else
                {
                    targetColor = Color.white;
                    station.transform.localScale = new Vector3(1.4f, 0.04f, 1.4f);
                }

                Renderer renderer = station.GetComponent<Renderer>();
                Material mat = renderer.material;
                mat.color = targetColor;
            }
        }
    }
}
