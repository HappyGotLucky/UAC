using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using UAC.UServe;

namespace UAC
{
    public class data
    {
        public uServiceClient client = new uServiceClient();
        public string account_name(int user_id)
        {
            string username = "";


            return username;
        }
        public bool resetpassword(string password)
        {
            UAC_UserInfo ui = new UAC_UserInfo();
            ui.User_Id = login.us.userid;
            ui.Guid = login.us.guid;
            ui.Password = password;
            ui = client.UAC_ChangePassword(ui);
            if (ui.SQLResult == "1") { return true; } else { return false; }

        }
        public user account_login(string username, string password)
        {
            user us = new user();
            us.userid = 0;
            UAC_UserInfo ui = new UAC_UserInfo();
            ui.Username = username;
            ui.Password = password;
            ui = client.UAC_EnterLogin(ui);
            if (ui.SQLResult == "1")
            {
                us.username = ui.Username;
                us.userid = ui.User_Id;
                us.email = ui.Email;
                us.guid = ui.Guid;
                if (ui.Is_Admin == 1) { us.admin = true; } else { us.admin = false; }
                us.needs_reset = ui.Needs_Reset;
            }

            return us;
        }

        public bool account_create(string player, string password, string email, int clan)
        {
            UAC_UserInfo ui = new UAC_UserInfo();
            ui.Username = player;
            ui.Email = email;
            ui.Password = password;
            ui = client.UAC_CreateAccount(ui);

            if (ui.SQLResult=="1")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public string account_email(string username)
        {
            string email = "";



            return email;

        }
        public bool account_exists(string player)
        {
            UAC_UserInfo ui = new UAC_UserInfo();
            ui.Email = player;
            ui = client.UAC_EmailInUse(ui);
            if (ui.SQLResult=="1")
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public bool iamonline()
        {
            UAC_UserInfo ui = new UAC_UserInfo();
            ui.User_Id = login.us.userid;
            ui = client.UAC_ImOnline(ui);
            if (ui.SQLResult=="1")
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
    public class user
    {
        public int userid;
        public string username;
        public string email;
        public string password;
        public int clan_id;
        public bool admin;
        public int isencrypted;
        public Int64 needs_reset;
        public string guid;
    }
    public class ScreenCapture
    {
        /// <summary>
        /// Creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        /// <returns></returns>
        public Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }
        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
        public Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            //Size sz = Screen.AllScreens.b;
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = 0;
            int height = 0;

            if (Screen.AllScreens.Length > 1)
            {
                width = SystemInformation.VirtualScreen.Width;//windowRect.right - windowRect.left;
                height = SystemInformation.VirtualScreen.Height;// windowRect.bottom - windowRect.top;
            }
            else
            {
                width = windowRect.right - windowRect.left;
                height = windowRect.bottom - windowRect.top;
            }
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);
            return img;
        }
        /// <summary>
        /// Captures a screen shot of a specific window, and saves it to a file
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
        {
            Image img = CaptureWindow(handle);
            img.Save(filename, format);
        }
        /// <summary>
        /// Captures a screen shot of the entire desktop, and saves it to a file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public void CaptureScreenToFile(string filename, ImageFormat format)
        {
            Image img = CaptureScreen();
            img.Save(filename, format);
        }

        /// <summary>
        /// Helper class containing Gdi32 API functions
        /// </summary>
        private class GDI32
        {

            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }

        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        }
    }
}
