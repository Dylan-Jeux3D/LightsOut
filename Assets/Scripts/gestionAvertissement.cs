using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gestionAvertissement : MonoBehaviour
{

    public GameObject textExperience;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("activerLeTextExperience", 10f);
        Invoke("allerVersMenuPrincipale", 20f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void activerLeTextExperience()
    {
        textExperience.SetActive(true);
    }

    void allerVersMenuPrincipale()
    {
        SceneManager.LoadScene(1);
    }
}
