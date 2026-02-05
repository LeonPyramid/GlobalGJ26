using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaskHandler : MonoBehaviour
{
    [SerializeField] List<EquipedMask> equipedMasks;
    private EquipedMask _currentMaskEquiped;
    public EquipedMask CurrentMask => _currentMaskEquiped;

    public Action<MaskEnum> OnNewMask;

    private void Start()
    {
        PreGameUI.OnMaskChanged += OnMaskChanged;
    }

    private void OnMaskChanged(MaskEnum mask)
    {
        foreach(var equipedMask in equipedMasks) {
            if(equipedMask.MaskEnum == mask)
            {
                equipedMask.MaskInPlayerGo.SetActive(true);

                _currentMaskEquiped = equipedMask;
            } 
            else
            {
                equipedMask.MaskInPlayerGo.SetActive(false);
            }
        }

        OnNewMask?.Invoke(mask);
    }

    public bool HasMaskEquiped(MaskEnum maskEnum)
    {
        return _currentMaskEquiped.MaskEnum == maskEnum;
    }

    private void OnDestroy()
    {
        PreGameUI.OnMaskChanged -= OnMaskChanged;
    }
}

[Serializable]
public class EquipedMask
{
    [SerializeField] private MaskEnum maskEnum;
    public MaskEnum MaskEnum => maskEnum;
    [SerializeField] private GameObject maskInPlayerGo;
    public GameObject MaskInPlayerGo => maskInPlayerGo;
}