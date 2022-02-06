using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    private Light light;
    private float value;

    private void Start()
    {
        light = GetComponent<Light>();
        value = 0;
    }

    void Update()
    {
        value += 2 * Time.deltaTime;
        value = value % (2 * Mathf.PI);
        light.intensity = 2 + Mathf.Sin(value)/2;
    }
}