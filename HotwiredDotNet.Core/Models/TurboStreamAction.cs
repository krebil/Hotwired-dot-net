namespace HotwiredDotNet.Core.Models
{
    /// <summary>
    /// Represents the action to perform on a <a href="https://turbo.hotwired.dev/reference/streams#the-seven-actions">turbo-stream target</a>. 
    /// </summary>
    public enum TurboStreamAction
    {
        /// <summary>
        /// <a href="https://turbo.hotwired.dev/reference/streams#append">Append</a> Append the template to the target.
        /// </summary>
        Append,

        /// <summary>
        /// <a href="https://turbo.hotwired.dev/reference/streams#prepend">Prepend</a> the template to the target.
        /// </summary>
        Prepend,

        /// <summary>
        /// <a href="https://turbo.hotwired.dev/reference/streams#replace">Replace</a> the target with the template.
        /// </summary>
        Replace,

        /// <summary>
        /// <a href="https://turbo.hotwired.dev/reference/streams#update">Update</a> the target with the template.
        /// </summary>
        Update,

        /// <summary>
        /// <a href="https://turbo.hotwired.dev/reference/streams#remove">Remove</a> the target.
        /// </summary>
        Remove,

        /// <summary>
        /// Insert the template  <a href="https://turbo.hotwired.dev/reference/streams#before">before</a> the target.
        /// </summary>
        Before,

        /// <summary>
        /// Insert the template <a href="https://turbo.hotwired.dev/reference/streams#after">after</a> the target.
        /// </summary>
        After
    }
}