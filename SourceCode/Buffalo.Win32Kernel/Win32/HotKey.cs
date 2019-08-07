using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Buffalo.Win32Kernel.Win32
{
    public class HotKey:IDisposable
    {
        /// <summary>
        /// ���ȼ�������
        /// </summary>
        public event DelOnWndProc OnHotKeyPress;

        int id = -1;

        /// <summary>
        /// �ȼ��ı�ʶ
        /// </summary>
        public int Id
        {
            get { return id; }
        }
        KeyModifiers modifierKey = KeyModifiers.None;

        /// <summary>
        /// ���μ�
        /// </summary>
        public KeyModifiers ModifierKey
        {
            get { return modifierKey; }
        }
        Keys key = Keys.None;

        /// <summary>
        /// ����
        /// </summary>
        public Keys Key
        {
            get { return key; }
        }

        Control bindControl;

        /// <summary>
        /// �󶨵Ŀؼ�s
        /// </summary>
        public Control BindControl
        {
          get { return bindControl; }
        }

        public HotKey(int id, KeyModifiers modifierKey, Keys key,Control bindControl) 
        {
            this.id = id;
            this.modifierKey = modifierKey;
            this.key = key;
            this.bindControl=bindControl;
        }
        bool isRegistered = false;//�Ƿ��Ѿ�ע��

        /// <summary>
        /// �Ƿ��Ѿ�ע�����ȼ�
        /// </summary>
        public bool IsRegistered
        {
            get { return isRegistered; }
        }

        /// <summary>
        /// ע����ȼ�
        /// </summary>
        public bool Register()
        {
            if (!isRegistered)
            {
                isRegistered = WindowsAPI.RegisterHotKey(bindControl.Handle, id, (uint)modifierKey, (uint)key);
                
            }
            return isRegistered;
        }

        /// <summary>
        /// ж�ش��ȼ��ȼ�
        /// </summary>
        public void UnRegister()
        {
            if (isRegistered)
            {
                WindowsAPI.UnregisterHotKey(bindControl.Handle, id);
                isRegistered = false;
            }
        }




        #region IDisposable ��Ա

        public void Dispose()
        {
            UnRegister();
            GC.SuppressFinalize(this);
        }

        #endregion

        ~HotKey() 
        {
            UnRegister();
        }

        /// <summary>
        /// ֪ͨ�������ؼ�������WndProc�¼�(����WndProc)�����)
        /// </summary>
        /// <param name="m">WndProc����Ϣ</param>
        /// <returns></returns>
        public void DoWndProc(Message m)
        {
            if (m.Msg == (int)Msg.WM_HOTKEY && id==(int)m.WParam)
            {
                if (OnHotKeyPress != null) 
                {
                    OnHotKeyPress(m);
                }
            }
        }
    }
}


/*=============����================
public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        HotKey objHotKey;
        private void Form1_Load(object sender, EventArgs e)
        {
            objHotKey = new HotKey(123, KeyModifiers.Control | KeyModifiers.Alt, Keys.W, this);
            objHotKey.Register();
            objHotKey.OnHotKeyPress += new DelOnWndProc(_hotKey_OnHotKeyPress);
            
        }
        
        
        protected override void WndProc(ref Message m)
        {
            if (_hotKey != null) 
            {
                _hotKey.DoWndProc(m);
            }
            base.WndProc(ref m);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            objHotKey.UnRegister();
        }
        void _hotKey_OnHotKeyPress(Message msg)
        {
            //�ȼ�����
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }
*/