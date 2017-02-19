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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Example.PWMLED
{
	class Program
	{
		static void Main(string[] args)
		{
			new Program().Run(args);
		}

		private void Run(string[] args)
		{
			Console.WriteLine("Press any key to exit...");

			using (var driver = new I2cDriver(ProcessorPin.Pin2, ProcessorPin.Pin3))
			{
				// random object to add some spice to the output
				Random rnd = new Random();

				// device support
				int i2cAddress = 0x40;
				var device = new Pca9685Connection(driver.Connect(i2cAddress));
				var pwmFrequency = Frequency.FromHertz(60);
				device.SetPwmUpdateRate(pwmFrequency);

				// task support
				CancellationTokenSource cancelSource = new CancellationTokenSource();
				List<Task> tasks = new List<Task>();

				// create the channel(s) and start task(s)
				PwmChannel[] pwmChannels = { PwmChannel.C0, }; // add more channels for more additional LEDs
				foreach (var channel in pwmChannels.Distinct())
				{
					// pass random sleep ms to have LEDs at different fade speeds
					Task task = Task.Run(() => RunLed(device, channel, rnd.Next(20, 41), cancelSource.Token));
				}

				// loop until key pressed
				while (!Console.KeyAvailable)
				{
					Thread.Sleep(250);
				}

				// cancel task(s) and wait
				cancelSource.Cancel();
				Task.WaitAll(tasks.ToArray());
			}
		}

		private void RunLed(Pca9685Connection device, PwmChannel channel, int sleepMs, CancellationToken cancelToken)
		{
			// PMW ticks range from 0 to 4095
			int increment = 100;
			int startCycleTick = 0;
			device.SetPwm(channel, 0, 0);
			while (!cancelToken.IsCancellationRequested)
			{
				// up we go
				for (int endCycleTick = 100; endCycleTick < 4100; endCycleTick += increment)
				{
					device.SetPwm(channel, startCycleTick, endCycleTick);
					Thread.Sleep(sleepMs);
				}

				// after we've gone up, show kindness and see if we're done
				if (cancelToken.IsCancellationRequested) break;

				// and down we go
				for (int endCycleTick = 4095; endCycleTick > 0; endCycleTick -= increment)
				{
					device.SetPwm(channel, startCycleTick, endCycleTick);
					Thread.Sleep(sleepMs);
				}
			}
		}
	}
}
