using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public static EffectsHolder instance;

    [System.Serializable]
    public struct NamedMod
    {
        public string name;
        public GameObject mod;
    }
    public NamedMod[] NamedMods;
    public Dictionary<string, GameObject> Mods;

    void Start()
    {
        instance = this;
        Mods = new Dictionary<string, GameObject>();
        foreach (NamedMod nm in NamedMods)
        {
            Mods.Add(nm.name, nm.mod);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
