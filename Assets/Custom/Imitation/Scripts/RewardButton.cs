using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        canUseButton = true;
    }
    

    private void Start()
    {
        ResetButton();

    }
    public bool CanUseButton()
    {
        return canUseButton;

    }

    public void UseButton()
    {
        if (canUseButton)
        {
            buttonMeshRenderer.material = greenDarkMaterial;
            buttonTransform.localScale = new Vector3(.5f, .2f, .5f);
            canUseButton = false;

            OnUsed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ImitationFoodAgent>(out ImitationFoodAgent imitationFoodAgent))
        {
            UseButton();
        }
    }

    public void ResetButton()
    {
        buttonMeshRenderer.material = greenMaterial;
        buttonTransform.localScale = new Vector3(.5f, .5f, .5f);

        transform.localPosition =
        new Vector3(
        transform.localPosition.x, transform.localPosition.y, 3);

        canUseButton = true;

    }

}
