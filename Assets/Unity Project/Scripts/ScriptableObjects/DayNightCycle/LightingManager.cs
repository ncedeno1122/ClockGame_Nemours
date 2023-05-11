using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset LightingPreset;
    [SerializeField, Range(0f, 24f)] private float TimeOfDay;

    private void OnValidate()
    {
        if (DirectionalLight != null) return;

        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                }
            }
        }
    }

    private void Update()
    {
        if (LightingPreset == null) return;

        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 24;
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercentage)
    {
        RenderSettings.ambientLight = LightingPreset.AmbientColor.Evaluate(timePercentage);
        RenderSettings.fogColor = LightingPreset.FogColor.Evaluate(timePercentage);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = LightingPreset.DirectionalColor.Evaluate(timePercentage);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3(
                (timePercentage * 360f) - 90f,
                170f,
                0f));
        }
    }
}
