using System;
using System.Collections.Concurrent;
using System.Text;

namespace Masev.CustomUnityDebug
{
    /// <summary>
    /// Shared pool for reusing <see cref="StringBuilder"/> instances to minimize GC pressure.
    /// </summary>
    public static class StringBuilderPool
    {
        private const int DefaultCapacity = 128;

        /// <summary>
        /// Maximum capacity (in characters) a builder can have to be returned to the pool.
        /// </summary>
        public const int MaxRetainedCapacity = 4096;

        private static readonly ConcurrentStack<StringBuilder> Pool = new();

        /// <summary>
        /// Retrieves a <see cref="StringBuilder"/> from the pool, growing it when a larger capacity is requested.
        /// </summary>
        /// <param name="capacity">Minimum capacity the returned builder must support.</param>
        /// <returns>A pooled builder ready for use.</returns>
        public static StringBuilder Get(int capacity = DefaultCapacity)
        {
            if (Pool.TryPop(out var builder))
            {
                if (builder.Capacity > MaxRetainedCapacity)
                    return new StringBuilder(capacity);

                if (builder.Capacity < capacity)
                    builder.EnsureCapacity(capacity);

                return builder;
            }
            return new StringBuilder(capacity);
        }

        /// <summary>
        /// Returns a <see cref="StringBuilder"/> to the pool after clearing it. Builders that grew beyond <see cref="MaxRetainedCapacity"/> are discarded.
        /// </summary>
        /// <param name="builder">Builder instance that is no longer needed.</param>
        public static void Return(StringBuilder builder)
        {
            if (builder == null || builder.Capacity > MaxRetainedCapacity) return;

            builder.Clear();
            Pool.Push(builder);
        }
        
        /// <summary>
        /// Rents a disposable wrapper that automatically returns its builder when disposed.
        /// </summary>
        /// <param name="capacity">Minimum capacity required for the underlying builder.</param>
        /// <returns>A wrapper that ensures the builder is returned.</returns>
        public static PooledStringBuilder Rent(int capacity = DefaultCapacity) => new(Get(capacity));
    }
    
    /// <summary>
    /// Disposable wrapper that guarantees a rented <see cref="StringBuilder"/> is returned to the pool.
    /// </summary>
    public class PooledStringBuilder : IDisposable
    {
        private readonly StringBuilder builder;

        /// <summary>
        /// Exposes the wrapped <see cref="StringBuilder"/> for direct access.
        /// </summary>
        public StringBuilder Builder => builder;
        
        /// <summary>
        /// Creates a wrapper around the provided builder.
        /// </summary>
        /// <param name="builder">Pooled builder instance.</param>
        public PooledStringBuilder(StringBuilder builder)
        {
            this.builder = builder;
        }
        
        /// <summary>
        /// Returns the wrapped builder to the pool.
        /// </summary>
        public void Dispose()
        {
            StringBuilderPool.Return(builder);
        }

        /// <inheritdoc />
        public override string ToString() => builder.ToString();
    }
}
