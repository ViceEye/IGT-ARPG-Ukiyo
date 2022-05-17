using System.Collections.Generic;
using Ukiyo.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Ukiyo.UI.Interface
{
    public class InputTips : MonoBehaviour
    {
        public static InputTips Instance;

        private void Start()
        {
            if (Instance == null)
                Instance = this;
        }

        public List<Image> _cdMasks = new List<Image>();
        
        public void RunCdMask(EnumKeys type, float cd)
        {
            int key = (int) type;
            _cdMasks[key].fillAmount = 1.0f;
            StartCoroutine(Utils.Filling(_cdMasks[key], 0.0f, cd, 0.0f));
        }
    }

    public enum EnumKeys
    {
        Equip,
        Inventory,
        Attack,
        Ultimate
    }
}