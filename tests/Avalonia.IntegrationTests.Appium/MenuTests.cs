﻿using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Interactions;
using Xunit;

namespace Avalonia.IntegrationTests.Appium
{
    [Collection("Default")]
    public class MenuTests
    {
        private readonly AppiumDriver _session;

        public MenuTests(TestAppFixture fixture)
        {
            _session = fixture.Session;

            var tabs = _session.FindElement(MobileBy.AccessibilityId("MainTabs"));
            var tab = tabs.FindElement(MobileBy.Name("Menu"));
            tab.Click();

            var reset = _session.FindElement(MobileBy.AccessibilityId("MenuClickedMenuItemReset"));
            reset.Click();

            var clickedMenuItem = _session.FindElement(MobileBy.AccessibilityId("ClickedMenuItem"));
            Assert.Equal("None", clickedMenuItem.Text);
        }

        [Fact]
        public void Click_Child()
        {
            var rootMenuItem = _session.FindElement(MobileBy.AccessibilityId("RootMenuItem"));
            
            rootMenuItem.SendClick();

            var childMenuItem = _session.FindElement(MobileBy.AccessibilityId("Child1MenuItem"));
            childMenuItem.SendClick();

            var clickedMenuItem = _session.FindElement(MobileBy.AccessibilityId("ClickedMenuItem"));
            Assert.Equal("_Child 1", clickedMenuItem.Text);
        }

        [Fact]
        public void Click_Grandchild()
        {
            var rootMenuItem = _session.FindElement(MobileBy.AccessibilityId("RootMenuItem"));
            
            rootMenuItem.SendClick();

            var childMenuItem = _session.FindElement(MobileBy.AccessibilityId("Child2MenuItem"));
            childMenuItem.SendClick();

            var grandchildMenuItem = _session.FindElement(MobileBy.AccessibilityId("GrandchildMenuItem"));
            grandchildMenuItem.SendClick();

            var clickedMenuItem = _session.FindElement(MobileBy.AccessibilityId("ClickedMenuItem"));
            Assert.Equal("_Grandchild", clickedMenuItem.Text);
        }

        [PlatformFact(TestPlatforms.Windows)]
        public void Select_Child_With_Alt_Arrow_Keys()
        {
            new Actions(_session)
                .KeyDown(Keys.Alt).KeyUp(Keys.Alt)
                .SendKeys(Keys.Down + Keys.Enter)
                .Perform();

            var clickedMenuItem = _session.FindElement(MobileBy.AccessibilityId("ClickedMenuItem"));
            Assert.Equal("_Child 1", clickedMenuItem.Text);
        }

        [PlatformFact(TestPlatforms.Windows)]
        public void Select_Grandchild_With_Alt_Arrow_Keys()
        {
            new Actions(_session)
                .KeyDown(Keys.Alt).KeyUp(Keys.Alt)
                .SendKeys(Keys.Down + Keys.Down + Keys.Right + Keys.Enter)
                .Perform();

            var clickedMenuItem = _session.FindElement(MobileBy.AccessibilityId("ClickedMenuItem"));
            Assert.Equal("_Grandchild", clickedMenuItem.Text);
        }

        [PlatformFact(TestPlatforms.Windows)]
        public void Select_Child_With_Alt_Access_Keys()
        {
            new Actions(_session)
                .KeyDown(Keys.Alt).KeyUp(Keys.Alt)
                .SendKeys("rc")
                .Perform();

            var clickedMenuItem = _session.FindElement(MobileBy.AccessibilityId("ClickedMenuItem"));
            Assert.Equal("_Child 1", clickedMenuItem.Text);
        }

        [PlatformFact(TestPlatforms.Windows)]
        public void Select_Grandchild_With_Alt_Access_Keys()
        {
            new Actions(_session)
                .KeyDown(Keys.Alt).KeyUp(Keys.Alt)
                .SendKeys("rhg")
                .Perform();

            var clickedMenuItem = _session.FindElement(MobileBy.AccessibilityId("ClickedMenuItem"));
            Assert.Equal("_Grandchild", clickedMenuItem.Text);
        }

        [PlatformFact(TestPlatforms.Windows)]
        public void Select_Child_With_Click_Arrow_Keys()
        {
            var rootMenuItem = _session.FindElement(MobileBy.AccessibilityId("RootMenuItem"));
            rootMenuItem.SendClick();

            new Actions(_session)
                .SendKeys(Keys.Down + Keys.Enter)
                .Perform();

            var clickedMenuItem = _session.FindElement(MobileBy.AccessibilityId("ClickedMenuItem"));
            Assert.Equal("_Child 1", clickedMenuItem.Text);
        }

        [PlatformFact(TestPlatforms.Windows)]
        public void Select_Grandchild_With_Click_Arrow_Keys()
        {
            var rootMenuItem = _session.FindElement(MobileBy.AccessibilityId("RootMenuItem"));
            rootMenuItem.SendClick();

            new Actions(_session)
                .SendKeys(Keys.Down + Keys.Down + Keys.Right + Keys.Enter)
                .Perform();

            var clickedMenuItem = _session.FindElement(MobileBy.AccessibilityId("ClickedMenuItem"));
            Assert.Equal("_Grandchild", clickedMenuItem.Text);
        }

        [PlatformFact(TestPlatforms.Windows)]
        public void Child_AcceleratorKey()
        {
            var rootMenuItem = _session.FindElement(MobileBy.AccessibilityId("RootMenuItem"));

            rootMenuItem.SendClick();

            var childMenuItem = _session.FindElement(MobileBy.AccessibilityId("Child1MenuItem"));

            Assert.Equal("Ctrl+O", childMenuItem.GetAttribute("AcceleratorKey"));
        }

        [PlatformFact(TestPlatforms.Windows)]
        public void PointerOver_Does_Not_Steal_Focus()
        {
            // Issue #7906
            var textBox = _session.FindElement(MobileBy.AccessibilityId("MenuFocusTest"));
            textBox.Click();

            Assert.True(textBox.GetIsFocused());

            var rootMenuItem = _session.FindElement(MobileBy.AccessibilityId("RootMenuItem"));
            rootMenuItem.MovePointerOver();

            Assert.True(textBox.GetIsFocused());
        }
    }
}