using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public GameObject[] stationObjectsFlat; // 10 elements = 2x5 Grid (only for testing, later 5x5)
    public int[,] rightStations = new int[2, 5];
    public int[,] previousValues = new int[2, 5];

    // will be set by rfids
    public int[,] currentValues = new int[2, 5];

    public Color greenColor = Color.green;
    public Color redColor = Color.red;
    public Color transparentColor = new Color(1, 1, 1, 0.1f); // default color
    void Start()
    {
        // initialize previousValues 
        for (int x = 0; x < 2; x++)
            for (int y = 0; y < 5; y++)
                previousValues[x, y] = -1;

        // Initialise rightStations
        for (int x = 0; x < 2; x++)
            for (int y = 0; y < 5; y++)
                previousValues[x, y] = -1;
        rightStations[0, 1] = 1;
        rightStations[1, 4] = 1;
    }


    void Update()
    {
        for (int x = 0; x < 2; x++) // will be 5 instead of 2 later
        {
            for (int y = 0; y < 5; y++)
            {
                int current = currentValues[x, y];
                int previous = previousValues[x, y];

                if (current != previous && current != -1)
                {
                    GameObject station = GetStationAt(x, y);
                    if (station != null)
                    {
                        Color targetColor = (rightStations[x,y] == 1) ? greenColor : redColor;
                        StartCoroutine(FlashStation(station, targetColor, 2f));
                    }
                }

                previousValues[x, y] = current;
            }
        }
    }
    IEnumerator FlashStation(GameObject station, Color color, float duration)
    {
        Renderer renderer = station.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = renderer.material;
            mat.color = color;

            yield return new WaitForSeconds(duration);

            mat.color = transparentColor;
        }
    }
    private GameObject GetStationAt(int x, int y)
    {
        return stationObjectsFlat[y * 2 + x];
    }
}
