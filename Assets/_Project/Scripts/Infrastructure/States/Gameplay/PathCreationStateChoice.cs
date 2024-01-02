namespace _Project.Scripts.Infrastructure.States.Gameplay
{
    public class PathCreationStateChoice
    {
        public enum PathCreationType
        {
            Manual,
            Automatic
        }
        
        public PathCreationType Type { get; set; }
    }
}