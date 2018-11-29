using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using Quobject.SocketIoClientDotNet.Client;

public class GameManager : Singleton<GameManager>
{
    private enum SocketEventType {None, Init, Start, Stop, Logout};
    private SocketEventType socketEvent = SocketEventType.None;
    private Camera cam;
    private ChargingStationController currentChargingStation;

    private int nextScene;
    public Animator animator;
    private int queuedSceneChange = -1;

    private GUIController guiController;

    private SocketManager socketManager;

    private string simulatorUrl = "localhost:3000";
    void Start()
    {
        //StartCoroutine(PrintChargingStation());
        simulatorUrl = GetSimulatorURL();
        // guiController = GetComponent<GUIController>();
        // socketManager = GetComponent<SocketManager>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        print(simulatorUrl);
    }


    void Update()
    {
        if (guiController == null)
        {
            guiController = GetComponent<GUIController>();
            if (guiController != null)
            {
                guiController.Init();
            }
        }
        if (socketManager == null)
        {
            socketManager = GetComponent<SocketManager>();
            if (socketManager != null)
            {
                socketManager.Init();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            guiController.HandleEscapeEvent(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            InitiateTrip();
            //SwitchScene(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1 ? SceneManager.GetActiveScene().buildIndex + 1 : 0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            StartTrip();
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            EndTrip();
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            Logout();
        }
    }



    void LateUpdate()
    {
        // if (queuedSceneChange >= 0)
        // {
        //     SwitchScene(queuedSceneChange);
        //     queuedSceneChange = -1;
        // }
        switch (socketEvent)
        {
            case SocketEventType.None:
            {
                break;
            }
            case SocketEventType.Init:
            {
                HandleInitiateTrip();
                break;
            }
            case SocketEventType.Start:
            {
                HandleStartTrip();
                break;
            }
            case SocketEventType.Stop:
            {
                HandleEndTrip();
                break;
            }
            case SocketEventType.Logout:
            {
                HandleLogout();
                break;
            }
            default:
            {
                break;
            }
        }
        socketEvent = SocketEventType.None;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    #region Socket Control
    public void SetSimulatorURL(string url)
    {
        PlayerPrefs.SetString("SIMURL", url);
        socketManager.SetUrl(url);
    }

    public string GetSimulatorURL()
    {
        if (!PlayerPrefs.HasKey("SIMURL"))
        {
            PlayerPrefs.SetString("SIMURL", simulatorUrl);
        }
        return PlayerPrefs.GetString("SIMURL");
    }

    public void SetSocketState(bool connected)
    {
        if (guiController != null)
        {
            guiController.SetSocketState(connected);
        }
    }
    public void InitiateTrip()
    {
        socketEvent = SocketEventType.Init;
    }
    public void StartTrip()
    {
        socketEvent = SocketEventType.Start;
    }
    public void EndTrip()
    {
        socketEvent = SocketEventType.Stop;
    }

    public void Logout()
    {
        socketEvent = SocketEventType.Logout;
    }
    public void HandleInitiateTrip()
    {
        SwitchScene(1);
    }
    public void HandleStartTrip()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            VehicleControls player = GameObject.FindGameObjectWithTag("Player").GetComponent<VehicleControls>();
            CameraManager camManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>();
            if (player != null)
                player.SetControlType(VehicleControls.ControlType.Mouse);
            if (camManager != null)
                camManager.SetCameraMode(CameraManager.CameraMode.BirdsEye);
        }
    }
    public void HandleEndTrip()
    {
        //queuedSceneChange = 0;
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            VehicleControls player = GameObject.FindGameObjectWithTag("Player").GetComponent<VehicleControls>();
            CameraManager camManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>();
            if (player != null)
                player.SetControlType(VehicleControls.ControlType.Off);
            if (camManager != null)
                camManager.SetCameraMode(CameraManager.CameraMode.Orbit);
        }
    }

    public void HandleLogout()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SwitchScene(0);
        }
    }

    #endregion

    #region Charging
    public void SetChargingStation(ChargingStationController current = null)
    {
        currentChargingStation = current != null ? current : null;
    }

    public void ToggleCharge(bool state)
    {
        print(state);
        if (currentChargingStation == null)
        {
            print("Tried to toggle, car not on charging point");
        }
        else
        {
            print("Toggling charge to " + state);
            currentChargingStation.SetState(state ? ChargingStationController.StationState.Charging : ChargingStationController.StationState.Proximity);
        }
    }

    /**
    FOR DEBUG ONLY
    */
    IEnumerator PrintChargingStation()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
        }
    }
    #endregion

    #region Scene Management
    private void SwitchScene(int sceneIndex)
    {
        print(sceneIndex);
        nextScene = sceneIndex;
        FadeToScene(sceneIndex);
    }

    public void GoToStartScreen ()
    {
        SwitchScene(0);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("Loaded scene " + scene.name);
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        FadeIn();
    }

    public void FadeToScene(int sceneIndex)
    {
        animator.SetTrigger("FadeOutTrigger");
    }

    public void FadeIn()
    {
        animator.SetTrigger("FadeInTrigger");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(nextScene);
    }
    #endregion
}
