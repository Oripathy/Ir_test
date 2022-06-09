using DefaultNamespace;

namespace Miner.States
{
    internal abstract class BaseState
    {
        private protected IMinerView _view;
        private protected MinerModel _model;
        private protected MinerPresenter _presenter; 
        private protected UpdateHandler _updateHandler;

        protected BaseState(IMinerView view, MinerModel model, UpdateHandler updateHandler, MinerPresenter presenter)
        {
            _view = view;
            _model = model;
            _updateHandler = updateHandler;
            _presenter = presenter;
        }

        public virtual void OnEnter()
        {
            
        }

        public virtual void OnExit()
        {
            
        }

        public virtual void Update()
        {
            SwitchState();
        }

        protected abstract void SwitchState();
    }
}