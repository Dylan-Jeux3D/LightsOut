using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gestionMenuFin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void recommencerPartie()
    {
        SceneManager.LoadScene(1);
    }

    public void quitterJeu()
    {
        Application.Quit();
    }

    public void commencerPartie()
    {
        SceneManager.LoadScene(2);
    }
}
