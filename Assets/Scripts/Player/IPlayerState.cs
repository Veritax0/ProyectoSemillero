namespace Player
{
    public interface IPlayerState
    {
        public void Execute();
        public void SetContext(PlayerController context);
    }
}