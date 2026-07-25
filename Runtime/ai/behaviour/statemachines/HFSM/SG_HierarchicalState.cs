namespace SGLib.AI.Behaviour.StateMachines.HFSM
{
    public abstract class SG_HierarchicalState : SG_State
    {
        public SG_HierarchicalState Ancestor { get; private set; }

        public void SetAncestor(SG_HierarchicalState ancestor)
        {
            Ancestor = ancestor; // can be null
        }
    }
}
