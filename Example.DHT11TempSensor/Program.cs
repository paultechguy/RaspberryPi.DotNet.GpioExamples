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

using Raspberry.IO.Components.Sensors.Temperature.Dht;
using Raspberry.IO.GeneralPurpose;
using System;

namespace Example.DHT11TempSensor
{
	class Program
	{
		static void Main(string[] args)
		{
			new Program().Run(args);
		}

		private void Run(string[] args)
		{
			const ProcessorPin measurePin = ProcessorPin.Pin18;
			int seconds = 3;

			Console.WriteLine("DHT-11 / DHT-22: Measure temperature and humidity");
			Console.WriteLine($"Measure: {measurePin} every {seconds} second(s)");
			Console.WriteLine();

			var driver = GpioConnectionSettings.GetBestDriver(GpioConnectionDriverCapabilities.CanChangePinDirectionRapidly);

			using (var pin = driver.InOut(measurePin))
			using (var dhtConnection = new Dht11Connection(pin))
			{
				while (!Console.KeyAvailable)
				{
					var data = dhtConnection.GetData();
					if (data != null)
					{
						Console.WriteLine(
							$"{data.RelativeHumidity.Percent:0.00}% humidity, {data.Temperature.DegreesCelsius:0.0}°C / {data.Temperature.DegreesFahrenheit:0.0}°F, {data.AttemptCount} attempt(s)");
					}
					else
					{
						Console.WriteLine("Unable to read sensor data");
					}

					System.Threading.Thread.Sleep(seconds * 1000);
				}
			}
		}

	}
}
