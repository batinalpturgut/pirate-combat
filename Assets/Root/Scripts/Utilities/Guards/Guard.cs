namespace Root.Scripts.Utilities.Guards
{
    public sealed class Guard
    {
        public static Guard Against { get; } = new Guard();
        private Guard(){ }
    }
}