namespace Ukiyo.Common.Object
{
    public interface IObject
    {
        void Init();

        void OnSpawn();

        void OnDespawn();
    }
}