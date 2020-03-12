using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameNetwork : MonoBehaviour
{

    static SocketIOComponent socket;
    public bool playerLoaded = false;
    public GameObject playerSpawn;
    Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    public int InLobby;
    // Start is called before the first frame update
    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("MakePlayer", MakePlayers);
        
    }

  
    private void GetPlayerInformation()
    {
        Debug.Log("Requesting Player information");
        JSONObject data = new JSONObject();
        data.AddField("username", GameObject.Find("DataTransfer").GetComponent<SceneTransfer>().StringTransfer);

    }

    private void MakePlayers(SocketIOEvent e)
    {
        InLobby++;
        GameObject player = Instantiate(playerSpawn);
        player.GetComponent<Player>().Name = e.data["name"].ToString();
       // player.GetComponent<Player>().Wins = e.data["Wins"].ToString();
      //  player.GetComponent<Player>().Losses = e.data["Losses"].ToString();
       // player.GetComponent<Player>().PlayerNumber = players.Count + 1;
        players.Add(player.name, player);
        playerLoaded = true;
        Debug.Log("Made Player: " + e.data["name"].ToString());
    }

    // Update is called once per frame
    void Update()
    {
        //socket.Emit("Test");

    }
}
