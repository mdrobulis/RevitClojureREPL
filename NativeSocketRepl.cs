using System;
using Autodesk.Revit.UI;

namespace RevitClojureRepl
{
    public class NativeSocketRepl:IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            //IExternalEventHandler h = new ExternalEvent();
            //var e = Autodesk.Revit.UI.ExternalEvent.Create(h);

            //e.Raise();

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            throw new NotImplementedException();
        }
    }
}