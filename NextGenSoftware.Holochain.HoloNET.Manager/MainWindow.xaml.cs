using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using NextGenSoftware.Holochain.HoloNET.Client;
using NextGenSoftware.Holochain.HoloNET.Manager.Enums;
using NextGenSoftware.Holochain.HoloNET.Manager.Interfaces;
using NextGenSoftware.Holochain.HoloNET.Manager.Managers;
using NextGenSoftware.Holochain.HoloNET.Manager.Models;
using NextGenSoftware.Holochain.HoloNET.Manager.UserControls;

namespace NextGenSoftware.Holochain.HoloNET.Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.Unloaded += MainWindow_Unloaded;
            this.Closing += MainWindow_Closing;
        }

        private void InitUI()
        {
            ucConductorSettingsPopup.txtAdminURI.Text = HoloNETManager.Instance.HoloNETClientAdmin.HoloNETDNA.HolochainConductorAdminURI;
            ucConductorSettingsPopup.chkAutoStartConductor.IsChecked = HoloNETManager.Instance.HoloNETClientAdmin.HoloNETDNA.AutoStartHolochainConductor;
            ucConductorSettingsPopup.chkAutoShutdownConductor.IsChecked = HoloNETManager.Instance.HoloNETClientAdmin.HoloNETDNA.AutoShutdownHolochainConductor;
            ucConductorSettingsPopup.chkShowConductorWindow.IsChecked = HoloNETManager.Instance.HoloNETClientAdmin.HoloNETDNA.ShowHolochainConductorWindow;
            ucConductorSettingsPopup.txtSecondsToWaitForConductorToStart.Text = HoloNETManager.Instance.HoloNETClientAdmin.HoloNETDNA.SecondsToWaitForHolochainConductorToStart.ToString();
        }

        private void ShowStatusMessage(string message, StatusMessageType type = StatusMessageType.Information, bool showSpinner = false, ucHoloNETEntry ucHoloNETEntry = null)
        {
            txtStatus.Text = $"{message}";

            switch (type)
            {
                case StatusMessageType.Success:
                    txtStatus.Foreground = Brushes.LightGreen;
                    break;

                case StatusMessageType.Information:
                    txtStatus.Foreground = Brushes.White;
                    break;

                case StatusMessageType.Error:
                    txtStatus.Foreground = Brushes.LightSalmon;
                    break;

                case StatusMessageType.Warning:
                    txtStatus.Foreground = Brushes.Yellow;
                    break;
            }

            if (showSpinner)
                spinner.Visibility = Visibility.Visible;
            else
                spinner.Visibility = Visibility.Hidden;

            if (ucHoloNETEntry != null)
                ucHoloNETEntry.ShowStatusMessage(message, type, showSpinner);

            sbAnimateStatus.Begin();
        }

        private void LogMessage(string message)
        {
            lstOutput.Items.Add(message);

            if (VisualTreeHelper.GetChildrenCount(lstOutput) > 0)
            {
                Border border = (Border)VisualTreeHelper.GetChild(lstOutput, 0);
                ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToBottom();
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            HoloNETManager.Instance.OnLogMessage += HoloNETManager_OnLogMessage;
            HoloNETManager.Instance.OnStatusMessage += HoloNETManager_OnStatusMessage;
            HoloNETManager.Instance.OnNumberOfClientConnectionsChanged += HoloNETManager_OnNumberOfClientConnectionsChanged;
            HoloNETManager.Instance.OnInstalledAppsChanged += HoloNETManager_OnInstalledAppsChanged;
            HoloNETManager.Instance.OnHoloNETManagerBooted += Instance_OnHoloNETManagerBooted;

            HoloNETManager.Instance.BootHoloNETManager();
            InitUI();
        }

        private void Instance_OnHoloNETManagerBooted(object sender)
        {
            btnAddAgentInfo.IsEnabled = true;
            btnDeleteCloneCell.IsEnabled = true;
            btnDumpNetworkMetrics.IsEnabled = true;
            btnDumpNetWorkStats.IsEnabled = true;
            btnDumpState.IsEnabled = true;
            btnFullDump.IsEnabled = true;
            btnGetAgentInfo.IsEnabled = true;
            btnGetDNADefinition.IsEnabled = true;
            btnGetStorageInfo.IsEnabled = true;
            btnGraftRecords.IsEnabled = true;
            btnHoloNETCollection.IsEnabled = true;
            btnHoloNETEntry.IsEnabled = true;
            btnInstall.IsEnabled = true;
            btnListAttachedInterfaces.IsEnabled = true;
            btnListCellIds.IsEnabled = true;
            btnListDNAs.IsEnabled = true;
            btnReboot.IsEnabled = true;
            btnRefreshInstalledhApps.IsEnabled = true;
            btnUpdateCoordinators.IsEnabled = true;

            mnuReboot.IsEnabled = true;
            mnuInstall.IsEnabled = true;
            mnuGetDNADefinition.IsEnabled = true;
            mnuUpdateAgentInfo.IsEnabled = true;
            mnuUpdateDNADefintion.IsEnabled = true;
            mnuDeleteClonedCell.IsEnabled = true;
            mnuDumpFullState.IsEnabled = true;
            mnuDumpNetworkMetrics.IsEnabled = true;
            mnuDumpNetWorkStats.IsEnabled= true;
            mnuDumpState.IsEnabled = true;
            mnuGetAgentInfo.IsEnabled = true;
            mnuGetStorageInfo.IsEnabled = true;
            mnuGraftRecords.IsEnabled = true;
            mnuHoloNETCollection.IsEnabled= true;
            mnuHoloNETEntry.IsEnabled= true;
            mnuListAttachedInterfaces.IsEnabled= true;
            mnuListCellIds.IsEnabled= true;
            mnuListDNAs.IsEnabled= true;
            mnuRefreshApps.IsEnabled= true;
            mnuUpdateCoordinates.IsEnabled= true;
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            HoloNETManager.Instance.ShutdownHoloNETManager();
            HoloNETManager.Instance.OnLogMessage -= HoloNETManager_OnLogMessage;
            HoloNETManager.Instance.OnStatusMessage -= HoloNETManager_OnStatusMessage;
            HoloNETManager.Instance.OnNumberOfClientConnectionsChanged -= HoloNETManager_OnNumberOfClientConnectionsChanged;
            HoloNETManager.Instance.OnInstalledAppsChanged -= HoloNETManager_OnInstalledAppsChanged;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            ucExitPopup.Visibility = Visibility.Visible;
            PopupManager.CurrentPopup = ucExitPopup;
        }

        private void HoloNETManager_OnInstalledAppsChanged(object sender, InstalledAppsEventArgs e)
        {
            //If the UI used does not support binding like WPF/XAML does then you will need to un-comment the line below to manually refresh the grid.
            //gridHapps.ItemsSource = e.InstalledApps;
        }

        private void HoloNETManager_OnNumberOfClientConnectionsChanged(object sender, NumberOfClientConnectionsEventArgs e)
        {
            txtConnections.Text = $"Client Connections: {e.NumberOfConnections}";
            sbAnimateConnections.Begin();
        }

        private void HoloNETManager_OnStatusMessage(object sender, StatusMessageEventArgs e)
        {
            ShowStatusMessage(e.Message, e.Type, e.ShowSpinner, e.ucHoloNETEntry);
        }

        private void HoloNETManager_OnLogMessage(object sender, LogMessageEventArgs e)
        {
            LogMessage(e.Message);
        }

        private void btnInstall_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "hApp files (*.hApp)|*.hApp";

            if (openFileDialog.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(openFileDialog.FileName)) 
                {
                    HoloNETManager.Instance.InstallingAppParams.InstallinghAppPath = openFileDialog.FileName;
                    FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                    string[] parts = fileInfo.Name.Split('.');

                    HoloNETManager.Instance.InstallingAppParams.InstallinghAppName = parts[0];
                    ucInstallAppPopup.txthAppName.Text = HoloNETManager.Instance.InstallingAppParams.InstallinghAppName;
                    ucInstallAppPopup.Visibility = Visibility.Visible;
                }
            }
        }

        private void btnRefreshInstalledhApps_Click(object sender, RoutedEventArgs e)
        {
            HoloNETManager.Instance.ListHapps();
        }

        private void btnListDNAs_Click(object sender, RoutedEventArgs e)
        {
            LogMessage("ADMIN: Listing DNAs...");
            ShowStatusMessage($"Listing DNAs...");
            HoloNETManager.Instance.HoloNETClientAdmin.ListDnas();
        }

        private void btnListCellIds_Click(object sender, RoutedEventArgs e)
        {
            LogMessage("ADMIN: Listing Cell Ids...");
            ShowStatusMessage($"Listing Cell Ids...", StatusMessageType.Information, true);
            HoloNETManager.Instance.HoloNETClientAdmin.ListCellIds();
        }

        private void btnListAttachedInterfaces_Click(object sender, RoutedEventArgs e)
        {
            LogMessage("ADMIN: Listing Attached Interfaces...");
            ShowStatusMessage($"Listing Attached Interfaces...", StatusMessageType.Information, true);
            HoloNETManager.Instance.HoloNETClientAdmin.ListInterfaces();
        }

        private void btnGetAgentInfo_Click(object sender, RoutedEventArgs e)
        {
            if (HoloNETManager.Instance != null && HoloNETManager.Instance.CurrentApp != null)
            {
                LogMessage("ADMIN: Getting Agent Info...");
                ShowStatusMessage($"Getting Agent Info...", StatusMessageType.Information, true);
                HoloNETManager.Instance.HoloNETClientAdmin.GetAgentInfo(HoloNETManager.Instance.CurrentApp.CellId);
            }
            else
            {
                LogMessage("ADMIN: Select A hApp From The Installed hApps List First!");
                ShowStatusMessage($"Select A hApp From The Installed hApps List First!", StatusMessageType.Information);
            }
        }

        // Runs an admin call and logs what came back (these calls have no dedicated result view, and the old sync calls
        // fired results into events nothing listened to, so the user saw nothing).
        private async System.Threading.Tasks.Task RunAdminCallAsync(string what, Func<System.Threading.Tasks.Task<NextGenSoftware.WebSocket.CallBackBaseEventArgs>> call)
        {
            LogMessage($"ADMIN: {what}...");
            ShowStatusMessage($"{what}...", StatusMessageType.Information, true);

            try
            {
                NextGenSoftware.WebSocket.CallBackBaseEventArgs result = await call();

                if (result == null || result.IsError)
                {
                    LogMessage($"ADMIN: {what} failed: {result?.Message}");
                    ShowStatusMessage($"{what} failed: {result?.Message}", StatusMessageType.Error);
                    return;
                }

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                    Error = (s, args) => args.ErrorContext.Handled = true
                });

                if (json.Length > 20000)
                    json = json.Substring(0, 20000) + "\n... (truncated)";

                LogMessage($"ADMIN: {what} done:\n{json}");
                ShowStatusMessage($"{what} done.", StatusMessageType.Success);
            }
            catch (Exception ex)
            {
                LogMessage($"ADMIN: {what} failed: {ex.Message}");
                ShowStatusMessage($"{what} failed: {ex.Message}", StatusMessageType.Error);
            }
        }

        private bool CheckAppSelected()
        {
            if (HoloNETManager.Instance != null && HoloNETManager.Instance.CurrentApp != null)
                return true;

            LogMessage("ADMIN: Select A hApp From The Installed hApps List First!");
            ShowStatusMessage($"Select A hApp From The Installed hApps List First!", StatusMessageType.Information);
            return false;
        }

        private static string PickFile(string title, string filter)
        {
            OpenFileDialog dialog = new OpenFileDialog { Title = title, Filter = filter };
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        private async void btnGetStorageInfo_Click(object sender, RoutedEventArgs e)
        {
            //LogMessage("ADMIN: Coming Soon...");
            //ShowStatusMessage($"Coming Soon....", StatusMessageType.Information);
            await RunAdminCallAsync("Getting Storage Info", async () => await HoloNETManager.Instance.HoloNETClientAdmin.GetStorageInfoAsync());
        }

        private async void btnDumpNetWorkStats_Click(object sender, RoutedEventArgs e)
        {
            // Was the sync DumpNetworkStats(): its result went to an event nothing handled, so nothing was shown.
            //HoloNETManager.Instance.HoloNETClientAdmin.DumpNetworkStats();
            await RunAdminCallAsync("Dumping Network Stats", async () => await HoloNETManager.Instance.HoloNETClientAdmin.DumpNetworkStatsAsync());
        }

        private async void btnDumpNetworkMetrics_Click(object sender, RoutedEventArgs e)
        {
            // Was the sync DumpNetworkMetrics() (result never shown, see above).
            //HoloNETManager.Instance.HoloNETClientAdmin.DumpNetworkMetrics();
            await RunAdminCallAsync("Dumping Network Metrics", async () => await HoloNETManager.Instance.HoloNETClientAdmin.DumpNetworkMetricsAsync());
        }

        private async void btnDumpState_Click(object sender, RoutedEventArgs e)
        {
            // Was the sync DumpState() with no cell (result never shown, see above); now dumps the selected hApp's cell.
            //HoloNETManager.Instance.HoloNETClientAdmin.DumpState();
            if (CheckAppSelected())
                await RunAdminCallAsync("Dumping State", async () => await HoloNETManager.Instance.HoloNETClientAdmin.DumpStateAsync(HoloNETManager.Instance.CurrentApp.CellId));
        }

        private async void btnFullDump_Click(object sender, RoutedEventArgs e)
        {
            // Was the sync DumpFullState() with no cell (result never shown, see above); now dumps the selected hApp's cell.
            //HoloNETManager.Instance.HoloNETClientAdmin.DumpFullState();
            if (CheckAppSelected())
                await RunAdminCallAsync("Dumping Full State", async () => await HoloNETManager.Instance.HoloNETClientAdmin.DumpFullStateAsync(HoloNETManager.Instance.CurrentApp.CellId));
        }

        //private void btnGetCellId_Click(object sender, RoutedEventArgs e)
        //{
        //    if (HoloNETManager.Instance != null && HoloNETManager.Instance.CurrentApp != null)
        //    {
        //        LogMessage("ADMIN: Getting Cell Id...");
        //        ShowStatusMessage($"Getting Cell Id....", StatusMessageType.Information, true);
        //        HoloNETManager.Instance.HoloNETClientAdmin.GetCellId();
        //    }
        //}

        private void btnGetDNADefinition_Click(object sender, RoutedEventArgs e)
        {
            if (HoloNETManager.Instance != null && HoloNETManager.Instance.CurrentApp != null)
            {
                LogMessage("ADMIN: Getting DNA Defintion...");
                ShowStatusMessage($"Getting DNA Defintion....", StatusMessageType.Information, true);
                HoloNETManager.Instance.HoloNETClientAdmin.GetDnaDefinition(HoloNETManager.Instance.CurrentApp.DnaHash);
            }
            else
            {
                LogMessage("ADMIN: Select A hApp From The Installed hApps List First!");
                ShowStatusMessage($"Select A hApp From The Installed hApps List First!", StatusMessageType.Information);
            }
        }

        private async void btnAddAgentInfo_Click(object sender, RoutedEventArgs e)
        {
            //LogMessage("ADMIN: Coming Soon...");
            //ShowStatusMessage($"Coming Soon....", StatusMessageType.Information);

            // Agent infos as Holochain encodes them (e.g. copied from another conductor's Get Agent Info), one per line.
            string path = PickFile("Agent infos to add (one encoded agent info per line)", "Text files (*.txt;*.json)|*.txt;*.json|All files (*.*)|*.*");
            if (path != null)
            {
                string[] infos = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.Trim()).ToArray();
                await RunAdminCallAsync($"Adding {infos.Length} Agent Info(s)", async () => await HoloNETManager.Instance.HoloNETClientAdmin.AddAgentInfoAsync((System.Collections.Generic.IEnumerable<string>)infos));
            }

            //LogMessage("ADMIN: Adding Agent Info...");
            //ShowStatusMessage($"Adding Agent Info....", StatusMessageType.Information, true);
            //HoloNETManager.Instance.HoloNETClientAdmin.AddAgentInfo();
        }

        private async void btnDeleteCloneCell_Click(object sender, RoutedEventArgs e)
        {
            if (HoloNETManager.Instance != null && HoloNETManager.Instance.CurrentApp != null)
            {
                //LogMessage("ADMIN: Coming Soon...");
                //ShowStatusMessage($"Coming Soon....", StatusMessageType.Information);

                // Holochain only deletes a clone cell that has been disabled first; the conductor's error says so if not.
                if (MessageBox.Show($"Delete the clone cell {HoloNETManager.Instance.CurrentApp.DnaHash} of '{HoloNETManager.Instance.CurrentApp.Name}'? This can't be undone.", "Delete Clone Cell", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    await RunAdminCallAsync("Deleting Clone Cell", async () => await HoloNETManager.Instance.HoloNETClientAdmin.DeleteCloneCellAsync(HoloNETManager.Instance.CurrentApp.Name, HoloNETManager.Instance.CurrentApp.CellId));

                //LogMessage("ADMIN: Deleting Clone Cell...");
                //ShowStatusMessage($"Deleting Clone Cell....", StatusMessageType.Information, true);
                //HoloNETManager.Instance.HoloNETClientAdmin.DeleteCloneCell(HoloNETManager.Instance.CurrentApp.Name, HoloNETManager.Instance.CurrentApp.AgentPubKey, HoloNETManager.Instance.CurrentApp.DnaHash);
            }
            else
            {
                LogMessage("ADMIN: Select A hApp From The Installed hApps List First!");
                ShowStatusMessage($"Select A hApp From The Installed hApps List First!", StatusMessageType.Information);
            }
        }

        private async void btnGraftRecords_Click(object sender, RoutedEventArgs e)
        {
            if (HoloNETManager.Instance != null && HoloNETManager.Instance.CurrentApp != null)
            {
                //LogMessage("ADMIN: Coming Soon...");
                //ShowStatusMessage($"Coming Soon....", StatusMessageType.Information);

                // A JSON array of records (e.g. saved from Dump State) to graft onto the selected cell's source chain, validated.
                string path = PickFile("Records to graft (JSON array)", "JSON files (*.json)|*.json|All files (*.*)|*.*");
                if (path != null)
                {
                    object[] records = Newtonsoft.Json.JsonConvert.DeserializeObject<object[]>(File.ReadAllText(path)) ?? new object[0];
                    await RunAdminCallAsync($"Grafting {records.Length} Record(s)", async () => await HoloNETManager.Instance.HoloNETClientAdmin.GraftRecordsAsync(HoloNETManager.Instance.CurrentApp.CellId, true, records));
                }

                //LogMessage("ADMIN: Grafting Records...");
                //ShowStatusMessage($"Grafting Records....", StatusMessageType.Information, true);
                //HoloNETManager.Instance.HoloNETClientAdmin.GraftRecords(HoloNETManager.Instance.CurrentApp.CellId, true, new object[1]);
            }
            else
            {
                LogMessage("ADMIN: Select A hApp From The Installed hApps List First!");
                ShowStatusMessage($"Select A hApp From The Installed hApps List First!", StatusMessageType.Information);
            }
        }

        private async void btnUpdateCoordinators_Click(object sender, RoutedEventArgs e)
        {
            if (HoloNETManager.Instance != null && HoloNETManager.Instance.CurrentApp != null)
            {
                //LogMessage("ADMIN: Coming Soon...");
                //ShowStatusMessage($"Coming Soon....", StatusMessageType.Information);

                // Hot-swap the selected DNA's coordinator zomes from a coordinator bundle (integrity zomes stay as they are).
                string path = PickFile("Coordinator bundle", "Coordinator bundles (*.dna;*.bundle)|*.dna;*.bundle|All files (*.*)|*.*");
                if (path != null)
                    await RunAdminCallAsync("Updating Coordinator Zomes", async () => await HoloNETManager.Instance.HoloNETClientAdmin.UpdateCoordinatorsAsync(HoloNETManager.Instance.CurrentApp.DnaHash, path));

                //LogMessage("ADMIN: Updating Coordinator Zomes...");
                //ShowStatusMessage($"Updating Coordinator Zomes....", StatusMessageType.Information, true);
                ////HoloNETManager.Instance.HoloNETClientAdmin.UpdateCoordinators(HoloNETManager.Instance.HoloNETClientAdmin.ConvertHoloHashToBytes(HoloNETManager.Instance.CurrentApp.DnaHash), "");
                //HoloNETManager.Instance.HoloNETClientAdmin.UpdateCoordinators(HoloNETManager.Instance.CurrentApp.CellId[0], "");
            }
            else
            {
                LogMessage("ADMIN: Select A hApp From The Installed hApps List First!");
                ShowStatusMessage($"Select A hApp From The Installed hApps List First!", StatusMessageType.Information);
            }
        }

        private void btnShowLog_Click(object sender, RoutedEventArgs e)
        {
            //Process.Start("notepad.exe", "Logs\\HoloNET.log");
            //Process.Start("notepad.exe", $"{HoloNETDNAManager.HoloNETDNA.LogPath}\\{HoloNETDNAManager.HoloNETDNA.LogFileName}");
            //Process.Start($"{HoloNETDNAManager.HoloNETDNA.LogPath}");
            Process.Start("explorer.exe", $"{HoloNETDNAManager.HoloNETDNA.LogPath}");
        }

        private void btnClearLog_Click(object sender, RoutedEventArgs e)
        {
            lstOutput.Items.Clear();
        }

        private void btnReboot_Click(object sender, RoutedEventArgs e)
        {
            LogMessage("Rebooting...");
            ShowStatusMessage($"Rebooting...");
            HoloNETManager.Instance.Reboot();
        }

        private void btnHoloNETEntry_Click(object sender, RoutedEventArgs e)
        {
            PopupManager.CurrentPopup = ucHoloNETEntryPopup;
            ucHoloNETEntryPopup.Visibility = Visibility.Visible;
        }

        private void btnHoloNETCollection_Click(object sender, RoutedEventArgs e)
        {
            PopupManager.CurrentPopup = ucHoloNETCollectionPopup;
            ucHoloNETCollectionPopup.Visibility = Visibility.Visible;
        }

        private void btnEnableApp_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source != null)
            {
                Button? button = e.Source as Button;

                if (button != null)
                {
                    IInstalledApp? app = button.DataContext as IInstalledApp;

                    if (app != null)
                    {
                        LogMessage($"ADMIN: Enabling {app.Name} hApp...");
                        ShowStatusMessage($"Enabling {app.Name} hApp...", StatusMessageType.Information, true);
                        HoloNETManager.Instance.HoloNETClientAdmin.EnablelApp(app.Name);
                    }
                }
            }
        }

        private void btnDisableApp_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source != null)
            {
                Button? button = e.Source as Button;

                if (button != null)
                {
                    InstalledApp? app = button.DataContext as InstalledApp;

                    if (app != null)
                    {
                        LogMessage($"ADMIN: Disabling {app.Name} hApp...");
                        ShowStatusMessage($"Disabling {app.Name} hApp...", StatusMessageType.Information, true);
                        HoloNETManager.Instance.HoloNETClientAdmin.DisableApp(app.Name);
                    }
                }
            }
        }

        private void btnUninstallApp_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source != null)
            {
                Button? button = e.Source as Button;

                if (button != null)
                {
                    InstalledApp? app = button.DataContext as InstalledApp;

                    if (app != null)
                    {
                        LogMessage($"ADMIN: Uninstalling {app.Name} hApp...");
                        ShowStatusMessage($"Uninstalling {app.Name} hApp...", StatusMessageType.Information, true);
                        HoloNETManager.Instance.HoloNETClientAdmin.UninstallApp(app.Name);
                    }
                }
            }
        }

        private void btnDisconnectClient_Click(object sender, RoutedEventArgs e)
        {
            IInstalledApp app = gridHapps.SelectedItem as IInstalledApp;

            if (app != null)
            {
                Dispatcher.InvokeAsync(async () =>
                {
                    if (await HoloNETManager.Instance.DisconnectAsync(app))
                    {
                        Button btnDisconnectClient = sender as Button;

                        if (btnDisconnectClient != null)
                            btnDisconnectClient.IsEnabled = false;
                    }
                });
            }
        }

        private void btnCallZomeFunction_Click(object sender, RoutedEventArgs e)
        {
            PopupManager.CurrentPopup = ucCallZomePopup;
            ucCallZomePopup.Visibility = Visibility.Visible;
        }

        private void btnViewDataEntries_Click(object sender, RoutedEventArgs e)
        {
            PopupManager.CurrentPopup = ucHoloNETEntryAndCollectionSharedPopup;
            ucHoloNETEntryAndCollectionSharedPopup.Visibility = Visibility.Visible;
        }

        private void gridLog_LayoutUpdated(object sender, EventArgs e)
        {
            if (gridLog.ActualHeight - 60 > 0)
                lstOutput.Height = gridLog.ActualHeight - 60;
        }

        private void chkShowDetailedMessages_Checked(object sender, RoutedEventArgs e)
        {
            HoloNETManager.Instance.ShowDetailedLogMessages = true;
        }

        private void chkShowDetailedMessages_Unchecked(object sender, RoutedEventArgs e)
        {
            HoloNETManager.Instance.ShowDetailedLogMessages = false;
        }

        private void gridHapps_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (PopupManager.CurrentPopup == null)
                HoloNETManager.Instance.CurrentApp = gridHapps.SelectedItem as InstalledApp;
        }

        private void btnHideButtons_Click(object sender, RoutedEventArgs e)
        {
           
            //animAnimateButtonsRow.From = rowDefButtons.Height; //stkpnlButtons.ActualHeight;
            //animAnimateButtonsRow.To = new GridLength(0);
            //sbAnimateButtonsRow.Begin();

            rowDefButtons.Height = new GridLength(0);
        }

        private void lblHeading_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ////animAnimateButtonsRowExpand.From = rowDefButtons.Height; //stkpnlButtons.ActualHeight;
            ////animAnimateButtonsRowExpand.To = new GridLength(0);
            
            //if (rowDefButtons.Height.Value == 0)
            //    sbAnimateButtonsRowExpand.Begin();
        }

        private void shkShowAdvancedAdminTools_Checked(object sender, RoutedEventArgs e)
        {
            btnGetStorageInfo.Visibility = Visibility.Visible;
            btnGetDNADefinition.Visibility = Visibility.Visible;
            btnAddAgentInfo.Visibility = Visibility.Visible;
            btnDeleteCloneCell.Visibility = Visibility.Visible;
            btnGraftRecords.Visibility = Visibility.Visible;
            btnUpdateCoordinators.Visibility = Visibility.Visible;
            btnDumpNetWorkStats.Visibility = Visibility.Visible;
            btnDumpNetworkMetrics.Visibility = Visibility.Visible;
            btnDumpState.Visibility = Visibility.Visible;
            btnFullDump.Visibility = Visibility.Visible;

            rowDefButtons.Height = new GridLength(180);
            rowDefButtons.MaxHeight = 180;
            //animAnimateButtonsRowExpand.From = rowDefButtons.Height; //stkpnlButtons.ActualHeight;
            //animAnimateButtonsRowExpand.To = new GridLength(180);
            //sbAnimateButtonsRowExpand.Begin();
        }

        private void shkShowAdvancedAdminTools_Unchecked(object sender, RoutedEventArgs e)
        {
            btnGetStorageInfo.Visibility = Visibility.Collapsed;
            btnGetDNADefinition.Visibility = Visibility.Collapsed;
            btnAddAgentInfo.Visibility = Visibility.Collapsed;
            btnDeleteCloneCell.Visibility = Visibility.Collapsed;
            btnGraftRecords.Visibility = Visibility.Collapsed;
            btnUpdateCoordinators.Visibility = Visibility.Collapsed;
            btnDumpNetWorkStats.Visibility = Visibility.Collapsed;
            btnDumpNetworkMetrics.Visibility = Visibility.Collapsed;
            btnDumpState.Visibility = Visibility.Collapsed;
            btnFullDump.Visibility = Visibility.Collapsed;

            rowDefButtons.Height = new GridLength(100);
            rowDefButtons.MaxHeight = 100;
            //animAnimateButtonsRowExpand.From = rowDefButtons.Height; //stkpnlButtons.ActualHeight;
            //animAnimateButtonsRowExpand.To = new GridLength(100);
            //sbAnimateButtonsRowExpand.Begin();
        }


        private void mnuOpenDna_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NextGenSoftware\\HoloNET\\HoloNETDNA.json"));
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            ucExitPopup.Visibility = Visibility.Visible;
            PopupManager.CurrentPopup = ucExitPopup;
        }

        private void mnuUpdateDNADefintion_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnuUpdaeAgentInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnuAbout_Click(object sender, RoutedEventArgs e)
        {
            ucAboutPopup.Visibility = Visibility.Visible;
            PopupManager.CurrentPopup = ucAboutPopup;
        }

        private void mnuConnect_Click(object sender, RoutedEventArgs e)
        {
            ucConductorSettingsPopup.Visibility = Visibility.Visible;
            PopupManager.CurrentPopup = ucConductorSettingsPopup;
        }
    }
}