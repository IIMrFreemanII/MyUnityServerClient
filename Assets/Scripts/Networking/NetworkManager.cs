using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        UnityThread.initUnityThread();
    }

    public static void ConnectToServer(string host, int port)
    {
        ClientHandleData.InitializePackets();
        ClientTcp.InitializingNetworking(host, port);
    }

    private void OnApplicationQuit()
    {
        ClientTcp.Disconnect();
    }
}
