# FitTrack Application (Version 1.0.2.2)

## Overview

FitTrack is a fitness tracking application designed to help athletes monitor their activities and caloric burn. The project includes various components organized into specific files and directories.

**Defaults:** The following accounts are implemented for testing purposes:  
- **[username, password]** -> ['admin', 'admin'], ['user', 'Password'].

## Version History

- **v1.0.2.2**: Cleans up and improves application performance for better maintainability and development. Primarily focused on revising `*.xaml` files for cleaner code.

## Project Structure

- **Core**  
  _Purpose_: Contains essential classes and core functionalities crucial for the application.
  
- **Components**  
  _Purpose_: Includes customized user controls, such as `PasswordBox.xaml`.
  
- **Converters**  
  _Purpose_: Used by Views to convert data types, such as converting Boolean values to visibility in the UI.

- **Database**  
  _Purpose_: Contains logic classes responsible for backend service interactions.

- **Dialogs**  
  _Purpose_: Defines custom message boxes and UI dialog elements.

- **Styles**  
  _Purpose_: Contains resource dictionaries that define styles and theming for the application's Views.

- **Utilities**  
  _Purpose_: Helper classes for various operations, including unit conversions (e.g., imperial to metric).

---

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