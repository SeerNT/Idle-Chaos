using System;
using System.Linq;
using UnityEngine;

public class HotkeyController : MonoBehaviour
{
    [SerializeField] private KeyCode[] hotbarAbilitiesKeys;
    [SerializeField] private Transform hotbarSlots;
    [SerializeField] private Transit windowController;

    private readonly KeyCode[] keyCodes = Enum.GetValues(typeof(KeyCode))
                                                 .Cast<KeyCode>()
                                                 .Where(k => ((int)k < (int)KeyCode.Mouse0))
                                                 .ToArray();
    private KeyCode? GetCurrentKeyDown()
    {
        if (!Input.anyKey)
        {
            return null;
        }

        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                return keyCodes[i];
            }
        }
        return null;
    }

    private void Update()
    {
        if(GetCurrentKeyDown() != null)
        {
            for(int i = 0; i < hotbarAbilitiesKeys.Length; i++)
            {
                if(GetCurrentKeyDown() == hotbarAbilitiesKeys[i])
                {
                    if (windowController.isShown[2])
                    {
                        hotbarSlots.GetChild(i).GetComponent<AbilityHotbar>().UseAbility();
                    }
                    break;
                }
            }
        }
    }
}
