using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common.Container.Management
{
    public class DialogService : IAdditionalService
    {
        private IntPtr ownerHandle;

        public DialogService(IntPtr hwnd)
        {
            ownerHandle = hwnd;
        }

        public DialogService() : this(IntPtr.Zero)
        {
        }

        public DialogResult ShowDialog(Form form)
        {
            return ShowDialog(form, IntPtr.Zero);
        }

        public DialogResult ShowDialog(Form form, IntPtr handle)
        {
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowInTaskbar = false;
            form.WindowState = FormWindowState.Normal;

            if (handle != IntPtr.Zero || ownerHandle != IntPtr.Zero)
            {
                using (DialogParentWrapper wrapper = new DialogParentWrapper(ownerHandle))
                {
                    return form.ShowDialog(wrapper.Owner);
                }
            }
            else
            {
                return form.ShowDialog();
            }
        }

        public void Show(Form form)
        {
            Show(form, IntPtr.Zero);
        }

        public void Show(Form form, IntPtr handle)
        {
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowInTaskbar = false;
            form.WindowState = FormWindowState.Normal;

            if (handle != IntPtr.Zero || ownerHandle != IntPtr.Zero)
            {
                using (DialogParentWrapper wrapper = new DialogParentWrapper(handle))
                {
                    form.Show(wrapper.Owner);
                }
            }
            else
            {
                form.Show();
            }
        }

        internal class DialogParentWrapper : IDisposable
        {
            NativeWindow owner;

            public DialogParentWrapper(IntPtr handle)
            {
                try
                {
                    owner = new NativeWindow();
                    owner.AssignHandle(handle);
                }
                catch { }
            }

            public NativeWindow Owner
            {
                get { return owner; }
            }

            private void Dispose(bool disposing)
            {
                if (disposing && owner != null)
                {
                    try
                    {
                        owner.ReleaseHandle();
                        owner.DestroyHandle();
                    }
                    catch { }
                    owner = null;
                }
            }

            #region IDisposable Members

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            #endregion
        }

        #region IDisposable Members

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                ownerHandle = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
