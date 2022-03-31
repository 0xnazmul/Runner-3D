namespace GameCreator.Runtime.Common
{
    public static class MathUtils
    {
        // INTERPOLATION: -------------------------------------------------------------------------
        
        public static double Lerp(double a, double b, double t)
        {
            return LerpUnclamped(a, b, Clamp01(t));
        }
        
        public static double LerpUnclamped(double a, double b, double t)
        {
            return a + (b - a) * t;
        }
        
        // CLAMP: ---------------------------------------------------------------------------------

        public static double Clamp01(double value)
        {
            return value switch
            {
                < 0 => 0,
                > 1 => 1,
                _ => value
            };
        }
    }
}