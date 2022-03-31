using GameCreator.Runtime.Common.Mathematics;

namespace GameCreator.Runtime.Common
{
    public static class Evaluate
    {
        public static float FromString(string expression)
        {
            return Parser.Evaluate(expression);
        }
    }
}
