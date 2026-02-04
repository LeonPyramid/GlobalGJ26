using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Singleton;


/// <summary>
/// A class storing all interactable objects on the scene
/// </summary>
public class ObjectManager: Singleton<ObjectManager>
{
    [SerializeField] List<Bin> allBins = new List<Bin>();
    [SerializeField] List<Pnj> allPnjs = new List<Pnj>();

    [SerializeField] int nbBinsActive;
    [SerializeField] int nbPnjActive;

    private void Start()
    {
        InitializeBins();
        InitializePnjs();
    }

    private void InitializeBins()
    {
        var shuffledBins = allBins.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < shuffledBins.Count; i++)
        {
            bool shouldBeActive = i < nbBinsActive;
            shuffledBins[i].SetAvailability(shouldBeActive);

            if (shouldBeActive && i == 0)
            {
                shuffledBins[i].AssignKey();
            }
        }
    }

    private void InitializePnjs()
    {
        var shuffledPnjs = allPnjs.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < shuffledPnjs.Count; i++)
        {
            bool shouldBeActive = i < nbPnjActive;
            shuffledPnjs[i].SetAvailability(shouldBeActive);
        }
    }


}

