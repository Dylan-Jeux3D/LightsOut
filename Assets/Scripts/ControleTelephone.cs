using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControleTelephone : MonoBehaviour
{
    public GameObject[] Lumiere;
    bool LumiereAllumer;
    public bool peutOuvrirCams;
    bool peutOuvrirLum;

    public GameObject cameraLogo;
    public GameObject toucheCamera;
    public TextMeshPro textTouche;
    public Color couleurTransparente;
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
        if (Input.GetKeyDown(KeyCode.F) && peutOuvrirLum)
        {
            LumiereAllumer = !LumiereAllumer;

            foreach (GameObject lum in Lumiere)
            {
                lum.SetActive(LumiereAllumer);
            }
        }

        if (breaker.breakerOuvert)
        {
            cameraLogo.GetComponent<SpriteRenderer>().color = Color.white;
            toucheCamera.GetComponent<SpriteRenderer>().color = Color.white;
            textTouche.color = Color.black;
        }
        else
        {
            cameraLogo.GetComponent<SpriteRenderer>().color = couleurTransparente;
            toucheCamera.GetComponent<SpriteRenderer>().color = couleurTransparente;
            textTouche.color = new Color(0f, 0f, 0f, 0.5f);
        }
    }

    void JouerAnimationTelephone()
    {
        GetComponent<Animator>().SetTrigger("DebutJeux");
        peutOuvrirCams = true;
        peutOuvrirLum = true;
    }
}
