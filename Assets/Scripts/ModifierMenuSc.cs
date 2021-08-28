using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Enemies;

public class ModifierMenuSc : MonoBehaviour
{
    public GameObject interLevelMenu;
    public LevelsScript LS;
    public List<string> bulletModifierNames;
    public List<Button> buttons;
    public CannonController cannon;
    public GameObject[] mods = new GameObject[2];
    public List<string> bulletModifierMessages;
    public List<string> enemyModifierNames;
    public List<string> enemyModifierMessages;

    private int chosenBulletModifier;
    private int chosenEnemyModifier;

    private List<int> bulletModifierOptions;
    private List<int> enemyModifierOptions;


    void Start()
    {
        bulletModifierOptions = GenerateNaturalNumbers(3);
        enemyModifierOptions = GenerateNaturalNumbers(3);

        for(int i = 0; i < buttons.Count; i++)
        {
            Button button = buttons[i];
            button.onClick.AddListener(delegate { ChooseModifier(i); });
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
        List<int> enemyIndices = PickRandomNaturals(buttons.Count, enemyModifierNames.Count);
        for(int i = 0; i < modifiersIndices.Count; i++)
        {
            string buttonText = bulletModifierNames[modifiersIndices[i]] + "\n\n" + 
                                bulletModifierMessages[modifiersIndices[i]] + "\n\n" +
                                enemyModifierNames[enemyIndices[i]] + "\n\n" +
                                enemyModifierMessages[enemyIndices[i]] + "\n\n";
            buttons[i].GetComponentInChildren<TMP_Text>().text = buttonText;

            bulletModifierOptions[i] = modifiersIndices[i];
            enemyModifierOptions[i] = enemyIndices[i];
        }
    }

    private void ChooseModifier(int i)
    {
        int chosenBulletModifier = bulletModifierOptions[i];
        int chosenEnemyModifier = enemyModifierOptions[i];
        BulletModifier bm = null;
        EnemyModifier em = null;
        switch (chosenBulletModifier)
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
                bm = new AddDemage(); // tu mia�a by� aura
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

        switch(chosenEnemyModifier)
        {
            case 0:
                em = new AddSpeedEnemyModifier();
                break;
            case 1:
                em = new BigBadEnemyModifier();
                break;
            case 2:
                em = new BombDropOnDeath(mods);
                break;
            case 3:
                em = new LowerTimeBetweenShotsModifier();
                break;
            case 4:
                em = new MoreHealthEnemyModifier(); 
                break;
            case 5:
                em = new ShootOnDeath();
                break;
        }

        /*
        Debug.Log(t);
        ConstructorInfo constructor = t.GetConstructor(Type.EmptyTypes);
        BulletModifier newModifier = (BulletModifier)constructor.Invoke(null);
        cannon.AddBulletModifier(newModifier);*/

        cannon.AddBulletModifier(bm);

        foreach (NotSharedPool pool in LS.enemyPools)
        {
            foreach (Transform enemy in pool.gameObject.transform)
            {
                EnemyController ec = enemy.gameObject.GetComponent<EnemyController>();
                ec.AddModifier(em);
            }
        }
        //TODO tu jako� trzeba da� wrogowi ten wybrany modyfikator 'em'

        LevelsScript.StartLevelEvent.Invoke();
        interLevelMenu.SetActive(false);
    }


    void Update()
    {

    }
}
