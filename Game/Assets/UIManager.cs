using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{



    // Start is called before the first frame update

    public GameObject LoginButton, CreateAccountButton, LoginFields, CreateAccountFields;


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

}
