using WebSocketSharp;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using System.Collections.Generic;
using System;

public class Packet
{
    [JsonProperty("l")] public string locate;
    [JsonProperty("t")] public string type;
    [JsonProperty("v")] public string value;

    public Packet(string _locate, string _type, string _value)
    {
        this.locate = _locate;
        this.type = _type;
        this.value = _value;
    }
}

public class Client : MonoBehaviour
{
    public static Client Instance = null;

    [SerializeField] string IP = "localhost", PORT = "3031";
    [SerializeField] TextMeshProUGUI codeTMP = null, noticeTMP = null;
    [SerializeField] GameObject watingPanel = null;
    public GameObject blockPanel = null;
    [SerializeField] GameController gameController = null;

    private WebSocket server;
    private Queue<Action> actions = new Queue<Action>();

    private void Awake()
    {
        if(Instance == null) Instance = this;

        server = new WebSocket("ws://" + IP + ":" + PORT);
        server.ConnectAsync();

        server.OnMessage += GetMessages;
    }

    private void GetMessages(object _sender, MessageEventArgs _args)
    {
        Packet packet = JsonConvert.DeserializeObject<Packet>(_args.Data);

        switch(packet.locate)
        {
            case "room":
                actions.Enqueue(() => RoomData(packet));
                break;
            case "game":
                actions.Enqueue(() => GameData(packet));
                break;
        }
    }

    private void RoomData(Packet _packet)
    {
        switch(_packet.type)
        {
            case "createRes":
                actions.Enqueue(() => {
                    codeTMP.text = _packet.value;
                    noticeTMP.text = "Waiting For Partner";
                    blockPanel.SetActive(false);
                });
                break;
            case "joinRes":
                actions.Enqueue(() => {
                    watingPanel.SetActive(false);
                    noticeTMP.text = "Other Turn";
                    blockPanel.SetActive(true);
                });
                break;
            case "joined":
                actions.Enqueue(() => {
                    watingPanel.SetActive(false);
                    noticeTMP.text = "Your Turn";
                });
                break;
        }
    }

    private void GameData(Packet _packet)
    {
        switch(_packet.type)
        {
            case "setted":
                actions.Enqueue(() => {
                    gameController.tmps[int.Parse(_packet.value)].GetComponentInParent<GridSpace>().SetSpace(false);
                    blockPanel.SetActive(false);
                    noticeTMP.text = "Your Turn";
                });
                break;
        }
    }

    private void Update()
    {
        while(actions.Count > 0) actions.Dequeue()?.Invoke();
    }

    public void Reset()
    {
        watingPanel.SetActive(true);
        noticeTMP.text = "Waiting Partner";
        codeTMP.text = "CODE : ";
    }

    public void CreateRoom()
    {
        SendMessages("room", "createReq", "");
    }

    public void JoinRoom(TMP_InputField _inputField)
    {
        SendMessages("room", "joinReq", _inputField.text);
    }

    public void SendMessages(string _locate, string _type, string _value)
    {
        Packet packet = new Packet(_locate, _type, _value);
        string data = JsonConvert.SerializeObject(packet);

        server.Send(data);
    }

    public void SendMessages(string _locate, string _type, object _value)
    {
        string value = JsonConvert.SerializeObject(_value);
        Packet packet = new Packet(_locate, _type, value);
        string data = JsonConvert.SerializeObject(packet);

        server.Send(data);
    }
}
