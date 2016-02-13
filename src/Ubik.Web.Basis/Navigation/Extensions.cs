namespace Ubik.Web.Basis.Navigation
{
    public static class Extensions
    {
        private class Sequense
        {
            private int _seed;

            private Sequense(int seed)
            {
                _seed = seed;
            }

            private int Next()
            {
                _seed++;
                return _seed;
            }
        }
    }
}