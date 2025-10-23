using UnityEngine;

public class GameManager : MonoBehaviour
{
    GrannyController grannyController;

    public GameObject aimPanel;

    private void Awake()
    {
        grannyController = GameObject.FindGameObjectWithTag("Player").GetComponent<GrannyController>();
    }

    private void Update()
    {
        if (grannyController.zoomIn)
        {
            aimPanel.SetActive(true);
        }

        else
        {
            aimPanel.SetActive(false);
        }
    }


}
