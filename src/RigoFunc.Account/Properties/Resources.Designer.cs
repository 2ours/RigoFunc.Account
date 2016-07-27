using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace RigoFunc.Account {
    /// <summary>
    ///    A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    internal class Resources {
        private static ResourceManager resourceMan;
        private static CultureInfo resourceCulture;
        
        internal Resources() {
        }
        
        /// <summary>
        ///    Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager {
            get {
                if (ReferenceEquals(resourceMan, null)) {
                    var temp = new ResourceManager("RigoFunc.Account.Resources", typeof(Resources).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        /// <summary>
        ///    Overrides the current thread's CurrentUICulture property for all
        ///    resource lookups using this strongly typed resource class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }

        /// <summary>
        ///    Looks up a localized string similar to The request argument is invalid..
        /// </summary>
        internal static string BadArgument {
            get {
                return ResourceManager.GetString("BadArgument", resourceCulture);
            }
        }

        /// <summary>
        ///    Looks up a localized string similar to Code {0} is invalid or timeout with 3 minutes.
        /// </summary>
        internal static string CodeInvalidOrTimeout {
            get {
                return ResourceManager.GetString("CodeInvalidOrTimeout", resourceCulture);
            }
        }

        /// <summary>
        ///    Looks up a localized string similar to An unknown failure has occurred..
        /// </summary>
        internal static string DefaultError {
            get {
                return ResourceManager.GetString("DefaultError", resourceCulture);
            }
        }

        /// <summary>
        ///    Looks up a localized string similar to Phone number {0} had been register..
        /// </summary>
        internal static string PhoneNumberHadBeenRegister {
            get {
                return ResourceManager.GetString("PhoneNumberHadBeenRegister", resourceCulture);
            }
        }

        /// <summary>
        ///    Looks up a localized string similar to Phone number or password error..
        /// </summary>
        internal static string PhoneNumberOrPasswordError {
            get {
                return ResourceManager.GetString("PhoneNumberOrPasswordError", resourceCulture);
            }
        }

        /// <summary>
        ///    Looks up a localized string similar to Register new user failure with phone number {0} and code {1}..
        /// </summary>
        internal static string RegisterNewUserFailure {
            get {
                return ResourceManager.GetString("RegisterNewUserFailure", resourceCulture);
            }
        }

        /// <summary>
        ///    Looks up a localized string similar to Reset password failure..
        /// </summary>
        internal static string ResetPasswordFailure {
            get {
                return ResourceManager.GetString("ResetPasswordFailure", resourceCulture);
            }
        }

        /// <summary>
        ///    Looks up a localized string similar to Send code failure..
        /// </summary>
        internal static string SendCodeFailure {
            get {
                return ResourceManager.GetString("SendCodeFailure", resourceCulture);
            }
        }

        /// <summary>
        ///    Looks up a localized string similar to Send the initial password failure..
        /// </summary>
        internal static string SendPasswordFailed {
            get {
                return ResourceManager.GetString("SendPasswordFailed", resourceCulture);
            }
        }

        /// <summary>
        ///    Looks up a localized string similar to User lockout failure..
        /// </summary>
        internal static string UserLockoutFailure {
            get {
                return ResourceManager.GetString("UserLockoutFailure", resourceCulture);
            }
        }

        /// <summary>
        ///    Looks up a localized string similar to User login failure because was lockout..
        /// </summary>
        internal static string UserLoginFailureBecauseIsLockout {
            get {
                return ResourceManager.GetString("UserLoginFailureBecauseIsLockout", resourceCulture);
            }
        }

        /// <summary>
        ///    Looks up a localized string similar to User login failure with phone number {0} and code {1}..
        /// </summary>
        internal static string UserLoginFailureWithPhoneAndCode {
            get {
                return ResourceManager.GetString("UserLoginFailureWithPhoneAndCode", resourceCulture);
            }
        }

        /// <summary>
        ///    Looks up a localized string similar to User not found by id {0}.
        /// </summary>
        internal static string UserNotFoundById {
            get {
                return ResourceManager.GetString("UserNotFoundById", resourceCulture);
            }
        }

        /// <summary>
        ///    Looks up a localized string similar to User not found by phone number {0}.
        /// </summary>
        internal static string UserNotFoundByPhoneNumber {
            get {
                return ResourceManager.GetString("UserNotFoundByPhoneNumber", resourceCulture);
            }
        }

        /// <summary>
        ///    Looks up a localized string similar to User update failure..
        /// </summary>
        internal static string UserUpdateFailure {
            get {
                return ResourceManager.GetString("UserUpdateFailure", resourceCulture);
            }
        }
    }
}
