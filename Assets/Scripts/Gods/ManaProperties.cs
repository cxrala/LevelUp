using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ManaProperties : MonoBehaviour
{
    [SerializeField] private float maxMana = 10;
    [SerializeField] private float currentMana = 5;
    [SerializeField] private float manaIncreaseRate = 1;

    private RectTransform manaBorderRect;
    private RectTransform manaContentsRect;

    [SerializeField] private GameObject manaBorder;
    [SerializeField] private GameObject manaContents;

    // Start is called before the first frame update
    void Start()
    {
        manaBorderRect = manaBorder.GetComponent<RectTransform>();
        manaContentsRect = manaContents.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        manaBorderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxMana + 5);
        UpdateManaContents();
        manaContentsRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentMana);
    }

    private void UpdateManaContents() {
        float newMana = WidthDelta(Time.deltaTime) + currentMana;
        currentMana = newMana < maxMana ? newMana : maxMana;
    }
    private float WidthDelta(float dt) {
        return dt * manaIncreaseRate;
    }

    // public method to deplete mana
    public void DepleteMana(float amount) {
        currentMana -= amount;
    }
}