using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;
using UnityEngine.Networking;
public class SocketManager : MonoBehaviour
{
    /**
    Socket IO framework from https://github.com/floatinghotpot/socket.io-unity
    */
    class BatteryInfo
    {
        public float currentCharge;
        public bool isCharging;
    }
    public enum HttpVerbs { GET, PUT, POST };

    private bool initialized = false;
    private Socket socket;
    private GameManager gameManager;
    private BatteryInfo batteryInfo;
    public string serverUrl = "localhosts:3000";

    private bool connected = false;
    private bool isConnecting = false;

    private Coroutine connectRoutine;
    void Start()
    {

    }

    void Update()
    {
        if (initialized)
        {
            if (!connected && !isConnecting)
            {
                RefreshConnection();
            }
            if (connected && isConnecting)
            {
                gameManager.SetSocketState(true);
            }
        }

    }

    public void Init()
    {
        if (gameManager == null)
        {
            gameManager = GetComponent<GameManager>();
            serverUrl = gameManager.GetSimulatorURL();
        }
        initialized = true;
        RefreshConnection();
    }
    public void SetUrl(string url)
    {
        serverUrl = url;

        RefreshConnection();
    }

    public void RefreshConnection()
    {
        print("INIT" + gameManager);
        if (connected || isConnecting)
        {
            if(connectRoutine != null)
            {
            StopCoroutine(connectRoutine);
            }
            connectRoutine = null;
            socket.Disconnect();
            socket.Close();
            connected = false;
            socket = null;
        }

        SetupConnection();
        connectRoutine = StartCoroutine(SocketConnect());
    }

    private void SetupConnection()
    {
        if (socket != null)
        {
            socket.Disconnect();
            socket.Close();
            socket = null;
        }
        IO.Options options = new IO.Options();
        options.ReconnectionAttempts = 100;
        options.ReconnectionDelay = 1000;
        options.Reconnection = true;
        options.Timeout = (long)2000;
        options.AutoConnect = false;

        socket = IO.Socket("ws://" + serverUrl, options);
        socket.On(Socket.EVENT_CONNECT, (sock) =>
        {
            print(socket.Io().EngineSocket.Id);
            print("#SOCKET CONNECTED");
            connected = true;
            socket.Emit("reportingAsCar");
        });

        socket.On(Socket.EVENT_CONNECT_ERROR, () =>
        {
            print("#SOCKET ERROR");
            connected = false;
        });

        socket.On(Socket.EVENT_CONNECT_TIMEOUT, () =>
        {
            print("#SOCKET TIMEOUT");
            connected = false;
        });

        socket.On(Socket.EVENT_DISCONNECT, () =>
        {
            print("#SOCKET DISCONNECT");
            connected = false;
        });

        socket.On(Socket.EVENT_RECONNECT, () =>
        {
            print("#SOCKET RECONNECT");
            connected = true;
        });

        socket.On(Socket.EVENT_RECONNECTING, () =>
        {
            print("#SOCKET RECONNECTING...");
            connected = false;
        });

        socket.On("toggleBatteryCharge", (data) =>
        {
            print("#toggleBatteryCharge");
            gameManager.ToggleCharge((bool)data);
            print(data);
        });

        socket.On("batteryInfo", (data) =>
        {
            batteryInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<BatteryInfo>((string)data);
            print(batteryInfo.currentCharge);
            print(data);
        });

        socket.On("initiateTrip", () =>
        {
            print("#initiateTrip");
            gameManager.InitiateTrip();
        });

        socket.On("sessionStart", (data) =>
        {
            print("#sessionStart");
            gameManager.StartTrip();
        });

        socket.On("sessionEnd", (data) =>
        {
            print("#sessionEnd");
            gameManager.EndTrip();
        });

        socket.On("logout", () => {
            print("#logout");
            gameManager.Logout();
        });
    }

    IEnumerator SocketConnect()
    {
        isConnecting = true;
        while (!connected)
        {
            gameManager.SetSocketState(false);
            print("# Attempting to connect ...");
            socket.Connect();
            yield return new WaitForSeconds(5);
        }
        gameManager.SetSocketState(true);
        isConnecting = false;
        connectRoutine = null;
        print("# Successfully connected to ws://" + serverUrl);
    }

    void OnDestroy()
    {
        if (socket != null)
        {
            socket.Close();
        }
    }

    public void TriggerTag(string uid)
    {
        StartCoroutine(PutTag(uid));
    }

    IEnumerator PutTag(string uid)
    {
        UnityWebRequest www = UnityWebRequest.Put("http://" + serverUrl + "/tags/" + uid, "null");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            print(www.error);
        }
        else
        {
            print("[Tag] Successfully sent tag: " + uid);
        }
    }

    IEnumerator StopBatteryCharge()
    {
        UnityWebRequest www = UnityWebRequest.Put("http://" + serverUrl + "/battery/off", "null");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            print(www.error);
        }
        else
        {
            print("[Tag] Successfully stopped charging simulator");
        }
    }
    // void Update () {
    //     if(Input.GetKeyDown(KeyCode.Space)) {
    //         StartCoroutine(SendBatteryStatus());
    //     }
    //     if(Input.GetKeyDown(KeyCode.Return)) {
    //         BatteryInfo data = new BatteryInfo();
    //         data.currentCharge = "0.2";
    //         data.isCharging = false;
    //         string json = JsonUtility.ToJson(data);

    //         socket.Emit("batteryInfo", json);
    //     }
    // }

    // IEnumerator SendBatteryStatus() {
    //     BatteryInfo data = new BatteryInfo();
    //     data.currentCharge = "0.2";
    //     data.isCharging = false;

    //     string json = JsonUtility.ToJson(data);

    //     print(json);

    //     UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/battery", json);
    //     yield return www.SendWebRequest();

    //     if(www.isNetworkError || www.isHttpError)
    // 	{
    // 		print(www.error);
    // 	}
    // 	else
    // 	{
    // 		print("Successfully sent battery status: ");
    // 	}
    // }
}
