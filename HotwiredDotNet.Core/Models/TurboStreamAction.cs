namespace HotwiredDotNet.Core.Models
{
    /// <summary>
    /// Represents the action to perform on a <see href="https://turbo.hotwired.dev/reference/streams#the-seven-actions">turbo-stream target</see>. 
    /// </summary>
    public enum TurboStreamAction
    {
        /// <summary>
        /// <see href="https://turbo.hotwired.dev/reference/streams#append">Append</see> Append the template to the target.
        /// </summary>
        Append,

        /// <summary>
        /// <see href="https://turbo.hotwired.dev/reference/streams#prepend">Prepend</see> the template to the target.
        /// </summary>
        Prepend,

        /// <summary>
        /// <see href="https://turbo.hotwired.dev/reference/streams#replace">Replace</see> the target with the template.
        /// </summary>
        Replace,

        /// <summary>
        /// <see href="https://turbo.hotwired.dev/reference/streams#update">Update</see> the target with the template.
        /// </summary>
        Update,

        /// <summary>
        /// <see href="https://turbo.hotwired.dev/reference/streams#remove">Remove</see> the target.
        /// </summary>
        Remove,

        /// <summary>
        /// Insert the template  <see href="https://turbo.hotwired.dev/reference/streams#before">before</see> the target.
        /// </summary>
        Before,

        /// <summary>
        /// Insert the template <see href="https://turbo.hotwired.dev/reference/streams#after">after</see> the target.
        /// </summary>
        After
    }
}