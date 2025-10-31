using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GrannyController grannyController;
    GrannyAttackScript grannyAttackScript;

    public GameObject aimPanel;


    //Charge Setup

    public Image chargeIcon;
    public float chargeMeter;
    public float chargeTimer = 2;

    private void Awake()
    {
        grannyController = GameObject.FindGameObjectWithTag("Player").GetComponent<GrannyController>();
        grannyAttackScript = GameObject.FindGameObjectWithTag("Player").GetComponent<GrannyAttackScript>();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (grannyController.zoomIn)
        {
            aimPanel.SetActive(true);
        }

        else
        {
            aimPanel.SetActive(false);
        }

        chargeMeter = grannyAttackScript.chargeGauge - 0.5f;
        chargeIcon.fillAmount = chargeMeter;

        if(chargeIcon.fillAmount <1 )
        {
            chargeIcon.color = Color.red;
        }
        else
        {
            chargeIcon.color = Color.green;
        }
    }


}
