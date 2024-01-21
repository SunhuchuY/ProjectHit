using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

public class Test : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private byte[] buffer = new byte[1024];
    private string receivedData = string.Empty;

    [SerializeField] private Transform target;
    [SerializeField] private Transform monster;

    void Start()
    {
        ConnectToServer("127.0.0.1", 5500);
    }

    void ConnectToServer(string host, int port)
    {
        client = new TcpClient();
        client.Connect(host, port);
        stream = client.GetStream();
        stream.BeginRead(buffer, 0, buffer.Length, OnReceiveData, null);
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(receivedData))
        {
            receivedData = receivedData.Replace("(", "").Replace(")", "");
            var dataParts = receivedData.Split(';');
            Debug.Log(receivedData);

            // 위치 정보 처리
            var positionData = dataParts[0].Split(',');
            transform.position = new Vector3(Single.Parse(positionData[0]), Single.Parse(positionData[1]), Single.Parse(positionData[2]));

            // 회전 정보 처리
            var rotationData = dataParts[1].Split(',');
            transform.rotation = Quaternion.Euler(Single.Parse(rotationData[0]), Single.Parse(rotationData[1]), Single.Parse(rotationData[2]));

            // 위치 정보 처리
            var _monster_positionData = dataParts[2].Split(',');
            monster.transform.position = new Vector3(Single.Parse(positionData[0]), Single.Parse(positionData[1]), Single.Parse(positionData[2]));

            // 회전 정보 처리
            var _monster_rotationData = dataParts[3].Split(',');
            monster.transform.rotation = Quaternion.Euler(Single.Parse(rotationData[0]), Single.Parse(rotationData[1]), Single.Parse(rotationData[2]));
                
            receivedData = string.Empty;
        }

        // 현재 위치 및 회전 정보 서버에 전송
        string positionStr = target.position.ToString("F2");
        string rotationStr = target.rotation.eulerAngles.ToString("F2");
        string monster_PositionStr = monster.position.ToString("F2");
        string monster_rotationData = monster.rotation.eulerAngles.ToString("F2");
        SendData(positionStr + ";" + rotationStr + ";" + monster_PositionStr + ";" + monster_rotationData) ;
    }

    void OnReceiveData(IAsyncResult result)
    {
        try
        {
            int bytesRead = stream.EndRead(result);
            if (bytesRead > 0)
            {
                Debug.Log(bytesRead);
                receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                // 데이터 처리를 위한 추가 로직...

                // 다음 데이터를 기다림
                stream.BeginRead(buffer, 0, buffer.Length, OnReceiveData, null);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error receiving data: " + e.Message);
            // 필요한 경우 연결 종료 또는 재시도 로직
        }
    }

    void SendData(string data)
    {
        byte[] bytesToSend = Encoding.ASCII.GetBytes(data);
        stream.Write(bytesToSend, 0, bytesToSend.Length);
    }

    void OnApplicationQuit()
    {
        if (client != null)
        {
            client.Close();
        }
    }
}
