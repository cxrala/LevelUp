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
        Debug.Log($"Showing sprite for {godName}");
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
        float elapsedTime = 0f;
        float lerpValue;
        float transitionTime = 0.25f;

        // turn up opacity
        while (elapsedTime < transitionTime) {
            lerpValue = Mathf.Clamp(elapsedTime / transitionTime, 0f, 1f);

            img.color = new Color(1f, 1f, 1f, lerpValue);

            elapsedTime += Time.deltaTime;

            // wait until next frame
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        // turn down opacity
        elapsedTime = 0;
        while (elapsedTime < transitionTime) {
            lerpValue = Mathf.Clamp(elapsedTime / transitionTime, 0f, 1f);

            img.color = new Color(1f, 1f, 1f, 1 - lerpValue);

            elapsedTime += Time.deltaTime;

            // wait until next frame
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
