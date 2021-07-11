namespace NotAnAPI.ReKact.Core.EventArgs
{
    public class WaitEventArgs : System.EventArgs
    {
        public int Time { get; private set; }
        public WaitEventArgs(int time)
        {
            this.Time = time;
        }
    }
}
