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
using Raspberry.IO.GeneralPurpose;

namespace Example.RGBLED
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

			var leds = new OutputPinConfiguration[]
			{
				ProcessorPin.Pin17.Output(),
				ProcessorPin.Pin18.Output(),
				ProcessorPin.Pin27.Output(),
			};

			using (var connection = new GpioConnection(leds))
			{
				while (!Console.KeyAvailable)
				{
					foreach (var led in leds)
					{
						connection.Pins[led].Toggle();
						System.Threading.Thread.Sleep(500);
						connection.Pins[led].Toggle();
					}
				}
			}
		}
	}
}
