using DefaultNamespace;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Miner.States
{
    internal class IdleState : BaseState
    {
        private TrackEntry _entry;
        private string _animName = "Idle";
        public IdleState(IMinerView view, MinerModel model, UpdateHandler updateHandler, MinerPresenter presenter) :
            base(view, model, updateHandler, presenter)
        {
        }

        public override void OnEnter() =>
            _entry = _view.SkeletonAnimation.AnimationState.SetAnimation(0, _view.Idle, true);
        

        protected override void SwitchState()
        {
        }
    }
}