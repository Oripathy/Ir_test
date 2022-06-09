using DefaultNamespace;
using Spine;

namespace Miner.States
{
    internal class ReceiveDamageState : BaseState
    {
        private TrackEntry _entry;
        private string _animName = "Damage";
        public ReceiveDamageState(IMinerView view, MinerModel model, UpdateHandler updateHandler,
            MinerPresenter presenter) : base(view, model, updateHandler, presenter)
        {
        }

        public override void OnEnter() =>
            _entry = _view.SkeletonAnimation.AnimationState.SetAnimation(0, _animName, false); 

        protected override void SwitchState()
        {
            if (_entry.IsComplete)
                _presenter.SwitchState<IdleState>();
        }
    }
}