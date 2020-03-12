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


    public GameObject ServerStatus,incorrect, DataTransfer, ManagerOfUI, gameManager, p1,p2;
    public GameObject playerSpawn;
    public int InLobby;
    public InputField regUsername, regPassword, regEmail, logUser,logPass;
    public int ClientNumber;
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
        socket.On("OppenentPicked", OpponentPicked);
        socket.On("Reset", ResetForNextRound);
    }

    private void ResetForNextRound(SocketIOEvent obj)
    {
        Debug.Log("Resetting");
        p1.GetComponent<Player>().picked = "";
        p2.GetComponent<Player>().picked = "";
        gameManager.GetComponent<GameManager>().p1Picked = false;
        gameManager.GetComponent<GameManager>().p2Picked = false;
        ManagerOfUI.GetComponent<UIManager>().Reset();
    }

    private void OpponentPicked(SocketIOEvent obj)
    {
        Debug.Log(int.Parse(obj.data["whoPicked"].ToString()));
        gameManager.GetComponent<GameManager>().OppenentPick(int.Parse(obj.data["whoPicked"].ToString()), obj.data["theChoice"].ToString().Trim('"'));

    }

    private void StartGame(SocketIOEvent obj)
    {
        Debug.Log("Game Started");
        ManagerOfUI.GetComponent<UIManager>().GameStart();
    }

    private void GetNumber(SocketIOEvent obj)
    {
        ClientNumber = int.Parse(obj.data["playerNumber"].ToString());

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

    public void SendChoice()
    {

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
        p1.GetComponent<Player>().picked = "";
        p2.GetComponent<Player>().picked = "";
        gameManager.GetComponent<GameManager>().p1Picked = false;
        gameManager.GetComponent<GameManager>().p2Picked = false;
    }

    

    public void Picked(string choice)
    {
        //Debug.Log("Sending a thing");
        ManagerOfUI.GetComponent<UIManager>().Waiting();
        JSONObject data = new JSONObject();
        data.AddField("whoPicked", ClientNumber);
        data.AddField("theChoice", choice);
        socket.Emit("APLayerPicked", data);
    }

    private void MakePlayers(SocketIOEvent e)
    {
        InLobby++;
        GameObject player = Instantiate(playerSpawn);
        int aNumber = players.Count + 1;
        player.name = "p"+ aNumber;
        player.GetComponent<Player>().Name = e.data["name"].ToString().Trim('"');
        player.GetComponent<Player>().Wins = int.Parse(e.data["Wins"].ToString());
        player.GetComponent<Player>().Losses = int.Parse(e.data["Losses"].ToString());
        player.GetComponent<Player>().PlayerNumber = players.Count + 1;
        players.Add(player.name, player);
        Debug.Log("Made Player: " + e.data["name"].ToString().Trim('"'));
        if (players.Count == 2)
        {
            AssignPlayers();
            gameManager.GetComponent<GameManager>().SetPlayers();
        }
    }

    public void SendResults()
    {
        JSONObject pResults = new JSONObject();
        pResults.AddField("p1name", p1.GetComponent<Player>().Name);
     
        pResults.AddField("p1Wins", p1.GetComponent<Player>().Wins);
        pResults.AddField("p1Losses", p1.GetComponent<Player>().Losses);
        pResults.AddField("p2name", p2.GetComponent<Player>().Name);
        pResults.AddField("p2Wins", p2.GetComponent<Player>().Wins);
        pResults.AddField("p2Losses", p2.GetComponent<Player>().Losses);
        socket.Emit("Results", pResults);
    }

    void AssignPlayers()
    {
        p1 = GameObject.Find("p1");
        p2 = GameObject.Find("p2");
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
