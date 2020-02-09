namespace Kernel.Library.Utils
{
    using Kernel.Library.Shared;

    public static class EntityExtension
    {
        public static T WithId<T>(this T value, int id)
            where T : Entity
        {
            typeof(T).GetProperty(nameof(value.Id)).SetValue(value, id);
            return value;
        }
    }
}
