namespace FitTrack.Core
{   
     /// <summary>
     /// Represents a message for changing the view in the application.
     /// </summary>
    public class ChangeViewMessage
    {
        /// <summary>
        /// Gets or sets ViewModel associated with this message.
        /// </summary>
        public object ViewModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeViewMessage"/> class.
        /// </summary>
        /// <param name="viewModel">ViewModel to be associated with this message.</param>
        public ChangeViewMessage(object viewModel)
        {
            ViewModel = viewModel;  
        }
    }
}
