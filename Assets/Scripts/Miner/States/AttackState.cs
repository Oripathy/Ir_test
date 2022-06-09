using DefaultNamespace;
using Spine;

namespace Miner.States
{
    internal class AttackState : BaseState
    {
        private string _animName = "PickaxeCharge";
        private TrackEntry _entry;
        private MinerModel _target;

        public AttackState(IMinerView view, MinerModel model, UpdateHandler updateHandler, MinerPresenter presenter) :
            base(view, model, updateHandler, presenter)
        {
        }

        public override void OnEnter()
        {
            _entry = _view.SkeletonAnimation.AnimationState.SetAnimation(0, _animName, false);
            _view.SkeletonAnimation.AnimationState.Event += Attack;
        }

        protected override void SwitchState()
        {
            if (_entry.IsComplete)
                _presenter.SwitchState<IdleState>();
        }

        public override void OnExit() => _model.OnActionDone();
        
        public void StartAttack(MinerModel miner) => _target = miner;

        private void Attack(TrackEntry entry, Spine.Event e) => _target.TakeDamage(_model.Damage);
        
    }
}