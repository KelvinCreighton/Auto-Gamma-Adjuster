# Auto-Gamma-Adjuster

This C# program dynamically adjusts the gamma level of the screen based on the average brightness of the pixels on the screen.

## Installation

1. Clone the repository: `git clone https://github.com/KelvinCreighton/Auto-Gamma-Adjuster.git`
2. Compile the program using the "compile.bat" (not required)

## Usage

1. Start the program by running the "AutoGammaAdjust.exe" file
2. The program will display the screen width and height.
3. The gamma level will be initially reset to 10.
4. The program will continuously calculate the average brightness of pixels on the screen and adjust the gamma level accordingly.
5. If the average brightness falls within a certain range (50 to 150), the gamma level will be mapped to a specific value between 3 and 10, providing a dynamic adjustment.
6. If the average brightness is below 50, the gamma level will be set to 3.
7. If the average brightness is above 150, the gamma level will be set to 10.
8. The program will continue running until manually terminated.

## Contributing

Contributions to the Gamma Adjuster Program are welcome! This program has many issues specifically the current exiting system. The unhooking and disposing do not occur along with the gamma reset feature which allows the gamma to stay set at whatever value after termination (current solution is to run the program and quickly exit it again to reset the gamma value back to its default state... makes me cringe). Another interesting feature would be to reinvent the current screen average and gamma setting caculations to introduce a non-linear mapping for smoother transition.
