﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Forms.PropertyGridInternal;
using Xunit;
using static System.Windows.Forms.PropertyGridInternal.PropertyGridView;

namespace System.Windows.Forms.Tests.AccessibleObjects
{
    public class PropertyGridViewAccessibleObjectTests
    {
        [Theory]
        [InlineData(NativeMethods.UIA_GridPatternId)]
        [InlineData(NativeMethods.UIA_TablePatternId)]
        public void PropertyGridAccessibleObject_Supports_TablePattern(int pattern)
        {
            PropertyGrid propertyGrid = new PropertyGrid();
            ComboBox comboBox = new ComboBox();
            propertyGrid.SelectedObject = comboBox;

            var propertyGridAccessibleObject = new PropertyGridAccessibleObject(propertyGrid);

            // First child should be PropertyGrid toolbox.
            var firstChild = propertyGridAccessibleObject.FragmentNavigate(
                UnsafeNativeMethods.NavigateDirection.FirstChild) as AccessibleObject;

            // Second child entry should be PropertyGridView.
            var gridViewChild = firstChild.FragmentNavigate(
                UnsafeNativeMethods.NavigateDirection.NextSibling) as AccessibleObject;

            Assert.True(gridViewChild.IsPatternSupported(pattern));
        }

        [Theory]
        [InlineData(NativeMethods.UIA_GridItemPatternId)]
        [InlineData(NativeMethods.UIA_TableItemPatternId)]
        public void PropertyGridViewAccessibleObject_Supports_TablePattern(int pattern)
        {
            PropertyGrid propertyGrid = new PropertyGrid();
            ComboBox comboBox = new ComboBox();
            propertyGrid.SelectedObject = comboBox;

            var defaultGridEntry = propertyGrid.GetDefaultGridEntry();
            var parentGridEntry = defaultGridEntry.ParentGridEntry; // Category which has item pattern.
            var accessibleObject = parentGridEntry.AccessibilityObject;
            Assert.True(accessibleObject.IsPatternSupported(pattern));
        }

        [WinFormsTheory]
        [InlineData((int)UnsafeNativeMethods.NavigateDirection.Parent)]
        [InlineData((int)UnsafeNativeMethods.NavigateDirection.NextSibling)]
        [InlineData((int)UnsafeNativeMethods.NavigateDirection.PreviousSibling)]
        [InlineData((int)UnsafeNativeMethods.NavigateDirection.FirstChild)]
        [InlineData((int)UnsafeNativeMethods.NavigateDirection.LastChild)]
        public void PropertyGridViewAccessibleObject_FragmentNavigate_DoesNotThrowExpection_WithoutOwnerGrid(int direction)
        {
            using PropertyGrid propertyGrid = new();
            using PropertyGridView propertyGridView = new(null, null);
            PropertyGridViewAccessibleObject accessibleObject = new(propertyGridView, propertyGrid);
            Assert.Null(accessibleObject.FragmentNavigate((UnsafeNativeMethods.NavigateDirection)direction));
        }
    }
}
