using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CountryMouseInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private PointerInformation pointerInformation;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Country selectedCountry = gameObject.GetComponent<Country>();
        pointerInformation.LastClicked = selectedCountry;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // TODO: implement shading/stats if time
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("Pointer Exited");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Debug.Log("Pointer up!");
    }

    // Start is called before the first frame update
    void Start()
    {
        pointerInformation = GameObject.Find("PointerInformation").GetComponent<PointerInformation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
