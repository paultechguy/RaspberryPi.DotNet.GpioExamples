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
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Example.ButtonLED
{
	class Program
	{
		static void Main(string[] args)
		{
			new Program().Run(args);
		}

		private void Run(string[] args)
		{
			var led = ProcessorPin.Pin18
				.Output()
				.Name("LED")
				.Revert()
				.Enable();

			using (var connection = new GpioConnection(led))
			{
				var button = ProcessorPin.Pin2
				.Input()
				.Name("Button")
				.Revert()
				.Switch()
				.Enable()
				.OnStatusChanged(b =>
				{
					Console.WriteLine("Button/LED switched {0}", b ? "On" : "Off");
					connection.Pins["LED"].Toggle();
				});

				connection.Add(button);

				Console.WriteLine("Press Enter to quit...");
				Console.ReadLine();
			}
		}
	}
}
