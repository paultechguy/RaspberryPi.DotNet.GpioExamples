// RaspberryPi.GpioExamples
//
// C# / Mono programming for the Raspberry Pi
// Copyright (c) 2017 Paul Carver
//
// RaspberryPi.GpioExamples is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// any later version.
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using Raspberry.IO.Components.Controllers.Pca9685;
using Raspberry.IO.GeneralPurpose;
using Raspberry.IO.InterIntegratedCircuit;
using UnitsNet;

namespace Example.PWMServo
{
	class Program
	{
		// configure min and max servo pulse lengths
		//
		// This values are for a TowerPro SG90 Servo at 60 hz frequency; you may have
		// to adjust based on other servos.  Hint: Use !ddd to input rotation which will
		// help you input raw cycle values to determine the min/max for your servo.
		//
		private int _servoMin = 190; // min pulse length out of 4096
		private int _servoMax = 680; // max pulse length out of 4096

		static void Main(string[] args)
		{
			new Program().Run(args);
		}

		private void Run(string[] args)
		{
			using (var driver = new I2cDriver(ProcessorPin.Pin2, ProcessorPin.Pin3))
			{
				// which pwm channel; allow override from args[0] as 0..n
				int c = 0;
				if (args.Length > 0)
				{
					Int32.TryParse(args[0], out c);
				}
				PwmChannel channel = (PwmChannel)c;
				Console.WriteLine($"Using PCA9685 channel {channel.ToString()}");

				// allow override of min/max cycle
				if (args.Length > 1)
				{
					Int32.TryParse(args[1], out _servoMin);
				}
				if (args.Length > 2)
				{
					Int32.TryParse(args[2], out _servoMax);
				}

				int i2cAddress = 0x40;
				Console.WriteLine($"Using i2C address 0x{i2cAddress:x}");

				// device support and prep channel
				var device = new Pca9685Connection(driver.Connect(i2cAddress));
				device.SetPwm(channel, 0, 0);

				int freq = 60;
				Console.WriteLine($"Setting PWM frequency to {freq}");
				var pwmFrequency = Frequency.FromHertz(freq);
				device.SetPwmUpdateRate(pwmFrequency);


				Console.WriteLine("\nPress any key to exit...\n");

				// loop until ENTER
				string input;
				do
				{
					Console.Write("Enter rotation 0 to 180 or RETURN to exit? ");
					input = Console.ReadLine().Trim();

					int cycle;
					if (TryParse(input, out cycle))
					{
						Console.WriteLine($"Setting PWM channel {channel.ToString()} to {cycle}");
						device.SetPwm(channel, 0, cycle);
					}
				} while (input != string.Empty);
			}
		}

		private bool TryParse(string s, out int cycle)
		{
			cycle = -1;
			int degrees;

			// if starts with !, this is a raw cycle value to help test min/max for
			// specific types of servos
			if (s.StartsWith("!"))
			{
				cycle = Int32.Parse(s.Substring(1));
			}
			else if (Int32.TryParse(s, out degrees))
			{
				if (degrees >= 0 && degrees <= 180)
				{
					cycle = (int)((((_servoMax - _servoMin) / 180m) * degrees) + _servoMin);
				}
			}

			bool status = cycle >= 0;
			if (!status && s.Length > 0)
			{
				Console.Error.WriteLine("%Invalid input");
			}

			return status;
		}
	}
}
