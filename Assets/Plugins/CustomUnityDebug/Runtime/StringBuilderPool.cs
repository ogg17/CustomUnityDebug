using System;
using System.Collections.Concurrent;
using System.Text;

namespace Masev.CustomUnityDebug
{
    public static class StringBuilderPool
    {
        private const int DefaultCapacity = 128;
        public const int MaxRetainedCapacity = 4096;
        
        // String Builder Pool
        private static readonly ConcurrentStack<StringBuilder> Pool = new();

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

        public static void Return(StringBuilder builder)
        {
            if (builder == null || builder.Capacity > MaxRetainedCapacity) return;

            builder.Clear();
            Pool.Push(builder);
        }
        
        public static PooledStringBuilder Rent(int capacity = DefaultCapacity) => new(Get(capacity));
    }
    
    public class PooledStringBuilder : IDisposable
    {
        private readonly StringBuilder builder;

        public StringBuilder Builder => builder;
        
        public PooledStringBuilder(StringBuilder builder)
        {
            this.builder = builder;
        }
        
        public void Dispose()
        {
            StringBuilderPool.Return(builder);
        }

        public override string ToString() => builder.ToString();
    }
}
