// WiX Toolset Pills 15mg
// Copyright (C) 2019-2022 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Windows.Threading;
using DustInTheWind.BundleWithGui.Gui.Commands;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace DustInTheWind.BundleWithGui.Gui.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private static Dispatcher dispatcher;
        private readonly CustomBootstrapperApplication bootstrapperApplication;

        private bool isLoading;

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }

        public InstallCommand InstallCommand { get; }

        public UninstallCommand UninstallCommand { get; }

        public ExitCommand ExitCommand { get; }

        public MainViewModel(CustomBootstrapperApplication bootstrapperApplication)
        {
            this.bootstrapperApplication = bootstrapperApplication ?? throw new ArgumentNullException(nameof(bootstrapperApplication));

            dispatcher = Dispatcher.CurrentDispatcher;

            InstallCommand = new InstallCommand(bootstrapperApplication);
            UninstallCommand = new UninstallCommand(bootstrapperApplication);
            ExitCommand = new ExitCommand(bootstrapperApplication);

            this.bootstrapperApplication.PlanBegin += HandlePlanBegin;
            this.bootstrapperApplication.ApplyComplete += HandleApplyComplete;
        }

        private void HandlePlanBegin(object sender, PlanBeginEventArgs e)
        {
            dispatcher.Invoke(() =>
            {
                IsLoading = true;
            });
        }

        private void HandleApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            dispatcher.Invoke(() =>
            {
                IsLoading = false;
            });
        }
    }
}