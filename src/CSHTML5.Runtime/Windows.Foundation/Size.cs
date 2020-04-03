﻿

/*===================================================================================
* 
*   Copyright (c) Userware/OpenSilver.net
*      
*   This file is part of the OpenSilver Runtime (https://opensilver.net), which is
*   licensed under the MIT license: https://opensource.org/licenses/MIT
*   
*   As stated in the MIT license, "the above copyright notice and this permission
*   notice shall be included in all copies or substantial portions of the Software."
*  
\*====================================================================================*/


using CSHTML5.Internal;
using DotNetForHtml5.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if MIGRATION
namespace System.Windows
#else
namespace Windows.Foundation
#endif
{
    /// <summary>
    /// Describes the width and height of an object.
    /// </summary>
#if FOR_DESIGN_TIME
    [TypeConverter(typeof(SizeConverter))]
#endif
    public partial struct Size
    {
        double _width;
        double _height;

        static Size()
        {
            TypeFromStringConverters.RegisterConverter(typeof(Size), s => Parse(s));
        }

        /// <summary>
        /// Initializes a new instance of the Windows.Foundation.Size
        /// structure and assigns it an initial width and height.
        /// </summary>
        /// <param name="width">The initial width of the instance of Windows.Foundation.Size.</param>
        /// <param name="height">The initial height of the instance of Windows.Foundation.Size.</param>
        public Size(double width, double height)
        {
#if !BRIDGE

            if ((!double.IsNaN(width) && width < 0) || (!double.IsNaN(height) && height < 0))
            {
                throw new ArgumentException("Width and Height cannot be negative.");
            }
#else
            if(width < 0 || height < 0)
            {
                throw new ArgumentException("Width and Height cannot be negative.");
            }
#endif
            this._width = width;
            this._height = height;
        }

        /// <summary>
        /// Compares two instances of Windows.Foundation.Size for
        /// inequality.</summary>
        /// <param name="size1">The first instance of Windows.Foundation.Size to compare.</param>
        /// <param name="size2">The second instance of Windows.Foundation.Size to compare.</param>
        /// <returns>
        /// true if the instances of Windows.Foundation.Size are not equal; otherwise
        /// false.
        /// </returns>
        public static bool operator !=(Size size1, Size size2)
        {
            return size1.Height != size2.Height || size1.Width != size2.Width;
        }

        /// <summary>
        /// Compares two instances of Windows.Foundation.Size for
        /// equality.</summary>
        /// <param name="size1">The first instance of Windows.Foundation.Size to compare.</param>
        /// <param name="size2">The second instance of Windows.Foundation.Size to compare.</param>
        /// <returns>
        /// true if the two instances of Windows.Foundation.Size are equal; otherwise
        /// false.
        /// </returns>
        public static bool operator ==(Size size1, Size size2)
        {
            return size1.Height == size2.Height && size1.Width == size2.Width;
        }

        /// <summary>
        /// Gets a value that represents a static empty Windows.Foundation.Size.
        /// </summary>
        public static Size Empty
        {
            get
            {
                return new Size
                {
                    _width = double.NegativeInfinity,
                    _height = double.NegativeInfinity
                };
            }
        }

        /// <summary>
        /// Gets or sets the height of this instance of Windows.Foundation.Size in pixels. The default is 0. The value cannot be negative.
        /// </summary>
        public double Height
        {
            get
            {
                return this._height;
            }
            set
            {
                if (this.IsEmpty)
                {
                    throw new InvalidOperationException("Cannot modify Empty size.");
                }
                if(value < 0)
                {
                    throw new ArgumentException("Height cannot be negative.");
                }
                this._height = value;
            }
        }
      
        /// <summary>
        /// Gets a value that indicates whether this instance of
        /// Windows.Foundation.Size is Windows.Foundation.Size.Empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this._width < 0;
            }
        }
      
        /// <summary>
        /// Gets or sets the width of this instance of Windows.Foundation.Size.
        /// </summary>
        public double Width
        {
            get
            {
                return this._width;
            }
            set
            {
                if (this.IsEmpty)
                {
                    throw new InvalidOperationException("Cannot modify Empty size.");
                }
                if(value < 0)
                {
                    throw new ArgumentException("Width cannot be negative.");
                }
                this._width = value;
            }
        }

        public static bool Equals(Size size1, Size size2)
        {
            if (size1.IsEmpty)
            {
                return size2.IsEmpty;
            }
            else
            {
                return size1.Width == size2.Width && size1.Height == size2.Height;
            }
        }

        /// <summary>
        /// Compares an object to an instance of Windows.Foundation.Size
        /// for equality.
        /// </summary>
        /// <param name="o">The System.Object to compare.</param>
        /// <returns>true if the sizes are equal; otherwise, false.</returns>
        public override bool Equals(object o)
        {
            if (o is Size value)
            {
               return Size.Equals(this, value);
            }
            return false;
        }


        /// <summary>
        /// Compares a value to an instance of Windows.Foundation.Size
        /// for equality.
        /// </summary>
        /// <param name="value">The size to compare to this current instance of Windows.Foundation.Size.</param>
        /// <returns>true if the instances of Windows.Foundation.Size are equal; otherwise, false.</returns>
        public bool Equals(Size value)
        {
            return Size.Equals(this, value);
        }

        /// <summary>
        /// Gets the hash code for this instance of Windows.Foundation.Size.
        /// </summary>
        /// <returns>The hash code for this instance of Windows.Foundation.Size.</returns>
        public override int GetHashCode()
        {
            if (this.IsEmpty)
            {
                return 0;
            }
            else
            {
                // Perform field-by-field XOR of HashCodes
                return this.Width.GetHashCode() ^ this.Height.GetHashCode();
            }
        }

        /// <summary>
        /// Returns a string representation of this Windows.Foundation.Size.
        /// </summary>
        /// <returns>A string representation of this Windows.Foundation.Size.</returns>
        public override string ToString()
        {
            if (this.IsEmpty)
            {
                return "Empty";
            }
            return Width + "," + Height;
        }

        public static Size Parse(string sizeAsString)
        {
            string[] splittedString = sizeAsString.Split(new[]{',', ' '}, StringSplitOptions.RemoveEmptyEntries);

            if (splittedString.Length == 2)
            {
                double width, height;
#if OPENSILVER
                if (double.TryParse(splittedString[0], NumberStyles.Any, CultureInfo.InvariantCulture, out width) && 
                    double.TryParse(splittedString[1], NumberStyles.Any, CultureInfo.InvariantCulture, out height))
#else
                if (double.TryParse(splittedString[0], out width) &&
                    double.TryParse(splittedString[1], out height))
#endif
                    return new Size(width, height);
            }
            
            throw new FormatException(sizeAsString + " is not an eligible value for a Size");
        }

    }
}