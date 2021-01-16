using System;
using System.Text;
using System.Windows;
using MixCreator.Model;
using MixCreator.Provider;
using MixCreator.ViewModel;
using MixCreator.Window;
using SimpleInjector;

namespace MixCreator
{


    static class Program
    {
        [STAThread]
        static void Main()
        {
            var container = Bootstrap();

            // Any additional other configuration, e.g. of your desired MVVM toolkit.

            RunApplication(container);
        }

        private static Container Bootstrap()
        {
            // Create the container as usual.
            var container = new Container();

            // Register your types, for instance:
            container.Register<IConfig, DefaultConfig>();
            container.Register<IManager, Manager>(/*Lifestyle.Singleton*/);
            container.Register<IDownloader, Downloader>();
            container.Register<IChecker, Checker>();
            container.Register<IMerger, Merger>();
            container.Register<IRepository<Song>, SongRepository>();
            container.Register<IContextFactory<Song>, SongContextFactory>();
            container.Register<IProviderFactory, DefaultProviderFactory>();

            // Register your windows and view models:
            container.Register<MainWindow>();
            container.Register<MainViewModel>();

            container.Verify();

            return container;
        }

        private static void RunApplication(Container container)
        {
            /* TODO try-catch in production code */
            //try
            //{
                var app = new App();
                var mainWindow = container.GetInstance<MainWindow>();
                app.Run(mainWindow);
            //}
            //catch (System.Exception)
            //{
            //    //Log the exception and exit
            //}
        }
    }
}
