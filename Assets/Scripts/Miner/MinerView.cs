using System;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Miner
{
    internal class MinerView : MonoBehaviour, IMinerView
    {
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        [SerializeField] private Image _pointer;
        [SerializeField] private Slider _healthBar;
        [SerializeField] public AnimationReferenceAsset _idle;
        [SerializeField] public AnimationReferenceAsset _damage;
        [SerializeField] public  AnimationReferenceAsset _attack;

        public SkeletonAnimation SkeletonAnimation => _skeletonAnimation;
        public AnimationReferenceAsset Idle => _idle;
        public Transform Transform => transform;
 
        public MinerView Init(Image pointer, Slider healthBar)
        {
            _pointer = pointer;
            _healthBar = healthBar;
            return this;
        }

        public void UpdateHealth(int health, int maxHealth)
        {
            _healthBar.value = (float) health / maxHealth;
        }

        public void SetPointer(bool isActive) => _pointer.gameObject.SetActive(isActive);

        public void SetUI(bool isActive)
        {
            _pointer.gameObject.SetActive(false);
            _healthBar.gameObject.SetActive(isActive);
        }
    }
}