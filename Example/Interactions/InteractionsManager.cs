using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InteractionsManager : MonoBehaviour
{
    public Interaction currentInteraction;
    [field: SerializeField] public List<float> _interactionsLoadTimes = new List<float>();
    [field: SerializeField] public List<string> _interactionsDescriptions = new List<string>();
    [field: SerializeField] public GameObject InteractionUIprefab {get; private set;}
    [SerializeField] public GameObject ChestUI;
    private Vector3 extens = new Vector3(0.5f, 2f, 0.25f);
    [SerializeField] public Inventory inventory;
    [SerializeField] public Chest chest;
    public enum InteractionType
    {
        item,
        chest,
    }
    public Interaction GetCurrentInteraction()
    {
        return currentInteraction;
    }
    public void SearchForInteract()
    {
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, extens, transform.forward, Quaternion.identity, 10f);
        SortByDistance(hits);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.TryGetComponent<Interaction>(out Interaction interactable))
            {
                currentInteraction = interactable;
                return;
            }
        }
    }
    private void SortByDistance(RaycastHit[] hits)
    {
        float[] distance = new float[hits.Length];
        for (int i = 0; i < hits.Length; i++)
        {
            distance[i] = Vector3.Distance(transform.position, hits[i].transform.position);
        }
       
        Array.Sort(distance, hits);
    }
}
