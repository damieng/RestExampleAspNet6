using System.ComponentModel.DataAnnotations;

namespace RestExample.Infrastructure
{
    /// <summary>
    /// Represents an entity within the system that has a unique identity and change tracking.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Unique identifier used to reference this customer.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// When this customer was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// When this customer was updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Generate an ETag for caching purposes.
        /// </summary>
        /// <returns>Opaque string used to uniquely identify an entity at a point in time.</returns>
        public string GetETag()
        {
            return UpdatedAt.ToBinary().ToString("x");
        }
    }
}
