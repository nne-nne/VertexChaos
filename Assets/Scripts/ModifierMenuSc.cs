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
    public GameObject[] mods = new GameObject[2];


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
        int t = UnityEngine.Random.Range(0, 12);
        BulletModifier bm = null;
        switch (t)
        {
            case 0:
                bm = new AddDemage();
                break;
            case 1:
                bm = new AddLifeTime();
                break;
            case 2:
                bm = new AddSpeed();
                break;
            case 3:
                bm = new CanonModifier();
                break;
            case 4:
                bm = new DamagingAuraModifier(mods);
                break;
            case 5:
                bm = new ExplosionModifier(mods);
                break;
            case 6:
                bm = new FourWayShootModifier();
                break;
            case 7:
                bm = new HomingBullet();
                break;
            case 8:
                bm = new PierceModifier();
                break;
            case 9:
                bm = new ScatterModifier();
                break;
            case 10:
                bm = new ShieldModifier();
                break;
            case 11:
                bm = new TargetterModifier();
                break;
        }
        /*
        Debug.Log(t);
        ConstructorInfo constructor = t.GetConstructor(Type.EmptyTypes);
        BulletModifier newModifier = (BulletModifier)constructor.Invoke(null);
        cannon.AddBulletModifier(newModifier);*/
        cannon.AddBulletModifier(bm);
        string message = bm.show_message();
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