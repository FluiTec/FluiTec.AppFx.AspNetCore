namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models
{
    /// <summary>   A data Model for the update. </summary>
    public class UpdateModel
    {
        /// <summary>   Gets or sets a value indicating whether this object is updated. </summary>
        /// <value> True if updated, false if not. </value>
        public bool Updated { get; set; }

        /// <summary>   Gets or sets a value indicating whether the successfully was updated. </summary>
        /// <value> True if successfully updated, false if not. </value>
        public bool SuccessfullyUpdated { get; set; }

        /// <summary>   Updates the success. </summary>
        public void UpdateSuccess()
        {
            Updated = true;
            SuccessfullyUpdated = true;
        }

        /// <summary>   Updates the model. </summary>
        public void Update()
        {
            Updated = true;
        }
    }
}