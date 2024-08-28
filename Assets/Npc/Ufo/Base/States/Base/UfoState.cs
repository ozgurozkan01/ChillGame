namespace Npc.Ufo.Base.States.Base
{
    public class UfoState
    {
        private UfoBase _ufo;

        public virtual void Init(UfoBase ufoInGame)
        {
            _ufo = ufoInGame;
    
        }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }

        
        public UfoBase ufo
        {
            get => _ufo;
            set => _ufo = value;
        }
    }

}