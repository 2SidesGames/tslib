namespace TSLib.AI.Behaviour.StateMachines.HFSM
{
    public abstract class TS_HierarchicalState : TS_State
    {
        public TS_HierarchicalState Ancestor { get; private set; }

        public void SetAncestor(TS_HierarchicalState ancestor)
        {
            Ancestor = ancestor; // can be null
        }
    }
}
