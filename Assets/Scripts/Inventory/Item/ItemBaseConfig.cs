using UnityEngine;

namespace WinterUniverse
{
    public class ItemBaseConfig : ScriptableObject
    {
        [SerializeField] private string _dispalyName = "Name";
        [SerializeField, TextArea] private string _description = "Description";
        [SerializeField] private Sprite _icon;
        [SerializeField] private GameObject _worldModel;
        [SerializeField, Range(0f, 1000f)] private float _weight = 0.5f;

        public string DisplayName => _dispalyName;
        public string Description => _description;
        public Sprite Icon => _icon;
        public GameObject WorldModel => _worldModel;
        public float Weight => _weight;
    }
}