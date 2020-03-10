using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public string Name, Wins, Losses;
    public int PlayerNumber;

    public Text winsText,nameText,lossesText;

    void Start()
    {
        winsText = GameObject.Find("Wins" + PlayerNumber).GetComponent<Text>();
        nameText = GameObject.Find("Name" + PlayerNumber).GetComponent<Text>();
        lossesText = GameObject.Find("Losses" + PlayerNumber).GetComponent<Text>();
        winsText.text += Wins;
        nameText.text += Name;
        lossesText.text += Losses;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
