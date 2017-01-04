using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using dnSpy.Contracts.Extension;
using dnSpy.Contracts.Menus;
using dnSpy.Contracts.Output;
using dnSpy.Contracts.Text;

// Creates an Output window text pane where our log messages will go.
// Adds context menu commands.

namespace dnSpy.dnpatch
{
    // Holds an instance of our logger text pane
    static class Logger
    {
        //TODO: Use your own GUID
        public static readonly Guid THE_GUID = new Guid("57616840-99EA-482C-97CE-0435B66AE1EC");

        public static IOutputTextPane Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("Logger hasn't been initialized yet");
                return _instance;
            }
            set
            {
                if (_instance != null)
                    throw new InvalidOperationException("Can't initialize the logger twice");
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _instance = value;
            }
        }
        static IOutputTextPane _instance;

        // This class initializes the Logger property. It gets auto loaded by dnSpy
        [ExportAutoLoaded(Order = double.MinValue)]
        sealed class InitializeLogger : IAutoLoaded
        {
            [ImportingConstructor]
            InitializeLogger(IOutputService outputService)
            {
                Instance = outputService.Create(THE_GUID, "dnpatch");
                Instance.WriteLine("dnpatch initialized!");
            }
        }
    }

    sealed class LogEditorCtxMenuContext
    {
        public readonly IOutputTextPane TextPane;

        public LogEditorCtxMenuContext(IOutputTextPane pane)
        {
            TextPane = pane;
        }
    }
}