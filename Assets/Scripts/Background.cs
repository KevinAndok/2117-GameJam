using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform nearestBackground;
    public float nearestCoefficient;
    [Space]
    public Transform midBackground;
    public float midCoefficient;
    [Space]
    public Transform farBackground;
    public float farCoefficient;

    private void Update()
    {
        nearestBackground.position = Camera.main.transform.position / nearestCoefficient;

        midBackground.position = Camera.main.transform.position / midCoefficient;

        farBackground.position = Camera.main.transform.position / farCoefficient;
    }
}
