using Game.Entities.Params;
using UI.Components;
using UnityEngine;

namespace UI
{
    public class UIPersonName : Essential.Mono
    {
        [SerializeField] private PersonName _personName;
        [SerializeField] private UIText _uiText;

        public void SetName(string objectName)
        {
            _uiText.SetText(objectName);
        }
    }
}