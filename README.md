<h1>FitTrack Application (Version 1.0.1.3)</h1>

This version is to clean up and improve performance of the application.

Overview
========
FitTrack is a fitness tracking application designed to help athletes monitor their activities and caloric burn. The project includes various components organized into specific files and directories.<br/>
Defaults: the following accounts are implemented for test purpose.<br/>
[username, password] -> ['admin','admin'], ['user','Password'].


Project Structure
=================
Core<br/>
Purpose: Contains crucial classes and core functionalities essential for the application.<br/>

Components<br/>
Purpose: Customized user controls such as PasswordBox.xaml are located here.<br/>

Converters<br/>
Purpose: Used by Views to convert data types, like converting Boolean values to visibility in the UI.<br/>

Database<br/>
Purpose: Contains logic classes responsible for backend service interactions.<br/>

Dialogs<br/>
Purpose: Custom message boxes and UI dialog elements are defined here.<br/>

Styles<br/>
Purpose: Resource dictionaries that define styles and theming for the application's Views.<br/>

Utilities<br/>
Purpose: Helper classes for various operations, including unit conversions (e.g., imperial to metric).<br/>


Helper Classes
==============
LocalStorage.cs: Handles locally saved user settings.<br/>
Rules.cs: Stores predefined rules, such as allowed password lengths.<br/>
SystemInfo.cs: Retrieves necessary system information.<br/>
TestAppRunner.cs: Provides methods to simulate application startup and exit events for testing purposes.<br/>

Note: In case of database initiation or reset, use DB.cs under Database.<br/>

Important: This project is for educational and learning purposes only and is intended to demonstrate the fundamental concepts of software development, including interface design, data management and user interaction.<br/>
Interface design and user-experience may not meet the standards of a commercial project. Features may be basic or incomplete, and performance optimization is not effectively maintained.<br/>
Total Hours spent on Research, Development, Debug, and Testing: 210 hours (Approximately 43 days)


Contact
=======
For any assistant or questions concerned, contact: tpshine1234@gmail.com.