using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpGameObject : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float lerpSpeed = 1f;
    [SerializeField] private bool bIsLocal = true;


    private void Update()
    {
        if(bIsLocal)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target.localPosition, lerpSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, target.position, lerpSpeed * Time.deltaTime);
        }
    }
}
