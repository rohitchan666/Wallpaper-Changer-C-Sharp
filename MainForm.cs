/*
 * Created by SharpDevelop.
 * User: 
 * Date: 01-Dec-22
 * Time: 1:01 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WallChangeV2
{
	
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		int intTimerInterval = 0;
		int intSeconds = 0;
		
		//Code for calling Windows functions, bcoz .NET doesnt have inbuilt function of changing wallpaper
		[DllImport("user32.dll", SetLastError = true)]
	    [return: MarshalAs(UnmanagedType.Bool)]
	    static extern bool SystemParametersInfo(uint uiAction, uint uiParam, String pvParam, uint fWinIni);	
		private const uint SPI_SETDESKWALLPAPER = 0x14;
	    private const uint SPIF_UPDATEINIFILE = 0x1;
	    private const uint SPIF_SENDWININICHANGE = 0x2;
	    
		public static void SetWallpaper(String strpath)
		{
		    SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, strpath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
		}
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			//Set Initial values
			openFileDialog1.Filter = "Images (*.jpg)|*.jpg";
			openFileDialog1.Multiselect = true;
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.CheckPathExists = true;
			openFileDialog1.ReadOnlyChecked = true;
			openFileDialog1.FileName = "";
			
			comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBox1.Items.Add("2 Min");
			comboBox1.Items.Add("5 Min");
			comboBox1.Items.Add("10 Min");
			comboBox1.Items.Add("15 Min");
			comboBox1.Items.Add("30 Min");
			comboBox1.Items.Add("60 Min");
			comboBox1.SelectedIndex = 0;
			
			label4.Text = "";
	
			pictureBox1.BorderStyle = BorderStyle.Fixed3D;
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
			
			timer1.Interval = 1000; //1 sec
			timer2.Interval = 60000; //1 min
			
		}
		
		void Button1Click(object sender, EventArgs e)
		{
	
			string[] strFileName;
			
			if (openFileDialog1.ShowDialog() == DialogResult.OK) 
			{

				strFileName = openFileDialog1.FileNames;
				//MessageBox.Show(Convert.ToString(strFileName.Length));
				
				for (int i = 0; i < strFileName.Length; i++) 
				{
					listBox1.Items.Add(strFileName[i]);
				}

			}

		}
		
		void Button2Click(object sender, EventArgs e)
		{
			listBox1.Items.Clear();
			timer1.Enabled = false;
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			if (listBox1.Items.Count == 0) {
				//Emptry listbox, Exit
				return;
			}
			
			SetWallpaper(pictureBox1.ImageLocation);
			
		}
		
		void ListBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			pictureBox1.ImageLocation = listBox1.SelectedItem.ToString();
		}
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{

			switch (checkBox1.Checked) {
				case true:
					if (listBox1.Items.Count == 0) 
					{
						MessageBox.Show("No Images file in listbox", Application.ProductName, MessageBoxButtons.OK);
						return;
					}
					
					switch (comboBox1.SelectedIndex) 
					{
						case 0: //2 min 
							intTimerInterval = 2;
							break;
						case 1: //5 min
							intTimerInterval = 5;
							break;
						case 2: //10 min
							intTimerInterval = 10;
							break;
						case 3: // 15 min
							intTimerInterval = 15;
							break;
						case 4: //30 min
							intTimerInterval = 30;
							break;
						case 5: //60 Min
							intTimerInterval = 60;
							break;
					}
				
			
					//1 second = 1000 miliseconds
					//1 min = 60000 miliseconds
					timer2.Interval = intTimerInterval * 60000;
					timer1.Enabled = true;
					timer2.Enabled = true;
					
					//multiple min to 60 seconds to get seconds then -1 to get negetive value.
					intSeconds = (intTimerInterval * 60);
					
					break;
				case false:
					timer1.Enabled = false;
					timer2.Enabled = false;
					label4.Text = "";
					break;
			}
			
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{	
			//timer 1 for counting seconds.
			intSeconds = intSeconds - 1;
			label4.Text = "Next Change in : " + intSeconds.ToString() + " Seconds...";
		}
		
		void Timer2Tick(object sender, EventArgs e)
		{
			Random rnd = new Random();
			// creates a number between 0 and listbox number of images
			int myrandom = rnd.Next(0, listBox1.Items.Count - 1);
			
			listBox1.SetSelected(myrandom, true);
			
			SetWallpaper(pictureBox1.ImageLocation);
			
			//multiple min to 60 seconds to get seconds then -1 to get negetive value.
			intSeconds = (intTimerInterval * 60);
			
		}
		
		
	}
}
