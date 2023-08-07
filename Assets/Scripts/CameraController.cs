using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    private float _rotateSemiAmount = 4;
    private float _shakeAmount;

    private Vector3 _startingLocalPos;

    void Start()
    {
        _startingLocalPos = transform.localPosition; // local posizyonunu atama
    }


    void Update()
    {
        if (_shakeAmount > 0.01f)
        {
            Vector3 localPos = _startingLocalPos;
            localPos.x += _shakeAmount * Random.Range(3, 5);
            localPos.y += _shakeAmount * Random.Range(3, 5);

            transform.localPosition = localPos;
            _shakeAmount = 0.9f * _shakeAmount;
        }
    }

    public void SmallShake()
    {
        _shakeAmount = Mathf.Min(0.1f, _shakeAmount + 0.01f);
    }
    public void MediumShake()
    {
        _shakeAmount = Mathf.Min(0.15f, _shakeAmount + 0.015f);
    }

    public void RotateToSide()
    {
        StartCoroutine(RotateToSideRoutine());
    }

    public void RotateToFront()
    {
        StartCoroutine(RotateToFrontRoutine());
    }

    IEnumerator RotateToSideRoutine()
    {
        int frame = 120;
        float increment = _rotateSemiAmount / (float) frame;

        for (int i = 0; i < frame; i++)
        {
            transform.parent.RotateAround(Vector3.zero, Vector3.up, increment);
            yield return null;
        }
        yield break;
    }

    IEnumerator RotateToFrontRoutine()
    {
        int frame = 120;
        float increment = _rotateSemiAmount / (float) frame;

        for (int i = 0; i < frame; i++)
        {
            transform.parent.RotateAround(Vector3.zero, Vector3.down, -increment);
            yield return null;
        }

        transform.parent.localEulerAngles = new Vector3(0, 0, 0);
        //yield break;
    }

}
