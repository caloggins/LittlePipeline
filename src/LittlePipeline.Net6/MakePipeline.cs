namespace LittlePipeline.Net6
{
    public static class MakePipeline
    {
        public static Builder<TSubject> ForSubject<TSubject>()
            where TSubject : class
        {
            return new Builder<TSubject>();
        }
    }
}