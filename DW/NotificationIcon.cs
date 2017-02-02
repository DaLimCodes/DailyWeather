/*
 * Created by SharpDevelop.
 * User: DVDLesher
 * Date: 2017/02/01
 * Time: 13:53
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

namespace DailyWeather
{
	public sealed class NotificationIcon
	{
		private NotifyIcon notifyIcon;
		private ContextMenu notificationMenu;
		private System.Threading.Timer timer;
		private String weatherText = "PLACEHOLDER MESSAGE";
		private AppSetting appSetting;
		static HttpClient client = new HttpClient();

		
		// TODO: make setting tweaking window
		// TODO: write setting to a file and read from it

		#region Initialize icon and menu
		public NotificationIcon()
		{
			appSetting = new AppSetting();
			notifyIcon = new NotifyIcon();
			notificationMenu = new ContextMenu(InitializeMenu());
			
			notifyIcon.MouseClick += IconClick;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationIcon));
			notifyIcon.Icon = (Icon)resources.GetObject("$this.Icon");
			notifyIcon.ContextMenu = notificationMenu;
			
			notifyIcon.BalloonTipTitle = "Daily Weather";
			notifyIcon.BalloonTipText = weatherText;
			notifyIcon.Text = weatherText;
			SetUpTimer(appSetting.getAlarmTime(), appSetting.getRepeatTime());
		}
		
		private MenuItem[] InitializeMenu()
		{
			MenuItem[] menu = new MenuItem[] {
				new MenuItem("About", menuAboutClick),
				new MenuItem("Exit", menuExitClick)
			};
			return menu;
		}
		#endregion
		
		#region Main - Program entry point
		/// <summary>Program entry point.</summary>
		/// <param name="args">Command Line Arguments</param>
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			bool isFirstInstance;
			// Please use a unique name for the mutex to prevent conflicts with other programs
			using (Mutex mtx = new Mutex(true, "DW", out isFirstInstance)) {
				if (isFirstInstance) {
					NotificationIcon notificationIcon = new NotificationIcon();
					notificationIcon.notifyIcon.Visible = true;
					Application.Run();
					notificationIcon.notifyIcon.Dispose();
				} else {
					// The application is already running
					// TODO: Display message box or change focus to existing application instance
				}
			} // releases the Mutex
		}
		#endregion
		
		private void SetUpTimer(TimeSpan alertTime, TimeSpan repeatTime)
		{
			DateTime current = DateTime.Now;
			TimeSpan timeToGo = alertTime - current.TimeOfDay;
			if (timeToGo < TimeSpan.Zero) {
				return;
			}
			timer = new System.Threading.Timer(x => this.requestDataFromServer(), null, timeToGo, repeatTime);
		}

		
		#region Event Handlers
		private void menuAboutClick(object sender, EventArgs e)
		{
			MessageBox.Show("About This Application");
		}
		
		private void menuExitClick(object sender, EventArgs e)
		{
			Application.Exit();
		}
		
		private void IconClick(object sender, EventArgs e)
		{
			requestDataFromServer();
		}
		#endregion
		
		private void requestDataFromServer(){
			JsonObject data = httpGetData();
			
		}
		
		private async Task<JsonObject> httpGetData()
		{
			// API usage
			// http://openweathermap.org/current#name
			//			
			// Use this for testing purpose
			// http://api.openweathermap.org/data/2.5/forecast/weather?zip=3051,AU&APPID=7f5e92bf53a48ebe24b14a7586423fca
			
			var httpClient = new HttpClient();
			var response = await httpClient.GetAsync("http://api.openweathermap.org/data/2.5/forecast/weather?zip=3051,AU&APPID=7f5e92bf53a48ebe24b14a7586423fca");

			//will throw an exception if not successful
			response.EnsureSuccessStatusCode();

			String content = await response.Content.ReadAsStringAsync();
			return await Task.Run(() => JsonObject.Parse(content));
   		}
	}
}
