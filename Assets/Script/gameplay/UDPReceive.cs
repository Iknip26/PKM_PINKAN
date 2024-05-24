using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{

    Thread receiveThread;
    UdpClient client; 
    public int port;
    public bool startRecieving = true;
    public bool printToConsole = false;
    public string data;


    public void Start()
    {
        receiveThread = new Thread(
        new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

    }

    // receive thread
    private void ReceiveData()
    {

        client = new UdpClient(port);
        while (startRecieving)
        {

            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataByte);

                if (printToConsole) { print(data); }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }


    public void disableScript()
    {
        startRecieving = false; // Menghentikan menerima data
        client.Close(); // Menutup koneksi UDP
        enabled = false; // Menonaktifkan skrip
    }

    public void removeScript(){
        Destroy(this);
    }

}


// using UnityEngine;
// using System;
// using System.Text;
// using System.Net;
// using System.Net.Sockets;
// using System.Threading;

// public class UDPReceive : MonoBehaviour
// {
//     private static UDPReceive _instance;
//     public static UDPReceive Instance
//     {
//         get
//         {
//             if (_instance == null)
//             {
//                 _instance = new GameObject("UDPReceive").AddComponent<UDPReceive>();
//                 DontDestroyOnLoad(_instance.gameObject);
//             }
//             return _instance;
//         }
//     }

//     private Thread receiveThread;
//     private UdpClient client;
//     public int port = 5055;
//     public bool startRecieving = true;
//     public bool printToConsole = false;
//     public string data;
//     public bool isRunning { get; private set; } = false;

//     void Awake()
//     {
//         if (_instance == null)
//         {
//             _instance = this;
//             DontDestroyOnLoad(gameObject);
//             InitializeUDP();
//         }
//         else if (_instance != this)
//         {
//             Destroy(gameObject);
//         }
//     }

//     private void InitializeUDP()
//     {
//         receiveThread = new Thread(new ThreadStart(ReceiveData));
//         receiveThread.IsBackground = true;
//         receiveThread.Start();
//         isRunning = true;
//     }

//     private void ReceiveData()
//     {
//         client = new UdpClient(port);
//         while (startRecieving)
//         {
//             try
//             {
//                 IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
//                 byte[] dataByte = client.Receive(ref anyIP);
//                 data = Encoding.UTF8.GetString(dataByte);

//                 if (printToConsole)
//                 {
//                     print(data);
//                 }
//             }
//             catch (Exception err)
//             {
//                 print(err.ToString());
//             }
//         }
//     }

//     public void disableScript()
//     {
//         startRecieving = false; // Menghentikan menerima data
//         if (client != null)
//         {
//             client.Close(); // Menutup koneksi UDP
//         }
//         if (receiveThread != null)
//         {
//             receiveThread.Abort();
//         }
//         isRunning = false;
//         enabled = false; // Menonaktifkan skrip
//     }

//     public void removeScript()
//     {
//         disableScript();
//         Destroy(this);
//     }

//     void OnApplicationQuit()
//     {
//         disableScript();
//     }
// }
