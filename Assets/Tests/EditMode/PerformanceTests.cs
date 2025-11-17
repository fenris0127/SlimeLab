using NUnit.Framework;
using SlimeLab.Core;
using SlimeLab.Systems;
using System.Collections.Generic;

namespace SlimeLab.Tests
{
    [TestFixture]
    public class PerformanceTests
    {
        [Test]
        public void ObjectPool_CanBeCreated()
        {
            var pool = new ObjectPool<Slime>(() => new Slime("Pooled", ElementType.Fire), 10);

            Assert.IsNotNull(pool);
        }

        [Test]
        public void ObjectPool_HasInitialCapacity()
        {
            var pool = new ObjectPool<Slime>(() => new Slime("Pooled", ElementType.Fire), 10);

            Assert.AreEqual(10, pool.Capacity);
        }

        [Test]
        public void ObjectPool_CanGetObject()
        {
            var pool = new ObjectPool<Slime>(() => new Slime("Pooled", ElementType.Fire), 10);

            var obj = pool.Get();

            Assert.IsNotNull(obj);
        }

        [Test]
        public void ObjectPool_CanReturnObject()
        {
            var pool = new ObjectPool<Slime>(() => new Slime("Pooled", ElementType.Fire), 10);

            var obj = pool.Get();
            pool.Return(obj);

            Assert.AreEqual(10, pool.AvailableCount);
        }

        [Test]
        public void ObjectPool_TracksAvailableCount()
        {
            var pool = new ObjectPool<Slime>(() => new Slime("Pooled", ElementType.Fire), 10);

            Assert.AreEqual(10, pool.AvailableCount);

            pool.Get();
            Assert.AreEqual(9, pool.AvailableCount);

            pool.Get();
            Assert.AreEqual(8, pool.AvailableCount);
        }

        [Test]
        public void ObjectPool_TracksActiveCount()
        {
            var pool = new ObjectPool<Slime>(() => new Slime("Pooled", ElementType.Fire), 10);

            Assert.AreEqual(0, pool.ActiveCount);

            pool.Get();
            Assert.AreEqual(1, pool.ActiveCount);
        }

        [Test]
        public void ObjectPool_ExpandsWhenEmpty()
        {
            var pool = new ObjectPool<Slime>(() => new Slime("Pooled", ElementType.Fire), 2);

            pool.Get();
            pool.Get();

            // Pool should expand when no objects available
            var obj = pool.Get();

            Assert.IsNotNull(obj);
            Assert.IsTrue(pool.Capacity > 2);
        }

        [Test]
        public void PerformanceMonitor_CanBeCreated()
        {
            var monitor = new PerformanceMonitor();

            Assert.IsNotNull(monitor);
        }

        [Test]
        public void PerformanceMonitor_CanRecordFrameTime()
        {
            var monitor = new PerformanceMonitor();

            monitor.RecordFrame(16.67f); // ~60fps

            Assert.AreEqual(1, monitor.FrameCount);
        }

        [Test]
        public void PerformanceMonitor_CanCalculateAverageFPS()
        {
            var monitor = new PerformanceMonitor();

            // Record 60 frames at 16.67ms each (60fps)
            for (int i = 0; i < 60; i++)
            {
                monitor.RecordFrame(16.67f);
            }

            float avgFPS = monitor.GetAverageFPS();

            Assert.Greater(avgFPS, 59f);
            Assert.Less(avgFPS, 61f);
        }

        [Test]
        public void PerformanceMonitor_CanGetMinFPS()
        {
            var monitor = new PerformanceMonitor();

            monitor.RecordFrame(16.67f); // 60fps
            monitor.RecordFrame(33.33f); // 30fps
            monitor.RecordFrame(16.67f); // 60fps

            float minFPS = monitor.GetMinFPS();

            Assert.Greater(minFPS, 29f);
            Assert.Less(minFPS, 31f);
        }

        [Test]
        public void PerformanceMonitor_CanGetMaxFPS()
        {
            var monitor = new PerformanceMonitor();

            monitor.RecordFrame(33.33f); // 30fps
            monitor.RecordFrame(16.67f); // 60fps
            monitor.RecordFrame(33.33f); // 30fps

            float maxFPS = monitor.GetMaxFPS();

            Assert.Greater(maxFPS, 59f);
            Assert.Less(maxFPS, 61f);
        }

        [Test]
        public void PerformanceMonitor_CanReset()
        {
            var monitor = new PerformanceMonitor();

            monitor.RecordFrame(16.67f);
            monitor.RecordFrame(16.67f);

            Assert.AreEqual(2, monitor.FrameCount);

            monitor.Reset();

            Assert.AreEqual(0, monitor.FrameCount);
        }

        [Test]
        public void MemoryManager_CanBeCreated()
        {
            var memoryManager = new MemoryManager();

            Assert.IsNotNull(memoryManager);
        }

        [Test]
        public void MemoryManager_CanTrackAllocations()
        {
            var memoryManager = new MemoryManager();

            memoryManager.RecordAllocation("Slime", 1024);

            Assert.AreEqual(1024, memoryManager.GetTotalAllocated("Slime"));
        }

        [Test]
        public void MemoryManager_CanTrackDeallocations()
        {
            var memoryManager = new MemoryManager();

            memoryManager.RecordAllocation("Slime", 1024);
            memoryManager.RecordDeallocation("Slime", 512);

            Assert.AreEqual(512, memoryManager.GetTotalAllocated("Slime"));
        }

        [Test]
        public void MemoryManager_CanGetTotalMemoryUsage()
        {
            var memoryManager = new MemoryManager();

            memoryManager.RecordAllocation("Slime", 1024);
            memoryManager.RecordAllocation("Resource", 512);

            Assert.AreEqual(1536, memoryManager.GetTotalMemoryUsage());
        }

        [Test]
        public void MemoryManager_CanDetectLeaks()
        {
            var memoryManager = new MemoryManager();

            memoryManager.RecordAllocation("Slime", 1024);
            memoryManager.RecordAllocation("Slime", 1024);
            memoryManager.RecordDeallocation("Slime", 1024);

            // Still 1024 bytes allocated
            Assert.IsTrue(memoryManager.HasPotentialLeak("Slime", 500)); // Threshold 500 bytes
        }

        [Test]
        public void CullingSystem_CanBeCreated()
        {
            var cullingSystem = new CullingSystem();

            Assert.IsNotNull(cullingSystem);
        }

        [Test]
        public void CullingSystem_CanRegisterObject()
        {
            var cullingSystem = new CullingSystem();
            var slime = new Slime("TestSlime", ElementType.Fire);

            cullingSystem.RegisterObject(slime, 0, 0);

            Assert.AreEqual(1, cullingSystem.RegisteredObjectCount);
        }

        [Test]
        public void CullingSystem_CanUnregisterObject()
        {
            var cullingSystem = new CullingSystem();
            var slime = new Slime("TestSlime", ElementType.Fire);

            cullingSystem.RegisterObject(slime, 0, 0);
            Assert.AreEqual(1, cullingSystem.RegisteredObjectCount);

            cullingSystem.UnregisterObject(slime);

            Assert.AreEqual(0, cullingSystem.RegisteredObjectCount);
        }

        [Test]
        public void CullingSystem_CanGetVisibleObjects()
        {
            var cullingSystem = new CullingSystem();

            var slime1 = new Slime("Slime1", ElementType.Fire);
            var slime2 = new Slime("Slime2", ElementType.Water);
            var slime3 = new Slime("Slime3", ElementType.Electric);

            cullingSystem.RegisterObject(slime1, 0, 0);
            cullingSystem.RegisterObject(slime2, 100, 0);
            cullingSystem.RegisterObject(slime3, 200, 0);

            // Get objects visible in viewport (0-150)
            var visible = cullingSystem.GetVisibleObjects(0, 0, 150, 100);

            Assert.AreEqual(2, visible.Count);
            Assert.Contains(slime1, visible);
            Assert.Contains(slime2, visible);
        }

        [Test]
        public void ObjectPool_CanClear()
        {
            var pool = new ObjectPool<Slime>(() => new Slime("Pooled", ElementType.Fire), 10);

            pool.Get();
            pool.Get();

            pool.Clear();

            Assert.AreEqual(0, pool.ActiveCount);
            Assert.AreEqual(10, pool.AvailableCount);
        }

        [Test]
        public void PerformanceMonitor_MaintainsSlidingWindow()
        {
            var monitor = new PerformanceMonitor(maxSamples: 10);

            // Record more frames than max samples
            for (int i = 0; i < 15; i++)
            {
                monitor.RecordFrame(16.67f);
            }

            // Should only keep last 10 samples
            Assert.AreEqual(10, monitor.FrameCount);
        }

        [Test]
        public void MemoryManager_CanGetAllocationsByType()
        {
            var memoryManager = new MemoryManager();

            memoryManager.RecordAllocation("Slime", 1024);
            memoryManager.RecordAllocation("Resource", 512);
            memoryManager.RecordAllocation("Slime", 2048);

            var allocations = memoryManager.GetAllocationsByType();

            Assert.AreEqual(2, allocations.Count);
            Assert.AreEqual(3072, allocations["Slime"]);
            Assert.AreEqual(512, allocations["Resource"]);
        }

        [Test]
        public void CullingSystem_UpdatesObjectPosition()
        {
            var cullingSystem = new CullingSystem();
            var slime = new Slime("TestSlime", ElementType.Fire);

            cullingSystem.RegisterObject(slime, 0, 0);

            // Initially visible
            var visible = cullingSystem.GetVisibleObjects(0, 0, 100, 100);
            Assert.Contains(slime, visible);

            // Move object outside viewport
            cullingSystem.UpdateObjectPosition(slime, 200, 200);

            // No longer visible
            visible = cullingSystem.GetVisibleObjects(0, 0, 100, 100);
            Assert.IsFalse(visible.Contains(slime));
        }

        [Test]
        public void ObjectPool_PrewarmsPool()
        {
            var pool = new ObjectPool<Slime>(() => new Slime("Pooled", ElementType.Fire), 5);

            // Pool should have 5 available objects immediately
            Assert.AreEqual(5, pool.AvailableCount);
        }

        [Test]
        public void PerformanceMonitor_CanGetCurrentFPS()
        {
            var monitor = new PerformanceMonitor();

            monitor.RecordFrame(16.67f);

            float currentFPS = monitor.GetCurrentFPS();

            Assert.Greater(currentFPS, 59f);
            Assert.Less(currentFPS, 61f);
        }

        [Test]
        public void MemoryManager_CanClear()
        {
            var memoryManager = new MemoryManager();

            memoryManager.RecordAllocation("Slime", 1024);
            memoryManager.RecordAllocation("Resource", 512);

            memoryManager.Clear();

            Assert.AreEqual(0, memoryManager.GetTotalMemoryUsage());
        }

        [Test]
        public void CullingSystem_HandlesEmptyViewport()
        {
            var cullingSystem = new CullingSystem();
            var slime = new Slime("TestSlime", ElementType.Fire);

            cullingSystem.RegisterObject(slime, 100, 100);

            var visible = cullingSystem.GetVisibleObjects(200, 200, 50, 50);

            Assert.AreEqual(0, visible.Count);
        }

        [Test]
        public void ObjectPool_ReusesReturnedObjects()
        {
            var pool = new ObjectPool<Slime>(() => new Slime("Pooled", ElementType.Fire), 2);

            var obj1 = pool.Get();
            var obj2 = pool.Get();

            pool.Return(obj1);

            var obj3 = pool.Get();

            // obj3 should be the reused obj1
            Assert.AreEqual(obj1, obj3);
        }

        [Test]
        public void PerformanceMonitor_HandlesZeroFrameTime()
        {
            var monitor = new PerformanceMonitor();

            monitor.RecordFrame(0f);

            // Should not crash, FPS should be very high or handled gracefully
            Assert.IsTrue(monitor.GetCurrentFPS() >= 0);
        }
    }
}
