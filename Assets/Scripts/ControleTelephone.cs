using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleTelephone : MonoBehaviour
{
    public GameObject[] Lumiere;
    bool LumiereAllumer;
    public bool peutOuvrirCams;
    bool peutOuvrirLum;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject lum in Lumiere)
        {
            lum.SetActive(false);
        }
        Invoke("JouerAnimationTelephone", 25f);
        peutOuvrirLum = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && peutOuvrirLum)
        {
            LumiereAllumer = !LumiereAllumer;

            foreach (GameObject lum in Lumiere)
            {
                lum.SetActive(LumiereAllumer);
            }
        }
    }

    void JouerAnimationTelephone()
    {
        GetComponent<Animator>().SetTrigger("DebutJeux");
        peutOuvrirCams = true;
        peutOuvrirLum = true;
    }
}
