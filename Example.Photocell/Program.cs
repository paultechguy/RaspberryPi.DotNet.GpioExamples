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
using System.Threading;
using Raspberry.IO.GeneralPurpose;

namespace Example.Photocell
{
	class Program
	{
		private const int MaxReads = 30000;

		static void Main(string[] args)
		{
			new Program().Run(args);
		}

		private void Run(string[] args)
		{
			Console.WriteLine("Press any key to exit...");

			var connection = new MemoryGpioConnectionDriver();
			var pcell = connection.InOut(ProcessorPin.Pin18);

			//
			// See: https://learn.adafruit.com/photocells/using-a-photocell#bonus-reading-photocells-without-analog-pins
			//

			while (!Console.KeyAvailable)
			{
				int reads = 0;

				pcell.AsOutput();
				pcell.Write(false);
				System.Threading.Thread.Sleep(100); // allow to go low/false

				pcell.AsInput();
				while (pcell.Read() == false)
				{
					++reads;
					if (reads >= MaxReads)
					{
						break;
					}
				}

				//
				// Note: first couple of reads may be zero
				//

				string output = null;
				if (reads >= MaxReads)
				{
					output = $"Dark:\treads={reads}";
				}
				else if (reads > 2500)
				{
					output = $"Medium:\treads={reads}";
				}
				else
				{
					output = $"Light:\treads={reads}";
				}
				Console.WriteLine(output);

				// wait in between reads
				System.Threading.Thread.Sleep(3000);
			}
		}
	}
}
