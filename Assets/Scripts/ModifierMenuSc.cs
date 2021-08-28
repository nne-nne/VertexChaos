using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class ModifierMenuSc : MonoBehaviour
{
    public GameObject interLevelMenu;
    public List<string> bulletModifierNames;
    public List<Button> buttons;
    public CannonController cannon;
    public GameObject[] mods;


    void Start()
    {
        foreach(Button button in buttons)
        {
            button.onClick.AddListener(ChooseModifier);
        }
        LevelsScript.EndLevelEvent.AddListener(InitializeMenu);
    }

    private void InitializeMenu()
    {
        Debug.Log("initializing menu");
        interLevelMenu.SetActive(true);
    }

    private void ChooseModifier()
    {
        Type t = Type.GetType(bulletModifierNames[UnityEngine.Random.Range(0, bulletModifierNames.Count)]);
        ConstructorInfo constructor = t.GetConstructor(Type.EmptyTypes);
        BulletModifier newModifier = (BulletModifier)constructor.Invoke(mods);
        cannon.AddBulletModifier(newModifier);

        LevelsScript.StartLevelEvent.Invoke();
        interLevelMenu.SetActive(false);
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            ChooseModifier();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            cannon.PrintBms();
        }
    }
}
