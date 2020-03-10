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


    public GameObject ServerStatus,incorrect, DataTransfer;

    
    public InputField regUsername, regPassword, regEmail, logUser,logPass;

    public bool IsConnected = false;
    // Start is called before the first frame update



    void Start()


    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("login", OnLogin);
        socket.On("LoginAccepted", LoginAccepted);
        socket.On("LoginDenied", LoginDenied);
        
    }

    

    private void LoginDenied(SocketIOEvent e)
    {
        incorrect.SetActive(true);
    }

    private void LoginAccepted(SocketIOEvent e)
    {
        
        SceneManager.LoadScene("Game");
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


    // Update is called once per frame
    void Update()   
    {
        if (IsConnected)
        {
            ServerStatus.GetComponent<Text>().text = "Server Status: Connected";
        }
      
    }
}
