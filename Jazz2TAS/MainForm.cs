using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Data;

namespace Jazz2TAS
{
    public partial class MainForm : Form
    {
        private string _CurrentProjectPath;
        private Thread _ProcessThread;
        private Process _Process;
        private ManualResetEvent _ProcessThreadExit;
        private ushort _PreviousFrame;
        private ushort _PreviousFinished;
        private ushort _PreviousEpisodeFinished;
        private bool _WaitForFrameReset;
        private int _CurrentHash;
        private int _Index;
        private ushort[] _Positions = new ushort[256];
        private Color _CurrentFrameBackgroundColor = Color.FromArgb(255, 0, 0);
        private Color _TableTickBackgroundColor = Color.FromArgb(192, 192, 192);

        private IntPtr _TASInputsPointer;
        private IntPtr _PositionHistoryPointer;
        private IntPtr _PauseFramePointer;
        private IntPtr _IsPausedPointer;

        public BindingList<Level> Levels => dataGridViewLevels.DataSource as BindingList<Level>;
        public BindingList<Inputs> Inputs => dataGridViewInputs.DataSource as BindingList<Inputs>;
        public Level SelectedLevel => dataGridViewLevels.SelectedRows.Count > 0 ? dataGridViewLevels.SelectedRows[0].DataBoundItem as Level : null;
        public Theme Theme
        {
            get
            {
                var theme = new Theme();
                theme.BackgroundColor = (BackColor.ToArgb() & 0xFFFFFF).ToString("X6");
                theme.TextColor = (labelLevels.ForeColor.ToArgb() & 0xFFFFFF).ToString("X6");
                theme.TableCurrentFrameBackgroundColor = (_CurrentFrameBackgroundColor.ToArgb() & 0xFFFFFF).ToString("X6");
                theme.TableTickBackgroundColor = (_TableTickBackgroundColor.ToArgb() & 0xFFFFFF).ToString("X6");
                theme.TableBackgroundColor = (dataGridViewLevels.BackgroundColor.ToArgb() & 0xFFFFFF).ToString("X6");
                theme.TableGridColor = (dataGridViewLevels.GridColor.ToArgb() & 0xFFFFFF).ToString("X6");
                theme.TableTextColor = (dataGridViewLevels.DefaultCellStyle.ForeColor.ToArgb() & 0xFFFFFF).ToString("X6");
                theme.TableSelectionBackgroundColor = (dataGridViewLevels.DefaultCellStyle.SelectionBackColor.ToArgb() & 0xFFFFFF).ToString("X6");
                theme.TableSelectionTextColor = (dataGridViewLevels.DefaultCellStyle.SelectionForeColor.ToArgb() & 0xFFFFFF).ToString("X6");
                theme.TableHeaderBackgroundColor = (dataGridViewLevels.ColumnHeadersDefaultCellStyle.BackColor.ToArgb() & 0xFFFFFF).ToString("X6");
                theme.TableHeaderTextColor = (dataGridViewLevels.ColumnHeadersDefaultCellStyle.ForeColor.ToArgb() & 0xFFFFFF).ToString("X6");
                return theme;
            }
            set
            {
                BackColor = value.GetBackgroundColor();
                panelBottom.BackColor = BackColor;
                labelLevels.ForeColor = value.GetTextColor();
                labelInputs.ForeColor = labelLevels.ForeColor;
                labelPositionHistory.ForeColor = labelLevels.ForeColor;
                labelInfo.ForeColor = labelLevels.ForeColor;
                _CurrentFrameBackgroundColor = value.GetTableCurrentFrameBackgroundColor();
                _TableTickBackgroundColor = value.GetTableTickBackgroundColor();
                foreach (var dataGridView in new[] { dataGridViewLevels, dataGridViewInputs, dataGridViewPositionHistory })
                {
                    dataGridView.BackgroundColor = value.GetTableBackgroundColor();
                    dataGridView.GridColor = value.GetTableGridColor();
                    dataGridView.DefaultCellStyle.BackColor = dataGridView.BackgroundColor;
                    dataGridView.DefaultCellStyle.ForeColor = value.GetTableTextColor();
                    dataGridView.DefaultCellStyle.SelectionBackColor = value.GetTableSelectionBackgroundColor();
                    dataGridView.DefaultCellStyle.SelectionForeColor = value.GetTableSelectionTextColor();
                    dataGridView.ColumnHeadersDefaultCellStyle.BackColor = value.GetTableHeaderBackgroundColor();
                    dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = value.GetTableHeaderTextColor();
                }
            }
        }

        public MainForm()
        {
            InitializeComponent();

            dataGridViewLevels.AutoGenerateColumns = false;
            dataGridViewLevels.DataSource = new BindingList<Level>();
            dataGridViewInputs.Visible = false;
            dataGridViewInputs.AutoGenerateColumns = false;

            dataGridViewLevels.EnableHeadersVisualStyles = false;
            dataGridViewInputs.EnableHeadersVisualStyles = false;
            dataGridViewPositionHistory.EnableHeadersVisualStyles = false;

            Text = ProductName + " " + ProductVersion;

            _ProcessThreadExit = new ManualResetEvent(false);
            _ProcessThread = new Thread(ProcessThreadMethod);
            _ProcessThread.Start();

            Theme = Theme.DefaultTheme;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            try
            {
                if (_Process == null || _Process.HasExited || _PositionHistoryPointer == IntPtr.Zero)
                    return;

                ushort frame;
                WinApi.ReadProcessMemory(_Process.Handle, _Process.MainModule.BaseAddress + 0x1ADC20, out frame, 4, out int bytesRead);
                WinApi.ReadProcessMemory(_Process.Handle, _PositionHistoryPointer, _Positions, 256, out bytesRead);
                int currentFrame = frame;

                dataGridViewPositionHistory.Rows.Clear();
                for (int i = 0; currentFrame > 0 && i < 64; i++)
                {
                    int index = (currentFrame-- % 64) * 2;
                    int previousIndex = (currentFrame % 64) * 2;
                    int x = _Positions[index];
                    int y = _Positions[index + 1];
                    if (i < 63)
                    {
                        int xSpeed = x - _Positions[previousIndex];
                        int ySpeed = y - _Positions[previousIndex + 1];
                        dataGridViewPositionHistory.Rows.Add(currentFrame, x + " (" + xSpeed + ")", y + " (" + ySpeed + ")");
                    }
                    else
                    {
                        dataGridViewPositionHistory.Rows.Add(currentFrame, x, y);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            SavePrompt();
            _ProcessThreadExit.Set();
            _ProcessThread.Join();
            base.OnClosing(e);
        }

        private void menuItemNew_Click(object sender, EventArgs e)
        {
            SavePrompt();
            dataGridViewLevels.DataSource = new BindingList<Level>();
            dataGridViewInputs.DataSource = null;
            dataGridViewInputs.Visible = false;
            _CurrentProjectPath = null;
        }

        private void menuItemOpen_Click(object sender, EventArgs e)
        {
            SavePrompt();
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "TAS project|*.xml";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var fileStream = new FileStream(dialog.FileName, FileMode.Open))
                        {
                            var serializer = new XmlSerializer(typeof(BindingList<Level>));
                            dataGridViewLevels.DataSource = serializer.Deserialize(fileStream);
                            _CurrentProjectPath = dialog.FileName;
                            HasChanged();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Failed to open TAS project.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void menuItemSave_Click(object sender, EventArgs e) => Save(_CurrentProjectPath);

        private void menuItemSaveAs_Click(object sender, EventArgs e) => Save(null);

        private void dataGridViewLevels_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 1)
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                    e.Graphics.DrawImage(Resource.Up, e.CellBounds.X + e.CellBounds.Width / 2 - 8, e.CellBounds.Y + e.CellBounds.Height / 2 - 8, 16, 16);
                    e.Handled = true;
                }
                else if (e.ColumnIndex == 2)
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                    e.Graphics.DrawImage(Resource.Down, e.CellBounds.X + e.CellBounds.Width / 2 - 8, e.CellBounds.Y + e.CellBounds.Height / 2 - 8, 16, 16);
                    e.Handled = true;
                }
                else if (e.ColumnIndex == 3)
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                    e.Graphics.DrawImage(Resource.Delete, e.CellBounds.X + e.CellBounds.Width / 2 - 8, e.CellBounds.Y + e.CellBounds.Height / 2 - 8, 16, 16);
                    e.Handled = true;
                }
            }
        }

        private void dataGridViewLevels_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < Levels.Count)
            {
                var levelInputs = Levels[e.RowIndex];
                if (e.ColumnIndex == 1)
                {
                    if (e.RowIndex > 0)
                    {
                        Levels[e.RowIndex] = Levels[e.RowIndex - 1];
                        Levels[e.RowIndex - 1] = levelInputs;
                        dataGridViewLevels.Rows[e.RowIndex - 1].Selected = true;
                    }
                }
                else if (e.ColumnIndex == 2)
                {
                    if (e.RowIndex < Levels.Count - 1)
                    {
                        Levels[e.RowIndex] = Levels[e.RowIndex + 1];
                        Levels[e.RowIndex + 1] = levelInputs;
                        dataGridViewLevels.Rows[e.RowIndex + 1].Selected = true;
                    }
                }
                else if (e.ColumnIndex == 3)
                {
                    Levels.Remove(levelInputs);
                }
            }
        }

        private void dataGridViewLevels_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewLevels.SelectedRows.Count > 0)
            {
                var level = dataGridViewLevels.SelectedRows[0].DataBoundItem as Level;
                if (level == null)
                {
                    dataGridViewInputs.Visible = false;
                    dataGridViewInputs.DataSource = null;
                }
                else
                {
                    level.Inputs.Sort();
                    dataGridViewInputs.DataSource = new BindingList<Inputs>(level.Inputs);
                    dataGridViewInputs.Visible = true;
                }
                TransferInputs();
            }
        }

        private void SavePrompt()
        {
            if (HasChanged() && MessageBox.Show("Do you want to save the current project?", "Save?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Save(_CurrentProjectPath);
        }

        private bool HasChanged()
        {
            int newHash = 0;
            if (Levels != null)
            {
                foreach (var level in Levels)
                {
                    if (level.LevelName != null)
                        newHash ^= level.LevelName.GetHashCode();
                    foreach (var inputs in level.Inputs)
                    {
                        newHash += inputs.GetCalculatedHash();
                    }
                }
            }

            if (newHash != _CurrentHash)
            {
                _CurrentHash = newHash;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Save(string filename)
        {
            if (filename == null)
            {
                using (var dialog = new SaveFileDialog())
                {
                    dialog.Filter = "TAS project|*.xml";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        Save(dialog.FileName);
                    }
                }
            }
            else
            {
                try
                {
                    using (var fileStream = new FileStream(filename, FileMode.Create))
                    {
                        var serializer = new XmlSerializer(typeof(BindingList<Level>));
                        serializer.Serialize(fileStream, Levels);
                        _CurrentProjectPath = filename;
                        HasChanged();
                    }
                }
                catch
                {
                    MessageBox.Show("Failed to save the TAS project.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            TransferInputs();
        }

        private void TransferInputs()
        {
            if (SelectedLevel == null || _Process == null || _Process.HasExited)
                return;

            dataGridViewInputs.EndEdit();
            var tasInputs = SelectedLevel.GetJazz2Inputs();
            WinApi.WriteProcessMemory(_Process.Handle, _TASInputsPointer, tasInputs, tasInputs.Length, out int bytesWritten);
        }

        private void InitializeTASFunctions()
        {
            _TASInputsPointer = WinApi.VirtualAllocEx(_Process.Handle, IntPtr.Zero, 0x10000 + 0x100 + 0x04, 0x1000, 0x04);
            TransferInputs();

            _PositionHistoryPointer = _TASInputsPointer + 0x10000;
            WinApi.WriteProcessMemory(_Process.Handle, _PositionHistoryPointer, new byte[0x100], 0x100, out int bytesWritten);

            _PauseFramePointer = _PositionHistoryPointer + 0x100;
            _IsPausedPointer = _Process.MainModule.BaseAddress + 0xf349e;
            WinApi.WriteProcessMemory(_Process.Handle, _PauseFramePointer, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }, 0x04, out bytesWritten);
            WinApi.WriteProcessMemory(_Process.Handle, _Process.MainModule.BaseAddress + 0x3F753, BitConverter.GetBytes((int)_IsPausedPointer), 4, out bytesWritten);
            WinApi.WriteProcessMemory(_Process.Handle, _Process.MainModule.BaseAddress + 0x8C956, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90 }, 5, out bytesWritten);

            byte[] playbackFunctionData = new byte[]
            {
                0x50, // push eax
                0x53, // push ebx
                0x57, // push edi
                0xBF, 0x00, 0x00, 0x00, 0x00, // mov edi,[_TASInputsPointer]
                0xA1, 0x00, 0x00, 0x00, 0x00, // mov eax,[Jazz2.exe+1ADC20]
                0xC1, 0xE0, 0x01, // shl eax,01
                0x8B, 0xD8, // mov ebx,eax
                0x01, 0xC7, // add edi,eax
                0x8B, 0x07, // mov eax,[edi]
                0x66, 0x0B, 0x05, 0x00, 0x00, 0x00, 0x00, // or ax,[Jazz2.exe+1C8AF0]
                0x66, 0xA3, 0x00, 0x00, 0x00, 0x00, // mov [Jazz2.exe+1C8AF0],ax
                0x89, 0x07, // mov [edi],eax
                0xD1, 0xE3, // shl ebx,1
                0x81, 0xE3, 0xFF, 0x00, 0x00, 0x00, // and ebx,000000FF
                0xBF, 0x00, 0x00, 0x00, 0x00, // mov edi,[_HistoryPointer]
                0x01, 0xDF, // add edi,ebx
                0x8B, 0x1D, 0x00, 0x00, 0x00, 0x00, // mov ebx,[Jazz2.exe+1C8572]
                0xC1, 0xE3, 0x10, // shl ebx,10
                0x66, 0x8B, 0x1D, 0x00, 0x00, 0x00, 0x00, // mov bx,[Jazz2.exe+1C856E]
                0x89, 0x1F, // mov [edi],ebx
                0xA1, 0x00, 0x00, 0x00, 0x00, // mov eax,[_PauseFramePointer]
                0x3B, 0x05, 0x00, 0x00, 0x00, 0x00, // cmp eax,[Jazz2.exe+1ADC20]
                0x75, 0x0A, // jne 00000000
                0xC7, 0x05, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, // mov [_IsPausedPointer],00000001
                0x58, // pop eax
                0x5B, // pop ebx
                0x5F, // pop edi
                0xC3, // ret
            };
            BitConverter.GetBytes((int)_TASInputsPointer).CopyTo(playbackFunctionData, 4);
            BitConverter.GetBytes((int)_Process.MainModule.BaseAddress + 0x1ADC20).CopyTo(playbackFunctionData, 9);
            BitConverter.GetBytes((int)_Process.MainModule.BaseAddress + 0x1C8AF0).CopyTo(playbackFunctionData, 25);
            BitConverter.GetBytes((int)_Process.MainModule.BaseAddress + 0x1C8AF0).CopyTo(playbackFunctionData, 31);
            BitConverter.GetBytes((int)_PositionHistoryPointer).CopyTo(playbackFunctionData, 46);
            BitConverter.GetBytes((int)_Process.MainModule.BaseAddress + 0x1C8572).CopyTo(playbackFunctionData, 54);
            BitConverter.GetBytes((int)_Process.MainModule.BaseAddress + 0x1C856E).CopyTo(playbackFunctionData, 64);
            BitConverter.GetBytes((int)_PauseFramePointer).CopyTo(playbackFunctionData, 71);
            BitConverter.GetBytes((int)_Process.MainModule.BaseAddress + 0x1ADC20).CopyTo(playbackFunctionData, 77);
            BitConverter.GetBytes((int)_IsPausedPointer).CopyTo(playbackFunctionData, 85);

            byte[] resetFunctionData = new byte[]
            {
                0x66, 0x50, // push ax
                0x66, 0xA1, 0x00, 0x00, 0x00, 0x00, // mov ax,[Jazz2.exe+E0714]
                0x66, 0x3D, 0x00, 0x00, // cmp ax,0000
                0x75, 0x09, // jne 00000000
                0x66, 0xC7, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // mov word ptr [Jazz2.exe+1ADC20],0000
                0x66, 0x58, // pop ax
                0xC3, // ret 
            };
            BitConverter.GetBytes((int)_Process.MainModule.BaseAddress + 0xE0714).CopyTo(resetFunctionData, 4);
            BitConverter.GetBytes((int)_Process.MainModule.BaseAddress + 0x1ADC20).CopyTo(resetFunctionData, 17);

            var playbackFunctionPointer = WinApi.VirtualAllocEx(_Process.Handle, IntPtr.Zero, playbackFunctionData.Length + resetFunctionData.Length, 0x1000, 0x40);
            WinApi.WriteProcessMemory(_Process.Handle, playbackFunctionPointer, playbackFunctionData, playbackFunctionData.Length, out bytesWritten);
            var resetFunctionPointer = playbackFunctionPointer + playbackFunctionData.Length;
            WinApi.WriteProcessMemory(_Process.Handle, resetFunctionPointer, resetFunctionData, resetFunctionData.Length, out bytesWritten);

            byte[] playbackCallData = new byte[]
            {
                0xE8, 0x00, 0x00, 0x00, 0x00, // call _PlaybackFunctionPointer
                0xC3, // ret
            };
            IntPtr playbackCallPointer = _Process.MainModule.BaseAddress + 0x61B91;
            BitConverter.GetBytes((int)playbackFunctionPointer - (int)playbackCallPointer - 5).CopyTo(playbackCallData, 1);
            WinApi.WriteProcessMemory(_Process.Handle, playbackCallPointer, playbackCallData, playbackCallData.Length, out bytesWritten);

            IntPtr resetCallPointer = _Process.MainModule.BaseAddress + 0x3F5C5;
            byte[] resetCallData = new byte[]
            {
                0xE8, 0x00, 0x00, 0x00, 0x00, // call _ResetFunctionPointer
                0x90, // nop
            };
            BitConverter.GetBytes((int)resetFunctionPointer - (int)resetCallPointer - 5).CopyTo(resetCallData, 1);
            WinApi.WriteProcessMemory(_Process.Handle, resetCallPointer, resetCallData, resetCallData.Length, out bytesWritten);
        }

        private void ProcessThreadMethod()
        {
            while (true)
            {
                try
                {
                    if (_Process == null || _Process.HasExited)
                    {
                        _Process = Process.GetProcessesByName("jazz2").FirstOrDefault();
                        if (_Process != null)
                        {
                            _Process.WaitForInputIdle();

                            if (_ProcessThreadExit.WaitOne(1000))
                                break;

                            InitializeTASFunctions();
                        }
                        BeginInvoke(new Action(RefreshGameInfo));

                        if (_ProcessThreadExit.WaitOne(1000))
                            break;
                    }
                    else
                    {
                        BeginInvoke(new Action(RefreshGameInfo));

                        if (_ProcessThreadExit.WaitOne(14))
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private void RefreshGameInfo()
        {
            try
            {
                if (_Process == null || _Process.HasExited)
                {
                    labelInfo.Text = "Jazz2.exe process not found";
                }
                else
                {
                    int bytesRead;
                    ushort x, y, frame, finished, paused, episodeFinished;
                    WinApi.ReadProcessMemory(_Process.Handle, _Process.MainModule.BaseAddress + 0x1C856E, out x, 2, out bytesRead);
                    WinApi.ReadProcessMemory(_Process.Handle, _Process.MainModule.BaseAddress + 0x1C8572, out y, 2, out bytesRead);
                    WinApi.ReadProcessMemory(_Process.Handle, _Process.MainModule.BaseAddress + 0x1ADC20, out frame, 2, out bytesRead);
                    WinApi.ReadProcessMemory(_Process.Handle, _Process.MainModule.BaseAddress + 0x1F3844, out finished, 2, out bytesRead);
                    WinApi.ReadProcessMemory(_Process.Handle, _Process.MainModule.BaseAddress + 0x12C4A8, out episodeFinished, 2, out bytesRead);
                    WinApi.ReadProcessMemory(_Process.Handle, _IsPausedPointer, out paused, 2, out bytesRead);

                    labelInfo.Text = string.Format("Position: {0} x {1}\r\nFrame: {2}", x, y, frame);

                    if (paused == 1)
                        buttonPlayPause.Text = "Play";
                    else
                        buttonPlayPause.Text = "Pause";

                    if (frame < _PreviousFrame)
                    {
                        if (_Index < dataGridViewInputs.Rows.Count)
                        {
                            dataGridViewInputs.InvalidateRow(_Index);
                            Debug.WriteLine("Row " + _Index + " invalidated.");
                        }
                        _Index = 0;
                        if (_Index < dataGridViewInputs.Rows.Count)
                        {
                            dataGridViewInputs.InvalidateRow(_Index);
                            Debug.WriteLine("Row " + _Index + " invalidated.");
                        }
                        _WaitForFrameReset = false;
                        Debug.WriteLine("Frame counter has been reset.");
                    }

                    if (!_WaitForFrameReset && finished == 0 && frame > _PreviousFrame && Inputs != null && Inputs.Count > 0)
                    {
                        int index = Math.Min(_Index, Inputs.Count - 1);

                        while (index < Inputs.Count && Inputs[index].Frame < frame)
                            index++;

                        if (--index != _Index)
                        {
                            dataGridViewInputs.InvalidateRow(_Index);
                            Debug.WriteLine("Row " + _Index + " invalidated.");
                            dataGridViewInputs.InvalidateRow(index);
                            Debug.WriteLine("Row " + index + " invalidated.");

                            int gun = 0;
                            while (_Index < index)
                            {
                                if (_Index < Inputs.Count)
                                {
                                    var inputs = Inputs[++_Index];
                                    if (inputs.Gun.HasValue)
                                    {
                                        gun = inputs.Gun.Value;
                                        Debug.WriteLine("Will switch to gun " + gun + ".");
                                    }
                                }
                            }

                            if (gun != 0)
                            {
                                Debug.WriteLine("Simulating key press: " + gun);
                                SendKeys.Send(gun.ToString());
                            }
                        }
                    }

                    if (episodeFinished > _PreviousEpisodeFinished)
                    {
                        Debug.WriteLine("Episode finished.");
                        Debug.WriteLine("Simulating key press: <SPACE>");
                        SendKeys.Send(" ");
                    }

                    if (finished > _PreviousFinished && dataGridViewLevels.SelectedRows.Count > 0)
                    {
                        Debug.WriteLine("Level finished.");
                        int newIndex = dataGridViewLevels.SelectedRows[0].Index + 1;
                        if (newIndex < dataGridViewLevels.Rows.Count)
                        {
                            dataGridViewLevels.Rows[newIndex].Selected = true;
                            Debug.WriteLine("Waiting for frame counter to be reset.");
                            _WaitForFrameReset = true;
                        }
                        else
                        {
                            dataGridViewLevels.ClearSelection();
                        }
                    }

                    _PreviousFinished = finished;
                    _PreviousFrame = frame;
                    _PreviousEpisodeFinished = episodeFinished;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
            }
        }

        private void dataGridViewInputs_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex == _Index)
            {
                using (var brush = new SolidBrush(_CurrentFrameBackgroundColor))
                {
                    e.Graphics.FillRectangle(brush, e.RowBounds);
                    e.PaintCells(e.ClipBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Background);
                    e.Handled = true;
                }
            }
        }

        private void menuItemOffset_Click(object sender, EventArgs e)
        {
            using (var dialog = new OffsetForm())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in dataGridViewInputs.SelectedRows)
                    {
                        var inputs = row.DataBoundItem as Inputs;

                        if (inputs != null)
                            inputs.Frame += dialog.Offset;
                    }
                }
            }
        }

        private void dataGridViewInputs_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex > 0)
            {
                if (e.Value != null && e.Value.GetType() == typeof(bool) && (bool)e.Value)
                {
                    using (var brush = new SolidBrush(_TableTickBackgroundColor))
                    {
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                        e.Paint(e.ClipBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Background);
                        e.Handled = true;
                    }
                }
            }
        }

        private void menuItemDefaultTheme_Click(object sender, EventArgs e)
        {
            Theme = Theme.DefaultTheme;
        }

        private void menuItemDarkTheme_Click(object sender, EventArgs e)
        {
            Theme = Theme.DarkTheme;
        }

        private void buttonPlayPause_Click(object sender, EventArgs e)
        {
            if (_Process == null || _Process.HasExited)
                return;

            ushort paused;
            WinApi.ReadProcessMemory(_Process.Handle, _IsPausedPointer, out paused, 2, out int byteCount);

            WinApi.WriteProcessMemory(_Process.Handle, _PauseFramePointer, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }, 4, out byteCount);
            if (paused == 1)
                WinApi.WriteProcessMemory(_Process.Handle, _IsPausedPointer, new byte[] { 0x00, 0x00, 0x00, 0x00 }, 4, out byteCount);
            else
                WinApi.WriteProcessMemory(_Process.Handle, _IsPausedPointer, new byte[] { 0x01, 0x00, 0x00, 0x00 }, 4, out byteCount);
        }

        private void buttonFrameAdvance_Click(object sender, EventArgs e)
        {
            if (_Process == null || _Process.HasExited)
                return;

            ushort frame;
            WinApi.ReadProcessMemory(_Process.Handle, _Process.MainModule.BaseAddress + 0x1ADC20, out frame, 4, out int byteCount);

            WinApi.WriteProcessMemory(_Process.Handle, _PauseFramePointer, BitConverter.GetBytes(frame + 1), 0x04, out byteCount);
            WinApi.WriteProcessMemory(_Process.Handle, _IsPausedPointer, BitConverter.GetBytes(0), 4, out byteCount);
        }

        private void menuItemShoot_Click(object sender, EventArgs e)
        {
            if (dataGridViewInputs.SelectedRows.Count == 0 || Inputs == null)
                return;

            using (var dialog = new ShootForm())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // Grab the selected frame to use as a baseline
                    // The baseline row is used so we don't lose inputs when switching on/off shoot
                    DataGridViewRow currentBaseline = dataGridViewInputs.SelectedRows[0] ;
                    var currentInputs = currentBaseline.DataBoundItem as Inputs ;

                    if (currentInputs == null)
                        return;

                    // Store the current index and frame
                    // We start the index at selected-1 to trick the i=0 iteration of the loop
                    var currentIndex = currentBaseline.Index-1 ;
                    var currentFrame = currentInputs.Frame ;

                    for(var i=0; i<dialog.Shoot; i++)
                    {
                        //---------//
                        //  Shoot  //
                        //---------//

                        if (Inputs.Count == currentIndex + 1)
                        {
                            // I'm at the end of the list, just make the next input
                            Inputs shoot = (Inputs)currentInputs.Clone();
                            shoot.Shoot = true;
                            shoot.Frame = currentFrame;
                            Inputs.Add(shoot);
                        }
                        else
                        {
                            // Check the next input to see if it falls on or around our frame window
                            Inputs nextInput = Inputs[currentIndex + 1];
                            var frame = nextInput.Frame;

                            if (frame == currentFrame)
                            {
                                // The next input falls on the frame we want to shoot, hijack the input
                                nextInput.Shoot = true;
                                currentInputs = nextInput;
                            }
                            else if (frame < currentFrame)
                            {
                                while (currentIndex+1 < Inputs.Count && Inputs[currentIndex + 1].Frame <= currentFrame)
                                {
                                    // We might've skipped multiple frames, find the highest frame that's < our current
                                    nextInput = Inputs[currentIndex + 1];
                                    currentIndex++;
                                    frame = nextInput.Frame;
                                }

                                // Oh no, we better check if the new frame is right on the frame we want
                                if (frame == currentFrame)
                                {
                                    // It was, our check was worth it! Hijack the input
                                    nextInput.Shoot = true;
                                    currentInputs = nextInput;
                                    // if we hit the frame exactly, drop the last index increment from the loop
                                    currentIndex--;
                                }
                                else
                                {
                                    // The input fell between our last shot and this one, use it as our baseline
                                    currentInputs = nextInput;
                                    Inputs shoot = (Inputs)currentInputs.Clone();
                                    shoot.Shoot = true;
                                    shoot.Frame = currentFrame;
                                    Inputs.Insert(currentIndex + 1, shoot);
                                }
                            }
                            else
                            {
                                // No new inputs, just use what we've been using
                                Inputs shoot = (Inputs)currentInputs.Clone();
                                shoot.Shoot = true;
                                shoot.Frame = currentFrame;
                                Inputs.Insert(currentIndex + 1, shoot);
                            }
                        }

                        // Increment the index
                        currentIndex++ ;

                        //------------//
                        //  Un-Shoot  //
                        //------------//

                        if (Inputs.Count == currentIndex + 1)
                        {
                            // I'm at the end of the list, just make the next input
                            Inputs unshoot = (Inputs)currentInputs.Clone();
                            unshoot.Shoot = false;
                            unshoot.Frame = currentFrame + 1;
                            Inputs.Add(unshoot);
                        }
                        else
                        {

                            // Check the next input to see if it falls on or around our frame window
                            Inputs nextInput = Inputs[currentIndex + 1];
                            var frame = nextInput.Frame;

                            if (frame == currentFrame + 1)
                            {
                                // The next input falls on the frame we want to unshoot, hijack the input
                                nextInput.Shoot = false;
                                currentInputs = nextInput;
                            }
                            else
                            {
                                // No input, just use what we've been using
                                Inputs unshoot = (Inputs)currentInputs.Clone();
                                unshoot.Shoot = false;
                                unshoot.Frame = currentFrame + 1;
                                Inputs.Insert(currentIndex + 1, unshoot);
                            }
                        }

                        // Increment the index and frame
                        // Frame goes up 6 because you can only shoot every 6 frames (for >2 shots)
                        currentIndex++ ;
                        currentFrame+=6 ;
                    }
                }
            }
        }

        private void dataGridViewInputs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 9)
            {
                var inputs = dataGridViewInputs.Rows[e.RowIndex].DataBoundItem as Inputs;

                if (inputs != null)
                {
                    using (var dialog = new SequenceForm(inputs))
                    {
                        dialog.ShowDialog();
                    }
                }
            }
        }

        private void menuInsert_Click(object sender, EventArgs e)
        {
            if (dataGridViewInputs.SelectedRows.Count == 0 || Inputs == null)
                return;

            using (var dialog = new InsertForm())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    DataGridViewRow insertAt = dataGridViewInputs.SelectedRows[0];
                    Inputs inputs = (Inputs)insertAt.DataBoundItem;
                    for (var i = 0; i < dialog.Insert; i++)
                    {
                        // I don't think inserting blank rows will be happy, so just default them to the selected row
                        Inputs.Insert(insertAt.Index, (Inputs)inputs.Clone());
                    }
                }
            }
        }
    }
}
