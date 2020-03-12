using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{



    // Start is called before the first frame update

    public GameObject LoginButton, CreateAccountButton, LoginFields, CreateAccountFields,uiGame, gameButtons, waiting,stats,oppWaiting;


    public GameObject player1, player2;

    void Start()
    {
        
    }


    public void Login()
    {
        LoginButton.SetActive(false);
        CreateAccountButton.SetActive(false);
        LoginFields.SetActive(true);
    }


    public void CreateAccount()
    {
        LoginButton.SetActive(false);
        CreateAccountButton.SetActive(false);
        CreateAccountFields.SetActive(true);
    }

    public void GameUI()
    {
        LoginButton.SetActive(false);
        CreateAccountButton.SetActive(false);
        CreateAccountFields.SetActive(false);
        LoginFields.SetActive(false);

        uiGame.SetActive(true);
    }
    public void GameStart()
    {
        oppWaiting.SetActive(false);
        waiting.SetActive(false);         
        gameButtons.SetActive(true);
    }
    public void Reset()
    {
        oppWaiting.SetActive(false);
        waiting.SetActive(false);
        gameButtons.SetActive(false);
        StartCoroutine(StartAgain());
    }

    public void Waiting()
    {
        oppWaiting.SetActive(true);
        gameButtons.SetActive(false);
    }

    IEnumerator StartAgain()
    {
        yield return new WaitForSeconds(1);
        gameButtons.SetActive(true);
    }
}
