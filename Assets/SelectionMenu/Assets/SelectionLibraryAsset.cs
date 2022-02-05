using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;

namespace BP.SelectionMenu
{
    [CreateAssetMenu(fileName ="new_selectionLib",menuName ="Selection/Selection Library Asset")]
    public class SelectionLibraryAsset : ScriptableObject
    {
        [SerializeField] private SelectableItemAsset[] m_items;
        [SerializeField] private SelectableItemAsset m_defaultItem = null;
        [SerializeField] private VoidGameEvent m_selectionUpdatedEvent = null;
        private SelectableItemAsset m_selectedItem = null;

        public SelectableItemAsset[] Library() { return m_items; }

        public void SetDefaultAsSelectedItem()
        {
            m_defaultItem.IsUnlocked(true);
            SetSelectedItem(m_defaultItem);
        }

        public void SetSelectedItem(SelectableItemAsset selectedItem)
        {
            m_selectedItem = selectedItem;
            m_selectionUpdatedEvent.Raise();
        }

        public SelectableItemAsset SelectedItem() 
        {
            if (!m_selectedItem) { SetDefaultAsSelectedItem(); }
            return m_selectedItem; 
        }
    }
}

