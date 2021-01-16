using System.Collections.Generic;

namespace MixCreator.Model
{
    public interface IRepository<T>
    {
        List<T> LoadAll();

        List<T> LoadByStatus(Song.SongStatus status);

        /** todo update methods which is intelligent and only updates specified fields **/
        void Update(T song);

        void UpdateStatus(T song);

        void UpdateStatusProgress(T song);

        void UpdateStatusNamePathSize(T song);

        void ChangeOrder(T song, bool directionDown);

        void AddOrUpdate(T song);

        void Remove(T song);
    }
}
