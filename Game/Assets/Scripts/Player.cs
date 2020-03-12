using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public string Name;
    public int Wins, Losses;
    public int PlayerNumber;

    public Text winsText, nameText, lossesText;
    public string picked;

    void Start()
    {
        winsText = GameObject.Find("Wins" + PlayerNumber).GetComponent<Text>();
        nameText = GameObject.Find("Name" + PlayerNumber).GetComponent<Text>();
        lossesText = GameObject.Find("Losses" + PlayerNumber).GetComponent<Text>();
        winsText.text += Wins;
        nameText.text += Name;
        lossesText.text += Losses;
    }

    void SetWinLoss()
    {
        this.winsText.text ="Wins: " +  Wins;
        this.lossesText.text += "Losses: " + Wins; 
    }



    // Update is called once per frame
    void Update()
    {
    
    }
}
