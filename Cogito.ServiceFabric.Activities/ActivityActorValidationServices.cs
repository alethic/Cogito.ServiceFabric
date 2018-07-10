using System;
using System.Activities.Validation;

namespace Cogito.ServiceFabric.Activities
{

    /// <summary>
    /// Provides the functionality to validate the workflow activity of an <see cref="ActivityActor"/>.
    /// </summary>
    public static class ActivityActorValidationServices
    {

        /// <summary>
        /// Verifies that a workflow activity on an <see cref="ActivityActor"/> is correctly configured according to
        /// the validation logic. This logic can be the <see cref="CodeActivity.CacheMetadata(CodeActivityMetadata)"/>
        /// method of the activitie to validate, or build and policy constraints, descriptive message, an error code,
        /// and other information.
        /// </summary>
        /// <param name="toValidate"></param>
        /// <returns></returns>
        public static ValidationResults Validate(ActivityActor toValidate)
        {
            if (toValidate == null)
                throw new ArgumentNullException(nameof(toValidate));

            return ActivityValidationServices.Validate(toValidate.CreateActivityInternal());
        }

        /// <summary>
        /// Verifies that a workflow activity on an <see cref="ActivityActor"/> is correctly configured according to
        /// the validation logic. This logic can be the <see cref="CodeActivity.CacheMetadata(CodeActivityMetadata)"/>
        /// method of the activities to validate, or build and policy constraints.
        /// </summary>
        /// <param name="toValidate"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ValidationResults Validate(ActivityActor toValidate, ValidationSettings settings)
        {
            if (toValidate == null)
                throw new ArgumentNullException(nameof(toValidate));
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            return ActivityValidationServices.Validate(toValidate.CreateActivityInternal(), settings);
        }

    }

}
