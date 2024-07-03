using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ManaProperties : MonoBehaviour
{
    [SerializeField] private float maxMana = 10;
    [SerializeField] private float currentMana = 0;
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
        manaBorderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxMana * 50);
        manaContentsRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentMana * 50);
    }
}
