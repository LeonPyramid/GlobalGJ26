using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// A class storing all interactable objects on the scene
/// </summary>
public class ObjectManager: Utils.Singleton.Singleton<ObjectManager>
{
    [SerializeField] List<PlayerInteraction> bins = new List<PlayerInteraction>();
    [SerializeField] List<PlayerInteraction> pnj = new List<PlayerInteraction>();

    [SerializeField] int nbBinsActive;
    [SerializeField] int nbPnjActive;

    private void Awake()
    {
        
        List<PlayerInteraction> binsCp = bins.ToList();
        for (int i = 0; i < nbBinsActive; i ++){
            int pos = Random.Range(0, binsCp.Count);
            if (i == 0){
                ((Bin)binsCp[pos].action).SetHasKey();
            }
            binsCp[pos].SetUnblocked();
            binsCp.RemoveAt(pos);
            if (binsCp.Count == 0)
                break;
        }
        foreach (var obj in binsCp){
            obj.SetBLocked();
        }

        List<PlayerInteraction> pnjCp = pnj.ToList();
        for (int i = 0; i < nbPnjActive; i ++){
            int pos = Random.Range(0, pnjCp.Count);
            pnjCp[pos].SetUnblocked();
            pnjCp.RemoveAt(pos);
            if (pnjCp.Count == 0)
                break;
        }
        foreach (var obj in pnjCp){
            obj.SetBLocked();
        }

    }

}
