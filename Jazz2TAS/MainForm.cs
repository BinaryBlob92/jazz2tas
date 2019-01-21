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
        private int _CurrentHash;
        private int _Index;
        private ushort[] _Positions = new ushort[256];
        private Color _CurrentFrameBackgroundColor = Color.FromArgb(255, 0, 0);
        private Color _TableTickBackgroundColor = Color.FromArgb(192, 192, 192);

        private IntPtr _TASInputsPointer;
        private IntPtr _PositionHistoryPointer;

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
                statusStrip.BackColor = BackColor;
                labelLevels.ForeColor = value.GetTextColor();
                labelInputs.ForeColor = labelLevels.ForeColor;
                labelPositionHistory.ForeColor = labelLevels.ForeColor;
                toolStripStatusLabelInfo.ForeColor = labelLevels.ForeColor;
                toolStripStatusLabelProcessFound.ForeColor = labelLevels.ForeColor;
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
                dialog.Filter = "XML-files|*.xml";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (var fileStream = new FileStream(dialog.FileName, FileMode.Open))
                    {
                        var serializer = new XmlSerializer(typeof(BindingList<Level>));
                        dataGridViewLevels.DataSource = serializer.Deserialize(fileStream);
                        _CurrentProjectPath = dialog.FileName;
                        HasChanged();
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
                    if(level.LevelName != null)
                        newHash ^= level.LevelName.GetHashCode();
                    foreach (var inputs in level.Inputs)
                    {
                        newHash += inputs.Frame ^ inputs.ToJazz2Inputs() ^ inputs.Gun.GetHashCode();
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
                    dialog.Filter = "XML-files|*.xml";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        Save(dialog.FileName);
                    }
                }
            }
            else
            {
                using (var fileStream = new FileStream(filename, FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(BindingList<Level>));
                    serializer.Serialize(fileStream, Levels);
                    _CurrentProjectPath = filename;
                    HasChanged();
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
            var tasInputs = SelectedLevel.GetJazz2InputsBytes();
            WinApi.WriteProcessMemory(_Process.Handle, _TASInputsPointer, tasInputs, tasInputs.Length * 2, out int bytesWritten);
        }

        private void InitializeTASFunctions()
        {
            _TASInputsPointer = WinApi.VirtualAllocEx(_Process.Handle, IntPtr.Zero, 0xFFFF + 0xFF, 0x1000, 0x04);
            TransferInputs();

            _PositionHistoryPointer = _TASInputsPointer + 0xFFFF;
            WinApi.WriteProcessMemory(_Process.Handle, _PositionHistoryPointer, new byte[256], 256, out int bytesWritten);

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

                        if (_ProcessThreadExit.WaitOne(50))
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
                    toolStripStatusLabelProcessFound.Text = "Jazz2.exe process not found";
                    toolStripStatusLabelInfo.Text = null;
                }
                else
                {
                    int bytesRead;
                    ushort x, y, frame, finished;
                    WinApi.ReadProcessMemory(_Process.Handle, _Process.MainModule.BaseAddress + 0x1C856E, out x, 4, out bytesRead);
                    WinApi.ReadProcessMemory(_Process.Handle, _Process.MainModule.BaseAddress + 0x1C8572, out y, 4, out bytesRead);
                    WinApi.ReadProcessMemory(_Process.Handle, _Process.MainModule.BaseAddress + 0x1ADC20, out frame, 4, out bytesRead);
                    WinApi.ReadProcessMemory(_Process.Handle, _Process.MainModule.BaseAddress + 0x1F3844, out finished, 4, out bytesRead);

                    toolStripStatusLabelProcessFound.Text = "Jazz2.exe process found";
                    toolStripStatusLabelInfo.Text = string.Format("Pos: {0} x {1}, Frame: {2}", x, y, frame);

                    if (finished == 0 && frame != _PreviousFrame && Inputs != null && Inputs.Count > 0)
                    {
                        int index = _Index;
                        int gun = 0;

                        while (index >= Inputs.Count || (index > 0 && Inputs[index].Frame > frame))
                            index--;

                        while (index < Inputs.Count && Inputs[index].Frame < frame)
                        {
                            var inputs = Inputs[index];
                            if (inputs.Gun.HasValue && inputs.Gun.Value > 0 && inputs.Gun.Value < 10)
                            {
                                gun = inputs.Gun.Value;
                            }
                            index++;
                        }

                        _PreviousFrame = frame;

                        if (--index != _Index)
                        {
                            int oldIndex = _Index;
                            _Index = index;
                            dataGridViewInputs.InvalidateRow(oldIndex);
                            dataGridViewInputs.InvalidateRow(index);

                            if (gun != 0)
                                SendKeys.Send(gun.ToString());
                        }
                    }

                    if (finished > _PreviousFinished && dataGridViewLevels.SelectedRows.Count > 0)
                    {
                        int newIndex = dataGridViewLevels.SelectedRows[0].Index + 1;
                        if (newIndex < dataGridViewLevels.Rows.Count)
                        {
                            dataGridViewLevels.Rows[newIndex].Selected = true;
                        }
                    }

                    _PreviousFinished = finished;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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
                if(e.Value != null && e.Value.GetType() == typeof(bool) && (bool)e.Value)
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
    }
}
