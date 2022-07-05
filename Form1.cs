using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Specify the COPYLOCK DLL to be used based on application mode (Single User or Multi-user)
        // you may also rename original default DLL file name and use this name here.
        const String dll_COPYLOCK = "xCL32.DLL"; //Standalone application

//        const String dll_COPYLOCK = "NETCL32.DLL"; //Multiuser application

        [DllImport(dll_COPYLOCK)]
        public static extern int cl_login(string name);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_get_model([MarshalAs(UnmanagedType.LPArray)] byte[] dModel);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_lock_ok();
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_get_id([MarshalAs(UnmanagedType.LPArray)] byte[] dID, String rPass);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_get_batch([MarshalAs(UnmanagedType.LPArray)] byte[] dBatch, String rPass);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_set_sign(String dSign, String wPass);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_get_sign([MarshalAs(UnmanagedType.LPArray)] byte[] dSign, String rPass);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_set_osign(String dSign, String wPass);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_get_osign([MarshalAs(UnmanagedType.LPArray)] byte[] dSign, String rPass);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_write_block(String dBuf, String wPass, int BlockNo, int StPos, int Count);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_read_block([MarshalAs(UnmanagedType.LPArray)] byte[] dBuf, String rPass, int BlockNo, int StPos, int Count);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_write_word(int dWord, String wPass, int BlockNo, int StPos);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_read_word(ref int dWord, String rPass, int BlockNo, int StPos);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_get_count(ref int dCo, String rPass);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_dec_count(int dVal, String wPass);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_logout();

        private void DisplayResult(int dResult, string data)
        {
            switch (dResult)
            {
                case 1:
                    if (data == "")
                    {
                        toolStripStatusLabel1.Text = "Success" ;
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Success" + ";   Dongle Data = " + data;
                    }
                    break;

                case -1:
                    toolStripStatusLabel1.Text = "Error#-1: Lock is missing" ;
                    break;

                case -2:
                    toolStripStatusLabel1.Text = "Error#-2: Driver load Error" ;
                    break;

                case -3:
                    toolStripStatusLabel1.Text = "Error#-3: Incorrect password" ;
                    break;

                case -4:
                    toolStripStatusLabel1.Text = "Error#-4: Fascility N.A." ;
                    break;

                case -5:
                    toolStripStatusLabel1.Text = "Error#-5: Write failed" ;
                    break;

                case -6:
                    toolStripStatusLabel1.Text = "Error#-6: Login users limit is over" ;
                    break;

                case -7:
                    toolStripStatusLabel1.Text = "Error#-7: Link to the server is not active" ;
                    break;

                case -9:
                    toolStripStatusLabel1.Text = "Error#-9: Cannot establish connection with server" ;
                    break;

                case -12 :
                   toolStripStatusLabel1.Text = "Error#-12: Time period expired";
                   break;

                default:

                    toolStripStatusLabel1.Text = "Error#nn: Internal Error";
                    break;
            }
        }

        private void clloginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int dResult = cl_login("032140501240040622010232");
            int myCo = 0;
            String rPass = "";   //read authentication password

            DisplayResult(dResult, "") ;
            if (dResult == 1)
            {
                dResult = cl_get_count(ref myCo, rPass);
                dResult = cl_dec_count(1, rPass);
                dResult = cl_get_count(ref myCo, rPass);
            }
        }

        private void clgetmodelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte[] dModel = new byte[100]; 
            String DongleModel = "";
            int dResult = cl_get_model(dModel);
            DongleModel = System.Text.Encoding.ASCII.GetString(dModel,0,20);
            DisplayResult(dResult, DongleModel);
        }

        private void cllockokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int dResult = cl_lock_ok();
            DisplayResult(dResult, "");
        }

        private void clgetidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte[] dID = new byte[100];
            String DongleID = "";
            String rPass = "";   //read authentication password
            int dResult = cl_get_id(dID, rPass);
            DongleID = System.Text.Encoding.ASCII.GetString(dID,0,8);
            DisplayResult(dResult, DongleID);
        }

        private void clgetbatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte[] dBatch = new byte[100];
            String DongleBatch = "";
            String rPass = "";   //read authentication password
            int dResult = cl_get_batch(dBatch, rPass);
            DongleBatch = System.Text.Encoding.ASCII.GetString(dBatch,0,8);
            DisplayResult(dResult, DongleBatch);
        }

        private void clsetsignToolStripMenuItem_Click(object sender, EventArgs e)
        {            // dSign max length can be 75 char
            String dSign = "Customer's Firm Name                                                       ";
            String wPass = "";   //write authentication password
            
            int dResult = cl_set_sign(dSign, wPass);
            DisplayResult(dResult, "");
        }

        private void clgetsignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte[] dSign = new byte[100];
            String DongleSign = "";
            String rPass = "";   //read authentication password
            int dResult = cl_get_sign(dSign, rPass);
            DongleSign = System.Text.Encoding.ASCII.GetString(dSign,0,75);
            DisplayResult(dResult, DongleSign);
        }

        private void clsetosignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // dSign max length can be 50 char
            String dSign = "My Firm Name                                      ";
            String wPass = "";   //write authentication password

            int dResult = cl_set_osign(dSign, wPass);
            DisplayResult(dResult, "");
        }

        private void clgetosignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte[] dSign = new byte[100];
            String DongleOSign = "";
            String rPass = "";   //read authentication password
            int dResult = cl_get_osign(dSign, rPass);
            DongleOSign = System.Text.Encoding.ASCII.GetString(dSign,0,50);
            DisplayResult(dResult, DongleOSign);
        }

        private void clwriteblockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // dBlock1 & dBlock2 each of max length can be 100 char
            String dBuf = "My Block1 Data                                                                                      ";
            String wPass = "";   //write authentication password
            int BlockNo = 1;
            int StPos = 1;
            int Count = 100;

            int dResult = cl_write_block(dBuf, wPass, BlockNo, StPos, Count);
            DisplayResult(dResult, "");
        }

        private void clreadblockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte[] dBuf = new byte[100];
            String DongleMemBlock1 = "";
            String rPass = "";   //read authentication password
            int BlockNo = 1;
            int StPos = 1;
            int Count = 100;

            int dResult = cl_read_block(dBuf, rPass, BlockNo, StPos, Count);
            DongleMemBlock1 = System.Text.Encoding.ASCII.GetString(dBuf, 0, 100);
            DisplayResult(dResult, DongleMemBlock1);
        }

        private void clwriteblock2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // dBlock1 & dBlock2 each of max length can be 100 char
            String dBuf = "My Block2 Data                                                                                      ";
            String wPass = "";   //write authentication password
            int BlockNo = 2;
            int StPos = 1;
            int Count = 100;

            int dResult = cl_write_block(dBuf, wPass, BlockNo, StPos, Count);
            DisplayResult(dResult, "");
        }

        private void clreadblock2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte[] dBuf = new byte[100];
            String DongleMemBlock2 = "";
            String rPass = "";   //read authentication password
            int BlockNo = 2;
            int StPos = 1;
            int Count = 100;

            int dResult = cl_read_block(dBuf, rPass, BlockNo, StPos, Count);
            DongleMemBlock2 = System.Text.Encoding.ASCII.GetString(dBuf, 0, 100);
            DisplayResult(dResult, DongleMemBlock2);
        }

        private void clwritewordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int dWord = 9999;
            String wPass = "";   //write authentication password
            int BlockNo = 1;
            int StPos = 11;

            int dResult = cl_write_word(dWord, wPass, BlockNo, StPos);
            DisplayResult(dResult, "");
        }

        private void clreadwordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int dWord = 0;
            String rPass = "";   //write authentication password
            int BlockNo = 1;
            int StPos = 11;

            int dResult = cl_read_word(ref dWord, rPass, BlockNo, StPos);
            DisplayResult(dResult, "");
        }

        private void cllogoutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            int dResult = cl_logout();
            DisplayResult(dResult, "");
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
