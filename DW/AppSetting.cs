/*
 * Created by SharpDevelop.
 * User: DVDLesher
 * Date: 2017/02/01
 * Time: 12:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace DailyWeather
{
	/// <summary>
	/// Description of Setting.
	/// </summary>
	public class AppSetting
	{
		//Do daily check at 9AM by default
		private TimeSpan alarmTime = new TimeSpan(9, 0, 0);
		//Do check every 1 day by default
		private TimeSpan repeatTime = new TimeSpan(1, 0, 0, 0);

		public AppSetting()
		{
			// Read the setting file to override the default setting.
			if (true) {
				// File exists
				try {
					
				} catch (Exception e) {
					// If there is a problem with the file, reset to default setting
					alarmTime = new TimeSpan(9, 0, 0);
					repeatTime = new TimeSpan(1, 0, 0, 0);
				}
			}
		}
		
		public TimeSpan getAlarmTime()
		{
			return alarmTime;
		}
		
		public TimeSpan getRepeatTime()
		{
			return repeatTime;
		}
		
		public void setAlarmTime(TimeSpan alarmTime)
		{
			this.alarmTime = alarmTime;
		}
		
		public void setRepeatTime(TimeSpan repeatTime)
		{
			this.repeatTime = repeatTime;
		}
	}
}
