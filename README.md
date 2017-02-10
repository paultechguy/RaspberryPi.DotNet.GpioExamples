GPIO .NET Examples  
Copyright (c) 2017 Paul Carver

# Examples Introduction
The GPIO .NET Examples repository was created to help developers learn how to develop C# applications on the Raspberry Pi, quickly and easily.  The development platform uses Microsoft Visual Studio for development and executes using [Mono](http://www.mono-project.com/) on a Raspberry Pi running Linux. The repository contains several examples which come packaged with:
- C# Source Code
- [Fritzing](http://fritzing.org) project file
- GPIO pinout connection images
- Breadboard connection images

As a developer, having the above artifacts makes it extremely easy to start taking advantage of the GPIO pins on the Raspberry Pi using the C# programming language.

The examples included with this repository are:

* Magnetic buzzer
* Single color LED
* Three color RGB LED
* Magnetic buzzer / single color LED combo
* Photocell (light detection)
* DHT11 Temperature Sensor
* TMP102 Temperature Sensor
* PWM Single color LED (pulse width modulation)
* PWM three color LED (pulse width modulation)

The GPIO .NET Examples source code leverages the publicly available [Raspberry# IO](https://github.com/raspberry-sharp/raspberry-sharp-io) respository.

# Requirements
The respository has been tested with:

* Debian 8.0 (jessie)
* Mono 4.6.2*
* .NET 4.6.2
* Raspberry Pi 2 B hardware board
* [Raspberry# IO](https://github.com/raspberry-sharp/raspberry-sharp-io) as of February 1, 2017

*To install Mono, see the [Mono download page](http://www.mono-project.com/download/).

# Building Examples
The GPIO DotNet Examples solution, RaspberryPi.GpioExamples.sln, can be built on a Windows computer using Visual Studio 2015.  You must have .NET 4.6.2+ installed.  After building the solution, copy the output for a specific example to a directory of your choosing on the Raspberry Pi; the copy should include all files and sub-directories in the example Release or Debug directory.

# Executing Examples
MANAGING THE GPIO HEADER AND CONNECTING ELECTRONIC COMPONENTS TO THE PI REQUIRES GREAT CARE. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS OF RASPBERRYPI.DOTNET.GPIOEXAMPLES BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

You can execute any example from a standard Linux terminal window on the Raspberry Pi. Before executing the example though, you will first need to set up your breadboard and connect it to the GPIO pins on the Raspberry Pi (see the **Breadboard Setup** section below).

Once your breadboard is set up, you can invoke an example executable using [Mono](http://www.mono-project.com/).  This should be done using the Linux *sudo* command since the example requires low-level permissions to access the Pi GPIO capabilities. For example:

	sudo mono Example.BlinkingLED.exe

**Breadboard Setup**  
You can view helpful images to assist in setting up your breadboard for each GPIO example; the images are located in the *Content* directory for each example.There is also a [Fritzing](http://fritzing.org/) project file. Be sure to configure your breadboard to using the same GPIO header pins so that each example program will work correctly.

# Contact
You can contact us at <raspberrypi@paultechguy.com>.