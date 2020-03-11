using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool rock, paper, scissors;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RockPicked()
    {
        rock = true;
    }
    public void PaperPicked()
    {
        paper = true;
    }
    public void ScissorsPicked()
    {
        scissors = true;
    }
}
