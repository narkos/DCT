using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingStationController : MonoBehaviour
{
    public enum StationState { Idle, Proximity, Charging, Off }
    public string stationName;
    public ParticleSystem psIdle;
    public ParticleSystem psCharging; // Might want an array
    public GameObject stationMesh;
    private Material meshMaterial;

    public Color idleColor = Color.blue;
    public float idleRate = 0.5f;
    public Color proximityColor = Color.green;
    public float proximityRate = 1.0f;
    public Color chargingColor = Color.red;
    public float chargingRate = 3.0f;

    public float glowIntensity = 2.0f;
    public float minGlowIntensity = 0.1f;

    private Color currentColor;
    private float currentRate;
    private bool isActive = false;
    private bool stateHasChanged = false;
    private StationState currentState = StationState.Idle;
    private StationState nextState;
    void Start()
    {
        currentColor = idleColor;
        currentRate = idleRate;
        isActive = true;
        if (stationMesh != null)
        {
            meshMaterial = stationMesh.GetComponent<Renderer>().material;
        }
        StartCoroutine(AnimateEmission(0.1f));
    }

    public void SetState(StationState state)
    {
        stateHasChanged = true;
        nextState = state;
    }

    // Update is called once per frame
    void Update()
    {
        if (stateHasChanged)
        {
            if (currentState == StationState.Charging) {
                
            }
            SwitchState(nextState);
            stateHasChanged = false;
        }
        // Go to idle
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ToggleIdleSystems(true);
            ToggleChargingSystems(false);
        }
        // Go to charging
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ToggleIdleSystems(false);
            ToggleChargingSystems(true);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            SwitchState(StationState.Idle);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            SwitchState(StationState.Proximity);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            SwitchState(StationState.Charging);
        }
    }

    public void SwitchState(StationState state)
    {
        switch (state)
        {
            case StationState.Idle:
                {
                    currentColor = idleColor;
                    currentRate = idleRate;
                    break;
                }
            case StationState.Proximity:
                {
                    currentColor = proximityColor;
                    currentRate = proximityRate;
                    break;
                }
            case StationState.Charging:
                {
                    currentColor = chargingColor;
                    currentRate = chargingRate;
                    break;
                }
            case StationState.Off:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }
        ToggleIdleSystems(state == StationState.Idle);
        ToggleChargingSystems(state == StationState.Charging);
        currentState = state;
    }

    void ToggleIdleSystems(bool state)
    {
        if (psIdle != null)
        {
            if (state)
            {
                psIdle.Play(true);
                return;
            }
            psIdle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    public void ToggleChargingSystems(bool state)
    {
        if (psCharging != null)
        {
            if (state)
            {
                psCharging.Play(true);
                return;
            }
            psCharging.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    IEnumerator AnimateEmission(float updateTime)
    {
        float currentIntensity = 0.0f;
        meshMaterial.EnableKeyword("_EMISSION");
        meshMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        while (isActive)
        {
            currentIntensity = Mathf.Max(Mathf.PingPong(Time.time * currentRate, 1.0f) * glowIntensity, minGlowIntensity);
            meshMaterial.SetColor("_EmissionColor", currentColor * currentIntensity);
            yield return new WaitForEndOfFrame();
        }
    }
}
