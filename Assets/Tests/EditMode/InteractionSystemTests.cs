using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;
using SlimeLab.UI;
using System.Collections.Generic;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class InteractionSystemTests
    {
        [Test]
        public void DragDropItem_CanBeCreated()
        {
            var slime = new Slime("TestSlime", ElementType.Fire);
            var dragItem = new DragDropItem(slime, DragDropType.Slime);

            Assert.IsNotNull(dragItem);
        }

        [Test]
        public void DragDropItem_HasType()
        {
            var slime = new Slime("TestSlime", ElementType.Fire);
            var dragItem = new DragDropItem(slime, DragDropType.Slime);

            Assert.AreEqual(DragDropType.Slime, dragItem.Type);
        }

        [Test]
        public void DragDropItem_CanGetData()
        {
            var slime = new Slime("TestSlime", ElementType.Fire);
            var dragItem = new DragDropItem(slime, DragDropType.Slime);

            var retrievedSlime = dragItem.GetData<Slime>();

            Assert.AreEqual(slime, retrievedSlime);
        }

        [Test]
        public void DragDropManager_CanBeCreated()
        {
            var manager = new DragDropManager();

            Assert.IsNotNull(manager);
        }

        [Test]
        public void DragDropManager_CanStartDrag()
        {
            var manager = new DragDropManager();
            var slime = new Slime("TestSlime", ElementType.Fire);
            var dragItem = new DragDropItem(slime, DragDropType.Slime);

            manager.StartDrag(dragItem);

            Assert.IsTrue(manager.IsDragging);
        }

        [Test]
        public void DragDropManager_TracksCurrentDragItem()
        {
            var manager = new DragDropManager();
            var slime = new Slime("TestSlime", ElementType.Fire);
            var dragItem = new DragDropItem(slime, DragDropType.Slime);

            manager.StartDrag(dragItem);

            Assert.AreEqual(dragItem, manager.CurrentDragItem);
        }

        [Test]
        public void DragDropManager_CanEndDrag()
        {
            var manager = new DragDropManager();
            var slime = new Slime("TestSlime", ElementType.Fire);
            var dragItem = new DragDropItem(slime, DragDropType.Slime);

            manager.StartDrag(dragItem);
            Assert.IsTrue(manager.IsDragging);

            manager.EndDrag();

            Assert.IsFalse(manager.IsDragging);
            Assert.IsNull(manager.CurrentDragItem);
        }

        [Test]
        public void DragDropManager_CanCancelDrag()
        {
            var manager = new DragDropManager();
            var slime = new Slime("TestSlime", ElementType.Fire);
            var dragItem = new DragDropItem(slime, DragDropType.Slime);

            manager.StartDrag(dragItem);
            Assert.IsTrue(manager.IsDragging);

            manager.CancelDrag();

            Assert.IsFalse(manager.IsDragging);
            Assert.IsNull(manager.CurrentDragItem);
        }

        [Test]
        public void DropZone_CanBeCreated()
        {
            var dropZone = new DropZone("Zone1", DragDropType.Slime);

            Assert.IsNotNull(dropZone);
            Assert.AreEqual("Zone1", dropZone.ID);
        }

        [Test]
        public void DropZone_AcceptsCompatibleTypes()
        {
            var dropZone = new DropZone("Zone1", DragDropType.Slime);

            Assert.IsTrue(dropZone.CanAccept(DragDropType.Slime));
        }

        [Test]
        public void DropZone_RejectsIncompatibleTypes()
        {
            var dropZone = new DropZone("Zone1", DragDropType.Slime);

            Assert.IsFalse(dropZone.CanAccept(DragDropType.Item));
        }

        [Test]
        public void DropZone_CanAcceptMultipleTypes()
        {
            var dropZone = new DropZone("Zone1", DragDropType.Slime);
            dropZone.AddAcceptedType(DragDropType.Item);

            Assert.IsTrue(dropZone.CanAccept(DragDropType.Slime));
            Assert.IsTrue(dropZone.CanAccept(DragDropType.Item));
        }

        [Test]
        public void DropZone_CanHandleDrop()
        {
            var dropZone = new DropZone("Zone1", DragDropType.Slime);
            var slime = new Slime("TestSlime", ElementType.Fire);
            var dragItem = new DragDropItem(slime, DragDropType.Slime);

            bool dropHandled = false;
            dropZone.OnDrop += (item) => { dropHandled = true; };

            dropZone.HandleDrop(dragItem);

            Assert.IsTrue(dropHandled);
        }

        [Test]
        public void ContextMenu_CanBeCreated()
        {
            var menu = new ContextMenu();

            Assert.IsNotNull(menu);
        }

        [Test]
        public void ContextMenu_CanAddAction()
        {
            var menu = new ContextMenu();
            var action = new ContextMenuAction("Feed", "Feed the slime");

            menu.AddAction(action);

            Assert.AreEqual(1, menu.ActionCount);
        }

        [Test]
        public void ContextMenu_CanGetActions()
        {
            var menu = new ContextMenu();
            var action1 = new ContextMenuAction("Feed", "Feed the slime");
            var action2 = new ContextMenuAction("Heal", "Heal the slime");

            menu.AddAction(action1);
            menu.AddAction(action2);

            var actions = menu.GetActions();

            Assert.AreEqual(2, actions.Count);
            Assert.Contains(action1, actions);
            Assert.Contains(action2, actions);
        }

        [Test]
        public void ContextMenuAction_CanBeCreated()
        {
            var action = new ContextMenuAction("Feed", "Feed the slime");

            Assert.IsNotNull(action);
            Assert.AreEqual("Feed", action.ID);
            Assert.AreEqual("Feed the slime", action.Label);
        }

        [Test]
        public void ContextMenuAction_CanBeExecuted()
        {
            var action = new ContextMenuAction("Feed", "Feed the slime");

            bool executed = false;
            action.OnExecute += () => { executed = true; };

            action.Execute();

            Assert.IsTrue(executed);
        }

        [Test]
        public void ContextMenuAction_CanBeDisabled()
        {
            var action = new ContextMenuAction("Feed", "Feed the slime");

            Assert.IsTrue(action.IsEnabled);

            action.SetEnabled(false);

            Assert.IsFalse(action.IsEnabled);
        }

        [Test]
        public void ContextMenuAction_DoesNotExecuteWhenDisabled()
        {
            var action = new ContextMenuAction("Feed", "Feed the slime");

            bool executed = false;
            action.OnExecute += () => { executed = true; };
            action.SetEnabled(false);

            action.Execute();

            Assert.IsFalse(executed);
        }

        [Test]
        public void ContextMenuManager_CanBeCreated()
        {
            var manager = new ContextMenuManager();

            Assert.IsNotNull(manager);
        }

        [Test]
        public void ContextMenuManager_CanShowMenu()
        {
            var manager = new ContextMenuManager();
            var menu = new ContextMenu();

            manager.ShowMenu(menu);

            Assert.IsTrue(manager.IsMenuVisible);
            Assert.AreEqual(menu, manager.CurrentMenu);
        }

        [Test]
        public void ContextMenuManager_CanHideMenu()
        {
            var manager = new ContextMenuManager();
            var menu = new ContextMenu();

            manager.ShowMenu(menu);
            Assert.IsTrue(manager.IsMenuVisible);

            manager.HideMenu();

            Assert.IsFalse(manager.IsMenuVisible);
            Assert.IsNull(manager.CurrentMenu);
        }

        [Test]
        public void DragDropType_HasExpectedValues()
        {
            // Validate enum exists and has expected values
            var slimeType = DragDropType.Slime;
            var itemType = DragDropType.Item;

            Assert.IsNotNull(slimeType);
            Assert.IsNotNull(itemType);
            Assert.AreNotEqual(slimeType, itemType);
        }

        [Test]
        public void DropZone_CanBeEnabled()
        {
            var dropZone = new DropZone("Zone1", DragDropType.Slime);

            Assert.IsTrue(dropZone.IsEnabled);

            dropZone.SetEnabled(false);

            Assert.IsFalse(dropZone.IsEnabled);
        }

        [Test]
        public void DropZone_DoesNotHandleDropWhenDisabled()
        {
            var dropZone = new DropZone("Zone1", DragDropType.Slime);
            var slime = new Slime("TestSlime", ElementType.Fire);
            var dragItem = new DragDropItem(slime, DragDropType.Slime);

            bool dropHandled = false;
            dropZone.OnDrop += (item) => { dropHandled = true; };
            dropZone.SetEnabled(false);

            dropZone.HandleDrop(dragItem);

            Assert.IsFalse(dropHandled);
        }

        [Test]
        public void ContextMenu_CanClearActions()
        {
            var menu = new ContextMenu();
            menu.AddAction(new ContextMenuAction("Action1", "First action"));
            menu.AddAction(new ContextMenuAction("Action2", "Second action"));

            Assert.AreEqual(2, menu.ActionCount);

            menu.ClearActions();

            Assert.AreEqual(0, menu.ActionCount);
        }

        [Test]
        public void ContextMenu_CanRemoveAction()
        {
            var menu = new ContextMenu();
            var action1 = new ContextMenuAction("Action1", "First action");
            var action2 = new ContextMenuAction("Action2", "Second action");

            menu.AddAction(action1);
            menu.AddAction(action2);

            Assert.AreEqual(2, menu.ActionCount);

            menu.RemoveAction(action1);

            Assert.AreEqual(1, menu.ActionCount);
            Assert.IsFalse(menu.GetActions().Contains(action1));
        }

        [Test]
        public void DragDropManager_CanRegisterDropZone()
        {
            var manager = new DragDropManager();
            var dropZone = new DropZone("Zone1", DragDropType.Slime);

            manager.RegisterDropZone(dropZone);

            Assert.AreEqual(1, manager.DropZoneCount);
        }

        [Test]
        public void DragDropManager_CanUnregisterDropZone()
        {
            var manager = new DragDropManager();
            var dropZone = new DropZone("Zone1", DragDropType.Slime);

            manager.RegisterDropZone(dropZone);
            Assert.AreEqual(1, manager.DropZoneCount);

            manager.UnregisterDropZone(dropZone);

            Assert.AreEqual(0, manager.DropZoneCount);
        }

        [Test]
        public void DragDropManager_CanFindDropZoneByID()
        {
            var manager = new DragDropManager();
            var dropZone = new DropZone("Zone1", DragDropType.Slime);

            manager.RegisterDropZone(dropZone);

            var found = manager.GetDropZone("Zone1");

            Assert.AreEqual(dropZone, found);
        }

        [Test]
        public void ContextMenuAction_CanHaveIcon()
        {
            var action = new ContextMenuAction("Feed", "Feed the slime", "icon_feed");

            Assert.AreEqual("icon_feed", action.Icon);
        }

        [Test]
        public void DragDropItem_CanHaveMetadata()
        {
            var slime = new Slime("TestSlime", ElementType.Fire);
            var dragItem = new DragDropItem(slime, DragDropType.Slime);

            dragItem.SetMetadata("SourceUnit", "Unit1");

            Assert.AreEqual("Unit1", dragItem.GetMetadata("SourceUnit"));
        }

        [Test]
        public void DropZone_CanValidateCustomConditions()
        {
            var dropZone = new DropZone("Zone1", DragDropType.Slime);
            dropZone.SetCustomValidator((item) =>
            {
                var slime = item.GetData<Slime>();
                return slime.Level >= 5;
            });

            var lowLevelSlime = new Slime("WeakSlime", ElementType.Fire);
            var highLevelSlime = new Slime("StrongSlime", ElementType.Fire);
            highLevelSlime.GainExperience(500);

            var lowLevelItem = new DragDropItem(lowLevelSlime, DragDropType.Slime);
            var highLevelItem = new DragDropItem(highLevelSlime, DragDropType.Slime);

            Assert.IsFalse(dropZone.ValidateDrop(lowLevelItem));
            Assert.IsTrue(dropZone.ValidateDrop(highLevelItem));
        }

        [Test]
        public void ContextMenuManager_CanGetMenuForTarget()
        {
            var manager = new ContextMenuManager();
            var slime = new Slime("TestSlime", ElementType.Fire);
            var menu = new ContextMenu();

            menu.AddAction(new ContextMenuAction("Feed", "Feed the slime"));

            manager.RegisterMenuBuilder("Slime", (target) => menu);

            var retrievedMenu = manager.GetMenuForTarget("Slime", slime);

            Assert.AreEqual(menu, retrievedMenu);
        }
    }
}
