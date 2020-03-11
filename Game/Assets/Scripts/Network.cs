using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Network : MonoBehaviour
{

    static SocketIOComponent socket;


    public GameObject ServerStatus,incorrect, DataTransfer, ManagerOfUI, gameManager;
    public GameObject playerSpawn;
    public int InLobby;
    public InputField regUsername, regPassword, regEmail, logUser,logPass;
    public int PlayerNumber;
    public bool IsConnected = false;
    // Start is called before the first frame update


    Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    void Start()


    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("login", OnLogin);
        socket.On("LoginAccepted", LoginAccepted);
        socket.On("LoginDenied", LoginDenied);
        socket.On("MakePlayer", MakePlayers);
        socket.On("GetPlayerNumber", GetNumber);
        socket.On("StartGame", StartGame);
    }

    private void StartGame(SocketIOEvent obj)
    {
        Debug.Log("Game Started");
        ManagerOfUI.GetComponent<UIManager>().GameStart();
    }

    private void GetNumber(SocketIOEvent obj)
    {
        PlayerNumber = int.Parse(obj.data["playerNumber"].ToString());
    }

    private void LoginDenied(SocketIOEvent e)
    {
        incorrect.SetActive(true);
    }

    private void LoginAccepted(SocketIOEvent e)
    {

       
        ManagerOfUI.GetComponent<UIManager>().GameUI();
        //PlayerNumber = int.Parse(e.data["playerNumber"].ToString());
        //SceneManager.LoadScene("Game");
        Debug.Log("Login Accepted");
    }

    void OnConnected(SocketIOEvent e)
    {
        ChangeServerStatus();
    }

    public void ChangeServerStatus()
    {
        Debug.Log("Connected");
        IsConnected = true;
    }

    public void Registered()
    {
        JSONObject data = new JSONObject();
        data.AddField("Username", regUsername.text);
        data.AddField("Password", regPassword.text);
        data.AddField("Email", regEmail.text);
        Debug.Log(data);
        socket.Emit("Registered", data);
    }
    public void AttemptLogin()
    {
        DataTransfer = GameObject.Find("DataTransfer");
        DataTransfer.GetComponent<SceneTransfer>().StringTransfer += logUser.text;
        JSONObject data = new JSONObject();
        data.AddField("Username", logUser.text);
        data.AddField("Password", logPass.text);
        socket.Emit("AttemptLogin", data);
    }
    void OnLogin(SocketIOEvent e)
    {
        throw new NotImplementedException();
    }


    private void MakePlayers(SocketIOEvent e)
    {
        InLobby++;
        GameObject player = Instantiate(playerSpawn);
        player.name = "Player"+ players.Count + 1;
        player.GetComponent<Player>().Name = e.data["name"].ToString();
        player.GetComponent<Player>().Wins = e.data["Wins"].ToString();
        player.GetComponent<Player>().Losses = e.data["Losses"].ToString();
        player.GetComponent<Player>().PlayerNumber = players.Count + 1;
        players.Add(player.name, player);
        Debug.Log("Made Player: " + e.data["name"].ToString());
    }


    // Update is called once per frame
    void Update()   
    {
        if (IsConnected)
        {
            ServerStatus.GetComponent<Text>().text = "Server Status: Connected";
        }
      
    }
}
