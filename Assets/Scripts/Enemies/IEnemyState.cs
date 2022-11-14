namespace Enemies
{
    public interface IEnemyState
    {
        public void Execute();
        public void SetContext(EnemyController context);
    }
}