using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BP.SelectionMenu
{
    [CreateAssetMenu(fileName = "new_selectable", menuName = "Selection/Selectable Item Asset")]
    public class SelectableItemAsset : ScriptableObject
    {
        [SerializeField] private string m_name = "Selectable Item";
        [SerializeField] private Sprite m_menuImage = null;
        [SerializeField] private bool m_unlocked;

        public string Name() { return m_name; }
        public Sprite MenuImage() { return m_menuImage; }

        public bool IsUnlocked() { return m_unlocked; }
        public void IsUnlocked(bool isUnlocked) { m_unlocked = isUnlocked; }
    }
}
