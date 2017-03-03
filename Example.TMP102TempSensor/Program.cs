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

using Raspberry.IO.GeneralPurpose;
using Raspberry.IO.InterIntegratedCircuit;
using System;

namespace Example.TMP102TempSensor
{
	class Program
	{
		static void Main(string[] args)
		{
			new Program().Run(args);
		}

		private void Run(string[] args)
		{
			//
			// see the end of this file more information on reading the TMP102
			//

			Console.WriteLine("Press any key to exit...\n");

			int seconds = 3;
			Console.WriteLine("TMP102 (Model: SEN-11931): Measure temperature");
			Console.WriteLine($"Measure: I2C every {seconds} second(s)");

			int i2cAddress = 0x48;
			using (var driver = new I2cDriver(ProcessorPin.Pin2, ProcessorPin.Pin3))
			{
				I2cDeviceConnection connection = driver.Connect(i2cAddress);
				while (!Console.KeyAvailable)
				{
					byte[] data = connection.Read(2);

					// the msb is the first byte on linux
					byte msb = data[0];

					// the lsb is the second byte on linux
					byte lsb = data[1];

					// now combine then back together with msb first
					int temperature = ((msb << 8) | lsb) >> 4;

					Console.WriteLine(
						"Temperature is {0}°C, {1}°F",
						temperature * .0625,
						(temperature * .0625 * 1.8) + 32);

					System.Threading.Thread.Sleep(seconds * 1000);
				}
			}
		}
	}
}

// This data comes from the TMP102 documentation.
//
// The Temperature Register of the TMP102 is configured as a 12-bit, read-only register
// (Configuration Register EM bit = '0', see the Extended Mode section), or as a 13-bit,
// read-only register (Configuration Register EM bit = '1') that stores the output of the
// most recent conversion.  Two bytes must be read to obtain data, and are described in
// Table 3 and Table 4. Note that byte 1 is the most significant byte, followed by
// byte 2, the least significant byte.  The first 12 bits (13 bits in Extended mode) are
// used to indicate temperature.  The least significant byte does not have to be read if
// that information is not needed.  The data format for temperature is summarized in
// Table 5 and Table 6. One  LSB equals 0.0625°C.  Negative numbers are represented in
// binary twos complement format.
//
// I2C address should be 0x48.
