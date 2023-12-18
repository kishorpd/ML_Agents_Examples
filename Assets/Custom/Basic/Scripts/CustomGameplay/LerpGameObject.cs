using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpGameObject : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float lerpSpeed = 1f;

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, target.localPosition, lerpSpeed * Time.deltaTime);
    }
}
