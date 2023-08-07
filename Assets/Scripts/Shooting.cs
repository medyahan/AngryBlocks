using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class Shooting : MonoBehaviour
{
    [Header("POWER")]
    [SerializeField] private float _power;
    
    private Vector2 _startPos;
    private Vector2 _endPos;
    
    // BOOL
    private bool _shooting, _aiming;
    private bool _canShot = true;

    [Header("DOT")]
    [SerializeField] private GameObject _dots;
    private List<GameObject> projectilesPath;

    [Header("BALL")]
    private Rigidbody2D _ballRB;
    [SerializeField] private GameObject _ballPref;
    
    private static readonly int IsNewShot = Animator.StringToHash("IsNewShot");

    void Start()
    {
        projectilesPath = _dots.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);

        UIManager.Instance.UpdateShotCountText();
        HideDots();
    } 
  
    void Update()
    {
        _ballRB = _ballPref.GetComponent<Rigidbody2D>();
        
        if (!GameManager.Instance.IsShotOver && !IsMouseOverUI())
        {
            Aim();
            RotateGun();

            if (Input.GetMouseButtonUp(0))
            {
                _endPos = Input.mousePosition;
                GameManager.Instance.GunAnimator.SetBool(IsNewShot, false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                UIManager.Instance.Invoke(nameof(UIManager.UpdateShotCountText), .8f);
                GameManager.Instance.GunAnimator.SetBool(IsNewShot, true);
            }
        }
    }

    private void Aim()
    {
        if (_shooting)
            return;
        
        if (Input.GetMouseButton(0)) // basili tutuluyorsa
        {
            if (!_aiming)
            {
                _aiming = true;
                _startPos = Input.mousePosition;
                
                GameManager.Instance.ShotCount++;
            }
            else
            {
                // aim calculate path
                CalculatePath();
            }
        }
        else if(_aiming && !_shooting) // hedef alınıyor ve ateş edilmiyor ise
        {
            // shoot
            _aiming = false;
            HideDots();
            StartCoroutine(Shoot());
             
            if(GameManager.Instance.ShotCount == 1)
                GameManager.Instance.CameraControl.RotateToSide();
        }
        
        
    }

    private Vector2 GetShootForce(Vector3 force)
    {
        return (new Vector2(_startPos.x, _startPos.y) - new Vector2(force.x, force.y)) * _power;
    }

    private Vector2 GetDotPath(Vector2 starPos, Vector2 startVelocity, float t)
    {
        return starPos + startVelocity * t + 0.5f * Physics2D.gravity * t * t;
    }

    private void CalculatePath()
    {
        Vector2 velocity = GetShootForce(Input.mousePosition) * Time.fixedDeltaTime / _ballRB.mass;
        ShowDots();
        
        for (int i = 0; i < projectilesPath.Count; i++)
        {
            float t = i / 15f;
            Vector3 point = GetDotPath(transform.position, velocity, t);
            point.z = 1;
            projectilesPath[i].transform.position = point;
        }
    }

    private void ShowDots()
    {
        for (int i = 0; i < projectilesPath.Count; i++)
        {
            projectilesPath[i].GetComponent<Renderer>().enabled = true;
        }
    }

    private void HideDots()
    {
        for (int i = 0; i < projectilesPath.Count; i++)
        {
            projectilesPath[i].GetComponent<Renderer>().enabled = false;
        }
    }

    private void RotateGun()
    {
        Vector2 dir = projectilesPath[1].transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    IEnumerator Shoot()
    {
        for (int i = 0; i < GameManager.Instance.BallCount; i++)
        {
            yield return new WaitForSeconds(0.07f);
            GameObject ball = Instantiate(_ballPref, new Vector3(transform.position.x, transform.position.y, 0.5f), Quaternion.identity, GameManager.Instance.BallContainer.transform);

            _ballRB = ball.GetComponent<Rigidbody2D>();
            _ballRB.AddForce(GetShootForce(_endPos));
            
            UIManager.Instance.SetBallCountText((GameManager.Instance.BallCount - i - 1).ToString());
        }
        if (GameManager.Instance.ShotCount == GameManager.Instance.MaxShotCount)
            GameManager.Instance.IsShotOver = true;
        yield return new WaitForSeconds(0.5f);
        
        UIManager.Instance.SetBallCountText(GameManager.Instance.BallCount.ToString());

    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

}
