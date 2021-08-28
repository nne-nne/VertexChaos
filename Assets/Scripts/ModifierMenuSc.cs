using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModifierMenuSc : MonoBehaviour
{
    public GameObject interLevelMenu;
    public List<string> bulletModifierNames;
    public List<Button> buttons;
    public CannonController cannon;
    public GameObject[] mods = new GameObject[2];
    public List<string> bulletModifierMessages;
    public List<string> enemyModifierNames;
    public List<string> enemyModifierMessages;


    void Start()
    {
        foreach(Button button in buttons)
        {
            button.onClick.AddListener(ChooseModifier);
        }
        LevelsScript.EndLevelEvent.AddListener(InitializeMenu);
    }

    private List<int> GenerateNaturalNumbers(int maxExclusive)
    {
        List<int> naturals = new List<int>();
        for(int i = 0; i < maxExclusive; i++)
        {
            naturals.Add(i);
        }
        return naturals;
    }

    private List<int> PickRandomNaturals(int howMany, int maxExclusive)
    {
        List<int> naturals = GenerateNaturalNumbers(maxExclusive);
        List<int> result = new List<int>();
        for(int i = 0; i < howMany; i++)
        {
            int index = UnityEngine.Random.Range(0, naturals.Count);
            int num = naturals[index];
            result.Add(num);
            naturals.Remove(num);
        }
        return result;
    }

    private void InitializeMenu()
    {
        Debug.Log("initializing menu");
        interLevelMenu.SetActive(true);

        List<int> modifiersIndices = PickRandomNaturals(buttons.Count, bulletModifierNames.Count);
        for(int i = 0; i < modifiersIndices.Count; i++)
        {
            buttons[i].GetComponentInChildren<TMP_Text>().text = bulletModifierNames[modifiersIndices[i]];
        }
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
