using UnityEngine;

public class KillScript : MonoBehaviour
{
    public float killTime = 2;

    void Start()
    {
        Destroy(gameObject, killTime);
    }
}
