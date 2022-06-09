using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Miner
{
    internal interface IMinerView
    {
        public SkeletonAnimation SkeletonAnimation { get; }
        public AnimationReferenceAsset Idle { get; }
        public Transform Transform { get; }


        public void UpdateHealth(int health, int maxHealth);
        public void SetPointer(bool isActive);
        public void SetUI(bool isActive);
    }
}