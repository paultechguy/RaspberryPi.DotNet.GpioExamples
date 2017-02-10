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
using Raspberry.IO.GeneralPurpose.Behaviors;

namespace Example.BlinkingLED
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

			var led = ProcessorPin.Pin23
				.Output();

			using (var connection = new GpioConnection(led))
			{
				while (!Console.KeyAvailable)
				{
					connection.Toggle(led);
					System.Threading.Thread.Sleep(250);
				}
			}

			// Another approach using a behavior with finite number of blinks (10)
			//BlinkBehavior behavior = new BlinkBehavior(new PinConfiguration[] { led }) { Interval = new TimeSpan(0, 0, seconds: 1), Count = 10 };
			//connection.Start(behavior);
			//while (!Console.KeyAvailable)
			//{
			//	System.Threading.Thread.Sleep(200);
			//}
			//connection.Stop(behavior);

		}
	}
}
