using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodProperties : MonoBehaviour
{
    // god data classes
    [System.Serializable]
    public class God
    {
        public float abilityCost;
    }

    [System.Serializable]
    public class Gods {
        public God ares;
        public God athena;
        public God aphrodite;
        public God demeter;
    }

    public TextAsset godJson;
    
    // object containing all the god data
    public Gods godData;

    // Start is called before the first frame update
    void Start()
    {
        godData = JsonUtility.FromJson<Gods>(godJson.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
