using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ManaProperties : MonoBehaviour
{
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

    // public method to deplete mana
    public void DepleteMana(float amount) {
        currManaShadow = currentMana;
        float newMana = currentMana - amount;
        if (newMana < 0) {
            NotEnoughManaAction();
            return;
        }
        // success, perform action
        GodAction();
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
}