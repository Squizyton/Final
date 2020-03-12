using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool rock, paper, scissors, p1Picked, p2Picked;
    public GameObject Network, p1, p2;
    bool checkedResults = false;
    // Start is called before the first frame update
    void Start()
    {
        Network = GameObject.Find("NetworkAsset");
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void SetPlayers()
    {
        p1 = GameObject.Find("p1");
        p2 = GameObject.Find("p2");
    }

    public void RockPicked()
    {
        if (Network.GetComponent<Network>().ClientNumber == 1)
        {
            p1Picked = true;
            p1.GetComponent<Player>().picked = "Rock";
            Network.GetComponent<Network>().Picked(p1.GetComponent<Player>().picked);
        }
        else
        {
            p2Picked = true;
            p2.GetComponent<Player>().picked = "Rock";
            Network.GetComponent<Network>().Picked(p2.GetComponent<Player>().picked);
        }
        rock = true;


    }
    public void PaperPicked()
    {
        if (Network.GetComponent<Network>().ClientNumber == 1)
        {
            p1Picked = true;
            p2.GetComponent<Player>().picked = "Paper";
            Network.GetComponent<Network>().Picked(p1.GetComponent<Player>().picked);
        }
        else
        {
            p2Picked = true;
            p2.GetComponent<Player>().picked = "Paper";
            Network.GetComponent<Network>().Picked(p2.GetComponent<Player>().picked);
        }
        paper = true;
    }
    public void ScissorsPicked()
    {
        if (Network.GetComponent<Network>().ClientNumber == 1)
        {
            p1Picked = true;
            p1.GetComponent<Player>().picked = "Scissors";
            Network.GetComponent<Network>().Picked(p1.GetComponent<Player>().picked);
        }
        else
        {
            p2Picked = true;
            p2.GetComponent<Player>().picked = "Scissors";
            Network.GetComponent<Network>().Picked(p2.GetComponent<Player>().picked);
        }
        scissors = true;
    }

    public void OppenentPick(int number, string Picked)
    {
        if (number == 1)
        {
            p1Picked = true;
            p1.GetComponent<Player>().picked = Picked;
        }
        else
        {
            p2Picked = true;
            p2.GetComponent<Player>().picked = Picked;
        }

        if (p1Picked == true && p2Picked == true)
        {
            Debug.Log("Deciding Winner");
            DecideWinner();
        }

    }

    void DecideWinner()
    {
        string p1Choice = p1.GetComponent<Player>().picked;
        string p2Choice = p2.GetComponent<Player>().picked;

        Debug.Log("P1 Choice: " + p1Choice + "P2 Choice: " + p2Choice);

        if (p1Choice == p2Choice)
        {
            Debug.Log("Nobody Wins!");
        }
        if (p1Choice == "Rock" && p2Choice == "Paper")
        {
            Debug.Log("One");

            p2.GetComponent<Player>().Wins++;
            p1.GetComponent<Player>().Losses++;
        }
        else if (p1Choice == "Rock" && p2Choice == "Scissors")
        {
            Debug.Log("Two");
            p1.GetComponent<Player>().Wins++;
            p2.GetComponent<Player>().Losses++;
        }
        else if (p1Choice == "Paper" && p2Choice == "Rock")
        {
            Debug.Log("Three");
            p1.GetComponent<Player>().Wins++;
            p2.GetComponent<Player>().Losses++;
        }
        else if (p1Choice == "Paper" && p2Choice == "Scissors")
        {
            Debug.Log("Four");
            p2.GetComponent<Player>().Wins++;
            p1.GetComponent<Player>().Losses++;
        }
        else if (p1Choice == "Scissors" && p2Choice == "Paper")
        {
            Debug.Log("Five");
            p1.GetComponent<Player>().Wins++;
            p2.GetComponent<Player>().Losses++;
        }
        else if (p1Choice == "Scissors" && p2Choice == "Rock")
        {
            Debug.Log("Six");
            p2.GetComponent<Player>().Wins++;
            p1.GetComponent<Player>().Losses++;
        }



        checkedResults = true;
      
            Debug.Log("Sent results");
            Network.GetComponent<Network>().SendResults();
    }
}
