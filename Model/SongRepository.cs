using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Entity;
using MixCreator.Exception;

namespace MixCreator.Model
{
    public class SongRepository : IRepository<Song>
    {
        public IContextFactory<Song> SongContextFactory { get; set; }

        public IConfig Config { get; set; }

        public SongRepository(IContextFactory<Song> songContextFactory, IConfig config)
        {
            SongContextFactory = songContextFactory;
            Config = config;
        }

        public List<Song> LoadAll()
        {
            using (IContext<Song> context = SongContextFactory.CreateContext(Config))
            {
                context.GetDatabase().EnsureCreated();
                context.GetTable().Load();
                return context.GetTable().OrderBy(item => item.Order).ToList();
            }
        }

        public List<Song> LoadByStatus(Song.SongStatus status)
        {
            using (var context = SongContextFactory.CreateContext(Config))
            {
                return context.GetTable().Where(item =>
                    item.Status == status).OrderBy(item => item.Order).ToList();
            }
        }

        public void Update(Song song)
        {
            using (var context = SongContextFactory.CreateContext(Config))
            {
                context.Update(song);
                context.Save();
            }
        }

        public void UpdateStatus(Song song)
        {
            using (var context = SongContextFactory.CreateContext(Config))
            {
                var oldsong = context.GetTable().FirstOrDefault(item => item.Guid == song.Guid);
                if(oldsong == null) throw new SongNotFoundException();

                oldsong.Status = song.Status;
                context.Update(oldsong);
                context.Save();
            }
        }

        public void UpdateStatusProgress(Song song)
        {
            using (var context = SongContextFactory.CreateContext(Config))
            {
                var oldsong = context.GetTable().FirstOrDefault(item => item.Guid == song.Guid);
                if (oldsong == null) throw new SongNotFoundException();

                oldsong.Progress = song.Progress;
                oldsong.Status = song.Status;
                context.Update(oldsong);
                context.Save();
            }
        }

        public void UpdateStatusNamePathSize(Song song)
        {
            using (var context = SongContextFactory.CreateContext(Config))
            {
                var oldsong = context.GetTable().FirstOrDefault(item => item.Guid == song.Guid);
                if (oldsong == null) throw new SongNotFoundException();

                oldsong.Status = song.Status;
                oldsong.Name = song.Name;
                oldsong.Size = song.Size;
                oldsong.Path = song.Path;
                context.Update(oldsong);
                context.Save();
            }
        }

        public void ChangeOrder(Song song, bool directionDown)
        {
            using (var context = SongContextFactory.CreateContext(Config))
            {
                if (!context.GetTable().Any(entry => entry.Guid == song.Guid))
                {
                    throw new SongNotFoundException();
                }

                var incrementValue = directionDown ? 1 : -1;

                var other = context.GetTable().FirstOrDefault(item => item.Order == song.Order + incrementValue);
                if (other == null) return; // do nothing
                
                song.Order += incrementValue;
                other.Order -= incrementValue;

                context.Update(song);
                context.Update(other);
                context.Save();
            }
        }

        public void AddOrUpdate(Song song)
        {
            using (var context = SongContextFactory.CreateContext(Config))
            {
                var oldsong = context.GetTable().FirstOrDefault(entry => entry.Guid == song.Guid);
                if (oldsong != null)
                {
                    if (oldsong.Url.Equals(song.Url))
                    {
                        // TODO reset song values if URL changed or just reset status?
                        //song.ResetValues();
                        song.Status = Song.SongStatus.Idle;
                    }

                    oldsong.CopyValues(song);
                    context.GetTable().Update(oldsong);
                }
                else
                {
                    int newOrderId = 1 + context.GetTable().Max(item => item.Order);
                    song.Order = newOrderId;
                    context.GetTable().Add(song);
                }

                context.Save();
            }
        }

        public void Remove(Song song)
        {
            using (var context = SongContextFactory.CreateContext(Config))
            {
                if (!context.GetTable().Any(entry => entry.Guid == song.Guid))
                {
                    throw new SongNotFoundException();
                }

                int orderId = song.Order;
                context.GetTable().Remove(song);

                // reorder other entries
                context.GetTable()
                    .Where(item => item.Order > orderId)
                    .ToList()
                    .ForEach(item => item.Order -= 1);

                context.Save();
            }
        }
    }
}
