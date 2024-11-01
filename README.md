FitTrack Application (Version 1.0.1.2)

Overview
========
FitTrack is a fitness tracking application designed to help athletes monitor their activities and caloric burn. The project includes various components organized into specific files and directories.
Defaults: the following accounts are implemented for test purpose.
[username, password] -> ['admin','admin'], ['user','Password'].


Project Structure
=================
Core
Purpose: Contains crucial classes and core functionalities essential for the application.

Components
Purpose: Customized user controls such as PasswordBox.xaml are located here.

Converters
Purpose: Used by Views to convert data types, like converting Boolean values to visibility in the UI.

Database
Purpose: Contains logic classes responsible for backend service interactions.

Dialogs
Purpose: Custom message boxes and UI dialog elements are defined here.

Styles
Purpose: Resource dictionaries that define styles and theming for the application's Views.

Utilities
Purpose: Helper classes for various operations, including unit conversions (e.g., imperial to metric).


Helper Classes
==============
LocalStorage.cs: Handles locally saved user settings.
Rules.cs: Stores predefined rules, such as allowed password lengths.
SystemInfo.cs: Retrieves necessary system information.
TestAppRunner.cs: Provides methods to simulate application startup and exit events for testing purposes.

Note: In case of database initiation or reset, use DB.cs under Database.

Important: This project is for educational and learning purposes only and is intended to demonstrate the fundamental concepts of software development, including interface design, data management and user interaction. Interface design and user-experience may not meet the standards of a commercial project. Features may be basic or incomplete, and performance optimization is not effectively maintained.
Total Hours spent on Research, Development, Debug, and Testing: 210 hours (Approximately 43 days)


Contact
=======
For any assistant or questions concerned, contact: tpshine1234@gmail.com