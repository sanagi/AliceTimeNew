namespace InputSupport {
    /// <summary>
    /// Type of InputPhase.
    /// </summary>
    public enum InputPhase
    {
        Began, Stay, Ended, Canceled, Missing
    }

    /// <summary>
    /// Base of InputParam.
    /// </summary>
    public abstract class InputParamBase<T> where T : InputParamBase<T> {
        /*** abstract methods  ***/
        public abstract void UpdateParam(T beforeParam, T afterParam);

        /*** inner params ***/
        protected int id;
        protected InputPhase phase;

        /*** getter, setter ***/
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public InputPhase Phase
        {
            get { return this.phase; }
            set { this.phase = value; }
        }
    }
}
