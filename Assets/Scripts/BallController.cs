using UnityEngine;

public class BallController : MonoBehaviour
{
    void Update()
    {
        ControlDestroying();
    }

    private void ControlDestroying()
    {
        if(transform.position.y < -15f)
            Destroy(gameObject);
    }
}
