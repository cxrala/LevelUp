using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class ManaProperties : MonoBehaviour
{

    // constant variables
    private const float manaUpperBound = 2000;
    private const float rateUpperBound = 100;

    // mana bar stats
    private float maxMana = 100;
    private float currentMana = 0;
    private float currManaShadow;
    private float manaIncreaseRate = 10;

    private RectTransform manaBorderRect;
    private RectTransform manaContentsRect;
    private RectTransform manaShadowRect;

    [SerializeField] private GameObject manaBorder;
    [SerializeField] private GameObject manaContents;
    [SerializeField] private GameObject manaShadow;

    [SerializeField] private GameObject godsObject;
    [SerializeField] private GameObject godOverlay;

    // Start is called before the first frame update
    void Start()
    {
        currManaShadow = currentMana;
        manaBorderRect = manaBorder.GetComponent<RectTransform>();
        manaContentsRect = manaContents.GetComponent<RectTransform>();
        manaShadowRect = manaShadow.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        manaBorderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxMana + 5);
        UpdateManaContents();
        manaContentsRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentMana);
        manaShadowRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currManaShadow);
    }

    private void UpdateManaContents() {
        float newMana = WidthDelta(Time.deltaTime) + currentMana;
        currentMana = newMana < maxMana ? newMana : maxMana;
        if (currManaShadow > currentMana) {
            UpdateManaShadow();
        } 
    }
    private float WidthDelta(float dt) {
        return dt * manaIncreaseRate;
    }

    // Update the mana shadow to trail behind the current mana.
    private void UpdateManaShadow() {
        float newManaShadow = currManaShadow - WidthDelta(Time.deltaTime * 5);
        currManaShadow = newManaShadow > currentMana ? newManaShadow : currentMana;
    }

    // Public method to trigger ability
    public void TriggerAbility(string godName) {
        currManaShadow = currentMana;
        float amount = 0;

        GodProperties gods = godsObject.GetComponent<GodProperties>();
        switch (godName){
            case "ares":
                amount = gods.godData.ares.abilityCost;
                break;
            case "athena":
                amount = gods.godData.athena.abilityCost;
                break;
            case "aphrodite":
                amount = gods.godData.aphrodite.abilityCost;
                break;
            case "demeter":
                amount = gods.godData.demeter.abilityCost;
                break;
        }

        float newMana = currentMana - amount;
        if (newMana < 0) {
            NotEnoughManaAction();
            return;
        }

        // success, perform action
        GodAction();
        // Show overlay
        godOverlay.GetComponent<OverlayBehaviour>().ShowImageOnClick(godName);
        Debug.Log($"Mana depleted by {amount} units");
        currentMana = newMana > 0 ? newMana : 0;
    }

    public void GodAction() {
        // update listeners with events
        Debug.Log("Successfully performed God action.");
    }

    public void NotEnoughManaAction() {
        // error for not enough mana
        Debug.Log("Not enough mana!");
    }

    public void UpgradeManaBar() {
        float newMaxMana = (float) (1.2 * maxMana);
        float newManaRate = (float) (1.1 * manaIncreaseRate);

        maxMana = newMaxMana < manaUpperBound ? newMaxMana : manaUpperBound;
        manaIncreaseRate = newManaRate < rateUpperBound ? newManaRate : rateUpperBound;
    }
}