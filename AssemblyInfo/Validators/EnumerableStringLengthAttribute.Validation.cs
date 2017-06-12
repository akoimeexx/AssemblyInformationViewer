using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.akoimeexx.utilities.assemblyinformation.Validators {
    [AttributeUsage(
        AttributeTargets.Property |
        AttributeTargets.Field |
        AttributeTargets.Parameter,
        AllowMultiple = false,
        Inherited = true
    )]
    public partial class EnumerableStringLengthAttribute : ValidationAttribute {
#region Properties
        internal uint MaxLength {
            get { return _maxLength; }
        } private readonly uint _maxLength;
        internal uint MinLength {
            get { return _minLength; }
        } private readonly uint _minLength;
#endregion Properties
    }
    public partial class EnumerableStringLengthAttribute {
#region Methods
        public override string FormatErrorMessage(string name) {
            return base.FormatErrorMessage(name);
        }
        public override bool IsValid(object value) {
            bool b = base.IsValid(value);
            try {
                if (
                    value != null && 
                    value as IEnumerable<string> == null
                )
                    throw new ValidationException(
                        "value must be of type IEnumerable<string>"
                    );
                foreach (string s in 
                    (value as IEnumerable<string>) ?? 
                    new string[] { }
                ) {
                    if (s.Length < MinLength)
                        throw new ValidationException(String.Format(
                            "string \"{0}\" is shorter than the minimum length of {1}", 
                            s, 
                            MinLength
                        ));
                    if (s.Length > MaxLength)
                        throw new ValidationException(String.Format(
                            "string \"{0}\" is longer than the maximum length of {1}",
                            s,
                            MaxLength
                        ));
                }
                b = true;
            } catch (Exception e) {
                b = false;
                throw e;
            }
            return b;
        }
#endregion Methods
    }
    public partial class EnumerableStringLengthAttribute {
#region Constructors & Destructor
        public EnumerableStringLengthAttribute() : this(uint.MaxValue) { }
        public EnumerableStringLengthAttribute(
            uint maxLength
        ) : this(uint.MinValue, maxLength) { }
        public EnumerableStringLengthAttribute(uint minLength, uint maxLength) {
            _minLength = minLength;
            _maxLength = maxLength;
        }
#endregion Constructors & Destructor
    }
}
