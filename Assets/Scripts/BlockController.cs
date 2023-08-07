using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BlockController : MonoBehaviour
{
    private int _count;
    [SerializeField] private Text _countText;
    
    void Start()
    {
        if (gameObject.activeInHierarchy)
        {
            GameManager.Instance.Blocks.Add(gameObject);
            AssignStartingCount();
        }
    }

    void Update()
    {
        if (transform.position.y <= -12)
        {
            Destroy(gameObject);
        }
    }

    private void AssignStartingCount()
    {
        int count = Random.Range(GameManager.Instance.MinBlockCount, GameManager.Instance.MaxBlockCount);
        _count = count;
        _countText.text = count.ToString();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball") && _count > 0)
        {
            _count--;
            GameManager.Instance.CameraControl.SmallShake();
            _countText.text = _count.ToString();

            GameManager.Instance.BounceSound.Play();
            if (_count == 0)
            {
                Destroy(gameObject);
                GameManager.Instance.CameraControl.MediumShake();
                UIManager.Instance.UpdateExtraBallBar();
            }
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.Blocks.Remove(gameObject);
    }
}
