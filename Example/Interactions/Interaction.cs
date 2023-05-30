using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Interaction : MonoBehaviour
{
    private float time;
    protected GameObject InteractionUIPrefab;
    private GameObject _InteractionUIObject = null;
    protected InteractionsManager InteractionsManager;
    protected InventorySystem InventorySystem;
    public abstract void ActivateInteraction();
    public abstract InteractionsManager.InteractionType ReturnInteractionType();

    private void Awake()
    {
        InteractionsManager = FindObjectOfType<InteractionsManager>();
        InventorySystem = FindObjectOfType<InventorySystem>();
        InteractionUIPrefab = InteractionsManager.InteractionUIprefab;
    }
    private void Start()
    { 
        
    }
    public void SpawnInteractionUI(string InteractionDescription)
    {
        if (_InteractionUIObject != null) return;
         
        _InteractionUIObject = Instantiate(InteractionUIPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        _InteractionUIObject.GetComponentInChildren<Text>().text = InteractionDescription;
    }
    public void DestroyInteractionUI()
    {
        DestroyImmediate(_InteractionUIObject,true);
        _InteractionUIObject = null;
    }
    public void LoadSlider(bool ispressed, float value = 0)
    {
        if(!ispressed)
        {
            time = 0;
        }

        time += Time.deltaTime;
        _InteractionUIObject.GetComponentInChildren<Slider>().value = value * time;

        if (_InteractionUIObject.GetComponentInChildren<Slider>().value == 100)
        {
            DestroyInteractionUI();
            InteractionsManager.currentInteraction.ActivateInteraction();
        }
    }
}
