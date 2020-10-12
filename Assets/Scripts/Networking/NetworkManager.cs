using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class NetworkManager : ScriptableObject
{
	private static NetworkManager singleton;
	public static NetworkManager GetInstance()
	{
		if(!singleton) { singleton = FindObjectOfType<NetworkManager>(); }
		if(!singleton) { singleton = CreateInstance<NetworkManager>(); }
		return singleton;
	}

	[SerializeField] int applicationPort = 25700;
	NetworkServer server = new NetworkServer();
	BroadcastSender broadcast = new BroadcastSender();
	NetClient client = new NetClient();
	BroadcastListener listen = new BroadcastListener();
	bool clientStarted;
	bool serverStarted;

	public void StartServer()
	{
		// Server & BroadcastSender
		Debug.Log("Starting Light");
		server.Initialize(25700, 1);
		server.AddCallback((ushort)DefaultMessageTypes.Connected, OnServerConnected);
		server.AddCallback((ushort)DefaultMessageTypes.Disconnected, OnServerDisconnected);
		broadcast.StartBroadcasting(25700, new MessageBase());
	}
	private void OnServerConnected(MessageBase msg)
	{
		Debug.Log("ServerConnected");
		broadcast.StopBroadcasting();
		DungeonSeed seed = new DungeonSeed();
		seed.seed = Random.state;
		server.SendToAll(10, seed);

	}
	private void OnServerDisconnected(MessageBase msg)
	{
		Debug.Log("ServerDisconnected");
	}

	public void Shutdown()
	{
		if(server.IsInitialized()) { server.Shutdown(); }
		if(client.IsInitialized()) { client.Shutdown(); }
		if(broadcast.IsBroadcasting()) { broadcast.StopBroadcasting(); }
		if(listen.IsListening()) { listen.StopListening(); }
	}


	public void StartClient()
	{
		// Client & BroadcastReceiver
		Debug.Log("Starting Dark");
		listen.StartListening(applicationPort, OnReceiveBroadcast);
		client.Initialize();
		client.AddCallback((ushort)DefaultMessageTypes.Connected, OnClientConnected);
		client.AddCallback((ushort)DefaultMessageTypes.Disconnected, OnClientDisconnected);
		client.AddCallback(10, OnLoadRNGState);
	}
	private void OnClientConnected(MessageBase msg)
	{
		Debug.Log("ClientConnected");
	}
	private void OnClientDisconnected(MessageBase msg)
	{
		Debug.Log("ClientDisconnected");
	}
	private void OnLoadRNGState(MessageBase msg)
	{
		Random.state = ((DungeonSeed)msg).seed;
		Debug.Log("Loaded RNG State");
	}
	//void OnFailedConnection(MessageBase msg)
	//{
	//	client.Shutdown();
	//	listen.StartListening(applicationPort, OnReceiveBroadcast);
	//}
	void OnReceiveBroadcast(string sourceIP, MessageBase msg)
	{
		listen.StopListening();
		client.Connect(sourceIP, applicationPort);
	}
}

[System.Serializable]
class DungeonSeed : MessageBase
{
	public Random.State seed;
}