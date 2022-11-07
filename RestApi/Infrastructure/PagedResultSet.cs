namespace RestExample.Infrastructure
{
    /// <summary>
    /// Provides for a paged set of results with any associated metadata.
    /// </summary>
    /// <typeparam name="T">Type of item being paged.</typeparam>
    public class PagedResultSet<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResultSet{T}"/> with the given results and total count.
        /// </summary>
        /// <param name="results">Results to include in this paged result set.</param>
        /// <param name="totalCount">Total count of items in the underlying data set.</param>
        public PagedResultSet(IEnumerable<T> results, long? totalCount)
        {
            Results = results;
            TotalCount = totalCount;
        }

        /// <summary>
        /// Results in this paged result set.
        /// </summary>
        public IEnumerable<T> Results { get; init; }

        /// <summary>
        /// How many items exist within the underlying data set.
        /// </summary>
        public long? TotalCount { get; init; }
    }
}
