using GradiGen.Enums;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen.App
{
    /// <summary>
    ///     Represents the application's lifetime.
    /// </summary>
    public static class Lifetime
    {
        /// <summary>
        ///     Determines if the application is running. To stop the application, call <see cref="Stop"/>.
        /// </summary>
        public static bool IsRunning { get; private set; }

        /// <summary>
        ///     A check that determines the current system.
        /// </summary>
        public static SystemEnvironment System { get; private set; }

        /// <summary>
        ///     Starts the application, requests the user for run approach if launched in debug.
        /// </summary>
        public static void Start()
        {
            //AnsiConsole.Record();
            System = Environment.OSVersion.Platform switch
            {
                PlatformID.Unix => SystemEnvironment.Unix,
                PlatformID.Win32NT => SystemEnvironment.Windows,
                _ => SystemEnvironment.Other,
            };

            Load("Starting up application... ");

            AnsiConsole.Write(
                new FigletText("GradiGen by Rozen")
                    .Centered()
                    .Color(Color.Orange1));
#if DEBUG
            var logLevel = AnsiConsole.Prompt(
                new SelectionPrompt<RunMode>()
                    .Title("What mode are you launching the application in?")
                    .AddChoices(Enum.GetValues<RunMode>()));
#else
            var logLevel = RunMode.Production;
#endif
            Debugger.IsDebugEnabled = logLevel is RunMode.Debug;

            IsRunning = true;
        }

        /// <summary>
        ///     Stops the application.
        /// </summary>
        public static void Stop()
        {
            Load("Shutting down application... ");

            IsRunning = false;
        }

        private static void Load(string reason)
        {
            AnsiConsole.Progress()
                .Columns(new ProgressColumn[]
                {
                    new TaskDescriptionColumn()
                    {
                        Alignment = Justify.Center
                    },
                    new ProgressBarColumn()
                    {
                        Width = 150,
                    },
                    new PercentageColumn(),
                    new SpinnerColumn(),
                })
                .AutoClear(false)
                .Start(ctx =>
                {
                    var random = new Random(DateTime.Now.Millisecond);
                    var task = ctx.AddTask(reason, autoStart: true);

                    while (!ctx.IsFinished)
                    {
                        task.Increment(12 * random.NextDouble());
                        Thread.Sleep(100);
                    }
                });
        }
    }
}
