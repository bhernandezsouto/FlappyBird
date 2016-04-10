using UnityEngine;
using System.Collections;

public class BotonesScript : MonoBehaviour {
    public GameObject bird;
    public void iniciarNormal()
    {
        Application.LoadLevel("Game");
    }
    public void Reiniciar()
    {
        Application.LoadLevel("Game");
    }
    public void ReiniciarH()
    {
        Application.LoadLevel("GameHard");
    }
    public void Menu()
    {
        Application.LoadLevel("Menu");
    }
}
