namespace RestExample.Infrastructure
{
    public class PagedResults<T>
    {
        public IEnumerable<T> Results { get; init; }

        public long? TotalCount { get; init; }
    }
}
