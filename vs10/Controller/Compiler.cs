using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;

namespace SoftGPL.vs10.Controller
{

    /// <summary>
    /// Controls interaction of the google closure compiler and other components via
    /// the Event Bus
    /// </summary>
    /// 
    sealed public class Compiler
    {

        /// <summary>
        /// Instance of event bus
        /// </summary>
        private EventBus _EventBus;

        /// Need to keep a local reference to the event managers to keep them in scope
        /// and from being garbage collected.
        private BuildEvents _BuildEvents = null;

        /// <summary>
        /// Instance of the JavaScirpt compiler
        /// </summary>
        private jsCompiler.Core.Compiler _jsCompiler = null;

        /// <summary>
        /// Build action cache for later use and firing off the build event.
        /// </summary>
        private EnvDTE.vsBuildAction _Action;


        public Compiler(EventBus eventBus)
        {
            _EventBus = eventBus;
            _BuildEvents = _EventBus.DTE.Events.BuildEvents;
            _BuildEvents.OnBuildBegin += new _dispBuildEvents_OnBuildBeginEventHandler(BuildEvents_OnBuildBegin);
            _BuildEvents.OnBuildProjConfigBegin += new _dispBuildEvents_OnBuildProjConfigBeginEventHandler(BuildEvents_OnBuildProjConfigBegin);
        }


        private void BuildEvents_OnBuildBegin(EnvDTE.vsBuildScope Scope, EnvDTE.vsBuildAction Action)
        {
            _Action = Action;
        }


        private void BuildEvents_OnBuildProjConfigBegin(string Project, string ProjectConfig, string Platform, string SolutionConfig)
        {
            if (_Action == vsBuildAction.vsBuildActionBuild || _Action == vsBuildAction.vsBuildActionRebuildAll)
            {
                Compile();
            }
        }


        public void Compile()
        {
            if (_EventBus.CompilerStatus.Status != CompilerStatus.EStatus.Ready)
            {
                _EventBus.CompilerStatus.Message("Unable to start another compilation because one is already in progress");
                return;
            }


            // If the compiler type does match global settings, then we gotta create one that
            // matches the settings.
            if (_jsCompiler == null || _jsCompiler.Type != _EventBus.MainViewModel.CompilerType)
            {
                try
                {
                    _jsCompiler = new jsCompiler.Core.Compiler(_EventBus.MainViewModel.CompilerType);
                }
                catch (Exception ex)
                {
                    _EventBus.ErrorList.HandleException(ex);
                    return;
                }
            }


            System.Threading.ThreadPool.QueueUserWorkItem((a) =>
            {
                try
                {
                    // Clear up all internal data
                    _EventBus.ErrorList.Clear();

                    _EventBus.CompilerStatus.Status = CompilerStatus.EStatus.Starting;
                    _EventBus.CompilerStatus.Message(_jsCompiler.Type.ToString());
                    _EventBus.CompilerStatus.Message(_jsCompiler.Version());
                    _EventBus.CompilerStatus.Status = CompilerStatus.EStatus.LoadingFiles;

                    _EventBus.MainViewModel.jsOptions.InputFiles.Clear();
                    _EventBus.MainViewModel.jsOptions.InputFiles.AddRange(_EventBus.DTEHelper.GetJavaScriptFilesFromSolution());

                    // If there are no files to be compiled, then we don't call the compiler
                    if (_EventBus.MainViewModel.jsOptions.InputFiles.Count == 0)
                        return;

                    _EventBus.CompilerStatus.Status = CompilerStatus.EStatus.Started;

                    using (jsCompiler.Core.Result result = _jsCompiler.Compile(_EventBus.MainViewModel.jsOptions))
                    {
                        _EventBus.CompilerStatus.Status = CompilerStatus.EStatus.Stopped;

                        if (result.Success)
                        {
                            String output = String.Format("Success. {0} warning(s)", result.Warnings.Count);
                            _EventBus.CompilerStatus.Message(output);

                            if ( _EventBus.MainViewModel.NewDocument == true )
                                _EventBus.Document.New(result.Output);
                        }
                        else
                        {
                            String output = String.Format("Failed. {0} error(s) {1} warning(s)", result.Errors.Count, result.Warnings.Count);
                            _EventBus.CompilerStatus.Message(output);
                        }

                        if (result.Errors.Count > _EventBus.MainViewModel.MaxErrors)
                        {
                            _EventBus.CompilerStatus.Message(String.Format("Too many errors; only displaying first {0}",_EventBus.MainViewModel.MaxErrors));
                            _EventBus.ErrorList.AddErrors(result.Errors.GetRange(0, 1000));
                        }
                        else
                        {
                            _EventBus.ErrorList.AddErrors(result.Errors);
                        }

                        if (result.Warnings.Count > _EventBus.MainViewModel.MaxWarnings)
                        {
                            _EventBus.CompilerStatus.Message(String.Format("Too many warnings; only displaying first {0}", _EventBus.MainViewModel.MaxWarnings));
                            _EventBus.ErrorList.AddWarnings(result.Warnings.GetRange(0, 1000));
                        }
                        else
                        {
                            _EventBus.ErrorList.AddWarnings(result.Warnings);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Clear up all internal data
                    _EventBus.ErrorList.Clear();
                    _EventBus.ErrorList.HandleException(ex);
                    _EventBus.CompilerStatus.Message(String.Format("Failure: {0}", ex.Message));
                }
                finally
                {
                    _EventBus.CompilerStatus.Status = CompilerStatus.EStatus.Ready;
                }
            });

        }


    }

}

