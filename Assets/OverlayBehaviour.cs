using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayBehaviour : MonoBehaviour
{
    private Sprite ares;
    private Sprite athena;
    private Sprite aphrodite;
    private Sprite demeter;

    private Sprite currentSprite;

    private Image img;

    [SerializeField] private GameObject godsObject;

    void Awake() {
        img = gameObject.GetComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        GodProperties gods = godsObject.GetComponent<GodProperties>();

        ares = Resources.Load<Sprite>(gods.godData.ares.imgPath);
        athena = Resources.Load<Sprite>(gods.godData.athena.imgPath);
        aphrodite = Resources.Load<Sprite>(gods.godData.aphrodite.imgPath);
        demeter = Resources.Load<Sprite>(gods.godData.demeter.imgPath);

        gameObject.SetActive(false);
    }

    public void ShowImageOnClick(string godName) {
        gameObject.SetActive(true);

        switch (godName){
            case "ares":
                currentSprite = ares;
                break;
            case "athena":
                currentSprite = athena;
                break;
            case "aphrodite":
                currentSprite = aphrodite;
                break;
            case "demeter":
                currentSprite = demeter;
                break;
        }

        img.sprite = currentSprite;

        StartCoroutine(ShowImage(godName));
    }

    IEnumerator ShowImage(string godName) {
        // turn up opacity
        img.color = new Color(1f, 1f, 1f, 1f);

        yield return new WaitForSeconds(1);

        // turn down opacity
        img.color = new Color(1f, 1f, 1f, 0f);

        gameObject.SetActive(false);
    }
}
