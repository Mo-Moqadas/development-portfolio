    public enum CPRbaseStates {
        CheckHealthState,
        CallHelpState,
        ClearAirWays,
        CheckVain,
        CutTHeShirt,
        HaertMassageState,
        BreathingMouthState,
        QuizeState,
        EndOfCprSession,
        MenuState,
    }

    public abstract class CPRbaseState {
        /// <summary>
        /// when you enter the state, call this method
        /// </summary>
        /// <param name="sceneMachine"></param>
        public abstract void EnterState (SceneStateMachine sceneMachine);

        /// <summary>
        /// when you update the state, call this method
        /// </summary>
        /// <param name="sceneMachine"></param>
        public abstract void UpdateState (SceneStateMachine sceneMachine);

        /// <summary>
        /// when you exit the state, call this method
        /// </summary>
        /// <param name="sceneMachine"></param>
        public abstract void ExitState (SceneStateMachine sceneMachine);
        
        /// <summary>
        /// to checke which state you are in
        /// </summary>
        /// <returns>the state name</returns>
        public abstract CPRbaseStates GetKeyState ();
    }