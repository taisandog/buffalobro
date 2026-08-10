using System;
using System.Drawing;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace AddInSetup.ConnStringUI
{
    /// <summary>
    /// MQ 后端类型，用于显示和输出各实现特有的可靠消费参数。
    /// </summary>
    public enum MQBackend
    {
        Kafka,
        MQTT,
        RabbitMQ,
        Redis
    }

    internal sealed class MQReliabilitySettings
    {
        public MQReliabilitySettings(MQBackend backend)
        {
            Backend = backend;
        }

        public MQBackend Backend { get; }
        public string AckMode { get; set; } = "manual";
        public bool RetryEnabled { get; set; } = true;
        public bool RetryOnException { get; set; } = true;
        public int MaxRetry { get; set; } = 4;
        public int RetryDelay { get; set; } = 1000;
        public int AckTimeout { get; set; } = 30000;
        public int PendingScanInterval { get; set; } = 5000;
        public bool DeadLetterEnabled { get; set; } = true;
        public string DeadLetterSuffix { get; set; } = ".DLQ";

        public ushort PrefetchCount { get; set; } = 1;
        public bool UseStartOffset { get; set; }
        public int StartOffset { get; set; }
        public int PartitionIndex { get; set; }
        public bool AutoAcknowledge { get; set; }
        public int StreamPageSize { get; set; } = 10;
        public int XTrimInterval { get; set; } = 30 * 60 * 1000;

        public void AppendTo(StringBuilder builder)
        {
            Append(builder, "ackMode", AckMode);
            Append(builder, "retryEnabled", RetryEnabled);
            Append(builder, "retryOnException", RetryOnException);
            Append(builder, "maxRetry", MaxRetry);
            Append(builder, "retryDelay", RetryDelay);
            Append(builder, "deadLetterEnabled", DeadLetterEnabled);
            Append(builder, "deadLetterSuffix", HttpUtility.UrlEncode(DeadLetterSuffix ?? string.Empty));

            switch (Backend)
            {
                case MQBackend.Kafka:
                    if (UseStartOffset)
                    {
                        Append(builder, "startOffset", StartOffset);
                        Append(builder, "partitionIndex", PartitionIndex);
                    }
                    break;
                case MQBackend.MQTT:
                    Append(builder, "autoAcknowledge", AutoAcknowledge);
                    break;
                case MQBackend.RabbitMQ:
                    Append(builder, "prefetchCount", PrefetchCount);
                    break;
                case MQBackend.Redis:
                    Append(builder, "ackTimeout", AckTimeout);
                    Append(builder, "pendingScanInterval", PendingScanInterval);
                    Append(builder, "streamPageSize", StreamPageSize);
                    Append(builder, "xTrimInterval", XTrimInterval);
                    break;
            }
        }

        private static void Append(StringBuilder builder, string name, bool value)
        {
            Append(builder, name, value ? "1" : "0");
        }

        private static void Append(StringBuilder builder, string name, object value)
        {
            builder.Append(name);
            builder.Append('=');
            builder.Append(value);
            builder.Append(';');
        }
    }

    internal sealed class MQReliabilitySettingsForm : Form
    {
        private readonly MQReliabilitySettings _settings;
        private readonly ComboBox _ackMode = CreateComboBox();
        private readonly CheckBox _retryEnabled = CreateCheckBox();
        private readonly CheckBox _retryOnException = CreateCheckBox();
        private readonly NumericUpDown _maxRetry = CreateNumber(0, int.MaxValue);
        private readonly NumericUpDown _retryDelay = CreateNumber(0, int.MaxValue);
        private readonly CheckBox _deadLetterEnabled = CreateCheckBox();
        private readonly TextBox _deadLetterSuffix = new TextBox { Width = 260 };

        private NumericUpDown _ackTimeout;
        private NumericUpDown _pendingScanInterval;
        private NumericUpDown _prefetchCount;
        private CheckBox _useStartOffset;
        private NumericUpDown _startOffset;
        private NumericUpDown _partitionIndex;
        private CheckBox _autoAcknowledge;
        private NumericUpDown _streamPageSize;
        private NumericUpDown _xTrimInterval;

        public MQReliabilitySettingsForm(MQReliabilitySettings settings)
        {
            _settings = settings;
            Text = GetBackendName(settings.Backend) + " - 可靠消费设置";
            Font = new Font("微软雅黑", 10F);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(620, 635);

            FlowLayoutPanel content = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(12, 10, 12, 10)
            };
            Controls.Add(content);

            Label note = new Label
            {
                AutoSize = false,
                Size = new Size(570, 42),
                Text = "失败消息会按重试上限重新投递；超过上限后写入死信。默认保持手动 ACK。",
                ForeColor = Color.DimGray
            };
            content.Controls.Add(note);

            _ackMode.Items.AddRange(new object[] { "manual（手动 ACK）", "onSuccess（成功后自动 ACK）" });
            content.Controls.Add(CreateCommonGroup());
            content.Controls.Add(CreateBackendGroup(settings.Backend));
            content.Controls.Add(CreateButtonPanel());

            LoadValues();
        }

        private GroupBox CreateCommonGroup()
        {
            TableLayoutPanel table = CreateTable(7);
            AddRow(table, 0, "ACK 模式", _ackMode);
            AddRow(table, 1, "启用失败重试", _retryEnabled);
            AddRow(table, 2, "回调异常时自动重试", _retryOnException);
            AddRow(table, 3, "最大重试次数", _maxRetry);
            AddRow(table, 4, "重试延迟（毫秒）", _retryDelay);
            AddRow(table, 5, "启用死信", _deadLetterEnabled);
            AddRow(table, 6, "死信后缀", _deadLetterSuffix);
            return CreateGroup("通用设置", table, 294);
        }

        private GroupBox CreateBackendGroup(MQBackend backend)
        {
            TableLayoutPanel table;
            int height;
            switch (backend)
            {
                case MQBackend.Kafka:
                    table = CreateTable(3);
                    _useStartOffset = CreateCheckBox();
                    _startOffset = CreateNumber(int.MinValue, int.MaxValue);
                    _partitionIndex = CreateNumber(0, int.MaxValue);
                    _useStartOffset.CheckedChanged += (_, __) => SetKafkaOffsetEnabled();
                    AddRow(table, 0, "指定起始 Offset", _useStartOffset);
                    AddRow(table, 1, "起始 Offset", _startOffset);
                    AddRow(table, 2, "分区索引", _partitionIndex);
                    height = 142;
                    break;
                case MQBackend.MQTT:
                    table = CreateTable(1);
                    _autoAcknowledge = CreateCheckBox();
                    AddRow(table, 0, "协议层自动 ACK", _autoAcknowledge);
                    height = 70;
                    break;
                case MQBackend.RabbitMQ:
                    table = CreateTable(1);
                    _prefetchCount = CreateNumber(1, ushort.MaxValue);
                    AddRow(table, 0, "预取消息数", _prefetchCount);
                    height = 70;
                    break;
                default:
                    table = CreateTable(4);
                    _ackTimeout = CreateNumber(0, int.MaxValue);
                    _pendingScanInterval = CreateNumber(0, int.MaxValue);
                    _streamPageSize = CreateNumber(1, int.MaxValue);
                    _xTrimInterval = CreateNumber(0, int.MaxValue);
                    AddRow(table, 0, "Pending 认领超时（毫秒）", _ackTimeout);
                    AddRow(table, 1, "Pending 扫描间隔（毫秒）", _pendingScanInterval);
                    AddRow(table, 2, "Stream 每批读取数", _streamPageSize);
                    AddRow(table, 3, "XTrim 间隔（毫秒）", _xTrimInterval);
                    height = 178;
                    break;
            }
            return CreateGroup(GetBackendName(backend) + " 专用设置", table, height);
        }

        private Control CreateButtonPanel()
        {
            Panel panel = new Panel { Size = new Size(570, 50) };
            Button ok = new Button
            {
                Text = "确定",
                Size = new Size(110, 36),
                Location = new Point(330, 7)
            };
            Button cancel = new Button
            {
                Text = "取消",
                Size = new Size(110, 36),
                Location = new Point(452, 7),
                DialogResult = DialogResult.Cancel
            };
            ok.Click += (_, __) =>
            {
                SaveValues();
                DialogResult = DialogResult.OK;
                Close();
            };
            panel.Controls.Add(ok);
            panel.Controls.Add(cancel);
            AcceptButton = ok;
            CancelButton = cancel;
            return panel;
        }

        private void LoadValues()
        {
            _ackMode.SelectedIndex = string.Equals(_settings.AckMode, "onSuccess",
                StringComparison.OrdinalIgnoreCase) ? 1 : 0;
            _retryEnabled.Checked = _settings.RetryEnabled;
            _retryOnException.Checked = _settings.RetryOnException;
            _maxRetry.Value = _settings.MaxRetry;
            _retryDelay.Value = _settings.RetryDelay;
            _deadLetterEnabled.Checked = _settings.DeadLetterEnabled;
            _deadLetterSuffix.Text = _settings.DeadLetterSuffix;

            switch (_settings.Backend)
            {
                case MQBackend.Kafka:
                    _useStartOffset.Checked = _settings.UseStartOffset;
                    _startOffset.Value = _settings.StartOffset;
                    _partitionIndex.Value = _settings.PartitionIndex;
                    SetKafkaOffsetEnabled();
                    break;
                case MQBackend.MQTT:
                    _autoAcknowledge.Checked = _settings.AutoAcknowledge;
                    break;
                case MQBackend.RabbitMQ:
                    _prefetchCount.Value = _settings.PrefetchCount;
                    break;
                case MQBackend.Redis:
                    _ackTimeout.Value = _settings.AckTimeout;
                    _pendingScanInterval.Value = _settings.PendingScanInterval;
                    _streamPageSize.Value = _settings.StreamPageSize;
                    _xTrimInterval.Value = _settings.XTrimInterval;
                    break;
            }
        }

        private void SaveValues()
        {
            _settings.AckMode = _ackMode.SelectedIndex == 1 ? "onSuccess" : "manual";
            _settings.RetryEnabled = _retryEnabled.Checked;
            _settings.RetryOnException = _retryOnException.Checked;
            _settings.MaxRetry = Decimal.ToInt32(_maxRetry.Value);
            _settings.RetryDelay = Decimal.ToInt32(_retryDelay.Value);
            _settings.DeadLetterEnabled = _deadLetterEnabled.Checked;
            _settings.DeadLetterSuffix = _deadLetterSuffix.Text.Trim();

            switch (_settings.Backend)
            {
                case MQBackend.Kafka:
                    _settings.UseStartOffset = _useStartOffset.Checked;
                    _settings.StartOffset = Decimal.ToInt32(_startOffset.Value);
                    _settings.PartitionIndex = Decimal.ToInt32(_partitionIndex.Value);
                    break;
                case MQBackend.MQTT:
                    _settings.AutoAcknowledge = _autoAcknowledge.Checked;
                    break;
                case MQBackend.RabbitMQ:
                    _settings.PrefetchCount = Decimal.ToUInt16(_prefetchCount.Value);
                    break;
                case MQBackend.Redis:
                    _settings.AckTimeout = Decimal.ToInt32(_ackTimeout.Value);
                    _settings.PendingScanInterval = Decimal.ToInt32(_pendingScanInterval.Value);
                    _settings.StreamPageSize = Decimal.ToInt32(_streamPageSize.Value);
                    _settings.XTrimInterval = Decimal.ToInt32(_xTrimInterval.Value);
                    break;
            }
        }

        private void SetKafkaOffsetEnabled()
        {
            bool enabled = _useStartOffset.Checked;
            _startOffset.Enabled = enabled;
            _partitionIndex.Enabled = enabled;
        }

        private static GroupBox CreateGroup(string text, Control content, int height)
        {
            GroupBox group = new GroupBox { Text = text, Size = new Size(570, height) };
            group.Controls.Add(content);
            return group;
        }

        private static TableLayoutPanel CreateTable(int rowCount)
        {
            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = rowCount,
                Padding = new Padding(10, 6, 10, 6)
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57F));
            for (int i = 0; i < rowCount; i++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowCount));
            }
            return table;
        }

        private static void AddRow(TableLayoutPanel table, int row, string labelText, Control control)
        {
            Label label = new Label
            {
                Text = labelText + "：",
                AutoSize = true,
                Anchor = AnchorStyles.Left
            };
            control.Anchor = AnchorStyles.Left;
            table.Controls.Add(label, 0, row);
            table.Controls.Add(control, 1, row);
        }

        private static ComboBox CreateComboBox()
        {
            return new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 260
            };
        }

        private static CheckBox CreateCheckBox()
        {
            return new CheckBox { AutoSize = true };
        }

        private static NumericUpDown CreateNumber(decimal minimum, decimal maximum)
        {
            return new NumericUpDown
            {
                Minimum = minimum,
                Maximum = maximum,
                Width = 180,
                ThousandsSeparator = true
            };
        }

        private static string GetBackendName(MQBackend backend)
        {
            switch (backend)
            {
                case MQBackend.Kafka:
                    return "Kafka";
                case MQBackend.MQTT:
                    return "MQTT";
                case MQBackend.RabbitMQ:
                    return "RabbitMQ";
                default:
                    return "Redis";
            }
        }
    }
}
