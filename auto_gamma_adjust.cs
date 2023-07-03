using System;
using System.Runtime.InteropServices;

namespace Program
{
	public class Program
	{
		// Get Pixel
		[DllImport("user32.dll")]
		public static extern IntPtr GetDesktopWindow();
		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowDC(IntPtr window);
		[DllImport("gdi32.dll")]
		public static extern uint GetPixel(IntPtr dc, int x, int y);
		[DllImport("user32.dll")]
		public static extern int ReleaseDC(IntPtr window, IntPtr dc);
		
		public static IntPtr DESKTOP = GetDesktopWindow();
		public static IntPtr DC = GetWindowDC(DESKTOP);
		
		public static int WIDTH = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
		public static int HEIGHT = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
		
		
		// Set Gamma
		[DllImport("gdi32.dll")]
		private static extern bool SetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);
		[DllImport("user32.dll")]
		static extern IntPtr GetDC(IntPtr hWnd);
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		
		private struct RAMP
		{
			[ MarshalAs(UnmanagedType.ByValArray, SizeConst=256)]
			public UInt16[] Red;
			[ MarshalAs(UnmanagedType.ByValArray, SizeConst=256)]
			public UInt16[] Green;
			[ MarshalAs(UnmanagedType.ByValArray, SizeConst=256)]
			public UInt16[] Blue;
		}
		private static RAMP ramp = new RAMP();




		public static void Main()
		{
			int updateSpeed = 100;	// At the cost of accuracy
			
			Console.WriteLine("Screen Width: " + WIDTH);
			Console.WriteLine("Screen Height: " + HEIGHT);
			Console.WriteLine();
			
			SetGamma(10);			// Reset Gamma
			
			while (true) {
				
				double average = 0;
				for (int y = 0; y < HEIGHT; y += updateSpeed) {
					for (int x = 0; x < WIDTH; x += updateSpeed) {
						int[] pixel = GetColourAt(x, y);
						double brightness = pixel[0] + pixel[1] + pixel[2];
						average += brightness;
					}
				}
				
				average /= (Math.Ceiling((double)HEIGHT/(double)updateSpeed) * Math.Ceiling((double)WIDTH/(double)updateSpeed));
				
				if (average <= 150 && average >= 50) {
					double outStart = 3;
					double outEnd = 10;
					double inStart = 25;
					double inEnd = 150;
					double mapGamma = outStart + ((outEnd-outStart) / (inEnd-inStart)) * (average - inStart);
					
					SetGamma(mapGamma);
					Console.WriteLine("1: " + average);
				}
				else if (average < 50) {
					SetGamma(3);
					Console.WriteLine("2: " + average);
				}
				else {
					SetGamma(10);
					Console.WriteLine("3: " + average);
				}
			}
			
			SetGamma(10);			// Reset Gamma
			ReleaseDC(DESKTOP, DC);	// Exit
		}
		
		private static int[] GetColourAt(int x, int y)
		{
			int c = (int)GetPixel(DC, x, y);
			return new int[] {(c >> 0)&0xff, (c >> 8)&0xff, (c >> 16)&0xff};
		}
		
		private static void SetGamma(double gamma)	// 3 to 44, 10 as default, 3 being the highest
		{
			ramp.Red = new ushort[256];
			ramp.Green = new ushort[256];
			ramp.Blue = new ushort[256];
			
			for (int i = 1; i < 256; i++)
			{
				ramp.Red[i] = ramp.Green[i] = ramp.Blue[i] =
				(ushort)(Math.Min(65535,
				Math.Max(0, Math.Pow((i+1) / 256.0, gamma*0.1) * 65535 + 0.5)));
			}
			SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref ramp);
		}
	}
}