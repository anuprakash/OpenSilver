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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;
using OpenSilver.Internal.Controls;

#if MIGRATION
using System.Windows.Media;
#else
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Markup;
#endif

#if MIGRATION
namespace System.Windows.Controls
#else
namespace Windows.UI.Xaml.Controls
#endif
{
    /// <summary>
    /// Provides the base class for defining a new control that encapsulates related
    /// existing controls and provides its own logic.
    /// </summary>
    [ContentProperty("Content")]
    public partial class UserControl : Control
    {
        /// <summary> 
        /// Returns enumerator to logical children.
        /// </summary>
        /*protected*/ internal override IEnumerator LogicalChildren
        {
            get
            {
                if (this.Content == null)
                {
                    return EmptyEnumerator.Instance;
                }

                // otherwise, its logical children is its visual children
                return new SingleChildEnumerator(this.Content);
            }
        }

#region Constructors

        static UserControl()
        {
            // UseContentTemplate
            ControlTemplate template = new ControlTemplate();
            template.SetMethodToInstantiateFrameworkTemplate((owner) =>
            {
                TemplateInstance instance = new TemplateInstance();

                instance.TemplateOwner = owner;
                instance.TemplateContent = ((UserControl)owner).Content as FrameworkElement;

                return instance;
            });
            template.Seal();

            UseContentTemplate = template;
        }

        public UserControl()
        {
            IsTabStop = false; //we want to avoid stopping on this element's div when pressing tab.
        }

#endregion Constructors

        /// <summary>
        /// Gets or sets the content that is contained within a user control.
        /// </summary>
        public UIElement Content
        {
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Identifies the Content dependency property
        /// </summary>
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(
                nameof(Content),
                typeof(UIElement),
                typeof(UserControl),
                new PropertyMetadata(null, OnContentChanged));

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UserControl uc = (UserControl)d;

            uc.TemplateChild = null;
            uc.RemoveLogicalChild(e.OldValue);
            uc.AddLogicalChild(e.NewValue);

            if (VisualTreeHelper.GetParent(uc) != null)
            {
                uc.InvalidateMeasureInternal();
            }
        }

        /// <summary>
        /// Gets the element that should be used as the StateGroupRoot for VisualStateMangager.GoToState calls
        /// </summary>
        internal override FrameworkElement StateGroupsRoot
        {
            get
            {
                return Content as FrameworkElement;
            }
        }

        internal override FrameworkTemplate TemplateCache
        {
            get { return UseContentTemplate; }
            set { }
        }

        internal override FrameworkTemplate TemplateInternal
        {
            get { return UseContentTemplate; }
        }

        private static ControlTemplate UseContentTemplate
        {
            get;
        }
    }
}
