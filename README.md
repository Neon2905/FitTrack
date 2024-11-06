<h1>FitTrack Application (Version 1.0.2.2)</h1>

Overview
========
FitTrack is a fitness tracking application designed to help athletes monitor their activities and caloric burn. The project includes various components organized into specific files and directories.<br/>
Defaults: the following accounts are implemented for test purpose.<br/>
[username, password] -> ['admin','admin'], ['user','Password'].

Version History
===============
v1.0.2.2: Cleans up and improves performance of the application for maintainablity and development. Mainly revised *.xaml for cleaner code.

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


# Helper Classes

- **LocalStorage.cs**: Manages locally stored user settings.
- **Rules.cs**: Contains predefined rules, such as permissible password lengths.
- **SystemInfo.cs**: Retrieves essential system information for application use.
- **TestAppRunner.cs**: Provides utility methods to simulate application startup and exit events, primarily for testing purposes.

**Note**: For database initialization or reset, refer to the `DB.cs` class in the **Database** folder.

---

## Important Information

This project is designed primarily for educational and learning purposes. It demonstrates fundamental software development concepts, such as interface design, data management, and user interaction. 

Please note:
- The interface design and user experience may not align with the standards of a commercial-grade project.
- Some features may be basic or incomplete.
- Performance optimization has not been a primary focus.

**Total Hours Spent on Research, Development, Debugging, and Testing**: 210 hours (approximately 43 days)

---

## Contact

- For ongoing development updates, visit the GitHub repository: [FitTrack GitHub Repository](https://github.com/Neon2905/FitTrack.git)
- For inquiries or assistance, reach out via email: [Aurthor](mailto:tpshine1234@gmail.com)