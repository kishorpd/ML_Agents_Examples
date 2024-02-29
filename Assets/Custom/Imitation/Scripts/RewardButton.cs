using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RewardButton : MonoBehaviour
{

    [SerializeField]
    public event EventHandler OnUsed;

    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material greenDarkMaterial;
    [SerializeField] private Transform buttonTransform;

    private MeshRenderer buttonMeshRenderer;
    private bool canUseButton;

    private void Awake()
    {
        buttonMeshRenderer = buttonTransform.GetComponent<MeshRenderer>();
        DisableUseButton();
    }
    

    private void Start()
    {
        ResetButton();
    }

    public bool CanUseButton()
    {
        return canUseButton;

    }


    public void EnableUseButton()
    {
        canUseButton=true;
    }

    public void DisableUseButton()
    {
        canUseButton=false;
    }

    public void UseButton()
    {
        if (canUseButton)
        {
            buttonMeshRenderer.material = greenDarkMaterial;
            buttonTransform.localScale = new Vector3(1, .5f, 1);
            canUseButton = false;

            OnUsed?.Invoke(this, EventArgs.Empty);
        }
    }


    public void ResetButton()
    {
        buttonMeshRenderer.material = greenMaterial;
        buttonTransform.localScale = new Vector3(1, 1, 1);

        transform.localPosition =
        new Vector3(
            0, 
        transform.localPosition.y,
        Random.Range(1, -1)
        );

        canUseButton = false;

    }

}