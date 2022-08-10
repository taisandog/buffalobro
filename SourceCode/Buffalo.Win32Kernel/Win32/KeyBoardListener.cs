using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Runtime.InteropServices;


namespace Buffalo.Win32Kernel.Win32
{
    
    public class KeyBoardListener:IDisposable
    {
        //
        // ժҪ:
        //     �ڿؼ��н��������°��¼�ʱ������
        public event KeyEventHandler KeyDown;
        //
        // ժҪ:
        //     �ڿؼ��н��������°��¼�ʱ������
        public event KeyPressEventHandler KeyPress;
        //
        // ժҪ:
        //     �ڿؼ��н����������ͷż�ʱ������
        public event KeyEventHandler KeyUp;


        
        WindowsHook hook;

        bool lShift=false;
        bool rShift = false;
        bool lCtrl = false;
        bool rCtrl = false;
        bool lAlt = false;
        bool rAlt = false;

        bool isListening = false;

        /// <summary>
        /// �Ƿ��Ѿ����
        /// </summary>
        public bool IsListening
        {
            get { return isListening; }
        }
        public KeyBoardListener() 
        {
            hook = new WindowsHook(HookType.WH_KEYBOARD_LL);
            hook.HookInvoked += new WindowsAPI.HookEventHandler(hook_HookInvoked);
            
        }
        public void StartListener() 
        {
            hook.Install();
            isListening = true;
        }

        public void StopListener() 
        {
            hook.Uninstall();
            isListening = false;
        }
        void hook_HookInvoked(object sender, WindowsAPI.HookEventArgs e)
        {
            KBDLLHOOKSTRUCT keyboardHook = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(e.lParam, typeof(KBDLLHOOKSTRUCT));
            KeyBoardType type = (KeyBoardType)e.wParam;
            if (e.HookCode >= 0 )
            {
                int vkCode = keyboardHook.vkCode;
                
                switch (type)
                {
                    case KeyBoardType.WM_KEYDOWN:
                        DoKeyDown(vkCode);
                        break;
                    case KeyBoardType.WM_KEYUP:
                        DoPress(vkCode);
                        DoKeyUp(vkCode);
                        break;
                    case KeyBoardType.WM_SYSKEYDOWN:
                        DoKeyDown(vkCode);
                        break;
                    case KeyBoardType.WM_SYSKEYUP:
                        DoKeyUp(vkCode);
                        break;
                    default:
                        break;
                }
            }

        }

        private void DoKeyDown(int vkCode) 
        {
            //downKey.AddProperty(vkCode);
            //downKey[vkCode] = true;
            if (KeyDown != null)
            {
                SetCombination(vkCode, true);
                Keys key = MakeKeys(vkCode);
                KeyEventArgs ex = new KeyEventArgs(key);
                KeyDown(null, ex);
            }
        }
        private void DoKeyUp(int vkCode)
        {
            
            
            if (KeyUp != null)
            {
                SetCombination(vkCode, true);
                Keys key = MakeKeys(vkCode);
                KeyEventArgs ex = new KeyEventArgs(key);
                KeyUp(null, ex);
            }
            //downKey.DeleteProperty(vkCode);
        }

        private void DoPress(int vkCode)
        {
            if (KeyPress != null)
            {
                KeyPressEventArgs ex = new KeyPressEventArgs((char)vkCode);
                KeyPress(null, ex);
            }
        }

        /// <summary>
        /// ������ϼ�
        /// </summary>
        /// <param name="vkCode"></param>
        private void SetCombination(int vkCode,bool isDown) 
        {
            switch ((KeyType)vkCode)
            {
                case KeyType.VK_LSHIFT:
                    lShift = isDown;
                    break;
                case KeyType.VK_RSHIFT:
                    rShift = isDown;
                    break;
                case KeyType.VK_LCONTROL:
                    lCtrl = isDown;
                    break;
                case KeyType.VK_RCONTROL:
                    rCtrl = isDown;
                    break;
                case KeyType.VK_LMENU:
                    lAlt = isDown;
                    break;
                case KeyType.VK_RMENU:
                    rAlt = isDown;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// ��ϰ�����Ϣ
        /// </summary>
        /// <param name="vkCode"></param>
        /// <returns></returns>
        private Keys MakeKeys(int vkCode)
        {
            Keys keys = (Keys)vkCode;

            if (lShift || rShift)
                keys |= Keys.Shift;
            if (lCtrl || rCtrl)
                keys |= Keys.Control;
            if (lAlt || rAlt)
                keys |= Keys.Alt;

            return keys;
        }
        #region IDisposable ��Ա

        public void Dispose()
        {
            StopListener();
        }

        #endregion
        ~KeyBoardListener() 
        {
            StopListener();
        }
    }
}
