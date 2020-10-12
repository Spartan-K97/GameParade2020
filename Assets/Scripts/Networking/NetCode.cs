using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;

[Serializable]
class MessageBase
{
	public ushort messageType;
}

enum DefaultMessageTypes
{
	ConnectionFailed = 0,
	Connected,
	Disconnected,
	UnrecoverableError
}

class Serialization
{
	public static byte[] Serialize<T>(T data)
	{
		System.IO.MemoryStream ms = new System.IO.MemoryStream();
		System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bin = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
		bin.Serialize(ms, data);
		return ms.ToArray();
	}

	public static T Deserialize<T>(byte[] data)
	{
		System.IO.MemoryStream ms = new System.IO.MemoryStream(data);
		System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bin = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
		return (T)bin.Deserialize(ms);
	}
}

class StaticMono : MonoBehaviour
{
	static StaticMono singleton = null;

	public static StaticMono GetRef()
	{
		if (singleton == null)
		{
			singleton = new GameObject("StaticObject").AddComponent<StaticMono>();
			DontDestroyOnLoad(singleton);
		}
		return singleton;
	}
}

class NetworkServer
{
	ConcurrentQueue<byte[]> messages;
	Dictionary<ushort, Action<MessageBase>> callbacks;

	Socket listener;
	Socket[] sockets;
	Thread[] threads;

	Coroutine yielder;

	public bool IsInitialized()
	{
		return yielder != null;
	}

	public void Initialize(int port, int numClients)
	{
		messages = new ConcurrentQueue<byte[]>();
		callbacks = new Dictionary<ushort, Action<MessageBase>>();

		IPAddress ipAddress = IPAddress.Any;

		listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		IPEndPoint localEP = new IPEndPoint(ipAddress, port);

		listener.Bind(localEP);
		listener.Listen(numClients);

		threads = new Thread[numClients];
		sockets = new Socket[numClients];
		for (int threadID = 0; threadID < numClients; ++threadID)
		{
			int num = threadID; // Make sure a copy is sent to the thread instead of a reference to the original
			threads[threadID] = new Thread(() => Connection(num));
			threads[threadID].IsBackground = true;
			threads[threadID].Start();
		}
		yielder = StaticMono.GetRef().StartCoroutine(Update());
	}
	public void SendToAll(ushort msgType, MessageBase msg)
	{
		msg.messageType = msgType;
		byte[] data = Serialization.Serialize(msg);
		byte[] finalData = new byte[data.Length + 2];
		Buffer.BlockCopy(BitConverter.GetBytes((ushort)data.Length), 0, finalData, 0, 2);
		Buffer.BlockCopy(data, 0, finalData, 2, data.Length);

		for (int x = 0; x < threads.Length; ++x)
		{
			sockets[x].Send(finalData);
		}
	}

	public void SendToClient(int clientID, ushort msgType, MessageBase msg)
	{
		msg.messageType = msgType;
		byte[] data = Serialization.Serialize(msg);
		byte[] finalData = new byte[data.Length + 2];
		Buffer.BlockCopy(BitConverter.GetBytes((ushort)data.Length), 0, finalData, 0, 2);
		Buffer.BlockCopy(data, 0, finalData, 2, data.Length);

		sockets[clientID].Send(finalData);
	}

	public void AddCallback(ushort msgType, Action<MessageBase> func)
	{
		callbacks.Add(msgType, func);
	}

	IEnumerator Update() // Switch to ienumerator with yields
	{
		byte[] res;
		while (true)
		{
			while (messages != null && messages.TryDequeue(out res))
			{
				MessageBase msg = Serialization.Deserialize<MessageBase>(res);
				Action<MessageBase> func;
				if(callbacks.TryGetValue(msg.messageType, out func))
				{
					func(msg);
				}
			}
			yield return true; // Wait for next frame
		}
	}

	void Connection(int connectionID)
	{
		try
		{
			while (true) // Reopen connection on failure
			{
				try { sockets[connectionID] = listener.Accept(); }
				catch (SocketException) { messages.Enqueue(Serialization.Serialize(new MessageBase { messageType = (ushort)DefaultMessageTypes.ConnectionFailed })); continue; }
				messages.Enqueue(Serialization.Serialize(new MessageBase { messageType = (ushort)DefaultMessageTypes.Connected }));
				byte[] dataHeader = new byte[2];
				while (true)
				{
					sockets[connectionID].Receive(dataHeader);
					if (dataHeader[0] == 0 && dataHeader[1] == 0)
					{
						messages.Enqueue(Serialization.Serialize(new MessageBase { messageType = (ushort)DefaultMessageTypes.Disconnected }));
						break;
					}
					ushort dataSize = BitConverter.ToUInt16(dataHeader, 0);

					byte[] data = new byte[dataSize]; // This can probably be handled better
					sockets[connectionID].Receive(data);

					messages.Enqueue(data);
				}
			}
		}
		catch (ThreadAbortException)
		{
			return;
		}
	}

	public void Shutdown()
	{
		StaticMono.GetRef().StopCoroutine(yielder); yielder = null;
		foreach (Thread thread in threads) { thread.Abort(); } threads = null;
		listener.Close(); listener = null;
		sockets = null;
		callbacks.Clear(); callbacks = null;
		byte[] res; while (messages.TryDequeue(out res)) {}; messages = null;
	}
}

class NetClient
{
	ConcurrentQueue<byte[]> messages;
	Dictionary<ushort, Action<MessageBase>> callbacks;

	Socket socket;
	Thread thread;

	Coroutine yielder;

	public bool IsInitialized()
	{
		return yielder != null;
	}

	public void Initialize()
	{
		callbacks = new Dictionary<ushort, Action<MessageBase>>();
		messages = new ConcurrentQueue<byte[]>();
	}

	public void Connect(string ip, int port)
	{
		IPAddress ipAddress = IPAddress.Parse(ip);

		socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		IPEndPoint targetEP = new IPEndPoint(ipAddress, port);

		thread = new Thread(() => Connection(targetEP));
		thread.IsBackground = true;
		thread.Start();

		yielder = StaticMono.GetRef().StartCoroutine(Update());
	}

	private void Disconnect()
	{
		// Not implemented
	}

	public void Send(ushort msgType, MessageBase msg)
	{
		msg.messageType = msgType;
		byte[] data = Serialization.Serialize(msg);
		byte[] finalData = new byte[data.Length + 2];
		Buffer.BlockCopy(BitConverter.GetBytes((ushort)data.Length), 0, finalData, 0, 2);
		Buffer.BlockCopy(data, 0, finalData, 2, data.Length);
		socket.Send(finalData);
	}

	public void AddCallback(ushort msgType, Action<MessageBase> func)
	{
		callbacks.Add(msgType, func);
	}

	IEnumerator Update() // Switch to ienumerator with yields
	{
		byte[] res;
		while (true)
		{
			while (messages != null && messages.TryDequeue(out res))
			{
				MessageBase msg = Serialization.Deserialize<MessageBase>(res);
				Action<MessageBase> func;
				if (callbacks.TryGetValue(msg.messageType, out func))
				{
					func(msg);
				}
			}
			yield return true; // Wait for next frame
		}
	}

	void Connection(IPEndPoint targetEP)
	{
		try
		{
			while (true) // Reopen connection on failure
			{
				try { socket.Connect(targetEP); }
				catch (SocketException) { messages.Enqueue(Serialization.Serialize(new MessageBase { messageType = (ushort)DefaultMessageTypes.ConnectionFailed })); continue; }
				messages.Enqueue(Serialization.Serialize(new MessageBase { messageType = (ushort)DefaultMessageTypes.Connected }));
				byte[] dataHeader = new byte[2];
				while (true)
				{
					socket.Receive(dataHeader);
					if(dataHeader[0] == 0 && dataHeader[1] == 0)
					{
						messages.Enqueue(Serialization.Serialize(new MessageBase { messageType = (ushort)DefaultMessageTypes.Disconnected }));
						break;
					}
					ushort dataSize = BitConverter.ToUInt16(dataHeader, 0);

					byte[] data = new byte[dataSize]; // This can probably be handled better
					socket.Receive(data);

					messages.Enqueue(data);
				}
			}
		}
		catch (ThreadAbortException)
		{
			return;
		}
	}

	public void Shutdown()
	{
		StaticMono.GetRef().StopCoroutine(yielder); yielder = null;
		thread.Abort(); thread = null;
		socket.Close(); socket = null;
		callbacks.Clear(); callbacks = null;
		byte[] res; while (messages.TryDequeue(out res)) {}; messages = null;
	}
}

class UDPNetwork
{
	ConcurrentQueue<byte[]> messages;
	Dictionary<ushort, Action<MessageBase>> callbacks;

	IPEndPoint targetEP;
	UdpClient listener;
	Socket sender;
	Thread thread;

	Coroutine yielder;

	public bool IsInitialized()
	{
		return yielder != null;
	}

	public void Initialize(string ip, int port)
	{
		messages = new ConcurrentQueue<byte[]>();
		callbacks = new Dictionary<ushort, Action<MessageBase>>();

		IPAddress ipAddress = IPAddress.Parse(ip);

		listener = new UdpClient(port);
		targetEP = new IPEndPoint(ipAddress, port);

		thread = new Thread(() => Connection());
		thread.IsBackground = true;
		thread.Start();

		yielder = StaticMono.GetRef().StartCoroutine(Update());

		sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		sender.EnableBroadcast = true;
	}

	public void Send(ushort msgType, MessageBase msg)
	{
		Serialization.Serialize(new Tuple<ushort, MessageBase>(msgType, msg));
		msg.messageType = msgType;

		sender.SendTo(Serialization.Serialize(msg), targetEP);
	}

	public void AddCallback(ushort msgType, Action<MessageBase> func)
	{
		callbacks.Add(msgType, func);
	}

	IEnumerator Update() // Switch to ienumerator with yields
	{
		byte[] data;
		while (true)
		{
			while (messages != null && messages.TryDequeue(out data))
			{
				MessageBase msg = Serialization.Deserialize<MessageBase>(data);
				Action<MessageBase> func;
				if (callbacks.TryGetValue(msg.messageType, out func))
				{
					func(msg);
				}
			}
			yield return true; // Wait for next frame
		}
	}

	void Connection()
	{
		IPEndPoint receivedFrom = targetEP; // Lazy initialize
		while (true)
		{
			try
			{
				messages.Enqueue(listener.Receive(ref receivedFrom));
			}
			catch (ThreadAbortException)
			{
				break;
			}
		}
	}

	public void Shutdown()
	{
		StaticMono.GetRef().StopCoroutine(yielder); yielder = null;
		thread.Abort(); thread = null;
		sender.Close(); sender = null;
		listener.Close(); listener = null;
		callbacks.Clear(); callbacks = null;
		byte[] res; while (messages.TryDequeue(out res)) { }; messages = null;
	}
}

class BroadcastSender
{
	byte[] data;
	IPEndPoint targetEP;

	Coroutine yielder;
	Socket sender;

	public bool IsBroadcasting()
	{
		return yielder != null;
	}

	public void StartBroadcasting(int port, MessageBase broadcastData)
	{
		targetEP = new IPEndPoint(IPAddress.Broadcast, port);

		data = Serialization.Serialize(broadcastData);

		sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		sender.EnableBroadcast = true;

		yielder = StaticMono.GetRef().StartCoroutine(Update());
	}

	IEnumerator Update()
	{
		while (true)
		{
			sender.SendTo(data, targetEP);
			yield return new WaitForSecondsRealtime(1);
		}
	}

	public void StopBroadcasting()
	{
		StaticMono.GetRef().StopCoroutine(yielder); yielder = null;
		sender.Close(); sender = null;
		data = null;
	}
}

class BroadcastListener
{
	ConcurrentQueue<Tuple<string, byte[]>> messages;
	Action<string, MessageBase> ReceivedBroadcast;

	IPEndPoint targetEP;
	UdpClient listener;
	Thread thread;

	Coroutine yielder;

	public bool IsListening()
	{
		return yielder != null;
	}

	public void StartListening(int port, Action<string, MessageBase> callback)
	{
		ReceivedBroadcast = callback;
		messages = new ConcurrentQueue<Tuple<string, byte[]>>();

		listener = new UdpClient(port);
		targetEP = new IPEndPoint(IPAddress.Any, port);

		thread = new Thread(() => Connection());
		thread.IsBackground = true;
		thread.Start();

		yielder = StaticMono.GetRef().StartCoroutine(Update());
	}

	IEnumerator Update() // Switch to ienumerator with yields
	{
		Tuple<string, byte[]> res;
		while (true)
		{
			while (messages != null && messages.TryDequeue(out res))
			{
				ReceivedBroadcast(res.Item1, Serialization.Deserialize<MessageBase>(res.Item2));
			}
			yield return true; // Wait for next frame
		}
	}

	void Connection()
	{
		IPEndPoint receivedFrom = targetEP; // Lazy initialize
		while (true)
		{
			try
			{
				byte[] data = listener.Receive(ref receivedFrom);

				messages.Enqueue(new Tuple<string, byte[]>(receivedFrom.Address.ToString(), data));
			}
			catch (ThreadAbortException)
			{
				break;
			}
		}
	}

	public void StopListening()
	{
		StaticMono.GetRef().StopCoroutine(yielder); yielder = null;
		thread.Abort(); thread = null;
		listener.Close(); listener = null;
		ReceivedBroadcast = null;
		Tuple<string, byte[]> res; while (messages.TryDequeue(out res)) { }; messages = null;
	}
}
